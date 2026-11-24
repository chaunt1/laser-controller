"""
Vector & Drawing Parsers for EasyLaser Web and CNC Importers.
Seamlessly integrates with sdk.python.cad_parsers.
"""

import os
from typing import Dict, Any

from sdk.python.cad_parsers import (
    VectorStroke,
    parse_dxf_content,
    parse_svg_content,
    parse_plt_content,
    parse_ai_content,
    parse_lightburn_content,
    parse_ezcad_content,
    convert_raster_image_to_laser_toolpath,
    load_any_cad_drawing
)


# Backwards compatibility alias
VectorPath = VectorStroke


def parse_vector_file(filepath: str) -> Dict[str, Any]:
    """Universal loader detecting file format by extension and returning normalized toolpaths."""
    return load_any_cad_drawing(filepath)
