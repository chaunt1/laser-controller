"""
Tab 02: 2D Vector Marking & Shape Studio (Based on Example 02 + DXF/PLT Parser).
Provides preset geometric shape generators (border, concentric circles, crosshairs,
dense hatching) and full DXF / PLT CAD vector file loading and execution.
"""

import os
from typing import List, Dict, Any, Optional
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QPushButton, QGroupBox, QFileDialog, QDoubleSpinBox, QSpinBox,
    QComboBox, QMessageBox, QFrame
)
from PyQt5.QtCore import Qt, pyqtSignal

from apps.easylaser.parsers import parse_dxf_content, parse_plt_content
from sdk.python.rhothor import RhothorController


class Marking2DTab(QWidget):
    """2D Vector Marking, geometric presets, and CAD vector file manager."""
    vectors_changed = pyqtSignal(list, object)  # (strokes, bbox)
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller
        self.current_strokes: List[List[Dict[str, Any]]] = []
        self.current_bbox: Optional[Dict[str, float]] = None
        self.active_filename = "Chưa có file"

        self._init_ui()

    def _init_ui(self):
        layout = QVBoxLayout(self)
        layout.setSpacing(12)

        # 1. Preset Shape Generators (From Example 02)
        preset_group = QGroupBox("1. HÌNH MẪU ĐỊNH HÌNH CÓ SẴN (EXAMPLE 02 SHAPES)")
        preset_layout = QGridLayout(preset_group)
        preset_layout.setSpacing(8)

        # Border 100x100
        self.btn_border = QPushButton("Khung Viền 100x100 mm")
        self.btn_border.clicked.connect(self.generate_border)
        preset_layout.addWidget(self.btn_border, 0, 0)

        # Concentric Circles
        self.btn_circles = QPushButton("Vòng Tròn Đồng Tâm (10-40mm)")
        self.btn_circles.clicked.connect(self.generate_concentric_circles)
        preset_layout.addWidget(self.btn_circles, 0, 1)

        # Crosshair
        self.btn_crosshair = QPushButton("Tâm Căn Chỉnh Crosshair")
        self.btn_crosshair.clicked.connect(self.generate_crosshair)
        preset_layout.addWidget(self.btn_crosshair, 1, 0)

        # Hatch Box
        self.btn_hatch = QPushButton("Ô Đan Lưới (Hatch 20x20mm)")
        self.btn_hatch.clicked.connect(self.generate_hatch_box)
        preset_layout.addWidget(self.btn_hatch, 1, 1)

        # Load Full Example 02 Combo
        self.btn_combo = QPushButton("⚡ TỔ HỢP TẤT CẢ MẪU THỬ EXAMPLE 02")
        self.btn_combo.setStyleSheet("background-color: #2a3b5c; color: #00d2ff; font-weight: bold; padding: 6px;")
        self.btn_combo.clicked.connect(self.generate_all_example2_shapes)
        preset_layout.addWidget(self.btn_combo, 2, 0, 1, 2)

        layout.addWidget(preset_group)

        # 2. File CAD Importer (DXF / PLT)
        file_group = QGroupBox("2. NHẬP BẢN VẼ TỪ MÁY TÍNH (DXF / PLT)")
        file_layout = QVBoxLayout(file_group)

        file_btn_row = QHBoxLayout()
        self.btn_open_file = QPushButton("📁 MỞ FILE DXF / PLT...")
        self.btn_open_file.setStyleSheet("background-color: #3b4252; color: white; font-weight: bold; padding: 8px;")
        self.btn_open_file.clicked.connect(self.open_file_dialog)
        file_btn_row.addWidget(self.btn_open_file)

        self.btn_sample_dxf = QPushButton("Mở Mẫu sample_ezcad.dxf")
        self.btn_sample_dxf.clicked.connect(self.load_sample_dxf)
        file_btn_row.addWidget(self.btn_sample_dxf)
        file_layout.addLayout(file_btn_row)

        self.lbl_file_info = QLabel("Tệp hiện tại: Chưa tải bản vẽ")
        self.lbl_file_info.setStyleSheet("color: #8e95a5; font-size: 11px;")
        file_layout.addWidget(self.lbl_file_info)

        layout.addWidget(file_group)

        # 3. Vector Transformations
        trans_group = QGroupBox("3. BIẾN ĐỔI HÌNH HỌC (TRANSFORMS)")
        trans_layout = QGridLayout(trans_group)

        # Scale
        trans_layout.addWidget(QLabel("Tỉ lệ (Scale):"), 0, 0)
        self.spin_scale = QDoubleSpinBox()
        self.spin_scale.setRange(0.01, 100.0)
        self.spin_scale.setSingleStep(0.1)
        self.spin_scale.setValue(1.0)
        self.spin_scale.valueChanged.connect(self.apply_transforms)
        trans_layout.addWidget(self.spin_scale, 0, 1)

        # Rotate 90
        self.btn_rotate = QPushButton("Xoay 90°")
        self.btn_rotate.clicked.connect(self.rotate_90)
        trans_layout.addWidget(self.btn_rotate, 0, 2)

        # Offset X
        trans_layout.addWidget(QLabel("Dịch X (mm):"), 1, 0)
        self.spin_offset_x = QDoubleSpinBox()
        self.spin_offset_x.setRange(-150.0, 150.0)
        self.spin_offset_x.setValue(0.0)
        self.spin_offset_x.valueChanged.connect(self.apply_transforms)
        trans_layout.addWidget(self.spin_offset_x, 1, 1)

        # Offset Y
        trans_layout.addWidget(QLabel("Dịch Y (mm):"), 1, 2)
        self.spin_offset_y = QDoubleSpinBox()
        self.spin_offset_y.setRange(-150.0, 150.0)
        self.spin_offset_y.setValue(0.0)
        self.spin_offset_y.valueChanged.connect(self.apply_transforms)
        trans_layout.addWidget(self.spin_offset_y, 1, 3)

        # Center alignment
        self.btn_center = QPushButton("🎯 CĂN GIỮA VÙNG QUÉT (CENTER TO 0,0)")
        self.btn_center.clicked.connect(self.center_vectors)
        trans_layout.addWidget(self.btn_center, 2, 0, 1, 4)

        layout.addWidget(trans_group)

        # 4. Laser Parameters (Speed, Delays, Passes)
        params_group = QGroupBox("4. THÔNG SỐ KHẮC (MARK PARAMETERS)")
        params_layout = QGridLayout(params_group)

        # Mark Speed
        params_layout.addWidget(QLabel("Tốc độ khắc (Mark Speed mm/s):"), 0, 0)
        self.spin_mark_speed = QDoubleSpinBox()
        self.spin_mark_speed.setRange(100.0, 6000.0)
        self.spin_mark_speed.setSingleStep(100.0)
        self.spin_mark_speed.setValue(2000.0)
        params_layout.addWidget(self.spin_mark_speed, 0, 1)

        # Jump Speed
        params_layout.addWidget(QLabel("Tốc độ nhảy (Jump Speed mm/s):"), 0, 2)
        self.spin_jump_speed = QDoubleSpinBox()
        self.spin_jump_speed.setRange(500.0, 8000.0)
        self.spin_jump_speed.setSingleStep(500.0)
        self.spin_jump_speed.setValue(6000.0)
        params_layout.addWidget(self.spin_jump_speed, 0, 3)

        # Passes
        params_layout.addWidget(QLabel("Số lượt khắc (Passes):"), 1, 0)
        self.spin_passes = QSpinBox()
        self.spin_passes.setRange(1, 100)
        self.spin_passes.setValue(1)
        params_layout.addWidget(self.spin_passes, 1, 1)

        # Export SVG
        self.btn_export_svg = QPushButton("Xuất File SVG Mô Phỏng")
        self.btn_export_svg.clicked.connect(self.export_svg)
        params_layout.addWidget(self.btn_export_svg, 1, 2, 1, 2)

        layout.addWidget(params_group)
        layout.addStretch()

        # Load default example
        self.generate_all_example2_shapes()

    # --- Shape Generators ---
    def generate_border(self):
        w, h = 100.0, 100.0
        hw, hh = w / 2.0, h / 2.0
        strokes = [[
            {"type": "JUMP", "x": -hw, "y": -hh},
            {"type": "LINE", "x": hw, "y": -hh},
            {"type": "LINE", "x": hw, "y": hh},
            {"type": "LINE", "x": -hw, "y": hh},
            {"type": "LINE", "x": -hw, "y": -hh}
        ]]
        self._update_artwork(strokes, "Khung viền 100x100 mm")

    def generate_concentric_circles(self):
        import math
        strokes = []
        for r in [10.0, 20.0, 30.0, 40.0]:
            pts = [{"type": "JUMP", "x": r, "y": 0.0}]
            num_segs = 36
            for i in range(1, num_segs + 1):
                ang = (i / num_segs) * 2 * math.pi
                pts.append({"type": "LINE", "x": r * math.cos(ang), "y": r * math.sin(ang)})
            strokes.append(pts)
        self._update_artwork(strokes, "Vòng tròn đồng tâm (10..40 mm)")

    def generate_crosshair(self):
        arm = 20.0
        strokes = [
            [{"type": "JUMP", "x": -arm, "y": 0.0}, {"type": "LINE", "x": arm, "y": 0.0}],
            [{"type": "JUMP", "x": 0.0, "y": -arm}, {"type": "LINE", "x": 0.0, "y": arm}]
        ]
        self._update_artwork(strokes, "Tâm căn chỉnh Crosshair")

    def generate_hatch_box(self):
        x_min, x_max = -20.0, 20.0
        y_min, y_max = -20.0, 20.0
        pitch = 1.0
        strokes = []
        y = y_min
        while y <= y_max:
            strokes.append([
                {"type": "JUMP", "x": x_min, "y": y},
                {"type": "LINE", "x": x_max, "y": y}
            ])
            y += pitch
        self._update_artwork(strokes, "Ô đan lưới (Hatch 40x40 mm)")

    def generate_all_example2_shapes(self):
        """Combines border, concentric circles, crosshair, and hatched box as in Example 02."""
        import math
        strokes = []

        # 1. Outer border 100x100
        hw, hh = 50.0, 50.0
        strokes.append([
            {"type": "JUMP", "x": -hw, "y": -hh},
            {"type": "LINE", "x": hw, "y": -hh},
            {"type": "LINE", "x": hw, "y": hh},
            {"type": "LINE", "x": -hw, "y": hh},
            {"type": "LINE", "x": -hw, "y": -hh}
        ])

        # 2. Concentric circles
        for r in [10.0, 20.0, 30.0, 40.0]:
            pts = [{"type": "JUMP", "x": r, "y": 0.0}]
            for i in range(1, 37):
                ang = (i / 36.0) * 2 * math.pi
                pts.append({"type": "LINE", "x": r * math.cos(ang), "y": r * math.sin(ang)})
            strokes.append(pts)

        # 3. Center crosshair
        strokes.append([{"type": "JUMP", "x": -15.0, "y": 0.0}, {"type": "LINE", "x": 15.0, "y": 0.0}])
        strokes.append([{"type": "JUMP", "x": 0.0, "y": -15.0}, {"type": "LINE", "x": 0.0, "y": 15.0}])

        # 4. Dense hatch box in upper quadrant (20..40, 20..40)
        y = 20.0
        while y <= 40.0:
            strokes.append([
                {"type": "JUMP", "x": 20.0, "y": y},
                {"type": "LINE", "x": 40.0, "y": y}
            ])
            y += 1.0

        self._update_artwork(strokes, "Example 02: 2D Vector Marking Master")

    # --- File Loading & Transformation ---
    def open_file_dialog(self):
        path, _ = QFileDialog.getOpenFileName(
            self, "Mở file bản vẽ Laser", "", "CAD Files (*.dxf *.plt *.hpgl *.hpg);;Tất cả file (*.*)"
        )
        if path:
            self.load_file(path)

    def load_sample_dxf(self):
        project_root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", ".."))
        sample_path = os.path.join(project_root, "examples", "sample_ezcad.dxf")
        if os.path.exists(sample_path):
            self.load_file(sample_path)
        else:
            QMessageBox.warning(self, "Không tìm thấy file", f"Không tìm thấy {sample_path}")

    def load_file(self, filepath: str):
        try:
            with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
                content = f.read()

            ext = os.path.splitext(filepath)[1].lower()
            if ext in (".plt", ".hpgl", ".hpg"):
                parsed = parse_plt_content(content)
            else:
                parsed = parse_dxf_content(content)

            filename = os.path.basename(filepath)
            self._update_artwork(parsed["strokes"], filename)
            self.log_message.emit(f"Loaded CAD file: {filename} ({len(parsed['strokes'])} strokes)")
        except Exception as e:
            QMessageBox.critical(self, "Lỗi đọc file", f"Không thể đọc file {filepath}:\n{e}")

    def _update_artwork(self, strokes: List[List[Dict[str, Any]]], name: str):
        self.current_strokes = strokes
        self.active_filename = name
        self.spin_scale.blockSignals(True)
        self.spin_offset_x.blockSignals(True)
        self.spin_offset_y.blockSignals(True)
        self.spin_scale.setValue(1.0)
        self.spin_offset_x.setValue(0.0)
        self.spin_offset_y.setValue(0.0)
        self.spin_scale.blockSignals(False)
        self.spin_offset_x.blockSignals(False)
        self.spin_offset_y.blockSignals(False)

        self._recompute_and_emit()

    def _recompute_and_emit(self):
        if not self.current_strokes:
            self.current_bbox = None
            self.vectors_changed.emit([], None)
            self.lbl_file_info.setText("Tệp hiện tại: Chưa có vector")
            return

        scale = self.spin_scale.value()
        ox = self.spin_offset_x.value()
        oy = self.spin_offset_y.value()

        min_x, max_x = 1e9, -1e9
        min_y, max_y = 1e9, -1e9
        transformed_strokes = []

        for stroke in self.current_strokes:
            t_stroke = []
            for pt in stroke:
                tx = pt.get("x", 0.0) * scale + ox
                ty = pt.get("y", 0.0) * scale + oy
                min_x = min(min_x, tx)
                max_x = max(max_x, tx)
                min_y = min(min_y, ty)
                max_y = max(max_y, ty)
                new_pt = dict(pt)
                new_pt["x"] = tx
                new_pt["y"] = ty
                t_stroke.append(new_pt)
            transformed_strokes.append(t_stroke)

        w = max(0.0, max_x - min_x)
        h = max(0.0, max_y - min_y)
        self.current_bbox = {"min_x": min_x, "max_x": max_x, "min_y": min_y, "max_y": max_y}

        self.lbl_file_info.setText(
            f"Tệp: {self.active_filename} | Kích thước: {w:.1f} x {h:.1f} mm | Số nét: {len(self.current_strokes)}"
        )
        self.vectors_changed.emit(transformed_strokes, self.current_bbox)

    def apply_transforms(self):
        self._recompute_and_emit()

    def center_vectors(self):
        if not self.current_strokes or not self.current_bbox:
            return
        cx = (self.current_bbox["min_x"] + self.current_bbox["max_x"]) / 2.0
        cy = (self.current_bbox["min_y"] + self.current_bbox["max_y"]) / 2.0

        for stroke in self.current_strokes:
            for pt in stroke:
                pt["x"] -= cx
                pt["y"] -= cy

        self.spin_offset_x.setValue(0.0)
        self.spin_offset_y.setValue(0.0)
        self._recompute_and_emit()

    def rotate_90(self):
        if not self.current_strokes:
            return
        import math
        rad = math.pi / 2.0
        cos_a = math.cos(rad)
        sin_a = math.sin(rad)

        for stroke in self.current_strokes:
            for pt in stroke:
                x, y = pt["x"], pt["y"]
                pt["x"] = x * cos_a - y * sin_a
                pt["y"] = x * sin_a + y * cos_a

        self._recompute_and_emit()

    def export_svg(self):
        path, _ = QFileDialog.getSaveFileName(self, "Lưu file SVG", "preview_output.svg", "SVG Images (*.svg)")
        if path:
            try:
                self.controller.export_simulation_svg(path)
                QMessageBox.information(self, "Thành Công", f"Đã xuất SVG mô phỏng tại:\n{path}")
            except Exception as e:
                QMessageBox.critical(self, "Lỗi Xuất File", f"Lỗi:\n{e}")

    def get_job_parameters(self) -> Dict[str, Any]:
        """Returns parameters needed by MarkWorker."""
        return {
            "strokes": self.current_strokes,
            "bbox": self.current_bbox,
            "mark_speed": self.spin_mark_speed.value(),
            "jump_speed": self.spin_jump_speed.value(),
            "passes": self.spin_passes.value(),
            "scale": self.spin_scale.value(),
            "offset_x": self.spin_offset_x.value(),
            "offset_y": self.spin_offset_y.value()
        }
