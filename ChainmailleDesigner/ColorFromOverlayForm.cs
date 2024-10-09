// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorFromOverlayForm.cs


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

// Transformation specification for translating colors within the same color
// space. These are scale, offset pairs for each of three components.
using ColorTransform = System.Tuple<System.Tuple<double, double>,
  System.Tuple<double, double>, System.Tuple<double, double>>;

namespace ChainmailleDesigner
{
  public partial class ColorFromOverlayForm : Form, IShapeProgressIndicator
  {
    string ringFilterString = string.Empty;
    ChainmailleDesign design = null;
    Palette palette = null;
    PaletteSection paletteSection = null;
    ColorTransform colorTransform;
    bool initializationComplete = false;

    // Dynamic UI element generation.
    private float xScaleFactor = 1;
    private float yScaleFactor = 1;
    private const string ringFilterNameBase = "ringFilterRadioButton";

    public ColorFromOverlayForm(ChainmailleDesign chainmailleDesign,
      Palette colorPalette)
    {
      InitializeComponent();

      design = chainmailleDesign;
      palette = colorPalette;

      InitializeControlSizes();

      InitializeFromDesign();
      InitializeFromPalette();
      SetColorTransform();
      initializationComplete = true;
    }

    private void AddRingFilterButton(int filterIndex, string filterName,
      bool isDefault = false)
    {
      RadioButton filterRadioButton = new RadioButton();
      filterRadioButton.Location = new Point(
      (int)((20) * xScaleFactor),
      (int)((50 + 45 * filterIndex) * yScaleFactor));
      filterRadioButton.Name = ringFilterNameBase + filterIndex;
      filterRadioButton.TabIndex = filterIndex;
      filterRadioButton.AutoSize = true;
      filterRadioButton.Text = filterName;
      filterRadioButton.Checked = isDefault;
      filterRadioButton.CheckedChanged +=
        new System.EventHandler(RingFilterButton_CheckedChanged);
      ringFilterGroupBox.Controls.Add(filterRadioButton);
    }

    private void ColorFromOverlayForm_Shown(object sender, EventArgs e)
    {
      DrawPreviewImage();
    }

    public ColorTransform ColorTransform
    {
      get { return colorTransform; }
    }

    private void DrawPreviewImage()
    {
      if (design != null && design.RenderedImage != null &&
        initializationComplete)
      {
        ringFilterString = string.Empty;
        if (ringFilterGroupBox.Visible)
        {
          // Determine which ring filter to apply.
          foreach (Control control in ringFilterGroupBox.Controls)
          {
            if (control is RadioButton && (control as RadioButton).Checked)
            {
              ringFilterString = (control as RadioButton).Text;
              if (ringFilterString == "All Rings")
              {
                // "All Rings" means no filtering.
                ringFilterString = string.Empty;
              }
              break;
            }
          }
        }

        if (previewPanel.BackgroundImage == null)
        {
          previewPanel.BackgroundImage = new Bitmap(
            previewPanel.ClientRectangle.Width,
            previewPanel.ClientRectangle.Height);
        }

        // Create a local copy of the color image of the design.
        ColorImage previewColorImage =
          new ColorImage(new Bitmap(design.ColorImage.BitmapImage));

        // Set the preview color image based on the overlay colors.
        design.ColorDesignFromOverlay(paletteSection, ringFilterString, this,
          colorTransform, previewColorImage);
        Bitmap sourceImage;
        // Because this is a preview, we don't want to change the render image
        // of the design yet, so we will use an alternative render image, then
        // transform it for dispaly in the preview panel.
        Bitmap alternativeRenderedImage =
          design.RenderImage(true, null, ringFilterString, false,
          previewColorImage);

        sourceImage =
          design.TransformRenderedImageForDisplay(alternativeRenderedImage);
        previewColorImage.Dispose();
        alternativeRenderedImage.Dispose();

        Graphics g = Graphics.FromImage(previewPanel.BackgroundImage);
        g.Clear(SystemColors.Control);
        float zoomFactor = Math.Min(
          previewPanel.BackgroundImage.Width /
          (float)sourceImage.Width,
          previewPanel.BackgroundImage.Height /
          (float)sourceImage.Height);
        Size destinationSize = new Size(
          (int)(sourceImage.Width * zoomFactor),
          (int)(sourceImage.Height * zoomFactor));
        Rectangle destinationRectangle = new Rectangle(
          (int)((previewPanel.BackgroundImage.Width -
          destinationSize.Width) * 0.5F),
          (int)((previewPanel.BackgroundImage.Height -
          destinationSize.Height) * 0.5F),
          destinationSize.Width, destinationSize.Height);
        ImageAttributes imageAttributes = new ImageAttributes();
        g.DrawImage(sourceImage, destinationRectangle,
          0, 0, sourceImage.Width, sourceImage.Height,
          GraphicsUnit.Pixel, imageAttributes);
        g.Dispose();
        sourceImage.Dispose();

        // Invalidate the panel so that it is redrawn.
        previewPanel.Invalidate();
      }
    }

