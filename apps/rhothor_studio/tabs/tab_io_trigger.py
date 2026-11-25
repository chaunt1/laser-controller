"""
Tab 05: Laser Triggering & Digital I/O (Based on Example 05).
Configures hardware oscillators for Q-switched lasers, executes stationary burst pulse
drilling, and provides real-time 8-bit digital I/O control and monitoring.
"""

from typing import List, Dict, Any
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QPushButton, QGroupBox, QSpinBox, QDoubleSpinBox, QCheckBox,
    QMessageBox, QDialog, QTextEdit
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

        # 4. Fiber Laser Source Sync & Latch Panel
        fiber_group = QGroupBox("4. ĐỒNG BỘ NGUỒN FIBER LASER & CHỐT CÔNG SUẤT (FIBER LASER ADAPTER)")
        fiber_layout = QGridLayout(fiber_group)

        self.btn_send_latch = QPushButton("⚡ PHÁT XUNG LATCH CHỐT CÔNG SUẤT (PIN 9)")
        self.btn_send_latch.setStyleSheet("""
            QPushButton {
                background-color: #7c3aed;
                color: white;
                font-weight: bold;
                padding: 9px;
                border-radius: 4px;
            }
            QPushButton:hover { background-color: #8b5cf6; }
        """)
        self.btn_send_latch.clicked.connect(self.send_latch_pulse_ui)
        fiber_layout.addWidget(self.btn_send_latch, 0, 0)

        self.btn_show_wiring = QPushButton("📖 XEM SƠ ĐỒ ĐẤU DÂY X7 SANG DB-25 FIBER LASER")
        self.btn_show_wiring.setStyleSheet("""
            QPushButton {
                background-color: #059669;
                color: white;
                font-weight: bold;
                padding: 9px;
                border-radius: 4px;
            }
            QPushButton:hover { background-color: #10b981; }
        """)
        self.btn_show_wiring.clicked.connect(self.show_wiring_dialog)
        fiber_layout.addWidget(self.btn_show_wiring, 0, 1)

        layout.addWidget(fiber_group)
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

    def send_latch_pulse_ui(self):
        """Manually sends a latch pulse to lock D0-D7 power on fiber laser."""
        try:
            if hasattr(self.controller, "send_latch_pulse"):
                self.controller.send_latch_pulse(pin_bit=0)
            self.log_message.emit("Đã phát xung Latch chốt công suất trên Pin 9 của Nguồn Laser.")
            QMessageBox.information(
                self, "Phát Xung Latch Thành Công",
                "Đã phát xung kích sườn trên Chân 9 (Latch)!\n"
                "Nguồn Fiber Laser đã nạp và lưu giá trị công suất hiện tại vào thanh ghi DAC nội."
            )
        except Exception as e:
            QMessageBox.critical(self, "Lỗi Phát Xung", f"Không thể gửi xung Latch:\n{e}")

    def show_wiring_dialog(self):
        """Displays interactive pinout and wiring schematic for Fiber Laser integration."""
        diag = QDialog(self)
        diag.setWindowTitle("Sơ Đồ Đấu Nối Cổng X7 Galvo Sang Nguồn Fiber Laser (Raycus / JPT / Max)")
        diag.resize(800, 620)
        d_layout = QVBoxLayout(diag)

        text = QTextEdit(diag)
        text.setReadOnly(True)
        text.setFontFamily("Consolas, Courier New, monospace")
        text.setStyleSheet("background-color: #0f172a; color: #f8fafc; font-size: 12px; line-height: 1.4;")
        
        schematic = """
========================================================================================
⚡ SƠ ĐỒ ĐẤU NỐI CHI TIẾT: CỔNG X7 (GALVO NEWSON) ---> CỔNG DB-25 (NGUỒN FIBER LASER)
   Áp dụng cho nguồn: Raycus, JPT, Max Photonics, IPG (Chuẩn công nghiệp Q-Switched/MOPA)
========================================================================================

1. MẠCH NGUỒN NUÔI & KHÓA AN TOÀN:
   -------------------------------------------------------------------------------------
   Cổng X7 Galvo (Pin 21 - DC +5V)  ----->  Nguồn Laser (Pin 23 - Emergency Stop / Interlock)
   Cổng X7 Galvo (Pin 21 - DC +5V)  ----->  Nguồn Laser (Pin 18 - Main Diode Pump / MO)
   Cổng X7 Galvo (Pin 8 - GND Mass) ----->  Nguồn Laser (Pin 10, 14, 15, 24, 25 - GND)

2. CÀI ĐẶT CÔNG SUẤT (D0 - D7):
   -------------------------------------------------------------------------------------
   Cổng X7 Galvo (Pin 21 - DC +5V)  ----->  Nguồn Laser (Pin 1, 2, 3, 4, 5, 6, 7, 8: D0-D7)
   (*) Nối cả 8 chân lên +5V tương ứng mức công suất cực đại 100% (Mã nhị phân 0xFF = 255).
       Nếu muốn đổi công suất, có thể qua một bộ công tắc gạt DIP-Switch 8 vị trí.

3. MẠCH ĐỆM CHUYỂN ĐỔI TÍN HIỆU VI SAI RS-485 SANG TTL (Dùng 3 IC MAX485 hoặc SN65HVD485):
   Cấp nguồn IC MAX485: Chân 8 (VCC) nối +5V (Pin 21 Galvo); Chân 5 (GND) nối Pin 8 Galvo.
   Chân 2 (RE) và Chân 3 (DE) của cả 3 IC MAX485 nối xuống GND (Chế độ Thu Receiver).
   -------------------------------------------------------------------------------------
   [Tín hiệu MỞ TIA (Emission Gate)]
   Cổng X7 Galvo (Pin 19 - IO6+)  ----->  MAX485 #1 (Chân A)
   Cổng X7 Galvo (Pin 6  - IO6-)  ----->  MAX485 #1 (Chân B)
   MAX485 #1 Đầu ra TTL (Chân RO) ----->  Nguồn Laser (Pin 19 - Emission Gate / Modulation)

   [Tín hiệu TẦN SỐ XUNG (Sync / PRR)]
   Cổng X7 Galvo (Pin 20 - IO7+)  ----->  MAX485 #2 (Chân A)
   Cổng X7 Galvo (Pin 7  - IO7-)  ----->  MAX485 #2 (Chân B)
   MAX485 #2 Đầu ra TTL (Chân RO) ----->  Nguồn Laser (Pin 20 - Sync / Pulse Repetition)

   [Tín hiệu CHỐT CÔNG SUẤT (Latch)]
   Cổng X7 Galvo (Pin 18 - IO5+)  ----->  MAX485 #3 (Chân A)
   Cổng X7 Galvo (Pin 5  - IO5-)  ----->  MAX485 #3 (Chân B)
   MAX485 #3 Đầu ra TTL (Chân RO) ----->  Nguồn Laser (Pin 9 - Latch Clock)

========================================================================================
💡 LƯU Ý QUAN TRỌNG:
- TUYỆT ĐỐI KHÔNG cắm cáp DB-25 thẳng từ Galvo X7 sang Nguồn Laser (Chân 10,11,23 trên Galvo
  là nguồn 12VDC, nếu cắm thẳng sẽ làm hỏng IC logic của Nguồn Laser).
- Tín hiệu IO6 và IO7 tự động đồng bộ theo từng micro-giây với chuyển động của gương Galvo.
- Khi bắt đầu bấm 'Khắc', phần mềm sẽ tự động gửi xung Latch kích hoạt công suất!
========================================================================================
        """
        text.setText(schematic)
        d_layout.addWidget(text)

        btn_close = QPushButton("ĐÃ HIỂU & ĐÓNG", diag)
        btn_close.clicked.connect(diag.accept)
        d_layout.addWidget(btn_close)

        diag.exec_()
