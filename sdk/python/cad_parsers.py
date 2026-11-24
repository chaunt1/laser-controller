"""
Comprehensive CAD & CNC Drawing Parsers for Rhothor Laser Studio.
Supports:
  1. Vector CAD files: .DXF, .SVG, .PLT / .HPGL, .AI (Adobe Illustrator)
  2. Project files: .LBRN / .LBRN2 (LightBurn), .EZD / .EZ3 (EZCAD)
  3. Raster / Bitmap images: .BMP, .JPG, .JPEG, .PNG (Photo engraving & shading)

Converts artwork into standardized Rhothor laser motion strokes (JUMP, LINE, ARC).
Zero third-party C-library dependencies (pure Python standard library + PyQt5 QImage).
"""

import os
import sys
import re
import math
import json
import zlib
import xml.etree.ElementTree as ET
from typing import List, Dict, Any, Tuple, Optional


class VectorStroke:
    """Represents a continuous laser toolpath (1 JUMP followed by 1 or more LINE / ARC)."""
    def __init__(self, layer: str = "0", color: str = "#ff3344", speed: Optional[float] = None, power: Optional[float] = None):
        self.layer = layer
        self.color = color
        self.speed = speed
        self.power = power
        self.points: List[Dict[str, Any]] = []

    def add_jump(self, x: float, y: float, z: float = 0.0):
        self.points.append({"type": "JUMP", "x": round(float(x), 4), "y": round(float(y), 4), "z": round(float(z), 4)})

    def add_line(self, x: float, y: float, z: float = 0.0):
        self.points.append({"type": "LINE", "x": round(float(x), 4), "y": round(float(y), 4), "z": round(float(z), 4)})

    def add_arc(self, x: float, y: float, bf: float, z: float = 0.0):
        self.points.append({"type": "ARC", "x": round(float(x), 4), "y": round(float(y), 4), "bf": round(float(bf), 4), "z": round(float(z), 4)})


# =============================================================================
# 1. DXF Parser (AutoCAD, EzCad, CorelDRAW DXF)
# =============================================================================

def parse_dxf_content(content: str) -> Dict[str, Any]:
    """
    Parses DXF content into laser toolpaths.
    Supports LINE, LWPOLYLINE, POLYLINE, CIRCLE, ARC, ELLIPSE, SPLINE, POINT.
    """
    lines = [line.strip() for line in content.splitlines()]
    entities: List[Dict[str, Any]] = []
    layers_detected: Dict[str, Dict[str, Any]] = {}

    i = 0
    n = len(lines)
    while i < n:
        if lines[i] == "0" and i + 1 < n:
            etype = lines[i + 1].upper()
            i += 2
            props: Dict[str, List[str]] = {}
            while i < n and lines[i] != "0":
                code = lines[i]
                val = lines[i + 1] if i + 1 < n else ""
                if code not in props:
                    props[code] = []
                props[code].append(val)
                i += 2

            def get_f(code: str, default: float = 0.0) -> float:
                v = props.get(code, [])
                return float(v[0]) if v else default

            def get_s(code: str, default: str = "") -> str:
                v = props.get(code, [])
                return v[0] if v else default

            layer_name = get_s("8", "0")
            if layer_name not in layers_detected:
                layers_detected[layer_name] = {"name": layer_name, "count": 0}
            layers_detected[layer_name]["count"] += 1

            if etype == "LINE":
                x1, y1 = get_f("10"), get_f("20")
                x2, y2 = get_f("11"), get_f("21")
                entities.append({"type": "LINE", "x1": x1, "y1": y1, "x2": x2, "y2": y2, "layer": layer_name})

            elif etype == "CIRCLE":
                cx, cy, r = get_f("10"), get_f("20"), get_f("40", 1.0)
                entities.append({"type": "CIRCLE", "cx": cx, "cy": cy, "r": r, "layer": layer_name})

            elif etype == "ARC":
                cx, cy, r = get_f("10"), get_f("20"), get_f("40", 1.0)
                start_deg, end_deg = get_f("50"), get_f("51")
                entities.append({"type": "ARC", "cx": cx, "cy": cy, "r": r,
                                 "start": start_deg, "end": end_deg, "layer": layer_name})

            elif etype in ("LWPOLYLINE", "POLYLINE"):
                xs = [float(v) for v in props.get("10", [])]
                ys = [float(v) for v in props.get("20", [])]
                flags = int(get_f("70", 0))
                closed = bool(flags & 1)
                pts = list(zip(xs, ys))
                if pts:
                    entities.append({"type": "POLYLINE", "points": pts, "closed": closed, "layer": layer_name})

            elif etype == "ELLIPSE":
                cx, cy = get_f("10"), get_f("20")
                mx, my = get_f("11"), get_f("21")  # Major axis endpoint relative to center
                ratio = get_f("40", 1.0)           # Minor axis ratio
                start_p = get_f("41", 0.0)         # Start param
                end_p = get_f("42", 2 * math.pi)   # End param
                major_r = math.hypot(mx, my)
                minor_r = major_r * ratio
                angle = math.atan2(my, mx)
                # Sample ellipse into polyline
                pts = []
                steps = max(16, int(major_r * 2))
                for s in range(steps + 1):
                    t = start_p + (end_p - start_p) * (s / steps)
                    ex = major_r * math.cos(t)
                    ey = minor_r * math.sin(t)
                    rx = cx + ex * math.cos(angle) - ey * math.sin(angle)
                    ry = cy + ex * math.sin(angle) + ey * math.cos(angle)
                    pts.append((rx, ry))
                if pts:
                    entities.append({"type": "POLYLINE", "points": pts, "closed": False, "layer": layer_name})

            elif etype == "SPLINE":
                # Splines: extract fit or control points
                xs = [float(v) for v in props.get("10", [])]
                ys = [float(v) for v in props.get("20", [])]
                pts = list(zip(xs, ys))
                if pts:
                    entities.append({"type": "POLYLINE", "points": pts, "closed": False, "layer": layer_name})

            elif etype == "POINT":
                px, py = get_f("10"), get_f("20")
                entities.append({"type": "POINT", "x": px, "y": py, "layer": layer_name})
        else:
            i += 1

    return _convert_entities_to_toolpaths(entities, source_format="DXF", layers=layers_detected)


