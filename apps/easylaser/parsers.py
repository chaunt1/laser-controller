"""
Vector Parsers for DXF and PLT (HPGL) files.
Converts vector artwork into Rhothor laser motion commands (JUMP, LINE, ARC).
Zero external dependencies (Pure Python 3 standard library).
"""

import math
import os
import re
from typing import List, Dict, Any, Tuple, Optional


class VectorPath:
    """Represents a continuous stroke path (1 JUMP followed by 1 or more LINE/ARC)."""
    def __init__(self):
        self.points: List[Dict[str, Any]] = []  # {"type": "JUMP"|"LINE"|"ARC", "x": float, "y": float, ...}

    def add_jump(self, x: float, y: float):
        self.points.append({"type": "JUMP", "x": round(x, 4), "y": round(y, 4)})

    def add_line(self, x: float, y: float):
        self.points.append({"type": "LINE", "x": round(x, 4), "y": round(y, 4)})

    def add_arc(self, x: float, y: float, bf: float):
        self.points.append({"type": "ARC", "x": round(x, 4), "y": round(y, 4), "bf": round(bf, 4)})


def parse_dxf_content(content: str) -> Dict[str, Any]:
    """Parses DXF content into laser toolpaths."""
    lines = [line.strip() for line in content.splitlines()]
    entities = []
    
    i = 0
    n = len(lines)
    while i < n:
        if lines[i] == "0" and i + 1 < n:
            etype = lines[i + 1]
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

            if etype == "LINE":
                x1, y1 = get_f("10"), get_f("20")
                x2, y2 = get_f("11"), get_f("21")
                entities.append({"type": "LINE", "x1": x1, "y1": y1, "x2": x2, "y2": y2})
            elif etype == "CIRCLE":
                cx, cy, r = get_f("10"), get_f("20"), get_f("40", 1.0)
                entities.append({"type": "CIRCLE", "cx": cx, "cy": cy, "r": r})
            elif etype == "ARC":
                cx, cy, r = get_f("10"), get_f("20"), get_f("40", 1.0)
                start_deg, end_deg = get_f("50"), get_f("51")
                entities.append({"type": "ARC", "cx": cx, "cy": cy, "r": r,
                                 "start": start_deg, "end": end_deg})
            elif etype == "LWPOLYLINE":
                xs = [float(v) for v in props.get("10", [])]
                ys = [float(v) for v in props.get("20", [])]
                flags = int(get_f("70", 0))
                closed = bool(flags & 1)
                pts = list(zip(xs, ys))
                if pts:
                    entities.append({"type": "POLYLINE", "points": pts, "closed": closed})
        else:
            i += 1

    return _convert_entities_to_toolpaths(entities, source_format="DXF")


def parse_plt_content(content: str, units_per_mm: float = 40.0) -> Dict[str, Any]:
    """
    Parses HPGL / PLT content exported from CorelDRAW, EzCad, AutoCAD, etc.
    Standard plotter scale: 40 units = 1 mm (0.025 mm / unit).
    """
    # Remove whitespace, normalize commands
    cleaned = content.replace("\r", " ").replace("\n", " ")
    # Split by semicolon or command letters
    commands = re.findall(r'([A-Za-z]{2})\s*([^;A-Za-z]*)', cleaned)

    entities = []
    pen_down = False
    cur_x, cur_y = 0.0, 0.0
    current_poly: List[Tuple[float, float]] = []

    for cmd, args_str in commands:
        cmd = cmd.upper()
        # Parse numbers in args
        nums = [float(x) for x in re.findall(r'[-+]?\d*\.?\d+', args_str)]

        if cmd == "IN":
            pen_down = False
        elif cmd in ("PU", "PD"):
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
                            entities.append({"type": "POLYLINE", "points": current_poly, "closed": False})
                        current_poly = []
                    cur_x, cur_y = mm_x, mm_y
            else:
                if not pen_down and len(current_poly) > 1:
                    entities.append({"type": "POLYLINE", "points": current_poly, "closed": False})
                    current_poly = []

    if len(current_poly) > 1:
        entities.append({"type": "POLYLINE", "points": current_poly, "closed": False})

    return _convert_entities_to_toolpaths(entities, source_format="PLT")


