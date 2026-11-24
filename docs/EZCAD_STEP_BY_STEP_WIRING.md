# Hướng Dẫn Từng Bước Đấu Dây Cáp & Cài Đặt EZCAD (Step-by-Step Cookbook)

Tài liệu này là cẩm nang thực hành chi tiết nhất giúp bạn tự làm cáp kết nối từ card **EzCad JCZ LMC** sang đầu quét **Newson Rhothor (LPKF #4281)** và cấu hình phần mềm EzCad hoạt động chính xác 100%.

---

## 1. Chuẩn Bị Vật Tư Cần Thiết

1. **Đầu cắm DB-15 Đực (DB-15 Male with Hood):** Cắm vào cổng `CON1` trên card EzCad JCZ LMC.
2. **Đầu cắm DB-25 Đực (DB-25 Male with Hood):** Cắm vào cổng `X7` trên bo Newson Rhothor RT-CU.
3. **Dây cáp bọc kim chống nhiễu (Shielded Twisted Pair):**
   * Khuyên dùng: Dây mạng xoắn đôi chống nhiễu **Cat6 STP/FTP** (có 4 cặp dây xoắn và lớp vỏ bọc lá nhôm + dây mass chống nhiễu).
   * Chiều dài khuyến nghị: Từ 0.5 mét đến 1.5 mét (không nên làm quá dài để tránh suy hao xung nhịp 2MHz).
4. Mỏ hàn thiếc, đồng hồ vạn năng (để đo thông mạch kiểm tra trước khi cắm điện).

---

## 2. Bảng Sơ Đồ Hàn Dây Chi Tiết (Kèm Màu Dây Khuyến Nghị)

Các tín hiệu điều khiển galvo là **tín hiệu số vi sai (Differential RS-422/RS-485)**, do đó **bắt buộc** mỗi cặp tín hiệu (+ và -) phải đi chung trên một cặp dây xoắn đôi với nhau:

| Cặp Dây Xoắn (Twisted Pair) | Tín Hiệu | Chân Card EzCad `CON1` (DB-15 Đực) | Chân Bo Rhothor `X7` (DB-25 Đực) | Màu Dây Đề Xuất (Theo chuẩn Cat6) |
| :---: | :--- | :---: | :---: | :--- |
| **Cặp 1** *(Clock 2MHz)* | **CLK -**<br>**CLK +** | **Chân 1**<br>**Chân 9** | **Chân 1** (IO1-)<br>**Chân 14** (IO1+) | Cam - Trắng<br>Cam |
| **Cặp 2** *(Sync)* | **SYNC -**<br>**SYNC +** | **Chân 2**<br>**Chân 10** | **Chân 2** (IO2-)<br>**Chân 15** (IO2+) | Xanh Lá - Trắng<br>Xanh Lá |
| **Cặp 3** *(X Axis)* | **X Channel -**<br>**X Channel +** | **Chân 3**<br>**Chân 11** | **Chân 3** (IO3-)<br>**Chân 16** (IO3+) | Xanh Dương - Trắng<br>Xanh Dương |
| **Cặp 4** *(Y Axis)* | **Y Channel -**<br>**Y Channel +** | **Chân 4**<br>**Chân 12** | **Chân 4** (IO4-)<br>**Chân 17** (IO4+) | Nâu - Trắng<br>Nâu |
| **Dây phụ** *(Status)* | **Status -**<br>**Status +** | **Chân 6**<br>**Chân 14** | **Chân 7** (IO7-)<br>**Chân 20** (IO7+) | Dây sợi rời |
| **Dây Mass** *(GND)* | **GND** | **Chân 8 & 15** (nối chung) | **Chân 8 & 13** (REF_IO, nối chung)| Dây mass trần của cáp |
| **Vỏ chống nhiễu** | **Shield Earth** | Vỏ sắt kẹp DB-15 | Vỏ sắt kẹp DB-25 | Lưới kim loại bọc ngoài |

> ⚠️ **Lưu ý kiểm tra quan trọng:**
> Dùng thang đo thông mạch (bíp) trên đồng hồ vạn năng để kiểm tra từng chân trước khi cắm vào máy. Tuyệt đối không để chập giữa chân tín hiệu với chân vỏ hoặc chân nguồn 12V!

---

## 3. Cấu Hình Bo Mạch Rhothor Sang Chế Độ XY2-100

Trước khi card EzCad có thể điều khiển đầu quét, bạn cần kích hoạt bộ giải mã XY2-100 của Rhothor một lần duy nhất:

1. Cấp nguồn 12VDC vào cổng X5, cắm tạm cáp USB vào cổng X6.
2. Mở phần mềm `Config.exe` (trong thư mục `vendor/rhothor_exe/src/Config.exe`).
3. Tại giao diện cấu hình:
   * Chuyển mục chế độ làm việc từ **Integrated Control** sang **XY2-100**.
   * Nhấn nút **Store** (hoặc *Write to Flash*) để lưu vĩnh viễn cấu hình vào bộ nhớ bo mạch.
4. Tắt nguồn bo mạch, rút cáp USB ra (từ nay về sau bo mạch sẽ nhận lệnh hoàn toàn qua cáp DB-25 vừa hàn).

---

## 4. Cài Đặt Tham Số Trong Phần Mềm EZCAD (Phím F3)

Mở phần mềm EzCad2 hoặc EzCad3 $\rightarrow$ Nhấn phím **`F3`** (hoặc vào menu `Param`):

### 1. Tab `Field` (Vùng làm việc)
* **Field (Vùng quét):** Nhập `201.65` mm (theo đúng thông số thấu kính máy LPKF).

### 2. Tab `Galvo 1` (Trục X) & `Galvo 2` (Trục Y)
* **Galvo 1 = X**, **Galvo 2 = Y**.
* Cài đặt tốc độ cơ bản:
  * **Jump Speed:** `5000` đến `6000` mm/s.
  * **Jump Delay:** `200` µs.
  * **Laser On Delay:** `50` đến `80` µs.
  * **Laser Off Delay:** `50` đến `80` µs.
  * **Mark Delay:** `100` µs.
  * **Polygon Delay:** `50` µs.

### 3. Khắc phục chiều chuyển động (Nếu hình bị lộn ngược)
* Nếu khắc ra chữ bị lật ngược gương trái $\leftrightarrow$ phải: Tích chọn ô **`Inverse`** ở mục **Galvo 1 (X)**.
* Nếu khắc ra chữ bị lộn ngược đầu $\updownarrow$ đuôi: Tích chọn ô **`Inverse`** ở mục **Galvo 2 (Y)**.
* Nếu chữ bị xoay nghiêng $90^\circ$: Đổi chỗ chọn cổng Galvo 1 thành Y và Galvo 2 thành X.

---

## 5. Hiệu Chuẩn Tỉ Lệ Kích Thước (Scale Ratio) Trong 1 Phút

Để nét cắt đạt độ chính xác tới $0.01\ \text{mm}$:
1. Trên EzCad, vẽ một hình vuông kích thước chính xác **`50.00 mm x 50.00 mm`**.
2. Nhấn nút **Mark (F2)** để khắc thử lên tấm nhôm hoặc phôi thử.
3. Dùng thước kẹp điện tử đo kích thước thực tế vừa khắc ra:
   * Giả sử trục X đo được: `49.80 mm`.
   * Giả sử trục Y đo được: `50.15 mm`.
4. Nhấn **F3** $\rightarrow$ chọn nút **`Scale`**:
   * Nhập kích thước lý thuyết: `50.0`
   * Nhập kích thước đo được: `49.80` (cho trục X) và `50.15` (cho trục Y).
5. Bấm **OK**. EzCad sẽ tự động nhân hệ số bù bước xung. Từ lúc này, mọi bản vẽ sẽ khắc ra chính xác tuyệt đối theo kích thước thật!