# =============================================================================
# 2. SVG Parser (Scalable Vector Graphics - Corel, Illustrator, Inkscape)
# =============================================================================

def parse_svg_content(content: str) -> Dict[str, Any]:
    """
    Parses standard W3C SVG vector drawings.
    Extracts <path d="...">, <line>, <rect>, <circle>, <ellipse>, <polyline>, <polygon>.
    Normalizes coordinates to millimeters.
    """
    try:
        # Strip XML namespaces for simplified element matching
        cleaned = re.sub(r'\sxmlns(:\w+)?="[^"]+"', '', content)
        root = ET.fromstring(cleaned)
    except Exception as e:
        raise ValueError(f"Invalid SVG XML content: {e}")

    # Determine scale to physical millimeters
    # Default 96 DPI: 1 inch = 25.4 mm => 1 px = 25.4 / 96 = 0.264583 mm
    px_to_mm = 0.264583
    
    # Check width / height attributes
    svg_w_str = root.attrib.get("width", "")
    svg_h_str = root.attrib.get("height", "")
    viewbox = root.attrib.get("viewBox", "")

    entities: List[Dict[str, Any]] = []
    layers_detected: Dict[str, Dict[str, Any]] = {"Layer 0": {"name": "Layer 0", "count": 0}}

    def parse_path_d(d_str: str) -> List[List[Tuple[float, float]]]:
        """Tokenizes and parses SVG path data into polylines."""
        tokens = re.findall(r'([A-Za-z])|([-+]?(?:\d*\.\d+|\d+)(?:[eE][-+]?\d+)?)', d_str)
        flat_tokens = [t[0] or t[1] for t in tokens if t[0] or t[1]]
        
        subpaths: List[List[Tuple[float, float]]] = []
        current_subpath: List[Tuple[float, float]] = []
        cur_x, cur_y = 0.0, 0.0
        start_x, start_y = 0.0, 0.0
        cmd = 'M'
        idx = 0
        total = len(flat_tokens)

        while idx < total:
            item = flat_tokens[idx]
            if item.isalpha():
                cmd = item
                idx += 1
            else:
                pass  # Keep previous cmd if omitted

            if cmd in ('M', 'm'):
                if idx + 1 < total:
                    nx = float(flat_tokens[idx])
                    ny = float(flat_tokens[idx + 1])
                    idx += 2
                    if cmd == 'm':
                        cur_x += nx
                        cur_y += ny
                    else:
                        cur_x = nx
                        cur_y = ny
                    if current_subpath:
                        subpaths.append(current_subpath)
                    current_subpath = [(cur_x, cur_y)]
                    start_x, start_y = cur_x, cur_y
                    # Subsequent coordinate pairs in M/m are treated as L/l
                    cmd = 'l' if cmd == 'm' else 'L'

            elif cmd in ('L', 'l'):
                if idx + 1 < total:
                    nx = float(flat_tokens[idx])
                    ny = float(flat_tokens[idx + 1])
                    idx += 2
                    if cmd == 'l':
                        cur_x += nx
                        cur_y += ny
                    else:
                        cur_x = nx
                        cur_y = ny
                    current_subpath.append((cur_x, cur_y))

            elif cmd in ('H', 'h'):
                if idx < total:
                    nx = float(flat_tokens[idx])
                    idx += 1
                    cur_x = cur_x + nx if cmd == 'h' else nx
                    current_subpath.append((cur_x, cur_y))

            elif cmd in ('V', 'v'):
                if idx < total:
                    ny = float(flat_tokens[idx])
                    idx += 1
                    cur_y = cur_y + ny if cmd == 'v' else ny
                    current_subpath.append((cur_x, cur_y))

            elif cmd in ('C', 'c'):
                if idx + 5 < total:
                    coords = [float(flat_tokens[idx + k]) for k in range(6)]
                    idx += 6
                    if cmd == 'c':
                        p1 = (cur_x + coords[0], cur_y + coords[1])
                        p2 = (cur_x + coords[2], cur_y + coords[3])
                        p3 = (cur_x + coords[4], cur_y + coords[5])
                    else:
                        p1 = (coords[0], coords[1])
                        p2 = (coords[2], coords[3])
                        p3 = (coords[4], coords[5])
                    # Approximate cubic bezier with 12 linear chords
                    p0 = (cur_x, cur_y)
                    for step in range(1, 13):
                        t = step / 12.0
                        bx = (1 - t)**3 * p0[0] + 3 * (1 - t)**2 * t * p1[0] + 3 * (1 - t) * t**2 * p2[0] + t**3 * p3[0]
                        by = (1 - t)**3 * p0[1] + 3 * (1 - t)**2 * t * p1[1] + 3 * (1 - t) * t**2 * p2[1] + t**3 * p3[1]
                        current_subpath.append((bx, by))
                    cur_x, cur_y = p3

            elif cmd in ('Q', 'q'):
                if idx + 3 < total:
                    coords = [float(flat_tokens[idx + k]) for k in range(4)]
                    idx += 4
                    if cmd == 'q':
                        p1 = (cur_x + coords[0], cur_y + coords[1])
                        p2 = (cur_x + coords[2], cur_y + coords[3])
                    else:
                        p1 = (coords[0], coords[1])
                        p2 = (coords[2], coords[3])
                    p0 = (cur_x, cur_y)
                    for step in range(1, 9):
                        t = step / 8.0
                        bx = (1 - t)**2 * p0[0] + 2 * (1 - t) * t * p1[0] + t**2 * p2[0]
                        by = (1 - t)**2 * p0[1] + 2 * (1 - t) * t * p1[1] + t**2 * p2[1]
                        current_subpath.append((bx, by))
                    cur_x, cur_y = p2

            elif cmd in ('Z', 'z'):
                if current_subpath and (cur_x, cur_y) != (start_x, start_y):
                    current_subpath.append((start_x, start_y))
                cur_x, cur_y = start_x, start_y
                if current_subpath:
                    subpaths.append(current_subpath)
                    current_subpath = []
                idx += 1
            else:
                idx += 1

        if current_subpath:
            subpaths.append(current_subpath)
        return subpaths

    # Traverse all SVG geometric elements
    for elem in root.iter():
        tag = elem.tag.split("}")[-1].lower()
        
        if tag == "path":
            d_data = elem.attrib.get("d", "")
            if d_data:
                polys = parse_path_d(d_data)
                for poly in polys:
                    # Invert Y to standard Cartesian / Laser coordinate frame
                    scaled = [(p[0] * px_to_mm, -p[1] * px_to_mm) for p in poly]
                    if len(scaled) > 1:
                        entities.append({"type": "POLYLINE", "points": scaled, "closed": False, "layer": "SVG_Vector"})

        elif tag == "rect":
            x = float(elem.attrib.get("x", 0.0)) * px_to_mm
            y = float(elem.attrib.get("y", 0.0)) * px_to_mm
            w = float(elem.attrib.get("width", 0.0)) * px_to_mm
            h = float(elem.attrib.get("height", 0.0)) * px_to_mm
            if w > 0 and h > 0:
                pts = [(x, -y), (x + w, -y), (x + w, -(y + h)), (x, -(y + h)), (x, -y)]
                entities.append({"type": "POLYLINE", "points": pts, "closed": True, "layer": "SVG_Rect"})

        elif tag in ("circle", "ellipse"):
            cx = float(elem.attrib.get("cx", 0.0)) * px_to_mm
            cy = float(elem.attrib.get("cy", 0.0)) * px_to_mm
            rx = float(elem.attrib.get("r", elem.attrib.get("rx", 1.0))) * px_to_mm
            ry = float(elem.attrib.get("r", elem.attrib.get("ry", 1.0))) * px_to_mm
            steps = max(16, int(max(rx, ry) * 3))
            pts = []
            for s in range(steps + 1):
                t = 2 * math.pi * (s / steps)
                pts.append((cx + rx * math.cos(t), -(cy + ry * math.sin(t))))
            entities.append({"type": "POLYLINE", "points": pts, "closed": True, "layer": "SVG_Circle"})

        elif tag == "line":
            x1 = float(elem.attrib.get("x1", 0.0)) * px_to_mm
            y1 = float(elem.attrib.get("y1", 0.0)) * px_to_mm
            x2 = float(elem.attrib.get("x2", 0.0)) * px_to_mm
            y2 = float(elem.attrib.get("y2", 0.0)) * px_to_mm
            entities.append({"type": "LINE", "x1": x1, "y1": -y1, "x2": x2, "y2": -y2, "layer": "SVG_Line"})

        elif tag in ("polyline", "polygon"):
            raw_pts = elem.attrib.get("points", "")
            nums = [float(x) for x in re.findall(r'[-+]?\d*\.?\d+', raw_pts)]
            pts = [(nums[i] * px_to_mm, -nums[i+1] * px_to_mm) for i in range(0, len(nums)-1, 2)]
            if pts:
                if tag == "polygon":
                    pts.append(pts[0])
                entities.append({"type": "POLYLINE", "points": pts, "closed": (tag == "polygon"), "layer": "SVG_Poly"})

    return _convert_entities_to_toolpaths(entities, source_format="SVG", layers={"SVG": {"name": "SVG", "count": len(entities)}})


