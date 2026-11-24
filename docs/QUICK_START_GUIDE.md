# Hướng Dẫn Cài Đặt Nhanh 5 Phút (Quick Start Guide)

Tài liệu này tóm tắt toàn bộ quy trình thiết lập từ A đến Z để bạn đưa đầu quét **Newson Rhothor (LPKF #4281)** vào hoạt động trên **Windows 10 / Windows 11 (64-bit)** chỉ trong 5 phút.

---

## ⚡ Tóm Tắt Quy Trình 3 Bước

```
[BƯỚC 1: CẤP NGUỒN & KẾT NỐI]
      |-- Nguồn 12VDC vào cổng X5
      |-- Cáp USB-B vào cổng X6 (PC)
      v
[BƯỚC 2: CÀI DRIVER QUA ZADIG (1 PHÚT)]
      |-- Mở Zadig -> Chọn RT-CU (VID 1525, PID 0007)
      |-- Chọn WinUSB -> Bấm Install Driver
      |-- Nhấp đúp file register_device_guid.reg
      v
[BƯỚC 3: KIỂM TRA HOẠT ĐỘNG]
      |-- Chạy launch_rhothor.bat hoặc rhothor_cli.py
      v
[HOÀN THÀNH: ĐẦU QUÉT SẴN SÀNG KHẮC LASER]
```

---

## Bước 1: Kết Nối Phần Cứng

1. **Cấp nguồn 12VDC (Cổng X5):**
   * Chân 1: `+12V` DC (tối thiểu 2A, chuẩn công nghiệp ổn định).
   * Chân 2: `0V` (GND nguồn).
   * Chân 3: Nối đất vỏ máy (Earth / Vỏ nhôm block galvo).
2. **Cắm cáp USB (Cổng X6):**
   * Cắm cáp USB chuẩn B (chân vuông máy in) vào bo mạch, đầu kia cắm vào cổng USB 2.0/3.0 phía sau case máy tính PC.

---

## Bước 2: Cài Driver WinUSB (Chỉ làm 1 lần duy nhất)

1. Tải công cụ miễn phí **Zadig**: [zadig.akeo.ie](https://zadig.akeo.ie/)
2. Mở Zadig:
   * Vào menu **Options** $\rightarrow$ bấm chọn **List All Devices**.
   * Trong danh sách xổ xuống, chọn **`RT-CU`** (kiểm tra thấy `USB ID: 1525 0007`).
   * Chọn mục Driver là **`WinUSB`**.
   * Nhấn nút lớn **Install Driver** (hoặc *Replace Driver*). Đợi thanh tiến trình hoàn tất 100%.
3. **Kích hoạt nhận diện cho `rhothor.dll`:**
   * Trong thư mục dự án, vào thư mục `tools/` và **nhấp đúp** vào file:
     [tools/register_device_guid.reg](file:///home/chaunt/rhothor_laser_controller/tools/register_device_guid.reg)
   * Chọn **Yes** $\rightarrow$ chọn **OK**.

---

## Bước 3: Mở Phần Mềm & Test Chuyển Động

### Cách A: Mở Phần Mềm Gốc của Hãng (`rhothor.exe`)
1. Vào thư mục `tools/windows_setup/` và nhấp đúp vào:
   `launch_rhothor.bat`
2. Phần mềm sẽ mở lên với đúng các thông số:
   * Scanner type: `RTSN - AR180`
   * Scanner delay: `60 µsec`
   * Field size: `201.65 mm`
   * Max speed: `6000 mm/sec`
   * Z Focus: `EL - S2` (`147 mm`)
3. Bấm nút **Run Test** trên thanh công cụ để galvo chạy thử quỹ đạo hình học mẫu!

### Cách B: Chạy Bằng Công Cụ Dòng Lệnh CLI (Tự động xuất file SVG)
Mở cửa sổ dòng lệnh (Terminal / PowerShell / CMD) và gõ:
```bash
python3 tools/rhothor_cli.py --config config/lpkf_ar180_els2.json --test all --size 60
```
Gương quét sẽ tự động chạy các hình kiểm tra: Hình vuông, hình tròn arc, dấu tâm cross và lưới 5x5!

---

## Bước Tiếp Theo Của Bạn:

* Nếu muốn chạy trực tiếp bằng phần mềm **EZCAD**: xem tiếp [docs/EZCAD_STEP_BY_STEP_WIRING.md](file:///home/chaunt/rhothor_laser_controller/docs/EZCAD_STEP_BY_STEP_WIRING.md).
* Bảng tra cứu nhanh thông số chuẩn máy LPKF: xem [docs/LPKF_CALIBRATION_CHEAT_SHEET.md](file:///home/chaunt/rhothor_laser_controller/docs/LPKF_CALIBRATION_CHEAT_SHEET.md).
* Khi gặp bất kỳ sự cố nào: tra cứu nhanh tại [docs/TROUBLESHOOTING_FAQ.md](file:///home/chaunt/rhothor_laser_controller/docs/TROUBLESHOOTING_FAQ.md).
