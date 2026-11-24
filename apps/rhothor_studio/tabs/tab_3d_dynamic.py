"""
Tab 03: 3D Dynamic Focusing (Based on Example 03: ELEVAthor EL-S2).
Demonstrates true 3D vector marking across multiple stepped focal planes
and continuous 3D sloped ramp marking without mechanical Z-table repositioning.
"""

import math
from typing import List, Dict, Any
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QPushButton, QGroupBox, QDoubleSpinBox, QMessageBox
)
from PyQt5.QtCore import Qt, pyqtSignal

from sdk.python.rhothor import RhothorController


class Dynamic3DTab(QWidget):
    """3D dynamic focus vector generator and control."""
    vectors_changed = pyqtSignal(list, object)
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller
        self._init_ui()

    def _init_ui(self):
        layout = QVBoxLayout(self)
        layout.setSpacing(14)

        # Overview banner
        info_banner = QLabel(
            "⚡ HỆ THỐNG TIÊU CỰ ĐỘNG ELEVAthor EL-S2:\n"
            "Vùng điều chỉnh Z: 147 mm (-73.5 mm đến +73.5 mm) | Tốc độ quét Z: 4000 mm/s\n"
            "Hỗ trợ khắc trên bề mặt bậc thang, dốc nghiêng và nón 3D theo thời gian thực."
        )
        info_banner.setStyleSheet("background-color: #1e293b; color: #38bdf8; padding: 10px; border-radius: 6px; font-weight: 500;")
        layout.addWidget(info_banner)

        # 1. Stepped Multi-Level Planes
        stepped_group = QGroupBox("1. BỀ MẶT BẬC THANG (MULTI-LEVEL STEPPED PLANES)")
        stepped_layout = QGridLayout(stepped_group)

        stepped_layout.addWidget(QLabel("Mặt cắt 1:"), 0, 0)
        self.spin_z1 = QDoubleSpinBox()
        self.spin_z1.setRange(-50.0, 50.0)
        self.spin_z1.setValue(-10.0)
        self.spin_z1.setPrefix("Z = ")
        self.spin_z1.setSuffix(" mm")
        stepped_layout.addWidget(self.spin_z1, 0, 1)

        stepped_layout.addWidget(QLabel("Kích thước ô 1:"), 0, 2)
        self.spin_size1 = QDoubleSpinBox()
        self.spin_size1.setRange(5.0, 100.0)
        self.spin_size1.setValue(30.0)
        self.spin_size1.setSuffix(" mm")
        stepped_layout.addWidget(self.spin_size1, 0, 3)

        stepped_layout.addWidget(QLabel("Mặt cắt 2:"), 1, 0)
        self.spin_z2 = QDoubleSpinBox()
        self.spin_z2.setRange(-50.0, 50.0)
        self.spin_z2.setValue(0.0)
        self.spin_z2.setPrefix("Z = ")
        self.spin_z2.setSuffix(" mm")
        stepped_layout.addWidget(self.spin_z2, 1, 1)

        stepped_layout.addWidget(QLabel("Kích thước ô 2:"), 1, 2)
        self.spin_size2 = QDoubleSpinBox()
        self.spin_size2.setRange(5.0, 100.0)
        self.spin_size2.setValue(40.0)
        self.spin_size2.setSuffix(" mm")
        stepped_layout.addWidget(self.spin_size2, 1, 3)

        stepped_layout.addWidget(QLabel("Mặt cắt 3:"), 2, 0)
        self.spin_z3 = QDoubleSpinBox()
        self.spin_z3.setRange(-50.0, 50.0)
        self.spin_z3.setValue(10.0)
        self.spin_z3.setPrefix("Z = ")
        self.spin_z3.setSuffix(" mm")
        stepped_layout.addWidget(self.spin_z3, 2, 1)

        stepped_layout.addWidget(QLabel("Kích thước ô 3:"), 2, 2)
        self.spin_size3 = QDoubleSpinBox()
        self.spin_size3.setRange(5.0, 100.0)
        self.spin_size3.setValue(50.0)
        self.spin_size3.setSuffix(" mm")
        stepped_layout.addWidget(self.spin_size3, 2, 3)

        self.btn_gen_stepped = QPushButton("⚡ TẠO TOOLPATH BẬC THANG 3 MẶT PHẲNG")
        self.btn_gen_stepped.setStyleSheet("background-color: #0369a1; color: white; font-weight: bold; padding: 8px;")
        self.btn_gen_stepped.clicked.connect(self.generate_stepped_planes)
        stepped_layout.addWidget(self.btn_gen_stepped, 3, 0, 1, 4)

        layout.addWidget(stepped_group)

        # 2. Sloped 3D Ramp Line
        ramp_group = QGroupBox("2. ĐƯỜNG DỐC NGHIÊNG LIÊN TỤC 3D (CONTINUOUS SLOPED RAMP)")
        ramp_layout = QGridLayout(ramp_group)

        ramp_layout.addWidget(QLabel("Điểm đầu (X1, Y1, Z1):"), 0, 0)
        self.spin_x1 = QDoubleSpinBox()
        self.spin_x1.setRange(-100, 100)
        self.spin_x1.setValue(-30.0)
        ramp_layout.addWidget(self.spin_x1, 0, 1)

        self.spin_y1 = QDoubleSpinBox()
        self.spin_y1.setRange(-100, 100)
        self.spin_y1.setValue(-30.0)
        ramp_layout.addWidget(self.spin_y1, 0, 2)

        self.spin_ramp_z1 = QDoubleSpinBox()
        self.spin_ramp_z1.setRange(-70, 70)
        self.spin_ramp_z1.setValue(-15.0)
        ramp_layout.addWidget(self.spin_ramp_z1, 0, 3)

        ramp_layout.addWidget(QLabel("Điểm cuối (X2, Y2, Z2):"), 1, 0)
        self.spin_x2 = QDoubleSpinBox()
        self.spin_x2.setRange(-100, 100)
        self.spin_x2.setValue(30.0)
        ramp_layout.addWidget(self.spin_x2, 1, 1)

        self.spin_y2 = QDoubleSpinBox()
        self.spin_y2.setRange(-100, 100)
        self.spin_y2.setValue(30.0)
        ramp_layout.addWidget(self.spin_y2, 1, 2)

        self.spin_ramp_z2 = QDoubleSpinBox()
        self.spin_ramp_z2.setRange(-70, 70)
        self.spin_ramp_z2.setValue(15.0)
        ramp_layout.addWidget(self.spin_ramp_z2, 1, 3)

        self.btn_gen_ramp = QPushButton("⚡ TẠO ĐƯỜNG DỐC 3D LIÊN TỤC (RAMP)")
        self.btn_gen_ramp.setStyleSheet("background-color: #0369a1; color: white; font-weight: bold; padding: 8px;")
        self.btn_gen_ramp.clicked.connect(self.generate_ramp)
        ramp_layout.addWidget(self.btn_gen_ramp, 2, 0, 1, 4)

        layout.addWidget(ramp_group)

        # 3. 3D Spiral Cone
        spiral_group = QGroupBox("3. KHẮC NÓN XOẮN ỐC 3D (3D SPIRAL CONE)")
        spiral_layout = QHBoxLayout(spiral_group)

        self.btn_gen_spiral = QPushButton("🌀 TẠO HÌNH NÓN XOẮN ỐC 3D (R: 5..35mm, Z: -20..+20mm)")
        self.btn_gen_spiral.setStyleSheet("background-color: #475569; color: white; font-weight: bold; padding: 8px;")
        self.btn_gen_spiral.clicked.connect(self.generate_spiral_cone)
        spiral_layout.addWidget(self.btn_gen_spiral)

        layout.addWidget(spiral_group)
        layout.addStretch()

    def generate_stepped_planes(self):
        """Generates 3 squares on 3 different Z focal planes as in Example 03."""
        planes = [
            (self.spin_z1.value(), self.spin_size1.value()),
            (self.spin_z2.value(), self.spin_size2.value()),
            (self.spin_z3.value(), self.spin_size3.value())
        ]
        strokes = []
        for z, size in planes:
            hs = size / 2.0
            strokes.append([
                {"type": "JUMP", "x": -hs, "y": -hs, "z": z},
                {"type": "LINE", "x": hs, "y": -hs, "z": z},
                {"type": "LINE", "x": hs, "y": hs, "z": z},
                {"type": "LINE", "x": -hs, "y": hs, "z": z},
                {"type": "LINE", "x": -hs, "y": -hs, "z": z}
            ])

        bbox = {"min_x": -30.0, "max_x": 30.0, "min_y": -30.0, "max_y": 30.0}
        self.vectors_changed.emit(strokes, bbox)
        self.log_message.emit(f"Generated 3D stepped planes: Z={self.spin_z1.value()}, {self.spin_z2.value()}, {self.spin_z3.value()} mm")

    def generate_ramp(self):
        """Generates a continuous sloped line in 3D."""
        x1, y1, z1 = self.spin_x1.value(), self.spin_y1.value(), self.spin_ramp_z1.value()
        x2, y2, z2 = self.spin_x2.value(), self.spin_y2.value(), self.spin_ramp_z2.value()

        strokes = [[
            {"type": "JUMP", "x": x1, "y": y1, "z": z1},
            {"type": "LINE", "x": x2, "y": y2, "z": z2}
        ]]
        bbox = {
            "min_x": min(x1, x2), "max_x": max(x1, x2),
            "min_y": min(y1, y2), "max_y": max(y1, y2)
        }
        self.vectors_changed.emit(strokes, bbox)
        self.log_message.emit(f"Generated continuous 3D ramp from ({x1},{y1},{z1}) to ({x2},{y2},{z2}) mm")

    def generate_spiral_cone(self):
        """Generates a smooth 3D spiral on a conical surface."""
        strokes = []
        pts = []
        turns = 6
        num_pts = 180
        for i in range(num_pts + 1):
            t = i / num_pts
            ang = t * turns * 2 * math.pi
            r = 5.0 + t * 30.0
            z = -20.0 + t * 40.0
            px = r * math.cos(ang)
            py = r * math.sin(ang)
            pt_type = "JUMP" if i == 0 else "LINE"
            pts.append({"type": pt_type, "x": px, "y": py, "z": z})

        strokes.append(pts)
        bbox = {"min_x": -35.0, "max_x": 35.0, "min_y": -35.0, "max_y": 35.0}
        self.vectors_changed.emit(strokes, bbox)
        self.log_message.emit("Generated 3D conical spiral toolpath.")