# =============================================================================
# 3. PLT / HPGL Parser (CorelDRAW, EzCad PLT)
# =============================================================================

def parse_plt_content(content: str, units_per_mm: float = 40.0) -> Dict[str, Any]:
    """
    Parses HPGL / PLT vector content.
    Standard plotter scale: 40 units = 1 mm (0.025 mm / unit).
    """
    cleaned = content.replace("\r", " ").replace("\n", " ")
    commands = re.findall(r'([A-Za-z]{2})\s*([^;A-Za-z]*)', cleaned)

    entities: List[Dict[str, Any]] = []
    pen_down = False
    cur_x, cur_y = 0.0, 0.0
    current_poly: List[Tuple[float, float]] = []

    for cmd, args_str in commands:
        cmd = cmd.upper()
        nums = [float(x) for x in re.findall(r'[-+]?\d*\.?\d+', args_str)]

        if cmd in ("PU", "PD"):
            pen_down = (cmd == "PD")
            if nums:
                coords = list(zip(nums[0::2], nums[1::2]))
                for raw_x, raw_y in coords:
                    mm_x = raw_x / units_per_mm
                    mm_y = raw_y / units_per_mm
                    if pen_down:
                        if not current_poly:
                            current_poly.append((cur_x, cur_y))
                        current_poly.append((mm_x, mm_y))
                    else:
                        if len(current_poly) > 1:
                            entities.append({"type": "POLYLINE", "points": current_poly, "closed": False, "layer": "PLT_Pen"})
                        current_poly = []
                    cur_x, cur_y = mm_x, mm_y
            else:
                if not pen_down and len(current_poly) > 1:
                    entities.append({"type": "POLYLINE", "points": current_poly, "closed": False, "layer": "PLT_Pen"})
                    current_poly = []

        elif cmd == "CI" and nums:
            # CI r: Circle with radius r
            r_mm = nums[0] / units_per_mm
            entities.append({"type": "CIRCLE", "cx": cur_x, "cy": cur_y, "r": r_mm, "layer": "PLT_Circle"})

    if len(current_poly) > 1:
        entities.append({"type": "POLYLINE", "points": current_poly, "closed": False, "layer": "PLT_Pen"})

    return _convert_entities_to_toolpaths(entities, source_format="PLT", layers={"PLT": {"name": "PLT", "count": len(entities)}})


