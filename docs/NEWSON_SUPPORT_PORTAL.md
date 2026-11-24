# Khảo Sát & Phân Tích Cổng Hỗ Trợ Chính Hãng Newson (support.newson.be)

Cổng hỗ trợ chính thức của hãng: **[support.newson.be/software.php](https://support.newson.be/software.php)**

Tài liệu này tổng hợp toàn bộ các gói phần mềm, firmware, bộ công cụ phát triển phần mềm (SDK) mới nhất tải trực tiếp từ máy chủ Newson và hướng dẫn ứng dụng vào dự án.

---

## 1. Danh Mục Các Gói Phần Mềm Mới Nhất Từ Hãng

| Tên Gói Phần Mềm | Phiên Bản | Ngày Phát Hành | Kích Thước | Mục Đích Sử Dụng |
| :--- | :---: | :---: | :---: | :--- |
| **rhothor Software Package** | `v11.0.48.12 v3` | **25/02/2026** | ~38.5 MB | Bộ công cụ chính thức: `Rhothor.exe`, `rhothor.dll`, Firmware patch CUA32/16, driver USB |
| **Flight Software Dev Kit (SDK)** | `v4.4.0.0` | **09/03/2026** | ~4.5 MB | SDK phần mềm khắc laser hoàn chỉnh kèm **toàn bộ mã nguồn C#** |
| **ELA-TR8-4G Firmware Patch** | `0x050B` | 2026 | ~3.8 MB | Bản vá firmware cho bộ dịch tiêu cự động thế hệ 4G |
| **RTA/RLA Firmware Patch** | `0x041B` | 2026 | ~3.8 MB | Firmware cho dòng đầu quét RTA và RLA |
| **CONtester Software Suite** | `2.0.0.1` | - | ~15 MB | Phần mềm đo kiểm và căn chỉnh độ chính xác galvo |

---

## 2. So Sánh: Bản v7.5 (Trong Drive Cũ) vs Bản v11.0 Mới Nhất

Hệ thống của bạn ban đầu đi kèm bản **v7.5.0.27** (năm 2013-2016). Phiên bản **v11.0.48.12 v3** mới nhất mang đến các nâng cấp bước ngoặt:

| Tiêu Chí | Bản v7.5 (Cũ Trong Drive) | Bản v11.0 (Chính Hãng Mới Nhất) |
| :--- | :--- | :--- |
| **Giao thức truyền thông** | Chỉ hỗ trợ duy nhất **USB 2.0 (WinUSB)** | Hỗ trợ cả **USB 2.0** và **Ethernet (TCP/IP + UDP)** |
| **Kiến trúc điều khiển** | Đơn lõi DSP 160 MIPS | Hỗ trợ đa nhân **CUA32 (CPU1, CPU2, CPU3)** & CUA16 |
| **Số hàm API (`rhothor.dll`)** | **55 hàm** | **215 hàm** (bổ sung kết nối IP, quản lý file flash, bù trôi) |
| **Tính toán đường cong** | Thuật toán Arc cũ | Sửa lỗi nội suy cung tròn / circular interpolation (*Fix arc*) |
| **Bộ dịch tiêu cự Z** | ELEVAthor EL-S2 truyền thống | Hỗ trợ dòng mới **ELA-TR8-4G / ELA-TR4-4G** |
| **Độ ổn định hệ điều hành** | Tối ưu cho WinXP / Win 7 | Tương thích hoàn toàn **Windows 10 / Windows 11 (64-bit)** |

---

## 3. Bộ Công Cụ Flight SDK v4.4.0.0 (Laser Marking Suite)

Điểm giá trị nhất trên cổng hỗ trợ chính hãng là **Flight SDK** chứa **toàn bộ mã nguồn C# (.NET WinForms)** của một ứng dụng điều khiển laser chuyên nghiệp:

Thư mục mã nguồn lưu tại: `rhothor_laser_controller/vendor/flight_sdk/`
* `flight_exe/DemoStd/src/`: Ứng dụng điều khiển khắc laser tiêu chuẩn:
  * `Flight.cs`: Giao diện chính điều khiển nét cắt, nạp file vector.
  * `rhothorLink`: Cầu nối trực tiếp giữa giao diện đồ họa với thư viện `rhothor.dll`.
  * `SettingDlg.cs`: Cài đặt tham số thấu kính, trường quét, trễ laser.
  * `FlashExplorer.cs`: Quản lý và nạp file macro vào bộ nhớ Flash trên bo mạch.
* `flight_exe/DemoTable/src/`: Ứng dụng tích hợp bàn di mẫu XY (Step & Repeat) và đầu đo hiệu chuẩn Beacon BCU (`CalibrationBCU.cs`, `BeaconTarget.cs`).
* `extra/fonts/`: Bộ font ký tự khắc laser chuyên dụng (SEMI OCR, Newson Cyrillic, Newson Western).

---

## 4. Tích Hợp Vào Dự Án Của Bạn

Toàn bộ gói v11 và Flight SDK đã được giải nén sẵn sàng trong dự án:
* **Driver mới nhất:** [vendor/official_v11/driver/](file:///home/chaunt/rhothor_laser_controller/vendor/official_v11/driver/)
  * `CUA32.inf`, `CUA.inf`, `beacon.inf`, `stick.inf`
* **DLL 64-bit mới nhất:** [vendor/official_v11/rhothor_dll/64-bit/](file:///home/chaunt/rhothor_laser_controller/vendor/official_v11/rhothor_dll/64-bit/)
  * `rhothor.dll` (v11.0.48.10)
  * `rhothorDll.h` (toàn bộ 215 hàm API)
* **Phần mềm test mới nhất:** [vendor/official_v11/rhothor_exe/Src/Rhothor.exe](file:///home/chaunt/rhothor_laser_controller/vendor/official_v11/rhothor_exe/Src/Rhothor.exe)
* **Mã nguồn C# Flight:** [vendor/flight_sdk/flight_exe/DemoStd/src/](file:///home/chaunt/rhothor_laser_controller/vendor/flight_sdk/flight_exe/DemoStd/src/)
