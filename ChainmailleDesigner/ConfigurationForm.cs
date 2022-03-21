// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ConfigurationForm.cs


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
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class ConfigurationForm : Form
  {
    public ConfigurationForm()
    {
      InitializeComponent();

      designDirectoryTextBox.Text =
        Properties.Settings.Default.DesignDirectory;
      weaveDirectoryTextBox.Text =
        Properties.Settings.Default.PatternDirectory;
      paletteDirectoryTextBox.Text =
        Properties.Settings.Default.PaletteDirectory;
      designerTextBox.Text =
        Properties.Settings.Default.DesignerName;
      historyLimitUpDown.Value = Properties.Settings.Default.HistoryLimit;
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.DesignDirectory =
        designDirectoryTextBox.Text;
      Properties.Settings.Default.PatternDirectory =
        weaveDirectoryTextBox.Text;
      Properties.Settings.Default.PaletteDirectory =
        paletteDirectoryTextBox.Text;
      Properties.Settings.Default.DesignerName =
        designerTextBox.Text;
            Properties.Settings.Default.HistoryLimit = (int)historyLimitUpDown.Value;
      Properties.Settings.Default.Save();
    }

    private void designDirectoryButton_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();
      dlg.Description = "Designate the directory where your designs are (or " +
        "where you want them to be).";
      dlg.ShowNewFolderButton = true;
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        designDirectoryTextBox.Text = dlg.SelectedPath;
      }
    }

    private void paletteDirectoryButton_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();
      dlg.Description = "Designate the directory where your palettes are " +
        "(or where you want them to be).";
      dlg.ShowNewFolderButton = true;
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        paletteDirectoryTextBox.Text = dlg.SelectedPath;
      }
    }

    private void weaveDirectoryButton_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog dlg = new FolderBrowserDialog();
      dlg.Description = "Designate the directory where your weave patterns " +
        "are (or where you want them to be).";
      dlg.ShowNewFolderButton = true;
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        weaveDirectoryTextBox.Text = dlg.SelectedPath;
      }
    }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
