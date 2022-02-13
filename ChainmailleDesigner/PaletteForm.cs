// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: PaletteForm.cs


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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class PaletteForm : Form
  {
    // The parent form.
    private Form parent = null;
    // The palette being managed by this form.
    private Palette palette = null;
    // A list of the non-fixed controls added to the palette panel to provide
    // access to the palette sections and colors.
    private List<Control> adaptiveControls = new List<Control>();
    private float xScaleFactor = 1;
    private float yScaleFactor = 1;

    private Color leftButtonColor = Color.Purple;
    private Color rightButtonColor = Color.Gold;
    private Color outlineColor = Properties.Settings.Default.OutlineColor;
    private Color backgroundColor = Properties.Settings.Default.BackgroundColor;

    // Image to show which colors are selected.
    private Bitmap selectedColorImage = null;
    private Graphics selectedColorImageGraphics = null;
    // These are the colors in the source image which will be replaced when
    // the image is drawn.
    private Color selectedColorImageBackgroundColor = Color.FromArgb(229, 229, 229);
    private Color selectedColorImageOutlineColor = Color.FromArgb(25, 25, 25);
    private Color selectedColorImageLeftColor = Color.FromArgb(0, 255, 0);
    private Color selectedColorImageRightColor = Color.FromArgb(255, 0, 0);

    public PaletteForm(Form parentForm = null)
    {
      InitializeComponent();
      Left += 1;
      parent = parentForm;
      selectedColorImage = new Bitmap(
        Properties.Settings.Default.PaletteSelectedColorImageFile);
      Size selectedColorsPictureSize = selectedColorsPictureBox.ClientSize;
      selectedColorsPictureBox.BackgroundImage = new Bitmap(
        selectedColorsPictureSize.Width, selectedColorsPictureBox.Height);
      selectedColorImageGraphics = Graphics.FromImage(
        selectedColorsPictureBox.BackgroundImage);

      // Design units are not the same as runtime units for position and
      // size. Figure out the conversion ratios knowing that the selected
      // colors picture box is 120 x 200 in design units.
      xScaleFactor = 1.0F * selectedColorsPictureBox.Width / 200;
      yScaleFactor = 1.0F * selectedColorsPictureBox.Height / 120;

      EnableMenuItems();
      RenderSelectedColorIndicator();
    }

    private DialogResult AskWhetherToSaveChanges()
    {
      string title = "Save Palette?";
      string message = "The palette has been changed. Do you wish to save " +
        "your changes before proceeding?";
      return MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
    }

    public Color BackgroundColor
    {
      get { return backgroundColor; }
      set
      {
        backgroundColor = value;
        RenderSelectedColorIndicator();
      }
    }

    private Panel BuildColorPanel(Color color, string sectionName,
      string colorName, Point position, Size size)
    {
      Panel panel = new Panel();
      panel.BackColor = color;
      panel.BorderStyle = BorderStyle.FixedSingle;
      panel.Location = position;
      panel.Name = (sectionName + colorName).Replace(" ", "").
        Replace(".", "") + "Button";
      panel.Size = size;
      panel.TabIndex = 8;
      panel.Text = string.Empty;
      panel.MouseClick += new MouseEventHandler(ColorPanel_MouseClick);
      paletteToolTip.SetToolTip(panel, colorName);

      return panel;
    }

    private Label BuildSectionLabel(string text, Point position, Size size)
    {
      Label label = new Label();
      label.BorderStyle = BorderStyle.FixedSingle;
      label.Location = position;
      label.Name = text.Replace(" ", "").Replace(".", "") + "Label";
      label.Size = size;
      label.TabIndex = 7;
      label.Text = text;
      label.MouseClick += new MouseEventHandler(PaletteSection_MouseClick);
      label.TextAlign = ContentAlignment.MiddleCenter;

      return label;
    }

    private Color ChooseAColor(Color oldColor)
    {
      Color result = oldColor;

      ColorSelectionForm dlg = new ColorSelectionForm(oldColor);
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        result = dlg.SelectedColor;
      }

      return result;
    }

    private void ColorPanel_MouseClick(object sender, MouseEventArgs e)
    {
      bool changed = false;
      Color color = (sender as Panel).BackColor;
      if (e.Button == MouseButtons.Left)
      {
        leftButtonColor = color;
        changed = true;
      }
      else if (e.Button == MouseButtons.Right)
      {
        rightButtonColor = color;
        changed = true;
      }
      if (changed)
      {
        RenderSelectedColorIndicator();
      }
    }

    private void EnableMenuItems()
    {
      bool haveAPalette = palette != null;
      bool paletteHasAFilename = haveAPalette &&
        !string.IsNullOrEmpty(palette.PaletteFile);

      paletteEditToolStripMenuItem.Enabled = haveAPalette;
      paletteSaveToolStripMenuItem.Enabled = paletteHasAFilename;
    }

    public List<string> HiddenSections
    {
      get
      {
        return palette != null ? palette.HiddenSections : new List<string>();
      }
      set
      {
        if (palette != null)
        {
          palette.HiddenSections = value;
          RebuildAdaptiveControls();
        }
      }
    }

    public Color LeftButtonColor
    {
      get { return leftButtonColor; }
      set
      {
        leftButtonColor = value;
        RenderSelectedColorIndicator();
      }
    }

    public bool OpenPalette(string paletteFilepath, string paletteFilename)
    {
      bool result = false;

      try
      {
        Palette = new Palette(paletteFilepath, paletteFilename);
        result = palette.HasBeenInitialized;
      }
      catch { }

      return result;
    }

    public Color OutlineColor
    {
      get { return outlineColor; }
      set
      {
        outlineColor = value;
        RenderSelectedColorIndicator();
      }
    }

    public Palette Palette
    {
      get { return palette; }
      set
      {
        palette = value;
        RebuildAdaptiveControls();
      }
    }

    public string PaletteFile
    {
      get { return palette != null ? palette.PaletteFile : string.Empty; }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          Palette = new Palette(value,
            Path.GetFileNameWithoutExtension(value));
        }
      }
    }

    private void PaletteSection_MouseClick(object sender, MouseEventArgs e)
    {
      string sectionName = (sender as Label).Text;
      if (e.Button == MouseButtons.Left)
      {
        PaletteSection paletteSection = palette.Section(sectionName);
        if (paletteSection != null)
        {
          paletteSection.Hidden = !paletteSection.Hidden;
          RebuildAdaptiveControls();
        }
      }
    }

    public void RebuildAdaptiveControls()
    {
      EnableMenuItems();

      // Remove all of the adaptive controls.
      foreach (Control control in adaptiveControls)
      {
        palettePanel.Controls.Remove(control);
        control.Dispose();
      }
      adaptiveControls.Clear();

      // Remove their tool tips too.
      paletteToolTip.RemoveAll();

      // Reset the height of the palette panel.
      palettePanel.Height = palettePanel.MinimumSize.Height;

      if (palette != null)
      {
        // Adjust the size of the palette panel to accommodate the palette
        // contents.
        int controlY = palettePanel.MinimumSize.Height;
        int marginX = (int)Math.Round(10 * xScaleFactor);
        int marginY = (int)Math.Round(10 * yScaleFactor);
        int buttonPadding = (int)Math.Round(5 * xScaleFactor);
        Point position = new Point(selectedColorsPictureBox.Left,
          palettePanel.MinimumSize.Height);
        Size labelSize = new Size(selectedColorsPictureBox.Width,
          (int)Math.Round(30 * yScaleFactor));
        Size buttonSize = new Size((int)Math.Round(40 * xScaleFactor),
          (int)Math.Round(40 * yScaleFactor));
        palettePanel.Height = palettePanel.MinimumSize.Height;
        foreach (PaletteSection section in palette.Sections)
        {
          // Adjust panel size and add the section label.
          position.X = (int)Math.Round(23 * xScaleFactor);
          palettePanel.Height += labelSize.Height + marginY;
          Label label = BuildSectionLabel(section.Name, position, labelSize);
          palettePanel.Controls.Add(label);
          adaptiveControls.Add(label);
          position.Y += labelSize.Height + marginY;

          if (!section.Hidden)
          {
            // Adjust panel size and add the color buttons.
            int nrButtonRows = (section.ColorCount + 3) / 4;
            palettePanel.Height +=
              nrButtonRows * (buttonSize.Height + marginY);
            Dictionary<string, Color> colors = section.Colors;
            int column = 0;
            int colorIndex = 0;
            foreach (string colorName in colors.Keys)
            {
              column = colorIndex % 4;
              position.X = (int)Math.Round(22 * xScaleFactor) +
                buttonPadding + column * (buttonSize.Width + marginX);
              Panel panel = BuildColorPanel(colors[colorName],
                section.Name, colorName, position, buttonSize);
              palettePanel.Controls.Add(panel);
              adaptiveControls.Add(panel);
              if (column == 3)
              {
                position.Y += buttonSize.Height + marginY;
              }
              colorIndex++;
            }
            if (nrButtonRows > 0 && column != 3)
            {
              position.Y += buttonSize.Height + marginY;
            }
          }
        }
      }
    }

    public void RenderSelectedColorIndicator()
    {
      // Map the colors in the source image to the selected colors.
      ImageAttributes imageAttributes = new ImageAttributes();
      ColorMap[] colorMap = new ColorMap[4];
      colorMap[0] = new ColorMap();
      colorMap[0].OldColor = selectedColorImageBackgroundColor;
      colorMap[0].NewColor = backgroundColor;
      colorMap[1] = new ColorMap();
      colorMap[1].OldColor = selectedColorImageOutlineColor;
      colorMap[1].NewColor = outlineColor;
      colorMap[2] = new ColorMap();
      colorMap[2].OldColor = selectedColorImageLeftColor;
      colorMap[2].NewColor = leftButtonColor;
      colorMap[3] = new ColorMap();
      colorMap[3].OldColor = selectedColorImageRightColor;
      colorMap[3].NewColor = rightButtonColor;
      imageAttributes.SetRemapTable(colorMap);

      // Draw the transformed source image into the picture box background.
      Rectangle destinationRectangle =
        selectedColorsPictureBox.ClientRectangle;
      selectedColorImageGraphics.DrawImage(selectedColorImage,
        destinationRectangle, 0, 0,
        selectedColorImage.Width, selectedColorImage.Height,
        GraphicsUnit.Pixel, imageAttributes);
      selectedColorsPictureBox.Invalidate();
    }

    public Color RightButtonColor
    {
      get { return rightButtonColor; }
      set
      {
        rightButtonColor = value;
        RenderSelectedColorIndicator();
      }
    }

    private void paletteEditToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PaletteEditorForm dlg = new PaletteEditorForm(palette);
      dlg.ShowDialog();
      if (palette.HasBeenChanged)
      {
        RebuildAdaptiveControls();
      }
    }

    private void PaletteForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      bool continueWithOperation = true;
      if (palette != null && palette.HasBeenChanged)
      {
        // Ask whether to save.
        DialogResult dialogResult = AskWhetherToSaveChanges();
        if (dialogResult == DialogResult.Yes)
        {
          // Save the design.
          palette.Save();
        }
        else if (dialogResult == DialogResult.Cancel)
        {
          continueWithOperation = false;
        }
      }
      // If the user clicked on the X box of the palette form, hide the form
      // instead of closing it. That way it can be shown again from the main
      // form.
      if (e.CloseReason == CloseReason.None ||
          e.CloseReason == CloseReason.UserClosing)
      {
        (parent as PaletteFormClientInterface).
          ShowPaletteForm(false);
        continueWithOperation = false;
      }
      e.Cancel = !continueWithOperation;
      if (continueWithOperation)
      {
        // The form is really closing. Remove the palette.
        palette = null;
      }
    }

    private void paletteNewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      palette = new Palette();
      paletteEditToolStripMenuItem_Click(sender, e);
    }

    private void paletteOpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog dlg = new OpenFileDialog();
      dlg.InitialDirectory = Properties.Settings.Default.PaletteDirectory;
      dlg.Filter = Properties.Settings.Default.PaletteFileFilter;
      dlg.FilterIndex = 1;
      dlg.CheckFileExists = true;
      dlg.CheckPathExists = true;
      dlg.Multiselect = false;
      dlg.Title = "Open Palette";
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        bool continueWithOperation = true;
        if (palette != null && palette.HasBeenChanged)
        {
          // Ask whether to save.
          DialogResult dialogResult = AskWhetherToSaveChanges();
          if (dialogResult == DialogResult.Yes)
          {
            // Save the design.
            palette.Save();
          }
          else if (dialogResult == DialogResult.Cancel)
          {
            continueWithOperation = false;
          }
        }
        if (continueWithOperation)
        {
          // Initialize the palette panel based on the palette contents..
          PaletteFile = dlg.FileName;
        }
      }
    }

    private void paletteSaveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (palette != null)
      {
        if (string.IsNullOrEmpty(palette.PaletteFile))
        {
          paletteSaveAsToolStripMenuItem_Click(sender, e);
        }
        else
        {
          palette.Save();
        }
      }
    }

    private void paletteSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFileDialog dlg = new SaveFileDialog();
      dlg.InitialDirectory = Properties.Settings.Default.PaletteDirectory;
      dlg.Filter = Properties.Settings.Default.PaletteFileFilter;
      dlg.FilterIndex = 1;
      dlg.Title = "Save Palette";
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        palette.PaletteFile = dlg.FileName;
        paletteSaveToolStripMenuItem_Click(sender, e);
        EnableMenuItems();
      }
    }

    private void setBackgroundColorButton_Click(object sender, EventArgs e)
    {
      backgroundColor = ChooseAColor(backgroundColor);
      RenderSelectedColorIndicator();
      (parent as PaletteFormClientInterface).
        BackgroundColorHasChanged(backgroundColor);
    }

    private void setOutlineColorButton_Click(object sender, EventArgs e)
    {
      outlineColor = ChooseAColor(outlineColor);
      RenderSelectedColorIndicator();
      (parent as PaletteFormClientInterface).
        OutlineColorHasChanged(outlineColor);
    }

    public bool TryClose()
    {
      bool continueWithOperation = true;
      if (palette != null && palette.HasBeenChanged)
      {
        // Ask whether to save.
        DialogResult dialogResult = AskWhetherToSaveChanges();
        if (dialogResult == DialogResult.Yes)
        {
          // Save the design.
          palette.Save();
        }
        else if (dialogResult == DialogResult.Cancel)
        {
          continueWithOperation = false;
        }
        else if (dialogResult == DialogResult.No)
        {
          // Clear the changed flag so that close can occur without
          // interruption.
          palette.HasBeenChanged = false;
        }
      }
      if (continueWithOperation)
      {
        // Close the form
        Close();
      }

      return continueWithOperation;
    }

    // The window does not honor the MaximumSize property.
    // By intercepting a few messages, correct behavior can be attained.
    // Thanks to Zach Johnson, "Overcome OS Imposed Windows Form Minimum Size
    // Limit".
    private const int MW_WINDOWPOSCHANGING = 0x0046;
    private const int WM_GETMINMAXINFO = 0x0024;
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == MW_WINDOWPOSCHANGING)
      {
        WindowPos windowPos = (WindowPos)m.GetLParam(typeof(WindowPos));

        // Make changes.
        windowPos.width = Math.Max(MinimumSize.Width,
          Math.Min(windowPos.width, MaximumSize.Width));
        windowPos.height = Math.Max(MinimumSize.Height,
          Math.Min(windowPos.height, MaximumSize.Height));

        // Marshal the changes back to the message.
        Marshal.StructureToPtr(windowPos, m.LParam, true);
      }

      // Do the usual processing.
      base.WndProc(ref m);

      // Make changes to WM_GETMINMAXINFO after it has been handled by the underlying
      // WndProc, so we only need to repopulate the minimum size constraints
      if (m.Msg == WM_GETMINMAXINFO)
      {
        MinMaxInfo minMaxInfo = (MinMaxInfo)m.GetLParam(typeof(MinMaxInfo));
        minMaxInfo.ptMinTrackSize.x = MinimumSize.Width;
        minMaxInfo.ptMinTrackSize.y = MinimumSize.Height;
        minMaxInfo.ptMaxTrackSize.x = MaximumSize.Width;
        minMaxInfo.ptMaxTrackSize.y = MaximumSize.Height;

        // Marshal the changes back to the message.
        Marshal.StructureToPtr(minMaxInfo, m.LParam, true);
      }
    }

    struct WindowPos
    {
      public IntPtr hwnd;
      public IntPtr hwndInsertAfter;
      public int x;
      public int y;
      public int width;
      public int height;
      public uint flags;
    }

    struct POINT
    {
      public int x;
      public int y;
    }

    struct MinMaxInfo
    {
      public POINT ptReserved;
      public POINT ptMaxSize;
      public POINT ptMaxPosition;
      public POINT ptMinTrackSize;
      public POINT ptMaxTrackSize;
    }

  }
}
