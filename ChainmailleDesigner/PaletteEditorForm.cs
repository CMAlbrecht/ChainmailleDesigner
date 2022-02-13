// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: PaletteEditorForm.cs


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
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class PaletteEditorForm : Form
  {
    private Palette palette;
    private bool hasBeenChanged = false;

    private float xScaleFactor = 1;
    private float yScaleFactor = 1;
    private Size deleteButtonSize;
    private Size colorPanelSize;
    private Size colorNameSize;

    private const string formNameBase = "Edit Palette";
    private const string sectionNameTextBoxNameBase = "sectionNameTextBox";
    private const string sectionAbbreviationTextBoxNameBase =
      "sectionAbbreviationTextBox";
    private const string addColorButtonNameBase = "addColorButton";
    private const string deleteButtonNameBase = "deleteButton";
    private const string colorPanelNameBase = "colorPanel";
    private const string colorNameTextBoxNameBase = "colorNameTextBox";
    private const string newTabText = "   +   ";


    // section index => number of colors
    Dictionary<int, int> nrSectionColors = new Dictionary<int, int>();
    Dictionary<int, Panel> sectionColorPanels = new Dictionary<int, Panel>();

    public PaletteEditorForm(Palette paletteToEdit)
    {
      InitializeComponent();

      palette = paletteToEdit;
      InitializePaletteControls();
    }

    private DialogResult AskWhetherToSaveChanges()
    {
      string title = "Save Changes?";
      string message = "The palette has been changed. Do you wish to retain " +
        "your changes?";
      return MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
    }

    private void ColorPanel_MouseClick(object sender, MouseEventArgs e)
    {
      Panel colorPanel = sender as Panel;
      ColorSelectionForm dlg = new ColorSelectionForm(colorPanel.BackColor);
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        colorPanel.BackColor = dlg.SelectedColor;
        hasBeenChanged = true;
      }
    }

    private void InitializeColorControls(Panel sectionColorPanel,
      int colorIndex, Color color, string colorName)
    {
      // Row and column indices.
      int r = colorIndex / 4;
      int c = colorIndex % 4;

      Button deleteButton = new Button();
      deleteButton.Location = new Point(
        (int)((5 + 270 * c) * xScaleFactor),
        (int)((30 + 60 * r) * yScaleFactor));
      deleteButton.Name = deleteButtonNameBase + colorIndex;
      deleteButton.Size = deleteButtonSize;
      deleteButton.TabIndex = 7 + 3 * colorIndex;
      deleteButton.Text = "x";
      deleteButton.Click += new EventHandler(DeleteColorButton_Click);
      sectionColorPanel.Controls.Add(deleteButton);

      Panel colorPanel = new Panel();
      colorPanel.BackColor = color;
      colorPanel.BorderStyle = BorderStyle.FixedSingle;
      colorPanel.Location = new Point(
      (int)((30 + 270 * c) * xScaleFactor),
      (int)((20 + 60 * r) * yScaleFactor));
      colorPanel.Name = colorPanelNameBase + colorIndex;
      colorPanel.Size = colorPanelSize;
      colorPanel.TabIndex = 8 + 3 * colorIndex;
      colorPanel.MouseClick += new MouseEventHandler(ColorPanel_MouseClick);
      sectionColorPanel.Controls.Add(colorPanel);

      TextBox colorNameTextBox = new TextBox();
      colorNameTextBox.Location = new Point(
      (int)((85 + 270 * c) * xScaleFactor),
      (int)((20 + 60 * r) * yScaleFactor));
      colorNameTextBox.Name = colorNameTextBoxNameBase + colorIndex;
      colorNameTextBox.Size = colorNameSize;
      colorNameTextBox.TabIndex = 9 + 3 * colorIndex;
      colorNameTextBox.Text = colorName;
      colorNameTextBox.TextChanged += new EventHandler(
        TextBox_TextChanged);
      sectionColorPanel.Controls.Add(colorNameTextBox);
    }

    private void InitializePaletteControls()
    {
      // Determine scale factors based on design size.
      xScaleFactor = ClientSize.Width / 1169F;
      yScaleFactor = ClientSize.Height / 669F;

      // Disconnect event handlers.
      paletteNameTextBox.TextChanged -= new EventHandler(TextBox_TextChanged);
      paletteDescriptionTextBox.TextChanged -=
        new EventHandler(TextBox_TextChanged);

      if (!string.IsNullOrEmpty(palette.Name))
      {
        Text = formNameBase + " - " + palette.Name;
      }
      paletteNameTextBox.Text = palette.Name;
      paletteDescriptionTextBox.Text = palette.Description;

      int nrSectionsInitialized = 0;
      foreach (PaletteSection section in palette.Sections)
      {
        TabPage tabPage = new TabPage(section.Name);
        paletteSectionTabControl.TabPages.Add(tabPage);

        InitializePaletteSectionControls(tabPage, section,
          nrSectionsInitialized);

        nrSectionsInitialized++;
      }

      // If there were no sections, i.e. the palette is new,
      // initialize one empty section.
      if (nrSectionsInitialized == 0)
      {
        TabPage tabPage = new TabPage(string.Empty);
        paletteSectionTabControl.TabPages.Add(tabPage);

        InitializePaletteSectionControls(tabPage, nrSectionsInitialized,
          string.Empty, string.Empty);
        ModifyAddColorButtonPlacementForInitialization(tabPage,
          nrSectionsInitialized);

        nrSectionsInitialized++;
      }


      // Add a "new section" tab.
      paletteSectionTabControl.TabPages.Add(new TabPage(newTabText));

      // Reconnect event handlers.
      paletteNameTextBox.TextChanged += new EventHandler(TextBox_TextChanged);
      paletteDescriptionTextBox.TextChanged +=
        new EventHandler(TextBox_TextChanged);
    }

    private void InitializePaletteSectionControls(TabPage tabPage,
      PaletteSection section, int sectionIndex)
    {
      InitializePaletteSectionControls(tabPage, sectionIndex, section.Name,
        section.AbbreviatedName);
      ModifyAddColorButtonPlacementForInitialization(tabPage, sectionIndex);

      // Add the colors to the section color panel.
      int colorIndex = 0;
      Panel sectionColorPanel = sectionColorPanels[sectionIndex];
      Dictionary<string, Color> colors = section.Colors;
      foreach (string colorName in colors.Keys)
      {
        InitializeColorControls(sectionColorPanel, colorIndex,
          colors[colorName], colorName);
        colorIndex++;
      }

      nrSectionColors[sectionIndex] = colorIndex;
    }

    private void InitializePaletteSectionControls(TabPage tabPage,
      int sectionIndex, string sectionName, string abbreviatedName)
    {
      deleteButtonSize = new Size(
        (int)(20 * xScaleFactor), (int)(20 * yScaleFactor));
      colorPanelSize = new Size(
        (int)(40 * xScaleFactor), (int)(40 * yScaleFactor));
      colorNameSize = new Size(
        (int)(160 * xScaleFactor), (int)(31 * yScaleFactor));

      Button deleteButton = new Button();
      deleteButton.Location = new Point(
        (int)(15 * xScaleFactor),
        (int)(23 * yScaleFactor));
      deleteButton.Name = "sectionDeleteButton" + sectionIndex;
      deleteButton.Size = deleteButtonSize;
      deleteButton.TabIndex = 0;
      deleteButton.Text = "x";
      deleteButton.Click += new EventHandler(DeleteSectionButton_Click);
      tabPage.Controls.Add(deleteButton);

      Label nameLabel = new Label();
      nameLabel.AutoSize = true;
      nameLabel.Location = new Point(
        (int)(35 * xScaleFactor), (int)(20 * yScaleFactor));
      nameLabel.Name = "sectionNameLabel" + sectionIndex;
      nameLabel.Size = new Size(
        (int)(152 * xScaleFactor), (int)(25 * yScaleFactor));
      nameLabel.TabIndex = 1;
      nameLabel.Text = "Section Name:";
      tabPage.Controls.Add(nameLabel);

      TextBox nameTextBox = new TextBox();
      nameTextBox.Location = new Point(
        (int)(200 * xScaleFactor), (int)(15 * yScaleFactor));
      nameTextBox.Name = sectionNameTextBoxNameBase + sectionIndex;
      nameTextBox.Size = new Size(
        (int)(200 * xScaleFactor), (int)(31 * yScaleFactor));
      nameTextBox.TabIndex = 2;
      nameTextBox.Text = sectionName;
      nameTextBox.TextChanged += new System.EventHandler(
        SectionNameTextBox_TextChanged);
      tabPage.Controls.Add(nameTextBox);

      Label abbreviationLabel = new Label();
      abbreviationLabel.AutoSize = true;
      abbreviationLabel.Location = new Point(
        (int)(440 * xScaleFactor), (int)(20 * yScaleFactor));
      abbreviationLabel.Name = "abbreviationLabel" + sectionIndex;
      abbreviationLabel.Size = new Size(
        (int)(140 * xScaleFactor), (int)(25 * yScaleFactor));
      abbreviationLabel.TabIndex = 3;
      abbreviationLabel.Text = "Abbreviation:";
      tabPage.Controls.Add(abbreviationLabel);

      TextBox abbreviationTextBox = new TextBox();
      abbreviationTextBox.Location = new Point(
        (int)(590 * xScaleFactor), (int)(15 * yScaleFactor));
      abbreviationTextBox.Name =
        sectionAbbreviationTextBoxNameBase + sectionIndex;
      abbreviationTextBox.Size = new Size(
        (int)(70 * xScaleFactor), (int)(31 * yScaleFactor));
      abbreviationTextBox.TabIndex = 4;
      abbreviationTextBox.Text = abbreviatedName;
      abbreviationTextBox.TextChanged += new System.EventHandler(
        TextBox_TextChanged);
      tabPage.Controls.Add(abbreviationTextBox);

      Button addColorButton = new Button();
      addColorButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      addColorButton.Location = new Point(
        (int)(920 * xScaleFactor), (int)(5 * yScaleFactor));
      addColorButton.Name = addColorButtonNameBase + sectionIndex;
      addColorButton.Size = new Size(
        (int)(150 * xScaleFactor), (int)(45 * yScaleFactor));
      addColorButton.TabIndex = 5;
      addColorButton.Text = "Add Color...";
      addColorButton.UseVisualStyleBackColor = true;
      addColorButton.Click += new System.EventHandler(AddColorButton_Click);
      tabPage.Controls.Add(addColorButton);

      Panel sectionColorPanel = new Panel();
      sectionColorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
        AnchorStyles.Left | AnchorStyles.Right;
      sectionColorPanel.Location = new Point(
        (int)(10 * xScaleFactor), (int)(50 * yScaleFactor));
      sectionColorPanel.Name = "sectionColorPanel" + sectionIndex;
      sectionColorPanel.Size = new Size(
        (int)(1091 * xScaleFactor), (int)(406 * yScaleFactor));
      sectionColorPanel.TabIndex = 6;
      tabPage.Controls.Add(sectionColorPanel);

      nrSectionColors.Add(sectionIndex, 0);
      sectionColorPanels.Add(sectionIndex, sectionColorPanel);
    }

    /// <summary>
    /// There is something very odd about the placement of the Add Color
    /// button. It needs to be at one position when initializing the form,
    /// but at a different position when adding a new tab to a running form.
    /// Here, we position it as it needs to be for form initialization.
    /// </summary>
    /// <param name="tabPage"></param>
    /// <param name="sectionIndex"></param>
    private void ModifyAddColorButtonPlacementForInitialization(
      TabPage tabPage, int sectionIndex)
    {
      string buttonName = addColorButtonNameBase + sectionIndex;
      Button addColorButton = null;
      foreach (Control control in tabPage.Controls)
      {
        if (control.Name == buttonName)
        {
          addColorButton = control as Button;
        }
      }
      if (addColorButton != null)
      {
        addColorButton.Location = new Point(
          (int)(200 * xScaleFactor), (int)(5 * yScaleFactor));
      }
    }

    private void SaveChangesIntoPalette()
    {
      if (palette == null)
      {
        // Create a new palette.
        palette = new Palette();
      }

      palette.Name = paletteNameTextBox.Text;
      palette.Description = paletteDescriptionTextBox.Text;

      palette.ClearSections();
      int sectionIndex = 0;
      while (sectionIndex < paletteSectionTabControl.TabCount)
      {
        TabPage tabPage = paletteSectionTabControl.TabPages[sectionIndex];
        if (tabPage.Text != newTabText)
        {
          // Check to make sure there is at least one color in the section.
          if (nrSectionColors[sectionIndex] > 0)
          {
            PaletteSection section = new PaletteSection();

            // Determine the section name.
            TextBox sectionNameTextBox = null;
            TextBox sectionAbbreviationTextBox = null;
            foreach (Control control in tabPage.Controls)
            {
              if (control.Name.StartsWith(sectionNameTextBoxNameBase))
              {
                sectionNameTextBox = control as TextBox;
              }
              else if (control.Name.StartsWith(
                         sectionAbbreviationTextBoxNameBase))
              {
                sectionAbbreviationTextBox = control as TextBox;
              }
            }

            string sectionName = string.Empty;
            if (sectionNameTextBox != null)
            {
              sectionName = sectionNameTextBox.Text;
            }
            if (string.IsNullOrEmpty(sectionName))
            {
              sectionName = "Section " + sectionIndex;
            }
            section.Name = sectionName;

            string sectionAbbreviation = string.Empty;
            if (sectionAbbreviationTextBox != null)
            {
              sectionAbbreviation = sectionAbbreviationTextBox.Text;
            }
            if (string.IsNullOrEmpty(sectionName))
            {
              sectionAbbreviation = "S" + sectionIndex;
            }
            section.AbbreviatedName = sectionAbbreviation;

            // Add the colors to the section.
            string colorTextBoxName;
            Panel sectionColorPanel = sectionColorPanels[sectionIndex];
            for (int colorIndex = 0;
                 colorIndex < nrSectionColors[sectionIndex]; colorIndex++)
            {
              string panelName = colorPanelNameBase + colorIndex;
              colorTextBoxName = colorNameTextBoxNameBase + colorIndex;
              Panel colorPanel = null;
              TextBox colorNameTextBox = null;
              foreach (Control control in sectionColorPanel.Controls)
              {
                if (control.Name == panelName)
                {
                  colorPanel = control as Panel;
                }
                else if (control.Name == colorTextBoxName)
                {
                  colorNameTextBox = control as TextBox;
                }
              }

              if (colorPanel != null && colorNameTextBox != null)
              {
                string colorName = colorNameTextBox.Text;
                if (string.IsNullOrEmpty(colorName))
                {
                  colorName = "Color " + colorIndex;
                }

                section.AddColor(colorPanel.BackColor, colorName);
              }
            }

            palette.AddSection(section);
          }
        }

        sectionIndex++;
      }
    }

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
      hasBeenChanged = true;
    }

    private void SectionNameTextBox_TextChanged(object sender, EventArgs e)
    {
      TextBox_TextChanged(sender, e);

      TextBox textBox = sender as TextBox;
      textBox.Parent.Text = textBox.Text;
    }

    private void SectionTabSelected(object sender, TabControlEventArgs e)
    {
      if (e.TabPage.Text == newTabText)
      {
        // Add controls to this tab, making it no longer a "new section" tab.
        e.TabPage.Text = string.Empty;
        InitializePaletteSectionControls(e.TabPage, e.TabPageIndex,
          string.Empty, string.Empty);

        // Add a new "new section" tab.
        paletteSectionTabControl.TabPages.Add(new TabPage(newTabText));
      }
    }

    private void AddColorButton_Click(object sender, EventArgs e)
    {
      ColorSelectionForm dlg = new ColorSelectionForm();
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        TabPage tabPage = (sender as Control).Parent as TabPage;
        TabControl tabControl = tabPage.Parent as TabControl;
        int sectionIndex = tabControl.SelectedIndex;
        Panel sectionColorPanel = sectionColorPanels[sectionIndex];
        InitializeColorControls(sectionColorPanel,
          nrSectionColors[sectionIndex], dlg.SelectedColor, string.Empty);

        nrSectionColors[sectionIndex]++;
        hasBeenChanged = true;
      }
    }

    private void DeleteColorButton_Click(object sender, EventArgs e)
    {
      Button deleteButton = sender as Button;
      int colorIndex = int.Parse(deleteButton.Name.Substring(
        deleteButtonNameBase.Length));
      Panel sectionColorPanel = deleteButton.Parent as Panel;
      TabPage tabPage = sectionColorPanel.Parent as TabPage;
      TabControl tabControl = tabPage.Parent as TabControl;
      int sectionIndex = tabControl.SelectedIndex;

      string buttonName = deleteButtonNameBase + colorIndex;
      string panelName = colorPanelNameBase + colorIndex;
      string textBoxName = colorNameTextBoxNameBase + colorIndex;

      Button currentColorButton = deleteButton;
      Panel currentColorPanel = null;
      TextBox currentColorTextBox = null;
      foreach (Control control in sectionColorPanel.Controls)
      {
        if (control.Name == panelName)
        {
          currentColorPanel = control as Panel;
        }
        else if (control.Name == textBoxName)
        {
          currentColorTextBox = control as TextBox;
        }
      }

      if (currentColorPanel != null && currentColorTextBox != null &&
          currentColorButton != null)
      {
        if (colorIndex < nrSectionColors[sectionIndex] - 1)
        {
          // Shift later colors to fill in the gap left by the deleted color.
          Button nextColorButton;
          Panel nextColorPanel;
          TextBox nextColorTextBox;
          for (; colorIndex < nrSectionColors[sectionIndex] - 1; colorIndex++)
          {
            // Hunt for the next color and name amongst the panel's controls.
            int nextColorIndex = colorIndex + 1;

            buttonName = deleteButtonNameBase + nextColorIndex;
            panelName = colorPanelNameBase + nextColorIndex;
            textBoxName = colorNameTextBoxNameBase + nextColorIndex;
            nextColorButton = null;
            nextColorPanel = null;
            nextColorTextBox = null;

            foreach (Control control in sectionColorPanel.Controls)
            {
              if (control.Name == buttonName)
              {
                nextColorButton = control as Button;
              }
              if (control.Name == panelName)
              {
                nextColorPanel = control as Panel;
              }
              else if (control.Name == textBoxName)
              {
                nextColorTextBox = control as TextBox;
              }
            }

            if (nextColorButton != null && nextColorPanel != null &&
                nextColorTextBox != null)
            {
              // Shift the next color and name.
              currentColorPanel.BackColor = nextColorPanel.BackColor;
              currentColorTextBox.Text = nextColorTextBox.Text;
            }
            else
            {
              break;
            }

            currentColorButton = nextColorButton;
            currentColorPanel = nextColorPanel;
            currentColorTextBox = nextColorTextBox;
          }
        }

        // Delete the last set of color controls.
        sectionColorPanel.Controls.Remove(currentColorButton);
        sectionColorPanel.Controls.Remove(currentColorPanel);
        sectionColorPanel.Controls.Remove(currentColorTextBox);
        currentColorButton.Dispose();
        currentColorPanel.Dispose();
        currentColorTextBox.Dispose();

        nrSectionColors[sectionIndex]--;
        hasBeenChanged = true;
      }
    }

    private void DeleteSectionButton_Click(object sender, EventArgs e)
    {
      // Don't allow the delete if the tab control has only two tabs
      // (i.e. a single section and the add-a-section tab).
      // The palette must have at least one section.
      TabPage tabPage = (sender as Control).Parent as TabPage;
      TabControl tabControl = tabPage.Parent as TabControl;
      int sectionIndex = tabControl.SelectedIndex;
      if (tabControl.TabCount > 2)
      {
        tabControl.TabPages.RemoveAt(sectionIndex);
        // Rebuild the dictionaries, leaving out the removed section.
        Dictionary<int, int> newNrSectionColors = new Dictionary<int, int>();
        Dictionary<int, Panel> newSectionColorPanels =
          new Dictionary<int, Panel>();
        foreach (int index in nrSectionColors.Keys)
        {
          if (index < sectionIndex)
          {
            newNrSectionColors.Add(index, nrSectionColors[index]);
            newSectionColorPanels.Add(index, sectionColorPanels[index]);
          }
          else if (index > sectionIndex)
          {
            newNrSectionColors.Add(index - 1, nrSectionColors[index]);
            newSectionColorPanels.Add(index - 1, sectionColorPanels[index]);
          }
        }
        nrSectionColors = newNrSectionColors;
        sectionColorPanels = newSectionColorPanels;
        hasBeenChanged = true;
      }
    }

    private void PaletteEditorForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      bool continueWithOperation = true;
      if (hasBeenChanged)
      {
        // Ask whether to save.
        DialogResult dialogResult = AskWhetherToSaveChanges();
        if (dialogResult == DialogResult.Yes)
        {
          // Save the palette changes.
          SaveChangesIntoPalette();
        }
        else if (dialogResult == DialogResult.Cancel)
        {
          continueWithOperation = false;
        }
      }
      e.Cancel = !continueWithOperation;
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      if (hasBeenChanged)
      {
        SaveChangesIntoPalette();
      }
      hasBeenChanged = false;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      hasBeenChanged = false;
    }
  }
}
