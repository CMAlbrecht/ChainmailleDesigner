// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: DesignSizeForm.Designer.cs


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
  partial class DesignSizeForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignSizeForm));
      this.designUnitsComboBox = new System.Windows.Forms.ComboBox();
      this.designHeightTextBox = new System.Windows.Forms.TextBox();
      this.designWidthTextBox = new System.Windows.Forms.TextBox();
      this.designHeightLabel = new System.Windows.Forms.Label();
      this.designWidthLabel = new System.Windows.Forms.Label();
      this.designSizeGroupBox = new System.Windows.Forms.GroupBox();
      this.sizeRoundingComboBox = new System.Windows.Forms.ComboBox();
      this.designSizeUnitsLabel = new System.Windows.Forms.Label();
      this.heightUnitsLabel = new System.Windows.Forms.Label();
      this.widthUnitsLabel = new System.Windows.Forms.Label();
      this.rightEdgeLabel = new System.Windows.Forms.Label();
      this.leftEdgeLabel = new System.Windows.Forms.Label();
      this.bottomEdgeLabel = new System.Windows.Forms.Label();
      this.topEdgeLabel = new System.Windows.Forms.Label();
      this.leftEdgeNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.rightEdgeNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.topEdgeNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.bottomEdgeNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.cancelbutton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.designSizeGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.leftEdgeNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.rightEdgeNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.topEdgeNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bottomEdgeNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // designUnitsComboBox
      // 
      this.designUnitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.designUnitsComboBox.FormattingEnabled = true;
      this.designUnitsComboBox.Location = new System.Drawing.Point(540, 30);
      this.designUnitsComboBox.Name = "designUnitsComboBox";
      this.designUnitsComboBox.Size = new System.Drawing.Size(200, 33);
      this.designUnitsComboBox.TabIndex = 4;
      this.designUnitsComboBox.SelectedIndexChanged += new System.EventHandler(this.designUnitsComboBox_SelectedIndexChanged);
      // 
      // designHeightTextBox
      // 
      this.designHeightTextBox.Enabled = false;
      this.designHeightTextBox.Location = new System.Drawing.Point(270, 185);
      this.designHeightTextBox.Name = "designHeightTextBox";
      this.designHeightTextBox.Size = new System.Drawing.Size(120, 31);
      this.designHeightTextBox.TabIndex = 3;
      // 
      // designWidthTextBox
      // 
      this.designWidthTextBox.Enabled = false;
      this.designWidthTextBox.Location = new System.Drawing.Point(270, 145);
      this.designWidthTextBox.Name = "designWidthTextBox";
      this.designWidthTextBox.Size = new System.Drawing.Size(120, 31);
      this.designWidthTextBox.TabIndex = 2;
      // 
      // designHeightLabel
      // 
      this.designHeightLabel.AutoSize = true;
      this.designHeightLabel.Location = new System.Drawing.Point(180, 190);
      this.designHeightLabel.Name = "designHeightLabel";
      this.designHeightLabel.Size = new System.Drawing.Size(80, 25);
      this.designHeightLabel.TabIndex = 1;
      this.designHeightLabel.Text = "Height:";
      // 
      // designWidthLabel
      // 
      this.designWidthLabel.AutoSize = true;
      this.designWidthLabel.Location = new System.Drawing.Point(180, 150);
      this.designWidthLabel.Name = "designWidthLabel";
      this.designWidthLabel.Size = new System.Drawing.Size(73, 25);
      this.designWidthLabel.TabIndex = 0;
      this.designWidthLabel.Text = "Width:";
      // 
      // designSizeGroupBox
      // 
      this.designSizeGroupBox.Controls.Add(this.sizeRoundingComboBox);
      this.designSizeGroupBox.Controls.Add(this.designSizeUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.heightUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.widthUnitsLabel);
      this.designSizeGroupBox.Controls.Add(this.rightEdgeLabel);
      this.designSizeGroupBox.Controls.Add(this.leftEdgeLabel);
      this.designSizeGroupBox.Controls.Add(this.bottomEdgeLabel);
      this.designSizeGroupBox.Controls.Add(this.topEdgeLabel);
      this.designSizeGroupBox.Controls.Add(this.leftEdgeNumericUpDown);
      this.designSizeGroupBox.Controls.Add(this.rightEdgeNumericUpDown);
      this.designSizeGroupBox.Controls.Add(this.topEdgeNumericUpDown);
      this.designSizeGroupBox.Controls.Add(this.bottomEdgeNumericUpDown);
      this.designSizeGroupBox.Controls.Add(this.designUnitsComboBox);
      this.designSizeGroupBox.Controls.Add(this.designHeightTextBox);
      this.designSizeGroupBox.Controls.Add(this.designWidthTextBox);
      this.designSizeGroupBox.Controls.Add(this.designHeightLabel);
      this.designSizeGroupBox.Controls.Add(this.designWidthLabel);
      this.designSizeGroupBox.Location = new System.Drawing.Point(12, 12);
      this.designSizeGroupBox.Name = "designSizeGroupBox";
      this.designSizeGroupBox.Size = new System.Drawing.Size(760, 410);
      this.designSizeGroupBox.TabIndex = 10;
      this.designSizeGroupBox.TabStop = false;
      this.designSizeGroupBox.Text = "Design Size Adjustment";
      // 
      // sizeRoundingComboBox
      // 
      this.sizeRoundingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.sizeRoundingComboBox.FormattingEnabled = true;
      this.sizeRoundingComboBox.Items.AddRange(new object[] {
            "Round units up (make at least this size)",
            "Round units down (no larger than this)"});
      this.sizeRoundingComboBox.Location = new System.Drawing.Point(320, 360);
      this.sizeRoundingComboBox.Name = "sizeRoundingComboBox";
      this.sizeRoundingComboBox.Size = new System.Drawing.Size(420, 33);
      this.sizeRoundingComboBox.TabIndex = 24;
      // 
      // designSizeUnitsLabel
      // 
      this.designSizeUnitsLabel.AutoSize = true;
      this.designSizeUnitsLabel.Location = new System.Drawing.Point(450, 38);
      this.designSizeUnitsLabel.Name = "designSizeUnitsLabel";
      this.designSizeUnitsLabel.Size = new System.Drawing.Size(67, 25);
      this.designSizeUnitsLabel.TabIndex = 23;
      this.designSizeUnitsLabel.Text = "Units:";
      // 
      // heightUnitsLabel
      // 
      this.heightUnitsLabel.AutoSize = true;
      this.heightUnitsLabel.Location = new System.Drawing.Point(400, 191);
      this.heightUnitsLabel.Name = "heightUnitsLabel";
      this.heightUnitsLabel.Size = new System.Drawing.Size(58, 25);
      this.heightUnitsLabel.TabIndex = 22;
      this.heightUnitsLabel.Text = "units";
      // 
      // widthUnitsLabel
      // 
      this.widthUnitsLabel.AutoSize = true;
      this.widthUnitsLabel.Location = new System.Drawing.Point(400, 150);
      this.widthUnitsLabel.Name = "widthUnitsLabel";
      this.widthUnitsLabel.Size = new System.Drawing.Size(58, 25);
      this.widthUnitsLabel.TabIndex = 21;
      this.widthUnitsLabel.Text = "units";
      // 
      // rightEdgeLabel
      // 
      this.rightEdgeLabel.AutoSize = true;
      this.rightEdgeLabel.Location = new System.Drawing.Point(545, 210);
      this.rightEdgeLabel.Name = "rightEdgeLabel";
      this.rightEdgeLabel.Size = new System.Drawing.Size(108, 25);
      this.rightEdgeLabel.TabIndex = 20;
      this.rightEdgeLabel.Text = "right edge";
      // 
      // leftEdgeLabel
      // 
      this.leftEdgeLabel.AutoSize = true;
      this.leftEdgeLabel.Location = new System.Drawing.Point(20, 210);
      this.leftEdgeLabel.Name = "leftEdgeLabel";
      this.leftEdgeLabel.Size = new System.Drawing.Size(95, 25);
      this.leftEdgeLabel.TabIndex = 19;
      this.leftEdgeLabel.Text = "left edge";
      // 
      // bottomEdgeLabel
      // 
      this.bottomEdgeLabel.AutoSize = true;
      this.bottomEdgeLabel.Location = new System.Drawing.Point(265, 260);
      this.bottomEdgeLabel.Name = "bottomEdgeLabel";
      this.bottomEdgeLabel.Size = new System.Drawing.Size(131, 25);
      this.bottomEdgeLabel.TabIndex = 18;
      this.bottomEdgeLabel.Text = "bottom edge";
      // 
      // topEdgeLabel
      // 
      this.topEdgeLabel.AutoSize = true;
      this.topEdgeLabel.Location = new System.Drawing.Point(280, 75);
      this.topEdgeLabel.Name = "topEdgeLabel";
      this.topEdgeLabel.Size = new System.Drawing.Size(96, 25);
      this.topEdgeLabel.TabIndex = 17;
      this.topEdgeLabel.Text = "top edge";
      // 
      // leftEdgeNumericUpDown
      // 
      this.leftEdgeNumericUpDown.DecimalPlaces = 1;
      this.leftEdgeNumericUpDown.Location = new System.Drawing.Point(10, 165);
      this.leftEdgeNumericUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.leftEdgeNumericUpDown.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
      this.leftEdgeNumericUpDown.Name = "leftEdgeNumericUpDown";
      this.leftEdgeNumericUpDown.Size = new System.Drawing.Size(120, 31);
      this.leftEdgeNumericUpDown.TabIndex = 15;
      this.leftEdgeNumericUpDown.TextChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
      // 
      // rightEdgeNumericUpDown
      // 
      this.rightEdgeNumericUpDown.DecimalPlaces = 1;
      this.rightEdgeNumericUpDown.Location = new System.Drawing.Point(540, 165);
      this.rightEdgeNumericUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.rightEdgeNumericUpDown.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
      this.rightEdgeNumericUpDown.Name = "rightEdgeNumericUpDown";
      this.rightEdgeNumericUpDown.Size = new System.Drawing.Size(120, 31);
      this.rightEdgeNumericUpDown.TabIndex = 16;
      this.rightEdgeNumericUpDown.TextChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
      // 
      // topEdgeNumericUpDown
      // 
      this.topEdgeNumericUpDown.DecimalPlaces = 1;
      this.topEdgeNumericUpDown.Location = new System.Drawing.Point(270, 30);
      this.topEdgeNumericUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.topEdgeNumericUpDown.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
      this.topEdgeNumericUpDown.Name = "topEdgeNumericUpDown";
      this.topEdgeNumericUpDown.Size = new System.Drawing.Size(120, 31);
      this.topEdgeNumericUpDown.TabIndex = 13;
      this.topEdgeNumericUpDown.TextChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
      // 
      // bottomEdgeNumericUpDown
      // 
      this.bottomEdgeNumericUpDown.DecimalPlaces = 1;
      this.bottomEdgeNumericUpDown.Location = new System.Drawing.Point(270, 300);
      this.bottomEdgeNumericUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.bottomEdgeNumericUpDown.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
      this.bottomEdgeNumericUpDown.Name = "bottomEdgeNumericUpDown";
      this.bottomEdgeNumericUpDown.Size = new System.Drawing.Size(120, 31);
      this.bottomEdgeNumericUpDown.TabIndex = 14;
      this.bottomEdgeNumericUpDown.TextChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
      // 
      // cancelbutton
      // 
      this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelbutton.Location = new System.Drawing.Point(800, 90);
      this.cancelbutton.Name = "cancelbutton";
      this.cancelbutton.Size = new System.Drawing.Size(160, 50);
      this.cancelbutton.TabIndex = 12;
      this.cancelbutton.Text = "Cancel";
      this.cancelbutton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(800, 20);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(160, 50);
      this.okButton.TabIndex = 11;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // DesignSizeForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(984, 439);
      this.Controls.Add(this.cancelbutton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.designSizeGroupBox);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DesignSizeForm";
      this.Text = "Change Design Size";
      this.designSizeGroupBox.ResumeLayout(false);
      this.designSizeGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.leftEdgeNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.rightEdgeNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.topEdgeNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bottomEdgeNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.ComboBox designUnitsComboBox;
    private System.Windows.Forms.TextBox designHeightTextBox;
    private System.Windows.Forms.TextBox designWidthTextBox;
    private System.Windows.Forms.Label designHeightLabel;
    private System.Windows.Forms.Label designWidthLabel;
    private System.Windows.Forms.GroupBox designSizeGroupBox;
    private System.Windows.Forms.NumericUpDown leftEdgeNumericUpDown;
    private System.Windows.Forms.NumericUpDown rightEdgeNumericUpDown;
    private System.Windows.Forms.NumericUpDown topEdgeNumericUpDown;
    private System.Windows.Forms.NumericUpDown bottomEdgeNumericUpDown;
    private System.Windows.Forms.Button cancelbutton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label heightUnitsLabel;
    private System.Windows.Forms.Label widthUnitsLabel;
    private System.Windows.Forms.Label rightEdgeLabel;
    private System.Windows.Forms.Label leftEdgeLabel;
    private System.Windows.Forms.Label bottomEdgeLabel;
    private System.Windows.Forms.Label topEdgeLabel;
    private System.Windows.Forms.Label designSizeUnitsLabel;
    private System.Windows.Forms.ComboBox sizeRoundingComboBox;
  }
}