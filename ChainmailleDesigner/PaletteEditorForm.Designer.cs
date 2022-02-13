// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: PaletteEditorForm.Designer.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


namespace ChainmailleDesigner
{
  partial class PaletteEditorForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteEditorForm));
      this.palettePanel = new System.Windows.Forms.Panel();
      this.paletteDescriptionTextBox = new System.Windows.Forms.TextBox();
      this.paletteDescriptionLabel = new System.Windows.Forms.Label();
      this.paletteNameTextBox = new System.Windows.Forms.TextBox();
      this.paletteNameLabel = new System.Windows.Forms.Label();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.paletteSectionTabControl = new System.Windows.Forms.TabControl();
      this.palettePanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // palettePanel
      // 
      this.palettePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.palettePanel.Controls.Add(this.paletteDescriptionTextBox);
      this.palettePanel.Controls.Add(this.paletteDescriptionLabel);
      this.palettePanel.Controls.Add(this.paletteNameTextBox);
      this.palettePanel.Controls.Add(this.paletteNameLabel);
      this.palettePanel.Controls.Add(this.cancelButton);
      this.palettePanel.Controls.Add(this.okButton);
      this.palettePanel.Location = new System.Drawing.Point(10, 10);
      this.palettePanel.Name = "palettePanel";
      this.palettePanel.Size = new System.Drawing.Size(1120, 130);
      this.palettePanel.TabIndex = 0;
      // 
      // paletteDescriptionTextBox
      // 
      this.paletteDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.paletteDescriptionTextBox.Location = new System.Drawing.Point(150, 45);
      this.paletteDescriptionTextBox.Multiline = true;
      this.paletteDescriptionTextBox.Name = "paletteDescriptionTextBox";
      this.paletteDescriptionTextBox.Size = new System.Drawing.Size(740, 75);
      this.paletteDescriptionTextBox.TabIndex = 5;
      this.paletteDescriptionTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
      // 
      // paletteDescriptionLabel
      // 
      this.paletteDescriptionLabel.AutoSize = true;
      this.paletteDescriptionLabel.Location = new System.Drawing.Point(10, 50);
      this.paletteDescriptionLabel.Name = "paletteDescriptionLabel";
      this.paletteDescriptionLabel.Size = new System.Drawing.Size(126, 25);
      this.paletteDescriptionLabel.TabIndex = 4;
      this.paletteDescriptionLabel.Text = "Description:";
      // 
      // paletteNameTextBox
      // 
      this.paletteNameTextBox.Location = new System.Drawing.Point(150, 0);
      this.paletteNameTextBox.Name = "paletteNameTextBox";
      this.paletteNameTextBox.Size = new System.Drawing.Size(360, 31);
      this.paletteNameTextBox.TabIndex = 3;
      this.paletteNameTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
      // 
      // paletteNameLabel
      // 
      this.paletteNameLabel.AutoSize = true;
      this.paletteNameLabel.Location = new System.Drawing.Point(5, 5);
      this.paletteNameLabel.Margin = new System.Windows.Forms.Padding(0);
      this.paletteNameLabel.Name = "paletteNameLabel";
      this.paletteNameLabel.Size = new System.Drawing.Size(74, 25);
      this.paletteNameLabel.TabIndex = 2;
      this.paletteNameLabel.Text = "Name:";
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(920, 75);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(200, 50);
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(920, 0);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(200, 50);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // paletteSectionTabControl
      // 
      this.paletteSectionTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.paletteSectionTabControl.Location = new System.Drawing.Point(10, 150);
      this.paletteSectionTabControl.Multiline = true;
      this.paletteSectionTabControl.Name = "paletteSectionTabControl";
      this.paletteSectionTabControl.SelectedIndex = 0;
      this.paletteSectionTabControl.Size = new System.Drawing.Size(1120, 510);
      this.paletteSectionTabControl.TabIndex = 1;
      this.paletteSectionTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.SectionTabSelected);
      // 
      // PaletteEditorForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1149, 669);
      this.Controls.Add(this.paletteSectionTabControl);
      this.Controls.Add(this.palettePanel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "PaletteEditorForm";
      this.Text = "Edit Palette";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PaletteEditorForm_FormClosing);
      this.palettePanel.ResumeLayout(false);
      this.palettePanel.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel palettePanel;
    private System.Windows.Forms.TextBox paletteDescriptionTextBox;
    private System.Windows.Forms.Label paletteDescriptionLabel;
    private System.Windows.Forms.TextBox paletteNameTextBox;
    private System.Windows.Forms.Label paletteNameLabel;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.TabControl paletteSectionTabControl;
  }
}