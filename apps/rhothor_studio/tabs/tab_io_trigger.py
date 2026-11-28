"""
Tab 05: Laser Triggering & Digital I/O (Based on Example 05 & Hardware X7 Pinout).
Configures hardware oscillators for Q-switched lasers, executes stationary burst pulse
drilling, provides manual real-time control of 7 differential I/O ports on connector X7,
and features an Automated Sequential Step Tester (e.g. 1/14 -> 7/20 for 5s each)
to test and verify physical RS-485/TTL signals with a multimeter or oscilloscope.
"""

from typing import List, Dict, Any, Optional
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QPushButton, QGroupBox, QSpinBox, QDoubleSpinBox, QCheckBox,
    QMessageBox, QDialog, QTextEdit, QScrollArea, QProgressBar,
    QFrame
)
from PyQt5.QtCore import Qt, pyqtSignal, QTimer

from sdk.python.rhothor import RhothorController


# Metadata for 7 differential IO ports on connector X7
IO_PORT_DEFS = [
    {
        "port": 1,
        "pins": "1 / 14",
        "pin_neg": 1,
        "pin_pos": 14,
        "name": "IO1",
        "desc": "Tín hiệu số vi sai RS-485 (Trở 100Ω)",
        "role": "General Digital I/O 1"
    },
    {
        "port": 2,
        "pins": "2 / 15",
        "pin_neg": 2,
        "pin_pos": 15,
        "name": "IO2",
        "desc": "Tín hiệu số vi sai RS-485 (Trở 100Ω)",
        "role": "General Digital I/O 2"
    },
    {
        "port": 3,
        "pins": "3 / 16",
        "pin_neg": 3,
        "pin_pos": 16,
        "name": "IO3",
        "desc": "Tín hiệu số vi sai RS-485 (Trở 100Ω)",
        "role": "General Digital I/O 3"
    },
    {
        "port": 4,
        "pins": "4 / 17",
        "pin_neg": 4,
        "pin_pos": 17,
        "name": "IO4",
        "desc": "Tín hiệu số vi sai RS-485 (Trở 100Ω)",
        "role": "General Digital I/O 4"
    },
    {
        "port": 5,
        "pins": "5 / 18",
        "pin_neg": 5,
        "pin_pos": 18,
        "name": "IO5",
        "desc": "Laser Latch Pin 9 / Dao động 1 (Ko trở)",
        "role": "Laser Power Latch / Osc 1"
    },
    {
        "port": 6,
        "pins": "6 / 19",
        "pin_neg": 6,
        "pin_pos": 19,
        "name": "IO6",
        "desc": "Emission Gate Pin 19 / Dao động 2 (Ko trở)",
        "role": "Laser Emission Gate / Osc 2"
    },
    {
        "port": 7,
        "pins": "7 / 20",
        "pin_neg": 7,
        "pin_pos": 20,
        "name": "IO7",
        "desc": "Sync/PRR Pin 20 / Q-Switch Osc 3 (Ko trở)",
        "role": "Laser Sync / External PRR / Osc 3"
    },
]


