"""
Tab 05: Laser Triggering & Digital I/O (Based on Example 05).
Configures hardware oscillators for Q-switched lasers, executes stationary burst pulse
drilling, and provides real-time 8-bit digital I/O control and monitoring.
"""

from typing import List, Dict, Any
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QPushButton, QGroupBox, QSpinBox, QDoubleSpinBox, QCheckBox,
    QMessageBox
)
from PyQt5.QtCore import Qt, pyqtSignal, QTimer

from sdk.python.rhothor import RhothorController


class IOTriggerTab(QWidget):
    """Digital I/O and laser oscillator configuration tab."""
    vectors_changed = pyqtSignal(list, object)
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller
        self.output_state = 0
        self._init_ui()

        # Timer to poll inputs periodically
        self.poll_timer = QTimer(self)
        self.poll_timer.timeout.connect(self.poll_inputs)
        self.poll_timer.start(500)  # 500 ms

    def _init_ui(self):
        layout = QVBoxLayout(self)
        layout.setSpacing(14)

        # 1. Oscillator Configuration
        osc_group = QGroupBox("1. BỘ DAO ĐỘNG PHÁT XUNG LASER Q-SWITCH (OSCILLATORS)")
        osc_layout = QGridLayout(osc_group)

        osc_layout.addWidget(QLabel("Kênh Oscillator:"), 0, 0)
        self.spin_osc_nr = QSpinBox()
        self.spin_osc_nr.setRange(1, 3)
        self.spin_osc_nr.setValue(3)
        self.spin_osc_nr.setSuffix(" (Gắn IO7)")
        osc_layout.addWidget(self.spin_osc_nr, 0, 1)

        osc_layout.addWidget(QLabel("Chu kỳ xung (Period):"), 0, 2)
        self.spin_period = QDoubleSpinBox()
        self.spin_period.setRange(1.0, 10000.0)
        self.spin_period.setValue(20.0)
        self.spin_period.setSuffix(" µs (50 kHz)")
        self.spin_period.valueChanged.connect(self._update_freq_label)
        osc_layout.addWidget(self.spin_period, 0, 3)

        osc_layout.addWidget(QLabel("Độ rộng xung (Pulse Width):"), 1, 0)
        self.spin_width = QDoubleSpinBox()
        self.spin_width.setRange(0.1, 1000.0)
        self.spin_width.setValue(5.0)
        self.spin_width.setSuffix(" µs")
        osc_layout.addWidget(self.spin_width, 1, 1)

        self.btn_set_osc = QPushButton("⚡ CÀI ĐẶT DAO ĐỘNG Q-SWITCH (SET OSCILLATOR)")
        self.btn_set_osc.setStyleSheet("background-color: #0d9488; color: white; font-weight: bold; padding: 8px;")
        self.btn_set_osc.clicked.connect(self.apply_oscillator)
        osc_layout.addWidget(self.btn_set_osc, 1, 2, 1, 2)

        layout.addWidget(osc_group)

        # 2. Burst Pulse Drilling
        burst_group = QGroupBox("2. BẮN XUNG KHOAN ĐIỂM TĨNH (STATIONARY BURST DRILLING)")
        burst_layout = QGridLayout(burst_group)

        burst_layout.addWidget(QLabel("Thời gian phát xung (Burst Time):"), 0, 0)
        self.spin_burst_time = QSpinBox()
        self.spin_burst_time.setRange(1, 50000)
        self.spin_burst_time.setValue(100)
        self.spin_burst_time.setSuffix(" µs")
        burst_layout.addWidget(self.spin_burst_time, 0, 1)

        burst_layout.addWidget(QLabel("Thời gian làm nguội (Cooling Delay):"), 0, 2)
        self.spin_cool_time = QSpinBox()
        self.spin_cool_time.setRange(1, 50000)
        self.spin_cool_time.setValue(50)
        self.spin_cool_time.setSuffix(" µs")
        burst_layout.addWidget(self.spin_cool_time, 0, 3)

        self.btn_gen_burst = QPushButton("⚡ TẠO 5 ĐIỂM BẮN XUNG TĨNH (BURST POINTS)")
        self.btn_gen_burst.setStyleSheet("background-color: #0284c7; color: white; font-weight: bold; padding: 8px;")
        self.btn_gen_burst.clicked.connect(self.generate_burst_pattern)
        burst_layout.addWidget(self.btn_gen_burst, 1, 0, 1, 4)

        layout.addWidget(burst_group)

        # 3. 8-Bit Digital I/O Control Board
        io_group = QGroupBox("3. BẢNG ĐIỀU KHIỂN & GIÁM SÁT DIGITAL I/O 8-BIT")
        io_layout = QGridLayout(io_group)

        # Outputs
        io_layout.addWidget(QLabel("<b>CỔNG XUẤT ĐẦU RA (OUTPUT BITS):</b>"), 0, 0, 1, 4)
        output_labels = [
            "IO0: Mở Cổng Laser (Gate)",
            "IO1: Khí Nén Thổi Phôi (Air Assist)",
            "IO2: Quạt Hút Khói (Exhaust)",
            "IO3: Đèn Laser Đỏ Định Vị (Red Guide)",
            "IO4: Đèn Báo Sẵn Sàng (Pilot Lamp)",
            "IO5: Trigger Laser Phụ / Osc 1",
            "IO6: Tín Hiệu Đồng Bộ / Osc 2",
            "IO7: Q-Switch External Trigger / Osc 3"
        ]
        self.out_checks = []
        for i, lbl in enumerate(output_labels):
            chk = QCheckBox(lbl)
            chk.stateChanged.connect(self._on_output_toggled)
            row = 1 + (i // 2)
            col = (i % 2) * 2
            io_layout.addWidget(chk, row, col, 1, 2)
            self.out_checks.append(chk)

        # Inputs
        io_layout.addWidget(QLabel("<b>CỔNG ĐỌC ĐẦU VÀO (INPUT SENSORS):</b>"), 5, 0, 1, 4)
        input_names = [
            "IN0: Cửa An Toàn (Interlock)",
            "IN1: Bàn Đạp Chân (Footswitch)",
            "IN2: Nước Làm Mát OK (Chiller)",
            "IN3: Nguồn Laser Sẵn Sàng (Ready)",
            "IN4: Cảm Biến Áp Suất Khí",
            "IN5: Báo Lỗi Laser (Error)",
            "IN6: Tín Hiệu Start Từ PLC",
            "IN7: Tín Hiệu Dừng Khẩn Cấp"
        ]
        self.in_labels = []
        for i, name in enumerate(input_names):
            lbl = QLabel(f"⚪ {name}")
            lbl.setStyleSheet("color: #64748b; font-size: 11px;")
            row = 6 + (i // 2)
            col = (i % 2) * 2
            io_layout.addWidget(lbl, row, col, 1, 2)
            self.in_labels.append(lbl)

        layout.addWidget(io_group)
        layout.addStretch()

    def _update_freq_label(self, val):
        if val > 0:
            khz = 1000.0 / val
            self.spin_period.setSuffix(f" µs ({khz:.1f} kHz)")

    def apply_oscillator(self):
        nr = self.spin_osc_nr.value()
        period = self.spin_period.value()
        width = self.spin_width.value()
        try:
            self.controller.set_oscillator(nr, period, width)
            self.log_message.emit(f"Configured Oscillator {nr}: Period={period} µs, Pulse Width={width} µs")
            QMessageBox.information(
                self, "Thành Công",
                f"Đã kích hoạt Oscillator {nr} trên IO{4+nr}:\nChu kỳ: {period} µs\nĐộ rộng xung: {width} µs"
            )
        except Exception as e:
            QMessageBox.critical(self, "Lỗi", f"Không thể cài đặt Oscillator:\n{e}")

    def generate_burst_pattern(self):
        """Generates 5 burst drill points as in Example 05."""
        points = [(-20.0, 0.0), (-10.0, 0.0), (0.0, 0.0), (10.0, 0.0), (20.0, 0.0)]
        strokes = []
        for pt in points:
            strokes.append([
                {"type": "JUMP", "x": pt[0], "y": pt[1]},
                {"type": "BURST", "x": pt[0], "y": pt[1], "time_us": self.spin_burst_time.value()}
            ])
        bbox = {"min_x": -25.0, "max_x": 25.0, "min_y": -5.0, "max_y": 5.0}
        self.vectors_changed.emit(strokes, bbox)
        self.log_message.emit("Generated 5 stationary burst drill test points.")

    def _on_output_toggled(self):
        mask = 0
        for i, chk in enumerate(self.out_checks):
            if chk.isChecked():
                mask |= (1 << i)

        self.output_state = mask
        try:
            self.controller.set_io(mask=0xFF, value=mask)
            self.log_message.emit(f"Digital Outputs updated: 0x{mask:02X}")
        except Exception as e:
            self.log_message.emit(f"Failed to set digital outputs: {e}")

    def poll_inputs(self):
        if not self.controller.is_connected:
            return
        try:
            in_val = self.controller.get_io()
            for i in range(8):
                is_high = bool(in_val & (1 << i))
                lbl = self.in_labels[i]
                txt = lbl.text()[2:]  # Strip icon
                if is_high:
                    lbl.setText(f"🟢 {txt}")
                    lbl.setStyleSheet("color: #4ade80; font-weight: bold; font-size: 11px;")
                else:
                    lbl.setText(f"⚪ {txt}")
                    lbl.setStyleSheet("color: #64748b; font-size: 11px;")
        except Exception:
            pass
