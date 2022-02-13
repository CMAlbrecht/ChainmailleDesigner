// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorSamplingForm.Designer.cs


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
  partial class ColorSamplingForm
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
      if (disposing)
      {
        components?.Dispose();
        sourceImage?.Dispose();
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSamplingForm));
      this.imagePanel = new System.Windows.Forms.Panel();
      this.imagePictureBox = new System.Windows.Forms.PictureBox();
      this.imagePanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // imagePanel
      // 
      this.imagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.imagePanel.AutoScroll = true;
      this.imagePanel.Controls.Add(this.imagePictureBox);
      this.imagePanel.Location = new System.Drawing.Point(1, 2);
      this.imagePanel.Name = "imagePanel";
      this.imagePanel.Size = new System.Drawing.Size(760, 486);
      this.imagePanel.TabIndex = 0;
      // 
      // imagePictureBox
      // 
      this.imagePictureBox.Location = new System.Drawing.Point(3, 3);
      this.imagePictureBox.Name = "imagePictureBox";
      this.imagePictureBox.Size = new System.Drawing.Size(714, 442);
      this.imagePictureBox.TabIndex = 0;
      this.imagePictureBox.TabStop = false;
      this.imagePictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImagePictureBox_MouseClick);
      this.imagePictureBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ImagePictureBox_MouseWheel);
      // 
      // ColorSamplingForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(758, 487);
      this.Controls.Add(this.imagePanel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ColorSamplingForm";
      this.Text = "Select a Color Sample";
      this.imagePanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel imagePanel;
    private System.Windows.Forms.PictureBox imagePictureBox;
  }
}