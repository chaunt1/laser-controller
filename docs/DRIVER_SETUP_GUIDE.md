# Hướng Dẫn Cài Đặt Driver & Kết Nối Thiết Bị Rhothor Trên Windows

Hệ thống Newson Rhothor sử dụng giao thức truyền nhận dữ liệu qua driver **WinUSB** chuẩn của Microsoft.

---

## 1. Thông Tin Nhận Diện USB Phần Cứng (Hardware IDs)

* **Vendor ID (VID):** `0x1525` (Newson Engineering NV)
* **Product ID (PID):**
  * `0x0007`: **Rhothor Deflection System** (Bộ điều khiển đầu quét chính)
  * `0x0008`: **Beacon Calibration System** (Hệ thống căn chỉnh quang học tự động)
* **Device Interface GUID:** `{FED7004F-CA54-478c-BA6F-E9EFE07066B4}`

---

## 2. Các Bước Cài Đặt Driver Trên Windows 10 / Windows 11 (64-bit)

### Bước 1: Chuẩn bị tệp tin Driver
Tệp tin cài đặt driver 64-bit nằm trong thư mục của dự án:
`rhothor_laser_controller/vendor/driver/64-bit/rhothor_64.inf`

### Bước 2: Tắt tính năng kiểm tra chữ ký số (Nếu Windows cảnh báo Unsigned Driver)
Do driver gốc của Newson được phát hành từ giai đoạn Windows 7/WinUSB v1.9, trên một số bản Windows 10/11 có thể yêu cầu chứng chỉ chữ ký số driver (Driver Signature Enforcement):
1. Nhấn giữ phím `Shift` và chọn `Restart` trong menu Start.
2. Chọn `Troubleshoot` $\rightarrow$ `Advanced options` $\rightarrow$ `Startup Settings` $\rightarrow$ `Restart`.
3. Khi máy khởi động lại, nhấn phím `7` hoặc `F7` để chọn **Disable driver signature enforcement**.

### Bước 3: Cài đặt qua Device Manager
1. Kết nối cáp USB-B từ bo mạch RT-CU vào máy tính. Cấp nguồn 12VDC vào cổng X5.
2. Mở `Device Manager` (nhấn tổ hợp phím `Win + X` $\rightarrow$ chọn Device Manager).
3. Trong danh sách thiết bị, tìm thiết bị có dấu chấm than màu vàng (thường hiển thị là `Newson Device` hoặc `Unknown Device`).
4. Nhấp chuột phải $\rightarrow$ chọn **Update driver**.
5. Chọn **Browse my computer for drivers**.
6. Trỏ đường dẫn đến thư mục:
   `...\rhothor_laser_controller\vendor\driver\64-bit\`
7. Nhấn **Next** và xác nhận **Install this driver software anyway** khi có hộp thoại cảnh báo của Windows.

### Bước 4: Kiểm tra trạng thái kết nối
Sau khi cài đặt thành công, trong Device Manager sẽ xuất hiện nhánh:
* `NewsonDeviceClass` $\rightarrow$ **Rhothor Deflection System** (hoặc trong mục `Universal Serial Bus devices`).

---

## 3. Khắc Phục Sự Cố Kết Nối Thường Gặp

| Triệu Chứng | Nguyên Nhân Khả Dĩ | Cách Xử Lý |
| :--- | :--- | :--- |
| **Đèn LED nguồn trên bo không sáng** | Mất nguồn cấp 12VDC | Kiểm tra bộ nguồn tại cổng X5. Đo điện áp đảm bảo nằm trong khoảng $11.5\text{V} - 13.0\text{V}$, dòng tải tối thiểu 2A. |
| **Device Manager không hiện thiết bị** | Cáp USB hỏng hoặc cổng USB lỗi | Cổng USB X6 được cách ly quang và lấy nguồn từ bus 5V của PC. Thử đổi cáp USB-B chất lượng cao và cắm trực tiếp vào cổng sau của bo mạch chủ máy tính. |
| **`rtInitialise()` trả về mã lỗi `5` (`ERR_HARDWARE`)** | Driver chưa nạp đúng hoặc xung đột cổng | Khởi động lại phần mềm, kiểm tra lại Device Manager. Thử gọi `rtSelectDevice(4281)` để trỏ đích danh số serial của máy LPKF. |
| **Galvo rung giật hoặc phát tiếng rít khi quét** | Hệ số `Scanner Delay` quá thấp | Trong phần mềm `rhothor.exe` hoặc file cấu hình, tăng `Scanner Delay` từ $60\ \mu\text{s}$ lên $70 - 80\ \mu\text{s}$, sau đó nhấn **Tune XY** để hệ thống tự cân chỉnh lại PID. |
