# Tài Liệu Tra Cứu Toàn Diện API Thư Viện rhothor.dll

Thư viện **`rhothor.dll`** (cung cấp cả 2 phiên bản 32-bit và 64-bit) là giao diện lập trình C/C++ chuẩn (Standard C API, calling convention `__stdcall`) cho phép các ứng dụng phần mềm giao tiếp trực tiếp với hệ thống quét laser Rhothor.

---

## 1. Mã Lỗi Trả Về (Return Error Codes)

Tất cả các hàm trong thư viện đều trả về kiểu số nguyên `long`:

| Mã Lỗi | Tên Hằng Số | Ý Nghĩa Kỹ Thuật |
| :---: | :--- | :--- |
| **`-1`** | `ERR_OK` | Thực thi thành công / Lệnh được thêm vào hàng đợi |
| **`2`** | `ERR_BUSY` | Hệ thống đang bận thực thi tác vụ quét trước đó |
| **`3`** | `ERR_JOB` | Lỗi tác vụ / Hàng đợi rỗng / Chưa gọi `rtListOpen` |
| **`5`** | `ERR_HARDWARE` | Phần cứng không phản hồi (Mất kết nối USB hoặc mất nguồn 12V) |
| **`13`** | `ERR_DATA` | Dữ liệu hoặc tham số đầu vào không hợp lệ |
| **`15`** | `ERR_MEMORY` | Bộ đệm DSP bị tràn, không thể cấp phát thêm bộ nhớ |
| **`23`** | `ERR_IMPLEMENTATION`| Hàm không được hỗ trợ bởi phiên bản phần cứng/firmware hiện tại |
| **`42`** | `ERR_LASER_LOW` | Công suất laser phản hồi đo được quá thấp |
| **`43`** | `ERR_TRACKING` | Hệ thống động cơ galvo đang dừng bám vị trí (Cần gọi `rtInitialise`) |
| **`46`** | `ERR_LASER_HIGH`| Công suất laser phản hồi đo được vượt ngưỡng an toàn |

---

## 2. Nhóm Hàm Kết Nối & Khởi Tạo (Accessing Functions)

### `rtInitialise()`
```c
long rtInitialise();
```
* **Mô tả:** Khởi tạo bộ điều khiển DSP, bật vòng điều khiển kín servo cho các động cơ Galvo X, Y, Z.
* **Bắt buộc:** Phải được gọi đầu tiên trước khi gọi bất kỳ hàm điều khiển hoặc truy vấn nào khác.

### `rtSelectDevice(long SerialNr)`
```c
long rtSelectDevice(long SerialNr);
```
* **Mô tả:** Chọn thiết bị đích khi có nhiều đầu quét kết nối cùng lúc với máy tính qua cổng USB.
* **Tham số:** `SerialNr` - Số serial của bo mạch (ví dụ: `4281`).

### `rtGetFreeDevList(long* SerialNrs, long* Size)`
```c
long rtGetFreeDevList(long* SerialNrs, long* Size);
```
* **Mô tả:** Lấy danh sách số serial của tất cả các thiết bị Rhothor đang kết nối qua USB và chưa bị ứng dụng khác chiếm quyền.

---

## 3. Nhóm Hàm Truy Vấn Trạng Thái (Query Functions)

| Tên Hàm | Tham Số | Đơn Vị | Mô Tả |
| :--- | :--- | :---: | :--- |
| `rtGetSerial` | `long* Serial` | - | Lấy số serial của thiết bị đang kết nối |
| `rtGetVersion` | `char* Version, long MaxSize` | Chuỗi | Lấy phiên bản firmware và DSP của thiết bị |
| `rtGetFieldSize` | `double* Size` | mm | Lấy kích thước vùng quét vuông (ví dụ: 201.65 mm) |
| `rtGetMaxSpeed` | `double* Speed` | mm/s | Lấy tốc độ nhảy tối đa của gương quét |
| `rtGetScannerDelay` | `long* Delay` | µs | Lấy độ trễ đáp ứng của động cơ galvo |
| `rtGetStatus` | `long* Memory` | Bytes | Lấy dung lượng bộ nhớ đệm còn trống trên bo DSP |
| `rtGetIO` | `long* Value` | Bitmask | Đọc trạng thái logic hiện tại của 7 chân Digital I/O |
| `rtGetChannelData` | `long nr, long* Setpoint, long* Actual, double* Ieff` | Bits, A | Đọc vị trí đặt, vị trí thực tế và dòng điện cuộn dây của kênh động cơ (1=X, 2=Y, 3=Z) |

---

## 4. Nhóm Hàm Quản Lý Hàng Đợi Lệnh (List Management)

