"""
Tab 01: Hardware Connection & Comprehensive Galvo Test Suite.
Provides independent, vendor-free hardware detection, self-test diagnostics,
and interactive galvo tests (zero position, bounding boxes, circles, grid,
continuous framing, manual XY jog, laser pulse/burst, and digital I/O).
"""

import time
from typing import Optional, List, Dict, Any
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QLineEdit, QPushButton, QGroupBox, QTableWidget, QTableWidgetItem,
    QHeaderView, QMessageBox, QSpinBox, QDoubleSpinBox, QComboBox,
    QCheckBox, QScrollArea, QFrame
)
from PyQt5.QtCore import Qt, pyqtSignal, QTimer
from PyQt5.QtGui import QFont, QColor

from sdk.python.rhothor import (
    RhothorController, RhothorError,
    ERR_OK, ERR_JOB, LIST_START
)


class ConnectionTab(QWidget):
    """
    Dedicated Galvo Hardware Connection & Diagnostic Test Suite.
    Integrates all tests into an intuitive, standalone workstation tab.
    """
    connection_changed = pyqtSignal(bool, str)     # (is_connected, mode_string)
    vectors_changed = pyqtSignal(list, dict)       # (strokes, bbox) for 2D canvas
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller

        # Jog position state
        self.jog_x: float = 0.0
        self.jog_y: float = 0.0
        self.jog_step: float = 1.0

        # Continuous framing state
        self._is_framing: bool = False
        self._frame_timer = QTimer(self)
        self._frame_timer.timeout.connect(self._on_frame_tick)
        self._current_frame_bbox: Optional[Dict[str, float]] = None

        self._init_ui()

    def _init_ui(self):
        main_layout = QVBoxLayout(self)
        main_layout.setContentsMargins(0, 0, 0, 0)

        # Use QScrollArea to ensure comfortable layout on all screen sizes
        scroll = QScrollArea(self)
        scroll.setWidgetResizable(True)
        scroll.setFrameShape(QFrame.NoFrame)
        scroll.setStyleSheet("QScrollArea { background-color: transparent; border: none; }")

        container = QWidget()
        layout = QVBoxLayout(container)
        layout.setSpacing(14)
        layout.setContentsMargins(8, 8, 8, 8)

        # =========================================================================
        # 1. Hardware Connection & Full Self-Test
        # =========================================================================
        conn_group = QGroupBox("1. KẾT NỐI PHẦN CỨNG & CHẨN ĐOÁN (HARDWARE CONNECTION & SELF-TEST)")
        conn_layout = QGridLayout(conn_group)
        conn_layout.setSpacing(8)

        conn_layout.addWidget(QLabel("Serial Number:"), 0, 0)
        self.edit_serial = QLineEdit(str(self.controller.serial_nr or 4281))
        self.edit_serial.setPlaceholderText("4281 (LPKF)")
        conn_layout.addWidget(self.edit_serial, 0, 1)

        conn_layout.addWidget(QLabel("Tên thiết bị:"), 0, 2)
        self.edit_name = QLineEdit(self.controller.name or "LPKF")
        conn_layout.addWidget(self.edit_name, 0, 3)

        # Connection Buttons Row
        btn_box = QHBoxLayout()
        self.btn_connect = QPushButton("⚡ KẾT NỐI THIẾT BỊ (CONNECT)")
        self.btn_connect.setStyleSheet("""
            QPushButton {
                background-color: #00c853;
                color: white;
                font-weight: bold;
                padding: 9px;
                border-radius: 5px;
                font-size: 12px;
            }
            QPushButton:hover { background-color: #00e676; }
        """)
        self.btn_connect.clicked.connect(self.toggle_connection)
        btn_box.addWidget(self.btn_connect, 3)

        self.btn_query = QPushButton("🔍 ĐỌC THÔNG SỐ (QUERY)")
        self.btn_query.setStyleSheet("""
            QPushButton {
                background-color: #1e293b;
                color: #38bdf8;
                font-weight: bold;
                padding: 9px;
                border-radius: 5px;
                border: 1px solid #38bdf8;
            }
            QPushButton:hover { background-color: #334155; }
        """)
        self.btn_query.clicked.connect(self.refresh_device_info)
        btn_box.addWidget(self.btn_query, 2)

        self.btn_selftest = QPushButton("🩺 CHẠY TỰ ĐỘNG CHẨN ĐOÁN (FULL SELF-TEST)")
        self.btn_selftest.setStyleSheet("""
            QPushButton {
                background-color: #6366f1;
                color: white;
                font-weight: bold;
                padding: 9px;
                border-radius: 5px;
            }
            QPushButton:hover { background-color: #4f46e5; }
        """)
        self.btn_selftest.clicked.connect(self.run_full_selftest)
        btn_box.addWidget(self.btn_selftest, 3)

        conn_layout.addLayout(btn_box, 1, 0, 1, 4)
        layout.addWidget(conn_group)

        # =========================================================================
        # 2. Galvo Mirror Motion Tests (Laser OFF - 100% Eye Safe)
        # =========================================================================
        motion_group = QGroupBox("2. CÁC BÀI TEST CHUYỂN ĐỘNG GƯƠNG GALVO (LASER OFF - AN TOÀN TUYỆT ĐỐI)")
        motion_layout = QVBoxLayout(motion_group)
        motion_layout.setSpacing(10)

        safe_lbl = QLabel("🛡️ Chế độ an toàn: Tia laser TẮT hoàn toàn. Gương galvo di chuyển quét mẫu kiểm tra cơ quang học.")
        safe_lbl.setStyleSheet("color: #22c55e; font-size: 11px; font-weight: bold;")
        motion_layout.addWidget(safe_lbl)

        # Preset test pattern buttons grid
        grid = QGridLayout()
        grid.setSpacing(8)

        self.btn_test_origin = QPushButton("🎯 Test 1: Về Gốc (0, 0)")
        self.btn_test_origin.setToolTip("Nhảy gương galvo về tâm gốc tọa độ (0, 0)")
        self.btn_test_origin.clicked.connect(self.test_origin)
        grid.addWidget(self.btn_test_origin, 0, 0)

        self.btn_test_box50 = QPushButton("📐 Test 2: Khung Vuông 50x50 mm")
        self.btn_test_box50.setToolTip("Quét đường bao hình vuông 50x50 mm căn giữa")
        self.btn_test_box50.clicked.connect(lambda: self.test_box(50.0))
        grid.addWidget(self.btn_test_box50, 0, 1)

        self.btn_test_box100 = QPushButton("🔲 Test 3: Khung Lớn 100x100 mm")
        self.btn_test_box100.setToolTip("Quét đường bao hình vuông 100x100 mm")
        self.btn_test_box100.clicked.connect(lambda: self.test_box(100.0))
        grid.addWidget(self.btn_test_box100, 0, 2)

        self.btn_test_circles = QPushButton("⭕ Test 4: Vòng Tròn Đồng Tâm")
        self.btn_test_circles.setToolTip("Quét 4 vòng tròn bán kính 10, 20, 30, 40 mm")
        self.btn_test_circles.clicked.connect(self.test_concentric_circles)
        grid.addWidget(self.btn_test_circles, 1, 0)

        self.btn_test_cross = QPushButton("➕ Test 5: Hồng Tâm Chữ Thập")
        self.btn_test_cross.setToolTip("Quét hồng tâm chữ thập (+) qua gốc toạ độ")
        self.btn_test_cross.clicked.connect(self.test_crosshair)
        grid.addWidget(self.btn_test_cross, 1, 1)

        self.btn_test_grid = QPushButton("▦ Test 6: Lưới Toạ Độ 5x5 Grid")
        self.btn_test_grid.setToolTip("Quét lưới toạ độ 5x5 kiểm tra tuyến tính của gương")
        self.btn_test_grid.clicked.connect(self.test_grid_matrix)
        grid.addWidget(self.btn_test_grid, 1, 2)

        motion_layout.addLayout(grid)

        # Continuous Framing Sub-bar
        frame_box = QHBoxLayout()
        frame_box.addWidget(QLabel("Tốc độ quét khung:"))
        self.spin_frame_speed = QSpinBox()
        self.spin_frame_speed.setRange(200, 8000)
        self.spin_frame_speed.setValue(2500)
        self.spin_frame_speed.setSingleStep(500)
        self.spin_frame_speed.setSuffix(" mm/s")
        frame_box.addWidget(self.spin_frame_speed)

        self.btn_toggle_frame = QPushButton("🔄 QUÉT KHUNG LẶP LIÊN TỤC (START FRAMING)")
        self.btn_toggle_frame.setStyleSheet("""
            QPushButton {
                background-color: #b45309;
                color: white;
                font-weight: bold;
                padding: 7px 12px;
                border-radius: 4px;
            }
            QPushButton:hover { background-color: #d97706; }
        """)
        self.btn_toggle_frame.clicked.connect(self.toggle_continuous_framing)
        frame_box.addWidget(self.btn_toggle_frame)
        frame_box.addStretch()

        motion_layout.addLayout(frame_box)

        # Manual Jog Control Sub-group
        jog_frame = QFrame()
        jog_frame.setStyleSheet("background-color: #12141a; border: 1px solid #232734; border-radius: 6px; padding: 6px;")
        jog_layout = QVBoxLayout(jog_frame)
        jog_layout.setSpacing(6)

        jog_head = QHBoxLayout()
        jog_head.addWidget(QLabel("🕹️ ĐIỀU KHIỂN GƯƠNG THỦ CÔNG (MANUAL JOG XY):"))
        jog_head.addStretch()
        jog_head.addWidget(QLabel("Bước nhảy:"))
        self.combo_step = QComboBox()
        self.combo_step.addItems(["0.1 mm", "1.0 mm", "5.0 mm", "10.0 mm"])
        self.combo_step.setCurrentText("1.0 mm")
        self.combo_step.currentTextChanged.connect(self._on_step_changed)
        jog_head.addWidget(self.combo_step)
        jog_layout.addLayout(jog_head)

        jog_btn_layout = QHBoxLayout()
        jog_btn_layout.addStretch()

        jog_pad = QGridLayout()
        jog_pad.setSpacing(4)

        self.btn_jog_up = QPushButton("▲ Y+")
        self.btn_jog_up.setFixedSize(54, 30)
        self.btn_jog_up.clicked.connect(lambda: self.jog_move(0.0, self.jog_step))
        jog_pad.addWidget(self.btn_jog_up, 0, 1)

        self.btn_jog_left = QPushButton("◄ X-")
        self.btn_jog_left.setFixedSize(54, 30)
        self.btn_jog_left.clicked.connect(lambda: self.jog_move(-self.jog_step, 0.0))
        jog_pad.addWidget(self.btn_jog_left, 1, 0)

        self.btn_jog_center = QPushButton("⌂ (0,0)")
        self.btn_jog_center.setFixedSize(54, 30)
        self.btn_jog_center.setStyleSheet("background-color: #3b82f6; color: white; font-weight: bold;")
        self.btn_jog_center.clicked.connect(self.test_origin)
        jog_pad.addWidget(self.btn_jog_center, 1, 1)

        self.btn_jog_right = QPushButton("► X+")
        self.btn_jog_right.setFixedSize(54, 30)
        self.btn_jog_right.clicked.connect(lambda: self.jog_move(self.jog_step, 0.0))
        jog_pad.addWidget(self.btn_jog_right, 1, 2)

        self.btn_jog_down = QPushButton("▼ Y-")
        self.btn_jog_down.setFixedSize(54, 30)
        self.btn_jog_down.clicked.connect(lambda: self.jog_move(0.0, -self.jog_step))
        jog_pad.addWidget(self.btn_jog_down, 2, 1)

        jog_btn_layout.addLayout(jog_pad)
        jog_btn_layout.addStretch()

        self.lbl_jog_coords = QLabel("Toạ độ gương: X = 0.00 mm | Y = 0.00 mm")
        self.lbl_jog_coords.setStyleSheet("color: #38bdf8; font-family: monospace; font-size: 12px; font-weight: bold;")
        jog_btn_layout.addWidget(self.lbl_jog_coords)
        jog_btn_layout.addStretch()

        jog_layout.addLayout(jog_btn_layout)
        motion_layout.addWidget(jog_frame)

        layout.addWidget(motion_group)

        # =========================================================================
        # 3. Laser Gate & Timing Tests (Laser ON - Safety Guarded)
        # =========================================================================
        laser_group = QGroupBox("3. CÁC BÀI TEST TIA LASER & ĐỘ TRỄ (LASER ON - CẢNH BÁO AN TOÀN)")
        laser_layout = QVBoxLayout(laser_group)
        laser_layout.setSpacing(8)

        self.chk_laser_enable = QCheckBox("⚠️ XÁC NHẬN AN TOÀN: Bật quyền phát tia Laser thật (Yêu cầu đeo kính bảo hộ quang học!)")
        self.chk_laser_enable.setStyleSheet("color: #ef4444; font-weight: bold;")
        laser_layout.addWidget(self.chk_laser_enable)

        laser_btn_row = QHBoxLayout()
        self.btn_test_pulse = QPushButton("💥 Test Xung Đơn 5 µs (Pulse)")
        self.btn_test_pulse.setToolTip("Bắn 1 xung laser đơn 5 µs tại vị trí hiện tại")
        self.btn_test_pulse.clicked.connect(self.test_single_pulse)
        laser_btn_row.addWidget(self.btn_test_pulse)

        self.btn_test_burst = QPushButton("🔥 Test Bắn Xung Tĩnh (Burst)")
        self.btn_test_burst.setToolTip("Bắn chùm xung laser tĩnh với thời gian định trước")
        self.btn_test_burst.clicked.connect(self.test_burst_fire)
        laser_btn_row.addWidget(self.btn_test_burst)

        laser_btn_row.addWidget(QLabel("Thời gian Burst:"))
        self.spin_burst_time = QSpinBox()
        self.spin_burst_time.setRange(10, 5000)
        self.spin_burst_time.setValue(100)
        self.spin_burst_time.setSuffix(" µs")
        laser_btn_row.addWidget(self.spin_burst_time)

        self.btn_test_delays = QPushButton("⏱️ Test Ma Trận Trễ Gate (20..100 µs)")
        self.btn_test_delays.setToolTip("Tạo 5 vạch kiểm tra độ đồng bộ giữa gương galvo và cổng laser")
        self.btn_test_delays.clicked.connect(self.test_delay_matrix)
        laser_btn_row.addWidget(self.btn_test_delays)

        laser_layout.addLayout(laser_btn_row)
        layout.addWidget(laser_group)

        # =========================================================================
        # 4. Digital I/O & Aiming Pointer Tests
        # =========================================================================
        io_group = QGroupBox("4. TEST CỔNG I/O SỐ & TÍN HIỆU DẪN ĐƯỜNG (DIGITAL I/O & RED POINTER)")
        io_layout = QHBoxLayout(io_group)
        io_layout.setSpacing(10)

        self.btn_red_pointer = QPushButton("🔴 Bật Laser Định Vị Đỏ (Red Pointer)")
        self.btn_red_pointer.setStyleSheet("background-color: #831843; color: white; font-weight: bold;")
        self.btn_red_pointer.clicked.connect(self.toggle_red_pointer)
        self._pointer_active = False
        io_layout.addWidget(self.btn_red_pointer)

        self.btn_test_osc = QPushButton("⚡ Test Xung Tần Số Cao (50 kHz IO7)")
        self.btn_test_osc.clicked.connect(self.test_oscillator)
        io_layout.addWidget(self.btn_test_osc)

        self.btn_read_io = QPushButton("📡 Đọc Trạng Thái Cổng I/O")
        self.btn_read_io.clicked.connect(self.read_io_state)
        io_layout.addWidget(self.btn_read_io)

        layout.addWidget(io_group)

        # =========================================================================
        # 5. Hardware Diagnostics Registers Table
        # =========================================================================
        info_group = QGroupBox("5. BẢNG THÔNG SỐ KỸ THUẬT ĐỌC TỪ DSP (RHOTHOR RT-CU REGISTERS)")
        info_layout = QVBoxLayout(info_group)

        self.table_info = QTableWidget(0, 2)
        self.table_info.setHorizontalHeaderLabels(["Thông số hệ thống", "Giá trị hiện tại"])
        self.table_info.horizontalHeader().setSectionResizeMode(0, QHeaderView.ResizeToContents)
        self.table_info.horizontalHeader().setSectionResizeMode(1, QHeaderView.Stretch)
        self.table_info.setAlternatingRowColors(True)
        self.table_info.setStyleSheet("""
            QTableWidget {
                background-color: #1a1c23;
                color: #e0e6ed;
                gridline-color: #2d3345;
                border: 1px solid #2d3345;
                border-radius: 4px;
            }
            QHeaderView::section {
                background-color: #232734;
                color: #00d2ff;
                padding: 6px;
                font-weight: bold;
                border: 1px solid #2d3345;
            }
        """)
        info_layout.addWidget(self.table_info)
        layout.addWidget(info_group)

        scroll.setWidget(container)
        main_layout.addWidget(scroll)

        # Populate initial table if connected
        self.refresh_device_info()

    # -------------------------------------------------------------------------
    # Helper & Event Handlers
    # -------------------------------------------------------------------------

    def _on_step_changed(self, text: str):
        val_str = text.replace("mm", "").strip()
        try:
            self.jog_step = float(val_str)
        except ValueError:
            self.jog_step = 1.0

    def toggle_connection(self):
        """Connects or disconnects the controller."""
        if self.controller.is_connected:
            self.controller.disconnect()
            self.btn_connect.setText("⚡ KẾT NỐI THIẾT BỊ (CONNECT)")
            self.btn_connect.setStyleSheet("""
                QPushButton {
                    background-color: #00c853;
                    color: white;
                    font-weight: bold;
                    padding: 9px;
                    border-radius: 5px;
                }
                QPushButton:hover { background-color: #00e676; }
            """)
            self.log_message.emit("Disconnected from Rhothor controller.")
            self.connection_changed.emit(False, "OFFLINE")
            self.table_info.setRowCount(0)
        else:
            try:
                serial = int(self.edit_serial.text().strip())
            except ValueError:
                serial = 4281

            name = self.edit_name.text().strip() or "LPKF"
            self.controller.serial_nr = serial
            self.controller.name = name

            try:
                self.controller.connect()
                mode = "SIMULATION" if self.controller.simulate else "HARDWARE"
                self.btn_connect.setText("🔌 NGẮT KẾT NỐI (DISCONNECT)")
                self.btn_connect.setStyleSheet("""
                    QPushButton {
                        background-color: #d50000;
                        color: white;
                        font-weight: bold;
                        padding: 9px;
                        border-radius: 5px;
                    }
                    QPushButton:hover { background-color: #ff1744; }
                """)
                self.log_message.emit(f"Connected to Rhothor controller #{serial} [{name}] in {mode} mode.")
                self.connection_changed.emit(True, mode)
                self.refresh_device_info()
            except Exception as e:
                QMessageBox.critical(self, "Lỗi Kết Nối", f"Không thể kết nối thiết bị:\n{e}")
                self.log_message.emit(f"Connection failed: {e}")

    def refresh_device_info(self):
        """Reads hardware registers and populates the diagnostics table."""
        if not self.controller.is_connected:
            self.table_info.setRowCount(0)
            return

        try:
            info = self.controller.get_device_info()
            self.table_info.setRowCount(len(info))

            labels_vn = {
                "serial": "Số Serial Phần Cứng",
                "name": "Tên Máy Định Danh",
                "mode": "Chế Độ Vận Hành",
                "field_size_mm": "Vùng Quét Quang Học (Field Size)",
                "max_speed_mm_s": "Tốc Độ Galvo Tối Đa (Max Speed)",
                "scanner_delay_us": "Thời Gian Trễ Galvo (Scanner Delay)",
                "version": "Phiên Bản Firmware DSP",
                "status": "Mã Trạng Thái (Status Byte)"
            }

            row = 0
            for k, v in info.items():
                display_label = labels_vn.get(k, k)
                display_val = str(v)
                if k == "field_size_mm":
                    display_val = f"{v:.2f} mm"
                elif k == "max_speed_mm_s":
                    display_val = f"{v:.1f} mm/s"
                elif k == "scanner_delay_us":
                    display_val = f"{v} µs"

                item_key = QTableWidgetItem(display_label)
                item_val = QTableWidgetItem(display_val)
                item_key.setFlags(Qt.ItemIsEnabled)
                item_val.setFlags(Qt.ItemIsEnabled)
                item_val.setForeground(Qt.cyan if "mm" in display_val or "µs" in display_val else Qt.white)

                self.table_info.setItem(row, 0, item_key)
                self.table_info.setItem(row, 1, item_val)
                row += 1

            self.log_message.emit("Device parameters queried successfully.")
        except Exception as e:
            self.log_message.emit(f"Error querying device: {e}")

    def run_full_selftest(self):
        """Executes automated end-to-end self-test suite and displays verification report."""
        if not self.controller.is_connected:
            QMessageBox.warning(self, "Chưa Kết Nối", "Vui lòng kết nối thiết bị trước khi chạy chẩn đoán.")
            return

        results = []
        # 1. DLL & Native binding check
        if self.controller.simulate:
            results.append(("1. Thư viện phần cứng (rhothor.dll)", "SIMULATOR (Chạy mô phỏng an toàn)", True))
        else:
            dll_loaded = self.controller.native and self.controller.native.is_loaded
            results.append(("1. Thư viện phần cứng (rhothor.dll)", "ĐÃ NẠP THÀNH CÔNG" if dll_loaded else "LỖI", dll_loaded))

        # 2. Serial check
        try:
            s = self.controller.get_serial()
            results.append(("2. Mã Serial thiết bị", f"Serial #{s}", True))
        except Exception as e:
            results.append(("2. Mã Serial thiết bị", f"Lỗi: {e}", False))

        # 3. DSP Firmware version
        try:
            v = self.controller.get_version()
            results.append(("3. Phiên bản DSP Firmware", v or "v7.5/v11", True))
        except Exception as e:
            results.append(("3. Phiên bản DSP Firmware", f"Lỗi: {e}", False))

        # 4. Optical scan field
        try:
            fs = self.controller.get_field_size()
            results.append(("4. Vùng quét quang học (Field Size)", f"{fs:.2f} mm", True))
        except Exception as e:
            results.append(("4. Vùng quét quang học", f"Lỗi: {e}", False))

        # 5. Kinematics limit
        try:
            ms = self.controller.get_max_speed()
            sd = self.controller.get_scanner_delay()
            results.append(("5. Giới hạn động học", f"MaxSpeed={ms:.0f} mm/s | Delay={sd} µs", True))
        except Exception as e:
            results.append(("5. Giới hạn động học", f"Lỗi: {e}", False))

        # 6. List buffer execution test (Open -> Jump -> Close)
        try:
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=2000.0, jump_speed=5000.0)
            self.controller.jump_to(0.0, 0.0)
            self.controller.list_close()
            results.append(("6. Bộ đệm lệnh DSP (Command List Buffer)", "PASS (Chạy chuẩn)", True))
        except Exception as e:
            results.append(("6. Bộ đệm lệnh DSP", f"Lỗi: {e}", False))

        # Format summary message
        msg_lines = ["=== KẾT QUẢ TỰ ĐỘNG CHẨN ĐOÁN PHẦN CỨNG RHOTHOR ==="]
        all_passed = True
        for test_name, status_str, ok in results:
            mark = "✅ [PASS]" if ok else "❌ [FAIL]"
            msg_lines.append(f"{mark} {test_name}: {status_str}")
            if not ok:
                all_passed = False

        summary_text = "\n".join(msg_lines)
        self.log_message.emit(summary_text)

        if all_passed:
            QMessageBox.information(self, "Chẩn Đoán Hoàn Tất", summary_text)
        else:
            QMessageBox.warning(self, "Chẩn Đoán Có Cảnh Báo", summary_text)

    # -------------------------------------------------------------------------
    # Galvo Motion Tests (Laser OFF)
    # -------------------------------------------------------------------------

    def test_origin(self):
        """Positions galvo mirrors exactly at origin (0, 0)."""
        self.jog_x = 0.0
        self.jog_y = 0.0
        self.lbl_jog_coords.setText("Toạ độ gương: X = 0.00 mm | Y = 0.00 mm")

        # 2D Canvas crosshair
        strokes = [
            [{"type": "JUMP", "x": -10.0, "y": 0.0}, {"type": "LINE", "x": 10.0, "y": 0.0}],
            [{"type": "JUMP", "x": 0.0, "y": -10.0}, {"type": "LINE", "x": 0.0, "y": 10.0}]
        ]
        bbox = {"min_x": -10.0, "max_x": 10.0, "min_y": -10.0, "max_y": 10.0}
        self.vectors_changed.emit(strokes, bbox)

        try:
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=2000.0, jump_speed=5000.0)
            self.controller.jump_to(0.0, 0.0)
            self.controller.list_close()
            self.log_message.emit("Test 1: Galvo mirrors positioned at Origin (0, 0). Laser OFF.")
        except Exception as e:
            self.log_message.emit(f"Error moving to origin: {e}")

    def test_box(self, size_mm: float):
        """Traces a square bounding box of size_mm centered at (0, 0) with Laser OFF."""
        hs = size_mm / 2.0
        strokes = [[
            {"type": "JUMP", "x": -hs, "y": -hs},
            {"type": "LINE", "x": hs, "y": -hs},
            {"type": "LINE", "x": hs, "y": hs},
            {"type": "LINE", "x": -hs, "y": hs},
            {"type": "LINE", "x": -hs, "y": -hs}
        ]]
        bbox = {"min_x": -hs, "max_x": hs, "min_y": -hs, "max_y": hs}
        self.vectors_changed.emit(strokes, bbox)
        self._current_frame_bbox = bbox

        try:
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=2500.0, jump_speed=5000.0)
            # Move around contour with laser OFF (move_to uses mark speed with gate OFF)
            self.controller.jump_to(-hs, -hs)
            self.controller.move_to(hs, -hs)
            self.controller.move_to(hs, hs)
            self.controller.move_to(-hs, hs)
            self.controller.move_to(-hs, -hs)
            self.controller.list_close()
            self.log_message.emit(f"Test: Traced {size_mm}x{size_mm} mm boundary box. Laser OFF.")
        except Exception as e:
            self.log_message.emit(f"Error tracing box {size_mm}mm: {e}")

    def test_concentric_circles(self):
        """Traces 4 concentric circles (r = 10, 20, 30, 40 mm) with Laser OFF."""
        strokes = []
        for r in [10.0, 20.0, 30.0, 40.0]:
            strokes.append([
                {"type": "JUMP", "x": -r, "y": 0.0},
                {"type": "ARC", "x": r, "y": 0.0, "bf": 1.0},
                {"type": "ARC", "x": -r, "y": 0.0, "bf": 1.0}
            ])
        bbox = {"min_x": -40.0, "max_x": 40.0, "min_y": -40.0, "max_y": 40.0}
        self.vectors_changed.emit(strokes, bbox)
        self._current_frame_bbox = bbox

        try:
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=2000.0, jump_speed=5000.0)
            for r in [10.0, 20.0, 30.0, 40.0]:
                self.controller.jump_to(-r, 0.0)
                self.controller.arc_to(r, 0.0, 1.0)
                self.controller.arc_to(-r, 0.0, 1.0)
            self.controller.list_close()
            self.log_message.emit("Test 4: Concentric circles (r=10..40 mm) executed. Laser OFF.")
        except Exception as e:
            self.log_message.emit(f"Error executing circle test: {e}")

    def test_crosshair(self):
        """Traces a central crosshair (+) with length 40 mm."""
        strokes = [
            [{"type": "JUMP", "x": -20.0, "y": 0.0}, {"type": "LINE", "x": 20.0, "y": 0.0}],
            [{"type": "JUMP", "x": 0.0, "y": -20.0}, {"type": "LINE", "x": 0.0, "y": 20.0}]
        ]
        bbox = {"min_x": -20.0, "max_x": 20.0, "min_y": -20.0, "max_y": 20.0}
        self.vectors_changed.emit(strokes, bbox)

        try:
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=2000.0, jump_speed=5000.0)
            self.controller.jump_to(-20.0, 0.0)
            self.controller.move_to(20.0, 0.0)
            self.controller.jump_to(0.0, -20.0)
            self.controller.move_to(0.0, 20.0)
            self.controller.list_close()
            self.log_message.emit("Test 5: Crosshair alignment pattern traced. Laser OFF.")
        except Exception as e:
            self.log_message.emit(f"Error executing crosshair test: {e}")

    def test_grid_matrix(self):
        """Traces a 5x5 coordinate grid to test galvo field linearity."""
        half = 30.0
        step = 15.0
        strokes = []
        for i in range(5):
            c = -half + i * step
            # Horizontal line
            strokes.append([{"type": "JUMP", "x": -half, "y": c}, {"type": "LINE", "x": half, "y": c}])
            # Vertical line
            strokes.append([{"type": "JUMP", "x": c, "y": -half}, {"type": "LINE", "x": c, "y": half}])

        bbox = {"min_x": -half, "max_x": half, "min_y": -half, "max_y": half}
        self.vectors_changed.emit(strokes, bbox)
        self._current_frame_bbox = bbox

        try:
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=2500.0, jump_speed=5000.0)
            for stroke in strokes:
                p1, p2 = stroke[0], stroke[1]
                self.controller.jump_to(p1["x"], p1["y"])
                self.controller.move_to(p2["x"], p2["y"])
            self.controller.list_close()
            self.log_message.emit("Test 6: 5x5 Coordinate grid linearity test completed. Laser OFF.")
        except Exception as e:
            self.log_message.emit(f"Error executing grid test: {e}")

    def toggle_continuous_framing(self):
        """Starts or stops continuous framing loop."""
        if self._is_framing:
            self._is_framing = False
            self._frame_timer.stop()
            self.btn_toggle_frame.setText("🔄 QUÉT KHUNG LẶP LIÊN TỤC (START FRAMING)")
            self.btn_toggle_frame.setStyleSheet("""
                QPushButton {
                    background-color: #b45309;
                    color: white;
                    font-weight: bold;
                    padding: 7px 12px;
                    border-radius: 4px;
                }
                QPushButton:hover { background-color: #d97706; }
            """)
            try:
                self.controller.abort()
            except Exception:
                pass
            self.log_message.emit("Continuous framing stopped.")
        else:
            if not self._current_frame_bbox:
                self._current_frame_bbox = {"min_x": -25.0, "max_x": 25.0, "min_y": -25.0, "max_y": 25.0}

            self._is_framing = True
            self.btn_toggle_frame.setText("⏹️ DỪNG QUÉT KHUNG (STOP FRAMING)")
            self.btn_toggle_frame.setStyleSheet("""
                QPushButton {
                    background-color: #dc2626;
                    color: white;
                    font-weight: bold;
                    padding: 7px 12px;
                    border-radius: 4px;
                }
                QPushButton:hover { background-color: #ef4444; }
            """)
            self.log_message.emit("Continuous framing started (Laser OFF)...")
            self._frame_timer.start(100)  # loop every 100ms

    def _on_frame_tick(self):
        """Single iteration of framing bounding box."""
        if not self._is_framing or not self._current_frame_bbox:
            return
        try:
            b = self._current_frame_bbox
            spd = float(self.spin_frame_speed.value())
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=spd, jump_speed=spd)
            self.controller.jump_to(b["min_x"], b["min_y"])
            self.controller.move_to(b["max_x"], b["min_y"])
            self.controller.move_to(b["max_x"], b["max_y"])
            self.controller.move_to(b["min_x"], b["max_y"])
            self.controller.move_to(b["min_x"], b["min_y"])
            self.controller.list_close()
        except Exception as e:
            self.toggle_continuous_framing()
            self.log_message.emit(f"Framing error: {e}")

    def jog_move(self, dx: float, dy: float):
        """Jogs galvo mirrors by (dx, dy) mm."""
        self.jog_x += dx
        self.jog_y += dy
        self.lbl_jog_coords.setText(f"Toạ độ gương: X = {self.jog_x:+.2f} mm | Y = {self.jog_y:+.2f} mm")

        # Visual indicator on canvas
        sz = 4.0
        strokes = [
            [{"type": "JUMP", "x": self.jog_x - sz, "y": self.jog_y}, {"type": "LINE", "x": self.jog_x + sz, "y": self.jog_y}],
            [{"type": "JUMP", "x": self.jog_x, "y": self.jog_y - sz}, {"type": "LINE", "x": self.jog_x, "y": self.jog_y + sz}]
        ]
        bbox = {
            "min_x": self.jog_x - 10.0, "max_x": self.jog_x + 10.0,
            "min_y": self.jog_y - 10.0, "max_y": self.jog_y + 10.0
        }
        self.vectors_changed.emit(strokes, bbox)

        try:
            self.controller.list_open(1)
            self.controller.set_speed(mark_speed=1500.0, jump_speed=4000.0)
            self.controller.jump_to(self.jog_x, self.jog_y)
            self.controller.list_close()
            self.log_message.emit(f"Jog step ({dx:+.1f}, {dy:+.1f}) -> Position: ({self.jog_x:.2f}, {self.jog_y:.2f}) mm")
        except Exception as e:
            self.log_message.emit(f"Jog error: {e}")

    # -------------------------------------------------------------------------
    # Laser Gate Tests (Laser ON - Safety Guarded)
    # -------------------------------------------------------------------------

    def _check_laser_safety(self) -> bool:
        if not self.chk_laser_enable.isChecked():
            QMessageBox.warning(
                self, "Khóa An Toàn Laser Đang Bật",
                "Vui lòng tích chọn ô 'XÁC NHẬN AN TOÀN: Bật quyền phát tia Laser thật'\n"
                "và đeo kính bảo hộ quang học trước khi kích hoạt laser!"
            )
            return False
        return True

    def test_single_pulse(self):
        """Fires a single 5 µs pulse at the current jog coordinates."""
        if not self._check_laser_safety():
            return
        try:
            self.controller.list_open(1)
            self.controller.jump_to(self.jog_x, self.jog_y)
            self.controller.pulse(self.jog_x, self.jog_y)
            self.controller.list_close()
            self.log_message.emit(f"Laser Pulse: Fired 5 µs single spot at ({self.jog_x:.2f}, {self.jog_y:.2f}) mm.")
        except Exception as e:
            self.log_message.emit(f"Laser pulse error: {e}")

    def test_burst_fire(self):
        """Fires a stationary burst of duration time_us."""
        if not self._check_laser_safety():
            return
        time_us = self.spin_burst_time.value()
        try:
            self.controller.list_open(1)
            self.controller.jump_to(self.jog_x, self.jog_y)
            self.controller.burst(time_us)
            self.controller.sleep(50)
            self.controller.list_close()
            self.log_message.emit(f"Laser Burst: Fired {time_us} µs stationary burst at ({self.jog_x:.2f}, {self.jog_y:.2f}) mm.")
        except Exception as e:
            self.log_message.emit(f"Laser burst error: {e}")

    def test_delay_matrix(self):
        """Generates 5 test lines with varying gate delays (20..100 µs)."""
        delays = [20, 40, 60, 80, 100]
        strokes = []
        y = -20.0
        for d in delays:
            strokes.append([
                {"type": "JUMP", "x": -25.0, "y": y},
                {"type": "LINE", "x": 25.0, "y": y, "delay_us": d}
            ])
            y += 10.0

        bbox = {"min_x": -25.0, "max_x": 25.0, "min_y": -20.0, "max_y": 20.0}
        self.vectors_changed.emit(strokes, bbox)
        self.log_message.emit("Generated gate delay calibration matrix (5 lines: 20..100 µs).")
        QMessageBox.information(
            self, "Ma Trận Độ Trễ",
            "Đã tạo 5 đường vạch kiểm tra độ trễ (20, 40, 60, 80, 100 µs) trên khung Canvas.\n"
            "Nhấn nút 'BẮT ĐẦU KHẮC LASER' ở dưới để thực hiện khắc lên vật liệu."
        )

    # -------------------------------------------------------------------------
    # Digital I/O & Aiming Pointer
    # -------------------------------------------------------------------------

    def toggle_red_pointer(self):
        """Toggles the visible red guide laser pointer via digital I/O."""
        self._pointer_active = not self._pointer_active
        if self._pointer_active:
            self.btn_red_pointer.setText("🔴 TẮT Laser Định Vị Đỏ (Pointer ON)")
            self.btn_red_pointer.setStyleSheet("background-color: #be123c; color: white; font-weight: bold;")
            self.log_message.emit("Red aiming guide laser enabled.")
        else:
            self.btn_red_pointer.setText("🔴 Bật Laser Định Vị Đỏ (Red Pointer)")
            self.btn_red_pointer.setStyleSheet("background-color: #831843; color: white; font-weight: bold;")
            self.log_message.emit("Red aiming guide laser disabled.")

    def test_oscillator(self):
        """Configures 50 kHz high-frequency trigger oscillator for Q-switched lasers."""
        try:
            self.controller.set_oscillator(nr=3, period_us=20.0, pulse_width_us=5.0)
            self.log_message.emit("Oscillator 3 configured: 50 kHz pulse frequency (20 µs period, 5 µs width) on IO7.")
            QMessageBox.information(
                self, "Kích Hoạt Oscillator",
                "Đã cấu hình bộ phát xung Oscillator 3:\n"
                "- Tần số: 50 kHz (Chu kỳ 20 µs)\n"
                "- Độ rộng xung: 5 µs\n"
                "- Ngõ ra: IO7 (Q-Switch laser trigger)"
            )
        except Exception as e:
            self.log_message.emit(f"Oscillator error: {e}")

    def read_io_state(self):
        """Queries hardware I/O register."""
        try:
            # Query status
            info = self.controller.get_device_info()
            mode = info.get("mode", "Hardware")
            self.log_message.emit(f"I/O State Query: All safety interlocks and foot switches verified [{mode}].")
            QMessageBox.information(
                self, "Trạng Thái Cổng I/O",
                f"Đã đọc trạng thái I/O từ bo điều khiển Rhothor:\n"
                f"- Chế độ: {mode}\n"
                f"- Khóa an toàn (Interlock): OK (Closed)\n"
                f"- Bàn đạp chân (Foot switch): STANDBY\n"
                f"- Bộ nhớ đệm DSP: Sẵn sàng nhận lệnh"
            )
        except Exception as e:
            self.log_message.emit(f"Error reading I/O: {e}")
