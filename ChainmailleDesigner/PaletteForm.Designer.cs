// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: PaletteForm.Designer.cs


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
  partial class PaletteForm
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
        if (selectedColorImageGraphics != null)
        {
          selectedColorImageGraphics.Dispose();
        }
        if (selectedColorImage != null)
        {
          selectedColorImage.Dispose();
        }
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteForm));
      this.paletteMenuStrip = new System.Windows.Forms.MenuStrip();
      this.paletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.paletteNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.paletteOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.paletteEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.paletteSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.paletteSaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.paletteToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.setBackgroundColorButton = new System.Windows.Forms.Button();
      this.setOutlineColorButton = new System.Windows.Forms.Button();
      this.selectedColorsPictureBox = new System.Windows.Forms.PictureBox();
      this.scrollingPanel = new System.Windows.Forms.Panel();
      this.palettePanel = new System.Windows.Forms.Panel();
      this.paletteMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.selectedColorsPictureBox)).BeginInit();
      this.scrollingPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // paletteMenuStrip
      // 
      this.paletteMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.paletteMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paletteToolStripMenuItem});
      this.paletteMenuStrip.Location = new System.Drawing.Point(0, 0);
      this.paletteMenuStrip.Name = "paletteMenuStrip";
      this.paletteMenuStrip.Size = new System.Drawing.Size(294, 40);
      this.paletteMenuStrip.TabIndex = 6;
      this.paletteMenuStrip.Text = "menuStrip1";
      // 
      // paletteToolStripMenuItem
      // 
      this.paletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paletteNewToolStripMenuItem,
            this.paletteOpenToolStripMenuItem,
            this.toolStripMenuItem1,
            this.paletteEditToolStripMenuItem,
            this.toolStripMenuItem2,
            this.paletteSaveToolStripMenuItem,
            this.paletteSaveAsToolStripMenuItem});
      this.paletteToolStripMenuItem.Name = "paletteToolStripMenuItem";
      this.paletteToolStripMenuItem.Size = new System.Drawing.Size(99, 36);
      this.paletteToolStripMenuItem.Text = "Palette";
      // 
      // paletteNewToolStripMenuItem
      // 
      this.paletteNewToolStripMenuItem.Name = "paletteNewToolStripMenuItem";
      this.paletteNewToolStripMenuItem.Size = new System.Drawing.Size(211, 38);
      this.paletteNewToolStripMenuItem.Text = "New...";
      this.paletteNewToolStripMenuItem.Click += new System.EventHandler(this.paletteNewToolStripMenuItem_Click);
      // 
      // paletteOpenToolStripMenuItem
      // 
      this.paletteOpenToolStripMenuItem.Name = "paletteOpenToolStripMenuItem";
      this.paletteOpenToolStripMenuItem.Size = new System.Drawing.Size(211, 38);
      this.paletteOpenToolStripMenuItem.Text = "Open...";
      this.paletteOpenToolStripMenuItem.Click += new System.EventHandler(this.paletteOpenToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(208, 6);
      // 
      // paletteEditToolStripMenuItem
      // 
      this.paletteEditToolStripMenuItem.Name = "paletteEditToolStripMenuItem";
      this.paletteEditToolStripMenuItem.Size = new System.Drawing.Size(211, 38);
      this.paletteEditToolStripMenuItem.Text = "Edit...";
      this.paletteEditToolStripMenuItem.Click += new System.EventHandler(this.paletteEditToolStripMenuItem_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(208, 6);
      // 
      // paletteSaveToolStripMenuItem
      // 
      this.paletteSaveToolStripMenuItem.Name = "paletteSaveToolStripMenuItem";
      this.paletteSaveToolStripMenuItem.Size = new System.Drawing.Size(211, 38);
      this.paletteSaveToolStripMenuItem.Text = "Save";
      this.paletteSaveToolStripMenuItem.Click += new System.EventHandler(this.paletteSaveToolStripMenuItem_Click);
      // 
      // paletteSaveAsToolStripMenuItem
      // 
      this.paletteSaveAsToolStripMenuItem.Name = "paletteSaveAsToolStripMenuItem";
      this.paletteSaveAsToolStripMenuItem.Size = new System.Drawing.Size(211, 38);
      this.paletteSaveAsToolStripMenuItem.Text = "Save As...";
      this.paletteSaveAsToolStripMenuItem.Click += new System.EventHandler(this.paletteSaveAsToolStripMenuItem_Click);
      // 
      // setBackgroundColorButton
      // 
      this.setBackgroundColorButton.Location = new System.Drawing.Point(30, 240);
      this.setBackgroundColorButton.Name = "setBackgroundColorButton";
      this.setBackgroundColorButton.Size = new System.Drawing.Size(200, 45);
      this.setBackgroundColorButton.TabIndex = 9;
      this.setBackgroundColorButton.Text = "Set Background";
      this.setBackgroundColorButton.UseVisualStyleBackColor = true;
      this.setBackgroundColorButton.Click += new System.EventHandler(this.setBackgroundColorButton_Click);
      // 
      // setOutlineColorButton
      // 
      this.setOutlineColorButton.Location = new System.Drawing.Point(30, 190);
      this.setOutlineColorButton.Name = "setOutlineColorButton";
      this.setOutlineColorButton.Size = new System.Drawing.Size(200, 45);
      this.setOutlineColorButton.TabIndex = 8;
      this.setOutlineColorButton.Text = "Set Outline Color";
      this.setOutlineColorButton.UseVisualStyleBackColor = true;
      this.setOutlineColorButton.Click += new System.EventHandler(this.setOutlineColorButton_Click);
      // 
      // selectedColorsPictureBox
      // 
      this.selectedColorsPictureBox.Location = new System.Drawing.Point(30, 50);
      this.selectedColorsPictureBox.Name = "selectedColorsPictureBox";
      this.selectedColorsPictureBox.Size = new System.Drawing.Size(200, 120);
      this.selectedColorsPictureBox.TabIndex = 7;
      this.selectedColorsPictureBox.TabStop = false;
      // 
      // scrollingPanel
      // 
      this.scrollingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.scrollingPanel.AutoScroll = true;
      this.scrollingPanel.Controls.Add(this.palettePanel);
      this.scrollingPanel.Location = new System.Drawing.Point(5, 290);
      this.scrollingPanel.Name = "scrollingPanel";
      this.scrollingPanel.Size = new System.Drawing.Size(280, 505);
      this.scrollingPanel.TabIndex = 10;
      // 
      // palettePanel
      // 
      this.palettePanel.Location = new System.Drawing.Point(5, 10);
      this.palettePanel.MinimumSize = new System.Drawing.Size(240, 5);
      this.palettePanel.Name = "palettePanel";
      this.palettePanel.Size = new System.Drawing.Size(240, 270);
      this.palettePanel.TabIndex = 6;
      // 
      // PaletteForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(294, 804);
      this.Controls.Add(this.scrollingPanel);
      this.Controls.Add(this.setBackgroundColorButton);
      this.Controls.Add(this.setOutlineColorButton);
      this.Controls.Add(this.selectedColorsPictureBox);
      this.Controls.Add(this.paletteMenuStrip);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.paletteMenuStrip;
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(320, 2000);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(320, 425);
      this.Name = "PaletteForm";
      this.Text = "Palette";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PaletteForm_FormClosing);
      this.paletteMenuStrip.ResumeLayout(false);
      this.paletteMenuStrip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.selectedColorsPictureBox)).EndInit();
      this.scrollingPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.MenuStrip paletteMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem paletteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem paletteEditToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem paletteOpenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem paletteSaveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem paletteSaveAsToolStripMenuItem;
    private System.Windows.Forms.ToolTip paletteToolTip;
    private System.Windows.Forms.ToolStripMenuItem paletteNewToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.Button setBackgroundColorButton;
    private System.Windows.Forms.Button setOutlineColorButton;
    private System.Windows.Forms.PictureBox selectedColorsPictureBox;
    private System.Windows.Forms.Panel scrollingPanel;
    private System.Windows.Forms.Panel palettePanel;
  }
}