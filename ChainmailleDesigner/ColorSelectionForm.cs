// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorSelectionForm.cs


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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class ColorSelectionForm : Form
  {
    // The selected color.
    public static Color defaultColor = Color.CornflowerBlue;
    private Color selectedColor = defaultColor;
    private Tuple<int, int, int> selectedColorHsl = ColorConverter.RgbToHsl(
      new Tuple<int, int, int>(
        defaultColor.R, defaultColor.G, defaultColor.B));
    private Tuple<int, int, int> selectedColorRgb = new Tuple<int, int, int>(
      defaultColor.R, defaultColor.G, defaultColor.B);

    // Flag to bypass value-changed events when setting control values
    // programatically.
    private bool ignoreControlChanges = false;

    // Drawing in the HSL map.
    private Graphics hslMapGraphics;
    private Pen hslMapPen = new Pen(Color.Black, 2);
    private SolidBrush hslMapBrush = new SolidBrush(SystemColors.Control);
    private float xScaleFactor;
    private float yScaleFactor;
    private PointF centerPoint;
    const float triangleRadius = 128F;
    const float triangleMidline = 1.5F * 128;
    const float triangleSide = 1.5F * 128 / 0.866F;
    const float innerRingRadius = 130F;
    const float outerRingRadius = 160F;
    const float innerIndicatorRadius = 162F;
    const float outerIndicatorRadius = 180F;
    const float innerCursorRadius = 10F;
    const float outerCursorRadius = 20F;

    private Rectangle hslMapRectangle;
    private Rectangle cursorRectangle;
    private Rectangle triangleRectangle;
    private Rectangle innerCircleRectangle;
    private Rectangle outerCircleRectangle;
    private PointF huePoint = new PointF();
    private PointF blackPoint = new PointF();
    private PointF whitePoint = new PointF();
    private PointF indicatorStart = new PointF();
    private PointF indicatorEnd = new PointF();

    private enum MouseModeEnum { NoAction, ChangeSL, ChangeH };
    private MouseModeEnum mouseMode = MouseModeEnum.NoAction;

    public ColorSelectionForm()
      : this(Color.CornflowerBlue)
    {
    }

    public ColorSelectionForm(Color initialColor)
    {
      InitializeComponent();
      InitializeBackgrounds();
      SetAllControlsForSelectedHsl();
      selectedColorRgb = new Tuple<int, int, int>(
        initialColor.R, initialColor.G, initialColor.B);
      SetAllControlsForSelectedRgb();
    }

    private void DrawHslMapBackground()
    {
      // Clear the background image.
      hslMapGraphics.FillRectangle(hslMapBrush, hslMapRectangle);

      // Draw the hue ring.
      int s = 240;
      int l = 120;
      Tuple<int, int, int> rgb;
      PointF innerPoint = new PointF();
      PointF outerPoint = new PointF();
      hslMapPen.Color = Color.Black;
      hslMapPen.Width = 2;
      for (int h = 0; h < 360; h++)
      {
        rgb = ColorConverter.HslToRgb(new Tuple<int, int, int>(h, s, l));
        hslMapPen.Color = Color.FromArgb(rgb.Item1, rgb.Item2, rgb.Item3);
        double hRadians = Math.PI * h / 180;
        double cosH = Math.Cos(hRadians);
        double sinH = Math.Sin(hRadians);
        innerPoint.X = (float)
          (centerPoint.X + xScaleFactor * innerRingRadius * cosH);
        innerPoint.Y = (float)
          (centerPoint.Y - yScaleFactor * innerRingRadius * sinH);
        outerPoint.X = (float)
          (centerPoint.X + xScaleFactor * outerRingRadius * cosH);
        outerPoint.Y = (float)
          (centerPoint.Y - yScaleFactor * outerRingRadius * sinH);
        hslMapGraphics.DrawLine(hslMapPen, innerPoint, outerPoint);
      }

      // Draw the inner and outer boundaries of the hue ring.
      hslMapPen.Color = Color.Black;
      outerCircleRectangle = new Rectangle(
        (int)(30 * xScaleFactor), (int)(30 * yScaleFactor),
        (int)(320 * xScaleFactor), (int)(320 * yScaleFactor));
      innerCircleRectangle = new Rectangle(
        (int)(60 * xScaleFactor), (int)(60 * yScaleFactor),
        (int)(260 * xScaleFactor), (int)(260 * yScaleFactor));
      triangleRectangle = new Rectangle(
        (int)(62 * xScaleFactor), (int)(62 * yScaleFactor),
        (int)(256 * xScaleFactor), (int)(256 * yScaleFactor));
      hslMapGraphics.DrawEllipse(hslMapPen, outerCircleRectangle);
      hslMapGraphics.DrawEllipse(hslMapPen, innerCircleRectangle);
    }

    /// <summary>
    /// Draws the gradient background for the selected color patch and the
    /// hue ring of the HSL map.
    /// </summary>
    private void InitializeBackgrounds()
    {
      // Initialize the background of the gradient box.
      gradientPictureBox.BackgroundImage = new Bitmap(
        gradientPictureBox.ClientSize.Width,
        gradientPictureBox.ClientSize.Height);
      Brush gradientBrush = new LinearGradientBrush(new Point(0, 0),
        new Point(gradientPictureBox.BackgroundImage.Width, 0),
        Color.Black, Color.White);
      Graphics g = Graphics.FromImage(gradientPictureBox.BackgroundImage);
      g.FillRectangle(gradientBrush, 0, 0,
        gradientPictureBox.BackgroundImage.Width,
        gradientPictureBox.BackgroundImage.Height);
      g.Dispose();

      // The design size of the HSV map is 380 x 380, but it is unlikely to
      // have that size when it is put on the screen. Experience has shown that
      // the scaling in the x and y directions is often different.
      hslMapRectangle = new Rectangle(0, 0,
        hslMapPictureBox.ClientSize.Width, hslMapPictureBox.ClientSize.Height);
      hslMapPictureBox.BackgroundImage = new Bitmap(
        hslMapRectangle.Width, hslMapRectangle.Height);
      hslMapGraphics = Graphics.FromImage(hslMapPictureBox.BackgroundImage);
      xScaleFactor = hslMapRectangle.Width / 380F;
      yScaleFactor = hslMapRectangle.Height / 380F;
      centerPoint = new PointF(xScaleFactor * 190, yScaleFactor * 190);
    }

    public Color SelectedColor
    {
      get { return selectedColor; }
    }

    /// <summary>
    /// Set all the controls for the selected color.
    /// </summary>
    private void SetAllControlsForSelectedColor()
    {
      selectedColor = Color.FromArgb(selectedColorRgb.Item1,
        selectedColorRgb.Item2, selectedColorRgb.Item3);
      selectedColorPictureBox.BackColor = selectedColor;
      SetHslControlsForSelectedColor();
      SetRgbControlsForSelectedColor();
      SetHslMapForSelectedColor();
    }

    /// <summary>
    /// Convert the selected HSL to RGB, then set the controls for the
    /// selected color.
    /// </summary>
    private void SetAllControlsForSelectedHsl()
    {
      selectedColorRgb = ColorConverter.HslToRgb(selectedColorHsl);
      SetAllControlsForSelectedColor();
    }

    /// <summary>
    /// Convert the selected RGB to HSL, then set the controls for the
    /// selected color.
    /// </summary>
    private void SetAllControlsForSelectedRgb()
    {
      selectedColorHsl = ColorConverter.RgbToHsl(selectedColorRgb);
      SetAllControlsForSelectedColor();
    }

    private void SetHslMapForSelectedColor()
    {
      DrawHslMapBackground();

      // Angular distance between points of the triangle,
      double triangleStep = 2.0 * Math.PI / 3.0;
      // The hue angle.
      double radians = Math.PI * selectedColorHsl.Item1 / 180.0;

      // The white point of the triangle.
      radians += triangleStep;
      double cos = Math.Cos(radians);
      double sin = Math.Sin(radians);
      whitePoint.X = (float)
        (centerPoint.X + xScaleFactor * cos * triangleRadius);
      whitePoint.Y = (float)
        (centerPoint.Y - yScaleFactor * sin * triangleRadius);

      // The black point of the triangle.
      radians += triangleStep;
      cos = Math.Cos(radians);
      sin = Math.Sin(radians);
      blackPoint.X = (float)
        (centerPoint.X + xScaleFactor * cos * triangleRadius);
      blackPoint.Y = (float)
        (centerPoint.Y - yScaleFactor * sin * triangleRadius);

      // The fully-saturated point of the triangle.
      radians += triangleStep;
      cos = Math.Cos(radians);
      sin = Math.Sin(radians);
      huePoint.X = (float)
        (centerPoint.X + xScaleFactor * cos * triangleRadius);
      huePoint.Y = (float)
        (centerPoint.Y - yScaleFactor * sin * triangleRadius);

      // Start and end for the hue indicator on the outside of the hue ring.
      indicatorStart = new PointF(
        (float)(centerPoint.X + xScaleFactor * cos * innerIndicatorRadius),
        (float)(centerPoint.Y - yScaleFactor * sin * innerIndicatorRadius));
      indicatorEnd = new PointF(
        (float)(centerPoint.X + xScaleFactor * cos * outerIndicatorRadius),
        (float)(centerPoint.Y - yScaleFactor * sin * outerIndicatorRadius));

      // Fill in the triangle from the hue point to the opposite side.
      // dx is along the saturation axis, with 0 fully saturated, i.e.
      // at the hue point. dy is along the luminance axis, i.e. in the
      // direction given by the white point and the black point.

      int dxLimit, dyLimit = (int)(triangleSide / 2);
      int s, l, x, y;
      float cx, cy, hx, hy;
      Tuple<int, int, int> rgb;

      for (int dy = dyLimit; dy >= -dyLimit; dy--)
      {
        l = 120 + 120 * dy / dyLimit;
        dxLimit = (int)(triangleMidline * (120 - Math.Abs(120 - l)) / 120);
        for (int dx = 0; dx <= dxLimit; dx++)
        {
          s = dx > 0 ? 240 * dx / dxLimit : 0;
          rgb = ColorConverter.HslToRgb(
            new Tuple<int, int, int>(selectedColorHsl.Item1, s, l));
          Color color = Color.FromArgb(255, rgb.Item1, rgb.Item2, rgb.Item3);
          // Rotate per hue.
          cx = dx - triangleMidline + triangleRadius;
          hx = (float)(cos * cx - sin * dy);
          hy = (float)(cos * dy + sin * cx);
          // Translate from center point.
          x = (int)(centerPoint.X + xScaleFactor * hx);
          y = (int)(centerPoint.Y - yScaleFactor * hy);
          // Set the pixel color.
          ((Bitmap)hslMapPictureBox.BackgroundImage).SetPixel(
            x, y, color);
        }
      }

      // Draw the boundary of the triangle.
      hslMapPen.Color = Color.Black;
      hslMapPen.Width = 2;
      PointF[] trianglePoints =
        new PointF[] { huePoint, whitePoint, blackPoint };
      hslMapGraphics.DrawPolygon(hslMapPen, trianglePoints);

      // Draw the S, L cursor.
      cy = triangleSide * (selectedColorHsl.Item3 - 120) / 240F;
      cx = triangleRadius - triangleMidline + (triangleMidline *
        (120 - Math.Abs(120 - selectedColorHsl.Item3)) / 120) *
        selectedColorHsl.Item2 / 240F;
      hx = (float)(cos * cx - sin * cy);
      hy = (float)(cos * cy + sin * cx);
      // Translate from center point.
      x = (int)(centerPoint.X + xScaleFactor * hx);
      y = (int)(centerPoint.Y - yScaleFactor * hy);
      // Draw the central circle of the cursor.
      cursorRectangle = new Rectangle(
        (int)(x - xScaleFactor * innerCursorRadius),
        (int)(y - yScaleFactor * innerCursorRadius),
        (int)(2 * xScaleFactor * innerCursorRadius),
        (int)(2 * yScaleFactor * innerCursorRadius));
      hslMapPen.Color = Color.White;
      hslMapPen.Width = 1;
      hslMapGraphics.DrawEllipse(hslMapPen, cursorRectangle);
      // Draw the four radiating lines of the cursor.
      float cursorLineLength = outerCursorRadius - innerCursorRadius;
      PointF cursorLineStart = new PointF(
        cursorRectangle.X + cursorRectangle.Width / 2, cursorRectangle.Y);
      PointF cursorLineEnd = new PointF(cursorLineStart.X,
        cursorLineStart.Y - yScaleFactor * cursorLineLength);
      hslMapGraphics.DrawLine(hslMapPen, cursorLineStart, cursorLineEnd);
      cursorLineStart = new PointF(
        cursorRectangle.X + cursorRectangle.Width / 2,
        cursorRectangle.Y + cursorRectangle.Height);
      cursorLineEnd = new PointF(cursorLineStart.X,
        cursorLineStart.Y + yScaleFactor * cursorLineLength);
      hslMapGraphics.DrawLine(hslMapPen, cursorLineStart, cursorLineEnd);
      cursorLineStart = new PointF(
        cursorRectangle.X, cursorRectangle.Y + cursorRectangle.Height / 2);
      cursorLineEnd = new PointF(
        cursorLineStart.X - xScaleFactor * cursorLineLength,
        cursorLineStart.Y);
      hslMapGraphics.DrawLine(hslMapPen, cursorLineStart, cursorLineEnd);
      cursorLineStart = new PointF(
        cursorRectangle.X + cursorRectangle.Width,
        cursorRectangle.Y + cursorRectangle.Height / 2);
      cursorLineEnd = new PointF(
        cursorLineStart.X + xScaleFactor * cursorLineLength,
        cursorLineStart.Y);
      hslMapGraphics.DrawLine(hslMapPen, cursorLineStart, cursorLineEnd);

      // Draw the indicator outside the hue circle.
      hslMapPen.Color = Color.Black;
      hslMapPen.Width = 3;
      hslMapGraphics.DrawLine(hslMapPen, indicatorStart, indicatorEnd);

      hslMapPictureBox.Invalidate();
    }

    private void SetHslControlsForSelectedColor()
    {
      SetHslTextBoxesForSelectedColor();
      SetHslSlidersForSelectedColor();
    }

    private void SetHslSlidersForSelectedColor()
    {
      ignoreControlChanges = true;

      hTrackBar.Value = selectedColorHsl.Item1;
      sTrackBar.Value = selectedColorHsl.Item2;
      lTrackBar.Value = selectedColorHsl.Item3;

      ignoreControlChanges = false;
    }

    private void SetHslTextBoxesForSelectedColor()
    {
      ignoreControlChanges = true;

      hTextBox.Text = selectedColorHsl.Item1.ToString();
      sTextBox.Text = selectedColorHsl.Item2.ToString();
      lTextBox.Text = selectedColorHsl.Item3.ToString();

      ignoreControlChanges = false;
    }

    private void SetRgbControlsForSelectedColor()
    {
      SetRgbTextBoxesForSelectedColor();
      SetRgbSlidersForSelectedColor();
    }

    private void SetRgbSlidersForSelectedColor()
    {
      ignoreControlChanges = true;

      rTrackBar.Value = selectedColorRgb.Item1;
      gTrackBar.Value = selectedColorRgb.Item2;
      bTrackBar.Value = selectedColorRgb.Item3;

      ignoreControlChanges = false;
    }

    private void SetRgbTextBoxesForSelectedColor()
    {
      ignoreControlChanges = true;

      rTextBox.Text = selectedColorRgb.Item1.ToString();
      gTextBox.Text = selectedColorRgb.Item2.ToString();
      bTextBox.Text = selectedColorRgb.Item3.ToString();

      ignoreControlChanges = false;
    }

    private bool XyToH(double x, double y, out int h)
    {
      bool result = false;
      h = 0;

      double distance = Math.Sqrt(x * x + y * y);
      if (distance > 0.0)
      {
        double radians = Math.Atan2(y, x);
        h = (int)(Math.Round(180 * radians / Math.PI));
        h += h < 0 ? 360 : 0;
        result = distance >= innerRingRadius;
      }

      return result;
    }

    private bool XyToSl(double x, double y, out int s, out int l)
    {
      // Counter-rotate to get to hx, hy normalized to zero hue.
      double radians = -Math.PI * selectedColorHsl.Item1 / 180.0;
      double cos = Math.Cos(radians);
      double sin = Math.Sin(radians);
      double hx = cos * x - sin * y;
      double hy = cos * y + sin * x;
      // Compute the L and S components.
      double cx = hx + triangleMidline - triangleRadius;
      l = (int)(Math.Round(120 + 240 * hy / triangleSide));
      double cxLimit = (int)(triangleMidline * (120 - Math.Abs(120 - l)) / 120);
      s = (int)(Math.Round(240 * cx / cxLimit));

      return l >= 0 && l <= 240 && s >= 0 && s <= 240;
    }

    private void hslMapPictureBox_MouseDown(object sender, MouseEventArgs e)
    {
      // Determine distance from center.
      double x = (e.Location.X - centerPoint.X) / xScaleFactor;
      double y = (centerPoint.Y - e.Location.Y) / yScaleFactor;
      double distance = Math.Sqrt(x * x + y * y);

      // Set mouse mode.
      mouseMode = distance >= innerRingRadius ?
        MouseModeEnum.ChangeH : MouseModeEnum.ChangeSL;

      switch (mouseMode)
      {
        case MouseModeEnum.ChangeH:
          {
            int h;
            if (XyToH(x, y, out h))
            {
              selectedColorHsl = new Tuple<int, int, int>(
                h, selectedColorHsl.Item2, selectedColorHsl.Item3);
              SetAllControlsForSelectedHsl();
            }
          }
          break;
        case MouseModeEnum.ChangeSL:
          {
            int s, l;
            if (XyToSl(x, y, out s, out l))
            {
              selectedColorHsl = new Tuple<int, int, int>(
                selectedColorHsl.Item1, s, l);
              SetAllControlsForSelectedHsl();
            }
            else
            {
              // Mouse was not in the triangle.
              mouseMode = MouseModeEnum.NoAction;
            }
          }
          break;
      }
    }

    private void hslMapPictureBox_MouseLeave(object sender, EventArgs e)
    {
      mouseMode = MouseModeEnum.NoAction;
    }

    private void hslMapPictureBox_MouseMove(object sender, MouseEventArgs e)
    {
      double x = (e.Location.X - centerPoint.X) / xScaleFactor;
      double y = (centerPoint.Y - e.Location.Y) / yScaleFactor;

      switch (mouseMode)
      {
        case MouseModeEnum.ChangeH:
          {
            int h;
            if (XyToH(x, y, out h))
            {
              selectedColorHsl = new Tuple<int, int, int>(
                h, selectedColorHsl.Item2, selectedColorHsl.Item3);
              SetAllControlsForSelectedHsl();
            }
          }
          break;
        case MouseModeEnum.ChangeSL:
          int s, l;
          if (XyToSl(x, y, out s, out l))
          {
            selectedColorHsl = new Tuple<int, int, int>(
              selectedColorHsl.Item1, s, l);
            SetAllControlsForSelectedHsl();
          }
          else
          {
            // Mouse was not in the triangle.
            mouseMode = MouseModeEnum.NoAction;
          }
          break;
      }
    }

    private void hslMapPictureBox_MouseUp(object sender, MouseEventArgs e)
    {
      mouseMode = MouseModeEnum.NoAction;
    }

    private void hslSliderChanged(object sender, EventArgs e)
    {
      if (!ignoreControlChanges)
      {
        try
        {
          selectedColorHsl = new Tuple<int, int, int>(
            hTrackBar.Value, sTrackBar.Value, lTrackBar.Value);
          SetAllControlsForSelectedHsl();
        }
        catch { }
      }
    }

    private void hslTextChanged(object sender, EventArgs e)
    {
      if (!ignoreControlChanges)
      {
        try
        {
          selectedColorHsl = new Tuple<int, int, int>(int.Parse(hTextBox.Text),
            int.Parse(sTextBox.Text), int.Parse(lTextBox.Text));
          SetAllControlsForSelectedHsl();
        }
        catch { }
      }
    }

    private void rgbSliderChanged(object sender, EventArgs e)
    {
      if (!ignoreControlChanges)
      {
        try
        {
          selectedColorRgb = new Tuple<int, int, int>(
            rTrackBar.Value, gTrackBar.Value, bTrackBar.Value);
          SetAllControlsForSelectedRgb();
        }
        catch { }
      }
    }

    private void rgbTextChanged(object sender, EventArgs e)
    {
      if (!ignoreControlChanges)
      {
        try
        {
          selectedColorRgb = new Tuple<int, int, int>(int.Parse(rTextBox.Text),
            int.Parse(gTextBox.Text), int.Parse(bTextBox.Text));
          SetAllControlsForSelectedRgb();
        }
        catch { }
      }
    }

    private void colorSampleButton_Click(object sender, EventArgs e)
    {
      OpenFileDialog dlg = new OpenFileDialog();
      dlg.InitialDirectory = Properties.Settings.Default.DesignDirectory;
      dlg.Filter = Properties.Settings.Default.ImageFileFilter;
      dlg.FilterIndex = 1;
      dlg.CheckFileExists = true;
      dlg.CheckPathExists = true;
      dlg.Multiselect = false;
      dlg.Title = "Open Image to Sample";
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        // Set the overlay file for the design.
        ColorSamplingForm dlg2 = new ColorSamplingForm(dlg.FileName);
        dlg2.ShowDialog();
        if (dlg2.ColorWasSampled)
        {
          Color sampledColor = dlg2.SampledColor;
          selectedColorRgb = new Tuple<int, int, int>(
            sampledColor.R, sampledColor.G, sampledColor.B);
          SetAllControlsForSelectedRgb();
        }
      }
    }

  }
}
