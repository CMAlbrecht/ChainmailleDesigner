// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmaillePatternElement.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System;
using System.Drawing;

namespace ChainmailleDesigner
{
  public class ChainmaillePatternElement : IDisposable
  {
    // Image of this one element, black element on a white background.
    private Bitmap elementImage = null;
    // Zero-based index of this element within the pattern's element list.
    private int elementIndex = 0;
    // Location of the top corner of the element image with respect to the top
    // corner of the overall pattern image.
    private Point patternOffset = new Point(0,0);
    // Location of the top corner of the element color data with respect to the
    // top corner of the overall color data.
    private Point colorOffset = new Point(0, 0);
    // Location of the element in the as-built rows and columns.
    private Point buildOffset = new Point(0, 0);
    // Name of the ring size used for the element, if specified.
    private string ringSizeName = string.Empty;

    public ChainmaillePatternElement(int index, string imageFile,
      Point elementOffset, Point elementColorOffset, Point elementBuildOffset,
      string sizeName)
    {
      elementIndex = index;
      patternOffset = elementOffset;
      colorOffset = elementColorOffset;
      buildOffset = elementBuildOffset;
      ringSizeName = sizeName;
      elementImage = new Bitmap(imageFile);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        elementImage?.Dispose();
      }
    }

    public Bitmap Image
    {
      get { return elementImage; }
    }

    public int Index
    {
      get { return elementIndex; }
    }

    public Point BuildOffset
    {
      get { return buildOffset; }
    }

    public Point ColorOffset
    {
      get { return colorOffset; }
    }

    public Point PatternOffset
    {
      get { return patternOffset; }
    }

    public string RingSizeName
    {
      get { return ringSizeName; }
    }

  }
}