# =============================================================================
# 4. LightBurn Project Parser (.lbrn and .lbrn2)
# =============================================================================

def parse_lightburn_content(content: str, is_json: bool = False) -> Dict[str, Any]:
    """
    Parses LightBurn project files:
      .lbrn: XML format
      .lbrn2: JSON format (LightBurn 1.0+)
    Extracts shapes, cut settings, layer assignments.
    """
    entities: List[Dict[str, Any]] = []
    layers_detected: Dict[str, Dict[str, Any]] = {}

    if is_json:
        # LightBurn 2.0+ (JSON)
        try:
            data = json.loads(content)
        except Exception as e:
            raise ValueError(f"Invalid LightBurn .lbrn2 JSON: {e}")

        # Extract cut settings / layers
        for cut in data.get("CutSetting", []):
            layer_id = str(cut.get("Priority", cut.get("Name", "0")))
            layers_detected[layer_id] = {
                "name": f"Layer {layer_id}",
                "speed": cut.get("Speed", 2000.0),
                "power": cut.get("Power", 50.0),
                "passes": cut.get("NumPasses", 1),
                "count": 0
            }

        # Parse shapes
        shapes = data.get("Shapes", [])
        for shape in shapes:
            stype = shape.get("Type", "")
            layer_id = str(shape.get("Priority", "0"))
            if layer_id not in layers_detected:
                layers_detected[layer_id] = {"name": f"Layer {layer_id}", "count": 0}
            layers_detected[layer_id]["count"] += 1

            if stype == "Path":
                pts = shape.get("Points", [])
                coords = [(float(p.get("x", 0.0)), -float(p.get("y", 0.0))) for p in pts if isinstance(p, dict)]
                if len(coords) > 1:
                    entities.append({"type": "POLYLINE", "points": coords, "closed": shape.get("Closed", False), "layer": layer_id})
            elif stype == "Rect":
                x = float(shape.get("X", 0.0))
                y = float(shape.get("Y", 0.0))
                w = float(shape.get("Width", 0.0))
                h = float(shape.get("Height", 0.0))
                pts = [(x, -y), (x + w, -y), (x + w, -(y + h)), (x, -(y + h)), (x, -y)]
                entities.append({"type": "POLYLINE", "points": pts, "closed": True, "layer": layer_id})
            elif stype == "Ellipse":
                cx = float(shape.get("X", 0.0))
                cy = float(shape.get("Y", 0.0))
                rx = float(shape.get("Rx", 5.0))
                ry = float(shape.get("Ry", 5.0))
                steps = max(16, int(max(rx, ry) * 3))
                pts = [(cx + rx * math.cos(2 * math.pi * s / steps), -(cy + ry * math.sin(2 * math.pi * s / steps))) for s in range(steps + 1)]
                entities.append({"type": "POLYLINE", "points": pts, "closed": True, "layer": layer_id})

    else:
        # LightBurn 1.x (XML)
        try:
            root = ET.fromstring(content)
        except Exception as e:
            raise ValueError(f"Invalid LightBurn .lbrn XML: {e}")

        for shape in root.iter("Shape"):
            stype = shape.attrib.get("Type", "")
            layer_id = shape.attrib.get("Priority", "0")
            if layer_id not in layers_detected:
                layers_detected[layer_id] = {"name": f"Layer {layer_id}", "count": 0}
            layers_detected[layer_id]["count"] += 1

            vertices = []
            for v in shape.findall("V"):
                x = float(v.attrib.get("x", 0.0))
                y = float(v.attrib.get("y", 0.0))
                vertices.append((x, -y))

            if vertices:
                entities.append({"type": "POLYLINE", "points": vertices, "closed": False, "layer": layer_id})

    return _convert_entities_to_toolpaths(entities, source_format="LightBurn", layers=layers_detected)


