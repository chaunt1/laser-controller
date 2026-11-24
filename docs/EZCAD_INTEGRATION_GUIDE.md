# Hướng Dẫn Tích Hợp Đầu Quét Laser Rhothor (LPKF) Với Phần Mềm EZCAD

Tài liệu này hướng dẫn chi tiết cách kết nối và vận hành hệ thống đầu quét **Newson Rhothor™ RT-CU (LPKF #4281)** hoạt động hoàn hảo với phần mềm khắc laser tiêu chuẩn công nghiệp **EZCAD (EzCad2 / EzCad3)**.

---

## 1. Bản Chất Kỹ Thuật: EzCad & Rhothor Giao Tiếp Như Thế Nào?

1. **Phần mềm EZCAD:** Được phát triển bởi hãng Beijing JCZ (Golden Orange). EzCad điều khiển đầu quét Galvo thông qua card điều khiển **JCZ LMC** (như LMCV4-DIGITAL, LMCV4-FIBER, USB-LMC).
2. **Giao thức chuẩn XY2-100:** Cổng điều khiển Galvo trên card EzCad JCZ (Cổng `CON1`) xuất ra tín hiệu số vi sai theo giao thức chuẩn công nghiệp quốc tế **XY2-100** (20-bit serial words, xung nhịp 2 MHz).
3. **Khả năng tương thích của Rhothor:** Bo mạch điều khiển Newson Rhothor RT-CU được tích hợp sẵn bộ giải mã phần cứng chuẩn **XY2-100** trên cổng `X7` (DB-25).
   * Khi chuyển sang chế độ **XY2-100 Mode**, bo mạch Rhothor tự động tắt bộ thông dịch vector nội bộ và chuyển sang lắng nghe trực tiếp tín hiệu tọa độ thời gian thực từ card EzCad JCZ!

---

## 2. Giải Pháp 1: Kết Nối Phần Cứng Trực Tiếp (Chuẩn Công Nghiệp — Khuyên Dùng 100%)

Đây là giải pháp chuyên nghiệp nhất được các nhà chế tạo máy laser áp dụng: Cho phép bạn dùng phần mềm **EzCad2 / EzCad3** với 100% tính năng (khắc vector, bitmap, hatching tô bóng, barcode, ma trận QR, trục xoay tròn Rotary, phím đạp chân...).

```
+------------------+          USB         +-----------------------+
|  Máy Tính PC     | <==================> | Card JCZ LMC (EzCad)  |
|  (Cài EzCad2/3)  |                      +-----------------------+
+------------------+                        | CON1 (DB-15)    | CON2 (DB-25)
                                            | (XY2-100)       | (Tín hiệu Laser)
                                            v                 v
                                  +-------------------+  +---------------+
                                  | Cổng X7 (DB-25)   |  | Nguồn Phát    |
                                  | Rhothor RT-CU     |  | Laser         |
                                  | (Đầu quét Galvo)  |  | (Fiber/CO2/UV)|
                                  +-------------------+  +---------------+
```

### Bước 1: Cấu hình bo mạch Rhothor sang chế độ XY2-100
1. Cắm tạm cáp USB từ PC vào cổng X6 của bo Rhothor RT-CU.
2. Mở phần mềm `Config.exe` (trong thư mục `vendor/rhothor_exe/src/Config.exe`).
3. Trong giao diện chọn cấu hình hệ thống:
   * Chuyển từ **Integrated Control** sang **XY2-100 Mode**.
   * Nhấn **Store / Save to Flash** để lưu vĩnh viễn cấu hình vào chip nhớ của bo mạch.
   * Rút cáp USB ra (khi chạy XY2-100, bo mạch không cần cắm USB vào PC nữa).

### Bước 2: Sơ đồ hàn cáp kết nối từ Cổng CON1 (JCZ LMC) sang Cổng X7 (Rhothor RT-CU)
Sử dụng một sợi cáp bọc kim chống nhiễu (Shielded Cable) với một đầu là **DB-15 Đực (cắm vào Card JCZ CON1)** và một đầu là **DB-25 Đực (cắm vào Bo Rhothor X7)**:

| Tên Tín Hiệu | Chân Card JCZ `CON1` (DB-15) | Chân Bo Rhothor `X7` (DB-25) | Ý Nghĩa Kỹ Thuật |
| :--- | :---: | :---: | :--- |
| **CLK -** | **Chân 1** | **Chân 1** (IO1-) | Tín hiệu xung nhịp đồng hồ vi sai âm (Clock 2MHz) |
| **CLK +** | **Chân 9** | **Chân 14** (IO1+) | Tín hiệu xung nhịp đồng hồ vi sai dương |
| **SYNC -** | **Chân 2** | **Chân 2** (IO2-) | Tín hiệu đồng bộ khung dữ liệu âm |
| **SYNC +** | **Chân 10** | **Chân 15** (IO2+) | Tín hiệu đồng bộ khung dữ liệu dương |
| **CHANNEL X -** | **Chân 3** | **Chân 3** (IO3-) | Dữ liệu vị trí trục X âm |
| **CHANNEL X +** | **Chân 11** | **Chân 16** (IO3+) | Dữ liệu vị trí trục X dương |
| **CHANNEL Y -** | **Chân 4** | **Chân 4** (IO4-) | Dữ liệu vị trí trục Y âm |
| **CHANNEL Y +** | **Chân 12** | **Chân 17** (IO4+) | Dữ liệu vị trí trục Y dương |
| **STATUS -** | **Chân 6** | **Chân 7** (IO7-) | Tín hiệu phản hồi trạng thái galvo âm |
| **STATUS +** | **Chân 14** | **Chân 20** (IO7+) | Tín hiệu phản hồi trạng thái galvo dương |
| **GND / Mass** | **Chân 8 & 15** | **Chân 8 & 13** (REF_IO) | Mass tín hiệu chung giữa hai bo |

*(Lưu ý: Nối lớp lưới chống nhiễu của vỏ bọc cáp vào vỏ sắt của đầu cắm DB để triệt tiêu nhiễu cao tần).*

### Bước 3: Đấu nối nguồn laser vào Card JCZ LMC
* Nguồn laser (Laser Source) được điều khiển trực tiếp từ cổng **`CON2` (DB-25)** của Card JCZ (Bật/tắt laser, xung PWM, công suất laser 0-100%).

### Bước 4: Cài đặt tham số trong phần mềm EZCAD (`F3 Parameter`)
1. Mở phần mềm EzCad $\rightarrow$ Nhấn phím `F3` (hoặc vào menu `Param (F3)`).
2. Tab **Field**:
   * Đặt kích thước vùng quét: `201.65 mm` (hoặc `200.0 mm`).
3. Tab **Galvo 1 (X) & Galvo 2 (Y)**:
   * Nếu hình khắc ra bị ngược gương trái/phải: Tích chọn ô **Inverse** tại Galvo 1.
   * Nếu hình khắc ra bị lộn ngược trên/dưới: Tích chọn ô **Inverse** tại Galvo 2.
   * Nếu trục X và Y bị đổi chỗ cho nhau: Đổi cổng Galvo 1 và Galvo 2.
4. **Hiệu chuẩn tỉ lệ (Scale Calibration):**
   * Vẽ một hình vuông kích thước $50.0\ \text{mm} \times 50.0\ \text{mm}$ trên EzCad và khắc lên mẫu thử.
   * Dùng thước kẹp điện tử đo kích thước thực tế đạt được (ví dụ đo được $49.85\ \text{mm}$).
   * Nhấn nút **Scale** trong EzCad $\rightarrow$ Nhập số thiết kế `50.0` và số thực tế `49.85` $\rightarrow$ EzCad sẽ tự động bù tỉ lệ xung/mm chính xác tuyệt đối.

---

## 3. Giải Pháp 2: Vận Hành Trực Tiếp Qua Cáp USB (Nếu Chưa Có Card JCZ LMC)

Nếu hiện tại bạn **chỉ có sợi cáp USB cắm từ máy tính vào bo mạch Rhothor RT-CU** và chưa trang bị card phần cứng JCZ LMC:

### Lý do EzCad không cho bắn laser khi chỉ cắm USB Rhothor:
Phần mềm `EzCad2.exe` có cơ chế bảo vệ bản quyền phần cứng: Nó quét tìm chip mã hóa trên bo mạch JCZ LMC qua cổng USB. Nếu không thấy card JCZ, EzCad sẽ hiển thị:
> *"Cannot find a valid lmc1 device. The software will work in demo state."*
Ở chế độ Demo, EzCad cho phép vẽ, chỉnh sửa, gõ chữ, tạo mã vạch nhưng **khóa nút Mark (không cho phát tia laser)**.

### Cách khắc phục hiệu quả:
1. **Quy trình: Thiết kế trên EzCad Demo $\rightarrow$ Cắt trên Rhothor qua USB:**
   * Bạn vẫn dùng giao diện EzCad để thiết kế bản vẽ, căn chỉnh chữ, logo, mã vạch (chạy bản EzCad Demo hoàn toàn miễn phí).
   * Sau khi thiết kế xong, chọn menu `File` $\rightarrow$ `Export Vector File` $\rightarrow$ Lưu dưới định dạng **DXF** hoặc **PLT**.
   * Sử dụng công cụ tự động hóa đã xây dựng trong dự án:
     [tools/ezd_dxf_to_rhothor.py](file:///home/chaunt/rhothor_laser_controller/tools/ezd_dxf_to_rhothor.py)
     ```bash
     python3 tools/ezd_dxf_to_rhothor.py --file my_design.dxf --speed 1500 --power 80
     ```
     Script sẽ tự động dịch các đường line, arc, polyline từ file DXF thành chuỗi lệnh `rtJumpTo`, `rtLineTo`, `rtArcTo` và bắn trực tiếp xuống bo Rhothor qua USB!

2. **Sử dụng bộ Flight SDK đi kèm của hãng Newson:**
   * Trong thư mục [vendor/flight_sdk/](file:///home/chaunt/rhothor_laser_controller/vendor/flight_sdk/), hãng Newson cung cấp sẵn phần mềm **Flight.exe** (kèm toàn bộ mã nguồn C#).
   * Giao diện của **Flight** tương tự như EzCad (có layer, màu bút, font chữ khắc, chỉnh tốc độ/công suất), nhưng được viết riêng cho Newson Rhothor và **giao tiếp trực tiếp 100% qua cáp USB** mà không cần bất kỳ card rời nào!
