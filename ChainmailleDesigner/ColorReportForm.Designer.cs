// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorReportForm.Designer.cs


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
  partial class ColorReportForm
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
        reportFont.Dispose();
        textBrush.Dispose();
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorReportForm));
      this.colorReportPanel = new System.Windows.Forms.Panel();
      this.colorReportMenuStrip = new System.Windows.Forms.MenuStrip();
      this.colorReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.colorReportPrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.colorReportSaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.colorReportMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // colorReportPanel
      // 
      this.colorReportPanel.Location = new System.Drawing.Point(12, 57);
      this.colorReportPanel.Name = "colorReportPanel";
      this.colorReportPanel.Size = new System.Drawing.Size(276, 251);
      this.colorReportPanel.TabIndex = 0;
      // 
      // colorReportMenuStrip
      // 
      this.colorReportMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.colorReportMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorReportToolStripMenuItem});
      this.colorReportMenuStrip.Location = new System.Drawing.Point(0, 0);
      this.colorReportMenuStrip.Name = "colorReportMenuStrip";
      this.colorReportMenuStrip.Size = new System.Drawing.Size(308, 40);
      this.colorReportMenuStrip.TabIndex = 1;
      this.colorReportMenuStrip.Text = "menuStrip1";
      // 
      // colorReportToolStripMenuItem
      // 
      this.colorReportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorReportPrintToolStripMenuItem,
            this.colorReportSaveAsToolStripMenuItem});
      this.colorReportToolStripMenuItem.Name = "colorReportToolStripMenuItem";
      this.colorReportToolStripMenuItem.Size = new System.Drawing.Size(233, 36);
      this.colorReportToolStripMenuItem.Text = "Color Count Report";
      // 
      // colorReportPrintToolStripMenuItem
      // 
      this.colorReportPrintToolStripMenuItem.Name = "colorReportPrintToolStripMenuItem";
      this.colorReportPrintToolStripMenuItem.Size = new System.Drawing.Size(211, 38);
      this.colorReportPrintToolStripMenuItem.Text = "Print";
      this.colorReportPrintToolStripMenuItem.Click += new System.EventHandler(this.colorReportPrintToolStripMenuItem_Click);
      // 
      // colorReportSaveAsToolStripMenuItem
      // 
      this.colorReportSaveAsToolStripMenuItem.Name = "colorReportSaveAsToolStripMenuItem";
      this.colorReportSaveAsToolStripMenuItem.Size = new System.Drawing.Size(211, 38);
      this.colorReportSaveAsToolStripMenuItem.Text = "Save As...";
      this.colorReportSaveAsToolStripMenuItem.Click += new System.EventHandler(this.colorReportSaveAsToolStripMenuItem_Click);
      // 
      // ColorReportForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(308, 534);
      this.Controls.Add(this.colorReportPanel);
      this.Controls.Add(this.colorReportMenuStrip);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.colorReportMenuStrip;
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(1000, 2000);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(100, 100);
      this.Name = "ColorReportForm";
      this.Text = "Color Counts";
      this.colorReportMenuStrip.ResumeLayout(false);
      this.colorReportMenuStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel colorReportPanel;
    private System.Windows.Forms.MenuStrip colorReportMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem colorReportToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem colorReportPrintToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem colorReportSaveAsToolStripMenuItem;
  }
}