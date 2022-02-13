// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: DesignSizeForm.cs


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
  public partial class DesignSizeForm : Form
  {
    ChainmaillePattern chainmaillePattern;
    PatternScale patternScale = null;
    WrapEnum designWrap = WrapEnum.None;
    private Size originalSizeInUnits;
    private SizeF currentSize;
    private SizeAdjustment currentAdjustment =
      new SizeAdjustment(0, 0, 0, 0, DesignSizeUnitsEnum.Units);
    private Size sizeInUnits;
    private SizeAdjustment adjustmentInUnits =
      new SizeAdjustment(0, 0, 0, 0, DesignSizeUnitsEnum.Units);
    bool roundSizeUp = true;
    string formatString = "F0";
    private bool updateResponseEnabled = true;

    private DesignSizeUnitsEnum adjustmentUnits =
      DesignSizeUnitsEnum.Units;

    public DesignSizeForm(Size designSizeInUnits, ChainmaillePattern pattern,
      PatternScale scale, WrapEnum wrap)
    {
      InitializeComponent();

      originalSizeInUnits = designSizeInUnits;
      sizeInUnits = new Size(originalSizeInUnits.Width,
        originalSizeInUnits.Height);
      CurrentSize = new SizeF(originalSizeInUnits.Width,
        originalSizeInUnits.Height);

      chainmaillePattern = pattern;
      patternScale = scale;
      designWrap = wrap;

      designUnitsComboBox.Items.AddRange(
        EnumUtils.GetAllDescriptions<DesignSizeUnitsEnum>());
      if (scale == null || scale.UnitSize.IsEmpty)
      {
        // Without a scale, there's no way to relate pattern units to distance.
        designUnitsComboBox.Items.Remove(
          DesignSizeUnitsEnum.Centimeters.GetDescription());
        designUnitsComboBox.Items.Remove(
          DesignSizeUnitsEnum.Inches.GetDescription());
      }
      designUnitsComboBox.SelectedItem = adjustmentUnits.GetDescription();
      sizeRoundingComboBox.SelectedIndex = roundSizeUp ? 0 : 1;
    }

    public DesignSizeUnitsEnum AdjustmentUnits
    {
      get { return adjustmentUnits; }
    }

    private SizeF CurrentSize
    {
      set
      {
        currentSize = value;
        designWidthTextBox.Text = currentSize.Width.ToString(formatString);
        designHeightTextBox.Text = currentSize.Height.ToString(formatString);
      }
    }

    private void designUnitsComboBox_SelectedIndexChanged(object sender,
      EventArgs e)
    {
      if (updateResponseEnabled)
      {
        updateResponseEnabled = false;

        SizeAdjustment previousAdjustment =
          new SizeAdjustment(currentAdjustment);

        adjustmentUnits =
          EnumUtils.ToEnumFromDescription<DesignSizeUnitsEnum>(
          designUnitsComboBox.SelectedItem.ToString());

        int formatDecimals;
        switch (adjustmentUnits)
        {
          case DesignSizeUnitsEnum.Centimeters:
            formatDecimals = 1;
            break;
          case DesignSizeUnitsEnum.Inches:
            formatDecimals = 2;
            break;
          default:
            formatDecimals = 0;
            break;
        }
        formatString = "F" + formatDecimals;

        // Convert values to the new units.
        currentAdjustment = chainmaillePattern.ConvertAdjustmentUnits(
          previousAdjustment, adjustmentUnits, patternScale);
        UpdateSizeDisplay();

        // Change the unit labels.
        if (adjustmentUnits == DesignSizeUnitsEnum.RowsColumns)
        {
          widthUnitsLabel.Text = ChainmailleDesignerConstants.columns;
          heightUnitsLabel.Text = ChainmailleDesignerConstants.rows;
        }
        else
        {
          widthUnitsLabel.Text = adjustmentUnits.GetDescription();
          heightUnitsLabel.Text = adjustmentUnits.GetDescription();
        }

        sizeRoundingComboBox.Visible =
          adjustmentUnits != DesignSizeUnitsEnum.Units;

        topEdgeNumericUpDown.DecimalPlaces = formatDecimals;
        bottomEdgeNumericUpDown.DecimalPlaces = formatDecimals;
        leftEdgeNumericUpDown.DecimalPlaces = formatDecimals;
        rightEdgeNumericUpDown.DecimalPlaces = formatDecimals;

        updateResponseEnabled = true;
      }
    }

    private void numericUpDown_ValueChanged(object sender, EventArgs e)
    {
      if (updateResponseEnabled)
      {
        updateResponseEnabled = false;

        currentAdjustment = new SizeAdjustment(
          (float)topEdgeNumericUpDown.Value,
          (float)bottomEdgeNumericUpDown.Value,
          (float)leftEdgeNumericUpDown.Value,
          (float)rightEdgeNumericUpDown.Value,
          adjustmentUnits);
        UpdateSizeDisplay();

        updateResponseEnabled = true;
      }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      roundSizeUp = sizeRoundingComboBox.SelectedIndex == 0;

      // Determine the adjustment in pattern units.
      adjustmentInUnits =
        chainmaillePattern.ConvertAdjustmentUnits(currentAdjustment,
        DesignSizeUnitsEnum.Units, patternScale);

      // Round the adjustments; we want only whole units.
      adjustmentInUnits.UpperLeftAdjustment.Width = (float)(roundSizeUp ?
        Math.Ceiling(adjustmentInUnits.UpperLeftAdjustment.Width) :
        Math.Floor(adjustmentInUnits.UpperLeftAdjustment.Width));
      adjustmentInUnits.UpperLeftAdjustment.Height = (float)(roundSizeUp ?
        Math.Ceiling(adjustmentInUnits.UpperLeftAdjustment.Height) :
        Math.Floor(adjustmentInUnits.UpperLeftAdjustment.Height));
      adjustmentInUnits.LowerRightAdjustment.Width = (float)(roundSizeUp ?
        Math.Ceiling(adjustmentInUnits.LowerRightAdjustment.Width) :
        Math.Floor(adjustmentInUnits.LowerRightAdjustment.Width));
      adjustmentInUnits.LowerRightAdjustment.Height = (float)(roundSizeUp ?
        Math.Ceiling(adjustmentInUnits.LowerRightAdjustment.Height) :
        Math.Floor(adjustmentInUnits.LowerRightAdjustment.Height));

      // Determine the adjusted size in pattern units.
      sizeInUnits = new Size(
        (int)(originalSizeInUnits.Width +
        adjustmentInUnits.UpperLeftAdjustment.Width +
        adjustmentInUnits.LowerRightAdjustment.Width),
        (int)(originalSizeInUnits.Height +
        adjustmentInUnits.UpperLeftAdjustment.Height +
        adjustmentInUnits.LowerRightAdjustment.Height));
    }

    public SizeAdjustment SizeAdjustment
    {
      get { return adjustmentInUnits; }
    }

    public Size SizeInUnits
    {
      get { return sizeInUnits; }
    }

    private void UpdateSizeDisplay()
    {
      SizeF designSize = new SizeF(originalSizeInUnits);
      if (chainmaillePattern != null)
      {
        designSize = chainmaillePattern.ConvertSizeUnits(
          originalSizeInUnits, adjustmentUnits, patternScale, designWrap);
      }

      SizeF sizeChange = new SizeF(
        currentAdjustment.UpperLeftAdjustment.Width +
        currentAdjustment.LowerRightAdjustment.Width,
        currentAdjustment.UpperLeftAdjustment.Height +
        currentAdjustment.LowerRightAdjustment.Height);

      CurrentSize = designSize + sizeChange;

      topEdgeNumericUpDown.Value =
        (decimal)currentAdjustment.UpperLeftAdjustment.Height;
      bottomEdgeNumericUpDown.Value =
        (decimal)currentAdjustment.LowerRightAdjustment.Height;
      leftEdgeNumericUpDown.Value =
        (decimal)currentAdjustment.UpperLeftAdjustment.Width;
      rightEdgeNumericUpDown.Value =
        (decimal)currentAdjustment.LowerRightAdjustment.Width;
    }

  }
}
