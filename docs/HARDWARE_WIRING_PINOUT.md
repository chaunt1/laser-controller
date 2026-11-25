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

---

## 6. Sơ Đồ Đấu Nối Hoàn Chỉnh Ghép Nối Nguồn Fiber Laser (Raycus / JPT / Max / IPG) Trực Tiếp (Cách 2)

Giải pháp này cho phép hệ thống điều khiển trực tiếp nguồn phát Fiber Laser công nghiệp thông qua cổng DB-25 mà **không cần thông qua card trung gian EZCAD (JCZ LMCV4)**.

> [!CAUTION]
> **CẢNH BÁO NGUY HIỂM - NGUY CƠ CHÁY HỎNG NGUỒN FIBER LASER**:
> * **TUYỆT ĐỐI KHÔNG DÙNG CÁP DB-25 ĐẦU THẲNG (1:1) CẮM TRỰC TIẾP TỪ CỔNG X7 VÀO NGUỒN FIBER!**
> * Trên cổng X7 của Rhothor, **chân 10, 11, 23 là nguồn +12V DC** (nối thông từ cổng nguồn X5). Trong khi đó, trên nguồn Fiber Laser, **chân 23 là Khóa an toàn Interlock/E-Stop (logic TTL 5V)** và **chân 11 là ngõ ra báo lỗi/báo sẵn sàng (TTL 5V)**. Nếu cắm cáp 1:1, điện áp 12V sẽ xông thẳng vào vi điều khiển của nguồn laser và phá hủy bo mạch nguồn!
> * Cần sử dụng mạch đệm/chuyển đổi tín hiệu (Adapter Board) như hướng dẫn bên dưới.

---

### 6.1 Sơ Đồ Khối & Nguyên Lý Hoạt Động

Nguồn Fiber Laser yêu cầu các nhóm tín hiệu chuẩn TTL (0–5V):
1. **D0 – D7 (Chân 1 – 8):** Xe bus 8-bit đặt công suất laser (00h = 0%, FFh = 100%).
2. **Latch (Chân 9):** Xung sườn âm/dương nạp thanh ghi công suất từ D0–D7 vào bộ nhớ đệm DAC nội bộ.
3. **Emission Gate (Chân 19):** Mức logic HIGH để phát tia theo từng nét khắc.
4. **Sync / PRR (Chân 20):** Xung clock kích tần số phát tia (20 kHz – 100 kHz).
5. **MO / Pump (Chân 18):** Mức HIGH để bật bộ dao động chủ (Master Oscillator).
6. **Red Guide (Chân 22):** Mức HIGH để bật đèn laser dẫn đường đỏ (660nm).
7. **Emergency Stop / Interlock (Chân 23):** Giữ mức HIGH (5V) liên tục để cho phép nguồn phát tia.

Do cổng X7 của Rhothor sử dụng chuẩn **RS-485 vi sai (Differential)** cho các chân IO, ta sử dụng **3 IC thu phát RS-485 (MAX485 / SN75176 / SN65HVD485E)** được cấp nguồn 5V lấy từ chính chân 21 của cổng X7.

```
+---------------------------+                      +-------------------------------+
|  RHOTHOR X7 (DB-25 Male)  |                      | FIBER LASER SOURCE (DB-25 Fem)|
|                           |                      | (Raycus, JPT, Max, IPG)       |
|  Pin 21 (+5V DC Out) ---->+======[ VCC (+5V) ]==>+-- Pin 23 (Interlock 5V)       |
|  Pin 8, 12 (GND) -------->+======[ GND (0V) ]===>+-- Pin 10, 14, 24, 25 (GND)    |
|                           |   |                  |                               |
|  Pin 5/18 (IO5- / IO5+) ->+---+-> [MAX485 #1] --->+-- Pin 9 (Power Latch Pulse)  |
|  Pin 6/19 (IO6- / IO6+) ->+---+-> [MAX485 #2] --->+-- Pin 19 (Emission Gate)      |
|  Pin 7/20 (IO7- / IO7+) ->+---+-> [MAX485 #3] --->+-- Pin 20 (Laser Sync / PRR)  |
|                           |   |                  |                               |
|  Công tắc ngoài / DipSW   |   |                  |                               |
|  D0-D7 (Treo trở 10k lên 5V)                     +-- Pin 1-8 (Power Bus D0-D7)   |
|  MO Switch (Công tắc 5V)                         +-- Pin 18 (Main Oscillator)    |
|  Guide Switch (Công tắc 5V)                      +-- Pin 22 (Guide Red Laser)    |
+---------------------------+                      +-------------------------------+
```

