#!/usr/bin/env python3
"""
Unit Test Suite for Rhothor Laser Studio and Vector Parsers.
Tests DXF parsing, PLT parsing, MainWindow instantiation, and tab functions.
"""

import os
import sys
import unittest

sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from apps.easylaser.parsers import parse_dxf_content, parse_plt_content


class TestVectorParsers(unittest.TestCase):

    def test_dxf_parsing(self):
        sample_dxf = """0
SECTION
2
ENTITIES
0
LINE
10
-20.0
20
-20.0
11
20.0
21
-20.0
0
CIRCLE
10
0.0
20
0.0
40
15.0
0
ENDSEC
0
EOF"""
        res = parse_dxf_content(sample_dxf)
        self.assertEqual(res["format"], "DXF")
        self.assertGreater(len(res["strokes"]), 0)
        self.assertIn("bbox", res)
        self.assertAlmostEqual(res["bbox"]["min_x"], -20.0)

    def test_plt_parsing(self):
        sample_plt = "IN;SP1;PU-500,-500;PD500,-500,500,500,-500,500,-500,-500;PU;"
        res = parse_plt_content(sample_plt)
        self.assertEqual(res["format"], "PLT")
        self.assertGreater(len(res["strokes"]), 0)
        self.assertIn("bbox", res)


class TestStudioGUI(unittest.TestCase):

    @classmethod
    def setUpClass(cls):
        os.environ["QT_QPA_PLATFORM"] = "offscreen"
        from PyQt5.QtWidgets import QApplication
        cls.app = QApplication.instance() or QApplication(sys.argv)

    def test_main_window_instantiation_and_tabs(self):
        from apps.rhothor_studio.main_window import MainWindow

        win = MainWindow(serial=4281, name="LPKF")
        self.assertEqual(win.tabs.count(), 5)
        self.assertTrue(win.controller.is_connected)

        # Test tab 2D shapes
        win.tab_2d.generate_border()
        self.assertEqual(len(win.active_strokes), 1)

        win.tab_2d.generate_concentric_circles()
        self.assertEqual(len(win.active_strokes), 4)

        # Test tab 3D stepped planes
        win.tab_3d.generate_stepped_planes()
        self.assertEqual(len(win.active_strokes), 3)

        # Test tab timing matrix
        win.tab_timing.generate_matrix()
        self.assertEqual(len(win.active_strokes), 5)

        # Test tab I/O burst pattern
        win.tab_io.generate_burst_pattern()
        self.assertEqual(len(win.active_strokes), 5)

        win.close()


if __name__ == "__main__":
    unittest.main()
