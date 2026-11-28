#!/usr/bin/env python3
"""
Công cụ kiểm tra tín hiệu số 7 cặp chân Digital I/O X7 (Newson Rhothor RT-CU).
Hỗ trợ kiểm tra tự động tuần tự từng cặp chân (5s/cổng) hoặc bật/tắt thủ công từng cổng
để kỹ thuật viên đo đạc kiểm tra điện áp bằng đồng hồ vạn năng (VOM) hoặc máy hiện sóng.

Sơ đồ 7 cặp chân trên cổng DB-25 X7:
  - IO1: Chân 1 (IO1-) và Chân 14 (IO1+) [Có trở 100Ω]
  - IO2: Chân 2 (IO2-) và Chân 15 (IO2+) [Có trở 100Ω]
  - IO3: Chân 3 (IO3-) và Chân 16 (IO3+) [Có trở 100Ω]
  - IO4: Chân 4 (IO4-) và Chân 17 (IO4+) [Có trở 100Ω]
  - IO5: Chân 5 (IO5-) và Chân 18 (IO5+) [Không trở] -> Kênh Chốt Công Suất Pin 9 (Latch)
  - IO6: Chân 6 (IO6-) và Chân 19 (IO6+) [Không trở] -> Kênh Mở Tia Laser Pin 19 (Emission Gate)
  - IO7: Chân 7 (IO7-) và Chân 20 (IO7+) [Không trở] -> Kênh Tần Số Xung Pin 20 (Sync/PRR)

Cách sử dụng:
  python tools/test_io_ports.py               # Chạy tự động tuần tự 5s mỗi cổng (lặp lại)
  python tools/test_io_ports.py --step-time 3 # Bật mỗi cổng 3 giây
  python tools/test_io_ports.py --port 5      # Bật cố định chỉ cổng IO5 (chân 5/18) để đo
  python tools/test_io_ports.py --all-on      # Bật tất cả 7 cổng
  python tools/test_io_ports.py --all-off     # Tắt tất cả 7 cổng
"""

import sys
import os
import time
import argparse
import signal

# Ensure project root in sys.path
PROJECT_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), ".."))
if PROJECT_ROOT not in sys.path:
    sys.path.insert(0, PROJECT_ROOT)

from sdk.python.rhothor import RhothorController, ERR_OK

IO_PORTS = [
    {"port": 1, "pins": "1 / 14", "pos": 14, "neg": 1, "name": "IO1", "desc": "RS-485 vi sai (Trở 100Ω)"},
    {"port": 2, "pins": "2 / 15", "pos": 15, "neg": 2, "name": "IO2", "desc": "RS-485 vi sai (Trở 100Ω)"},
    {"port": 3, "pins": "3 / 16", "pos": 16, "neg": 3, "name": "IO3", "desc": "RS-485 vi sai (Trở 100Ω)"},
    {"port": 4, "pins": "4 / 17", "pos": 17, "neg": 4, "name": "IO4", "desc": "RS-485 vi sai (Trở 100Ω)"},
    {"port": 5, "pins": "5 / 18", "pos": 18, "neg": 5, "name": "IO5", "desc": "Chốt Công Suất Pin 9 (Latch) / Dao động 1"},
    {"port": 6, "pins": "6 / 19", "pos": 19, "neg": 6, "name": "IO6", "desc": "Mở Tia Laser Pin 19 (Gate) / Dao động 2"},
    {"port": 7, "pins": "7 / 20", "pos": 20, "neg": 7, "name": "IO7", "desc": "Tần Số Xung Pin 20 (Sync/PRR) / Dao động 3"},
]


def print_banner():
    print("=" * 78)
    print("  RHOTHOR X7 DIGITAL I/O PORT TESTER & VOLTAGE VERIFIER")
    print("  Publisher: chaunt <chaunt.dev@gmail.com>")
    print("=" * 78)
    print("  Cổng kết nối X7 (DB-25 Male) - Chuẩn vi sai RS-485 (SN65HVD485E):")
    for p in IO_PORTS:
        print(f"    • {p['name']:4s}: Chân {p['pins']:6s} (Pin {p['pos']:2d}: IO+ | Pin {p['neg']:2d}: IO-) -> {p['desc']}")
    print("-" * 78)
    print("  💡 HƯỚNG DẪN ĐO ĐỒNG HỒ VOM (THANG DC 20V):")
    print("    • Que ĐỎ cắm Pin IO+ (14..20), que ĐEN cắm Pin IO- (1..7).")
    print("    • KHI BẬT (HIGH): Đồng hồ đo được +2.5V đến +4.5V (hoặc đo IO+ so GND Pin 8: ~ +3.3V đến +5.0V).")
    print("    • KHI TẮT (LOW):  Đồng hồ đo được -2.5V hoặc 0V.")
    print("=" * 78 + "\n")


