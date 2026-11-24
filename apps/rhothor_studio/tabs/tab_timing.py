"""
Tab 04: Laser Gate Timing & Delay Tuning (Based on Example 04).
Calibrates gate_on_delay and gate_off_delay to eliminate burned start spots,
corner scorching, and cut tails.
"""

from typing import List, Dict, Any
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QPushButton, QGroupBox, QSpinBox, QMessageBox, QTextEdit
)
from PyQt5.QtCore import Qt, pyqtSignal

from sdk.python.rhothor import RhothorController


class TimingTab(QWidget):
    """Laser timing and delay calibration matrix tab."""
    vectors_changed = pyqtSignal(list, object)
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller
        self._init_ui()

    def _init_ui(self):
        layout = QVBoxLayout(self)
        layout.setSpacing(14)

        # 1. Active Timing Registers
        reg_group = QGroupBox("1. CÀI ĐẶT THỜI GIAN TRỄ TIA LASER (ACTIVE DELAY REGISTERS)")
        reg_layout = QGridLayout(reg_group)

        # Gate On Delay
        reg_layout.addWidget(QLabel("Thời gian trễ mở tia (Gate On Delay):"), 0, 0)
        self.spin_on_delay = QSpinBox()
        self.spin_on_delay.setRange(0, 1000)
        self.spin_on_delay.setValue(60)
        self.spin_on_delay.setSuffix(" µs")
        reg_layout.addWidget(self.spin_on_delay, 0, 1)

        # Gate Off Delay
        reg_layout.addWidget(QLabel("Thời gian trễ đóng tia (Gate Off Delay):"), 1, 0)
        self.spin_off_delay = QSpinBox()
        self.spin_off_delay.setRange(0, 1000)
        self.spin_off_delay.setValue(60)
        self.spin_off_delay.setSuffix(" µs")
        reg_layout.addWidget(self.spin_off_delay, 1, 1)

        # Apply button
        self.btn_apply = QPushButton("💾 LƯU THỜI GIAN TRỄ VÀO BỘ NHỚ DSP")
        self.btn_apply.setStyleSheet("background-color: #0284c7; color: white; font-weight: bold; padding: 8px;")
        self.btn_apply.clicked.connect(self.apply_delays)
        reg_layout.addWidget(self.btn_apply, 2, 0, 1, 2)

        layout.addWidget(reg_group)

        # 2. Automated Calibration Matrix
        matrix_group = QGroupBox("2. TẠO BẢNG TEST HIỆU CHUẨN ĐỘ TRỄ (EXAMPLE 04 CALIBRATION MATRIX)")
        matrix_layout = QVBoxLayout(matrix_group)

        matrix_desc = QLabel(
            "Chức năng này tạo 5 đường vạch song song thử nghiệm với các mức trễ:\n"
            "20 µs, 40 µs, 60 µs, 80 µs, 100 µs (tương ứng từ Y = -40mm đến Y = +40mm).\n"
            "Soi các điểm đầu mút vạch dưới kính hiển vi để chọn độ trễ hoàn hảo cho phôi."
        )
        matrix_desc.setStyleSheet("color: #94a3b8; font-size: 11px;")
        matrix_layout.addWidget(matrix_desc)

        self.btn_gen_matrix = QPushButton("⚡ TẠO TẬP LỆNH TEST MA TRẬN 5 MỨC TRỄ (5 LINES)")
        self.btn_gen_matrix.setStyleSheet("background-color: #059669; color: white; font-weight: bold; padding: 10px;")
        self.btn_gen_matrix.clicked.connect(self.generate_matrix)
        matrix_layout.addWidget(self.btn_gen_matrix)

        layout.addWidget(matrix_group)

        # 3. Diagnostic Guide
        guide_group = QGroupBox("3. HƯỚNG DẪN ĐỌC KẾT QUẢ KHI TEST")
        guide_layout = QVBoxLayout(guide_group)

        guide_text = QTextEdit()
        guide_text.setReadOnly(True)
        guide_text.setStyleSheet("background-color: #1e293b; color: #cbd5e1; font-size: 11px;")
        guide_text.setHtml(
            "<b>1. Điểm đầu đường nét bị cháy đốm to:</b><br>"
            "&nbsp;&nbsp;&bull; Nguyên nhân: Tia laser phát trước khi gương galvo bắt đầu chuyển động.<br>"
            "&nbsp;&nbsp;&bull; Khắc phục: <i>TĂNG Gate On Delay</i> (+10 đến +20 µs).<br><br>"
            "<b>2. Điểm đầu đường nét bị hụt, đứt đoạn:</b><br>"
            "&nbsp;&nbsp;&bull; Nguyên nhân: Gương galvo đã chạy trước khi tia laser kịp phát sáng.<br>"
            "&nbsp;&nbsp;&bull; Khắc phục: <i>GIẢM Gate On Delay</i>.<br><br>"
            "<b>3. Điểm cuối đường nét bị cháy đốm hoặc vệt kéo dài:</b><br>"
            "&nbsp;&nbsp;&bull; Nguyên nhân: Tia laser chưa tắt sau khi gương galvo đã dừng lại.<br>"
            "&nbsp;&nbsp;&bull; Khắc phục: <i>GIẢM Gate Off Delay</i>.<br><br>"
            "<b>4. Điểm cuối đường nét bị cụt, thiếu nét:</b><br>"
            "&nbsp;&nbsp;&bull; Khắc phục: <i>TĂNG Gate Off Delay</i>."
        )
        guide_layout.addWidget(guide_text)

        layout.addWidget(guide_group)
        layout.addStretch()

    def apply_delays(self):
        on_d = self.spin_on_delay.value()
        off_d = self.spin_off_delay.value()
        try:
            self.controller.set_laser_delays(on_d, off_d)
            self.log_message.emit(f"Applied laser delays: Gate On = {on_d} µs, Gate Off = {off_d} µs")
            QMessageBox.information(self, "Thành Công", f"Đã áp dụng thời gian trễ:\nOn: {on_d} µs | Off: {off_d} µs")
        except Exception as e:
            QMessageBox.critical(self, "Lỗi", f"Không thể lưu độ trễ:\n{e}")

    def generate_matrix(self):
        """Generates 5 lines with delays [20, 40, 60, 80, 100] µs as in Example 04."""
        delays = [20, 40, 60, 80, 100]
        strokes = []
        y = -40.0
        for d in delays:
            strokes.append([
                {"type": "JUMP", "x": -30.0, "y": y},
                {"type": "LINE", "x": 30.0, "y": y, "delay_us": d}
            ])
            y += 20.0

        bbox = {"min_x": -30.0, "max_x": 30.0, "min_y": -40.0, "max_y": 40.0}
        self.vectors_changed.emit(strokes, bbox)
        self.log_message.emit("Generated delay tuning calibration matrix (5 lines: 20..100 µs).")
