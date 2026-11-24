# Bảng Tra Cứu Tham Số Chuẩn Máy LPKF #4281 (Calibration Cheat Sheet)

Bảng tổng hợp nhanh toàn bộ thông số chuẩn của đầu quét **Newson Rhothor LPKF #4281** để bạn copy-paste trực tiếp vào phần mềm `rhothor.exe`, `Config.exe`, hoặc `EzCad`.

---

## 1. Thông Số Nhận Diện Thiết Bị
* **Serial Number:** `4281`
* **Profile / Machine Name:** `LPKF`
* **Kiểu kết nối:** USB 2.0 (WinUSB) / XY2-100 Digital

---

## 2. Thông Số Động Cơ Quét Trục XY (Galvo Scanner)

| Tham Số (Parameter) | Giá Trị Chuẩn | Ghi Chú / Ý Nghĩa Kỹ Thuật |
| :--- | :---: | :--- |
| **System** | `X + Y` | Bật cả 2 kênh động cơ X và Y |
| **Scanner Type** | `RTSN - AR180` | Góc quét 180 mrad (~10.3°), động cơ Lorentz |
| **Field Size** | `201.65 mm` | Chiều dài cạnh vùng quét vuông quang học |
| **Scanner Delay** | `60 µsec` | Độ trễ bám đuôi vòng điều khiển kín (drag error) |
| **Max Speed** | `6000 mm/sec` | Tốc độ nhảy nhanh giữa các nét vẽ |
| **Mark Speed** (Gia công) | `1500 - 2500 mm/s` | Tốc độ cắt/khắc tùy công suất nguồn laser |
| **Tracking Mode** | `Normal` | Cân bằng độ chính xác và gia tốc |
| **Mirror X** | `Off (False)` | Không đảo chiều trục X |
| **Mirror Y** | `Off (False)` | Không đảo chiều trục Y |
| **Swap XY** | `Off (False)` | Kênh 1 là X, Kênh 2 là Y |

---

## 3. Thông Số Trục Z Lấy Nét Động (ELEVAthor EL-S2)

| Tham Số (Parameter) | Giá Trị Chuẩn | Ghi Chú / Ý Nghĩa Kỹ Thuật |
| :--- | :---: | :--- |
| **System Z** | `On (True)` | Bật trục lấy nét động 3D |
| **Scanner Type Z** | `EL - S2` | Thấu kính tịnh tiến tiêu cự động ELEVAthor |
| **Field Size Z** | `147.0 mm` | Dải dịch chuyển mặt phẳng tiêu cự |
| **Max Speed Z** | `4000 mm/sec` | Tốc độ dịch chuyển thấu kính Z tối đa |
| **Mirror Z** | `Off (False)` | Chiều tăng Z hướng về phía phôi |

---

## 4. Thông Số Bù Độ Trễ Bật/Tắt Laser (Laser Timing)

| Tham Số (Parameter) | Giá Trị Khuyến Nghị | Ý Nghĩa Chống Lỗi Quang Học |
| :--- | :---: | :--- |
| **Gate On Delay** | `50 - 60 µsec` | Chờ động cơ galvo đạt đủ tốc độ trước khi bật laser (tránh cháy đốm đầu nét) |
| **Gate Off Delay**| `50 - 60 µsec` | Tắt laser trước khi gương quét bắt đầu giảm tốc (tránh cháy đốm đuôi nét) |
| **Jump Delay** | `200 - 300 µsec` | Thời gian ổn định gương quét sau một cú nhảy nhanh |
| **Min Gate Period** | `10 µsec` | Khoảng thời gian đóng mở tối thiểu của xung laser |

---

## 5. Tham Số Bộ Phát Xung Phần Cứng (Oscillator) Cho Laser Q-Switch

* **Oscillator Số:** `3` (Được liên kết với chân kích xung `IO7`).
* **Chu kỳ phát xung (Period):** `20.0 µsec` (tương đương tần số lặp xung **50 kHz**).
* **Độ rộng xung (Pulse Width):** `5.0 µsec` (độ rộng xung mở Q-switch).
* *Lưu ý: Bộ dao động số 3 tự động đồng bộ theo sườn lên của tín hiệu Gate.*
