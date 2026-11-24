"""
Tab 01: Connection & Hardware Diagnostics (Based on Example 01).
Allows the operator to connect to Rhothor USB hardware or Simulator,
and query internal parameters, channel data, and firmware state.
"""

from typing import Optional, Callable
from PyQt5.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QGridLayout, QLabel,
    QLineEdit, QPushButton, QGroupBox, QTableWidget, QTableWidgetItem,
    QHeaderView, QMessageBox
)
from PyQt5.QtCore import Qt, pyqtSignal
from PyQt5.QtGui import QFont

from sdk.python.rhothor import RhothorController


class ConnectionTab(QWidget):
    """Hardware connection and status query tab."""
    connection_changed = pyqtSignal(bool, str)  # (is_connected, mode_string)
    log_message = pyqtSignal(str)

    def __init__(self, controller: RhothorController, parent=None):
        super().__init__(parent)
        self.controller = controller
        self._init_ui()

    def _init_ui(self):
        layout = QVBoxLayout(self)
        layout.setSpacing(16)

        # 1. Device Connection Group
        conn_group = QGroupBox("1. KẾT NỐI PHẦN CỨNG (HARDWARE CONNECTION)")
        conn_layout = QGridLayout(conn_group)
        conn_layout.setSpacing(12)

        # Serial Number
        conn_layout.addWidget(QLabel("Serial Number:"), 0, 0)
        self.edit_serial = QLineEdit(str(self.controller.serial_nr or 4281))
        self.edit_serial.setPlaceholderText("4281 (LPKF)")
        conn_layout.addWidget(self.edit_serial, 0, 1)

        # Device Name
        conn_layout.addWidget(QLabel("Tên thiết bị (Name):"), 0, 2)
        self.edit_name = QLineEdit(self.controller.name or "LPKF")
        conn_layout.addWidget(self.edit_name, 0, 3)

        # Connect / Disconnect Button
        self.btn_connect = QPushButton("⚡ KẾT NỐI THIẾT BỊ (CONNECT)")
        self.btn_connect.setStyleSheet("""
            QPushButton {
                background-color: #00c853;
                color: white;
                font-weight: bold;
                padding: 10px;
                border-radius: 6px;
                font-size: 13px;
            }
            QPushButton:hover { background-color: #00e676; }
        """)
        self.btn_connect.clicked.connect(self.toggle_connection)
        conn_layout.addWidget(self.btn_connect, 1, 0, 1, 2)

        # Query Button
        self.btn_query = QPushButton("🔍 ĐỌC THÔNG SỐ (QUERY)")
        self.btn_query.setStyleSheet("""
            QPushButton {
                background-color: #2a3b5c;
                color: #00d2ff;
                font-weight: bold;
                padding: 10px;
                border-radius: 6px;
                border: 1px solid #00d2ff;
            }
            QPushButton:hover { background-color: #364d78; }
        """)
        self.btn_query.clicked.connect(self.refresh_device_info)
        conn_layout.addWidget(self.btn_query, 1, 2, 1, 2)

        layout.addWidget(conn_group)

        # 2. Hardware Parameters Table
        info_group = QGroupBox("2. THÔNG SỐ KỸ THUẬT QUÉT (RHOTHOR RT-CU DIAGNOSTICS)")
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

        # Populate initial table
        self.refresh_device_info()

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
                    padding: 10px;
                    border-radius: 6px;
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
                        padding: 10px;
                        border-radius: 6px;
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
        """Reads hardware registers and populates the table."""
        if not self.controller.is_connected:
            self.table_info.setRowCount(0)
            return

        try:
            info = self.controller.get_device_info()
            self.table_info.setRowCount(len(info))

            labels_vn = {
                "serial_number": "Số Serial Phần Cứng",
                "name": "Tên Máy Định Danh",
                "mode": "Chế Độ Vận Hành",
                "field_size_mm": "Vùng Quét Quang Học (Field Size)",
                "max_speed_mm_s": "Tốc Độ Galvo Tối Đa (Max Speed)",
                "scanner_delay_us": "Thời Gian Trễ Galvo (Scanner Delay)",
                "firmware_version": "Phiên Bản Firmware DSP",
                "status_code": "Mã Trạng Thái (Status Byte)",
                "status_name": "Tình Trạng Thiết Bị",
                "resolvers": "Độ Phân Giải Resolver (X, Y)",
                "setpoint_filter": "Bộ Lọc Setpoint Filter"
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
