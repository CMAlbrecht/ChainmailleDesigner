// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorSampleVerificationForm.Designer.cs


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
  partial class ColorSampleVerificationForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSampleVerificationForm));
      this.verificationLabel = new System.Windows.Forms.Label();
      this.colorPanel = new System.Windows.Forms.Panel();
      this.yesButton = new System.Windows.Forms.Button();
      this.noButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // verificationLabel
      // 
      this.verificationLabel.AutoSize = true;
      this.verificationLabel.Location = new System.Drawing.Point(20, 25);
      this.verificationLabel.Name = "verificationLabel";
      this.verificationLabel.Size = new System.Drawing.Size(261, 25);
      this.verificationLabel.TabIndex = 0;
      this.verificationLabel.Text = "Is this the color you want?";
      // 
      // colorPanel
      // 
      this.colorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.colorPanel.Location = new System.Drawing.Point(20, 65);
      this.colorPanel.Name = "colorPanel";
      this.colorPanel.Size = new System.Drawing.Size(265, 75);
      this.colorPanel.TabIndex = 1;
      // 
      // yesButton
      // 
      this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
      this.yesButton.Location = new System.Drawing.Point(300, 25);
      this.yesButton.Name = "yesButton";
      this.yesButton.Size = new System.Drawing.Size(160, 45);
      this.yesButton.TabIndex = 2;
      this.yesButton.Text = "Yes";
      this.yesButton.UseVisualStyleBackColor = true;
      // 
      // noButton
      // 
      this.noButton.DialogResult = System.Windows.Forms.DialogResult.No;
      this.noButton.Location = new System.Drawing.Point(300, 90);
      this.noButton.Name = "noButton";
      this.noButton.Size = new System.Drawing.Size(160, 45);
      this.noButton.TabIndex = 3;
      this.noButton.Text = "No";
      this.noButton.UseVisualStyleBackColor = true;
      // 
      // ColorSampleVerificationForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(480, 156);
      this.Controls.Add(this.noButton);
      this.Controls.Add(this.yesButton);
      this.Controls.Add(this.colorPanel);
      this.Controls.Add(this.verificationLabel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ColorSampleVerificationForm";
      this.Text = "Confirm Selection";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label verificationLabel;
    private System.Windows.Forms.Panel colorPanel;
    private System.Windows.Forms.Button yesButton;
    private System.Windows.Forms.Button noButton;
  }
}