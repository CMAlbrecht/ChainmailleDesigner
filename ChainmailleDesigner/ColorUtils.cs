// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorUtils.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System.Collections.Generic;
using System.Drawing;

// Tuples for colors in specific color spaces.
using HslColor = System.Tuple<int, int, int>;
using LabColor = System.Tuple<double, double, double>;
using RgbColor = System.Tuple<int, int, int>;
using XyzColor = System.Tuple<double, double, double>;

namespace ChainmailleDesigner
{
  public static class ColorUtils
  {

    public static Color HslToRgb(HslColor color)
    {
      RgbColor rgb = ColorConverter.HslToRgb(color);
      return Color.FromArgb(255, rgb.Item1, rgb.Item2, rgb.Item3);
    }

    public static Color LabToRgb(LabColor color)
    {
      RgbColor rgb = ColorConverter.LabToRgb(color);
      return Color.FromArgb(255, rgb.Item1, rgb.Item2, rgb.Item3);
    }

    public static HslColor RgbToHsl(Color color)
    {
      return ColorConverter.RgbToHsl(new RgbColor(color.R, color.G, color.B));
    }

    public static LabColor RgbToLab(Color color)
    {
      return ColorConverter.RgbToLab(new RgbColor(color.R, color.G, color.B));
    }

    public static XyzColor RgbToXyz(Color color)
    {
      return ColorConverter.RgbToXyz(new RgbColor(color.R, color.G, color.B));
    }

    public static Color XyzToRgb(XyzColor color)
    {
      RgbColor rgb = ColorConverter.XyzToRgb(color);
      return Color.FromArgb(255, rgb.Item1, rgb.Item2, rgb.Item3);
    }

    public static LabColor ColorAverage(List<LabColor> colors)
    {
      double[] colorSum = new double[3] { 0, 0, 0 };
      if (colors.Count > 0)
      {
        foreach (LabColor color in colors)
        {
          colorSum[0] += color.Item1;
          colorSum[1] += color.Item2;
          colorSum[2] += color.Item3;
        }

        for (int i = 0; i < 3; i++)
        {
          colorSum[i] /= colors.Count;
        }
      }

      return new LabColor(colorSum[0], colorSum[1], colorSum[2]);
    }

  }
}
