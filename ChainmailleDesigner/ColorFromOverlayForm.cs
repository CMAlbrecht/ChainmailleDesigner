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
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class ColorFromOverlayForm : Form
  {
    string ringFilterString = string.Empty;
    ChainmailleDesign design = null;
    Palette palette = null;
    PaletteSection paletteSection = null;

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
      }
    }

    public string RingFilterString
    {
      get { return ringFilterString; }
    }

  }
}
