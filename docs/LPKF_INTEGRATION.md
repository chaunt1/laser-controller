# Phân Tích Kỹ Thuật Hệ Thống Rhothor LPKF (Serial #4281)

Tài liệu này phân tích chi tiết cấu hình phần cứng và phần mềm của hệ thống **Newson Rhothor™** trên máy khắc/cắt laser **LPKF** dựa trên ảnh chụp thực tế và tài liệu kỹ thuật của hãng.

---

## 1. Thông Tin Nhận Diện Phần Cứng (Từ Ảnh Chụp)

| Tham Số | Giá Trị Cấu Hình | Ý Nghĩa Kỹ Thuật |
| :--- | :--- | :--- |
| **Serial Number** | `4281` | Mã định danh duy nhất của bộ điều khiển RT-CU |
| **Machine / Name** | `LPKF` | Nhà chế tạo máy (LPKF Laser & Electronics AG) |
| **Giao Tiếp PC** | USB 2.0 (WinUSB) | Kết nối trực tiếp máy tính qua cổng X6 (cách ly quang 2.5 kV) |
| **Chế Độ Hoạt Động**| Integrated Control | Bộ điều khiển nhúng DSP 160 MIPS tự xử lý vector & phát xung laser |

---

## 2. Cấu Hình Trục Quét XY (2D Deflection)

* **Hệ thống (System):** `X+Y` (Cả 2 động cơ Galvo X và Galvo Y đều kích hoạt).
* **Loại động cơ (Scanner Type):** `RTSN - AR180`
  * Dòng động cơ **RTSN** của Newson áp dụng công nghệ **Lorenz Force** (lực từ trường Lorentz) thay vì nam châm chuyển động (moving magnet) truyền thống.
  * Giảm quán tính rotor đáng kể, triệt tiêu tổn hao nhiệt và dòng xoáy Foucault.
  * Góc quét quang học (Optical Angular Range): **180 mrad** (~10.3°).
  * Độ phân giải góc định vị: `0.002%` của dải góc quét tối đa.
  * Trôi nhiệt (Drift qua 8 giờ): `< ± 0.004%`.
* **Độ trễ quét (Scanner Delay):** `60 µsec`
  * Thời gian trễ bám đuôi (drag error) của vòng điều khiển kín (closed-loop).
  * Với khẩu độ quang học (aperture) 10mm, giá trị khuyến nghị từ hãng là $\ge 50\ \mu\text{s}$. Giá trị `60 µs` tối ưu hóa độ sắc nét tại các góc cua mà không gây rung lắc hoặc quá tải động cơ.
* **Kích thước vùng làm việc (Field Size):** `201.65 mm`
  * Quan hệ giữa tiêu cự thấu kính ($F$) và góc quét ($\theta$):
    $$\text{FieldSize} = \frac{F \times \text{Angular Range}}{1000}$$
  * Với $\theta = 180\ \text{mrad}$ và vùng quét $201.65\ \text{mm}$, hệ thống quang học được hiệu chuẩn chính xác tương đương tiêu cự quét $\approx 1120\ \text{mm}$ (hoặc qua bộ thấu kính tổ hợp F-theta).
* **Tốc độ tối đa (Max Speed):** `6000 mm/sec`
  * Tốc độ nhảy nhanh (Fast Jump) giữa các nét vẽ khi laser tắt.
* **Chế độ bám đuôi (Track Mode):** `Normal`
  * Hỗ trợ 3 chế độ: *Smooth* (rất êm, chính xác cao cho vi mạch), *Normal* (cân bằng tốc độ và độ chính xác), *Sharp* (góc cạnh cực bén ở tốc độ cao, có chút vọt lố).
* **Đảo trục & Hoán đổi:**
  * `Mirror X`: Không kích hoạt.
  * `Mirror Y`: Không kích hoạt.
  * `Swap XY`: Không kích hoạt (Channel 1 là X, Channel 2 là Y).

---

## 3. Cấu Hình Trục Z (3D Dynamic Focusing)

* **Trạng thái hệ thống (System Z):** `ON`
* **Loại thấu kính/động cơ Z (Scanner Type):** `EL - S2`
  * Bộ lấy nét động **ELEVAthor™** của Newson.
  * Sử dụng thấu kính tịnh tiến dọc trục quang học để thay đổi mặt phẳng tiêu cự liên tục trong không gian 3 chiều.
  * Khắc phục độ cong trường quét mà không nhất thiết phải dùng thấu kính F-theta phẳng đắt tiền, đồng thời cho phép khắc trên bề mặt bậc thang, dốc nghiêng hoặc biên dạng 3D lồi lõm.
* **Hành trình tiêu cự Z (Field Size Z):** `147 mm`
* **Tốc độ tối đa trục Z (Max Speed Z):** `4000 mm/sec`
* **Đảo chiều Z (Mirror Z):** Không kích hoạt.

---

## 4. Lưu Ý Quan Trọng Khi Hiệu Chuẩn

1. **Retune System After Changing Parameter:**
   Khi thay đổi bất kỳ tham số nào có dấu `(*)` (như Scanner Type, Scanner Delay, Max Speed), cần nhấn nút **Tune XY** và **Tune Z** trên thanh công cụ để bộ điều khiển DSP tự động tính toán lại ma trận hệ số PID của mạch Class-D.
2. **Lưu trữ cấu hình:**
   Sau khi tinh chỉnh, lưu cấu hình vào bộ nhớ không bay hơi (Flash) của bo mạch bằng nút **Store**, hoặc lưu ra file cấu hình trên máy tính bằng nút **Save**.
