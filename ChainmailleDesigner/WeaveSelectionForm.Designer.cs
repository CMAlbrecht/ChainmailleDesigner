// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: WeaveSelectionForm.Designer.cs


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
  partial class WeaveSelectionForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeaveSelectionForm));
      this.tabControl = new System.Windows.Forms.TabControl();
      this.weaveListTabPage = new System.Windows.Forms.TabPage();
      this.weaveListView = new System.Windows.Forms.ListView();
      this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.descriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ringsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.fileColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.weaveGalleryTabPage = new System.Windows.Forms.TabPage();
      this.weaveGalleryListView = new System.Windows.Forms.ListView();
      this.weaveGalleryNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.weaveGalleryDescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.weaveGalleryRingsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.weaveGalleryFileColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.cancelbutton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.tabControl.SuspendLayout();
      this.weaveListTabPage.SuspendLayout();
      this.weaveGalleryTabPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.weaveListTabPage);
      this.tabControl.Controls.Add(this.weaveGalleryTabPage);
      this.tabControl.Location = new System.Drawing.Point(10, 10);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(1020, 570);
      this.tabControl.TabIndex = 0;
      // 
      // weaveListTabPage
      // 
      this.weaveListTabPage.Controls.Add(this.weaveListView);
      this.weaveListTabPage.Location = new System.Drawing.Point(8, 39);
      this.weaveListTabPage.Name = "weaveListTabPage";
      this.weaveListTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.weaveListTabPage.Size = new System.Drawing.Size(1004, 523);
      this.weaveListTabPage.TabIndex = 0;
      this.weaveListTabPage.Text = "List";
      this.weaveListTabPage.UseVisualStyleBackColor = true;
      // 
      // weaveListView
      // 
      this.weaveListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.weaveListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.descriptionColumnHeader,
            this.ringsColumnHeader,
            this.fileColumnHeader});
      this.weaveListView.FullRowSelect = true;
      this.weaveListView.GridLines = true;
      this.weaveListView.Location = new System.Drawing.Point(0, 0);
      this.weaveListView.MultiSelect = false;
      this.weaveListView.Name = "weaveListView";
      this.weaveListView.Size = new System.Drawing.Size(1004, 523);
      this.weaveListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.weaveListView.TabIndex = 0;
      this.weaveListView.UseCompatibleStateImageBehavior = false;
      this.weaveListView.View = System.Windows.Forms.View.Details;
      this.weaveListView.DoubleClick += new System.EventHandler(this.weaveListView_DoubleClick);
      // 
      // nameColumnHeader
      // 
      this.nameColumnHeader.Text = "Name";
      this.nameColumnHeader.Width = 120;
      // 
      // descriptionColumnHeader
      // 
      this.descriptionColumnHeader.Text = "Description";
      this.descriptionColumnHeader.Width = 200;
      // 
      // ringsColumnHeader
      // 
      this.ringsColumnHeader.Text = "Ring Size(s)";
      this.ringsColumnHeader.Width = 200;
      // 
      // fileColumnHeader
      // 
      this.fileColumnHeader.Text = "File";
      this.fileColumnHeader.Width = 300;
      // 
      // weaveGalleryTabPage
      // 
      this.weaveGalleryTabPage.Controls.Add(this.weaveGalleryListView);
      this.weaveGalleryTabPage.Location = new System.Drawing.Point(8, 39);
      this.weaveGalleryTabPage.Name = "weaveGalleryTabPage";
      this.weaveGalleryTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.weaveGalleryTabPage.Size = new System.Drawing.Size(1004, 523);
      this.weaveGalleryTabPage.TabIndex = 1;
      this.weaveGalleryTabPage.Text = "Gallery";
      this.weaveGalleryTabPage.UseVisualStyleBackColor = true;
      // 
      // weaveGalleryListView
      // 
      this.weaveGalleryListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.weaveGalleryListView.Location = new System.Drawing.Point(0, 0);
      this.weaveGalleryListView.MultiSelect = false;
      this.weaveGalleryListView.Name = "weaveGalleryListView";
      this.weaveGalleryListView.Size = new System.Drawing.Size(1004, 523);
      this.weaveGalleryListView.TabIndex = 0;
      this.weaveGalleryListView.UseCompatibleStateImageBehavior = false;
      this.weaveGalleryListView.View = System.Windows.Forms.View.Tile;
      this.weaveGalleryListView.DoubleClick += new System.EventHandler(this.weaveListView_DoubleClick);
      // 
      // weaveGalleryNameColumnHeader
      // 
      this.weaveGalleryNameColumnHeader.Name = "weaveGalleryNameColumnHeader";
      this.weaveGalleryNameColumnHeader.Text = "Name";
      this.weaveGalleryNameColumnHeader.Width = 900;
      // 
      // weaveGalleryDescriptionColumnHeader
      // 
      this.weaveGalleryDescriptionColumnHeader.Name = "weaveGalleryDescriptionColumnHeader";
      this.weaveGalleryDescriptionColumnHeader.Text = "Description";
      this.weaveGalleryDescriptionColumnHeader.Width = 900;
      // 
      // weaveGalleryRingsColumnHeader
      // 
      this.weaveGalleryRingsColumnHeader.Name = "weaveGalleryRingsColumnHeader";
      this.weaveGalleryRingsColumnHeader.Text = "Ring Size(s)";
      this.weaveGalleryRingsColumnHeader.Width = 900;
      // 
      // weaveGalleryFileColumnHeader
      // 
      this.weaveGalleryFileColumnHeader.Name = "weaveGalleryFileColumnHeader";
      this.weaveGalleryFileColumnHeader.Text = "File";
      this.weaveGalleryFileColumnHeader.Width = 900;
      // 
      // cancelbutton
      // 
      this.cancelbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelbutton.Location = new System.Drawing.Point(1070, 80);
      this.cancelbutton.Name = "cancelbutton";
      this.cancelbutton.Size = new System.Drawing.Size(160, 50);
      this.cancelbutton.TabIndex = 10;
      this.cancelbutton.Text = "Cancel";
      this.cancelbutton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(1070, 10);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(160, 50);
      this.okButton.TabIndex = 9;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // WeaveSelectionForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1255, 595);
      this.Controls.Add(this.cancelbutton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.tabControl);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "WeaveSelectionForm";
      this.Text = "Choose a Weave";
      this.tabControl.ResumeLayout(false);
      this.weaveListTabPage.ResumeLayout(false);
      this.weaveGalleryTabPage.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage weaveListTabPage;
    private System.Windows.Forms.TabPage weaveGalleryTabPage;
    private System.Windows.Forms.Button cancelbutton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.ListView weaveListView;
    private System.Windows.Forms.ColumnHeader nameColumnHeader;
    private System.Windows.Forms.ColumnHeader descriptionColumnHeader;
    private System.Windows.Forms.ColumnHeader ringsColumnHeader;
    private System.Windows.Forms.ColumnHeader fileColumnHeader;
    private System.Windows.Forms.ListView weaveGalleryListView;
    private System.Windows.Forms.ColumnHeader weaveGalleryNameColumnHeader;
    private System.Windows.Forms.ColumnHeader weaveGalleryDescriptionColumnHeader;
    private System.Windows.Forms.ColumnHeader weaveGalleryRingsColumnHeader;
    private System.Windows.Forms.ColumnHeader weaveGalleryFileColumnHeader;
  }
}