    private void InitializeControlSizes()
    {
      // Determine scale factors based on design size.
      xScaleFactor = ClientSize.Width / 746F;
      yScaleFactor = ClientSize.Height / 398F;
    }

    private void InitializeFromDesign()
    {
      if (design != null && design.ChainmailPattern != null)
      {
        if (design.ChainmailPattern.HasRingSizeNames &&
            design.ChainmailPattern.HasScales)
        {
          PatternScale firstScale = design.ChainmailPattern.Scales[0];

          int sizeIndex = 0;
          AddRingFilterButton(sizeIndex++, "All Rings", true);
          foreach (RingSize ringSize in firstScale.RingSizes)
          {
            if (!string.IsNullOrEmpty(ringSize.Name))
            {
              AddRingFilterButton(sizeIndex++, ringSize.Name);
            }
          }

          ringFilterGroupBox.Visible = true;
        }
        else
        {
          ringFilterGroupBox.Visible = false;
        }
      }
    }

    private void InitializeFromPalette()
    {
      if (palette != null)
      {
        // Initialize palette section selector.
        paletteSectionComboBox.Items.Clear();
        foreach (PaletteSection paletteSection in palette.Sections)
        {
          if (!paletteSection.Hidden)
          {
            paletteSectionComboBox.Items.Add(paletteSection.Name);
          }
        }

        if (paletteSectionComboBox.Items.Count > 0)
        {
          paletteSectionComboBox.SelectedIndex = 0;
        }
      }
    }

    public PaletteSection PaletteSection
    { get { return paletteSection; } }

    private void paletteSectionComboBox_SelectedIndexChanged(object sender,
      EventArgs e)
    {
      paletteSection =
        palette.Section((string)paletteSectionComboBox.SelectedItem);
      DrawPreviewImage();
    }

    private void RingFilterButton_CheckedChanged(object sender, EventArgs e)
    {
      // For each action, one button is checked, while another is unchecked.
      // We only want to act on the checked button.
      RadioButton ringFilterButton = sender as RadioButton;
      if (ringFilterButton != null && ringFilterButton.Checked)
      {
        ringFilterString = ringFilterButton.Text;
        if (ringFilterString == "All Rings")
        {
          // "All Rings" means no filtering.
          ringFilterString = string.Empty;
        }
        DrawPreviewImage();
      }
    }

    public string RingFilterString
    {
      get { return ringFilterString; }
    }

    private void SetColorTransform()
    {
      double lScale = 1.0;
      double lOffset = 0.0;
      if (lCheckBox.Checked)
      {
        lScale = 1.0 + 0.1 * lScaleTrackBar.Value;
        lOffset = lOffsetTrackBar.Value;
      }

      double aScale = 1.0;
      double aOffset = 0.0;
      if (aCheckBox.Checked)
      {

        aScale = 1.0 + 0.1 * aScaleTrackBar.Value;
        aOffset = aOffsetTrackBar.Value;
      }

      double bScale = 1.0;
      double bOffset = 0.0;
      if (bCheckBox.Checked)
      {
        bScale = 1.0 + 0.1 * bScaleTrackBar.Value;
        bOffset = bOffsetTrackBar.Value;
      }

      colorTransform = new ColorTransform(
        new Tuple<double, double>(lScale, lOffset),
        new Tuple<double, double>(aScale, aOffset),
        new Tuple<double, double>(bScale, bOffset));
    }

    public int ShapeProgressScale
    {
      set { shapeProgressBar.Maximum = value; }
    }

    public int ShapeProgressValue
    {
      set { shapeProgressBar.Value = value; }
    }

    private void aCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void aOffsetTrackBar_MouseUp(object sender, MouseEventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void aScaleTrackBar_MouseUp(object sender, MouseEventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void bCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void bOffsetTrackBar_MouseUp(object sender, MouseEventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void bScaleTrackBar_MouseUp(object sender, MouseEventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void lCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void lOffsetTrackBar_MouseUp(object sender, MouseEventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

    private void lScaleTrackBar_MouseUp(object sender, MouseEventArgs e)
    {
      SetColorTransform();
      DrawPreviewImage();
    }

  }
}