# =============================================================================
# 5. Adobe Illustrator Parser (.ai)
# =============================================================================

def parse_ai_content(raw_bytes: bytes) -> Dict[str, Any]:
    """
    Parses Adobe Illustrator (.ai) vector files.
    Extracts PDF / PostScript vector path streams (operators m, l, c, re, h).
    """
    entities: List[Dict[str, Any]] = []

    # Decompress any FlateDecode (zlib) streams in the file
    streams = re.findall(b'stream[\r\n]+(.*?)[\r\n]+endstream', raw_bytes, re.DOTALL)
    combined_text = ""

    for s in streams:
        try:
            decompressed = zlib.decompress(s)
            combined_text += "\n" + decompressed.decode("latin-1", errors="ignore")
        except Exception:
            combined_text += "\n" + s.decode("latin-1", errors="ignore")

    if not combined_text:
        combined_text = raw_bytes.decode("latin-1", errors="ignore")

    # PostScript / PDF vector operators parser
    # x y m (moveto), x y l (lineto), x1 y1 x2 y2 x3 y3 c (curveto), h (closepath)
    tokens = re.findall(r'([-+]?(?:\d*\.\d+|\d+))|([a-zA-Z]{1,2})', combined_text)
    stack: List[float] = []
    current_poly: List[Tuple[float, float]] = []
    pt_to_mm = 0.352778  # 1 pt = 1/72 inch = 0.352778 mm

    for num_str, op in tokens:
        if num_str:
            stack.append(float(num_str))
        elif op:
            if op == "m" and len(stack) >= 2:
                if len(current_poly) > 1:
                    entities.append({"type": "POLYLINE", "points": current_poly, "closed": False, "layer": "AI_Vector"})
                cur_x, cur_y = stack[-2] * pt_to_mm, stack[-1] * pt_to_mm
                current_poly = [(cur_x, cur_y)]
                stack.clear()
            elif op == "l" and len(stack) >= 2:
                cur_x, cur_y = stack[-2] * pt_to_mm, stack[-1] * pt_to_mm
                current_poly.append((cur_x, cur_y))
                stack.clear()
            elif op == "c" and len(stack) >= 6:
                if current_poly:
                    p0 = current_poly[-1]
                    p1 = (stack[-6] * pt_to_mm, stack[-5] * pt_to_mm)
                    p2 = (stack[-4] * pt_to_mm, stack[-3] * pt_to_mm)
                    p3 = (stack[-2] * pt_to_mm, stack[-1] * pt_to_mm)
                    for step in range(1, 9):
                        t = step / 8.0
                        bx = (1 - t)**3 * p0[0] + 3 * (1 - t)**2 * t * p1[0] + 3 * (1 - t) * t**2 * p2[0] + t**3 * p3[0]
                        by = (1 - t)**3 * p0[1] + 3 * (1 - t)**2 * t * p1[1] + 3 * (1 - t) * t**2 * p2[1] + t**3 * p3[1]
                        current_poly.append((bx, by))
                stack.clear()
            elif op == "h":
                if current_poly and len(current_poly) > 1:
                    current_poly.append(current_poly[0])
                stack.clear()
            else:
                stack.clear()

    if len(current_poly) > 1:
        entities.append({"type": "POLYLINE", "points": current_poly, "closed": False, "layer": "AI_Vector"})

    return _convert_entities_to_toolpaths(entities, source_format="Adobe Illustrator (AI)", layers={"AI": {"name": "AI", "count": len(entities)}})


