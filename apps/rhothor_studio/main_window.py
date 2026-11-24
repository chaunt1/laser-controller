"""
Main Window for Rhothor Laser Studio.
Integrates all tabs, interactive vector canvas, execution workers,
and diagnostic log console into a professional dark-themed workstation.
"""

from typing import List, Dict, Any, Optional
from PyQt5.QtWidgets import (
    QMainWindow, QWidget, QVBoxLayout, QHBoxLayout, QSplitter,
    QTabWidget, QLabel, QPushButton, QProgressBar, QTextEdit,
    QStatusBar, QMessageBox, QFrame
)
from PyQt5.QtCore import Qt, pyqtSlot
import os
from PyQt5.QtGui import QFont, QColor, QIcon

from sdk.python.rhothor import RhothorController
from .canvas import LaserCanvasView
from .workers import MarkWorker, FrameWorker
from .tabs.tab_connection import ConnectionTab
from .tabs.tab_cnc_preview import CNCPreviewTab
from .tabs.tab_2d_marking import Marking2DTab
from .tabs.tab_3d_dynamic import Dynamic3DTab
from .tabs.tab_timing import TimingTab
from .tabs.tab_io_trigger import IOTriggerTab


class MainWindow(QMainWindow):
    """Primary application window for Rhothor Laser Studio."""

    def __init__(self, serial: int = 4281, name: str = "LPKF", parent=None):
        super().__init__(parent)
        self.setWindowTitle("Rhothor Laser Studio v1.1.0 - LPKF Galvo Controller")
        self.resize(1340, 840)
        self.setMinimumSize(1024, 700)

        # Set application icon if available
        icon_path = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "resources", "app_icon.png"))
        if os.path.exists(icon_path):
            self.setWindowIcon(QIcon(icon_path))

        # Initialize shared hardware controller
        self.controller = RhothorController(serial_nr=serial, name=name)
        try:
            self.controller.connect()
        except Exception:
            pass

        self.mark_worker: Optional[MarkWorker] = None
        self.frame_worker: Optional[FrameWorker] = None
        self.active_strokes: List[List[Dict[str, Any]]] = []
        self.active_bbox: Optional[Dict[str, float]] = None

        self._apply_dark_theme()
        self._init_ui()

    def _apply_dark_theme(self):
        """Applies a modern industrial dark stylesheet."""
        self.setStyleSheet("""
            QMainWindow, QWidget {
                background-color: #0f1115;
                color: #e2e8f0;
                font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Arial, sans-serif;
                font-size: 12px;
            }
            QGroupBox {
                border: 1px solid #2d3345;
                border-radius: 6px;
                margin-top: 12px;
                padding-top: 14px;
                font-weight: bold;
                color: #38bdf8;
            }
            QGroupBox::title {
                subcontrol-origin: margin;
                left: 10px;
                padding: 0 4px;
            }
            QTabWidget::pane {
                border: 1px solid #2d3345;
                background-color: #171920;
                border-radius: 6px;
            }
            QTabBar::tab {
                background-color: #1c1f28;
                color: #94a3b8;
                padding: 10px 18px;
                border: 1px solid #2d3345;
                border-bottom: none;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
                font-weight: 600;
            }
            QTabBar::tab:selected {
                background-color: #171920;
                color: #00d2ff;
                border-top: 2px solid #00d2ff;
            }
            QTabBar::tab:hover:!selected {
                background-color: #262a36;
                color: #e2e8f0;
            }
            QPushButton {
                background-color: #262a36;
                color: #e2e8f0;
                border: 1px solid #3b4252;
                border-radius: 4px;
                padding: 6px 12px;
                font-weight: 500;
            }
            QPushButton:hover {
                background-color: #363c4e;
                border-color: #00d2ff;
            }
            QPushButton:pressed {
                background-color: #1a1d26;
            }
            QLineEdit, QSpinBox, QDoubleSpinBox, QComboBox {
                background-color: #1a1c24;
                border: 1px solid #2d3345;
                border-radius: 4px;
                padding: 5px 8px;
                color: #f8fafc;
            }
            QLineEdit:focus, QSpinBox:focus, QDoubleSpinBox:focus, QComboBox:focus {
                border-color: #00d2ff;
            }
            QProgressBar {
                border: 1px solid #2d3345;
                border-radius: 4px;
                text-align: center;
                background-color: #171920;
                color: #ffffff;
                font-weight: bold;
            }
            QProgressBar::chunk {
                background-color: #00c853;
                border-radius: 3px;
            }
        """)

    def _init_ui(self):
        central_widget = QWidget()
        self.setCentralWidget(central_widget)
        main_layout = QVBoxLayout(central_widget)
        main_layout.setContentsMargins(12, 10, 12, 10)
        main_layout.setSpacing(10)

        # 1. Top Bar
        top_bar = QHBoxLayout()

        brand_lbl = QLabel("⚡ RHOTHOR LASER STUDIO v1.1.0")
        brand_lbl.setStyleSheet("font-size: 16px; font-weight: 900; color: #00d2ff; letter-spacing: 0.5px;")
        top_bar.addWidget(brand_lbl)

        sub_lbl = QLabel("LPKF #4281 | Publisher: chaunt <chaunt.dev@gmail.com>")
        sub_lbl.setStyleSheet("color: #64748b; font-size: 11px; padding-left: 10px;")
        top_bar.addWidget(sub_lbl)

        top_bar.addStretch()

        self.pill_status = QLabel("ONLINE (LPKF #4281)")
        self._update_status_pill(True, "SIMULATION" if self.controller.simulate else "HARDWARE")
        top_bar.addWidget(self.pill_status)

        main_layout.addLayout(top_bar)

        # 2. Main Splitter: Tabs (Left) & Canvas + Stats (Right)
        splitter = QSplitter(Qt.Horizontal)

        # Left Container: TabWidget
        self.tabs = QTabWidget()
        self.tab_conn = ConnectionTab(self.controller, self)
        self.tab_cnc = CNCPreviewTab(self.controller, self)
        self.tab_2d = Marking2DTab(self.controller, self)
        self.tab_3d = Dynamic3DTab(self.controller, self)
        self.tab_timing = TimingTab(self.controller, self)
        self.tab_io = IOTriggerTab(self.controller, self)

        self.tabs.addTab(self.tab_conn, "🔌 1. Test Kết Nối (Diagnostics)")
        self.tabs.addTab(self.tab_cnc, "📂 2. Bản Vẽ CNC & Preview (CAD/CAM)")
        self.tabs.addTab(self.tab_2d, "📐 3. Khắc Nét 2D (Ex 02)")
        self.tabs.addTab(self.tab_3d, "🏔️ 4. Tiêu Cự Động 3D (Ex 03)")
        self.tabs.addTab(self.tab_timing, "⏱️ 5. Hiệu Chuẩn Trễ (Ex 04)")
        self.tabs.addTab(self.tab_io, "⚡ 6. Q-Switch & I/O (Ex 05)")

        splitter.addWidget(self.tabs)

        # Right Container: Canvas View + Toolbar + Metrics
        right_container = QWidget()
        right_layout = QVBoxLayout(right_container)
        right_layout.setContentsMargins(0, 0, 0, 0)
        right_layout.setSpacing(6)

        # Canvas Toolbar
        canvas_bar = QHBoxLayout()
        self.btn_fit = QPushButton("⛶ Vừa Vùng Quét")
        self.btn_fit.clicked.connect(lambda: self.canvas.fit_scan_field())
        canvas_bar.addWidget(self.btn_fit)

        self.btn_zoom_in = QPushButton("🔍 +")
        self.btn_zoom_in.clicked.connect(lambda: self.canvas.scale(1.25, 1.25))
        canvas_bar.addWidget(self.btn_zoom_in)

        self.btn_zoom_out = QPushButton("🔍 -")
        self.btn_zoom_out.clicked.connect(lambda: self.canvas.scale(0.8, 0.8))
        canvas_bar.addWidget(self.btn_zoom_out)

        canvas_bar.addStretch()

        self.lbl_hud = QLabel("X: 0.00 mm | Y: 0.00 mm")
        self.lbl_hud.setStyleSheet("color: #38bdf8; font-family: monospace; font-size: 11px;")
        canvas_bar.addWidget(self.lbl_hud)

        right_layout.addLayout(canvas_bar)

        # Interactive Canvas
        self.canvas = LaserCanvasView(field_size_mm=201.65, parent=self)
        self.canvas.coordinates_changed.connect(self._on_canvas_coords)
        right_layout.addWidget(self.canvas)

        # Simulation Statistics Bar
        self.lbl_stats = QLabel("Quãng đường khắc: 0 mm | Quãng đường nhảy: 0 mm | Thời gian ước tính: 0.00 s")
        self.lbl_stats.setStyleSheet("background-color: #171920; color: #94a3b8; padding: 6px; border-radius: 4px; font-size: 11px;")
        right_layout.addWidget(self.lbl_stats)

        splitter.addWidget(right_container)
        splitter.setStretchFactor(0, 5)
        splitter.setStretchFactor(1, 6)
        main_layout.addWidget(splitter, 1)

        # 3. Bottom Master Execution Controls & Logs
        bottom_box = QFrame()
        bottom_box.setStyleSheet("background-color: #171920; border: 1px solid #2d3345; border-radius: 6px; padding: 6px;")
        bottom_layout = QVBoxLayout(bottom_box)
        bottom_layout.setSpacing(8)

        btn_row = QHBoxLayout()

        # Frame Button (Laser OFF)
        self.btn_frame = QPushButton("🔍 1. QUÉT KHUNG ĐỊNH VỊ PHÔI (RED FRAME - LASER OFF)")
        self.btn_frame.setStyleSheet("""
            QPushButton {
                background-color: #b45309;
                color: white;
                font-weight: bold;
                padding: 12px;
                border-radius: 6px;
                font-size: 13px;
            }
            QPushButton:hover { background-color: #d97706; }
        """)
        self.btn_frame.clicked.connect(self.start_frame_trace)
        btn_row.addWidget(self.btn_frame, 3)

        # Start Mark Button
        self.btn_mark = QPushButton("⚡ 2. BẮT ĐẦU KHẮC LASER (START MARK)")
        self.btn_mark.setStyleSheet("""
            QPushButton {
                background-color: #15803d;
                color: white;
                font-weight: bold;
                padding: 12px;
                border-radius: 6px;
                font-size: 14px;
            }
            QPushButton:hover { background-color: #16a34a; }
        """)
        self.btn_mark.clicked.connect(self.start_laser_mark)
        btn_row.addWidget(self.btn_mark, 4)

        # Emergency Stop
        self.btn_stop = QPushButton("⛔ DỪNG KHẨN CẤP (STOP / ABORT)")
        self.btn_stop.setStyleSheet("""
            QPushButton {
                background-color: #b91c1c;
                color: white;
                font-weight: bold;
                padding: 12px;
                border-radius: 6px;
                font-size: 13px;
            }
            QPushButton:hover { background-color: #dc2626; }
        """)
        self.btn_stop.clicked.connect(self.emergency_abort)
        btn_row.addWidget(self.btn_stop, 2)

        bottom_layout.addLayout(btn_row)

        # Progress bar
        self.progress_bar = QProgressBar()
        self.progress_bar.setRange(0, 100)
        self.progress_bar.setValue(0)
        self.progress_bar.setFixedHeight(14)
        bottom_layout.addWidget(self.progress_bar)

        # Collapsible Log Text Box
        self.log_box = QTextEdit()
        self.log_box.setReadOnly(True)
        self.log_box.setMaximumHeight(80)
        self.log_box.setStyleSheet("background-color: #0b0c10; color: #64748b; font-family: monospace; font-size: 10px;")
        bottom_layout.addWidget(self.log_box)

        main_layout.addWidget(bottom_box)

        # Wire cross-tab signals
        self.tab_cnc.vectors_changed.connect(self._on_vectors_updated)
        self.tab_cnc.frame_changed.connect(self.canvas.set_frame_preview)
        self.tab_cnc.log_message.connect(self.log)
        self.tab_2d.vectors_changed.connect(self._on_vectors_updated)
        self.tab_2d.log_message.connect(self.log)
        self.tab_3d.vectors_changed.connect(self._on_vectors_updated)
        self.tab_3d.log_message.connect(self.log)
        self.tab_timing.vectors_changed.connect(self._on_vectors_updated)
        self.tab_timing.log_message.connect(self.log)
        self.tab_io.vectors_changed.connect(self._on_vectors_updated)
        self.tab_io.log_message.connect(self.log)
        self.tab_conn.vectors_changed.connect(self._on_vectors_updated)
        self.tab_conn.connection_changed.connect(self._update_status_pill)
        self.tab_conn.log_message.connect(self.log)

        self.log("Rhothor Laser Studio initialized. Ready for operations.")

    def _update_status_pill(self, is_connected: bool, mode: str):
        if is_connected:
            self.pill_status.setText(f"ONLINE: {self.controller.name} #{self.controller.serial_nr} ({mode})")
            self.pill_status.setStyleSheet("""
                background-color: rgba(0, 230, 118, 0.15);
                color: #00e676;
                border: 1px solid #00e676;
                padding: 4px 10px;
                border-radius: 12px;
                font-weight: bold;
                font-size: 11px;
            """)
        else:
            self.pill_status.setText("DISCONNECTED (OFFLINE)")
            self.pill_status.setStyleSheet("""
                background-color: rgba(255, 61, 87, 0.15);
                color: #ff3d57;
                border: 1px solid #ff3d57;
                padding: 4px 10px;
                border-radius: 12px;
                font-weight: bold;
                font-size: 11px;
            """)

    @pyqtSlot(list, object)
    def _on_vectors_updated(self, strokes: List[List[Dict[str, Any]]], bbox: Optional[Dict[str, float]]):
        self.active_strokes = strokes
        self.active_bbox = bbox
        self.canvas.load_strokes(strokes, bbox)

        # Update stats
        if strokes:
            total_mark = 0.0
            total_jump = 0.0
            for stroke in strokes:
                last_pt = None
                for pt in stroke:
                    if last_pt:
                        import math
                        dist = math.hypot(pt["x"] - last_pt["x"], pt["y"] - last_pt["y"])
                        if pt.get("type") == "JUMP":
                            total_jump += dist
                        else:
                            total_mark += dist
                    last_pt = pt

            est_time = (total_mark / 2000.0) + (total_jump / 6000.0)
            self.lbl_stats.setText(
                f"Quãng đường khắc: {total_mark:.1f} mm | Quãng đường nhảy: {total_jump:.1f} mm | Thời gian ước tính: {est_time:.2f} s"
            )
        else:
            self.lbl_stats.setText("Chưa nạp nét vẽ")

    @pyqtSlot(float, float)
    def _on_canvas_coords(self, x: float, y: float):
        self.lbl_hud.setText(f"X: {x:.2f} mm | Y: {y:.2f} mm")

    def log(self, msg: str):
        self.log_box.append(f"> {msg}")
        self.log_box.verticalScrollBar().setValue(self.log_box.verticalScrollBar().maximum())

    def start_frame_trace(self):
        """Traces the workpiece boundary frame."""
        if not self.active_bbox:
            QMessageBox.warning(self, "Chưa có bản vẽ", "Vui lòng chọn hoặc nạp bản vẽ trước khi quét khung!")
            return

        if self.frame_worker and self.frame_worker.isRunning():
            self.frame_worker.stop()
            self.btn_frame.setText("🔍 1. QUÉT KHUNG ĐỊNH VỊ PHÔI (RED FRAME - LASER OFF)")
            return

        self.btn_frame.setText("⏹️ DỪNG QUÉT KHUNG...")
        self.canvas.set_frame_preview(self.active_bbox)

        self.frame_worker = FrameWorker(
            controller=self.controller,
            bbox=self.active_bbox,
            speed=2500.0,
            continuous=False,
            parent=self
        )
        self.frame_worker.status_msg.connect(self.log)
        self.frame_worker.finished.connect(self._on_frame_finished)
        self.frame_worker.error.connect(lambda e: QMessageBox.critical(self, "Lỗi quét khung", e))
        self.frame_worker.start()

    def _on_frame_finished(self):
        self.btn_frame.setText("🔍 1. QUÉT KHUNG ĐỊNH VỊ PHÔI (RED FRAME - LASER OFF)")
        self.canvas.set_frame_preview(None)
        self.log("Khung định vị hoàn tất.")

    def start_laser_mark(self):
        """Executes the active toolpath with the laser."""
        if not self.active_strokes:
            QMessageBox.warning(self, "Chưa có nét vẽ", "Vui lòng nạp bản vẽ hoặc chọn hình mẫu trước khi bắt đầu!")
            return

        if self.mark_worker and self.mark_worker.isRunning():
            QMessageBox.information(self, "Đang khắc", "Thiết bị đang thực hiện tác vụ khắc laser!")
            return

        # Fetch job parameters from active 2D tab
        params = self.tab_2d.get_job_parameters()

        self.btn_mark.setEnabled(False)
        self.btn_mark.setText("⏳ ĐANG KHẮC LASER...")
        self.progress_bar.setValue(0)

        self.mark_worker = MarkWorker(
            controller=self.controller,
            strokes=self.active_strokes,
            passes=params["passes"],
            mark_speed=params["mark_speed"],
            jump_speed=params["jump_speed"],
            offset_x=params["offset_x"],
            offset_y=params["offset_y"],
            scale=params["scale"],
            parent=self
        )
        self.mark_worker.progress.connect(self._on_mark_progress)
        self.mark_worker.status_msg.connect(self.log)
        self.mark_worker.finished.connect(self._on_mark_finished)
        self.mark_worker.error.connect(self._on_mark_error)
        self.mark_worker.start()

    def _on_mark_progress(self, curr: int, total: int):
        pct = int((curr / total) * 100)
        self.progress_bar.setValue(pct)

    def _on_mark_finished(self, stats: dict):
        self.btn_mark.setEnabled(True)
        self.btn_mark.setText("⚡ 2. BẮT ĐẦU KHẮC LASER (START MARK)")
        self.progress_bar.setValue(100)
        dur = stats.get("actual_elapsed_seconds", 0)
        self.log(f"✅ KHẮC HOÀN TẤT: {stats.get('total_commands', 0)} lệnh, thời gian: {dur:.2f} s")

    def _on_mark_error(self, err_msg: str):
        self.btn_mark.setEnabled(True)
        self.btn_mark.setText("⚡ 2. BẮT ĐẦU KHẮC LASER (START MARK)")
        QMessageBox.critical(self, "Lỗi Khắc Laser", f"Đã xảy ra lỗi:\n{err_msg}")

    def emergency_abort(self):
        """Emergency stop: immediately halts galvos and laser."""
        if self.mark_worker and self.mark_worker.isRunning():
            self.mark_worker.abort()
        if self.frame_worker and self.frame_worker.isRunning():
            self.frame_worker.stop()

        try:
            self.controller.abort()
        except Exception:
            pass

        self.btn_mark.setEnabled(True)
        self.btn_mark.setText("⚡ 2. BẮT ĐẦU KHẮC LASER (START MARK)")
        self.btn_frame.setText("🔍 1. QUÉT KHUNG ĐỊNH VỊ PHÔI (RED FRAME - LASER OFF)")
        self.canvas.set_frame_preview(None)
        self.progress_bar.setValue(0)
        self.log("⛔ EMERGENCY STOP ACTIVATED. All motions halted.")
        QMessageBox.warning(self, "Dừng Khẩn Cấp", "ĐÃ KÍCH HOẠT DỪNG KHẨN CẤP!\nĐầu quét và nguồn laser đã bị ngắt.")

    def closeEvent(self, event):
        """Cleanly releases hardware when closing window."""
        if self.mark_worker and self.mark_worker.isRunning():
            self.mark_worker.abort()
            self.mark_worker.wait(1000)
        if self.controller.is_connected:
            self.controller.disconnect()
        event.accept()
