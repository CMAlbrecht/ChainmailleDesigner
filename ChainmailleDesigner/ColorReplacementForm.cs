// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorReplacementForm.cs


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
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class ColorReplacementForm : Form
  {
    Dictionary<Color, Color> colorReplacements =
      new Dictionary<Color, Color>();
    string ringFilterString = string.Empty;
    ChainmailleDesign design = null;
    Palette palette = null;

    // color name, color, color index
    Tuple<string, Color, int> currentDesignColor = null;
    Tuple<string, Color, int> currentReplacementColor = null;

    // Dynamic UI element generation.
    private float xScaleFactor = 1;
    private float yScaleFactor = 1;
    Size colorPanelSize;
    Size colorNameSize;
    private const string ringFilterNameBase = "ringFilterRadioButton";

    // Mapping panel management.
    bool someRingsAreTransparent = false;
    private const string paletteNameNameBase = "paletteNameLabel";
    private const string designColorPanelNameBase = "designColorPanel";
    private const string designColorNameNameBase = "designColorNameLabel";
    private const string replacementColorPanelNameBase =
      "replacementColorPanel";
    private const string replacementColorNameNameBase =
      "replacementColorNameLabel";

    // Palette panel management.
    private const string paletteColorPanelNameBase = "paletteColorPanel";
    private const string paletteColorNameNameBase = "paletteColorNameLabel";
    private int emptyColorSelectionHeight = 110;

    public ColorReplacementForm(ChainmailleDesign chainmailleDesign,
      Palette colorPalette)
    {
      InitializeComponent();

      design = chainmailleDesign;
      palette = colorPalette;

      InitializeControlSizes();

      InitializeFromDesign();
      InitializeFromPalette();
      InitializeImageAttributes();
      DrawPreviewImage();
    }

    private void AddMappingToPanel(int colorIndex, Color color, string colorName)
    {
      // Row and column indices.
      int r = colorIndex;
      int c = 0;

      Panel colorPanel = new Panel();
      colorPanel.BackColor = color;
      colorPanel.BorderStyle = BorderStyle.FixedSingle;
      colorPanel.Location = new Point(
      (int)((15 + 280 * c) * xScaleFactor),
      (int)((60 + 50 * r) * yScaleFactor));
      colorPanel.Name = designColorPanelNameBase + colorIndex;
      colorPanel.Size = colorPanelSize;
      colorPanel.TabIndex = 0 + 4 * colorIndex;
      colorPanel.MouseClick +=
        new MouseEventHandler(DesignColor_MouseClick);
      colorReplacementPanel.Controls.Add(colorPanel);

      Label colorNameLabel = new Label();
      colorNameLabel.Location = new Point(
      (int)((65 + 280 * c) * xScaleFactor),
      (int)((65 + 50 * r) * yScaleFactor));
      colorNameLabel.Name = designColorNameNameBase + colorIndex;
      colorNameLabel.Size = colorNameSize;
      colorNameLabel.TabIndex = 1 + 4 * colorIndex;
      colorNameLabel.Text = colorName;
      colorNameLabel.MouseClick +=
        new MouseEventHandler(DesignColor_MouseClick);
      colorReplacementPanel.Controls.Add(colorNameLabel);

      c = 1;

      Panel replacementColorPanel = new Panel();
      replacementColorPanel.BackColor = color;
      replacementColorPanel.BorderStyle = BorderStyle.FixedSingle;
      replacementColorPanel.Location = new Point(
      (int)((15 + 280 * c) * xScaleFactor),
      (int)((60 + 50 * r) * yScaleFactor));
      replacementColorPanel.Name = replacementColorPanelNameBase + colorIndex;
      replacementColorPanel.Size = colorPanelSize;
      replacementColorPanel.TabIndex = 2 + 4 * colorIndex;
      replacementColorPanel.MouseClick +=
        new MouseEventHandler(DesignColor_MouseClick);
      replacementColorPanel.Visible = false;
      colorReplacementPanel.Controls.Add(replacementColorPanel);

      Label replacementColorNameLabel = new Label();
      replacementColorNameLabel.Location = new Point(
      (int)((65 + 280 * c) * xScaleFactor),
      (int)((65 + 50 * r) * yScaleFactor));
      replacementColorNameLabel.Name = replacementColorNameNameBase + colorIndex;
      replacementColorNameLabel.Size = colorNameSize;
      replacementColorNameLabel.TabIndex = 3 + 4 * colorIndex;
      replacementColorNameLabel.Text = colorName;
      replacementColorNameLabel.MouseClick +=
        new MouseEventHandler(DesignColor_MouseClick);
      replacementColorNameLabel.Visible = false;
      colorReplacementPanel.Controls.Add(replacementColorNameLabel);

      colorReplacementPanel.Height = Math.Max(colorReplacementPanel.Height,
        colorPanel.Location.Y + colorPanel.Height + (int)(20 * yScaleFactor));
      AdjustControlPositions();
    }

    private void AddPaletteColorToPanel(int colorIndex, Color color, string colorName)
    {
      // Row and column indices.
      int r = colorIndex / 2;
      int c = colorIndex % 2;

      Panel colorPanel = new Panel();
      colorPanel.BackColor = color;
      colorPanel.BorderStyle = BorderStyle.FixedSingle;
      colorPanel.Location = new Point(
      (int)((15 + 280 * c) * xScaleFactor),
      (int)((110 + 50 * r) * yScaleFactor));
      colorPanel.Name = paletteColorPanelNameBase + colorIndex;
      colorPanel.Size = colorPanelSize;
      colorPanel.TabIndex = 4 + 2 * colorIndex;
      colorPanel.MouseClick +=
        new MouseEventHandler(PaletteColor_MouseClick);
      colorSelectionPanel.Controls.Add(colorPanel);

      Label colorNameLabel = new Label();
      colorNameLabel.Location = new Point(
      (int)((65 + 280 * c) * xScaleFactor),
      (int)((115 + 50 * r) * yScaleFactor));
      colorNameLabel.Name = paletteColorNameNameBase + colorIndex;
      colorNameLabel.Size = colorNameSize;
      colorNameLabel.TabIndex = 5 + 2 * colorIndex;
      colorNameLabel.Text = colorName;
      colorNameLabel.MouseClick +=
        new MouseEventHandler(PaletteColor_MouseClick);
      colorSelectionPanel.Controls.Add(colorNameLabel);

      colorSelectionPanel.Height = Math.Max(colorSelectionPanel.Height,
        colorPanel.Location.Y + colorPanel.Height + (int)(20 * yScaleFactor));
      AdjustControlPositions();
    }

    private void AddPaletteNameToPanel(int colorIndex, string paletteName)
    {
      Label paletteNameLabel = new Label();
      paletteNameLabel.Location = new Point(
      (int)((65) * xScaleFactor),
      (int)((65 + 50 * colorIndex) * yScaleFactor));
      paletteNameLabel.Name = paletteNameNameBase + colorIndex;
      paletteNameLabel.Size = colorNameSize;
      paletteNameLabel.TabIndex = 4 * colorIndex;
      paletteNameLabel.Text = paletteName;
      paletteNameLabel.TextAlign = ContentAlignment.MiddleCenter;
      paletteNameLabel.BorderStyle = BorderStyle.FixedSingle;
      colorReplacementPanel.Controls.Add(paletteNameLabel);
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

    private void AdjustControlPositions()
    {
      colorReplacementGroupBox.Height = (int)(40 * yScaleFactor) +
        colorReplacementPanel.Location.Y + colorReplacementPanel.Height;
      colorSelectionGroupBox.Height = (int)(40 * yScaleFactor) +
        colorSelectionPanel.Location.Y + colorSelectionPanel.Height;
      previewGroupBox.Location = new Point(previewGroupBox.Location.X,
        (int)(10 * yScaleFactor) + Math.Max(
        colorReplacementGroupBox.Location.Y + colorReplacementGroupBox.Height,
        colorSelectionGroupBox.Location.Y + colorSelectionGroupBox.Height));
      Height = previewGroupBox.Location.Y + previewGroupBox.Height +
        (int)(100 * yScaleFactor);
    }

    private void ApplyCurrentReplacementToMapping()
    {
      if (currentDesignColor != null && currentReplacementColor != null)
      {
        Panel replacementColorPanel = null;
        Control[] foundPanels = colorReplacementPanel.Controls.Find(
          replacementColorPanelNameBase + currentDesignColor.Item3, false);
        if (foundPanels.Length > 0)
        {
          replacementColorPanel = foundPanels[0] as Panel;
        }

        Label replacementColorLabel = null;
        Control[] foundLabels = colorReplacementPanel.Controls.Find(
          replacementColorNameNameBase + currentDesignColor.Item3, false);
        if (foundLabels.Length > 0)
        {
          replacementColorLabel = foundLabels[0] as Label;
        }

        replacementColorPanel.BackColor = currentReplacementColor.Item2;
        replacementColorLabel.Text = currentReplacementColor.Item1;
        replacementColorPanel.Visible = true;
        replacementColorLabel.Visible = true;

        // Update the replacement map
        colorReplacements[currentDesignColor.Item2] =
          currentReplacementColor.Item2;
        // Redraw the preview image.
        DrawPreviewImage();
      }
    }

    private void ClearTransitoryPaletteControls()
    {
      // Remove all of the transitory controls.
      List<Control> controlsToRemove = new List<Control>();
      foreach (Control control in colorSelectionPanel.Controls)
      {
        if (control.Name.StartsWith(paletteColorPanelNameBase) ||
            control.Name.StartsWith(paletteColorNameNameBase))
        {
          controlsToRemove.Add(control);
        }
      }
      foreach (Control control in controlsToRemove)
      {
        colorSelectionPanel.Controls.Remove(control);
        control.Dispose();
      }

      // Set up initial control positions.
      colorSelectionPanel.Height =
        (int)((emptyColorSelectionHeight + 10) * yScaleFactor);
      colorSelectionGroupBox.Height =
        (int)((emptyColorSelectionHeight + 50) * xScaleFactor);
    }

    public Dictionary<Color, Color> ColorReplacements
    {
      get { return colorReplacements; }
    }

    private void DrawPreviewImage()
    {
      if (design != null && design.RenderedImage != null)
      {
        ringFilterString = string.Empty;
        bool isMappingUncoloredRings = false;

        // Set color mapping.
        ImageAttributes imageAttributes = new ImageAttributes();
        if (colorReplacements.Count > 0)
        {
          ColorMap[] colorMap = new ColorMap[colorReplacements.Count];
          int colorMapIndex = 0;
          foreach (Color designColor in colorReplacements.Keys)
          {
            if (designColor == Color.FromArgb(0, 0, 0, 0))
            {
              isMappingUncoloredRings = true;
            }
            colorMap[colorMapIndex] = new ColorMap();
            colorMap[colorMapIndex].OldColor = designColor;
            colorMap[colorMapIndex].NewColor = colorReplacements[designColor];
            colorMapIndex++;
          }
          imageAttributes.SetRemapTable(colorMap);

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
        }

        if (previewPanel.BackgroundImage == null)
        {
          previewPanel.BackgroundImage = new Bitmap(
            previewPanel.ClientRectangle.Width,
            previewPanel.ClientRectangle.Height);
        }

        Bitmap sourceImage;
        if (string.IsNullOrEmpty(ringFilterString) && !isMappingUncoloredRings)
        {
          // No ring filtering, so relatively simple.
          sourceImage = design.TransformedRenderedImage;
        }
        else
        {
          // We can't just apply the color mapping to the rendered image
          // because the mapping is supposed to apply to some rings but
          // not others. This means we have to create an alternative
          // rendering, then transform it for display.
          Bitmap alternativeRenderedImage =
            design.RenderImage(true, colorReplacements, ringFilterString);

          sourceImage =
            design.TransformRenderedImageForDisplay(alternativeRenderedImage);

          // Don't use the imageAttributes; we have already mapped the colors.
          imageAttributes = new ImageAttributes();
        }

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
      xScaleFactor = ClientSize.Width / 1450F;
      yScaleFactor = ClientSize.Height / 840F;

      colorPanelSize = new Size(
        (int)(40 * xScaleFactor), (int)(40 * yScaleFactor));
      colorNameSize = new Size(
        (int)(220 * xScaleFactor), (int)(31 * yScaleFactor));
    }

    private void InitializeFromDesign()
    {
      if (design != null)
      {
        ColorCounter colorCounter = new ColorCounter(design, palette);
        someRingsAreTransparent = colorCounter.SomeRingsAreTransparent;
        // palette section name => color name => (number of rings, ring color)
        SortedDictionary<string, SortedDictionary<string, Tuple<int, Color>>>
          colorCounts = colorCounter.ColorCountByColorName;

        bool multiplePalettes = colorCounts.Keys.Count > 1;

        // Add mappings for the colors in the design.
        int colorIndex = 0;
        foreach (string paletteName in colorCounts.Keys)
        {
          if (multiplePalettes)
          {
            // Add a header line for the palette name.
            AddPaletteNameToPanel(colorIndex++, paletteName);
          }

          SortedDictionary<string, Tuple<int, Color>> sectionCounts =
            colorCounts[paletteName];
          foreach (string colorName in sectionCounts.Keys)
          {
            AddMappingToPanel(colorIndex++, sectionCounts[colorName].Item2,
              colorName);
          }
        }

        if (someRingsAreTransparent)
        {
          // Add the transparent mapping to the bottom.
          AddMappingToPanel(colorIndex++, Color.FromArgb(0, 0, 0, 0),
            "uncolored");
        }

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
          paletteSectionComboBox.Items.Add(paletteSection.Name);
        }

        if (paletteSectionComboBox.Items.Count > 0)
        {
          paletteSectionComboBox.SelectedIndex = 0;
        }
      }
    }

    private void InitializeImageAttributes()
    {
      // Do something.
    }

    private void DesignColor_MouseClick(object sender, MouseEventArgs e)
    {
      Control sendingControl = sender as Control;
      Panel colorPanel = null;
      string colorIndex = "-1";
      if (sendingControl.Name.StartsWith(designColorPanelNameBase))
      {
        colorIndex = sendingControl.Name.Substring(
          designColorPanelNameBase.Length);
        colorPanel = sender as Panel;
      }
      else if (sendingControl.Name.StartsWith(designColorNameNameBase))
      {
        colorIndex = sendingControl.Name.Substring(
          designColorNameNameBase.Length);
        // User clicked on the design color name, so get the panel
        // with the design color.
        Control[] foundPanels = colorReplacementPanel.Controls.Find(
          designColorPanelNameBase + colorIndex, false);
        if (foundPanels.Length > 0 && foundPanels[0] is Panel)
        {
          colorPanel = foundPanels[0] as Panel;
        }
      }
      else if (sendingControl.Name.StartsWith(replacementColorPanelNameBase))
      {
        colorIndex = sendingControl.Name.Substring(
          replacementColorPanelNameBase.Length);
        // User clicked on the replacement color panel, so get the panel
        // with the original design color.
        Control[] foundPanels = colorReplacementPanel.Controls.Find(
          designColorPanelNameBase + colorIndex, false);
        if (foundPanels.Length > 0 && foundPanels[0] is Panel)
        {
          colorPanel = foundPanels[0] as Panel;
        }
      }
      else if (sendingControl.Name.StartsWith(replacementColorNameNameBase))
      {
        colorIndex = sendingControl.Name.Substring(
          replacementColorNameNameBase.Length);
        // User clicked on the replacement color name, so get the panel
        // with the original design color.
        Control[] foundPanels = colorReplacementPanel.Controls.Find(
          designColorPanelNameBase + colorIndex, false);
        if (foundPanels.Length > 0 && foundPanels[0] is Panel)
        {
          colorPanel = foundPanels[0] as Panel;
        }
      }

      // Remove the highlight from the previously-selected design color,
      // if any.
      if (currentDesignColor != null)
      {
        Control[] foundPanels = colorReplacementPanel.Controls.Find(
          designColorNameNameBase + currentDesignColor.Item3, false);
        if (foundPanels.Length > 0 && foundPanels[0] is Label)
        {
          Label oldLabel = foundPanels[0] as Label;
          oldLabel.BackColor = SystemColors.Control;
          oldLabel.ForeColor = SystemColors.ControlText;
        }
      }

      if (colorPanel != null)
      {
        Color designColor = colorPanel.BackColor;
        Control[] foundLabels = colorReplacementPanel.Controls.Find(
          designColorNameNameBase + colorIndex, false);
        if (foundLabels.Length > 0)
        {
          Label colorNameLabel = foundLabels[0] as Label;
          // Highlight the selected color name.
          colorNameLabel.BackColor = SystemColors.Highlight;
          colorNameLabel.ForeColor = SystemColors.HighlightText;
          // Get the color name.
          string colorName = colorNameLabel.Text;
          // Set the current design color.
          currentDesignColor = new Tuple<string, Color, int>(
            colorName, designColor, int.Parse(colorIndex));
          // Null out the replacement color to force one to be chosen
          // subsequent to choosing the design color.
          currentReplacementColor = null;
        }
      }
    }

    private void PaletteColor_MouseClick(object sender, MouseEventArgs e)
    {
      Control sendingControl = sender as Control;
      Panel colorPanel = null;
      string colorIndex = "-1";
      if (sendingControl.Name.StartsWith(paletteColorPanelNameBase))
      {
        colorIndex = sendingControl.Name.Substring(
          paletteColorPanelNameBase.Length);
        colorPanel = sender as Panel;
      }
      else if (sendingControl.Name.StartsWith(paletteColorNameNameBase))
      {
        colorIndex = sendingControl.Name.Substring(
          paletteColorNameNameBase.Length);
        // User clicked on the palette color name, so get the panel
        // with the palette color.
        Control[] foundPanels = colorSelectionPanel.Controls.Find(
          paletteColorPanelNameBase + colorIndex, false);
        if (foundPanels.Length > 0 && foundPanels[0] is Panel)
        {
          colorPanel = foundPanels[0] as Panel;
        }
      }

      if (colorPanel != null)
      {
        Color replacementColor = colorPanel.BackColor;
        Control[] foundLabels = colorSelectionPanel.Controls.Find(
          paletteColorNameNameBase + colorIndex, false);
        if (foundLabels.Length > 0)
        {
          string colorName = foundLabels[0].Text;
          currentReplacementColor = new Tuple<string, Color, int>(
            colorName, replacementColor, int.Parse(colorIndex));
          ApplyCurrentReplacementToMapping();
        }
      }
    }

    private void paletteSectionComboBox_SelectedIndexChanged(object sender,
      EventArgs e)
    {
      ClearTransitoryPaletteControls();

      PaletteSection paletteSection =
        palette.Section((string)paletteSectionComboBox.SelectedItem);
      if (paletteSection != null)
      {
        Dictionary<string, Color> sectionColors = paletteSection.Colors;
        int colorIndex = 0;
        foreach (string colorName in sectionColors.Keys)
        {
          AddPaletteColorToPanel(
            colorIndex++, sectionColors[colorName], colorName);
        }
        AddPaletteColorToPanel(
          colorIndex++, Color.FromArgb(0, 0, 0, 0), "uncolored");
      }
    }

    private void otherColorButton_Click(object sender, EventArgs e)
    {
      ColorSelectionForm dlg = new ColorSelectionForm(
        currentDesignColor != null ? currentDesignColor.Item2 :
        Color.CornflowerBlue);
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        Color replacementColor = dlg.SelectedColor;
        string colorName = string.Format("{0,3} {1,3} {2,3}",
          replacementColor.R, replacementColor.G, replacementColor.B);

        // Attempt to find a "nearest color" name to use in place of the
        // RBG triple.
        Bitmap renderedImage = design.RenderedImage;
        if (renderedImage != null)
        {
          Graphics g = Graphics.FromImage(renderedImage);
          Color nearestColor = g.GetNearestColor(replacementColor);
          KnownColor knownColor = KnownColors.MatchingColor(nearestColor);
          if (knownColor != 0)
          {
            colorName = knownColor.ToString();
          }
          g.Dispose();
        }

        currentReplacementColor = new Tuple<string, Color, int>(
          colorName, replacementColor, -1);
        ApplyCurrentReplacementToMapping();
      }
    }

    private void RingFilterButton_CheckedChanged(object sender, EventArgs e)
    {
      // For each action, one button is checked, while another is unchecked.
      // We only want to draw the preview image once, so we do it for the
      // checked button.
      if ((sender as RadioButton).Checked)
      {
        DrawPreviewImage();
      }
    }

    public string RingFilterString
    {
      get { return ringFilterString; }
    }

  }
}
