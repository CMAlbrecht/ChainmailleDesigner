// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorImage.cs


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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ChainmailleDesigner
{
  /// <summary>
  /// Class for representing the ring colors of a chainmaille design, one pixel
  /// per ring.
  /// </summary>
  public class ColorImage : IDisposable
  {
    Bitmap bitmapImage = null;

    // Note: bitmap images can be created from files of the BMP, GIF, EXIF,
    // JPG, PNG, and TIFF formats, but not all of these are appropriate for use
    // as a color image.
    public static List<string> ColorImageFileExtensions =
      new List<string>() { ".png", ".bmp", ".tif" };

    /// <summary>
    /// Constructor for new color image.
    /// </summary>
    /// <param name="size"></param>
    public ColorImage(Size size)
    {
      bitmapImage = new Bitmap(size.Width, size.Height,
        PixelFormat.Format32bppArgb);
      // Paint the entire color image the color of an unspecified element.
      Graphics g = Graphics.FromImage(bitmapImage);
      g.Clear(Properties.Settings.Default.UnspecifiedElementColor);
      g.Dispose();
    }

    /// <summary>
    /// Constructor for new color image.
    /// </summary>
    /// <param name="size"></param>
    public ColorImage(Bitmap image)
    {
      if (image.PixelFormat == PixelFormat.Format32bppArgb)
      {
        // Supplied image is already in the correct format.
        bitmapImage = image;
      }
      else
      {
        // Re-draw the supplied image into a new image of the correct format.
        bitmapImage = new Bitmap(image.Width, image.Height,
          PixelFormat.Format32bppArgb);
        Graphics g = Graphics.FromImage(bitmapImage);
        g.Clear(Properties.Settings.Default.UnspecifiedElementColor);
        g.DrawImageUnscaled(image, 0, 0);
        g.Dispose();
      }
    }

    /// <summary>
    /// Constructor from specified image file.
    /// </summary>
    /// <param name="imageFilepath"></param>
    /// <param name="imageName"></param>
    public ColorImage(string imageFilepath)
    {
      FileStream fileStream = new FileStream(imageFilepath, FileMode.Open);
      Bitmap rawBitmapImage = new Bitmap(fileStream);
      fileStream.Close();

      // When we create a bitmap from the specified file, the file may or may
      // not have had the right pixel format. We want a pixel format that
      // permits transparency as well as a decent color range.
      if (rawBitmapImage.PixelFormat == PixelFormat.Format32bppArgb)
      {
        // The image from file is the right format, so use it.
        bitmapImage = rawBitmapImage;
      }
      else
      {
        // The image from file is not in the format that we need.
        // Create the color image bitmap to be the same size as the image from
        // file, but in 32bppARGB format, then draw the image from file into
        // the color image bitmap.
        bitmapImage = new Bitmap(rawBitmapImage.Width, rawBitmapImage.Height,
          PixelFormat.Format32bppArgb);
        Graphics g = Graphics.FromImage(bitmapImage);
        g.DrawImage(rawBitmapImage, new Rectangle(
          0, 0, rawBitmapImage.Width, rawBitmapImage.Height));
        g.Dispose();
      }
    }

    public Bitmap BitmapImage
    {
      get { return bitmapImage; }
      set
      {
        if (bitmapImage != null && bitmapImage != value)
        {
          // Dispose of the old bitmap.
          bitmapImage.Dispose();
        }
        bitmapImage = value;
      }
    }

    public Color ColorAt(int x, int y)
    {
      Color color = bitmapImage != null ?
        Properties.Settings.Default.BackgroundColor :
        Properties.Settings.Default.UnspecifiedElementColor;

      if (bitmapImage != null &&
          x >= 0 && x < bitmapImage.Width &&
          y >= 0 && y < bitmapImage.Height)
      {
        color = bitmapImage.GetPixel(x, y);
      }

      return color;
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
        bitmapImage?.Dispose();
      }
    }

    public int Height
    {
      get { return bitmapImage.Height; }
    }

    public PixelFormat PixelFormat
    {
      get { return bitmapImage.PixelFormat; }
    }

    public Size Size
    {
      get { return bitmapImage.Size; }
    }

    public int Width
    {
      get { return bitmapImage.Width; }
    }

  }
}
