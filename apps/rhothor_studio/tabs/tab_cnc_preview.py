"""
CNC & Laser CAD Drawing Preview & CAM Workstation Tab.
Supports loading and visualizing:
  - Vector CAD: .DXF, .SVG, .AI, .PLT / .HPGL
  - Project Files: .LBRN / .LBRN2 (LightBurn), .EZD / .EZ3 (EZCAD)
  - Raster / Bitmap: .BMP, .JPG, .JPEG, .PNG (Photo engraving & shading)
"""

import os
import math
from typing import List, Dict, Any, Optional

from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QGroupBox,
    QLabel, QPushButton, QFileDialog, QMessageBox, QDoubleSpinBox,
    QSpinBox, QComboBox, QSlider, QTableWidget, QTableWidgetItem,
    QHeaderView, QFrame, QScrollArea
)
from PyQt5.QtCore import Qt, pyqtSignal
from PyQt5.QtGui import QColor, QFont

from sdk.python.rhothor import RhothorController
from sdk.python.cad_parsers import load_any_cad_drawing, convert_raster_image_to_laser_toolpath


class CNCPreviewTab(QWidget):
    """
    Advanced CNC & Laser Drawing Importer, Layer Inspector & Preview Workstation.
    """
    vectors_changed = pyqtSignal(list, dict)  # (strokes, bbox)
    frame_changed = pyqtSignal(dict)          # (bbox)
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller

        self.current_drawing_data: Optional[Dict[str, Any]] = None
        self.base_strokes: List[List[Dict[str, Any]]] = []
        self.transformed_strokes: List[List[Dict[str, Any]]] = []
        self.current_bbox: Dict[str, float] = {"min_x": 0.0, "max_x": 0.0, "min_y": 0.0, "max_y": 0.0}

        self.current_scale = 1.0
        self.current_rotation = 0.0
        self.current_offset_x = 0.0
        self.current_offset_y = 0.0
        self.invert_x = False
        self.invert_y = False

        self._init_ui()

    def _init_ui(self):
        main_layout = QVBoxLayout(self)
        main_layout.setContentsMargins(10, 8, 10, 8)
        main_layout.setSpacing(8)

        # Scrollable area for controls
        scroll = QScrollArea()
        scroll.setWidgetResizable(True)
        scroll.setFrameShape(QFrame.NoFrame)
        content_widget = QWidget()
        layout = QVBoxLayout(content_widget)
        layout.setContentsMargins(0, 0, 0, 0)
        layout.setSpacing(8)

        # =========================================================================
        # 1. File Loader & Sample Presets
        # =========================================================================
        file_group = QGroupBox("1. NHẬP BẢN VẼ CNC & THIẾT KẾ LASER")
        file_layout = QVBoxLayout(file_group)

        row_open = QHBoxLayout()
        self.btn_open = QPushButton("📂 MỞ BẢN VẼ (DXF, SVG, AI, PLT, EZCAD, LIGHTBURN, ẢNH)...")
        self.btn_open.setStyleSheet("""
            QPushButton {
                background-color: #0284c7;
                color: white;
                font-weight: 800;
                font-size: 12px;
                padding: 10px;
                border-radius: 5px;
            }
            QPushButton:hover { background-color: #0369a1; }
        """)
        self.btn_open.clicked.connect(self.open_file_dialog)
        row_open.addWidget(self.btn_open, 3)

        self.lbl_format_badge = QLabel("Chưa mở file")
        self.lbl_format_badge.setStyleSheet("""
            QLabel {
                background-color: #1e293b;
                color: #94a3b8;
                padding: 4px 10px;
                border-radius: 4px;
                font-weight: bold;
                font-size: 11px;
            }
        """)
        row_open.addWidget(self.lbl_format_badge, 1)
        file_layout.addLayout(row_open)

        # Fast sample loading buttons
        sample_row = QHBoxLayout()
        self.btn_sample_dxf = QPushButton("📄 Mẫu DXF EZCAD")
        self.btn_sample_dxf.clicked.connect(self.load_sample_dxf)
        sample_row.addWidget(self.btn_sample_dxf)

        self.btn_sample_svg = QPushButton("🎨 Mẫu SVG Vector")
        self.btn_sample_svg.clicked.connect(self.load_sample_svg)
        sample_row.addWidget(self.btn_sample_svg)

        self.btn_sample_lbrn = QPushButton("🔥 Mẫu LightBurn")
        self.btn_sample_lbrn.clicked.connect(self.load_sample_lightburn)
        sample_row.addWidget(self.btn_sample_lbrn)

        self.btn_sample_raster = QPushButton("🖼️ Mẫu Ảnh Khắc")
        self.btn_sample_raster.clicked.connect(self.load_sample_raster)
        sample_row.addWidget(self.btn_sample_raster)
        file_layout.addLayout(sample_row)

        layout.addWidget(file_group)

        # =========================================================================
        # 2. Drawing Inspector & Specifications
        # =========================================================================
        info_group = QGroupBox("2. THÔNG SỐ BẢN VẼ & KÍCH THƯỚC VÙNG KHẮC (INSPECTION)")
        info_layout = QGridLayout(info_group)
        info_layout.setSpacing(6)

        self.lbl_dims = QLabel("Kích thước: 0.00 x 0.00 mm")
        self.lbl_dims.setStyleSheet("color: #38bdf8; font-weight: bold;")
        info_layout.addWidget(self.lbl_dims, 0, 0)

        self.lbl_entities = QLabel("Thực thể: 0 nét | 0 đường cắt")
        self.lbl_entities.setStyleSheet("color: #cbd5e1;")
        info_layout.addWidget(self.lbl_entities, 0, 1)

        self.lbl_distance = QLabel("Tổng quãng đường: 0.00 mm")
        self.lbl_distance.setStyleSheet("color: #4ade80; font-weight: bold;")
        info_layout.addWidget(self.lbl_distance, 1, 0)

        self.lbl_time_est = QLabel("Thời gian ước tính: 0.00 s")
        self.lbl_time_est.setStyleSheet("color: #cbd5e1;")
        info_layout.addWidget(self.lbl_time_est, 1, 1)

        # Layers / Pens Table
        self.table_layers = QTableWidget(0, 3)
        self.table_layers.setHorizontalHeaderLabels(["Layer / Pen", "Số Thực Thể", "Trạng Thái"])
        self.table_layers.horizontalHeader().setSectionResizeMode(0, QHeaderView.Stretch)
        self.table_layers.horizontalHeader().setSectionResizeMode(1, QHeaderView.ResizeToContents)
        self.table_layers.horizontalHeader().setSectionResizeMode(2, QHeaderView.ResizeToContents)
        self.table_layers.setMaximumHeight(85)
        info_layout.addWidget(self.table_layers, 2, 0, 1, 2)

        layout.addWidget(info_group)

        # =========================================================================
        # 3. Geometric Alignment & Workfield Fitting
        # =========================================================================
        geo_group = QGroupBox("3. CĂN CHỈNH TỌA ĐỘ & VÙNG QUÉT (ALIGNMENT & FITTING)")
        geo_layout = QGridLayout(geo_group)
        geo_layout.setSpacing(6)

        # Auto-fit & Center buttons
        btn_fit_box = QHBoxLayout()
        self.btn_autofit = QPushButton("🎯 Tự Động Căn Vừa Vùng Quét (Auto-Fit)")
        self.btn_autofit.setStyleSheet("background-color: #334155; color: #38bdf8; font-weight: bold; padding: 6px;")
        self.btn_autofit.clicked.connect(self.auto_fit_to_field)
        btn_fit_box.addWidget(self.btn_autofit)

        self.btn_center = QPushButton("🎯 Về Tâm Gốc (0, 0)")
        self.btn_center.setStyleSheet("background-color: #334155; color: #f8fafc; font-weight: bold; padding: 6px;")
        self.btn_center.clicked.connect(self.center_to_origin)
        btn_fit_box.addWidget(self.btn_center)
        geo_layout.addLayout(btn_fit_box, 0, 0, 1, 4)

        # Scale
        geo_layout.addWidget(QLabel("Tỉ lệ (Scale):"), 1, 0)
        self.spin_scale = QDoubleSpinBox()
        self.spin_scale.setRange(0.01, 100.0)
        self.spin_scale.setSingleStep(0.1)
        self.spin_scale.setValue(1.0)
        self.spin_scale.valueChanged.connect(self._on_transform_changed)
        geo_layout.addWidget(self.spin_scale, 1, 1)

        # Rotation
        geo_layout.addWidget(QLabel("Xoay góc (Rotate):"), 1, 2)
        self.combo_rotate = QComboBox()
        self.combo_rotate.addItems(["0°", "90°", "180°", "270°"])
        self.combo_rotate.currentIndexChanged.connect(self._on_transform_changed)
        geo_layout.addWidget(self.combo_rotate, 1, 3)

        # Offsets
        geo_layout.addWidget(QLabel("Dịch X (mm):"), 2, 0)
        self.spin_offset_x = QDoubleSpinBox()
        self.spin_offset_x.setRange(-150.0, 150.0)
        self.spin_offset_x.setSingleStep(1.0)
        self.spin_offset_x.setValue(0.0)
        self.spin_offset_x.valueChanged.connect(self._on_transform_changed)
        geo_layout.addWidget(self.spin_offset_x, 2, 1)

        geo_layout.addWidget(QLabel("Dịch Y (mm):"), 2, 2)
        self.spin_offset_y = QDoubleSpinBox()
        self.spin_offset_y.setRange(-150.0, 150.0)
        self.spin_offset_y.setSingleStep(1.0)
        self.spin_offset_y.setValue(0.0)
        self.spin_offset_y.valueChanged.connect(self._on_transform_changed)
        geo_layout.addWidget(self.spin_offset_y, 2, 3)

        # Mirrors
        mirror_box = QHBoxLayout()
        self.btn_mirror_x = QPushButton("↔️ Lật Đối Xứng X")
        self.btn_mirror_x.setCheckable(True)
        self.btn_mirror_x.clicked.connect(self._on_transform_changed)
        mirror_box.addWidget(self.btn_mirror_x)

        self.btn_mirror_y = QPushButton("↕️ Lật Đối Xứng Y")
        self.btn_mirror_y.setCheckable(True)
        self.btn_mirror_y.clicked.connect(self._on_transform_changed)
        mirror_box.addWidget(self.btn_mirror_y)
        geo_layout.addLayout(mirror_box, 3, 0, 1, 4)

        layout.addWidget(geo_group)

        # =========================================================================
        # 4. Raster Shading & Photo Engraving Controls (Contextual)
        # =========================================================================
        self.raster_group = QGroupBox("4. THÔNG SỐ KHẮC ẢNH BITMAP (PHOTO SHADING & ENGRAVE)")
        raster_layout = QGridLayout(self.raster_group)
        raster_layout.setSpacing(6)

        raster_layout.addWidget(QLabel("Chế độ khắc ảnh:"), 0, 0)
        self.combo_raster_mode = QComboBox()
        self.combo_raster_mode.addItems([
            "Quét ngang hai chiều (Hatch Scan)",
            "Tán sắc ảnh nghệ thuật (Floyd-Steinberg Dither)",
            "Dò viền đối tượng (Contour Edge)"
        ])
        raster_layout.addWidget(self.combo_raster_mode, 0, 1)

        raster_layout.addWidget(QLabel("Khoảng cách dòng quét:"), 0, 2)
        self.spin_interval = QDoubleSpinBox()
        self.spin_interval.setRange(0.02, 1.0)
        self.spin_interval.setSingleStep(0.02)
        self.spin_interval.setValue(0.1)
        self.spin_interval.setSuffix(" mm")
        raster_layout.addWidget(self.spin_interval, 0, 3)

        raster_layout.addWidget(QLabel("Kích thước ảnh X:"), 1, 0)
        self.spin_img_w = QDoubleSpinBox()
        self.spin_img_w.setRange(5.0, 200.0)
        self.spin_img_w.setValue(50.0)
        self.spin_img_w.setSuffix(" mm")
        raster_layout.addWidget(self.spin_img_w, 1, 1)

        raster_layout.addWidget(QLabel("Kích thước ảnh Y:"), 1, 2)
        self.spin_img_h = QDoubleSpinBox()
        self.spin_img_h.setRange(5.0, 200.0)
        self.spin_img_h.setValue(50.0)
        self.spin_img_h.setSuffix(" mm")
        raster_layout.addWidget(self.spin_img_h, 1, 3)

        self.btn_rebuild_raster = QPushButton("🔄 Tái Tạo Lại Toolpath Ảnh")
        self.btn_rebuild_raster.setStyleSheet("background-color: #3b4252; color: #38bdf8; font-weight: bold; padding: 6px;")
        self.btn_rebuild_raster.clicked.connect(self.rebuild_raster_toolpath)
        raster_layout.addWidget(self.btn_rebuild_raster, 2, 0, 1, 4)

        self.raster_group.setVisible(False)  # Hidden unless bitmap image is loaded
        layout.addWidget(self.raster_group)

        # =========================================================================
        # 5. Quick Preview & Framing Actions
        # =========================================================================
        action_group = QGroupBox("5. ĐIỀU KHIỂN XEM TRƯỚC & QUÉT KHUNG")
        action_layout = QHBoxLayout(action_group)

        self.btn_frame_preview = QPushButton("⛶ Quét Khung Phôi (Laser OFF)")
        self.btn_frame_preview.setStyleSheet("""
            QPushButton {
                background-color: #0d9488;
                color: white;
                font-weight: bold;
                padding: 9px;
                border-radius: 5px;
            }
            QPushButton:hover { background-color: #0f766e; }
        """)
        self.btn_frame_preview.clicked.connect(self.execute_framing)
        action_layout.addWidget(self.btn_frame_preview)

        self.btn_export_std_svg = QPushButton("💾 Xuất File Ra SVG Chuẩn")
        self.btn_export_std_svg.clicked.connect(self.export_current_to_svg)
        action_layout.addWidget(self.btn_export_std_svg)

        layout.addWidget(action_group)
        layout.addStretch()

        scroll.setWidget(content_widget)
        main_layout.addWidget(scroll)

    # -------------------------------------------------------------------------
    # File Loading Operations
    # -------------------------------------------------------------------------

    def open_file_dialog(self):
        """Opens file dialog with all supported CNC vector, project, and raster formats."""
        filter_str = (
            "Tất cả định dạng hỗ trợ (*.dxf *.svg *.ai *.plt *.hpgl *.lbrn *.lbrn2 *.ezd *.ez3 *.png *.jpg *.jpeg *.bmp);;"
            "File Vector CAD (*.dxf *.svg *.ai *.plt *.hpgl);;"
            "File Dự Án Laser (*.lbrn *.lbrn2 *.ezd *.ez3);;"
            "Hình Ảnh Bitmap / Raster (*.png *.jpg *.jpeg *.bmp);;"
            "Tất cả file (*.*)"
        )
        filepath, _ = QFileDialog.getOpenFileName(self, "Chọn File Bản Vẽ CNC / Laser", "", filter_str)
        if filepath:
            self.load_file(filepath)

    def load_file(self, filepath: str):
        """Loads and processes any CAD/CAM drawing or image."""
        try:
            ext = os.path.splitext(filepath)[1].lower()
            is_raster = ext in (".png", ".jpg", ".jpeg", ".bmp")
            self.raster_group.setVisible(is_raster)

            data = load_any_cad_drawing(
                filepath,
                width_mm=self.spin_img_w.value(),
                height_mm=self.spin_img_h.value(),
                interval_mm=self.spin_interval.value(),
                mode="raster" if self.combo_raster_mode.currentIndex() == 0 else "dither"
            )

            self.current_drawing_data = data
            self.base_strokes = data.get("strokes", [])
            self.lbl_format_badge.setText(f"{data.get('format', 'CAD')} ({data.get('stroke_count', 0)} nét)")
            self.lbl_format_badge.setStyleSheet("background-color: #065f46; color: #34d399; padding: 4px 10px; border-radius: 4px; font-weight: bold;")

            # Populate inspection labels
            bbox = data.get("bbox", {})
            w = bbox.get("width", 0.0)
            h = bbox.get("height", 0.0)
            self.lbl_dims.setText(f"Kích thước: {w:.2f} x {h:.2f} mm")
            self.lbl_entities.setText(f"Thực thể: {data.get('entity_count', 0)} | {len(self.base_strokes)} nét")
            dist = data.get("total_mark_distance_mm", 0.0)
            self.lbl_distance.setText(f"Tổng quãng đường: {dist:.1f} mm")
            
            # Estimated time at 1500 mm/s
            est_sec = dist / 1500.0 if dist > 0 else 0.0
            self.lbl_time_est.setText(f"Thời gian ước tính: ~{est_sec:.1f} s")

            # Populate Layers table
            layers = data.get("layers", [])
            self.table_layers.setRowCount(len(layers))
            for row, lay in enumerate(layers):
                name = lay.get("name", f"Layer {row}")
                count = lay.get("count", 0)
                item_name = QTableWidgetItem(str(name))
                item_count = QTableWidgetItem(f"{count} nét")
                item_status = QTableWidgetItem("SẴN SÀNG")
                item_status.setForeground(Qt.cyan)
                self.table_layers.setItem(row, 0, item_name)
                self.table_layers.setItem(row, 1, item_count)
                self.table_layers.setItem(row, 2, item_status)

            # Apply geometry and push to canvas
            self.spin_scale.blockSignals(True)
            self.spin_scale.setValue(1.0)
            self.spin_scale.blockSignals(False)

            self._apply_transformations()
            self.log_message.emit(f"Successfully loaded {data.get('filename')} [{data.get('format')}]. {len(self.base_strokes)} strokes.")

        except Exception as e:
            QMessageBox.critical(self, "Lỗi Tải Bản Vẽ", f"Không thể phân tích file {filepath}:\n{e}")
            self.log_message.emit(f"Error loading file {filepath}: {e}")

    def rebuild_raster_toolpath(self):
        """Re-computes raster toolpaths when user alters resolution, interval or mode."""
        if not self.current_drawing_data:
            return
        filepath = self.current_drawing_data.get("filepath", "")
        if filepath and os.path.exists(filepath):
            mode_str = "raster" if self.combo_raster_mode.currentIndex() == 0 else "dither"
            data = convert_raster_image_to_laser_toolpath(
                filepath,
                target_width_mm=self.spin_img_w.value(),
                target_height_mm=self.spin_img_h.value(),
                line_interval_mm=self.spin_interval.value(),
                mode=mode_str
            )
            self.current_drawing_data["strokes"] = data["strokes"]
            self.current_drawing_data["bbox"] = data["bbox"]
            self.base_strokes = data["strokes"]
            self._apply_transformations()
            self.log_message.emit(f"Raster toolpath re-generated: {len(self.base_strokes)} scanlines.")

    # -------------------------------------------------------------------------
    # Geometric Transformations
    # -------------------------------------------------------------------------

    def _on_transform_changed(self):
        self._apply_transformations()

    def _apply_transformations(self):
        """Applies scale, rotation, mirroring, and translation offsets."""
        if not self.base_strokes:
            return

        scale = self.spin_scale.value()
        rot_idx = self.combo_rotate.currentIndex()
        angle_rad = math.radians([0.0, 90.0, 180.0, 270.0][rot_idx])
        cos_a = math.cos(angle_rad)
        sin_a = math.sin(angle_rad)

        off_x = self.spin_offset_x.value()
        off_y = self.spin_offset_y.value()
        mx = -1.0 if self.btn_mirror_x.isChecked() else 1.0
        my = -1.0 if self.btn_mirror_y.isChecked() else 1.0

        all_pts = []
        new_strokes = []

        for stroke in self.base_strokes:
            transformed_stroke = []
            for pt in stroke:
                px = pt.get("x", 0.0) * mx
                py = pt.get("y", 0.0) * my
                pz = pt.get("z", 0.0)
                pt_type = pt.get("type", "LINE")

                # Scale
                px *= scale
                py *= scale

                # Rotate
                rx = px * cos_a - py * sin_a
                ry = px * sin_a + py * cos_a

                # Translate
                fx = rx + off_x
                fy = ry + off_y

                new_pt = {"type": pt_type, "x": round(fx, 4), "y": round(fy, 4), "z": round(pz, 4)}
                if "bf" in pt:
                    new_pt["bf"] = pt["bf"]
                transformed_stroke.append(new_pt)
                all_pts.append((fx, fy))

            if transformed_stroke:
                new_strokes.append(transformed_stroke)

        # Compute new bounding box
        if all_pts:
            xs = [p[0] for p in all_pts]
            ys = [p[1] for p in all_pts]
            min_x, max_x = min(xs), max(xs)
            min_y, max_y = min(ys), max(ys)
            bbox = {
                "min_x": round(min_x, 2), "max_x": round(max_x, 2),
                "min_y": round(min_y, 2), "max_y": round(max_y, 2),
                "width": round(max_x - min_x, 2), "height": round(max_y - min_y, 2),
                "center_x": round((min_x + max_x) / 2.0, 2), "center_y": round((min_y + max_y) / 2.0, 2)
            }
        else:
            bbox = {"min_x": 0.0, "max_x": 0.0, "min_y": 0.0, "max_y": 0.0, "width": 0.0, "height": 0.0, "center_x": 0.0, "center_y": 0.0}

        self.transformed_strokes = new_strokes
        self.current_bbox = bbox

        # Emit to interactive canvas
        self.vectors_changed.emit(self.transformed_strokes, self.current_bbox)

    def auto_fit_to_field(self):
        """Automatically scales drawing to fit within 80% of scanner working field size."""
        if not self.base_strokes:
            return
        try:
            field_sz = self.controller.get_field_size()
        except Exception:
            field_sz = 100.0  # Safe default matching LPKF field

        target_span = field_sz * 0.85
        cur_w = self.current_bbox.get("width", 1.0)
        cur_h = self.current_bbox.get("height", 1.0)
        max_dim = max(cur_w, cur_h, 0.1)

        ratio = target_span / max_dim
        new_scale = max(0.01, min(100.0, self.spin_scale.value() * ratio))
        self.spin_scale.setValue(new_scale)
        self.center_to_origin()
        self.log_message.emit(f"Auto-fit applied: Scale set to {new_scale:.2f}x to fit {target_span:.1f} mm field.")

    def center_to_origin(self):
        """Centers current drawing exactly at (0, 0)."""
        if not self.current_bbox:
            return
        cx = self.current_bbox.get("center_x", 0.0)
        cy = self.current_bbox.get("center_y", 0.0)
        self.spin_offset_x.setValue(self.spin_offset_x.value() - cx)
        self.spin_offset_y.setValue(self.spin_offset_y.value() - cy)
        self.log_message.emit("Drawing centered at Origin (0, 0).")

    # -------------------------------------------------------------------------
    # Quick Samples
    # -------------------------------------------------------------------------

    def load_sample_dxf(self):
        root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", ".."))
        p = os.path.join(root, "examples", "sample_ezcad.dxf")
        if os.path.exists(p):
            self.load_file(p)

    def load_sample_svg(self):
        root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", ".."))
        p = os.path.join(root, "demo_all.svg")
        if os.path.exists(p):
            self.load_file(p)

    def load_sample_lightburn(self):
        """Generates a demo LightBurn 2.0 JSON file and loads it."""
        import tempfile
        sample_lbrn = {
            "CutSetting": [{"Priority": 0, "Speed": 1500, "Power": 60, "NumPasses": 1}],
            "Shapes": [
                {"Type": "Rect", "X": -20, "Y": -20, "Width": 40, "Height": 40, "Priority": 0},
                {"Type": "Ellipse", "X": 0, "Y": 0, "Rx": 15, "Ry": 15, "Priority": 0},
                {"Type": "Path", "Points": [{"x": -15, "y": 0}, {"x": 15, "y": 0}], "Priority": 0},
                {"Type": "Path", "Points": [{"x": 0, "y": -15}, {"x": 0, "y": 15}], "Priority": 0}
            ]
        }
        import json
        tmp = tempfile.mktemp(suffix=".lbrn2")
        with open(tmp, "w", encoding="utf-8") as f:
            json.dump(sample_lbrn, f)
        self.load_file(tmp)
        try:
            os.remove(tmp)
        except Exception:
            pass

    def load_sample_raster(self):
        """Generates a sample photo engraving bitmap and loads it."""
        import tempfile
        from PyQt5.QtGui import QImage, QPainter
        img = QImage(120, 120, QImage.Format_RGB32)
        img.fill(QColor(255, 255, 255))
        p = QPainter(img)
        p.setPen(QColor(0, 0, 0))
        p.drawRect(10, 10, 100, 100)
        p.drawEllipse(30, 30, 60, 60)
        p.drawLine(10, 10, 110, 110)
        p.end()

        tmp = tempfile.mktemp(suffix=".png")
        img.save(tmp)
        self.load_file(tmp)
        try:
            os.remove(tmp)
        except Exception:
            pass

    # -------------------------------------------------------------------------
    # Framing & Export
    # -------------------------------------------------------------------------

    def execute_framing(self):
        """Traces the bounding box of the active drawing with Laser OFF (Eye-Safe)."""
        if not self.transformed_strokes or not self.current_bbox:
            QMessageBox.warning(self, "Chưa Có Bản Vẽ", "Vui lòng mở bản vẽ CNC trước khi quét khung.")
            return

        b = self.current_bbox
        min_x, max_x = b["min_x"], b["max_x"]
        min_y, max_y = b["min_y"], b["max_y"]

        # Signal canvas guide frame
        self.frame_changed.emit(b)

        try:
            ms = getattr(self.controller, "max_speed", 2000.0)
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=min(1500.0, ms), jump_speed=min(2000.0, ms))
            self.controller.jump_to(min_x, min_y)
            self.controller.move_to(max_x, min_y)
            self.controller.move_to(max_x, max_y)
            self.controller.move_to(min_x, max_y)
            self.controller.move_to(min_x, min_y)
            self.controller.list_close()
            self.log_message.emit(f"Framing contour traced: [{min_x:.1f}, {min_y:.1f}] to [{max_x:.1f}, {max_y:.1f}]. Laser OFF.")
        except Exception as e:
            QMessageBox.warning(self, "Lỗi Quét Khung", f"Không thể thực hiện quét khung galvo:\n{e}")

    def export_current_to_svg(self):
        """Exports currently loaded and transformed toolpaths as a clean SVG."""
        if not self.transformed_strokes:
            QMessageBox.warning(self, "Chưa Có Dữ Liệu", "Chưa có bản vẽ nào để xuất.")
            return

        path, _ = QFileDialog.getSaveFileName(self, "Xuất Bản Vẽ Ra File SVG", "laser_drawing.svg", "SVG Files (*.svg)")
        if not path:
            return

        try:
            b = self.current_bbox
            w = max(10.0, b["width"])
            h = max(10.0, b["height"])
            ox = b["min_x"]
            oy = b["min_y"]

            svg_lines = [
                f'<svg xmlns="http://www.w3.org/2000/svg" width="{w:.2f}mm" height="{h:.2f}mm" viewBox="{ox:.2f} {-b["max_y"]:.2f} {w:.2f} {h:.2f}">',
                '  <g stroke="#ff0000" stroke-width="0.1" fill="none">'
            ]

            for stroke in self.transformed_strokes:
                d_cmds = []
                for pt in stroke:
                    cmd = pt.get("type", "LINE")
                    # Invert Y back to standard SVG space
                    sx = pt.get("x", 0.0)
                    sy = -pt.get("y", 0.0)
                    if cmd == "JUMP":
                        d_cmds.append(f"M {sx:.3f} {sy:.3f}")
                    else:
                        d_cmds.append(f"L {sx:.3f} {sy:.3f}")
                if d_cmds:
                    svg_lines.append(f'    <path d="{" ".join(d_cmds)}" />')

            svg_lines.append('  </g>')
            svg_lines.append('</svg>')

            with open(path, "w", encoding="utf-8") as f:
                f.write("\n".join(svg_lines))

            QMessageBox.information(self, "Xuất Thành Công", f"Đã lưu bản vẽ tại:\n{path}")
            self.log_message.emit(f"Exported drawing to SVG: {path}")
        except Exception as e:
            QMessageBox.critical(self, "Lỗi Xuất File", f"Không thể lưu file SVG:\n{e}")