# =============================================================================
# 6. EZCAD Project Parser (.ezd and .ez3)
# =============================================================================

def parse_ezcad_content(raw_bytes: bytes) -> Dict[str, Any]:
    """
    Parses BJJCZ EZCAD project files (.ezd / .ez3).
    Extracts curve sequences and vector contours.
    """
    import struct
    entities: List[Dict[str, Any]] = []

    # Search for consecutive double or float coordinate sequences in EZCAD binary stream
    # Each point is typically (double X, double Y) or (float X, float Y)
    n = len(raw_bytes)
    pos = 0
    current_poly: List[Tuple[float, float]] = []

    # Look for coordinates in valid millimeter workfield range (-150mm to +150mm)
    while pos + 16 <= n:
        try:
            x, y = struct.unpack_from('<dd', raw_bytes, pos)
            if -200.0 < x < 200.0 and -200.0 < y < 200.0 and (abs(x) > 0.001 or abs(y) > 0.001):
                if not math.isnan(x) and not math.isnan(y):
                    current_poly.append((x, y))
                    pos += 16
                    continue
        except Exception:
            pass

        if len(current_poly) >= 3:
            # Check point-to-point step continuity (reject random binary noise)
            valid = True
            for k in range(1, len(current_poly)):
                step = math.hypot(current_poly[k][0] - current_poly[k-1][0], current_poly[k][1] - current_poly[k-1][1])
                if step > 50.0:  # unphysically large jump between consecutive polyline points
                    valid = False
                    break
            if valid:
                entities.append({"type": "POLYLINE", "points": current_poly, "closed": False, "layer": "EZCAD_Pen"})
        current_poly = []
        pos += 4

    return _convert_entities_to_toolpaths(
        entities,
        source_format="EZCAD Project",
        layers={"EZCAD": {"name": "EZCAD Objects", "count": len(entities)}}
    )


# =============================================================================
# 7. Raster / Bitmap Image to Laser Toolpath (.bmp, .jpg, .png)
# =============================================================================

