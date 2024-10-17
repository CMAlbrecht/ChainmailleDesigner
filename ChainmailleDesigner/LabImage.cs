// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: LabImage.cs


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

using LabColor = System.Tuple<double, double, double>;

namespace ChainmailleDesigner
{
  /// <summary>
  /// Represents an image in CIE L*a*b* colors.
  /// </summary>
  public class LabImage
  {
    private int width = 0;
    private int height = 0;
    private Dictionary<int, Dictionary<int, LabColor>> pixels =
      new Dictionary<int, Dictionary<int, LabColor>>();

    public LabImage(Bitmap bmp)
    {
      width = bmp.Width;
      height = bmp.Height;
      ConvertBitmap(bmp);
    }

    public LabImage(int imageWidth, int imageHeight)
    {
      width = imageWidth;
      height = imageHeight;
      Clear();
    }

    public void Clear()
    {
      pixels.Clear();
      for (int x = 0; x < width; x++)
      {
        pixels.Add(x, new Dictionary<int, LabColor>());
        for (int y = 0; y < height; y++)
        {
          pixels[x].Add(y, new LabColor(0, 0, 0));
        }
      }
    }

    private void ConvertBitmap(Bitmap bmp)
    {
      pixels.Clear();
      for (int x = 0; x < width; x++)
      {
        pixels[x] = new Dictionary<int, LabColor>();
        for (int y = 0; y < height; y++)
        {
          pixels[x][y] = ColorUtils.RgbToLab(bmp.GetPixel(x, y));
        }
      }
    }

    public LabColor GetPixel(int x, int y)
    {
      LabColor result = new LabColor(0, 0, 0);

      if (x >= 0 && x < width && y >= 0 && y < height)
      {
        result = pixels[x][y];
      }

      return result;
    }

    public int Height
    {
      get { return height; }
    }

    public void SetPixel(int x, int y, LabColor labColor)
    {
      if (x >= 0 && x < width && y >= 0 && y < height)
      {
        pixels[x][y] = labColor;
      }
    }

    public int Width
    {
      get { return width; }
    }

  }
}
