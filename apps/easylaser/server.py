#!/usr/bin/env python3
"""
Rhothor EasyLaser Backend Server
Lightweight, Zero-Dependency HTTP Service for Laser Marking Operators.
Provides REST API for DXF/PLT parsing, toolpath preview, and Rhothor hardware execution.
"""

import os
import sys
import json
import webbrowser
from http.server import ThreadingHTTPServer, SimpleHTTPRequestHandler
from typing import Dict, Any

# Add project root to sys.path
PROJECT_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
sys.path.insert(0, PROJECT_ROOT)

from apps.easylaser.parsers import parse_dxf_content, parse_plt_content
from sdk.python.rhothor import RhothorController, RhothorError

# Global controller instance (managed by server)
HARDWARE_PROFILE = {
    "serial_number": 4281,
    "name": "LPKF",
    "field_size_mm": 201.65,
    "max_speed_mm_s": 6000.0,
    "scanner_delay_us": 60
}

current_vector_job: Dict[str, Any] = {}
galvo_instance: RhothorController = None


def get_galvo() -> RhothorController:
    global galvo_instance
    if galvo_instance is None:
        galvo_instance = RhothorController(
            serial_nr=HARDWARE_PROFILE["serial_number"],
            name=HARDWARE_PROFILE["name"]
        )
        galvo_instance.connect()
    return galvo_instance


class EasyLaserHandler(SimpleHTTPRequestHandler):
    """Custom HTTP handler serving web assets and JSON REST APIs."""

    def __init__(self, *args, **kwargs):
        static_dir = os.path.join(os.path.dirname(__file__), "static")
        super().__init__(*args, directory=static_dir, **kwargs)

    def _send_json(self, data: Dict[str, Any], status: int = 200):
        body = json.dumps(data).encode("utf-8")
        self.send_response(status)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.send_header("Access-Control-Allow-Origin", "*")
        self.end_headers()
        self.wfile.write(body)

    def do_OPTIONS(self):
        self.send_response(200)
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        self.end_headers()

    def do_GET(self):
        if self.path == "/api/status":
            try:
                g = get_galvo()
                info = g.get_device_info()
                self._send_json({"status": "ok", "device": info, "profile": HARDWARE_PROFILE})
            except Exception as e:
                self._send_json({"status": "error", "message": str(e), "profile": HARDWARE_PROFILE})
            return

        # Serve static assets
        super().do_GET()

    def do_POST(self):
        global current_vector_job
        content_len = int(self.headers.get("Content-Length", 0))
        body = self.rfile.read(content_len).decode("utf-8", errors="ignore")

        if self.path == "/api/parse":
            try:
                req = json.loads(body)
                content = req.get("content", "")
                filename = req.get("filename", "unnamed.dxf")
                ext = os.path.splitext(filename)[1].lower()

                if ext in (".plt", ".hpgl", ".hpg"):
                    parsed = parse_plt_content(content)
                else:
                    parsed = parse_dxf_content(content)

                parsed["filename"] = filename
                current_vector_job = parsed
                self._send_json({"status": "ok", "job": parsed})
            except Exception as e:
                self._send_json({"status": "error", "message": str(e)}, 400)
            return

        elif self.path == "/api/mark":
            try:
                req = json.loads(body)
                mark_speed = float(req.get("mark_speed", 1500.0))
                jump_speed = float(req.get("jump_speed", 5000.0))
                passes = int(req.get("passes", 1))
                offset_x = float(req.get("offset_x", 0.0))
                offset_y = float(req.get("offset_y", 0.0))
                scale = float(req.get("scale", 1.0))
                strokes = req.get("strokes") or current_vector_job.get("strokes", [])

                if not strokes:
                    self._send_json({"status": "error", "message": "No vector data loaded"}, 400)
                    return

                g = get_galvo()
                for p in range(passes):
                    g.list_open(1)
                    g.set_speed(mark_speed, jump_speed)
                    if offset_x != 0.0 or offset_y != 0.0:
                        g.set_offsets(offset_x, offset_y)
                    for stroke in strokes:
                        for pt in stroke:
                            pt_type = pt.get("type")
                            px = pt.get("x", 0.0) * scale
                            py = pt.get("y", 0.0) * scale
                            if pt_type == "JUMP":
                                g.jump_to(px, py)
                            elif pt_type == "LINE":
                                g.line_to(px, py)
                            elif pt_type == "ARC":
                                g.arc_to(px, py, pt.get("bf", 1.0))
                    g.list_close()

                metrics = {}
                if g.simulate:
                    metrics = g.get_simulation_stats()

                self._send_json({"status": "ok", "passes": passes, "metrics": metrics})
            except Exception as e:
                self._send_json({"status": "error", "message": str(e)}, 500)
            return

        elif self.path == "/api/frame":
            # Traces bounding box outline with laser off for workpiece alignment
            try:
                req = json.loads(body)
                bbox = req.get("bbox") or current_vector_job.get("bbox", {})
                scale = float(req.get("scale", 1.0))
                ox = float(req.get("offset_x", 0.0))
                oy = float(req.get("offset_y", 0.0))

                if not bbox:
                    self._send_json({"status": "error", "message": "No bounding box found"}, 400)
                    return

                min_x = bbox["min_x"] * scale + ox
                max_x = bbox["max_x"] * scale + ox
                min_y = bbox["min_y"] * scale + oy
                max_y = bbox["max_y"] * scale + oy

                g = get_galvo()
                g.list_open(1)
                g.set_speed(mark_speed=2000.0, jump_speed=5000.0)
                # Move around contour with laser OFF
                g.jump_to(min_x, min_y)
                g.move_to(max_x, min_y)
                g.move_to(max_x, max_y)
                g.move_to(min_x, max_y)
                g.move_to(min_x, min_y)
                g.list_close()

                self._send_json({"status": "ok", "message": "Frame contour traced"})
            except Exception as e:
                self._send_json({"status": "error", "message": str(e)}, 500)
            return

        elif self.path == "/api/abort":
            try:
                g = get_galvo()
                g.abort()
                self._send_json({"status": "ok", "message": "Motion aborted"})
            except Exception as e:
                self._send_json({"status": "error", "message": str(e)}, 500)
            return

        self._send_json({"status": "error", "message": "Endpoint not found"}, 404)


def run_server(port: int = 8080, open_browser: bool = True):
    server = ThreadingHTTPServer(("0.0.0.0", port), EasyLaserHandler)
    url = f"http://localhost:{port}"
    print("=" * 60)
    print("  Rhothor EasyLaser: Beginner-Friendly Laser Software")
    print(f"  Web Interface running at: {url}")
    print("  Press Ctrl+C to stop.")
    print("=" * 60)

    if open_browser:
        try:
            webbrowser.open(url)
        except Exception:
            pass

    try:
        server.serve_forever()
    except KeyboardInterrupt:
        print("\nShutting down EasyLaser server...")
        server.shutdown()


if __name__ == "__main__":
    port = 8080
    if len(sys.argv) > 1 and sys.argv[1].isdigit():
        port = int(sys.argv[1])
    run_server(port=port, open_browser=False)
