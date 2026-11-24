# Hướng Dẫn Đóng Gói Ứng Dụng Windows (.EXE) - Rhothor Laser Studio

Tài liệu này hướng dẫn chi tiết cách build **Rhothor Laser Studio** từ mã nguồn Python thành một file thực thi độc lập duy nhất (`RhothorStudio.exe`) trên hệ điều hành **Windows 10, Windows 11 và Windows 7 (32-bit & 64-bit)**.

---

## 1. Yêu Cầu Môi Trường Để Build

Trên máy tính Windows dùng để build file `.exe`, bạn chỉ cần:
1. **Python 3.8 – 3.12** tải từ [python.org](https://www.python.org/downloads/).
   > [!IMPORTANT]
   > Khi cài đặt Python trên Windows, **BẮT BUỘC** tích chọn ô: **`Add Python to PATH`** ở màn hình cài đặt đầu tiên.

---

## 2. Cách Build Tự Động 1-Click (Khuyên Dùng)

Chúng tôi đã tạo sẵn script tự động hóa hoàn toàn quy trình tải thư viện và build:

1. Mở thư mục dự án trên máy Windows.
2. Vào thư mục `tools\windows_setup\`.
3. Nhấp đúp chuột vào file:
   ```cmd
   build_windows_app.bat
   ```
4. Script sẽ tự động:
   - Kiểm tra môi trường Python.
   - Cài đặt `PyQt5` và `pyinstaller` qua pip.
   - Biên dịch toàn bộ mã nguồn, DLL và tài nguyên theo file `RhothorStudio.spec`.
   - Tạo ra file thực thi:
     ```
     dist\RhothorStudio.exe
     ```

---

## 3. Cách Build Thủ Công Bằng Dòng Lệnh (Command Prompt / PowerShell)

Nếu bạn muốn build thủ công bằng terminal:

```cmd
# 1. Di chuyển vào thư mục gốc của dự án
cd rhothor_laser_controller

# 2. Cài đặt các thư viện cần thiết
python -m pip install --upgrade pip
python -m pip install PyQt5 pyinstaller

# 3. Chạy lệnh đóng gói bằng file Spec
pyinstaller --clean RhothorStudio.spec
```

Khi tiến trình hoàn thành, file `.exe` sẽ nằm tại thư mục `dist\RhothorStudio.exe`.

---

## 4. Cấu Trúc File Spec (`RhothorStudio.spec`)

File spec đã được cấu hình tối ưu sẵn:
- **`console=False`**: Chế độ GUI thuần túy, không bị bật cửa sổ cmd đen khi người vận hành nhấp mở app.
- **`icon='resources/icon.ico'`**: Gán icon laser công nghiệp sắc nét 256x256 px.
- **Tự động bundle DLL**:
  - `sdk/lib/x64/rhothor.dll` (dành cho Windows 64-bit)
  - `sdk/lib/x86/rhothor.dll` (dành cho Windows 32-bit)
  - `vendor/rhothor_exe/rhothor.dll`
- **Tự động đóng gói tài nguyên**:
  - Các file mẫu CAD: `examples/sample_ezcad.dxf`
  - Giao diện web phụ trợ: `apps/easylaser/static/`
- **Loại trừ thư viện thừa (`excludes`)**: Loại bỏ `matplotlib`, `numpy`, `scipy`, `pandas` để giữ dung lượng file `.exe` nhẹ (~50MB), khởi động tức thì dưới 1 giây.

---

## 5. Phân Phối Cho Khách Hàng / Công Nhân Vận Hành

Sau khi build xong:
1. Bạn chỉ cần copy duy nhất file:
   ```
   dist\RhothorStudio.exe
   ```
2. Gửi sang bất kỳ máy tính Windows nào của xưởng (kể cả máy không có Python, không có Internet).
3. Người vận hành chỉ cần **nhấp đúp chuột vào `RhothorStudio.exe`** là phần mềm mở lên ngay lập tức!
4. Nếu kết nối với máy laser thật, đảm bảo máy tính đã cài driver WinUSB cho Rhothor RT-CU (xem hướng dẫn tại [WINDOWS_10_11_SETUP.md](file:///home/chaunt/rhothor_laser_controller/docs/WINDOWS_10_11_SETUP.md)).
