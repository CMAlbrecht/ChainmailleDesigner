// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ConfigurationForm.Designer.cs


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
  partial class ConfigurationForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
      this.cancelbutton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.directoryGroupBox = new System.Windows.Forms.GroupBox();
      this.paletteDirectoryButton = new System.Windows.Forms.Button();
      this.weaveDirectoryButton = new System.Windows.Forms.Button();
      this.designDirectoryButton = new System.Windows.Forms.Button();
      this.paletteDirectoryTextBox = new System.Windows.Forms.TextBox();
      this.designDirectoryTextBox = new System.Windows.Forms.TextBox();
      this.weaveDirectoryTextBox = new System.Windows.Forms.TextBox();
      this.paletteDirectoryLabel = new System.Windows.Forms.Label();
      this.weaveDirectoryLabel = new System.Windows.Forms.Label();
      this.designDirectoryLabel = new System.Windows.Forms.Label();
      this.designGroupBox = new System.Windows.Forms.GroupBox();
      this.designerTextBox = new System.Windows.Forms.TextBox();
      this.designerLabel = new System.Windows.Forms.Label();
      this.directoryGroupBox.SuspendLayout();
      this.designGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // cancelbutton
      // 
      this.cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelbutton.Location = new System.Drawing.Point(1220, 85);
      this.cancelbutton.Name = "cancelbutton";
      this.cancelbutton.Size = new System.Drawing.Size(150, 55);
      this.cancelbutton.TabIndex = 14;
      this.cancelbutton.Text = "Cancel";
      this.cancelbutton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(1220, 15);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(150, 55);
      this.okButton.TabIndex = 13;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // directoryGroupBox
      // 
      this.directoryGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.directoryGroupBox.Controls.Add(this.paletteDirectoryButton);
      this.directoryGroupBox.Controls.Add(this.weaveDirectoryButton);
      this.directoryGroupBox.Controls.Add(this.designDirectoryButton);
      this.directoryGroupBox.Controls.Add(this.paletteDirectoryTextBox);
      this.directoryGroupBox.Controls.Add(this.designDirectoryTextBox);
      this.directoryGroupBox.Controls.Add(this.weaveDirectoryTextBox);
      this.directoryGroupBox.Controls.Add(this.paletteDirectoryLabel);
      this.directoryGroupBox.Controls.Add(this.weaveDirectoryLabel);
      this.directoryGroupBox.Controls.Add(this.designDirectoryLabel);
      this.directoryGroupBox.Location = new System.Drawing.Point(15, 16);
      this.directoryGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.directoryGroupBox.Name = "directoryGroupBox";
      this.directoryGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.directoryGroupBox.Size = new System.Drawing.Size(1180, 250);
      this.directoryGroupBox.TabIndex = 15;
      this.directoryGroupBox.TabStop = false;
      this.directoryGroupBox.Text = "Configured Directories";
      // 
      // paletteDirectoryButton
      // 
      this.paletteDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.paletteDirectoryButton.Location = new System.Drawing.Point(1090, 164);
      this.paletteDirectoryButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.paletteDirectoryButton.Name = "paletteDirectoryButton";
      this.paletteDirectoryButton.Size = new System.Drawing.Size(75, 47);
      this.paletteDirectoryButton.TabIndex = 8;
      this.paletteDirectoryButton.Text = "...";
      this.paletteDirectoryButton.UseVisualStyleBackColor = true;
      this.paletteDirectoryButton.Click += new System.EventHandler(this.paletteDirectoryButton_Click);
      // 
      // weaveDirectoryButton
      // 
      this.weaveDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.weaveDirectoryButton.Location = new System.Drawing.Point(1090, 102);
      this.weaveDirectoryButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.weaveDirectoryButton.Name = "weaveDirectoryButton";
      this.weaveDirectoryButton.Size = new System.Drawing.Size(75, 47);
      this.weaveDirectoryButton.TabIndex = 7;
      this.weaveDirectoryButton.Text = "...";
      this.weaveDirectoryButton.UseVisualStyleBackColor = true;
      this.weaveDirectoryButton.Click += new System.EventHandler(this.weaveDirectoryButton_Click);
      // 
      // designDirectoryButton
      // 
      this.designDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.designDirectoryButton.Location = new System.Drawing.Point(1090, 39);
      this.designDirectoryButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.designDirectoryButton.Name = "designDirectoryButton";
      this.designDirectoryButton.Size = new System.Drawing.Size(75, 47);
      this.designDirectoryButton.TabIndex = 6;
      this.designDirectoryButton.Text = "...";
      this.designDirectoryButton.UseVisualStyleBackColor = true;
      this.designDirectoryButton.Click += new System.EventHandler(this.designDirectoryButton_Click);
      // 
      // paletteDirectoryTextBox
      // 
      this.paletteDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.paletteDirectoryTextBox.Location = new System.Drawing.Point(270, 172);
      this.paletteDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.paletteDirectoryTextBox.Name = "paletteDirectoryTextBox";
      this.paletteDirectoryTextBox.Size = new System.Drawing.Size(800, 31);
      this.paletteDirectoryTextBox.TabIndex = 5;
      // 
      // designDirectoryTextBox
      // 
      this.designDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.designDirectoryTextBox.Location = new System.Drawing.Point(270, 47);
      this.designDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.designDirectoryTextBox.Name = "designDirectoryTextBox";
      this.designDirectoryTextBox.Size = new System.Drawing.Size(800, 31);
      this.designDirectoryTextBox.TabIndex = 4;
      // 
      // weaveDirectoryTextBox
      // 
      this.weaveDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.weaveDirectoryTextBox.Location = new System.Drawing.Point(270, 109);
      this.weaveDirectoryTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.weaveDirectoryTextBox.Name = "weaveDirectoryTextBox";
      this.weaveDirectoryTextBox.Size = new System.Drawing.Size(800, 31);
      this.weaveDirectoryTextBox.TabIndex = 3;
      // 
      // paletteDirectoryLabel
      // 
      this.paletteDirectoryLabel.AutoSize = true;
      this.paletteDirectoryLabel.Location = new System.Drawing.Point(45, 172);
      this.paletteDirectoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.paletteDirectoryLabel.Name = "paletteDirectoryLabel";
      this.paletteDirectoryLabel.Size = new System.Drawing.Size(137, 25);
      this.paletteDirectoryLabel.TabIndex = 2;
      this.paletteDirectoryLabel.Text = "Palette Files:";
      // 
      // weaveDirectoryLabel
      // 
      this.weaveDirectoryLabel.AutoSize = true;
      this.weaveDirectoryLabel.Location = new System.Drawing.Point(45, 109);
      this.weaveDirectoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.weaveDirectoryLabel.Name = "weaveDirectoryLabel";
      this.weaveDirectoryLabel.Size = new System.Drawing.Size(212, 25);
      this.weaveDirectoryLabel.TabIndex = 1;
      this.weaveDirectoryLabel.Text = "Weave Pattern Files:";
      // 
      // designDirectoryLabel
      // 
      this.designDirectoryLabel.AutoSize = true;
      this.designDirectoryLabel.Location = new System.Drawing.Point(45, 47);
      this.designDirectoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.designDirectoryLabel.Name = "designDirectoryLabel";
      this.designDirectoryLabel.Size = new System.Drawing.Size(137, 25);
      this.designDirectoryLabel.TabIndex = 0;
      this.designDirectoryLabel.Text = "Design Files:";
      // 
      // designGroupBox
      // 
      this.designGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.designGroupBox.Controls.Add(this.designerTextBox);
      this.designGroupBox.Controls.Add(this.designerLabel);
      this.designGroupBox.Location = new System.Drawing.Point(15, 281);
      this.designGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.designGroupBox.Name = "designGroupBox";
      this.designGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.designGroupBox.Size = new System.Drawing.Size(1180, 125);
      this.designGroupBox.TabIndex = 16;
      this.designGroupBox.TabStop = false;
      this.designGroupBox.Text = "Design Information";
      // 
      // designerTextBox
      // 
      this.designerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.designerTextBox.Location = new System.Drawing.Point(270, 47);
      this.designerTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.designerTextBox.Name = "designerTextBox";
      this.designerTextBox.Size = new System.Drawing.Size(800, 31);
      this.designerTextBox.TabIndex = 1;
      // 
      // designerLabel
      // 
      this.designerLabel.AutoSize = true;
      this.designerLabel.Location = new System.Drawing.Point(45, 47);
      this.designerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.designerLabel.Name = "designerLabel";
      this.designerLabel.Size = new System.Drawing.Size(140, 25);
      this.designerLabel.TabIndex = 0;
      this.designerLabel.Text = "Designed By:";
      // 
      // ConfigurationForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1394, 429);
      this.Controls.Add(this.designGroupBox);
      this.Controls.Add(this.directoryGroupBox);
      this.Controls.Add(this.cancelbutton);
      this.Controls.Add(this.okButton);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "ConfigurationForm";
      this.Text = "Configuration";
      this.directoryGroupBox.ResumeLayout(false);
      this.directoryGroupBox.PerformLayout();
      this.designGroupBox.ResumeLayout(false);
      this.designGroupBox.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cancelbutton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.GroupBox directoryGroupBox;
    private System.Windows.Forms.Button paletteDirectoryButton;
    private System.Windows.Forms.Button weaveDirectoryButton;
    private System.Windows.Forms.Button designDirectoryButton;
    private System.Windows.Forms.TextBox paletteDirectoryTextBox;
    private System.Windows.Forms.TextBox designDirectoryTextBox;
    private System.Windows.Forms.TextBox weaveDirectoryTextBox;
    private System.Windows.Forms.Label paletteDirectoryLabel;
    private System.Windows.Forms.Label weaveDirectoryLabel;
    private System.Windows.Forms.Label designDirectoryLabel;
    private System.Windows.Forms.GroupBox designGroupBox;
    private System.Windows.Forms.TextBox designerTextBox;
    private System.Windows.Forms.Label designerLabel;
  }
}