// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: DesignInfoForm.Designer.cs


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
  partial class DesignInfoForm
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
        selectedPattern?.Dispose();
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignInfoForm));
      this.designSizeGroupBox = new System.Windows.Forms.GroupBox();
      this.heightUnitsLabel = new System.Windows.Forms.Label();
      this.widthUnitsLabel = new System.Windows.Forms.Label();
      this.changeSizeButton = new System.Windows.Forms.Button();
      this.designHeightTextBox = new System.Windows.Forms.TextBox();
      this.designWidthTextBox = new System.Windows.Forms.TextBox();
      this.designHeightLabel = new System.Windows.Forms.Label();
      this.designWidthLabel = new System.Windows.Forms.Label();
      this.cancelbutton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.designNameTextBox = new System.Windows.Forms.TextBox();
      this.designNameLabel = new System.Windows.Forms.Label();
      this.designedForLabel = new System.Windows.Forms.Label();
      this.dateLabel = new System.Windows.Forms.Label();
      this.designedByLabel = new System.Windows.Forms.Label();
      this.descriptionLabel = new System.Windows.Forms.Label();
      this.designedForTextBox = new System.Windows.Forms.TextBox();
      this.dateTextBox = new System.Windows.Forms.TextBox();
      this.designedByTextBox = new System.Windows.Forms.TextBox();
      this.descriptionTextBox = new System.Windows.Forms.TextBox();
      this.weaveLabel = new System.Windows.Forms.Label();
      this.weaveTextBox = new System.Windows.Forms.TextBox();
      this.changeWeaveButton = new System.Windows.Forms.Button();
      this.designScaleComboBox = new System.Windows.Forms.ComboBox();
      this.designScaleLabel = new System.Windows.Forms.Label();
      this.wrapComboBox = new System.Windows.Forms.ComboBox();
      this.wrapLabel = new System.Windows.Forms.Label();
      this.designSizeGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // designSizeGroupBox
      // 
      this.designSizeGroupBox.Controls.Add(this.heightUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.widthUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.changeSizeButton);
      this.designSizeGroupBox.Controls.Add(this.designHeightTextBox);
      this.designSizeGroupBox.Controls.Add(this.designWidthTextBox);
      this.designSizeGroupBox.Controls.Add(this.designHeightLabel);
      this.designSizeGroupBox.Controls.Add(this.designWidthLabel);
      this.designSizeGroupBox.Location = new System.Drawing.Point(15, 180);
      this.designSizeGroupBox.Name = "designSizeGroupBox";
      this.designSizeGroupBox.Size = new System.Drawing.Size(475, 155);
      this.designSizeGroupBox.TabIndex = 9;
      this.designSizeGroupBox.TabStop = false;
      this.designSizeGroupBox.Text = "Design Size";
      // 
      // heightUnitsLabel
      // 
      this.heightUnitsLabel.AutoSize = true;
      this.heightUnitsLabel.Location = new System.Drawing.Point(260, 85);
      this.heightUnitsLabel.Name = "heightUnitsLabel";
      this.heightUnitsLabel.Size = new System.Drawing.Size(58, 25);
      this.heightUnitsLabel.TabIndex = 8;
      this.heightUnitsLabel.Text = "units";
      // 
      // widthUnitsLabel
      // 
      this.widthUnitsLabel.AutoSize = true;
      this.widthUnitsLabel.Location = new System.Drawing.Point(260, 45);
      this.widthUnitsLabel.Name = "widthUnitsLabel";
      this.widthUnitsLabel.Size = new System.Drawing.Size(58, 25);
      this.widthUnitsLabel.TabIndex = 7;
      this.widthUnitsLabel.Text = "units";
      // 
      // changeSizeButton
      // 
      this.changeSizeButton.Location = new System.Drawing.Point(330, 39);
      this.changeSizeButton.Name = "changeSizeButton";
      this.changeSizeButton.Size = new System.Drawing.Size(120, 80);
      this.changeSizeButton.TabIndex = 6;
      this.changeSizeButton.Text = "Change Size";
      this.changeSizeButton.UseVisualStyleBackColor = true;
      this.changeSizeButton.Click += new System.EventHandler(this.changeSizeButton_Click);
      // 
      // designHeightTextBox
      // 
      this.designHeightTextBox.Enabled = false;
      this.designHeightTextBox.Location = new System.Drawing.Point(150, 80);
      this.designHeightTextBox.Name = "designHeightTextBox";
      this.designHeightTextBox.Size = new System.Drawing.Size(100, 31);
      this.designHeightTextBox.TabIndex = 3;
      // 
      // designWidthTextBox
      // 
      this.designWidthTextBox.Enabled = false;
      this.designWidthTextBox.Location = new System.Drawing.Point(150, 40);
      this.designWidthTextBox.Name = "designWidthTextBox";
      this.designWidthTextBox.Size = new System.Drawing.Size(100, 31);
      this.designWidthTextBox.TabIndex = 2;
      // 
      // designHeightLabel
      // 
      this.designHeightLabel.AutoSize = true;
      this.designHeightLabel.Location = new System.Drawing.Point(30, 85);
      this.designHeightLabel.Name = "designHeightLabel";
      this.designHeightLabel.Size = new System.Drawing.Size(80, 25);
      this.designHeightLabel.TabIndex = 1;
      this.designHeightLabel.Text = "Height:";
      // 
      // designWidthLabel
      // 
      this.designWidthLabel.AutoSize = true;
      this.designWidthLabel.Location = new System.Drawing.Point(30, 45);
      this.designWidthLabel.Name = "designWidthLabel";
      this.designWidthLabel.Size = new System.Drawing.Size(73, 25);
      this.designWidthLabel.TabIndex = 0;
      this.designWidthLabel.Text = "Width:";
      // 
      // cancelbutton
      // 
      this.cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelbutton.Location = new System.Drawing.Point(650, 78);
      this.cancelbutton.Name = "cancelbutton";
      this.cancelbutton.Size = new System.Drawing.Size(160, 50);
      this.cancelbutton.TabIndex = 8;
      this.cancelbutton.Text = "Cancel";
      this.cancelbutton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(650, 16);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(160, 50);
      this.okButton.TabIndex = 7;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // designNameTextBox
      // 
      this.designNameTextBox.Location = new System.Drawing.Point(100, 10);
      this.designNameTextBox.Name = "designNameTextBox";
      this.designNameTextBox.Size = new System.Drawing.Size(390, 31);
      this.designNameTextBox.TabIndex = 6;
      // 
      // designNameLabel
      // 
      this.designNameLabel.AutoSize = true;
      this.designNameLabel.Location = new System.Drawing.Point(10, 15);
      this.designNameLabel.Name = "designNameLabel";
      this.designNameLabel.Size = new System.Drawing.Size(74, 25);
      this.designNameLabel.TabIndex = 5;
      this.designNameLabel.Text = "Name:";
      // 
      // designedForLabel
      // 
      this.designedForLabel.AutoSize = true;
      this.designedForLabel.Location = new System.Drawing.Point(30, 365);
      this.designedForLabel.Name = "designedForLabel";
      this.designedForLabel.Size = new System.Drawing.Size(147, 25);
      this.designedForLabel.TabIndex = 10;
      this.designedForLabel.Text = "Designed For:";
      // 
      // dateLabel
      // 
      this.dateLabel.AutoSize = true;
      this.dateLabel.Location = new System.Drawing.Point(30, 405);
      this.dateLabel.Name = "dateLabel";
      this.dateLabel.Size = new System.Drawing.Size(63, 25);
      this.dateLabel.TabIndex = 11;
      this.dateLabel.Text = "Date:";
      // 
      // designedByLabel
      // 
      this.designedByLabel.AutoSize = true;
      this.designedByLabel.Location = new System.Drawing.Point(30, 445);
      this.designedByLabel.Name = "designedByLabel";
      this.designedByLabel.Size = new System.Drawing.Size(140, 25);
      this.designedByLabel.TabIndex = 12;
      this.designedByLabel.Text = "Designed By:";
      // 
      // descriptionLabel
      // 
      this.descriptionLabel.AutoSize = true;
      this.descriptionLabel.Location = new System.Drawing.Point(30, 485);
      this.descriptionLabel.Name = "descriptionLabel";
      this.descriptionLabel.Size = new System.Drawing.Size(126, 25);
      this.descriptionLabel.TabIndex = 13;
      this.descriptionLabel.Text = "Description:";
      // 
      // designedForTextBox
      // 
      this.designedForTextBox.Location = new System.Drawing.Point(190, 360);
      this.designedForTextBox.Name = "designedForTextBox";
      this.designedForTextBox.Size = new System.Drawing.Size(300, 31);
      this.designedForTextBox.TabIndex = 14;
      // 
      // dateTextBox
      // 
      this.dateTextBox.Location = new System.Drawing.Point(190, 400);
      this.dateTextBox.Name = "dateTextBox";
      this.dateTextBox.Size = new System.Drawing.Size(300, 31);
      this.dateTextBox.TabIndex = 15;
      // 
      // designedByTextBox
      // 
      this.designedByTextBox.Location = new System.Drawing.Point(190, 440);
      this.designedByTextBox.Name = "designedByTextBox";
      this.designedByTextBox.Size = new System.Drawing.Size(300, 31);
      this.designedByTextBox.TabIndex = 16;
      // 
      // descriptionTextBox
      // 
      this.descriptionTextBox.Location = new System.Drawing.Point(190, 480);
      this.descriptionTextBox.Multiline = true;
      this.descriptionTextBox.Name = "descriptionTextBox";
      this.descriptionTextBox.Size = new System.Drawing.Size(528, 127);
      this.descriptionTextBox.TabIndex = 17;
      // 
      // weaveLabel
      // 
      this.weaveLabel.AutoSize = true;
      this.weaveLabel.Location = new System.Drawing.Point(10, 55);
      this.weaveLabel.Name = "weaveLabel";
      this.weaveLabel.Size = new System.Drawing.Size(85, 25);
      this.weaveLabel.TabIndex = 19;
      this.weaveLabel.Text = "Weave:";
      // 
      // weaveTextBox
      // 
      this.weaveTextBox.Enabled = false;
      this.weaveTextBox.Location = new System.Drawing.Point(100, 50);
      this.weaveTextBox.Name = "weaveTextBox";
      this.weaveTextBox.Size = new System.Drawing.Size(390, 31);
      this.weaveTextBox.TabIndex = 20;
      // 
      // changeWeaveButton
      // 
      this.changeWeaveButton.Location = new System.Drawing.Point(495, 49);
      this.changeWeaveButton.Name = "changeWeaveButton";
      this.changeWeaveButton.Size = new System.Drawing.Size(120, 41);
      this.changeWeaveButton.TabIndex = 21;
      this.changeWeaveButton.Text = "Change";
      this.changeWeaveButton.UseVisualStyleBackColor = true;
      this.changeWeaveButton.Click += new System.EventHandler(this.changeWeaveButton_Click);
      // 
      // designScaleComboBox
      // 
      this.designScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.designScaleComboBox.FormattingEnabled = true;
      this.designScaleComboBox.Location = new System.Drawing.Point(100, 130);
      this.designScaleComboBox.Name = "designScaleComboBox";
      this.designScaleComboBox.Size = new System.Drawing.Size(390, 33);
      this.designScaleComboBox.TabIndex = 26;
      this.designScaleComboBox.SelectedIndexChanged += new System.EventHandler(this.designScaleComboBox_SelectedIndexChanged);
      // 
      // designScaleLabel
      // 
      this.designScaleLabel.AutoSize = true;
      this.designScaleLabel.Location = new System.Drawing.Point(10, 138);
      this.designScaleLabel.Name = "designScaleLabel";
      this.designScaleLabel.Size = new System.Drawing.Size(72, 25);
      this.designScaleLabel.TabIndex = 25;
      this.designScaleLabel.Text = "Scale:";
      // 
      // wrapComboBox
      // 
      this.wrapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.wrapComboBox.FormattingEnabled = true;
      this.wrapComboBox.Location = new System.Drawing.Point(100, 90);
      this.wrapComboBox.Name = "wrapComboBox";
      this.wrapComboBox.Size = new System.Drawing.Size(390, 33);
      this.wrapComboBox.TabIndex = 24;
      this.wrapComboBox.SelectedIndexChanged += new System.EventHandler(this.wrapComboBox_SelectedIndexChanged);
      // 
      // wrapLabel
      // 
      this.wrapLabel.AutoSize = true;
      this.wrapLabel.Location = new System.Drawing.Point(10, 98);
      this.wrapLabel.Name = "wrapLabel";
      this.wrapLabel.Size = new System.Drawing.Size(69, 25);
      this.wrapLabel.TabIndex = 23;
      this.wrapLabel.Text = "Wrap:";
      // 
      // DesignInfoForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(834, 619);
      this.Controls.Add(this.designScaleComboBox);
      this.Controls.Add(this.designScaleLabel);
      this.Controls.Add(this.wrapComboBox);
      this.Controls.Add(this.wrapLabel);
      this.Controls.Add(this.changeWeaveButton);
      this.Controls.Add(this.weaveTextBox);
      this.Controls.Add(this.weaveLabel);
      this.Controls.Add(this.descriptionTextBox);
      this.Controls.Add(this.designedByTextBox);
      this.Controls.Add(this.dateTextBox);
      this.Controls.Add(this.designedForTextBox);
      this.Controls.Add(this.descriptionLabel);
      this.Controls.Add(this.designedByLabel);
      this.Controls.Add(this.dateLabel);
      this.Controls.Add(this.designedForLabel);
      this.Controls.Add(this.designSizeGroupBox);
      this.Controls.Add(this.cancelbutton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.designNameTextBox);
      this.Controls.Add(this.designNameLabel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DesignInfoForm";
      this.Text = "Design Information";
      this.designSizeGroupBox.ResumeLayout(false);
      this.designSizeGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox designSizeGroupBox;
    private System.Windows.Forms.TextBox designHeightTextBox;
    private System.Windows.Forms.TextBox designWidthTextBox;
    private System.Windows.Forms.Label designHeightLabel;
    private System.Windows.Forms.Label designWidthLabel;
    private System.Windows.Forms.Button cancelbutton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.TextBox designNameTextBox;
    private System.Windows.Forms.Label designNameLabel;
    private System.Windows.Forms.Label designedForLabel;
    private System.Windows.Forms.Label dateLabel;
    private System.Windows.Forms.Label designedByLabel;
    private System.Windows.Forms.Label descriptionLabel;
    private System.Windows.Forms.TextBox designedForTextBox;
    private System.Windows.Forms.TextBox dateTextBox;
    private System.Windows.Forms.TextBox designedByTextBox;
    private System.Windows.Forms.TextBox descriptionTextBox;
    private System.Windows.Forms.Label weaveLabel;
    private System.Windows.Forms.TextBox weaveTextBox;
    private System.Windows.Forms.Button changeSizeButton;
    private System.Windows.Forms.Button changeWeaveButton;
    private System.Windows.Forms.Label heightUnitsLabel;
    private System.Windows.Forms.Label widthUnitsLabel;
    private System.Windows.Forms.ComboBox designScaleComboBox;
    private System.Windows.Forms.Label designScaleLabel;
    private System.Windows.Forms.ComboBox wrapComboBox;
    private System.Windows.Forms.Label wrapLabel;
  }
}