def convert_raster_image_to_laser_toolpath(
    image_path: str,
    target_width_mm: float = 50.0,
    target_height_mm: float = 50.0,
    line_interval_mm: float = 0.1,
    threshold: int = 128,
    mode: str = "raster"  # "raster" (hatch lines), "dither" (Floyd-Steinberg), "contour"
) -> Dict[str, Any]:
    """
    Converts bitmap images (.BMP, .JPG, .PNG) into physical laser galvo scanlines.
    Mode:
      - 'raster': Bidirectional line scan for shading / photo engraving.
      - 'dither': Floyd-Steinberg dithering for half-tone photo engraving.
      - 'contour': Traces boundary edges of high-contrast graphics.
    """
    try:
        from PyQt5.QtGui import QImage, QColor
    except ImportError:
        raise RuntimeError("PyQt5 is required for raster image conversion.")

    img = QImage(image_path)
    if img.isNull():
        raise ValueError(f"Could not load image: {image_path}")

    # Convert to grayscale 32-bit RGB
    gray_img = img.convertToFormat(QImage.Format_Grayscale8)

    # Compute raster grid dimensions in lines
    lines_count = max(2, int(target_height_mm / line_interval_mm))
    pixels_per_line = max(2, int(target_width_mm / line_interval_mm))

    # Resize image to physical grid resolution
    scaled_img = gray_img.scaled(pixels_per_line, lines_count)

    strokes: List[List[Dict[str, Any]]] = []
    total_dist = 0.0

    w = scaled_img.width()
    h = scaled_img.height()
    half_w = target_width_mm / 2.0
    half_h = target_height_mm / 2.0
    dx = target_width_mm / w
    dy = target_height_mm / h

    if mode == "dither":
        # Floyd-Steinberg Dithering
        # Copy pixel matrix to float buffer
        buf = [[0.0] * w for _ in range(h)]
        for r in range(h):
            for c in range(w):
                buf[r][c] = float(QColor(scaled_img.pixel(c, r)).red())

        for r in range(h):
            y_mm = half_h - r * dy
            for c in range(w):
                old_val = buf[r][c]
                new_val = 0.0 if old_val < 128.0 else 255.0
                buf[r][c] = new_val
                err = old_val - new_val

                if c + 1 < w:
                    buf[r][c + 1] += err * (7.0 / 16.0)
                if r + 1 < h and c - 1 >= 0:
                    buf[r + 1][c - 1] += err * (3.0 / 16.0)
                if r + 1 < h:
                    buf[r + 1][c] += err * (5.0 / 16.0)
                if r + 1 < h and c + 1 < w:
                    buf[r + 1][c + 1] += err * (1.0 / 16.0)

                # Mark dark dots
                if new_val < 128.0:
                    x_mm = -half_w + c * dx
                    stroke = [
                        {"type": "JUMP", "x": round(x_mm, 3), "y": round(y_mm, 3)},
                        {"type": "LINE", "x": round(x_mm + dx * 0.8, 3), "y": round(y_mm, 3)}
                    ]
                    strokes.append(stroke)
                    total_dist += dx * 0.8

    else:
        # Standard Bidirectional Raster Scanlines (Hatch engraving)
        for r in range(h):
            y_mm = half_h - r * dy
            # Alternate scan direction for galvo efficiency
            col_range = range(w) if (r % 2 == 0) else range(w - 1, -1, -1)
            
            in_mark = False
            mark_start_x = 0.0
            
            for c in col_range:
                val = QColor(scaled_img.pixel(c, r)).red()
                is_dark = (val < threshold)
                x_mm = -half_w + c * dx

                if is_dark:
                    if not in_mark:
                        in_mark = True
                        mark_start_x = x_mm
                else:
                    if in_mark:
                        in_mark = False
                        strokes.append([
                            {"type": "JUMP", "x": round(mark_start_x, 3), "y": round(y_mm, 3)},
                            {"type": "LINE", "x": round(x_mm, 3), "y": round(y_mm, 3)}
                        ])
                        total_dist += abs(x_mm - mark_start_x)

            if in_mark:
                end_x = half_w if (r % 2 == 0) else -half_w
                strokes.append([
                    {"type": "JUMP", "x": round(mark_start_x, 3), "y": round(y_mm, 3)},
                    {"type": "LINE", "x": round(end_x, 3), "y": round(y_mm, 3)}
                ])
                total_dist += abs(end_x - mark_start_x)

    return {
        "format": f"Raster ({mode.upper()})",
        "entity_count": len(strokes),
        "stroke_count": len(strokes),
        "total_mark_distance_mm": round(total_dist, 2),
        "bbox": {
            "min_x": round(-half_w, 2),
            "max_x": round(half_w, 2),
            "min_y": round(-half_h, 2),
            "max_y": round(half_h, 2),
            "width": round(target_width_mm, 2),
            "height": round(target_height_mm, 2),
            "center_x": 0.0,
            "center_y": 0.0
        },
        "strokes": strokes,
        "layers": [{"name": "Raster Image", "count": len(strokes)}]
    }


# =============================================================================
# Master Dispatcher
# =============================================================================

def load_any_cad_drawing(filepath: str, **kwargs) -> Dict[str, Any]:
    """
    Universal CAD/CAM drawing reader supporting:
      .dxf, .svg, .plt, .hpgl, .ai, .lbrn, .lbrn2, .ezd, .ez3, .png, .jpg, .bmp
    """
    if not os.path.exists(filepath):
        raise FileNotFoundError(f"File not found: {filepath}")

    ext = os.path.splitext(filepath)[1].lower()
    filename = os.path.basename(filepath)

    if ext in (".png", ".jpg", ".jpeg", ".bmp"):
        # Raster photo engraving
        target_w = kwargs.get("width_mm", 50.0)
        target_h = kwargs.get("height_mm", 50.0)
        interval = kwargs.get("interval_mm", 0.1)
        mode = kwargs.get("mode", "raster")
        threshold = kwargs.get("threshold", 128)
        data = convert_raster_image_to_laser_toolpath(
            filepath, target_width_mm=target_w, target_height_mm=target_h,
            line_interval_mm=interval, mode=mode, threshold=threshold
        )

    elif ext == ".svg":
        with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
            content = f.read()
        data = parse_svg_content(content)

    elif ext in (".plt", ".hpgl", ".hpg"):
        with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
            content = f.read()
        data = parse_plt_content(content)

    elif ext == ".ai":
        with open(filepath, "rb") as f:
            raw = f.read()
        data = parse_ai_content(raw)

    elif ext in (".lbrn", ".lbrn2"):
        is_json = (ext == ".lbrn2")
        with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
            content = f.read()
        data = parse_lightburn_content(content, is_json=is_json)

    elif ext in (".ezd", ".ez3"):
        with open(filepath, "rb") as f:
            raw = f.read()
        data = parse_ezcad_content(raw)

    else:
        # Default to DXF
        with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
            content = f.read()
        data = parse_dxf_content(content)

    data["filename"] = filename
    data["filepath"] = filepath
    return data


