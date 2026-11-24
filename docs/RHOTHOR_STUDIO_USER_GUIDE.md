# Sổ Tay Vận Hành Phần Mềm Rhothor Laser Studio

**Rhothor Laser Studio** là phần mềm máy tính (Desktop App) chuyên nghiệp được xây dựng trên nền tảng **Python + PyQt5**, kết hợp toàn bộ tính năng từ bộ mã nguồn mẫu (`examples/`) thành một ứng dụng hoàn chỉnh, trực quan, dễ sử dụng cho thợ vận hành máy laser và kỹ sư tự động hóa.

---

## 1. Tổng Quan Giao Diện

Giao diện phần mềm được chia làm 3 khu vực chính:
1. **Thanh tiêu đề (Top Bar)**:
   - Hiển thị logo, tên máy và trạng thái kết nối phần cứng:
     - `ONLINE: LPKF #4281 (HARDWARE)`: Đã nhận diện mạch điều khiển USB Rhothor thật.
     - `ONLINE: LPKF #4281 (SIMULATION)`: Đang chạy chế độ mô phỏng kinematics (không cần cắm máy thật).
2. **Khu vực làm việc chính (Trung tâm)**:
   - **Bên trái**: 5 tab chức năng tương ứng 5 ví dụ mẫu trong SDK.
   - **Bên phải**: Màn hình mô phỏng đồ họa vector thời gian thực (`QGraphicsView`) kèm tọa độ mm tức thời.
3. **Thanh điều khiển vận hành (Dưới cùng)**:
   - 🔍 **Nút 1: Quét Khung Định Vị Phôi (Red Frame - Laser OFF)**.
   - ⚡ **Nút 2: Bắt Đầu Khắc Laser (Start Mark)**.
   - ⛔ **Nút 3: Dừng Khẩn Cấp (Emergency Stop / Abort)**.
   - Thanh tiến độ (Progress bar) và nhật ký sự kiện hệ thống (System Event Log).

---

## 2. Chi Tiết Các Tab Chức Năng (Tương Ứng Bộ File @examples)

### Tab 1: 📐 2D Khắc Nét & Nhập Bản Vẽ CAD (Kế Thừa Example 02)
- **Hình mẫu định hình có sẵn**:
  - `Khung Viền 100x100 mm`: Khung chuẩn kiểm tra kích thước quang học.
  - `Vòng Tròn Đồng Tâm`: 4 vòng bán kính 10, 20, 30, 40 mm (sử dụng lệnh `ArcTo`).
  - `Tâm Căn Chỉnh Crosshair`: Chữ thập định vị tâm (0,0).
  - `Ô Đan Lưới (Hatch)`: Lưới vạch dày bước 1.0 mm kiểm tra độ đều nhiệt.
  - `Tổ Hợp Tất Cả Mẫu Thử Example 02`: Nạp toàn bộ bài test Example 02 chỉ với 1 click.
- **Nhập file DXF / PLT từ máy tính**:
  - Nhấn `Mở file DXF / PLT...` để nạp bản vẽ từ AutoCAD, CorelDRAW, Illustrator hoặc EzCad.
  - Tự động hiển thị kích thước phôi (Dài x Rộng mm), số lượng đường nét và tổng quãng đường chạy tia.
- **Căn chỉnh & Biến đổi hình học**:
  - `Tỉ lệ (Scale)`: Thu nhỏ / phóng to kích thước bản vẽ.
  - `Dịch X / Y`: Di chuyển vị trí khắc trên bàn máy.
  - `Xoay 90°`: Đổi hướng phôi tức thời.
  - `Căn Giữa (Center)`: Đưa trọng tâm bản vẽ về đúng tâm quang học (0, 0).
- **Thông số khắc**:
  - `Tốc độ khắc (Mark Speed)`: 100 – 6000 mm/s.
  - `Tốc độ nhảy (Jump Speed)`: 500 – 8000 mm/s.
  - `Số lượt khắc (Passes)`: 1 – 100 lần.
  - `Xuất File SVG Mô Phỏng`: Xuất vector và thông số động học ra file SVG vector tiêu chuẩn.

---

### Tab 2: 🏔️ Tiêu Cự Động 3D (Kế Thừa Example 03: ELEVAthor EL-S2)
Dành cho máy tích hợp trục Z tiêu cự quang học động **Newson ELEVAthor EL-S2** (phạm vi Z 147 mm, tốc độ 4000 mm/s):
- **Bề mặt bậc thang (Stepped Planes)**:
  - Khắc 3 hình vuông đồng tâm trên 3 mặt phẳng tiêu cự khác nhau (ví dụ: Z = -10 mm, Z = 0 mm, Z = +10 mm) mà không cần di chuyển bàn nâng cơ khí.
- **Đường dốc nghiêng 3D liên tục (Continuous Sloped Ramp)**:
  - Chạy tia laser liên tục từ tọa độ `(X1, Y1, Z1)` sang `(X2, Y2, Z2)` với thấu kính Z thay đổi tiêu cự bám sát góc nghiêng phôi.
