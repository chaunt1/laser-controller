# Sơ Đồ Chân & Đấu Dây Phần Cứng Bo Mạch Điều Khiển Newson Rhothor RT-CU

Tài liệu này hướng dẫn chi tiết sơ đồ cổng kết nối, điện áp nguồn, tín hiệu số vi sai RS-485 và chuẩn ghép nối laser cho bo mạch **Newson Rhothor™ RT-CU**.

---

## 1. Tổng Quan Các Cổng Kết Nối Trên Bo Mạch RT-CU

```
+--------------------------------------------------------------+
|                          BO MẠCH RT-CU                       |
|                                                              |
|   [GND]                                              [X7]    |
|   (Bắt mass vỏ)                                    DB-25 I/O |
|                                                              |
|   [X2] Motor Galvo X                                         |
|                                                              |
|   [X3, X4] Bus Mở Rộng Trục Z (ELEVAthor EL-S2)             |
|                                                              |
|   [X1] Motor Galvo Y                                 [X6]    |
|                                                     USB-B    |
|   [X5] Cấp Nguồn 12VDC                                       |
+--------------------------------------------------------------+
```

---

## 2. Cổng Nguồn X5 (Power Supply Connector)

Bo mạch yêu cầu bộ nguồn công nghiệp 12VDC cách ly riêng biệt.
* **Loại cổng trên bo mạch:** Phoenix Contact `MC1.5/3-GF-3.81`
* **Loại đầu nối cáp:** Phoenix Contact `MC1.5/3-STF-3.81`
* **Tiêu chuẩn an toàn:** Tương thích chuẩn phát xạ EMI `EN55022 Class B`, chống tĩnh điện ESD.

| Chân (Pin) | Ký Hiệu | Mô Tả Kỹ Thuật |
| :---: | :---: | :--- |
| **1** | `DC +12V` | Ngõ vào nguồn dương: **+11.5V đến +13.0V DC** (Chuẩn 12.0V) |
| **2** | `DC 0V` | Ngõ vào cực âm (Mass nguồn DC) |
| **3** | `GND` | Nối đất vỏ máy / Block gá động cơ Galvo (dây vàng sọc xanh) |

> **Thông số dòng điện:** Dòng danh định 2A (đỉnh tải khi quét gia tốc lớn có thể lên tới 3A).

---

## 3. Cổng Giao Tiếp Máy Tính X6 (USB-B Connector)

* **Loại cổng:** USB Type-B tiêu chuẩn.
* **Tốc độ:** USB 2.0 Full Speed (12 Mbit/s).
* **Đặc tính kỹ thuật:**
  * **Cách ly quang / Galvanic Isolation:** Điện áp cách ly an toàn lên đến **2.5 kV**.
  * Chống sốc điện ESD: Bảo vệ lên tới **12 kV**.
  * Nguồn mạch cách ly USB được nuôi trực tiếp từ bus USB máy tính (5V, tiêu thụ < 50 mA).

---

## 4. Cổng Tín Hiệu Số X7 (Digital I/O & Laser Interface)

* **Loại cổng trên bo:** D-Sub 25 chân cái (DB-25 Female).
* **Loại đầu cắm cáp:** D-Sub 25 chân đực (DB-25 Male).
* **Chuẩn truyền dẫn:** TIA/EIA-485A (RS-485 vi sai viễn thông tốc độ cao, dùng IC Texas Instruments `SN65HVD485E`).

### 4.1 Bảng Sơ Đồ 25 Chân (Pinout Table)

| Chân Vi Sai | Tên Tín Hiệu | Chế Độ Tích Hợp (Integrated Control) | Chế Độ Chuẩn (XY2-100) | Trở Kháng Đầu Cuối |
| :---: | :---: | :--- | :--- | :--- |
| **1 / 14** | `IO1- / IO1+` | Digital I/O 1 (RS-485) | `SENDCK` (Xung Clock 2MHz) | Có trở 100 Ω |
| **2 / 15** | `IO2- / IO2+` | Digital I/O 2 (RS-485) | `SYNC` (Đồng bộ truyền dữ liệu) | Có trở 100 Ω |
| **3 / 16** | `IO3- / IO3+` | Digital I/O 3 (RS-485) | `CHANNELX` (Dữ liệu vị trí X) | Có trở 100 Ω |
| **4 / 17** | `IO4- / IO4+` | Digital I/O 4 (RS-485) | `CHANNELY` (Dữ liệu vị trí Y) | Có trở 100 Ω |
| **5 / 18** | `IO5- / IO5+` | Tín hiệu Laser Gate / Dao động 1 | Digital I/O | Không có trở |
| **6 / 19** | `IO6- / IO6+` | Tín hiệu Laser Gate / Dao động 2 | Digital I/O | Không có trở |
| **7 / 20** | `IO7- / IO7+` | Laser External Trigger / Dao động 3 | `STATUS` (Báo trạng thái galvo) | Không có trở |
| **13** | `REF_IO` | Điểm tham chiếu I/O (Nối GND qua trở 100 Ω) | Nối mass GND bo điều khiển | - |
| **21** | `DC +5V` | Nguồn cấp phụ +5V DC (Tối đa 200 mA cho mạch ngoài) | - | - |
| **8** | `GND` | Mass tín hiệu logic | Mass logic | - |
| **10, 11, 23**| `DC +12V` | Nối thông bên trong với chân 1 của cổng X5 | - | - |
| **12, 24, 25**| `DC 0V` | Nối thông bên trong với chân 2 của cổng X5 | - | - |

---

## 5. Ghép Nối Điều Khiển Nguồn Phát Laser

Bo mạch RT-CU trong chế độ **Integrated Control** tự động đồng bộ việc bật/tắt chùm tia laser với quỹ đạo chuyển động cơ học của gương quét:

1. **Laser CW (Liên tục) hoặc Tự Kích Xung Nội Bộ:**
   * Sử dụng chân `IO5` hoặc `IO6` làm tín hiệu **Laser Gate** (Bật chùm tia khi bắt đầu nét vẽ, tắt chùm tia khi kết thúc nét vẽ).
   * Lệnh cấu hình: `rtSetLaserTimes(GateOnDelay, GateOffDelay)` để bù độ trễ quang/nhiệt của buồng laser.
2. **Laser Q-Switch Kích Xung Ngoài (External Triggering):**
   * Sử dụng chân `IO7` làm xung kích Laser Trigger.
   * `IO7` được liên kết trực tiếp với **Bộ dao động phần cứng số 3 (Oscillator 3)**.
   * Tần số xung và độ rộng xung được điều khiển bằng lệnh `rtSetOscillator(3, ChuKỳ_us, ĐộRộng_us)`.
   * Bộ dao động số 3 tự động đồng bộ hóa với sườn lên (rising edge) của tín hiệu Gate, đảm bảo xung laser đầu tiên phát ra chính xác tuyệt đối tại điểm bắt đầu nét cắt.
