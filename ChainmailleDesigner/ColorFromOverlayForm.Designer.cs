﻿// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorFromOverlayForm.Designer.cs


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
  partial class ColorFromOverlayForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorFromOverlayForm));
      this.cancelbutton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.previewgroupBox = new System.Windows.Forms.GroupBox();
      this.previewPanel = new System.Windows.Forms.Panel();
      this.ringFilterGroupBox = new System.Windows.Forms.GroupBox();
      this.colorSelectionGroupBox = new System.Windows.Forms.GroupBox();
      this.colorSelectionPanel = new System.Windows.Forms.Panel();
      this.paletteSectionLabel = new System.Windows.Forms.Label();
      this.paletteSectionComboBox = new System.Windows.Forms.ComboBox();
      this.colorMatchingGroupBox = new System.Windows.Forms.GroupBox();
      this.bScaleTrackBar = new System.Windows.Forms.TrackBar();
      this.bStripPanel = new System.Windows.Forms.Panel();
      this.bCheckBox = new System.Windows.Forms.CheckBox();
      this.bOffsetTrackBar = new System.Windows.Forms.TrackBar();
      this.aScaleTrackBar = new System.Windows.Forms.TrackBar();
      this.aStripPanel = new System.Windows.Forms.Panel();
      this.aCheckBox = new System.Windows.Forms.CheckBox();
      this.aOffsetTrackBar = new System.Windows.Forms.TrackBar();
      this.lScaleTrackBar = new System.Windows.Forms.TrackBar();
      this.lStripPanel = new System.Windows.Forms.Panel();
      this.lCheckBox = new System.Windows.Forms.CheckBox();
      this.lOffsetTrackBar = new System.Windows.Forms.TrackBar();
      this.shapeProgressBar = new System.Windows.Forms.ProgressBar();
      this.previewgroupBox.SuspendLayout();
      this.colorSelectionGroupBox.SuspendLayout();
      this.colorSelectionPanel.SuspendLayout();
      this.colorMatchingGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bScaleTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bOffsetTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.aScaleTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.aOffsetTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lScaleTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lOffsetTrackBar)).BeginInit();
      this.SuspendLayout();
      // 
      // cancelbutton
      // 
      this.cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelbutton.Location = new System.Drawing.Point(532, 43);
      this.cancelbutton.Margin = new System.Windows.Forms.Padding(2);
      this.cancelbutton.Name = "cancelbutton";
      this.cancelbutton.Size = new System.Drawing.Size(79, 26);
      this.cancelbutton.TabIndex = 16;
      this.cancelbutton.Text = "Cancel";
      this.cancelbutton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(532, 10);
      this.okButton.Margin = new System.Windows.Forms.Padding(2);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(79, 26);
      this.okButton.TabIndex = 15;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // previewgroupBox
      // 
      this.previewgroupBox.Controls.Add(this.previewPanel);
      this.previewgroupBox.Location = new System.Drawing.Point(210, 8);
      this.previewgroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.previewgroupBox.Name = "previewgroupBox";
      this.previewgroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.previewgroupBox.Size = new System.Drawing.Size(308, 337);
      this.previewgroupBox.TabIndex = 22;
      this.previewgroupBox.TabStop = false;
      this.previewgroupBox.Text = "Preview";
      // 
      // previewPanel
      // 
      this.previewPanel.Location = new System.Drawing.Point(4, 12);
      this.previewPanel.Margin = new System.Windows.Forms.Padding(2);
      this.previewPanel.Name = "previewPanel";
      this.previewPanel.Size = new System.Drawing.Size(300, 320);
      this.previewPanel.TabIndex = 1;
      // 
      // ringFilterGroupBox
      // 
      this.ringFilterGroupBox.Location = new System.Drawing.Point(531, 93);
      this.ringFilterGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.ringFilterGroupBox.Name = "ringFilterGroupBox";
      this.ringFilterGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.ringFilterGroupBox.Size = new System.Drawing.Size(79, 145);
      this.ringFilterGroupBox.TabIndex = 23;
      this.ringFilterGroupBox.TabStop = false;
      this.ringFilterGroupBox.Text = "Affect Rings";
      // 
      // colorSelectionGroupBox
      // 
      this.colorSelectionGroupBox.Controls.Add(this.colorSelectionPanel);
      this.colorSelectionGroupBox.Location = new System.Drawing.Point(8, 8);
      this.colorSelectionGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.colorSelectionGroupBox.Name = "colorSelectionGroupBox";
      this.colorSelectionGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.colorSelectionGroupBox.Size = new System.Drawing.Size(188, 89);
      this.colorSelectionGroupBox.TabIndex = 24;
      this.colorSelectionGroupBox.TabStop = false;
      this.colorSelectionGroupBox.Text = "Color Selection";
      // 
      // colorSelectionPanel
      // 
      this.colorSelectionPanel.Controls.Add(this.paletteSectionLabel);
      this.colorSelectionPanel.Controls.Add(this.paletteSectionComboBox);
      this.colorSelectionPanel.Location = new System.Drawing.Point(5, 15);
      this.colorSelectionPanel.Margin = new System.Windows.Forms.Padding(2);
      this.colorSelectionPanel.Name = "colorSelectionPanel";
      this.colorSelectionPanel.Size = new System.Drawing.Size(180, 61);
      this.colorSelectionPanel.TabIndex = 2;
      // 
      // paletteSectionLabel
      // 
      this.paletteSectionLabel.AutoSize = true;
      this.paletteSectionLabel.Location = new System.Drawing.Point(8, 5);
      this.paletteSectionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.paletteSectionLabel.Name = "paletteSectionLabel";
      this.paletteSectionLabel.Size = new System.Drawing.Size(79, 13);
      this.paletteSectionLabel.TabIndex = 1;
      this.paletteSectionLabel.Text = "Palette Section";
      // 
      // paletteSectionComboBox
      // 
      this.paletteSectionComboBox.FormattingEnabled = true;
      this.paletteSectionComboBox.Location = new System.Drawing.Point(8, 21);
      this.paletteSectionComboBox.Margin = new System.Windows.Forms.Padding(2);
      this.paletteSectionComboBox.Name = "paletteSectionComboBox";
      this.paletteSectionComboBox.Size = new System.Drawing.Size(166, 21);
      this.paletteSectionComboBox.TabIndex = 0;
      this.paletteSectionComboBox.SelectedIndexChanged += new System.EventHandler(this.paletteSectionComboBox_SelectedIndexChanged);
      // 
      // colorMatchingGroupBox
      // 
      this.colorMatchingGroupBox.Controls.Add(this.bScaleTrackBar);
      this.colorMatchingGroupBox.Controls.Add(this.bStripPanel);
      this.colorMatchingGroupBox.Controls.Add(this.bCheckBox);
      this.colorMatchingGroupBox.Controls.Add(this.bOffsetTrackBar);
      this.colorMatchingGroupBox.Controls.Add(this.aScaleTrackBar);
      this.colorMatchingGroupBox.Controls.Add(this.aStripPanel);
      this.colorMatchingGroupBox.Controls.Add(this.aCheckBox);
      this.colorMatchingGroupBox.Controls.Add(this.aOffsetTrackBar);
      this.colorMatchingGroupBox.Controls.Add(this.lScaleTrackBar);
      this.colorMatchingGroupBox.Controls.Add(this.lStripPanel);
      this.colorMatchingGroupBox.Controls.Add(this.lCheckBox);
      this.colorMatchingGroupBox.Controls.Add(this.lOffsetTrackBar);
      this.colorMatchingGroupBox.Location = new System.Drawing.Point(8, 106);
      this.colorMatchingGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.colorMatchingGroupBox.Name = "colorMatchingGroupBox";
      this.colorMatchingGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.colorMatchingGroupBox.Size = new System.Drawing.Size(188, 240);
      this.colorMatchingGroupBox.TabIndex = 25;
      this.colorMatchingGroupBox.TabStop = false;
      this.colorMatchingGroupBox.Text = "Color Matching";
      // 
      // bScaleTrackBar
      // 
      this.bScaleTrackBar.AutoSize = false;
      this.bScaleTrackBar.Location = new System.Drawing.Point(38, 207);
      this.bScaleTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.bScaleTrackBar.Maximum = 15;
      this.bScaleTrackBar.Minimum = -15;
      this.bScaleTrackBar.Name = "bScaleTrackBar";
      this.bScaleTrackBar.RightToLeftLayout = true;
      this.bScaleTrackBar.Size = new System.Drawing.Size(142, 16);
      this.bScaleTrackBar.TabIndex = 11;
      this.bScaleTrackBar.TickFrequency = 2;
      this.bScaleTrackBar.ValueChanged += new System.EventHandler(this.bScaleTrackBar_ValueChanged);
      // 
      // bStripPanel
      // 
      this.bStripPanel.Location = new System.Drawing.Point(45, 187);
      this.bStripPanel.Margin = new System.Windows.Forms.Padding(2);
      this.bStripPanel.Name = "bStripPanel";
      this.bStripPanel.Size = new System.Drawing.Size(128, 16);
      this.bStripPanel.TabIndex = 10;
      // 
      // bCheckBox
      // 
      this.bCheckBox.AutoSize = true;
      this.bCheckBox.Checked = true;
      this.bCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.bCheckBox.Location = new System.Drawing.Point(8, 187);
      this.bCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.bCheckBox.Name = "bCheckBox";
      this.bCheckBox.Size = new System.Drawing.Size(36, 17);
      this.bCheckBox.TabIndex = 9;
      this.bCheckBox.Text = "b*";
      this.bCheckBox.UseVisualStyleBackColor = true;
      this.bCheckBox.CheckedChanged += new System.EventHandler(this.bCheckBox_CheckedChanged);
      // 
      // bOffsetTrackBar
      // 
      this.bOffsetTrackBar.AutoSize = false;
      this.bOffsetTrackBar.LargeChange = 10;
      this.bOffsetTrackBar.Location = new System.Drawing.Point(38, 167);
      this.bOffsetTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.bOffsetTrackBar.Maximum = 50;
      this.bOffsetTrackBar.Minimum = -50;
      this.bOffsetTrackBar.Name = "bOffsetTrackBar";
      this.bOffsetTrackBar.RightToLeftLayout = true;
      this.bOffsetTrackBar.Size = new System.Drawing.Size(142, 16);
      this.bOffsetTrackBar.TabIndex = 8;
      this.bOffsetTrackBar.TickFrequency = 10;
      this.bOffsetTrackBar.ValueChanged += new System.EventHandler(this.bOffsetTrackBar_ValueChanged);
      // 
      // aScaleTrackBar
      // 
      this.aScaleTrackBar.AutoSize = false;
      this.aScaleTrackBar.Location = new System.Drawing.Point(38, 134);
      this.aScaleTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.aScaleTrackBar.Maximum = 15;
      this.aScaleTrackBar.Minimum = -15;
      this.aScaleTrackBar.Name = "aScaleTrackBar";
      this.aScaleTrackBar.RightToLeftLayout = true;
      this.aScaleTrackBar.Size = new System.Drawing.Size(142, 16);
      this.aScaleTrackBar.TabIndex = 7;
      this.aScaleTrackBar.TickFrequency = 2;
      this.aScaleTrackBar.ValueChanged += new System.EventHandler(this.aScaleTrackBar_ValueChanged);
      // 
      // aStripPanel
      // 
      this.aStripPanel.Location = new System.Drawing.Point(45, 114);
      this.aStripPanel.Margin = new System.Windows.Forms.Padding(2);
      this.aStripPanel.Name = "aStripPanel";
      this.aStripPanel.Size = new System.Drawing.Size(128, 16);
      this.aStripPanel.TabIndex = 6;
      // 
      // aCheckBox
      // 
      this.aCheckBox.AutoSize = true;
      this.aCheckBox.Checked = true;
      this.aCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.aCheckBox.Location = new System.Drawing.Point(8, 114);
      this.aCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.aCheckBox.Name = "aCheckBox";
      this.aCheckBox.Size = new System.Drawing.Size(36, 17);
      this.aCheckBox.TabIndex = 5;
      this.aCheckBox.Text = "a*";
      this.aCheckBox.UseVisualStyleBackColor = true;
      this.aCheckBox.CheckedChanged += new System.EventHandler(this.aCheckBox_CheckedChanged);
      // 
      // aOffsetTrackBar
      // 
      this.aOffsetTrackBar.AutoSize = false;
      this.aOffsetTrackBar.LargeChange = 10;
      this.aOffsetTrackBar.Location = new System.Drawing.Point(38, 93);
      this.aOffsetTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.aOffsetTrackBar.Maximum = 50;
      this.aOffsetTrackBar.Minimum = -50;
      this.aOffsetTrackBar.Name = "aOffsetTrackBar";
      this.aOffsetTrackBar.RightToLeftLayout = true;
      this.aOffsetTrackBar.Size = new System.Drawing.Size(142, 16);
      this.aOffsetTrackBar.TabIndex = 4;
      this.aOffsetTrackBar.TickFrequency = 10;
      this.aOffsetTrackBar.ValueChanged += new System.EventHandler(this.aOffsetTrackBar_ValueChanged);
      // 
      // lScaleTrackBar
      // 
      this.lScaleTrackBar.AutoSize = false;
      this.lScaleTrackBar.Location = new System.Drawing.Point(38, 61);
      this.lScaleTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.lScaleTrackBar.Maximum = 15;
      this.lScaleTrackBar.Minimum = -15;
      this.lScaleTrackBar.Name = "lScaleTrackBar";
      this.lScaleTrackBar.RightToLeftLayout = true;
      this.lScaleTrackBar.Size = new System.Drawing.Size(142, 16);
      this.lScaleTrackBar.TabIndex = 3;
      this.lScaleTrackBar.TickFrequency = 2;
      this.lScaleTrackBar.ValueChanged += new System.EventHandler(this.lScaleTrackBar_ValueChanged);
      // 
      // lStripPanel
      // 
      this.lStripPanel.Location = new System.Drawing.Point(45, 41);
      this.lStripPanel.Margin = new System.Windows.Forms.Padding(2);
      this.lStripPanel.Name = "lStripPanel";
      this.lStripPanel.Size = new System.Drawing.Size(128, 16);
      this.lStripPanel.TabIndex = 2;
      // 
      // lCheckBox
      // 
      this.lCheckBox.AutoSize = true;
      this.lCheckBox.Checked = true;
      this.lCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.lCheckBox.Location = new System.Drawing.Point(8, 41);
      this.lCheckBox.Margin = new System.Windows.Forms.Padding(2);
      this.lCheckBox.Name = "lCheckBox";
      this.lCheckBox.Size = new System.Drawing.Size(36, 17);
      this.lCheckBox.TabIndex = 1;
      this.lCheckBox.Text = "L*";
      this.lCheckBox.UseVisualStyleBackColor = true;
      this.lCheckBox.CheckedChanged += new System.EventHandler(this.lCheckBox_CheckedChanged);
      // 
      // lOffsetTrackBar
      // 
      this.lOffsetTrackBar.AutoSize = false;
      this.lOffsetTrackBar.LargeChange = 10;
      this.lOffsetTrackBar.Location = new System.Drawing.Point(38, 20);
      this.lOffsetTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.lOffsetTrackBar.Maximum = 50;
      this.lOffsetTrackBar.Minimum = -50;
      this.lOffsetTrackBar.Name = "lOffsetTrackBar";
      this.lOffsetTrackBar.RightToLeftLayout = true;
      this.lOffsetTrackBar.Size = new System.Drawing.Size(142, 16);
      this.lOffsetTrackBar.TabIndex = 0;
      this.lOffsetTrackBar.TickFrequency = 10;
      this.lOffsetTrackBar.ValueChanged += new System.EventHandler(this.lOffsetTrackBar_ValueChanged);
      // 
      // shapeProgressBar
      // 
      this.shapeProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.shapeProgressBar.Location = new System.Drawing.Point(529, 79);
      this.shapeProgressBar.Margin = new System.Windows.Forms.Padding(2);
      this.shapeProgressBar.Name = "shapeProgressBar";
      this.shapeProgressBar.Size = new System.Drawing.Size(82, 5);
      this.shapeProgressBar.TabIndex = 26;
      // 
      // ColorFromOverlayForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(624, 352);
      this.Controls.Add(this.shapeProgressBar);
      this.Controls.Add(this.colorMatchingGroupBox);
      this.Controls.Add(this.colorSelectionGroupBox);
      this.Controls.Add(this.ringFilterGroupBox);
      this.Controls.Add(this.previewgroupBox);
      this.Controls.Add(this.cancelbutton);
      this.Controls.Add(this.okButton);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "ColorFromOverlayForm";
      this.Text = "Color Rings from Overlay Colors";
      this.Shown += new System.EventHandler(this.ColorFromOverlayForm_Shown);
      this.previewgroupBox.ResumeLayout(false);
      this.colorSelectionGroupBox.ResumeLayout(false);
      this.colorSelectionPanel.ResumeLayout(false);
      this.colorSelectionPanel.PerformLayout();
      this.colorMatchingGroupBox.ResumeLayout(false);
      this.colorMatchingGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bScaleTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bOffsetTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.aScaleTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.aOffsetTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lScaleTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lOffsetTrackBar)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button cancelbutton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.GroupBox previewgroupBox;
    private System.Windows.Forms.Panel previewPanel;
    private System.Windows.Forms.GroupBox ringFilterGroupBox;
    private System.Windows.Forms.GroupBox colorSelectionGroupBox;
    private System.Windows.Forms.Panel colorSelectionPanel;
    private System.Windows.Forms.Label paletteSectionLabel;
    private System.Windows.Forms.ComboBox paletteSectionComboBox;
    private System.Windows.Forms.GroupBox colorMatchingGroupBox;
    private System.Windows.Forms.TrackBar bScaleTrackBar;
    private System.Windows.Forms.Panel bStripPanel;
    private System.Windows.Forms.CheckBox bCheckBox;
    private System.Windows.Forms.TrackBar bOffsetTrackBar;
    private System.Windows.Forms.TrackBar aScaleTrackBar;
    private System.Windows.Forms.Panel aStripPanel;
    private System.Windows.Forms.CheckBox aCheckBox;
    private System.Windows.Forms.TrackBar aOffsetTrackBar;
    private System.Windows.Forms.TrackBar lScaleTrackBar;
    private System.Windows.Forms.Panel lStripPanel;
    private System.Windows.Forms.CheckBox lCheckBox;
    private System.Windows.Forms.TrackBar lOffsetTrackBar;
    private System.Windows.Forms.ProgressBar shapeProgressBar;
  }
}
