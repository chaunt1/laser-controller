# Hỏi Đáp & Khắc Phục Sự Cố Trong 1 Phút (Troubleshooting & FAQ)

Tổng hợp các hiện tượng lỗi thường gặp nhất trong thực tế khi kết nối, cài đặt driver và vận hành đầu quét Newson Rhothor kèm cách xử lý tức thì.

---

### Câu 1: Cắm cáp USB vào máy tính Windows 10/11 nhưng không thấy máy tính nhận thiết bị?
* **Nguyên nhân:** Bo mạch RT-CU dùng nguồn nuôi mạch cách ly USB từ cổng USB máy tính, nhưng toàn bộ mạch điều khiển DSP và động cơ yêu cầu nguồn 12VDC từ cổng X5.
* **Cách xử lý:** Kiểm tra nguồn cấp tại cổng X5. Đo điện áp đảm bảo nằm trong khoảng $11.5\text{V} - 13.0\text{V}$ (dòng tải $\ge 2\text{A}$). Thử đổi cổng cắm USB phía sau bo mạch chủ máy tính.

---

### Câu 2: Phần mềm báo lỗi `ERR_HARDWARE (5)` hoặc `Hardware not responding`?
* **Nguyên nhân:** Driver WinUSB đã cài nhưng thiếu mã định danh `DeviceInterfaceGUID` trong Windows Registry, khiến `rhothor.dll` không tìm thấy đường dẫn USB của bo mạch.
* **Cách xử lý:** 
  1. Vào thư mục `tools/` của dự án.
  2. Nhấp đúp chuột vào file [tools/register_device_guid.reg](file:///home/chaunt/rhothor_laser_controller/tools/register_device_guid.reg) $\rightarrow$ bấm **Yes** $\rightarrow$ bấm **OK**.
  3. Khởi động lại phần mềm `rhothor.exe` hoặc script Python.

---

### Câu 3: Khi cài đặt driver trên Windows 10/11, Windows báo lỗi chữ ký số (The third-party INF does not contain digital signature)?
* **Nguyên nhân:** File INF cũ từ năm 2012 không có chữ ký số điện tử của Microsoft.
* **Cách xử lý:** Dùng phần mềm miễn phí **Zadig** (xem hướng dẫn tại [docs/QUICK_START_GUIDE.md](file:///home/chaunt/rhothor_laser_controller/docs/QUICK_START_GUIDE.md)). Zadig tự động tạo chứng chỉ bảo mật và cài đặt driver WinUSB sạch mà không bị Windows chặn.

---

### Câu 4: Động cơ galvo phát ra tiếng rít chói tai hoặc bị rung lắc giật cục khi đứng yên?
* **Nguyên nhân:** Tham số `Scanner Delay` bị đặt quá thấp (ví dụ dưới $50\ \mu\text{s}$) hoặc hệ số điều khiển PID bị lệch sau khi thay đổi thông số.
* **Cách xử lý:**
  1. Trong phần mềm `rhothor.exe`, đặt lại `Scanner delay` thành `60` hoặc `70` µsec.
  2. Bấm nút **Tune XY** (và **Tune Z**) trên thanh công cụ để hệ thống tự cân chỉnh lại dòng điện động cơ.
  3. Nhấn nút **Store** để lưu vào chip nhớ.

---

### Câu 5: Tại sao mở EzCad2 lên thì phần mềm báo: "Cannot find a valid lmc1 device. The software will work in demo state"?
* **Nguyên nhân:** EzCad2 khóa bản quyền bằng phần cứng chip bảo mật trên card USB-LMC. Nếu bạn chỉ cắm duy nhất cáp USB của Rhothor vào máy tính, EzCad không tìm thấy card LMC của hãng JCZ nên sẽ chuyển sang chế độ Demo.
* **Cách xử lý:**
  * **Nếu muốn dùng EzCad trực tiếp:** Cần mua thêm một card rời JCZ LMC (loại card USB phổ thông giá rẻ), cắm USB card LMC vào máy tính, và nối cổng CON1 của card LMC sang cổng X7 của Rhothor theo [docs/EZCAD_STEP_BY_STEP_WIRING.md](file:///home/chaunt/rhothor_laser_controller/docs/EZCAD_STEP_BY_STEP_WIRING.md).
  * **Nếu không muốn mua card JCZ:** Bạn dùng EzCad Demo để vẽ $\rightarrow$ Export ra file `DXF` $\rightarrow$ chạy lệnh `python3 tools/ezd_dxf_to_rhothor.py --file my_file.dxf` để bắn laser trực tiếp qua cáp USB của Rhothor!

---

### Câu 6: Nét khắc bị cháy đậm ở điểm đầu hoặc điểm cuối của đường thẳng?
* **Nguyên nhân:** Thời gian trễ bật/tắt laser (`Gate On Delay`, `Gate Off Delay`) chưa khớp với gia tốc của gương quét. Laser bật quá sớm khi gương chưa kịp tăng tốc, hoặc laser tắt quá muộn khi gương đã dừng lại.
* **Cách xử lý:**
  * Nếu cháy đầu nét: Tăng `Gate On Delay` lên thêm $10 - 20\ \mu\text{s}$.
  * Nếu cháy đuôi nét: Giảm `Gate Off Delay` hoặc tăng `Laser Off Delay`.
  * Có thể chạy file mẫu [examples/04_laser_timing_tuning.py](file:///home/chaunt/rhothor_laser_controller/examples/04_laser_timing_tuning.py) để bắn một dải vạch thử nghiệm tìm giá trị tối ưu.

---

### Câu 7: Hình khắc ra bị méo thành hình bình hành hoặc hình thang?
* **Nguyên nhân:** Hai trục X và Y bị lệch góc vuông cơ khí hoặc chưa nạp bảng hiệu chuẩn méo thấu kính quang học (Optical Distortion).
* **Cách xử lý:**
  * Trong EzCad (phím `F3` $\rightarrow$ tab `Field`): Chỉnh thông số **Distortion** hoặc **Barrel/Pincushion correction**.
  * Trong chế độ Rhothor: Nạp file ma trận hiệu chuẩn `.cal` thông qua hàm `rtLoadCalibrationFile()`.

---

### Câu 8: Hình khắc ra bị lộn ngược gương (chữ viết ngược trái/phải)?
* **Cách xử lý:** 
  * Nếu dùng EzCad: Nhấn phím `F3` $\rightarrow$ tab **Galvo 1 (X)** $\rightarrow$ tích chọn ô **`Inverse`**.
  * Nếu dùng phần mềm `rhothor.exe`: Bấm nút **Mirror X** (hoặc **Mirror Y**) trong tab Properties rồi bấm **Store**.