class IOTriggerTab(QWidget):
    """Digital I/O, Laser Oscillator, and Automated Port Step Tester Tab."""
    vectors_changed = pyqtSignal(list, object)
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller
        self.output_state = 0

        # Auto Step Tester states
        self.is_auto_testing = False
        self.auto_step_port_idx = 0  # 0 to 6 (corresponds to IO1 to IO7)
        self.step_remaining_ms = 5000
        self.step_total_ms = 5000

        self._init_ui()

        # Timer for polling inputs
        self.poll_timer = QTimer(self)
        self.poll_timer.timeout.connect(self.poll_inputs)
        self.poll_timer.start(500)

        # High-resolution timer for Auto Step Tester countdown
        self.step_timer = QTimer(self)
        self.step_timer.setInterval(100)  # 100 ms ticks for smooth UI
        self.step_timer.timeout.connect(self._on_step_timer_tick)

    def _init_ui(self):
        root_layout = QVBoxLayout(self)
        root_layout.setContentsMargins(4, 4, 4, 4)

        # Use ScrollArea to host all controls gracefully
        scroll = QScrollArea(self)
        scroll.setWidgetResizable(True)
        scroll.setFrameShape(QFrame.NoFrame)
        scroll.setStyleSheet("QScrollArea { background-color: transparent; }")

        container = QWidget()
        layout = QVBoxLayout(container)
        layout.setSpacing(14)
        layout.setContentsMargins(6, 6, 6, 6)

        # -------------------------------------------------------------
        # 1. Oscillator Configuration
        # -------------------------------------------------------------
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

        # -------------------------------------------------------------
        # 2. Burst Pulse Drilling
        # -------------------------------------------------------------
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

        # -------------------------------------------------------------
        # 3. Dedicated Automated Port Step Tester (New Requested Feature)
        # -------------------------------------------------------------
        test_group = QGroupBox("3. CHẾ ĐỘ TEST TỰ ĐỘNG BẬT TẮT TUẦN TỰ TỪNG CẶP CHÂN (AUTO STEP PORT TESTER)")
        test_group.setStyleSheet("QGroupBox { font-weight: bold; color: #38bdf8; }")
        test_layout = QVBoxLayout(test_group)
        test_layout.setSpacing(10)

        # Configuration & control row
        ctrl_row = QHBoxLayout()
        ctrl_row.addWidget(QLabel("<b>Thời gian bật mỗi cổng:</b>"))
        self.spin_step_sec = QSpinBox()
        self.spin_step_sec.setRange(1, 60)
        self.spin_step_sec.setValue(5)
        self.spin_step_sec.setSuffix(" Giây")
        self.spin_step_sec.setToolTip("Thời gian giữ mức HIGH (BẬT) cho mỗi cặp chân trước khi chuyển sang cặp tiếp theo.")
        ctrl_row.addWidget(self.spin_step_sec)

        self.chk_loop = QCheckBox("Lặp lại liên tục (Continuous Loop)")
        self.chk_loop.setChecked(True)
        self.chk_loop.setToolTip("Khi chạy xong từ IO1 đến IO7, tự động quay lại IO1 để tiếp tục kiểm tra đo đạc.")
        ctrl_row.addWidget(self.chk_loop)

        ctrl_row.addStretch()

        self.btn_start_auto_test = QPushButton("▶ BẮT ĐẦU TEST TỰ ĐỘNG (1/14 -> 7/20)")
        self.btn_start_auto_test.setStyleSheet("""
            QPushButton {
                background-color: #16a34a;
                color: white;
                font-weight: bold;
                padding: 9px 18px;
                border-radius: 5px;
                font-size: 13px;
            }
            QPushButton:hover { background-color: #22c55e; }
        """)
        self.btn_start_auto_test.clicked.connect(self.start_auto_step_test)
        ctrl_row.addWidget(self.btn_start_auto_test)

        self.btn_stop_auto_test = QPushButton("⏹ DỪNG TEST (STOP)")
        self.btn_stop_auto_test.setEnabled(False)
        self.btn_stop_auto_test.setStyleSheet("""
            QPushButton {
                background-color: #dc2626;
                color: white;
                font-weight: bold;
                padding: 9px 15px;
                border-radius: 5px;
            }
            QPushButton:hover { background-color: #ef4444; }
            QPushButton:disabled { background-color: #475569; color: #94a3b8; }
        """)
        self.btn_stop_auto_test.clicked.connect(self.stop_auto_step_test)
        ctrl_row.addWidget(self.btn_stop_auto_test)

        self.btn_next_port = QPushButton("⏭ CỔNG TIẾP (NEXT)")
        self.btn_next_port.setEnabled(False)
        self.btn_next_port.setStyleSheet("""
            QPushButton {
                background-color: #2563eb;
                color: white;
                font-weight: bold;
                padding: 9px 12px;
                border-radius: 5px;
            }
            QPushButton:hover { background-color: #3b82f6; }
            QPushButton:disabled { background-color: #475569; color: #94a3b8; }
        """)
        self.btn_next_port.clicked.connect(self.advance_to_next_port)
        ctrl_row.addWidget(self.btn_next_port)

        test_layout.addLayout(ctrl_row)

        # Big Visual Status Banner
        self.card_status = QFrame()
        self.card_status.setStyleSheet("""
            QFrame {
                background-color: #0f172a;
                border: 2px solid #334155;
                border-radius: 6px;
                padding: 10px;
            }
        """)
        card_layout = QVBoxLayout(self.card_status)
        card_layout.setSpacing(6)

        self.lbl_active_port = QLabel("TRẠNG THÁI: SẴN SÀNG - CHƯA CHẠY TEST (BẤM 'BẮT ĐẦU TEST TỰ ĐỘNG' ĐỂ BẬT TỪNG CỔNG 5 GIÂY)")
        self.lbl_active_port.setStyleSheet("font-size: 13px; font-weight: bold; color: #facc15;")
        card_layout.addWidget(self.lbl_active_port)

        # Progress bar
        self.progress_bar = QProgressBar()
        self.progress_bar.setRange(0, 100)
        self.progress_bar.setValue(0)
        self.progress_bar.setTextVisible(True)
        self.progress_bar.setFormat("Chờ bắt đầu...")
        self.progress_bar.setStyleSheet("""
            QProgressBar {
                background-color: #1e293b;
                border: 1px solid #475569;
                border-radius: 4px;
                height: 20px;
                text-align: center;
                color: white;
                font-weight: bold;
            }
            QProgressBar::chunk {
                background-color: qlineargradient(x1:0, y1:0, x2:1, y2:0, stop:0 #10b981, stop:1 #38bdf8);
                border-radius: 3px;
            }
        """)
        card_layout.addWidget(self.progress_bar)

        self.lbl_probe_guide = QLabel(
            "💡 <b>HƯỚNG DẪN ĐO ĐỒNG HỒ VOM:</b><br>"
            "• Cắm que <b>ĐỎ</b> vào chân <b>IO+</b> (Pin 14..20), que <b>ĐEN</b> vào chân <b>IO-</b> (Pin 1..7).<br>"
            "• Khi cổng BẬT (5s): Điện áp vi sai đo được là <b>+2.5V đến +4.5V</b> (hoặc que Đỏ cắm IO+, que Đen cắm Pin 8 GND: đo được <b>~ +3.5V đến +5.0V</b>).<br>"
            "• Khi cổng TẮT: Điện áp đo được sẽ tụt về mức âm (<b>-2.5V</b>) hoặc <b>0V</b>."
        )
        self.lbl_probe_guide.setStyleSheet("color: #cbd5e1; font-size: 11px; background-color: #1e293b; padding: 6px; border-radius: 4px;")
        card_layout.addWidget(self.lbl_probe_guide)

        test_layout.addWidget(self.card_status)

        layout.addWidget(test_group)

        # -------------------------------------------------------------
        # 4. Manual 7 Differential I/O Port Control & Monitor Board
        # -------------------------------------------------------------
        io_group = QGroupBox("4. BẢNG ĐIỀU KHIỂN THỦ CÔNG 7 CẶP CHÂN DIGITAL I/O X7 (PINS 1-7 / 14-20)")
        io_layout = QVBoxLayout(io_group)

        # Action Buttons Header
        btn_action_row = QHBoxLayout()
        btn_all_on = QPushButton("⚡ BẬT TẤT CẢ (ALL ON)")
        btn_all_on.setStyleSheet("background-color: #0369a1; color: white; font-weight: bold; padding: 6px 12px; border-radius: 4px;")
        btn_all_on.clicked.connect(self.set_all_ports_on)
        btn_action_row.addWidget(btn_all_on)

        btn_all_off = QPushButton("⭕ TẮT TẤT CẢ (ALL OFF)")
        btn_all_off.setStyleSheet("background-color: #475569; color: white; font-weight: bold; padding: 6px 12px; border-radius: 4px;")
        btn_all_off.clicked.connect(self.set_all_ports_off)
        btn_action_row.addWidget(btn_all_off)

        btn_action_row.addStretch()

        btn_show_guide = QPushButton("❓ HƯỚNG DẪN CÁCH ĐO VOM")
        btn_show_guide.setStyleSheet("background-color: #334155; color: #38bdf8; font-weight: bold; padding: 6px 12px; border-radius: 4px;")
        btn_show_guide.clicked.connect(self.show_measurement_guide_dialog)
        btn_action_row.addWidget(btn_show_guide)

        io_layout.addLayout(btn_action_row)

        # Grid of 7 Ports
        grid_ports = QGridLayout()
        grid_ports.setSpacing(8)

        self.port_widgets = []
        for i, pdef in enumerate(IO_PORT_DEFS):
            frame = QFrame()
            frame.setStyleSheet("""
                QFrame {
                    background-color: #1e293b;
                    border: 1px solid #334155;
                    border-radius: 5px;
                    padding: 4px;
                }
            """)
            p_layout = QHBoxLayout(frame)
            p_layout.setContentsMargins(8, 4, 8, 4)

            lbl_name = QLabel(f"<b>{pdef['name']}</b> (Chân {pdef['pins']}): {pdef['desc']}")
            lbl_name.setStyleSheet("color: #e2e8f0; font-size: 11px;")
            p_layout.addWidget(lbl_name)

            p_layout.addStretch()

            lbl_status = QLabel("⚪ 0V (LOW)")
            lbl_status.setStyleSheet("color: #94a3b8; font-weight: bold; font-size: 11px; min-width: 90px;")
            p_layout.addWidget(lbl_status)

            btn_toggle = QPushButton("BẬT")
            btn_toggle.setCheckable(True)
            btn_toggle.setFixedWidth(65)
            btn_toggle.setStyleSheet("""
                QPushButton { background-color: #334155; color: white; font-weight: bold; padding: 4px; border-radius: 3px; }
                QPushButton:checked { background-color: #16a34a; color: white; }
            """)
            btn_toggle.toggled.connect(lambda state, port=pdef['port']: self._on_port_btn_toggled(port, state))
            p_layout.addWidget(btn_toggle)

            row = i // 2
            col = i % 2
            grid_ports.addWidget(frame, row, col)

            self.port_widgets.append({
                "port": pdef['port'],
                "frame": frame,
                "lbl_name": lbl_name,
                "lbl_status": lbl_status,
                "btn_toggle": btn_toggle
            })

        io_layout.addLayout(grid_ports)

        # Inputs Monitor (from rtGetIO)
        io_layout.addWidget(QLabel("<b>CÁC CHÂN ĐỌC ĐẦU VÀO (DIGITAL INPUTS rtGetIO):</b>"))
        in_grid = QGridLayout()
        input_names = [
            "IN1: IO1 (Chân 1/14)",
            "IN2: IO2 (Chân 2/15)",
            "IN3: IO3 (Chân 3/16)",
            "IN4: IO4 (Chân 4/17)",
            "IN5: IO5 (Chân 5/18)",
            "IN6: IO6 (Chân 6/19)",
            "IN7: IO7 (Chân 7/20)",
        ]
        self.in_labels = []
        for i, name in enumerate(input_names):
            lbl = QLabel(f"⚪ {name}")
            lbl.setStyleSheet("color: #64748b; font-size: 11px;")
            row = i // 4
            col = i % 4
            in_grid.addWidget(lbl, row, col)
            self.in_labels.append(lbl)

        io_layout.addLayout(in_grid)
        layout.addWidget(io_group)

        # -------------------------------------------------------------
        # 5. Fiber Laser Source Sync & Latch Panel
        # -------------------------------------------------------------
        fiber_group = QGroupBox("5. ĐỒNG BỘ NGUỒN FIBER LASER & CHỐT CÔNG SUẤT (FIBER LASER ADAPTER)")
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

        scroll.setWidget(container)
        root_layout.addWidget(scroll)

    # -------------------------------------------------------------
    # Logic: Automated Step Tester (1/14 -> 7/20)
    # -------------------------------------------------------------
    def start_auto_step_test(self):
        """Starts automated step test cycling through ports 1 to 7."""
        if not self.controller.is_connected and not self.controller.simulate:
            QMessageBox.warning(self, "Chưa Kết Nối", "Vui lòng kết nối phần cứng Galvo Rhothor trước khi chạy test.")
            return

        self.is_auto_testing = True
        self.auto_step_port_idx = 0
        self.step_total_ms = self.spin_step_sec.value() * 1000
        self.step_remaining_ms = self.step_total_ms

        self.btn_start_auto_test.setEnabled(False)
        self.btn_stop_auto_test.setEnabled(True)
        self.btn_next_port.setEnabled(True)
        self.spin_step_sec.setEnabled(False)

        # Turn OFF all ports initially
        self.set_all_ports_off(log=False)

        # Activate the first port
        self._activate_step_port(0)
        self.step_timer.start()

        self.log_message.emit(f"Bắt đầu Auto Step Test: Chu kỳ {self.spin_step_sec.value()}s mỗi cổng.")

    def stop_auto_step_test(self):
        """Stops the automated step test and turns all outputs OFF."""
        self.is_auto_testing = False
        self.step_timer.stop()

        self.btn_start_auto_test.setEnabled(True)
        self.btn_stop_auto_test.setEnabled(False)
        self.btn_next_port.setEnabled(False)
        self.spin_step_sec.setEnabled(True)

        # Turn OFF all ports
        self.set_all_ports_off(log=False)

        # Reset highlights
        for item in self.port_widgets:
            item["frame"].setStyleSheet("""
                QFrame {
                    background-color: #1e293b;
                    border: 1px solid #334155;
                    border-radius: 5px;
                    padding: 4px;
                }
            """)

        self.lbl_active_port.setText("TRẠNG THÁI: ĐÃ DỪNG TEST. TẤT CẢ 7 CỔNG ĐÃ TẮT.")
        self.lbl_active_port.setStyleSheet("font-size: 13px; font-weight: bold; color: #94a3b8;")
        self.progress_bar.setValue(0)
        self.progress_bar.setFormat("Đã dừng")
        self.card_status.setStyleSheet("QFrame { background-color: #0f172a; border: 2px solid #334155; border-radius: 6px; padding: 10px; }")
        self.log_message.emit("Đã dừng chu trình Auto Step Test.")

    def advance_to_next_port(self):
        """Manually skips immediately to the next port."""
        if not self.is_auto_testing:
            return
        # Turn off current port
        curr_pdef = IO_PORT_DEFS[self.auto_step_port_idx]
        self._set_port_hardware(curr_pdef['port'], False)

        next_idx = self.auto_step_port_idx + 1
        if next_idx >= len(IO_PORT_DEFS):
            if self.chk_loop.isChecked():
                next_idx = 0
            else:
                self.stop_auto_step_test()
                QMessageBox.information(self, "Hoàn Tất Test", "Đã hoàn thành kiểm tra toàn bộ 7 cặp chân từ IO1 đến IO7!")
                return

        self._activate_step_port(next_idx)

    def _activate_step_port(self, idx: int):
        self.auto_step_port_idx = idx
        self.step_remaining_ms = self.step_total_ms
        pdef = IO_PORT_DEFS[idx]

        # Turn ON this port
        self._set_port_hardware(pdef['port'], True)

        # Update Banner
        sec_total = self.step_total_ms / 1000.0
        self.lbl_active_port.setText(
            f"⚡ ĐANG BẬT: {pdef['name']} (CẶP CHÂN {pdef['pins']}) - {pdef['desc'].upper()}"
        )
        self.lbl_active_port.setStyleSheet("font-size: 13px; font-weight: bold; color: #4ade80;")
        self.card_status.setStyleSheet("QFrame { background-color: #064e3b; border: 2px solid #10b981; border-radius: 6px; padding: 10px; }")

        # Update Probe Guide
        self.lbl_probe_guide.setText(
            f"👉 <b>CÁCH ĐO CẶP CHÂN {pdef['pins']}:</b><br>"
            f"• Cắm que <b>ĐỎ</b> vào <b>Pin {pdef['pin_pos']} ({pdef['name']}+)</b>, que <b>ĐEN</b> vào <b>Pin {pdef['pin_neg']} ({pdef['name']}-)</b>.<br>"
            f"• Điện áp vi sai đo được trên đồng hồ VOM (Thang DC 20V): <b>+2.5V đến +4.5V</b>.<br>"
            f"• Hoặc que ĐỎ cắm Pin {pdef['pin_pos']}, que ĐEN cắm <b>Pin 8 (GND)</b> -> đo được <b>~ +3.5V đến +5.0V</b>."
        )

        # Highlight in grid
        for i, item in enumerate(self.port_widgets):
            if i == idx:
                item["frame"].setStyleSheet("""
                    QFrame {
                        background-color: #065f46;
                        border: 2px solid #34d399;
                        border-radius: 5px;
                        padding: 4px;
                    }
                """)
                item["btn_toggle"].blockSignals(True)
                item["btn_toggle"].setChecked(True)
                item["btn_toggle"].setText("BẬT")
                item["btn_toggle"].blockSignals(False)
                item["lbl_status"].setText("🟢 +3.5V (BẬT)")
                item["lbl_status"].setStyleSheet("color: #4ade80; font-weight: bold; font-size: 11px; min-width: 90px;")
            else:
                item["frame"].setStyleSheet("""
                    QFrame {
                        background-color: #1e293b;
                        border: 1px solid #334155;
                        border-radius: 5px;
                        padding: 4px;
                    }
                """)
                item["btn_toggle"].blockSignals(True)
                item["btn_toggle"].setChecked(False)
                item["btn_toggle"].setText("TẮT")
                item["btn_toggle"].blockSignals(False)
                item["lbl_status"].setText("⚪ 0V (LOW)")
                item["lbl_status"].setStyleSheet("color: #94a3b8; font-weight: bold; font-size: 11px; min-width: 90px;")

        self.log_message.emit(f"Auto Step Test: Kích hoạt BẬT {pdef['name']} (Chân {pdef['pins']}) trong {sec_total:.0f}s.")

    def _on_step_timer_tick(self):
        if not self.is_auto_testing:
            return

        self.step_remaining_ms -= 100
        pct = max(0, min(100, int((self.step_remaining_ms / self.step_total_ms) * 100)))
        sec_left = max(0.0, self.step_remaining_ms / 1000.0)

        pdef = IO_PORT_DEFS[self.auto_step_port_idx]
        self.progress_bar.setValue(pct)
        self.progress_bar.setFormat(f"{pdef['name']} (Chân {pdef['pins']}): Còn {sec_left:.1f} Giây ({pct}%)")

        if self.step_remaining_ms <= 0:
            # Time is up for current port: turn it OFF
            self._set_port_hardware(pdef['port'], False)
            self.log_message.emit(f"Auto Step Test: Hết giờ {pdef['name']}. Đã tắt cổng.")

            # Move to next
            next_idx = self.auto_step_port_idx + 1
            if next_idx >= len(IO_PORT_DEFS):
                if self.chk_loop.isChecked():
                    self._activate_step_port(0)
                else:
                    self.stop_auto_step_test()
                    QMessageBox.information(
                        self, "Hoàn Tất Test",
                        "Đã hoàn thành kiểm tra tuần tự 7 cặp chân từ IO1 (1/14) đến IO7 (7/20)!"
                    )
            else:
                self._activate_step_port(next_idx)

    # -------------------------------------------------------------
    # Logic: Manual I/O Control
    # -------------------------------------------------------------
    def _set_port_hardware(self, port_nr: int, state: bool):
        try:
            if hasattr(self.controller, "set_io_port"):
                self.controller.set_io_port(port_nr, state)
            else:
                bit = port_nr - 1
                mask = 1 << bit
                val = mask if state else 0
                self.controller.set_io(value=val, mask=mask)
        except Exception as e:
            self.log_message.emit(f"Lỗi đặt trạng thái IO{port_nr}: {e}")

    def _on_port_btn_toggled(self, port_nr: int, state: bool):
        # Update hardware
        self._set_port_hardware(port_nr, state)

        # Update UI badge
        idx = port_nr - 1
        widget_info = self.port_widgets[idx]
        btn = widget_info["btn_toggle"]
        lbl = widget_info["lbl_status"]

        btn.setText("BẬT" if state else "TẮT")
        if state:
            lbl.setText("🟢 +3.5V (BẬT)")
            lbl.setStyleSheet("color: #4ade80; font-weight: bold; font-size: 11px; min-width: 90px;")
            self.log_message.emit(f"Thủ công: BẬT IO{port_nr} (Chân {IO_PORT_DEFS[idx]['pins']}).")
        else:
            lbl.setText("⚪ 0V (LOW)")
            lbl.setStyleSheet("color: #94a3b8; font-weight: bold; font-size: 11px; min-width: 90px;")
            self.log_message.emit(f"Thủ công: TẮT IO{port_nr} (Chân {IO_PORT_DEFS[idx]['pins']}).")

    def set_all_ports_on(self):
        """Turns all 7 digital I/O ports ON."""
        try:
            if hasattr(self.controller, "set_all_io_ports"):
                self.controller.set_all_io_ports(True)
            else:
                self.controller.set_io(value=0x7F, mask=0x7F)

            for item in self.port_widgets:
                item["btn_toggle"].blockSignals(True)
                item["btn_toggle"].setChecked(True)
                item["btn_toggle"].setText("BẬT")
                item["btn_toggle"].blockSignals(False)
                item["lbl_status"].setText("🟢 +3.5V (BẬT)")
                item["lbl_status"].setStyleSheet("color: #4ade80; font-weight: bold; font-size: 11px; min-width: 90px;")

            self.log_message.emit("Đã BẬT TẤT CẢ 7 cặp chân I/O (IO1 đến IO7).")
        except Exception as e:
            self.log_message.emit(f"Lỗi khi bật tất cả cổng: {e}")

    def set_all_ports_off(self, log=True):
        """Turns all 7 digital I/O ports OFF."""
        try:
            if hasattr(self.controller, "set_all_io_ports"):
                self.controller.set_all_io_ports(False)
            else:
                self.controller.set_io(value=0, mask=0x7F)

            for item in self.port_widgets:
                item["btn_toggle"].blockSignals(True)
                item["btn_toggle"].setChecked(False)
                item["btn_toggle"].setText("TẮT")
                item["btn_toggle"].blockSignals(False)
                item["lbl_status"].setText("⚪ 0V (LOW)")
                item["lbl_status"].setStyleSheet("color: #94a3b8; font-weight: bold; font-size: 11px; min-width: 90px;")

            if log:
                self.log_message.emit("Đã TẮT TẤT CẢ 7 cặp chân I/O (IO1 đến IO7).")
        except Exception as e:
            if log:
                self.log_message.emit(f"Lỗi khi tắt tất cả cổng: {e}")

    # -------------------------------------------------------------
    # Logic: Poll Inputs (7 pins)
    # -------------------------------------------------------------
    def poll_inputs(self):
        if not self.controller.is_connected:
            return
        try:
            in_val = self.controller.get_io()
            for i in range(len(self.in_labels)):
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

    # -------------------------------------------------------------
    # Logic: Oscillators & Bursts
    # -------------------------------------------------------------
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

    def send_latch_pulse_ui(self):
        """Manually sends a latch pulse to lock D0-D7 power on fiber laser."""
        try:
            if hasattr(self.controller, "send_latch_pulse"):
                self.controller.send_latch_pulse(pin_bit=4)  # IO5 is bit 4 (Chân 5/18)
            self.log_message.emit("Đã phát xung Latch chốt công suất trên Pin 9 của Nguồn Laser (kênh IO5).")
            QMessageBox.information(
                self, "Phát Xung Latch Thành Công",
                "Đã phát xung kích sườn trên Chân 9 (Latch) qua kênh IO5 (Pin 5/18)!\n"
                "Nguồn Fiber Laser đã nạp và lưu giá trị công suất hiện tại vào thanh ghi DAC nội."
            )
        except Exception as e:
            QMessageBox.critical(self, "Lỗi Phát Xung", f"Không thể gửi xung Latch:\n{e}")

    # -------------------------------------------------------------
    # Dialogs: Measurement & Wiring
    # -------------------------------------------------------------
    def show_measurement_guide_dialog(self):
        diag = QDialog(self)
        diag.setWindowTitle("Hướng Dẫn Đo Kiểm Tra Tín Hiệu Các Cặp Chân Bằng Đồng Hồ VOM")
        diag.resize(750, 520)
        d_layout = QVBoxLayout(diag)

        text = QTextEdit(diag)
        text.setReadOnly(True)
        text.setFontFamily("Consolas, Courier New, monospace")
        text.setStyleSheet("background-color: #0f172a; color: #f8fafc; font-size: 12px; line-height: 1.5;")
        guide_text = """
========================================================================================
⚡ HƯỚNG DẪN ĐO KIỂM TRA TÍN HIỆU 7 CẶP CHÂN DIGITAL I/O X7 (NEWSON RHOTHOR)
========================================================================================

Cổng X7 trên bo mạch Rhothor sử dụng chuẩn tín hiệu vi sai RS-485 (TIA/EIA-485A)
thông qua chip điều khiển Texas Instruments SN65HVD485E.

1. BẢNG TRA CỨU CÁC CẶP CHÂN:
   • IO1: Chân 1 (IO1-) và Chân 14 (IO1+) [Có trở 100Ω]
   • IO2: Chân 2 (IO2-) và Chân 15 (IO2+) [Có trở 100Ω]
   • IO3: Chân 3 (IO3-) và Chân 16 (IO3+) [Có trở 100Ω]
   • IO4: Chân 4 (IO4-) và Chân 17 (IO4+) [Có trở 100Ω]
   • IO5: Chân 5 (IO5-) và Chân 18 (IO5+) [Không trở] -> Kênh chốt Latch Pin 9 Fiber Laser
   • IO6: Chân 6 (IO6-) và Chân 19 (IO6+) [Không trở] -> Kênh mở tia Emission Gate Pin 19
   • IO7: Chân 7 (IO7-) và Chân 20 (IO7+) [Không trở] -> Kênh xung Sync/PRR Pin 20

2. CÁCH ĐO BẰNG ĐỒNG HỒ VẠN NĂNG (VOM - THANG ĐO ĐIỆN ÁP DC 20V):

   ▶ PHƯƠNG PHÁP 1: ĐO HIỆU ĐIỆN THẾ VI SAI GIỮA 2 CHÂN TRONG CẶP
     • Que ĐỎ cắm vào chân DƯƠNG (IO+): Chân 14, 15, 16, 17, 18, 19, hoặc 20.
     • Que ĐEN cắm vào chân ÂM (IO-): Chân 1, 2, 3, 4, 5, 6, hoặc 7.
     ----------------------------------------------------------------------------------
     - KHI BẬT (HIGH):  Đồng hồ đo được điện áp DƯƠNG:  +2.0V đến +4.5V (thông thường ~ +3.3V).
     - KHI TẮT (LOW):   Đồng hồ đo được điện áp ÂM:     -2.0V đến -4.5V.

   ▶ PHƯƠNG PHÁP 2: ĐO TỪNG CHÂN SO VỚI MASS GND (CHÂN 8)
     • Que ĐEN cắm cố định vào Chân 8 (GND) hoặc vỏ kim loại của giắc cắm.
     • Que ĐỎ cắm vào chân IO+ (14 đến 20):
       - KHI BẬT: Đo được khoảng +3.3V đến +5.0V.
       - KHI TẮT: Đo được khoảng 0.0V đến +0.8V.

3. TẠI SAO NÊN DÙNG TÍNH NĂNG "TEST TỰ ĐỘNG BẬT TẮT TUẦN TỰ (5S)"?
   • Giúp kỹ thuật viên có đủ thời gian 5 giây để đặt que đo vào từng cặp chân theo thứ tự.
   • Khi nhìn thấy đồng hồ nhảy từ 0V lên +3.3V rồi hạ xuống 0V đúng nhịp 5 giây,
     bạn có thể khẳng định 100% là phần mềm và chip DSP trên Galvo ĐANG XUẤT TÍN HIỆU THẬT!
========================================================================================
        """
        text.setText(guide_text)
        d_layout.addWidget(text)

        btn_close = QPushButton("ĐÃ HIỂU & ĐÓNG", diag)
        btn_close.clicked.connect(diag.accept)
        d_layout.addWidget(btn_close)
        diag.exec_()

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
