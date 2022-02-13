// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorSamplingForm.cs


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
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class ColorSamplingForm : Form
  {
    bool colorWasSampled = false;
    Color sampledColor = Color.CornflowerBlue;
    Bitmap sourceImage = null;
    float zoomFactor = 1.0F;

    public ColorSamplingForm(string imageFileName)
    {
      InitializeComponent();

      ReadImageFromFile(imageFileName);
    }

    private bool AskIfColorIsOkay()
    {
      ColorSampleVerificationForm dlg =
        new ColorSampleVerificationForm(sampledColor);
      return dlg.ShowDialog() == DialogResult.Yes;
    }

    public bool ColorWasSampled
    {
      get { return colorWasSampled; }
    }

    private void ImagePictureBox_MouseClick(object sender, MouseEventArgs e)
    {
      if (imagePictureBox.BackgroundImage != null &&
          imagePictureBox.BackgroundImage is Bitmap)
      {
        Point clickedPoint = e.Location;
        if (e.Button == MouseButtons.Left)
        {
          // Sample the colors in a 5 x 5 square centered at the mouse position.
          Tuple<int, int, int> rgb;
          Tuple<int, int, int> hsl;
          // Hue is a cycle. A bunch of hues clustered around red should not be
          // averaged to cyan just because some of them were near zero and the
          // others were near 360.
          float sumH = 0;
          float sumHAlt = 0;
          int minH = 360;
          int maxH = 0;
          float sumS = 0;
          float sumL = 0;
          float sumI = 0;
          for (int x = Math.Max(0, clickedPoint.X - 2);
            x <= clickedPoint.X + 2 &&
            x < imagePictureBox.BackgroundImage.Width; x++)
          {
            for (int y = Math.Max(0, clickedPoint.Y - 2);
              y <= clickedPoint.Y + 2 &&
              y < imagePictureBox.BackgroundImage.Height; y++)
            {
              Color color = (imagePictureBox.BackgroundImage as Bitmap).
                GetPixel(x, y);
              rgb = new Tuple<int, int, int>(color.R, color.G, color.B);
              hsl = ColorConverter.RgbToHsl(rgb);
              minH = Math.Min(minH, hsl.Item1);
              maxH = Math.Max(maxH, hsl.Item1);
              sumH += hsl.Item1;
              sumHAlt += (hsl.Item1 + 180) % 360;
              sumS += hsl.Item2;
              sumL += hsl.Item3;
              sumI += 1;
            }
          }

          if (sumI > 0)
          {
            // Compute the average color of the samples.
            if (maxH - minH >= 180)
            {
              // Values likely span the 0/360 transition. Use the alternate sum.
              sumH = sumHAlt - 180 * sumI;
              if (sumH < 0)
              {
                sumH += 360 * sumI;
              }
            }
            hsl = new Tuple<int, int, int>((int)Math.Round(sumH / sumI),
              (int)Math.Round(sumS / sumI), (int)Math.Round(sumL / sumI));
            rgb = ColorConverter.HslToRgb(hsl);
            sampledColor = Color.FromArgb(rgb.Item1, rgb.Item2, rgb.Item3);
            colorWasSampled = true;
          }
        }
        else if (e.Button == MouseButtons.Right)
        {
          // Take a single sample at the mouse position.
          if (clickedPoint.X >= 0 && clickedPoint.Y >= 0 &&
              clickedPoint.X < imagePictureBox.BackgroundImage.Width &&
              clickedPoint.Y < imagePictureBox.BackgroundImage.Height)
          {
            sampledColor = (imagePictureBox.BackgroundImage as Bitmap).
              GetPixel(clickedPoint.X, clickedPoint.Y);
            colorWasSampled = true;
          }
        }
        if (colorWasSampled)
        {
          if (AskIfColorIsOkay())
          {
            Close();
          }
        }
      }
    }

    private void ImagePictureBox_MouseWheel(object sender,
      MouseEventArgs e)
    {
      Keys keys = Control.ModifierKeys;
      if (keys == Keys.None)
      {
        zoomFactor *= 1.0F + 0.0005F * e.Delta;
        ShowImageAtZoom();
        // Don't let the event bubble up to the panel else it will use it for
        // scrolling.
        ((HandledMouseEventArgs)e).Handled = true;
      }
    }

    private void ReadImageFromFile(string imageFilename)
    {
      // Note: bitmap images can be created from files of the BMP, GIF, EXIF,
      // JPG, PNG, and TIFF formats.
      Bitmap rawBitmapImage = new Bitmap(imageFilename);

      // When we create a bitmap from the specified file, the file may or may
      // not have had the right pixel format.
      if (rawBitmapImage.PixelFormat == PixelFormat.Format32bppRgb)
      {
        // The image from file is the right format, so use it.
        sourceImage = rawBitmapImage;
      }
      else
      {
        // The image from file is not in the format that we need.
        // Create the source image bitmap to be the same size as the image from
        // file, but in 32bppRGB format, then draw the image from file into
        // the source image bitmap.
        sourceImage = new Bitmap(rawBitmapImage.Width, rawBitmapImage.Height,
          PixelFormat.Format32bppRgb);
        Graphics g = Graphics.FromImage(sourceImage);
        g.DrawImage(rawBitmapImage, new Rectangle(
          0, 0, rawBitmapImage.Width, rawBitmapImage.Height));
        g.Dispose();
      }

      // Place the image into the image picture box.
      ShowImageAtZoom();
    }

    private void ShowImageAtZoom()
    {
      Image oldZoomedImage = imagePictureBox.BackgroundImage;
      Size zoomedImageSize = new Size(
        (int)Math.Round(zoomFactor * sourceImage.Width),
        (int)Math.Round(zoomFactor * sourceImage.Height));
      try
      {
        Bitmap newZoomedImage = new Bitmap(sourceImage, zoomedImageSize);
        imagePictureBox.Size = newZoomedImage.Size;
        imagePictureBox.BackgroundImage = newZoomedImage;
        if (oldZoomedImage != null)
        {
          oldZoomedImage.Dispose();
        }
      }
      catch
      {
        // Probably zoomed too close and ran out of memory trying to create
        // the bitmap.
      }
    }

    public Color SampledColor
    {
      get { return sampledColor; }
    }

  }
}
