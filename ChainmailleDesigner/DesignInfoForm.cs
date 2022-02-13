// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: DesignInfoForm.cs


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
  public partial class DesignInfoForm : Form
  {
    private string description;
    private DateTime designDate;
    private string designedBy;
    private string designedFor;
    private int designHeight;
    private string designName;
    private int designWidth;
    private string weaveName;
    private string weaveFile = string.Empty;
    private ChainmaillePattern selectedPattern = null;
    private ChainmailleDesign chainmailleDesign;
    private ChainmailleDesignerForm parentForm;
    private WrapEnum wrap;
    private PatternScale scale;
    SizeAdjustment sizeAdjustment =
      new SizeAdjustment(0, 0, 0, 0, DesignSizeUnitsEnum.Units);

    public DesignInfoForm(ChainmailleDesign design, ChainmailleDesignerForm parent)
    {
      InitializeComponent();

      parentForm = parent;
      chainmailleDesign = design;
      DesignName = design.DesignName;
      WeaveName = design.WeaveName;
      wrapComboBox.Items.AddRange(
        EnumUtils.GetAllDescriptions<WrapEnum>());
      selectedPattern = design.ChainmailPattern;
      SetSelectedPatternScales();

      Wrap = design.Wrap;
      PatternScale = design.Scale;
      DesignSizeInUnits = design.SizeInUnits;

      DesignedFor = design.DesignedFor;
      DesignDate = design.DesignDate;
      DesignedBy = design.DesignedBy;
      Description = design.Description;
    }

    private void changeSizeButton_Click(object sender, EventArgs e)
    {
      if (selectedPattern != null)
      {
        DesignSizeForm dlg = new DesignSizeForm(DesignSizeInUnits,
          selectedPattern, scale, wrap);
        if (dlg.ShowDialog() == DialogResult.OK)
        {
          sizeAdjustment = dlg.SizeAdjustment;
          DesignSizeInUnits = dlg.SizeInUnits;
        }
      }
    }

    private void changeWeaveButton_Click(object sender, EventArgs e)
    {
      WeaveSelectionForm dlg = new WeaveSelectionForm();
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        weaveFile = dlg.SelectedWeaveFile;
        WeaveName = dlg.SelectedWeaveName;
        SetSelectedPattern();
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

    public string DesignName
    {
      get { return designName; }
      set
      {
        designName = value;
        designNameTextBox.Text = designName;
      }
    }

    private void designScaleComboBox_SelectedIndexChanged(object sender,
      EventArgs e)
    {
      scale = selectedPattern.Scale(
        designScaleComboBox.SelectedItem.ToString());
    }

    public Size DesignSizeInUnits
    {
      get
      { return new Size(designWidth, designHeight); }
      set
      {
        designWidth = value.Width;
        designHeight = value.Height;

        designWidthTextBox.Text = designWidth.ToString();
        designHeightTextBox.Text = designHeight.ToString();
      }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      designName = designNameTextBox.Text;
      weaveName = weaveTextBox.Text;
      wrap = EnumUtils.ToEnumFromDescription<WrapEnum>(
        wrapComboBox.SelectedItem.ToString());

      if (selectedPattern != null && selectedPattern.HasScales &&
          designScaleComboBox.SelectedItem != null)
      {
        foreach (PatternScale patternScale in selectedPattern.Scales)
        {
          if (patternScale.Description ==
                designScaleComboBox.SelectedItem.ToString())
          {
            scale = patternScale;
            break;
          }
        }
      }

      int i;
      if (int.TryParse(designWidthTextBox.Text, out i))
      {
        designWidth = i;
      }
      if (int.TryParse(designHeightTextBox.Text, out i))
      {
        designHeight = i;
      }
      designedFor = designedForTextBox.Text;
      DateTime dateTime;
      if (DateTime.TryParse(dateTextBox.Text, out dateTime))
      {
        designDate = dateTime;
      }
      designedBy = designedByTextBox.Text;
      description = descriptionTextBox.Text;
    }

    public PatternScale PatternScale
    {
      get { return scale; }
      set
      {
        if (selectedPattern == null || !selectedPattern.HasScales)
        {
          scale = null;
        }
        else if (value != null)
        {
          scale = value;
          designScaleComboBox.SelectedItem = scale.Description;
        }
      }
    }

    private void SetSelectedPattern()
    {
      selectedPattern = new ChainmaillePattern(weaveFile, weaveName);
      SetSelectedPatternScales();
    }

    private void SetSelectedPatternScales()
    {
      // If the pattern has scales, allow them to be selected.
      designScaleComboBox.Enabled = selectedPattern != null ?
        selectedPattern.HasScales : false;
      string selectedScale = designScaleComboBox.SelectedItem != null ?
        designScaleComboBox.SelectedItem.ToString() : string.Empty;
      designScaleComboBox.Items.Clear();
      if (selectedPattern != null && selectedPattern.HasScales)
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
      else
      {
        scale = null;
      }
    }

    public SizeAdjustment SizeAdjustment
    {
      get { return sizeAdjustment; }
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
      set
      {
        wrap = value;
        wrapComboBox.SelectedItem = wrap.GetDescription();
      }
    }

    private void wrapComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      wrap = EnumUtils.ToEnumFromDescription<WrapEnum>(
        wrapComboBox.SelectedItem.ToString());
    }

  }
}