- **Hình nón xoắn ốc 3D (Spiral Cone)**:
  - Khắc đường xoắn ốc đa chiều liên tục.
- **Hiển thị 3D Gradient**:
  - Trên màn hình đồ họa, các đường nét được đổi màu theo độ sâu Z:
    - **Màu Xanh Dương**: Tọa độ Z âm (tiêu cự thụt vào).
    - **Màu Xanh Lá**: Tọa độ Z = 0 (mặt phẳng tiêu cự chuẩn).
    - **Màu Đỏ**: Tọa độ Z dương (tiêu cự nhô ra).

---

### Tab 3: ⏱️ Hiệu Chuẩn Độ Trễ Tia Laser (Kế Thừa Example 04)
Hiệu chuẩn thời gian đóng/mở cổng quang để triệt tiêu hiện tượng cháy đốm hoặc hụt nét:
- **Tham số**:
  - `Gate On Delay`: Trễ phát laser sau khi gương galvo bắt đầu tăng tốc.
  - `Gate Off Delay`: Trễ tắt laser trước khi gương galvo dừng lại.
- **Tạo bảng test ma trận tự động (5 Levels)**:
  - Nhấn `Tạo Tập Lệnh Test Ma Trận 5 Mức Trễ` để tạo 5 vạch kẻ thử nghiệm ứng với 20 µs, 40 µs, 60 µs, 80 µs, 100 µs.
  - Soi kính hiển vi đầu mút vạch khắc để chọn giá trị micro-giây tối ưu cho từng loại vật liệu.

---

### Tab 4: ⚡ Q-Switch & Digital I/O (Kế Thừa Example 05)
Cấu hình phát xung laser công suất cao và điều khiển thiết bị ngoại vi qua cổng DB25:
- **Bộ dao động xung phần cứng (Oscillator)**:
  - Kích hoạt Oscillator 1, 2 hoặc 3 (Osc 3 xuất tín hiệu trigger ra chân IO7 cho nguồn laser Q-switched / MOPA).
  - Cài đặt Chu kỳ xung (Period µs) và Độ rộng xung (Pulse width µs).
- **Bắn xung khoan điểm tĩnh (Burst Drilling)**:
  - Bắn xung laser tại chỗ với thời gian tính bằng micro-giây và khoảng làm nguội giữa các điểm.
- **Bảng điều khiển 8-Bit Digital I/O**:
  - **Output**: Bật/tắt trực tiếp Khí nén thổi xỉ (Air Assist), Quạt hút khói (Exhaust), Đèn laser đỏ định vị (Red Guide), Đèn báo sẵn sàng (Pilot Lamp).
  - **Input**: Đèn LED xanh/trắng giám sát cảm biến an toàn: Cửa buồng gia công (Interlock), Bàn đạp chân (Footswitch), Cảm biến nước làm mát (Chiller Flow OK), Tín hiệu sẵn sàng từ nguồn laser.

---

### Tab 5: 🔌 Kết Nối & Đọc Tham Số Phần Cứng (Kế Thừa Example 01)
- Nhập số Serial thiết bị (mặc định: `4281` cho hệ thống LPKF).
- Nút **Kết nối / Ngắt kết nối** an toàn.
- Nút **Đọc thông số (Query)**: Hiển thị bảng chi tiết:
  - Vùng quét quang học (201.65 mm).
  - Tốc độ tối đa (6000 mm/s).
  - Độ trễ scanner delay (60 µs).
  - Phiên bản firmware DSP.
  - Thanh ghi trạng thái Status byte.

---

## 3. Quy Trình Vận Hành Tiêu Chuẩn 3 Bước Cho Công Nhân

1. **Bước 1 - Nạp Bản Vẽ**:
   - Vào Tab 2D, chọn file `.dxf` hoặc `.plt`.
   - Nhấn `Căn Giữa (Center)` và điều chỉnh `Tỉ lệ (Scale)` cho vừa kích thước sản phẩm.
2. **Bước 2 - Định Vị Phôi (Khung Đỏ)**:
   - Đặt phôi gia công lên bàn máy.
   - Nhấn nút màu cam: **`🔍 1. QUÉT KHUNG ĐỊNH VỊ PHÔI`**.
   - Đầu quét sẽ chạy bao quanh hình chữ nhật phôi (Laser TẮT hoàn toàn) để căn chỉnh vị trí phôi thực tế.
3. **Bước 3 - Khắc Laser**:
   - Đóng cửa chắn bảo hộ an toàn.
   - Nhấn nút màu xanh: **`⚡ 2. BẮT ĐẦU KHẮC LASER`**.
   - Phần mềm sẽ tự động gửi lệnh đôi bộ đệm (double-buffered) xuống DSP của Rhothor và báo hoàn tất khi xong.
   - Trong trường hợp khẩn cấp, nhấn **`⛔ DỪNG KHẨN CẤP`** (hoặc phím ESC).
