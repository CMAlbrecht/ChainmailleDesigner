// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: NewDesignForm.cs


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
using System.IO;
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class NewDesignForm : Form
  {
    private string designName = string.Empty;
    private string weaveName = string.Empty;
    private string weaveFile = string.Empty;
    private float designWidth = 1;
    private float designHeight = 1;
    private DesignSizeUnitsEnum designSizeUnits =
      DesignSizeUnitsEnum.Units;
    private string designScale = string.Empty;
    bool roundSizeUp = true;
    private ChainmaillePattern selectedPattern = null;
    private WrapEnum wrap = WrapEnum.None;
    private string description = string.Empty;
    private DateTime designDate = DateTime.UtcNow;
    private string designedBy = Properties.Settings.Default.DesignerName;
    private string designedFor = string.Empty;

    public NewDesignForm()
    {
      InitializeComponent();
      wrapComboBox.Items.AddRange(
        EnumUtils.GetAllDescriptions<WrapEnum>());
      wrapComboBox.SelectedIndex = 0;
      designUnitsComboBox.SelectedItem = designSizeUnits.GetDescription();
      sizeRoundingComboBox.SelectedIndex = roundSizeUp ? 0 : 1;
      weaveFile = Properties.Settings.Default.CurrentPattern;
      WeaveName = Path.GetFileNameWithoutExtension(weaveFile);
      SetSelectedPattern();
      SetSelectedScale();
      SetWidthHeightUnits();
      descriptionTextBox.Text = description;
      dateTextBox.Text = designDate.ToString();
      designedByTextBox.Text = designedBy;
      designedForTextBox.Text = designedFor;
    }

    private void changeWeaveButton_Click(object sender, EventArgs e)
    {
      WeaveSelectionForm dlg = new WeaveSelectionForm();
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        weaveFile = dlg.SelectedWeaveFile;
        WeaveName = dlg.SelectedWeaveName;
        SetSelectedPattern();
        SetSelectedScale();
        SetWidthHeightUnits();
      }
    }

    public string Description
    {
      get { return description; }
      set
      {
        description = value;
        descriptionTextBox.Text = description;
      }
    }

    public DateTime DesignDate
    {
      get { return designDate; }
      set
      {
        designDate = value;
        dateTextBox.Text = designDate.ToString();
      }
    }

    public string DesignedBy
    {
      get { return designedBy; }
      set
      {
        designedBy = value;
        designedByTextBox.Text = designedBy;
      }
    }

    public string DesignedFor
    {
      get { return designedFor; }
      set
      {
        designedFor = value;
        designedForTextBox.Text = designedFor;
      }
    }

    public float DesignHeight
    {
      get { return designHeight; }
    }

    public string DesignName
    {
      get { return designName; }
    }

    private void designScaleComboBox_SelectedIndexChanged(
      object sender, EventArgs e)
    {
      SetSelectedScale();
      SetWidthHeightUnits();
    }

    public SizeF DesignSize
    {
      get { return new SizeF(designWidth, designHeight); }
    }

    public DesignSizeUnitsEnum DesignSizeUnits
    {
      get { return designSizeUnits; }
    }

    private void designUnitsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetWidthHeightUnits();
    }

    public float DesignWidth
    {
      get { return designWidth; }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      try
      {
        designName = designNameTextBox.Text;

        wrap = EnumUtils.ToEnum<WrapEnum>(
          wrapComboBox.SelectedItem.ToString());

        designScale = selectedPattern != null && designScaleComboBox.Enabled &&
          designScaleComboBox.SelectedItem != null ?
          designScaleComboBox.SelectedItem.ToString() : string.Empty;

        designedFor = designedForTextBox.Text;
        DateTime dateTime;
        if (DateTime.TryParse(dateTextBox.Text, out dateTime))
        {
          designDate = dateTime;
        }
        designedBy = designedByTextBox.Text;
        description = descriptionTextBox.Text;

        float d;
        if (float.TryParse(designWidthTextBox.Text, out d))
        {
          designWidth = d;
        }

        if (float.TryParse(designHeightTextBox.Text, out d))
        {
          designHeight = d;
        }

        roundSizeUp = sizeRoundingComboBox.SelectedIndex == 0;
      }
      catch { }
    }

    public bool RoundSizeUp
    {
      get { return roundSizeUp; }
    }

    public PatternScale DesignScale
    {
      get
      {
        return selectedPattern != null ?
          selectedPattern.Scale(designScale) : null;
      }
    }

    private void SetSelectedPattern()
    {
      selectedPattern = new ChainmaillePattern(weaveFile, weaveName);

      // If the pattern has scales, allow them to be selected.
      designScaleComboBox.Enabled = selectedPattern.HasScales;
      string selectedScale = designScaleComboBox.SelectedItem != null ?
        designScaleComboBox.SelectedItem.ToString() : string.Empty;
      designScaleComboBox.Items.Clear();
      if (selectedPattern.HasScales)
      {
        foreach (PatternScale patternScale in selectedPattern.Scales)
        {
          designScaleComboBox.Items.Add(patternScale.Description);
        }
        if (designScaleComboBox.Items.Contains(selectedScale))
        {
          designScaleComboBox.SelectedItem = selectedScale;
        }
        else
        {
          designScaleComboBox.SelectedIndex = 0;
        }
      }
    }

    private void SetSelectedScale()
    {
      designScale = selectedPattern != null && designScaleComboBox.Enabled &&
        designScaleComboBox.SelectedItem != null ?
        designScaleComboBox.SelectedItem.ToString() : string.Empty;
      PatternScale selectedScale = DesignScale;

      // Don't show distance units if the selected scale has no measure.
      string selectedUnits = designUnitsComboBox.SelectedItem != null ?
        designUnitsComboBox.SelectedItem.ToString() : string.Empty;
      designUnitsComboBox.Items.Clear();
      designUnitsComboBox.Items.AddRange(
        EnumUtils.GetAllDescriptions<DesignSizeUnitsEnum>());
      if (selectedScale == null || selectedScale.UnitSize.IsEmpty)
      {
        designUnitsComboBox.Items.Remove(DesignSizeUnitsEnum.Centimeters.GetDescription());
        designUnitsComboBox.Items.Remove(DesignSizeUnitsEnum.Inches.GetDescription());
      }
      if (designUnitsComboBox.Items.Contains(selectedUnits))
      {
        designUnitsComboBox.SelectedItem = selectedUnits;
      }
      else
      {
        designUnitsComboBox.SelectedItem = DesignSizeUnitsEnum.Units.GetDescription();
      }
    }

    private void SetWidthHeightUnits()
    {
      // Debug
      string adjustmentUnitsString =
        designUnitsComboBox.SelectedItem.ToString();
      // End debug
      designSizeUnits =
        EnumUtils.ToEnumFromDescription<DesignSizeUnitsEnum>(
        designUnitsComboBox.SelectedItem.ToString());
      if (designSizeUnits == DesignSizeUnitsEnum.RowsColumns)
      {
        widthUnitsLabel.Text = ChainmailleDesignerConstants.columns;
        heightUnitsLabel.Text = ChainmailleDesignerConstants.rows;
      }
      else
      {
        widthUnitsLabel.Text = designSizeUnits.GetDescription();
        heightUnitsLabel.Text = designSizeUnits.GetDescription();
      }

      sizeRoundingComboBox.Visible =
        designSizeUnits != DesignSizeUnitsEnum.Units;
    }

    public string WeaveFile
    {
      get { return weaveFile; }
    }

    public string WeaveName
    {
      get { return weaveName; }
      set
      {
        weaveName = value;
        weaveTextBox.Text = weaveName;
      }
    }

    public WrapEnum Wrap
    {
      get { return wrap; }
    }

  }
}