---

### 6.2 Bảng Đấu Dây Chi Tiết Từng Chân (Master Pin-to-Pin Mapping Table)

| STT | Tín Hiệu Laser | Chân Laser DB-25 | Nguồn Tín Hiệu Từ Rhothor X7 | Phương Thức Xử Lý & Ghép Nối |
| :---: | :--- | :---: | :--- | :--- |
| **1** | **D0 (LSB)** | **Pin 1** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **2** | **D1** | **Pin 2** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **3** | **D2** | **Pin 3** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **4** | **D3** | **Pin 4** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **5** | **D4** | **Pin 5** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **6** | **D5** | **Pin 6** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **7** | **D6** | **Pin 7** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **8** | **D7 (MSB)** | **Pin 8** | Nguồn 5V phụ qua trở / Dip Switch | Mức 1 (5V) qua trở kéo 4.7kΩ lên 5V (hoặc gạt mass) |
| **9** | **LATCH (Chốt CS)**| **Pin 9** | **X7 Pin 5 (IO5-) & Pin 18 (IO5+)** | Qua IC MAX485 #1 (chân RO xuất TTL 5V vào Pin 9) |
| **10**| **GND (Mass logic)**| **Pin 10** | **X7 Pin 8 (GND)** | Nối chung mass logic giữa Galvo và Laser |
| **11**| **Alarm: Warning**| **Pin 11** | Đèn LED báo lỗi (Tùy chọn) | Đấu qua LED + trở 1kΩ xuống GND (KHÔNG ĐẤU VÀO X7!) |
| **12**| **Alarm: Temp** | **Pin 12** | Đèn LED báo lỗi nhiệt | Đấu qua LED + trở 1kΩ xuống GND |
| **13**| **Reserved** | **Pin 13** | Bỏ trống (NC) | Không đấu nối |
| **14**| **GND (Mass logic)**| **Pin 14** | **X7 Pin 8 (GND)** | Nối chung mass |
| **15**| **Reserved** | **Pin 15** | Bỏ trống (NC) | Không đấu nối |
| **16**| **Alarm: Status** | **Pin 16** | Đèn LED báo trạng thái | Đấu qua LED + trở 1kΩ |
| **17**| **Reserved** | **Pin 17** | Bỏ trống (NC) | Không đấu nối |
| **18**| **MO (Bật Bơm)** | **Pin 18** | Công tắc cấp 5V từ **X7 Pin 21** | Đóng mức HIGH (5V) trước khi khắc 0.5s – 5s |
| **19**| **EMISSION GATE** | **Pin 19** | **X7 Pin 6 (IO6-) & Pin 19 (IO6+)** | Qua IC MAX485 #2 (chân RO xuất TTL 5V vào Pin 19) |
| **20**| **SYNC / PRR** | **Pin 20** | **X7 Pin 7 (IO7-) & Pin 20 (IO7+)** | Qua IC MAX485 #3 (chân RO xuất xung vào Pin 20) |
| **21**| **Alarm: Power** | **Pin 21** | Đèn LED báo nguồn | Đấu qua LED báo (KHÔNG ĐẤU VÀO X7 PIN 21!) |
| **22**| **GUIDE LASER** | **Pin 22** | Công tắc cấp 5V từ **X7 Pin 21** | Đóng công tắc sang 5V để bật tia đỏ định vị phôi |
| **23**| **INTERLOCK / ESTOP**| **Pin 23** | **X7 Pin 21 (+5V DC)** qua Nút Dừng | Nối thẳng 5V (hoặc qua nút E-Stop cơ học thường đóng) |
| **24**| **GND (Mass logic)**| **Pin 24** | **X7 Pin 8 / 12 (GND)** | Nối chung mass |
| **25**| **GND (Mass logic)**| **Pin 25** | **X7 Pin 8 / 12 (GND)** | Nối chung mass |