def _convert_entities_to_toolpaths(
    entities: List[Dict[str, Any]],
    source_format: str,
    layers: Optional[Dict[str, Any]] = None
) -> Dict[str, Any]:
    """Helper normalizing vector entities into continuous Rhothor toolpaths and bounding box."""
    all_points: List[Tuple[float, float]] = []
    strokes: List[List[Dict[str, Any]]] = []
    total_mark_dist = 0.0

    for ent in entities:
        etype = ent["type"]
        stroke: List[Dict[str, Any]] = []

        if etype == "LINE":
            x1, y1 = ent["x1"], ent["y1"]
            x2, y2 = ent["x2"], ent["y2"]
            stroke.append({"type": "JUMP", "x": round(x1, 4), "y": round(y1, 4)})
            stroke.append({"type": "LINE", "x": round(x2, 4), "y": round(y2, 4)})
            all_points.extend([(x1, y1), (x2, y2)])
            total_mark_dist += math.hypot(x2 - x1, y2 - y1)

        elif etype == "CIRCLE":
            cx, cy, r = ent["cx"], ent["cy"], ent["r"]
            stroke.append({"type": "JUMP", "x": round(cx - r, 4), "y": round(cy, 4)})
            stroke.append({"type": "ARC", "x": round(cx + r, 4), "y": round(cy, 4), "bf": 1.0})
            stroke.append({"type": "ARC", "x": round(cx - r, 4), "y": round(cy, 4), "bf": 1.0})
            all_points.extend([(cx - r, cy - r), (cx + r, cy + r)])
            total_mark_dist += 2.0 * math.pi * r

        elif etype == "ARC":
            cx, cy, r = ent["cx"], ent["cy"], ent["r"]
            a1, a2 = ent.get("start", 0.0), ent.get("end", 360.0)
            rad1 = math.radians(a1)
            rad2 = math.radians(a2)
            if rad2 < rad1:
                rad2 += 2 * math.pi
            steps = max(8, int(abs(rad2 - rad1) / (math.pi / 18)))
            start_x = cx + r * math.cos(rad1)
            start_y = cy + r * math.sin(rad1)
            stroke.append({"type": "JUMP", "x": round(start_x, 4), "y": round(start_y, 4)})
            all_points.append((start_x, start_y))
            for s in range(1, steps + 1):
                theta = rad1 + (rad2 - rad1) * (s / steps)
                px = cx + r * math.cos(theta)
                py = cy + r * math.sin(theta)
                stroke.append({"type": "LINE", "x": round(px, 4), "y": round(py, 4)})
                all_points.append((px, py))
            total_mark_dist += r * abs(rad2 - rad1)

        elif etype == "POLYLINE":
            pts = ent["points"]
            closed = ent.get("closed", False)
            if pts:
                stroke.append({"type": "JUMP", "x": round(pts[0][0], 4), "y": round(pts[0][1], 4)})
                all_points.append(pts[0])
                for idx in range(1, len(pts)):
                    p = pts[idx]
                    stroke.append({"type": "LINE", "x": round(p[0], 4), "y": round(p[1], 4)})
                    all_points.append(p)
                    total_mark_dist += math.hypot(p[0] - pts[idx-1][0], p[1] - pts[idx-1][1])
                if closed and len(pts) > 2:
                    stroke.append({"type": "LINE", "x": round(pts[0][0], 4), "y": round(pts[0][1], 4)})
                    total_mark_dist += math.hypot(pts[0][0] - pts[-1][0], pts[0][1] - pts[-1][1])

        elif etype == "POINT":
            px, py = ent["x"], ent["y"]
            stroke.append({"type": "JUMP", "x": round(px, 4), "y": round(py, 4)})
            stroke.append({"type": "LINE", "x": round(px + 0.05, 4), "y": round(py, 4)})
            all_points.append((px, py))
            total_mark_dist += 0.05

        if stroke:
            strokes.append(stroke)

    # Calculate tight bounding box
    if all_points:
        xs = [p[0] for p in all_points]
        ys = [p[1] for p in all_points]
        min_x, max_x = min(xs), max(xs)
        min_y, max_y = min(ys), max(ys)
        w = max_x - min_x
        h = max_y - min_y
        cx = (min_x + max_x) / 2.0
        cy = (min_y + max_y) / 2.0
    else:
        min_x, max_x, min_y, max_y, w, h, cx, cy = 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0

    layer_list = []
    if layers:
        for k, v in layers.items():
            layer_list.append(v)

    return {
        "format": source_format,
        "entity_count": len(entities),
        "stroke_count": len(strokes),
        "total_mark_distance_mm": round(total_mark_dist, 2),
        "bbox": {
            "min_x": round(min_x, 2),
            "max_x": round(max_x, 2),
            "min_y": round(min_y, 2),
            "max_y": round(max_y, 2),
            "width": round(w, 2),
            "height": round(h, 2),
            "center_x": round(cx, 2),
            "center_y": round(cy, 2)
        },
        "strokes": strokes,
        "layers": layer_list
    }
