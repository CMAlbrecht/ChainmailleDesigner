// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorReplacementForm.Designer.cs


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
  partial class ColorReplacementForm
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
      this.cancelbutton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.colorReplacementGroupBox = new System.Windows.Forms.GroupBox();
      this.colorReplacementPanel = new System.Windows.Forms.Panel();
      this.withLabel = new System.Windows.Forms.Label();
      this.replaceLabel = new System.Windows.Forms.Label();
      this.colorSelectionGroupBox = new System.Windows.Forms.GroupBox();
      this.colorSelectionPanel = new System.Windows.Forms.Panel();
      this.otherColorButton = new System.Windows.Forms.Button();
      this.paletteSectionLabel = new System.Windows.Forms.Label();
      this.paletteSectionComboBox = new System.Windows.Forms.ComboBox();
      this.previewGroupBox = new System.Windows.Forms.GroupBox();
      this.previewPanel = new System.Windows.Forms.Panel();
      this.ringFilterGroupBox = new System.Windows.Forms.GroupBox();
      this.colorReplacementGroupBox.SuspendLayout();
      this.colorReplacementPanel.SuspendLayout();
      this.colorSelectionGroupBox.SuspendLayout();
      this.colorSelectionPanel.SuspendLayout();
      this.previewGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // cancelbutton
      // 
      this.cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelbutton.Location = new System.Drawing.Point(825, 61);
      this.cancelbutton.Margin = new System.Windows.Forms.Padding(2);
      this.cancelbutton.Name = "cancelbutton";
      this.cancelbutton.Size = new System.Drawing.Size(105, 32);
      this.cancelbutton.TabIndex = 10;
      this.cancelbutton.Text = "Cancel";
      this.cancelbutton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(825, 19);
      this.okButton.Margin = new System.Windows.Forms.Padding(2);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(105, 32);
      this.okButton.TabIndex = 9;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // colorReplacementGroupBox
      // 
      this.colorReplacementGroupBox.Controls.Add(this.colorReplacementPanel);
      this.colorReplacementGroupBox.Location = new System.Drawing.Point(7, 13);
      this.colorReplacementGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.colorReplacementGroupBox.Name = "colorReplacementGroupBox";
      this.colorReplacementGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.colorReplacementGroupBox.Size = new System.Drawing.Size(393, 211);
      this.colorReplacementGroupBox.TabIndex = 11;
      this.colorReplacementGroupBox.TabStop = false;
      this.colorReplacementGroupBox.Text = "Color Replacements";
      // 
      // colorReplacementPanel
      // 
      this.colorReplacementPanel.Controls.Add(this.withLabel);
      this.colorReplacementPanel.Controls.Add(this.replaceLabel);
      this.colorReplacementPanel.Location = new System.Drawing.Point(7, 19);
      this.colorReplacementPanel.Margin = new System.Windows.Forms.Padding(2);
      this.colorReplacementPanel.Name = "colorReplacementPanel";
      this.colorReplacementPanel.Size = new System.Drawing.Size(380, 189);
      this.colorReplacementPanel.TabIndex = 1;
      // 
      // withLabel
      // 
      this.withLabel.AutoSize = true;
      this.withLabel.Location = new System.Drawing.Point(230, 13);
      this.withLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.withLabel.Name = "withLabel";
      this.withLabel.Size = new System.Drawing.Size(36, 17);
      this.withLabel.TabIndex = 1;
      this.withLabel.Text = "With";
      // 
      // replaceLabel
      // 
      this.replaceLabel.AutoSize = true;
      this.replaceLabel.Location = new System.Drawing.Point(43, 13);
      this.replaceLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.replaceLabel.Name = "replaceLabel";
      this.replaceLabel.Size = new System.Drawing.Size(60, 17);
      this.replaceLabel.TabIndex = 0;
      this.replaceLabel.Text = "Replace";
      // 
      // colorSelectionGroupBox
      // 
      this.colorSelectionGroupBox.Controls.Add(this.colorSelectionPanel);
      this.colorSelectionGroupBox.Location = new System.Drawing.Point(413, 13);
      this.colorSelectionGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.colorSelectionGroupBox.Name = "colorSelectionGroupBox";
      this.colorSelectionGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.colorSelectionGroupBox.Size = new System.Drawing.Size(393, 211);
      this.colorSelectionGroupBox.TabIndex = 12;
      this.colorSelectionGroupBox.TabStop = false;
      this.colorSelectionGroupBox.Text = "Color Selection";
      // 
      // colorSelectionPanel
      // 
      this.colorSelectionPanel.Controls.Add(this.otherColorButton);
      this.colorSelectionPanel.Controls.Add(this.paletteSectionLabel);
      this.colorSelectionPanel.Controls.Add(this.paletteSectionComboBox);
      this.colorSelectionPanel.Location = new System.Drawing.Point(7, 19);
      this.colorSelectionPanel.Margin = new System.Windows.Forms.Padding(2);
      this.colorSelectionPanel.Name = "colorSelectionPanel";
      this.colorSelectionPanel.Size = new System.Drawing.Size(380, 189);
      this.colorSelectionPanel.TabIndex = 2;
      // 
      // otherColorButton
      // 
      this.otherColorButton.Location = new System.Drawing.Point(253, 10);
      this.otherColorButton.Margin = new System.Windows.Forms.Padding(2);
      this.otherColorButton.Name = "otherColorButton";
      this.otherColorButton.Size = new System.Drawing.Size(113, 42);
      this.otherColorButton.TabIndex = 2;
      this.otherColorButton.Text = "Other Color";
      this.otherColorButton.UseVisualStyleBackColor = true;
      this.otherColorButton.Click += new System.EventHandler(this.otherColorButton_Click);
      // 
      // paletteSectionLabel
      // 
      this.paletteSectionLabel.AutoSize = true;
      this.paletteSectionLabel.Location = new System.Drawing.Point(10, 6);
      this.paletteSectionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.paletteSectionLabel.Name = "paletteSectionLabel";
      this.paletteSectionLabel.Size = new System.Drawing.Size(103, 17);
      this.paletteSectionLabel.TabIndex = 1;
      this.paletteSectionLabel.Text = "Palette Section";
      // 
      // paletteSectionComboBox
      // 
      this.paletteSectionComboBox.FormattingEnabled = true;
      this.paletteSectionComboBox.Location = new System.Drawing.Point(10, 26);
      this.paletteSectionComboBox.Margin = new System.Windows.Forms.Padding(2);
      this.paletteSectionComboBox.Name = "paletteSectionComboBox";
      this.paletteSectionComboBox.Size = new System.Drawing.Size(148, 24);
      this.paletteSectionComboBox.TabIndex = 0;
      this.paletteSectionComboBox.SelectedIndexChanged += new System.EventHandler(this.paletteSectionComboBox_SelectedIndexChanged);
      // 
      // previewGroupBox
      // 
      this.previewGroupBox.Controls.Add(this.previewPanel);
      this.previewGroupBox.Location = new System.Drawing.Point(7, 230);
      this.previewGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.previewGroupBox.Name = "previewGroupBox";
      this.previewGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.previewGroupBox.Size = new System.Drawing.Size(800, 250);
      this.previewGroupBox.TabIndex = 13;
      this.previewGroupBox.TabStop = false;
      this.previewGroupBox.Text = "Preview";
      // 
      // previewPanel
      // 
      this.previewPanel.Location = new System.Drawing.Point(7, 19);
      this.previewPanel.Margin = new System.Windows.Forms.Padding(2);
      this.previewPanel.Name = "previewPanel";
      this.previewPanel.Size = new System.Drawing.Size(787, 227);
      this.previewPanel.TabIndex = 0;
      // 
      // ringFilterGroupBox
      // 
      this.ringFilterGroupBox.Location = new System.Drawing.Point(825, 120);
      this.ringFilterGroupBox.Name = "ringFilterGroupBox";
      this.ringFilterGroupBox.Size = new System.Drawing.Size(105, 360);
      this.ringFilterGroupBox.TabIndex = 14;
      this.ringFilterGroupBox.TabStop = false;
      this.ringFilterGroupBox.Text = "Affect Rings";
      // 
      // ColorReplacementForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(949, 492);
      this.Controls.Add(this.ringFilterGroupBox);
      this.Controls.Add(this.previewGroupBox);
      this.Controls.Add(this.colorSelectionGroupBox);
      this.Controls.Add(this.colorReplacementGroupBox);
      this.Controls.Add(this.cancelbutton);
      this.Controls.Add(this.okButton);
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "ColorReplacementForm";
      this.Text = "Replace Colors in Design";
      this.colorReplacementGroupBox.ResumeLayout(false);
      this.colorReplacementPanel.ResumeLayout(false);
      this.colorReplacementPanel.PerformLayout();
      this.colorSelectionGroupBox.ResumeLayout(false);
      this.colorSelectionPanel.ResumeLayout(false);
      this.colorSelectionPanel.PerformLayout();
      this.previewGroupBox.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cancelbutton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.GroupBox colorReplacementGroupBox;
    private System.Windows.Forms.Panel colorReplacementPanel;
    private System.Windows.Forms.GroupBox colorSelectionGroupBox;
    private System.Windows.Forms.Panel colorSelectionPanel;
    private System.Windows.Forms.GroupBox previewGroupBox;
    private System.Windows.Forms.Panel previewPanel;
    private System.Windows.Forms.Label withLabel;
    private System.Windows.Forms.Label replaceLabel;
    private System.Windows.Forms.Button otherColorButton;
    private System.Windows.Forms.Label paletteSectionLabel;
    private System.Windows.Forms.ComboBox paletteSectionComboBox;
    private System.Windows.Forms.GroupBox ringFilterGroupBox;
  }
}