---

### 6.3 Sơ Đồ Đấu Mạch Chuyển Đổi MAX485 (RS-485 sang TTL)

Ba IC MAX485 (hoặc module chuyển RS485-TTL bán sẵn) được mắc ở chế độ **Receiver liên tục** (chân $\overline{\text{RE}}$ và DE nối xuống GND):

```
                       SƠ ĐỒ NGUYÊN LÝ 1 KÊNH MAX485
                      +-----------------------------+
                      |         IC MAX485           |
                      |                             |
                      | 1: RO (TTL Out) ------------+---> Tới Laser DB-25 (Pin 9 / 19 / 20)
                      | 2: /RE (Receiver Enable) ---+---+
                      | 3: DE (Driver Enable) ------+   | (Nối mass để luôn nhận)
                      | 4: DI (Driver In - NC)      |   |
                      | 5: GND ---------------------+---+---> Nối GND (X7 Pin 8)
                      | 6: A (Non-inverting Input) -+-------> Nối X7 Pin IO+ (18, 19, hoặc 20)
                      | 7: B (Inverting Input) -----+-------> Nối X7 Pin IO- (5, 6, hoặc 7)
                      | 8: VCC (+5V) ---------------+-------> Nối X7 Pin 21 (+5V DC)
                      +-----------------------------+
```

* **Kênh 1 (Xung Chốt Latch Pin 9):**
  * X7 Pin 18 (`IO5+`) $\rightarrow$ Chân 6 (A) của MAX485 #1
  * X7 Pin 5 (`IO5-`) $\rightarrow$ Chân 7 (B) của MAX485 #1
  * Chân 1 (RO) của MAX485 #1 $\rightarrow$ Pin 9 của Laser DB-25
* **Kênh 2 (Xung Phát Tia Emission Gate Pin 19):**
  * X7 Pin 19 (`IO6+`) $\rightarrow$ Chân 6 (A) của MAX485 #2
  * X7 Pin 6 (`IO6-`) $\rightarrow$ Chân 7 (B) của MAX485 #2
  * Chân 1 (RO) của MAX485 #2 $\rightarrow$ Pin 19 của Laser DB-25
* **Kênh 3 (Tần Số Xung Laser Sync/PRR Pin 20):**
  * X7 Pin 20 (`IO7+`) $\rightarrow$ Chân 6 (A) của MAX485 #3
  * X7 Pin 7 (`IO7-`) $\rightarrow$ Chân 7 (B) của MAX485 #3
  * Chân 1 (RO) của MAX485 #3 $\rightarrow$ Pin 20 của Laser DB-25

---

### 6.4 Nguyên Lý Chốt Công Suất (Pin 9 Latch) Bằng Phần Mềm

1. Để thiết lập công suất (ví dụ: gạt bus $D_0 \dots D_7$ ở mức 80% hoặc 100% bằng Dip Switch), giá trị điện áp chỉ lưu vào thanh ghi nội khi chân 9 nhận được xung cạnh kích hoạt ($\ge 10\,\mu\text{s}$).
2. Phần mềm **Rhothor Laser Studio v1.2.0** đã tự động hóa:
   * **Tự động chốt khi bắt đầu khắc:** Trong luồng gia công `MarkWorker`, trước khi phát lệnh vẽ nét vector đầu tiên, phần mềm tự động gọi hàm `controller.send_latch_pulse()` gửi xung vi sai trên kênh `IO5` (Pin 5/18) để khóa chắc chắn mức công suất mong muốn.
   * **Kích xung chốt thủ công:** Trên giao diện Tab 4 (I/O & Kích Xung), người dùng có thể bấm nút **⚡ PHÁT XUNG LATCH CHỐT CÔNG SUẤT (PIN 9)** bất kỳ lúc nào để nạp lại giá trị công suất sau khi gạt thay đổi switch phần cứng.