Để đảm bảo động cơ quét chuyển động liên tục, không bị khựng hoặc dừng đột ngột giữa chừng, các lệnh vẽ vector được đóng gói vào danh sách lệnh (Command List):

### `rtListOpen(long Mode)`
* **Chế độ `LIST_START` (`1`):** Chế độ đệm kép (Double Buffering). Ứng dụng nạp các lệnh vào danh sách 1 trong khi bo mạch đang thực thi danh sách 2. Nét cắt chỉ bắt đầu khi gọi `rtListClose()`.
* **Chế độ `AUTO_START` (`2`):** Chế độ hàng đợi FIFO trực tiếp. Lệnh được truyền xuống bo mạch và thực thi ngay lập tức.
* **Chế độ `AUTO_REPEAT` (`3`):** Nạp toàn bộ chuỗi lệnh vào bộ nhớ Flash tĩnh để chạy lặp lại chu kỳ gia công độc lập.

### `rtListClose()`
* Đóng danh sách lệnh và kích hoạt động cơ thực thi ngay lập tức.

### `rtAbort()`
* Dừng khẩn cấp: Tắt chùm tia laser ngay lập tức, triệt tiêu mọi lệnh còn tồn trong hàng đợi và hãm dừng động cơ.

---

## 5. Nhóm Hàm Vẽ Vector & Điều Khiển Động Học (Motion Commands)

### `rtJumpTo(double X, double Y)` / `rtJumpTo3D(double X, double Y, double Z)`
* Di chuyển nhanh đến tọa độ đích với **LASER TẮT**. Sử dụng tốc độ `JumpSpeed`.

### `rtLineTo(double X, double Y)` / `rtLineTo3D(double X, double Y, double Z)`
* Cắt/khắc đường thẳng từ vị trí hiện tại đến tọa độ đích với **LASER BẬT**. Sử dụng tốc độ `MarkSpeed`.

### `rtArcTo(double X, double Y, double BF)`
* Khắc cung tròn từ vị trí hiện tại đến tọa độ $(X, Y)$ với **LASER BẬT**.
* `BF` (Bulge Factor): Hệ số uốn cong của cung tròn $= \tan(\theta / 4)$. Giá trị dương uốn ngược chiều kim đồng hồ, âm uốn cùng chiều kim đồng hồ. Bằng `1.0` tạo thành nửa hình tròn ($180^\circ$).

### `rtMoveTo(double X, double Y)` / `rtMoveTo3D(double X, double Y, double Z)`
* Di chuyển trơn đến tọa độ đích ở tốc độ `MarkSpeed` với **LASER TẮT**.

### `rtPulse(double X, double Y)` / `rtPulse3D(double X, double Y, double Z)`
* Di chuyển đến tọa độ đích và phát đúng một xung laser cực ngắn ($5\ \mu\text{s}$).

### `rtBurst(long Time)`
* Đứng yên tại vị trí hiện tại và bật laser liên tục trong khoảng thời gian `Time` (đơn vị micro-giây, $\mu\text{s}$) để khoan lỗ hoặc đốt sâu.

### `rtSleep(long Time)`
* Tạm dừng trong khoảng thời gian `Time` ($\mu\text{s}$) với laser tắt (dùng để chờ làm mát phôi hoặc đồng bộ hóa).

---

## 6. Nhóm Hàm Cài Đặt Tham Số Laser & Tọa Độ

| Tên Hàm | Cú Pháp | Mô Tả |
| :--- | :--- | :--- |
| `rtSetSpeed` | `rtSetSpeed(double Speed)` | Đặt tốc độ khắc nét thẳng / cong (mm/giây) |
| `rtSetJumpSpeed`| `rtSetJumpSpeed(double Speed)` | Đặt tốc độ nhảy không khắc giữa các nét (mm/giây) |
| `rtSetLaserTimes`| `rtSetLaserTimes(long GateOn, long GateOff)` | Cài đặt độ trễ bật/tắt chùm tia laser ($\mu\text{s}$) |
| `rtSetLaser` | `rtSetLaser(bool OnOff)` | Bật/tắt thủ công chùm tia laser |
| `rtSetMatrix` | `rtSetMatrix(double a11, a12, a21, a22)` | Ma trận biến đổi affine (co giãn tỉ lệ, xoay góc, kéo xiên) |
| `rtSetOffsXY` | `rtSetOffsXY(double X, double Y)` | Đặt gốc tọa độ dịch chuyển $X, Y$ |
| `rtSetOffsZ` | `rtSetOffsZ(double Z)` | Đặt gốc tọa độ dịch chuyển tiêu cự $Z$ |
| `rtSetOscillator`| `rtSetOscillator(long Nr, double Period, double Width)` | Cài đặt chu kỳ và độ rộng xung cho bộ phát xung phần cứng $1, 2, 3$ |