def run_auto_step_test(galvo: RhothorController, step_time: float, loop: bool):
    print(f"[*] Bắt đầu bài test tự động tuần tự: {step_time:.1f} giây mỗi cặp chân.")
    print(f"    Chế độ lặp lại: {'BẬT (Nhấn Ctrl+C để dừng)' if loop else 'TẮT (Chạy 1 vòng rồi kết thúc)'}\n")

    cycle = 1
    try:
        while True:
            print(f">>> BẮT ĐẦU CHU KỲ KIỂM TRA #{cycle} <<<")
            for p in IO_PORTS:
                port_nr = p["port"]
                pins = p["pins"]
                pos = p["pos"]
                neg = p["neg"]
                name = p["name"]

                # Turn all off first, then turn on only this port
                galvo.set_all_io_ports(False)
                galvo.set_io_port(port_nr, True)

                print(f"\n[⚡ BẬT] {name} - CẶP CHÂN {pins} ({p['desc']})")
                print(f"       👉 HƯỚNG DẪN: Que ĐỎ cắm Pin {pos} ({name}+), Que ĐEN cắm Pin {neg} ({name}-)")
                print(f"       👉 Mức điện áp dự kiến: +2.5V đến +4.5V (hoặc que Đỏ Pin {pos}, que Đen Pin 8 GND: ~3.5V..5V)")

                # Countdown
                steps = int(step_time * 10)
                for step in range(steps, 0, -1):
                    sec_left = step / 10.0
                    bar_len = 25
                    filled = int((1.0 - (step / steps)) * bar_len)
                    bar = "█" * filled + "░" * (bar_len - filled)
                    sys.stdout.write(f"\r       Đang đếm ngược: [{bar}] còn {sec_left:4.1f}s ")
                    sys.stdout.flush()
                    time.sleep(0.1)

                # Turn off this port
                galvo.set_io_port(port_nr, False)
                sys.stdout.write(f"\r       [⭕ ĐÃ TẮT] {name} (Chân {pins}) -> Đồng hồ đã hạ về 0V / -2.5V.         \n")
                sys.stdout.flush()

                # Short 0.5s pause between ports
                time.sleep(0.5)

            if not loop:
                print("\n[+] Đã hoàn thành 1 chu trình kiểm tra đầy đủ 7 cặp chân!")
                break
            cycle += 1

    except KeyboardInterrupt:
        print("\n\n[!] Đã nhận lệnh dừng từ người dùng (Ctrl+C).")
    finally:
        galvo.set_all_io_ports(False)
        print("[+] Đã ngắt toàn bộ 7 cổng I/O về mức an toàn (0V).\n")