def _convert_entities_to_toolpaths(entities: List[Dict[str, Any]], source_format: str) -> Dict[str, Any]:
    """Normalizes geometric entities into continuous Rhothor toolpaths and calculates bounding box."""
    all_points: List[Tuple[float, float]] = []
    strokes: List[List[Dict[str, Any]]] = []
    total_mark_dist = 0.0

    for ent in entities:
        etype = ent["type"]
        stroke: List[Dict[str, Any]] = []

        if etype == "LINE":
            x1, y1 = ent["x1"], ent["y1"]
            x2, y2 = ent["x2"], ent["y2"]
            stroke.append({"type": "JUMP", "x": x1, "y": y1})
            stroke.append({"type": "LINE", "x": x2, "y": y2})
            all_points.extend([(x1, y1), (x2, y2)])
            total_mark_dist += math.hypot(x2 - x1, y2 - y1)

        elif etype == "CIRCLE":
            cx, cy, r = ent["cx"], ent["cy"], ent["r"]
            # Represent circle with two semicircle arcs (bf = 1.0)
            stroke.append({"type": "JUMP", "x": cx - r, "y": cy})
            stroke.append({"type": "ARC", "x": cx + r, "y": cy, "bf": 1.0})
            stroke.append({"type": "ARC", "x": cx - r, "y": cy, "bf": 1.0})
            all_points.extend([(cx - r, cy - r), (cx + r, cy + r)])
            total_mark_dist += 2.0 * math.pi * r

        elif etype == "ARC":
            cx, cy, r = ent["cx"], ent["cy"], ent["r"]
            a1, a2 = ent["start"], ent["end"]
            rad1 = math.radians(a1)
            rad2 = math.radians(a2)
            if rad2 < rad1:
                rad2 += 2 * math.pi
            steps = max(8, int(abs(rad2 - rad1) / (math.pi / 18)))
            start_x = cx + r * math.cos(rad1)
            start_y = cy + r * math.sin(rad1)
            stroke.append({"type": "JUMP", "x": start_x, "y": start_y})
            all_points.append((start_x, start_y))
            for s in range(1, steps + 1):
                theta = rad1 + (rad2 - rad1) * (s / steps)
                px = cx + r * math.cos(theta)
                py = cy + r * math.sin(theta)
                stroke.append({"type": "LINE", "x": px, "y": py})
                all_points.append((px, py))
            total_mark_dist += r * abs(rad2 - rad1)

        elif etype == "POLYLINE":
            pts = ent["points"]
            closed = ent.get("closed", False)
            if pts:
                stroke.append({"type": "JUMP", "x": pts[0][0], "y": pts[0][1]})
                all_points.append(pts[0])
                for idx in range(1, len(pts)):
                    p = pts[idx]
                    stroke.append({"type": "LINE", "x": p[0], "y": p[1]})
                    all_points.append(p)
                    total_mark_dist += math.hypot(p[0] - pts[idx-1][0], p[1] - pts[idx-1][1])
                if closed and len(pts) > 2:
                    stroke.append({"type": "LINE", "x": pts[0][0], "y": pts[0][1]})
                    total_mark_dist += math.hypot(pts[0][0] - pts[-1][0], pts[0][1] - pts[-1][1])

        if stroke:
            strokes.append(stroke)

    # Compute bounding box
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
        min_x, max_x, min_y, max_y, w, h, cx, cy = 0, 0, 0, 0, 0, 0, 0, 0

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
        "strokes": strokes
    }


def parse_vector_file(filepath: str) -> Dict[str, Any]:
    """Detects format by extension and parses."""
    ext = os.path.splitext(filepath)[1].lower()
    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
        content = f.read()

    if ext in (".plt", ".hpgl", ".hpg"):
        data = parse_plt_content(content)
    else:
        # Default to DXF
        data = parse_dxf_content(content)
        
    data["filename"] = os.path.basename(filepath)
    return data
