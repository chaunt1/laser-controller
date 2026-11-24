# Hướng Dẫn Chi Tiết Cài Đặt Driver Rhothor v7.5 Trên Windows 10 & Windows 11 (64-bit)

Tài liệu này giải quyết triệt để vấn đề: **Phần cứng thế hệ cũ (v7.5) chỉ tương thích với driver v7.5, nhưng khi cắm vào Windows 10 hoặc Windows 11 (64-bit) thì bị chặn driver hoặc không nhận thiết bị.**

---

## 1. Nguyên Nhân Gây Lỗi Trên Windows 10 / 11

1. **Chặn chữ ký số (Driver Signature Enforcement):**
   File `rhothor_64.inf` gốc phát hành từ năm 2012 không có chữ ký số điện tử của Microsoft (`.cat` file), nên Windows 10/11 mặc định chặn cài đặt (Báo lỗi *The third-party INF does not contain digital signature information* hoặc mã lỗi Code 28, Code 52).
2. **Lỗi Co-installer KMDF 1.9 cổ điển:**
   Trong file INF cũ có các dòng gọi `WdfCoInstaller01009.dll` và `winusbcoinstaller2.dll`. Các thư viện này được thiết kế cho Windows Vista/7 và xung đột trực tiếp với nhân Kernel của Windows 10/11 hiện đại.
3. **Mất liên kết GUID:**
   Thư viện `rhothor.dll` (v7.5) quét cổng USB dựa trên mã GUID định danh:
   `{FED7004F-CA54-478c-BA6F-E9EFE07066B4}`.
   Nếu cài đặt bằng công cụ chung mà thiếu việc đăng ký GUID này vào Windows Registry, `rhothor.dll` sẽ báo lỗi `ERR_HARDWARE (5)`.

---

## 2. Cách 1: Cài Đặt Bằng Zadig (Nhanh Nhất, Khuyên Dùng 100%)

Công cụ **Zadig** là tiêu chuẩn công nghiệp để nạp driver WinUSB sạch trên Windows 10 và Windows 11 mà không cần tắt bảo mật của Windows.

### Bước 1: Chuẩn bị
* Cấp nguồn 12VDC vào cổng X5 của bo mạch RT-CU.
* Cắm cáp USB-B từ cổng X6 vào máy tính Windows 10/11.
* Tải công cụ Zadig miễn phí từ: `https://zadig.akeo.ie/` (hoặc chạy bản portable).

### Bước 2: Nạp WinUSB Driver bằng Zadig
1. Mở `Zadig.exe`.
2. Trên thanh menu, chọn **Options** $\rightarrow$ tích chọn **List All Devices**.
3. Trong ô danh sách xổ xuống, chọn thiết bị: **`RT-CU`** (hoặc thiết bị có thông số `USB ID: 1525 0007`).
4. Ở ô bên phải (Driver mục tiêu), chọn mũi tên để hiển thị: **`WinUSB (v6.1.7600.16385)`** (hoặc phiên bản WinUSB mới nhất).
5. Nhấn nút **Install Driver** (hoặc **Replace Driver**). Đợi khoảng 10-20 giây để Windows nạp driver thành công.

### Bước 3: Đăng ký Device GUID cho `rhothor.dll`
Do `rhothor.dll` yêu cầu đúng GUID `{FED7004F-CA54-478c-BA6F-E9EFE07066B4}`, bạn chạy một trong hai cách sau:
* **Cách nhanh:** Nhấp đúp chuột vào file:
  [tools/register_device_guid.reg](file:///home/chaunt/rhothor_laser_controller/tools/register_device_guid.reg) $\rightarrow$ Chọn **Yes** $\rightarrow$ **OK**.
* **Hoặc bằng PowerShell:** Nhấp chuột phải vào file [tools/register_device_guid.ps1](file:///home/chaunt/rhothor_laser_controller/tools/register_device_guid.ps1) $\rightarrow$ Chọn **Run with PowerShell** (với quyền Administrator).

---

## 3. Cách 2: Cài Đặt Bằng File INF Sạch (Modern Clean INF)

Trong dự án đã tạo sẵn file INF tối ưu cho Windows 10/11:
[vendor/driver/rhothor_win10_11_x64.inf](file:///home/chaunt/rhothor_laser_controller/vendor/driver/rhothor_win10_11_x64.inf)
*(Đã loại bỏ toàn bộ các co-installer KMDF lỗi thời, trỏ trực tiếp vào driver WinUSB nội tại của Windows và tự động gắn GUID)*.

### Các bước thực hiện:
1. Tạm thời tắt kiểm tra chữ ký số:
   * Giữ phím `Shift` $\rightarrow$ Nhấn `Restart` máy tính.
   * Chọn `Troubleshoot` $\rightarrow$ `Advanced options` $\rightarrow$ `Startup Settings` $\rightarrow$ `Restart`.
   * Nhấn phím `7` (Disable driver signature enforcement).
2. Sau khi vào Windows, mở `Device Manager` $\rightarrow$ Chuột phải vào thiết bị `RT-CU` $\rightarrow$ **Update driver**.
3. Chọn **Browse my computer for drivers** $\rightarrow$ Chọn đường dẫn đến thư mục chứa file [rhothor_win10_11_x64.inf](file:///home/chaunt/rhothor_laser_controller/vendor/driver/rhothor_win10_11_x64.inf).
4. Nhấn **Install this driver software anyway** khi xuất hiện cảnh báo màu đỏ.

---

## 4. Cấu Hình Tương Thích Để Chạy `Config.exe` và `rhothor.exe` (v7.5)

Các phần mềm đồ họa của bộ v7.5 (`Config.exe`, `rhothor.exe`) được biên dịch từ thời VC++ 2008 / MFC:

1. **Chế độ tương thích (Compatibility Mode):**
   * Nhấp chuột phải vào file `Config.exe` hoặc `rhothor.exe` $\rightarrow$ Chọn **Properties**.
   * Chuyển sang tab **Compatibility**.
   * Tích chọn **Run this program in compatibility mode for:** $\rightarrow$ Chọn **Windows 7**.
   * Tích chọn **Run this program as an administrator**.
   * Nhấn **Apply** $\rightarrow$ **OK**.
2. **Đảm bảo tệp DLL đi kèm:**
   Luôn đặt file `rhothor.dll` (bản 32-bit cho file exe 32-bit) nằm cùng thư mục chứa `Config.exe` và `rhothor.exe`.
