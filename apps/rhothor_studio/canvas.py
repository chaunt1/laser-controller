"""
Interactive Vector Canvas for Rhothor Laser Studio.
Provides hardware-accelerated 2D/3D toolpath visualization, real-time coordinate tracking,
scanner working field limits, and interactive pan/zoom navigation.
"""

from typing import List, Dict, Any, Optional
from PyQt5.QtWidgets import QGraphicsView, QGraphicsScene, QGraphicsItem
from PyQt5.QtCore import Qt, QPointF, QRectF, pyqtSignal
from PyQt5.QtGui import (
    QPen, QBrush, QColor, QPainter, QFont, QPainterPath, QTransform
)


class LaserCanvasView(QGraphicsView):
    """
    QGraphicsView tailored for laser scanning kinematics visualization.
    Coordinates are defined in physical millimeters (mm), where (0,0) is scan center.
    """
    coordinates_changed = pyqtSignal(float, float)  # Emits (x_mm, y_mm)

    def __init__(self, field_size_mm: float = 201.65, parent=None):
        super().__init__(parent)
        self.field_size_mm = field_size_mm
        self.scene = QGraphicsScene(self)
        self.setScene(self.scene)

        # Rendering options
        self.setRenderHint(QPainter.Antialiasing, True)
        self.setRenderHint(QPainter.SmoothPixmapTransform, True)
        self.setDragMode(QGraphicsView.ScrollHandDrag)
        self.setHorizontalScrollBarPolicy(Qt.ScrollBarAlwaysOff)
        self.setVerticalScrollBarPolicy(Qt.ScrollBarAlwaysOff)
        self.setBackgroundBrush(QBrush(QColor("#111317")))
        self.setMouseTracking(True)

        # Invert Y axis so +Y is up (standard Cartesian/galvo geometry)
        self.scale(1.0, -1.0)

        # Data storage
        self.current_strokes: List[List[Dict[str, Any]]] = []
        self.bounding_box: Optional[Dict[str, float]] = None
        self.active_frame_box: Optional[Dict[str, float]] = None

        self._draw_grid_and_field()
        self.fit_scan_field()

    def set_field_size(self, size_mm: float):
        self.field_size_mm = size_mm
        self.refresh()
        self.fit_scan_field()

    def _draw_grid_and_field(self):
        """Draws the physical boundary of the galvo field and millimeter grid."""
        half = self.field_size_mm / 2.0
        
        # 1. Scanner field boundary
        field_rect = QRectF(-half, -half, self.field_size_mm, self.field_size_mm)
        field_pen = QPen(QColor("#2a3040"), 0.8, Qt.SolidLine)
        field_pen.setCosmetic(True)
        self.scene.addRect(field_rect, field_pen)

        # 2. Grid lines
        minor_pen = QPen(QColor("#181b24"), 0.5, Qt.SolidLine)
        minor_pen.setCosmetic(True)
        major_pen = QPen(QColor("#222735"), 0.8, Qt.SolidLine)
        major_pen.setCosmetic(True)

        step = 10.0  # 10 mm minor grid
        x = -half
        while x <= half:
            pen = major_pen if abs(x % 50.0) < 0.001 else minor_pen
            line = self.scene.addLine(x, -half, x, half, pen)
            line.setZValue(-10)
            x += step

        y = -half
        while y <= half:
            pen = major_pen if abs(y % 50.0) < 0.001 else minor_pen
            line = self.scene.addLine(-half, y, half, y, pen)
            line.setZValue(-10)
            y += step

        # 3. Origin crosshair (0, 0)
        axis_pen = QPen(QColor("#3d4b68"), 1.2, Qt.SolidLine)
        axis_pen.setCosmetic(True)
        self.scene.addLine(-10.0, 0, 10.0, 0, axis_pen)
        self.scene.addLine(0, -10.0, 0, 10.0, axis_pen)

    def load_strokes(self, strokes: List[List[Dict[str, Any]]], bbox: Optional[Dict[str, float]] = None):
        """Loads and renders vector strokes (Mark and Jump commands)."""
        self.current_strokes = strokes
        self.bounding_box = bbox
        self.refresh()

    def clear_vectors(self):
        self.current_strokes = []
        self.bounding_box = None
        self.active_frame_box = None
        self.refresh()

    def set_frame_preview(self, bbox: Optional[Dict[str, float]]):
        self.active_frame_box = bbox
        self.refresh()

    def refresh(self):
        """Redraws the entire scene."""
        self.scene.clear()
        self._draw_grid_and_field()

        # Render Bounding Box
        if self.bounding_box:
            b = self.bounding_box
            w = b["max_x"] - b["min_x"]
            h = b["max_y"] - b["min_y"]
            rect = QRectF(b["min_x"], b["min_y"], w, h)
            bbox_pen = QPen(QColor("#e6c800"), 1.0, Qt.DashLine)
            bbox_pen.setCosmetic(True)
            self.scene.addRect(rect, bbox_pen)

        # Render Active Frame Box (laser guide)
        if self.active_frame_box:
            b = self.active_frame_box
            w = b["max_x"] - b["min_x"]
            h = b["max_y"] - b["min_y"]
            rect = QRectF(b["min_x"], b["min_y"], w, h)
            guide_pen = QPen(QColor("#ff0033"), 2.0, Qt.SolidLine)
            guide_pen.setCosmetic(True)
            self.scene.addRect(rect, guide_pen)

        # Pens for vector marking
        jump_pen = QPen(QColor("#00c8ff"), 0.8, Qt.DotLine)
        jump_pen.setCosmetic(True)

        default_mark_pen = QPen(QColor("#ff3344"), 1.6, Qt.SolidLine)
        default_mark_pen.setCosmetic(True)

        for stroke in self.current_strokes:
            last_pt = None
            for pt in stroke:
                pt_type = pt.get("type", "LINE")
                px = pt.get("x", 0.0)
                py = pt.get("y", 0.0)
                pz = pt.get("z", 0.0)

                if last_pt is not None:
                    lx, ly, _ = last_pt
                    if pt_type == "JUMP":
                        line = self.scene.addLine(lx, ly, px, py, jump_pen)
                        line.setZValue(1)
                    else:
                        # 3D color gradient if Z is varied
                        if abs(pz) > 0.01:
                            # Map Z (-20 mm to +20 mm) to Blue -> Green -> Red
                            norm_z = max(-1.0, min(1.0, pz / 20.0))
                            if norm_z < 0:
                                r = 0
                                g = int((1.0 + norm_z) * 255)
                                b = int(-norm_z * 255)
                            else:
                                r = int(norm_z * 255)
                                g = int((1.0 - norm_z) * 255)
                                b = 0
                            pen = QPen(QColor(r, g, b), 1.6, Qt.SolidLine)
                            pen.setCosmetic(True)
                        else:
                            pen = default_mark_pen

                        line = self.scene.addLine(lx, ly, px, py, pen)
                        line.setZValue(2)

                last_pt = (px, py, pz)

    def fit_scan_field(self):
        """Fits the entire working field into the view viewport with margin."""
        half = (self.field_size_mm / 2.0) * 1.15
        rect = QRectF(-half, -half, half * 2, half * 2)
        self.fitInView(rect, Qt.KeepAspectRatio)

    def wheelEvent(self, event):
        """Smooth zooming via mouse wheel centered on cursor."""
        factor = 1.15 if event.angleDelta().y() > 0 else 0.87
        self.scale(factor, factor)

    def mouseMoveEvent(self, event):
        """Tracks cursor position and emits physical millimeters."""
        super().mouseMoveEvent(event)
        scene_pt = self.mapToScene(event.pos())
        self.coordinates_changed.emit(scene_pt.x(), scene_pt.y())
