// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: NewDesignForm.Designer.cs


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
  partial class NewDesignForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDesignForm));
      this.okButton = new System.Windows.Forms.Button();
      this.cancelbutton = new System.Windows.Forms.Button();
      this.designSizeGroupBox = new System.Windows.Forms.GroupBox();
      this.heightUnitsLabel = new System.Windows.Forms.Label();
      this.widthUnitsLabel = new System.Windows.Forms.Label();
      this.designSizeUnitsLabel = new System.Windows.Forms.Label();
      this.designUnitsComboBox = new System.Windows.Forms.ComboBox();
      this.sizeRoundingComboBox = new System.Windows.Forms.ComboBox();
      this.designHeightTextBox = new System.Windows.Forms.TextBox();
      this.designWidthTextBox = new System.Windows.Forms.TextBox();
      this.designHeightLabel = new System.Windows.Forms.Label();
      this.designWidthLabel = new System.Windows.Forms.Label();
      this.newDesignTabControl = new System.Windows.Forms.TabControl();
      this.patternAndSizeTabPage = new System.Windows.Forms.TabPage();
      this.designScaleComboBox = new System.Windows.Forms.ComboBox();
      this.designScaleLabel = new System.Windows.Forms.Label();
      this.wrapComboBox = new System.Windows.Forms.ComboBox();
      this.wrapLabel = new System.Windows.Forms.Label();
      this.changeWeaveButton = new System.Windows.Forms.Button();
      this.weaveTextBox = new System.Windows.Forms.TextBox();
      this.weaveLabel = new System.Windows.Forms.Label();
      this.designNameTextBox = new System.Windows.Forms.TextBox();
      this.designNameLabel = new System.Windows.Forms.Label();
      this.designInfoTabPage = new System.Windows.Forms.TabPage();
      this.descriptionTextBox = new System.Windows.Forms.TextBox();
      this.descriptionLabel = new System.Windows.Forms.Label();
      this.designedByTextBox = new System.Windows.Forms.TextBox();
      this.dateTextBox = new System.Windows.Forms.TextBox();
      this.designedForTextBox = new System.Windows.Forms.TextBox();
      this.designedByLabel = new System.Windows.Forms.Label();
      this.dateLabel = new System.Windows.Forms.Label();
      this.designedForLabel = new System.Windows.Forms.Label();
      this.designSizeGroupBox.SuspendLayout();
      this.newDesignTabControl.SuspendLayout();
      this.patternAndSizeTabPage.SuspendLayout();
      this.designInfoTabPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(760, 50);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(160, 50);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelbutton
      // 
      this.cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelbutton.Location = new System.Drawing.Point(760, 120);
      this.cancelbutton.Name = "cancelbutton";
      this.cancelbutton.Size = new System.Drawing.Size(160, 50);
      this.cancelbutton.TabIndex = 3;
      this.cancelbutton.Text = "Cancel";
      this.cancelbutton.UseVisualStyleBackColor = true;
      // 
      // designSizeGroupBox
      // 
      this.designSizeGroupBox.Controls.Add(this.heightUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.widthUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.designSizeUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.designUnitsComboBox);
      this.designSizeGroupBox.Controls.Add(this.sizeRoundingComboBox);
      this.designSizeGroupBox.Controls.Add(this.designHeightTextBox);
      this.designSizeGroupBox.Controls.Add(this.designWidthTextBox);
      this.designSizeGroupBox.Controls.Add(this.designHeightLabel);
      this.designSizeGroupBox.Controls.Add(this.designWidthLabel);
      this.designSizeGroupBox.Location = new System.Drawing.Point(15, 180);
      this.designSizeGroupBox.Name = "designSizeGroupBox";
      this.designSizeGroupBox.Size = new System.Drawing.Size(475, 250);
      this.designSizeGroupBox.TabIndex = 4;
      this.designSizeGroupBox.TabStop = false;
      this.designSizeGroupBox.Text = "Design Size";
      // 
      // heightUnitsLabel
      // 
      this.heightUnitsLabel.AutoSize = true;
      this.heightUnitsLabel.Location = new System.Drawing.Point(260, 145);
      this.heightUnitsLabel.Name = "heightUnitsLabel";
      this.heightUnitsLabel.Size = new System.Drawing.Size(58, 25);
      this.heightUnitsLabel.TabIndex = 24;
      this.heightUnitsLabel.Text = "units";
      // 
      // widthUnitsLabel
      // 
      this.widthUnitsLabel.AutoSize = true;
      this.widthUnitsLabel.Location = new System.Drawing.Point(260, 95);
      this.widthUnitsLabel.Name = "widthUnitsLabel";
      this.widthUnitsLabel.Size = new System.Drawing.Size(58, 25);
      this.widthUnitsLabel.TabIndex = 23;
      this.widthUnitsLabel.Text = "units";
      // 
      // designSizeUnitsLabel
      // 
      this.designSizeUnitsLabel.AutoSize = true;
      this.designSizeUnitsLabel.Location = new System.Drawing.Point(40, 48);
      this.designSizeUnitsLabel.Name = "designSizeUnitsLabel";
      this.designSizeUnitsLabel.Size = new System.Drawing.Size(67, 25);
      this.designSizeUnitsLabel.TabIndex = 8;
      this.designSizeUnitsLabel.Text = "Units:";
      // 
      // designUnitsComboBox
      // 
      this.designUnitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.designUnitsComboBox.FormattingEnabled = true;
      this.designUnitsComboBox.Location = new System.Drawing.Point(150, 40);
      this.designUnitsComboBox.Name = "designUnitsComboBox";
      this.designUnitsComboBox.Size = new System.Drawing.Size(290, 33);
      this.designUnitsComboBox.TabIndex = 7;
      this.designUnitsComboBox.SelectedIndexChanged += new System.EventHandler(this.designUnitsComboBox_SelectedIndexChanged);
      // 
      // sizeRoundingComboBox
      // 
      this.sizeRoundingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.sizeRoundingComboBox.FormattingEnabled = true;
      this.sizeRoundingComboBox.Items.AddRange(new object[] {
            "Round units up (make at least this size)",
            "Round units down (no larger than this)"});
      this.sizeRoundingComboBox.Location = new System.Drawing.Point(20, 190);
      this.sizeRoundingComboBox.Name = "sizeRoundingComboBox";
      this.sizeRoundingComboBox.Size = new System.Drawing.Size(420, 33);
      this.sizeRoundingComboBox.TabIndex = 6;
      // 
      // designHeightTextBox
      // 
      this.designHeightTextBox.Location = new System.Drawing.Point(150, 140);
      this.designHeightTextBox.Name = "designHeightTextBox";
      this.designHeightTextBox.Size = new System.Drawing.Size(100, 31);
      this.designHeightTextBox.TabIndex = 3;
      // 
      // designWidthTextBox
      // 
      this.designWidthTextBox.Location = new System.Drawing.Point(150, 90);
      this.designWidthTextBox.Name = "designWidthTextBox";
      this.designWidthTextBox.Size = new System.Drawing.Size(100, 31);
      this.designWidthTextBox.TabIndex = 2;
      // 
      // designHeightLabel
      // 
      this.designHeightLabel.AutoSize = true;
      this.designHeightLabel.Location = new System.Drawing.Point(40, 145);
      this.designHeightLabel.Name = "designHeightLabel";
      this.designHeightLabel.Size = new System.Drawing.Size(80, 25);
      this.designHeightLabel.TabIndex = 1;
      this.designHeightLabel.Text = "Height:";
      // 
      // designWidthLabel
      // 
      this.designWidthLabel.AutoSize = true;
      this.designWidthLabel.Location = new System.Drawing.Point(40, 95);
      this.designWidthLabel.Name = "designWidthLabel";
      this.designWidthLabel.Size = new System.Drawing.Size(73, 25);
      this.designWidthLabel.TabIndex = 0;
      this.designWidthLabel.Text = "Width:";
      // 
      // newDesignTabControl
      // 
      this.newDesignTabControl.Controls.Add(this.patternAndSizeTabPage);
      this.newDesignTabControl.Controls.Add(this.designInfoTabPage);
      this.newDesignTabControl.Location = new System.Drawing.Point(10, 10);
      this.newDesignTabControl.Name = "newDesignTabControl";
      this.newDesignTabControl.SelectedIndex = 0;
      this.newDesignTabControl.Size = new System.Drawing.Size(730, 500);
      this.newDesignTabControl.TabIndex = 10;
      // 
      // patternAndSizeTabPage
      // 
      this.patternAndSizeTabPage.Controls.Add(this.designScaleComboBox);
      this.patternAndSizeTabPage.Controls.Add(this.designScaleLabel);
      this.patternAndSizeTabPage.Controls.Add(this.wrapComboBox);
      this.patternAndSizeTabPage.Controls.Add(this.designSizeGroupBox);
      this.patternAndSizeTabPage.Controls.Add(this.wrapLabel);
      this.patternAndSizeTabPage.Controls.Add(this.changeWeaveButton);
      this.patternAndSizeTabPage.Controls.Add(this.weaveTextBox);
      this.patternAndSizeTabPage.Controls.Add(this.weaveLabel);
      this.patternAndSizeTabPage.Controls.Add(this.designNameTextBox);
      this.patternAndSizeTabPage.Controls.Add(this.designNameLabel);
      this.patternAndSizeTabPage.Location = new System.Drawing.Point(8, 39);
      this.patternAndSizeTabPage.Name = "patternAndSizeTabPage";
      this.patternAndSizeTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.patternAndSizeTabPage.Size = new System.Drawing.Size(714, 453);
      this.patternAndSizeTabPage.TabIndex = 0;
      this.patternAndSizeTabPage.Text = "Weave and Size";
      this.patternAndSizeTabPage.UseVisualStyleBackColor = true;
      // 
      // designScaleComboBox
      // 
      this.designScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.designScaleComboBox.FormattingEnabled = true;
      this.designScaleComboBox.Location = new System.Drawing.Point(100, 130);
      this.designScaleComboBox.Name = "designScaleComboBox";
      this.designScaleComboBox.Size = new System.Drawing.Size(390, 33);
      this.designScaleComboBox.TabIndex = 18;
      this.designScaleComboBox.SelectedIndexChanged += new System.EventHandler(this.designScaleComboBox_SelectedIndexChanged);
      // 
      // designScaleLabel
      // 
      this.designScaleLabel.AutoSize = true;
      this.designScaleLabel.Location = new System.Drawing.Point(10, 138);
      this.designScaleLabel.Name = "designScaleLabel";
      this.designScaleLabel.Size = new System.Drawing.Size(72, 25);
      this.designScaleLabel.TabIndex = 17;
      this.designScaleLabel.Text = "Scale:";
      // 
      // wrapComboBox
      // 
      this.wrapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.wrapComboBox.FormattingEnabled = true;
      this.wrapComboBox.Location = new System.Drawing.Point(100, 90);
      this.wrapComboBox.Name = "wrapComboBox";
      this.wrapComboBox.Size = new System.Drawing.Size(390, 33);
      this.wrapComboBox.TabIndex = 16;
      // 
      // wrapLabel
      // 
      this.wrapLabel.AutoSize = true;
      this.wrapLabel.Location = new System.Drawing.Point(10, 98);
      this.wrapLabel.Name = "wrapLabel";
      this.wrapLabel.Size = new System.Drawing.Size(69, 25);
      this.wrapLabel.TabIndex = 15;
      this.wrapLabel.Text = "Wrap:";
      // 
      // changeWeaveButton
      // 
      this.changeWeaveButton.Location = new System.Drawing.Point(495, 49);
      this.changeWeaveButton.Name = "changeWeaveButton";
      this.changeWeaveButton.Size = new System.Drawing.Size(120, 40);
      this.changeWeaveButton.TabIndex = 14;
      this.changeWeaveButton.Text = "Change";
      this.changeWeaveButton.UseVisualStyleBackColor = true;
      this.changeWeaveButton.Click += new System.EventHandler(this.changeWeaveButton_Click);
      // 
      // weaveTextBox
      // 
      this.weaveTextBox.Enabled = false;
      this.weaveTextBox.Location = new System.Drawing.Point(100, 50);
      this.weaveTextBox.Name = "weaveTextBox";
      this.weaveTextBox.Size = new System.Drawing.Size(390, 31);
      this.weaveTextBox.TabIndex = 13;
      // 
      // weaveLabel
      // 
      this.weaveLabel.AutoSize = true;
      this.weaveLabel.Location = new System.Drawing.Point(10, 55);
      this.weaveLabel.Name = "weaveLabel";
      this.weaveLabel.Size = new System.Drawing.Size(85, 25);
      this.weaveLabel.TabIndex = 12;
      this.weaveLabel.Text = "Weave:";
      // 
      // designNameTextBox
      // 
      this.designNameTextBox.Location = new System.Drawing.Point(100, 10);
      this.designNameTextBox.Name = "designNameTextBox";
      this.designNameTextBox.Size = new System.Drawing.Size(390, 31);
      this.designNameTextBox.TabIndex = 11;
      // 
      // designNameLabel
      // 
      this.designNameLabel.AutoSize = true;
      this.designNameLabel.Location = new System.Drawing.Point(10, 15);
      this.designNameLabel.Name = "designNameLabel";
      this.designNameLabel.Size = new System.Drawing.Size(74, 25);
      this.designNameLabel.TabIndex = 10;
      this.designNameLabel.Text = "Name:";
      // 
      // designInfoTabPage
      // 
      this.designInfoTabPage.Controls.Add(this.descriptionTextBox);
      this.designInfoTabPage.Controls.Add(this.descriptionLabel);
      this.designInfoTabPage.Controls.Add(this.designedByTextBox);
      this.designInfoTabPage.Controls.Add(this.dateTextBox);
      this.designInfoTabPage.Controls.Add(this.designedForTextBox);
      this.designInfoTabPage.Controls.Add(this.designedByLabel);
      this.designInfoTabPage.Controls.Add(this.dateLabel);
      this.designInfoTabPage.Controls.Add(this.designedForLabel);
      this.designInfoTabPage.Location = new System.Drawing.Point(8, 39);
      this.designInfoTabPage.Name = "designInfoTabPage";
      this.designInfoTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.designInfoTabPage.Size = new System.Drawing.Size(714, 453);
      this.designInfoTabPage.TabIndex = 1;
      this.designInfoTabPage.Text = "Information";
      this.designInfoTabPage.UseVisualStyleBackColor = true;
      // 
      // descriptionTextBox
      // 
      this.descriptionTextBox.Location = new System.Drawing.Point(160, 130);
      this.descriptionTextBox.Multiline = true;
      this.descriptionTextBox.Name = "descriptionTextBox";
      this.descriptionTextBox.Size = new System.Drawing.Size(530, 170);
      this.descriptionTextBox.TabIndex = 24;
      // 
      // descriptionLabel
      // 
      this.descriptionLabel.AutoSize = true;
      this.descriptionLabel.Location = new System.Drawing.Point(10, 135);
      this.descriptionLabel.Name = "descriptionLabel";
      this.descriptionLabel.Size = new System.Drawing.Size(126, 25);
      this.descriptionLabel.TabIndex = 23;
      this.descriptionLabel.Text = "Description:";
      // 
      // designedByTextBox
      // 
      this.designedByTextBox.Location = new System.Drawing.Point(160, 90);
      this.designedByTextBox.Name = "designedByTextBox";
      this.designedByTextBox.Size = new System.Drawing.Size(300, 31);
      this.designedByTextBox.TabIndex = 22;
      // 
      // dateTextBox
      // 
      this.dateTextBox.Location = new System.Drawing.Point(160, 50);
      this.dateTextBox.Name = "dateTextBox";
      this.dateTextBox.Size = new System.Drawing.Size(300, 31);
      this.dateTextBox.TabIndex = 21;
      // 
      // designedForTextBox
      // 
      this.designedForTextBox.Location = new System.Drawing.Point(160, 10);
      this.designedForTextBox.Name = "designedForTextBox";
      this.designedForTextBox.Size = new System.Drawing.Size(300, 31);
      this.designedForTextBox.TabIndex = 20;
      // 
      // designedByLabel
      // 
      this.designedByLabel.AutoSize = true;
      this.designedByLabel.Location = new System.Drawing.Point(10, 95);
      this.designedByLabel.Name = "designedByLabel";
      this.designedByLabel.Size = new System.Drawing.Size(140, 25);
      this.designedByLabel.TabIndex = 19;
      this.designedByLabel.Text = "Designed By:";
      // 
      // dateLabel
      // 
      this.dateLabel.AutoSize = true;
      this.dateLabel.Location = new System.Drawing.Point(10, 55);
      this.dateLabel.Name = "dateLabel";
      this.dateLabel.Size = new System.Drawing.Size(63, 25);
      this.dateLabel.TabIndex = 18;
      this.dateLabel.Text = "Date:";
      // 
      // designedForLabel
      // 
      this.designedForLabel.AutoSize = true;
      this.designedForLabel.Location = new System.Drawing.Point(10, 15);
      this.designedForLabel.Name = "designedForLabel";
      this.designedForLabel.Size = new System.Drawing.Size(147, 25);
      this.designedForLabel.TabIndex = 17;
      this.designedForLabel.Text = "Designed For:";
      // 
      // NewDesignForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelbutton;
      this.ClientSize = new System.Drawing.Size(944, 529);
      this.Controls.Add(this.newDesignTabControl);
      this.Controls.Add(this.cancelbutton);
      this.Controls.Add(this.okButton);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "NewDesignForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "New Design";
      this.designSizeGroupBox.ResumeLayout(false);
      this.designSizeGroupBox.PerformLayout();
      this.newDesignTabControl.ResumeLayout(false);
      this.patternAndSizeTabPage.ResumeLayout(false);
      this.patternAndSizeTabPage.PerformLayout();
      this.designInfoTabPage.ResumeLayout(false);
      this.designInfoTabPage.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelbutton;
    private System.Windows.Forms.GroupBox designSizeGroupBox;
    private System.Windows.Forms.TextBox designHeightTextBox;
    private System.Windows.Forms.TextBox designWidthTextBox;
    private System.Windows.Forms.Label designHeightLabel;
    private System.Windows.Forms.Label designWidthLabel;
    private System.Windows.Forms.TabControl newDesignTabControl;
    private System.Windows.Forms.TabPage patternAndSizeTabPage;
    private System.Windows.Forms.ComboBox wrapComboBox;
    private System.Windows.Forms.Label wrapLabel;
    private System.Windows.Forms.Button changeWeaveButton;
    private System.Windows.Forms.TextBox weaveTextBox;
    private System.Windows.Forms.Label weaveLabel;
    private System.Windows.Forms.TextBox designNameTextBox;
    private System.Windows.Forms.Label designNameLabel;
    private System.Windows.Forms.TabPage designInfoTabPage;
    private System.Windows.Forms.TextBox designedByTextBox;
    private System.Windows.Forms.TextBox dateTextBox;
    private System.Windows.Forms.TextBox designedForTextBox;
    private System.Windows.Forms.Label designedByLabel;
    private System.Windows.Forms.Label dateLabel;
    private System.Windows.Forms.Label designedForLabel;
    private System.Windows.Forms.TextBox descriptionTextBox;
    private System.Windows.Forms.Label descriptionLabel;
    private System.Windows.Forms.ComboBox sizeRoundingComboBox;
    private System.Windows.Forms.Label designSizeUnitsLabel;
    private System.Windows.Forms.ComboBox designUnitsComboBox;
    private System.Windows.Forms.Label heightUnitsLabel;
    private System.Windows.Forms.Label widthUnitsLabel;
    private System.Windows.Forms.ComboBox designScaleComboBox;
    private System.Windows.Forms.Label designScaleLabel;
  }
}