def main():
    parser = argparse.ArgumentParser(description="Rhothor X7 Digital I/O Port Step Tester & Voltage Verifier")
    parser.add_argument("--step-time", type=float, default=5.0, help="Thời gian giữ BẬT mỗi cổng (giây, mặc định: 5.0s)")
    parser.add_argument("--no-loop", action="store_true", help="Chỉ chạy 1 vòng từ IO1 đến IO7 rồi dừng, không lặp")
    parser.add_argument("--port", type=int, choices=range(1, 8), help="Bật cố định 1 cổng duy nhất (1..7)")
    parser.add_argument("--test-fiber-sync", action="store_true", help="Kiểm tra đồng bộ 3 kênh Fiber Laser (Latch Pin 9 -> Gate Pin 19 + Sync Pin 20)")
    parser.add_argument("--all-on", action="store_true", help="Bật tất cả 7 cổng cùng lúc")
    parser.add_argument("--all-off", action="store_true", help="Tắt tất cả 7 cổng")
    parser.add_argument("--serial", type=int, default=None, help="Mã Serial Number của Galvo Rhothor (mặc định: auto-detect)")
    parser.add_argument("--simulate", action="store_true", help="Chạy mô phỏng nếu chưa gắn phần cứng")

    args = parser.parse_args()

    print_banner()

    # Handle Ctrl+C gracefully
    def sig_handler(sig, frame):
        print("\n[!] Thoát chương trình...")
        sys.exit(0)
    signal.signal(signal.SIGINT, sig_handler)

    try:
        galvo = RhothorController(serial_nr=args.serial, simulate=args.simulate)
        galvo.connect()
    except Exception as e:
        print(f"[!] Không thể kết nối phần cứng Galvo (Lỗi: {e}).")
        print("[!] Đang tự động chuyển sang chế độ Mô Phỏng (Simulation) để chạy thử...\n")
        galvo = RhothorController(serial_nr=args.serial, simulate=True)
        galvo.connect()

    try:
        if args.all_on:
            print("[*] Đang BẬT TẤT CẢ 7 CỔNG DIGITAL I/O (IO1 đến IO7)...")
            galvo.set_all_io_ports(True)
            print("[+] Đã bật thành công! Nhấn Enter để tắt và thoát...")
            input()
            galvo.set_all_io_ports(False)
        elif args.all_off:
            print("[*] Đang TẮT TẤT CẢ 7 CỔNG DIGITAL I/O...")
            galvo.set_all_io_ports(False)
            print("[+] Đã tắt hoàn toàn 7 cổng.")
        elif args.test_fiber_sync:
            print("[*] BẮT ĐẦU TEST ĐỒNG BỘ 3 KÊNH NGUỒN FIBER LASER:")
            print("    1. [Chốt Công Suất]: X7 Pin 18 (IO5+) & Pin 5 (IO5-) -> MAX485 #3 -> Laser Pin 9 (Latch)")
            print("    2. [Mở Tia Laser] : X7 Pin 19 (IO6+) & Pin 6 (IO6-) -> MAX485 #1 -> Laser Pin 19 (Gate)")
            print("    3. [Tần Số Xung]  : X7 Pin 20 (IO7+) & Pin 7 (IO7-) -> MAX485 #2 -> Laser Pin 20 (Sync/PRR)")
            print("-" * 78)
            print("    ⚡ Bước 1: Phát xung Latch chốt công suất trên IO5 (Chân 5/18)...")
            galvo.send_latch_pulse(pin_bit=4)
            time.sleep(0.1)
            print("    ⚡ Bước 2: BẬT Mở Tia Gate (IO6) và Xung Sync 50kHz (IO7) trong 3.0 giây...")
            galvo.set_laser_frequency(50.0, 3.0)
            mask = (1 << 5) | (1 << 6)
            galvo.set_io(value=mask, mask=mask)
            for s in range(30, 0, -1):
                sys.stdout.write(f"\r       Đang phát tia test: còn {s/10.0:4.1f}s... ")
                sys.stdout.flush()
                time.sleep(0.1)
            galvo.set_io(value=0, mask=mask)
            print("\n    [+] ĐÃ TẮT AN TOÀN toàn bộ ngõ ra laser.")
            print("[+] Test đồng bộ 3 kênh Fiber Laser thành công!")
        elif args.port:
            p = IO_PORTS[args.port - 1]
            print(f"[*] Bật cố định cổng {p['name']} (Chân {p['pins']} - {p['desc']})...")
            print(f"    👉 Cắm que ĐỎ vào Pin {p['pos']} ({p['name']}+), que ĐEN vào Pin {p['neg']} ({p['name']}-)")
            print(f"    👉 Mức điện áp dự kiến: +2.5V đến +4.5V (hoặc đo que Đỏ Pin {p['pos']} so Pin 8 GND: ~3.5V..5V)")
            galvo.set_io_port(args.port, True)
            print("\n[+] Đang giữ cổng ở mức HIGH. Nhấn Enter để tắt và kết thúc...")
            input()
            galvo.set_io_port(args.port, False)
            print("[+] Đã tắt cổng.")
        else:
            run_auto_step_test(galvo, step_time=args.step_time, loop=not args.no_loop)

    finally:
        galvo.disconnect()


if __name__ == "__main__":
    main()
