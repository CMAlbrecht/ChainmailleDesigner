// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorSelectionForm.Designer.cs


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
  partial class ColorSelectionForm
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
        hslMapGraphics?.Dispose();
        hslMapPen.Dispose();
        hslMapBrush.Dispose();
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSelectionForm));
      this.hslMapPictureBox = new System.Windows.Forms.PictureBox();
      this.hLabel = new System.Windows.Forms.Label();
      this.sLabel = new System.Windows.Forms.Label();
      this.lLabel = new System.Windows.Forms.Label();
      this.rLabel = new System.Windows.Forms.Label();
      this.gLabel = new System.Windows.Forms.Label();
      this.bLabel = new System.Windows.Forms.Label();
      this.hTextBox = new System.Windows.Forms.TextBox();
      this.sTextBox = new System.Windows.Forms.TextBox();
      this.lTextBox = new System.Windows.Forms.TextBox();
      this.rTextBox = new System.Windows.Forms.TextBox();
      this.gTextBox = new System.Windows.Forms.TextBox();
      this.bTextBox = new System.Windows.Forms.TextBox();
      this.hTrackBar = new System.Windows.Forms.TrackBar();
      this.sTrackBar = new System.Windows.Forms.TrackBar();
      this.lTrackBar = new System.Windows.Forms.TrackBar();
      this.rTrackBar = new System.Windows.Forms.TrackBar();
      this.gTrackBar = new System.Windows.Forms.TrackBar();
      this.bTrackBar = new System.Windows.Forms.TrackBar();
      this.gradientPictureBox = new System.Windows.Forms.PictureBox();
      this.selectedColorPictureBox = new System.Windows.Forms.PictureBox();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.colorSampleButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.hslMapPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.hTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.sTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.rTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.gTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bTrackBar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.gradientPictureBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.selectedColorPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // hslMapPictureBox
      // 
      this.hslMapPictureBox.Location = new System.Drawing.Point(10, 10);
      this.hslMapPictureBox.Name = "hslMapPictureBox";
      this.hslMapPictureBox.Size = new System.Drawing.Size(380, 380);
      this.hslMapPictureBox.TabIndex = 0;
      this.hslMapPictureBox.TabStop = false;
      this.hslMapPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hslMapPictureBox_MouseDown);
      this.hslMapPictureBox.MouseLeave += new System.EventHandler(this.hslMapPictureBox_MouseLeave);
      this.hslMapPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hslMapPictureBox_MouseMove);
      this.hslMapPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hslMapPictureBox_MouseUp);
      // 
      // hLabel
      // 
      this.hLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hLabel.Location = new System.Drawing.Point(440, 160);
      this.hLabel.Name = "hLabel";
      this.hLabel.Size = new System.Drawing.Size(35, 30);
      this.hLabel.TabIndex = 1;
      this.hLabel.Text = "H";
      this.hLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // sLabel
      // 
      this.sLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.sLabel.Location = new System.Drawing.Point(440, 200);
      this.sLabel.Name = "sLabel";
      this.sLabel.Size = new System.Drawing.Size(35, 30);
      this.sLabel.TabIndex = 2;
      this.sLabel.Text = "S";
      this.sLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lLabel
      // 
      this.lLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lLabel.Location = new System.Drawing.Point(440, 240);
      this.lLabel.Name = "lLabel";
      this.lLabel.Size = new System.Drawing.Size(35, 30);
      this.lLabel.TabIndex = 3;
      this.lLabel.Text = "L";
      this.lLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // rLabel
      // 
      this.rLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rLabel.Location = new System.Drawing.Point(440, 280);
      this.rLabel.Name = "rLabel";
      this.rLabel.Size = new System.Drawing.Size(35, 30);
      this.rLabel.TabIndex = 4;
      this.rLabel.Text = "R";
      this.rLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // gLabel
      // 
      this.gLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.gLabel.Location = new System.Drawing.Point(440, 320);
      this.gLabel.Name = "gLabel";
      this.gLabel.Size = new System.Drawing.Size(35, 30);
      this.gLabel.TabIndex = 5;
      this.gLabel.Text = "G";
      this.gLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // bLabel
      // 
      this.bLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.bLabel.Location = new System.Drawing.Point(440, 360);
      this.bLabel.Name = "bLabel";
      this.bLabel.Size = new System.Drawing.Size(35, 30);
      this.bLabel.TabIndex = 6;
      this.bLabel.Text = "B";
      this.bLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // hTextBox
      // 
      this.hTextBox.Location = new System.Drawing.Point(480, 160);
      this.hTextBox.Name = "hTextBox";
      this.hTextBox.Size = new System.Drawing.Size(60, 31);
      this.hTextBox.TabIndex = 7;
      this.hTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.hTextBox.TextChanged += new System.EventHandler(this.hslTextChanged);
      // 
      // sTextBox
      // 
      this.sTextBox.Location = new System.Drawing.Point(480, 200);
      this.sTextBox.Name = "sTextBox";
      this.sTextBox.Size = new System.Drawing.Size(60, 31);
      this.sTextBox.TabIndex = 8;
      this.sTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.sTextBox.TextChanged += new System.EventHandler(this.hslTextChanged);
      // 
      // lTextBox
      // 
      this.lTextBox.Location = new System.Drawing.Point(480, 240);
      this.lTextBox.Name = "lTextBox";
      this.lTextBox.Size = new System.Drawing.Size(60, 31);
      this.lTextBox.TabIndex = 9;
      this.lTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.lTextBox.TextChanged += new System.EventHandler(this.hslTextChanged);
      // 
      // rTextBox
      // 
      this.rTextBox.Location = new System.Drawing.Point(480, 280);
      this.rTextBox.Name = "rTextBox";
      this.rTextBox.Size = new System.Drawing.Size(60, 31);
      this.rTextBox.TabIndex = 10;
      this.rTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.rTextBox.TextChanged += new System.EventHandler(this.rgbTextChanged);
      // 
      // gTextBox
      // 
      this.gTextBox.Location = new System.Drawing.Point(480, 320);
      this.gTextBox.Name = "gTextBox";
      this.gTextBox.Size = new System.Drawing.Size(60, 31);
      this.gTextBox.TabIndex = 11;
      this.gTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.gTextBox.TextChanged += new System.EventHandler(this.rgbTextChanged);
      // 
      // bTextBox
      // 
      this.bTextBox.Location = new System.Drawing.Point(480, 360);
      this.bTextBox.Name = "bTextBox";
      this.bTextBox.Size = new System.Drawing.Size(60, 31);
      this.bTextBox.TabIndex = 12;
      this.bTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.bTextBox.TextChanged += new System.EventHandler(this.rgbTextChanged);
      // 
      // hTrackBar
      // 
      this.hTrackBar.AutoSize = false;
      this.hTrackBar.Location = new System.Drawing.Point(560, 160);
      this.hTrackBar.Maximum = 360;
      this.hTrackBar.Name = "hTrackBar";
      this.hTrackBar.Size = new System.Drawing.Size(120, 30);
      this.hTrackBar.TabIndex = 13;
      this.hTrackBar.TickFrequency = 20;
      this.hTrackBar.ValueChanged += new System.EventHandler(this.hslSliderChanged);
      // 
      // sTrackBar
      // 
      this.sTrackBar.AutoSize = false;
      this.sTrackBar.Location = new System.Drawing.Point(560, 200);
      this.sTrackBar.Maximum = 240;
      this.sTrackBar.Name = "sTrackBar";
      this.sTrackBar.Size = new System.Drawing.Size(120, 30);
      this.sTrackBar.TabIndex = 14;
      this.sTrackBar.TickFrequency = 20;
      this.sTrackBar.ValueChanged += new System.EventHandler(this.hslSliderChanged);
      // 
      // lTrackBar
      // 
      this.lTrackBar.AutoSize = false;
      this.lTrackBar.Location = new System.Drawing.Point(560, 240);
      this.lTrackBar.Maximum = 240;
      this.lTrackBar.Name = "lTrackBar";
      this.lTrackBar.Size = new System.Drawing.Size(120, 30);
      this.lTrackBar.TabIndex = 15;
      this.lTrackBar.TickFrequency = 20;
      this.lTrackBar.ValueChanged += new System.EventHandler(this.hslSliderChanged);
      // 
      // rTrackBar
      // 
      this.rTrackBar.AutoSize = false;
      this.rTrackBar.LargeChange = 8;
      this.rTrackBar.Location = new System.Drawing.Point(560, 280);
      this.rTrackBar.Maximum = 255;
      this.rTrackBar.Name = "rTrackBar";
      this.rTrackBar.Size = new System.Drawing.Size(120, 30);
      this.rTrackBar.TabIndex = 16;
      this.rTrackBar.TickFrequency = 16;
      this.rTrackBar.ValueChanged += new System.EventHandler(this.rgbSliderChanged);
      // 
      // gTrackBar
      // 
      this.gTrackBar.AutoSize = false;
      this.gTrackBar.LargeChange = 8;
      this.gTrackBar.Location = new System.Drawing.Point(560, 320);
      this.gTrackBar.Maximum = 255;
      this.gTrackBar.Name = "gTrackBar";
      this.gTrackBar.Size = new System.Drawing.Size(120, 30);
      this.gTrackBar.TabIndex = 17;
      this.gTrackBar.TickFrequency = 16;
      this.gTrackBar.ValueChanged += new System.EventHandler(this.rgbSliderChanged);
      // 
      // bTrackBar
      // 
      this.bTrackBar.AutoSize = false;
      this.bTrackBar.LargeChange = 8;
      this.bTrackBar.Location = new System.Drawing.Point(560, 360);
      this.bTrackBar.Maximum = 255;
      this.bTrackBar.Name = "bTrackBar";
      this.bTrackBar.Size = new System.Drawing.Size(120, 30);
      this.bTrackBar.TabIndex = 18;
      this.bTrackBar.TickFrequency = 16;
      this.bTrackBar.ValueChanged += new System.EventHandler(this.rgbSliderChanged);
      // 
      // gradientPictureBox
      // 
      this.gradientPictureBox.Location = new System.Drawing.Point(450, 20);
      this.gradientPictureBox.Name = "gradientPictureBox";
      this.gradientPictureBox.Size = new System.Drawing.Size(220, 110);
      this.gradientPictureBox.TabIndex = 19;
      this.gradientPictureBox.TabStop = false;
      // 
      // selectedColorPictureBox
      // 
      this.selectedColorPictureBox.Location = new System.Drawing.Point(450, 50);
      this.selectedColorPictureBox.Name = "selectedColorPictureBox";
      this.selectedColorPictureBox.Size = new System.Drawing.Size(220, 50);
      this.selectedColorPictureBox.TabIndex = 20;
      this.selectedColorPictureBox.TabStop = false;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(710, 20);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(150, 45);
      this.okButton.TabIndex = 21;
      this.okButton.Text = "OK";
      this.okButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(710, 85);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(150, 45);
      this.cancelButton.TabIndex = 22;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // colorSampleButton
      // 
      this.colorSampleButton.Location = new System.Drawing.Point(710, 340);
      this.colorSampleButton.Name = "colorSampleButton";
      this.colorSampleButton.Size = new System.Drawing.Size(150, 45);
      this.colorSampleButton.TabIndex = 23;
      this.colorSampleButton.Text = "Sample...";
      this.colorSampleButton.UseVisualStyleBackColor = true;
      this.colorSampleButton.Click += new System.EventHandler(this.colorSampleButton_Click);
      // 
      // ColorSelectionForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(886, 404);
      this.Controls.Add(this.colorSampleButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.selectedColorPictureBox);
      this.Controls.Add(this.gradientPictureBox);
      this.Controls.Add(this.bTrackBar);
      this.Controls.Add(this.gTrackBar);
      this.Controls.Add(this.rTrackBar);
      this.Controls.Add(this.lTrackBar);
      this.Controls.Add(this.sTrackBar);
      this.Controls.Add(this.hTrackBar);
      this.Controls.Add(this.bTextBox);
      this.Controls.Add(this.gTextBox);
      this.Controls.Add(this.rTextBox);
      this.Controls.Add(this.lTextBox);
      this.Controls.Add(this.sTextBox);
      this.Controls.Add(this.hTextBox);
      this.Controls.Add(this.bLabel);
      this.Controls.Add(this.gLabel);
      this.Controls.Add(this.rLabel);
      this.Controls.Add(this.lLabel);
      this.Controls.Add(this.sLabel);
      this.Controls.Add(this.hLabel);
      this.Controls.Add(this.hslMapPictureBox);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ColorSelectionForm";
      this.Text = "Choose a Color";
      ((System.ComponentModel.ISupportInitialize)(this.hslMapPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.hTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.sTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.rTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.gTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bTrackBar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.gradientPictureBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.selectedColorPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox hslMapPictureBox;
    private System.Windows.Forms.Label hLabel;
    private System.Windows.Forms.Label sLabel;
    private System.Windows.Forms.Label lLabel;
    private System.Windows.Forms.Label rLabel;
    private System.Windows.Forms.Label gLabel;
    private System.Windows.Forms.Label bLabel;
    private System.Windows.Forms.TextBox hTextBox;
    private System.Windows.Forms.TextBox sTextBox;
    private System.Windows.Forms.TextBox lTextBox;
    private System.Windows.Forms.TextBox rTextBox;
    private System.Windows.Forms.TextBox gTextBox;
    private System.Windows.Forms.TextBox bTextBox;
    private System.Windows.Forms.TrackBar hTrackBar;
    private System.Windows.Forms.TrackBar sTrackBar;
    private System.Windows.Forms.TrackBar lTrackBar;
    private System.Windows.Forms.TrackBar rTrackBar;
    private System.Windows.Forms.TrackBar gTrackBar;
    private System.Windows.Forms.TrackBar bTrackBar;
    private System.Windows.Forms.PictureBox gradientPictureBox;
    private System.Windows.Forms.PictureBox selectedColorPictureBox;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button colorSampleButton;
  }
}