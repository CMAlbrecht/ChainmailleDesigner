// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmailleDesignerForm.Designer.cs


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
  partial class ChainmailleDesignerForm
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
        blankingBrush.Dispose();
        guidelinePen.Dispose();
        rubberBandPen.Dispose();
        chainmailleDesign?.Dispose();
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChainmailleDesignerForm));
      this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.fileCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveDesignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveDesignAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveRenderedImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
      this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.printRenderedImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.designToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.guidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showHideGuidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.horizontalGuidelineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.verticalGuidelineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
      this.halvesGuidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.thirdsGuidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.quartersGuidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
      this.clearAllGuidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.designOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.overlaySeparatorToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
      this.showHideOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.rotateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.clockwiseOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.counterclockwiseOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clockwiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.counterclockwiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.colorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.countColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.replaceInDesignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.colorRingsFromOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
      this.clearAllRingColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.paletteWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpAboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.licenseAgreementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.renderedImagePictureBox = new System.Windows.Forms.PictureBox();
      this.renderedImagePanel = new System.Windows.Forms.Panel();
      this.zoomedImageVerticalScrollBar = new System.Windows.Forms.VScrollBar();
      this.zoomedImageHorizontalScrollBar = new System.Windows.Forms.HScrollBar();
      this.zoomLabel = new System.Windows.Forms.Label();
      this.zoomTextBox = new System.Windows.Forms.TextBox();
      this.zoomResetButton = new System.Windows.Forms.Button();
      this.zoomControlPanel = new System.Windows.Forms.Panel();
      this.zoomFitButton = new System.Windows.Forms.Button();
      this.patternElementIdLabel = new System.Windows.Forms.Label();
      this.zoomPercentLabel = new System.Windows.Forms.Label();
      this.zoomTrackBar = new System.Windows.Forms.TrackBar();
      this.ringFilterGroupBox = new System.Windows.Forms.GroupBox();
      this.ringFilterRadioButton0 = new System.Windows.Forms.RadioButton();
      this.shapesGroupBox = new System.Windows.Forms.GroupBox();
      this.lineShapeRadioButton = new System.Windows.Forms.RadioButton();
      this.polygonShapeRadioButton = new System.Windows.Forms.RadioButton();
      this.ellipseShapeRadioButton = new System.Windows.Forms.RadioButton();
      this.rectangleShapeRadioButton = new System.Windows.Forms.RadioButton();
      this.freehandShapeRadioButton = new System.Windows.Forms.RadioButton();
      this.fillGroupBox = new System.Windows.Forms.GroupBox();
      this.hollowFillRadioButton = new System.Windows.Forms.RadioButton();
      this.solidFillRadioButton = new System.Windows.Forms.RadioButton();
      this.shapeProgressBar = new System.Windows.Forms.ProgressBar();
      this.overlayGroupBox = new System.Windows.Forms.GroupBox();
      this.overlayOpacityLabel = new System.Windows.Forms.Label();
      this.overlayOpacityTrackBar = new System.Windows.Forms.TrackBar();
      this.overlayCheckBox = new System.Windows.Forms.CheckBox();
      this.drawingToolPanel = new System.Windows.Forms.Panel();
      this.mainMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.renderedImagePictureBox)).BeginInit();
      this.renderedImagePanel.SuspendLayout();
      this.zoomControlPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).BeginInit();
      this.ringFilterGroupBox.SuspendLayout();
      this.shapesGroupBox.SuspendLayout();
      this.fillGroupBox.SuspendLayout();
      this.overlayGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.overlayOpacityTrackBar)).BeginInit();
      this.drawingToolPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainMenuStrip
      // 
      this.mainMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.designToolStripMenuItem,
            this.colorsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
      this.mainMenuStrip.Name = "mainMenuStrip";
      this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
      this.mainMenuStrip.Size = new System.Drawing.Size(800, 24);
      this.mainMenuStrip.TabIndex = 0;
      this.mainMenuStrip.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNewToolStripMenuItem,
            this.fileOpenToolStripMenuItem,
            this.toolStripMenuItem1,
            this.fileCloseToolStripMenuItem,
            this.saveDesignToolStripMenuItem,
            this.saveDesignAsToolStripMenuItem,
            this.saveRenderedImageToolStripMenuItem,
            this.toolStripMenuItem3,
            this.printToolStripMenuItem,
            this.toolStripMenuItem4,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // fileNewToolStripMenuItem
      // 
      this.fileNewToolStripMenuItem.Name = "fileNewToolStripMenuItem";
      this.fileNewToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.fileNewToolStripMenuItem.Text = "New...";
      this.fileNewToolStripMenuItem.Click += new System.EventHandler(this.fileNewToolStripMenuItem_Click);
      // 
      // fileOpenToolStripMenuItem
      // 
      this.fileOpenToolStripMenuItem.Name = "fileOpenToolStripMenuItem";
      this.fileOpenToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.fileOpenToolStripMenuItem.Text = "Open...";
      this.fileOpenToolStripMenuItem.Click += new System.EventHandler(this.fileOpenToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(193, 6);
      // 
      // fileCloseToolStripMenuItem
      // 
      this.fileCloseToolStripMenuItem.Name = "fileCloseToolStripMenuItem";
      this.fileCloseToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.fileCloseToolStripMenuItem.Text = "Close";
      this.fileCloseToolStripMenuItem.Click += new System.EventHandler(this.fileCloseToolStripMenuItem_Click);
      // 
      // saveDesignToolStripMenuItem
      // 
      this.saveDesignToolStripMenuItem.Name = "saveDesignToolStripMenuItem";
      this.saveDesignToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.saveDesignToolStripMenuItem.Text = "Save Design";
      this.saveDesignToolStripMenuItem.Click += new System.EventHandler(this.saveDesignToolStripMenuItem_Click);
      // 
      // saveDesignAsToolStripMenuItem
      // 
      this.saveDesignAsToolStripMenuItem.Name = "saveDesignAsToolStripMenuItem";
      this.saveDesignAsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.saveDesignAsToolStripMenuItem.Text = "Save Design As....";
      this.saveDesignAsToolStripMenuItem.Click += new System.EventHandler(this.saveDesignAsToolStripMenuItem_Click);
      // 
      // saveRenderedImageToolStripMenuItem
      // 
      this.saveRenderedImageToolStripMenuItem.Name = "saveRenderedImageToolStripMenuItem";
      this.saveRenderedImageToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.saveRenderedImageToolStripMenuItem.Text = "Save Rendered Image...";
      this.saveRenderedImageToolStripMenuItem.Click += new System.EventHandler(this.saveRenderedImageToolStripMenuItem_Click);
      // 
      // toolStripMenuItem3
      // 
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new System.Drawing.Size(193, 6);
      // 
      // printToolStripMenuItem
      // 
      this.printToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printRenderedImageToolStripMenuItem});
      this.printToolStripMenuItem.Name = "printToolStripMenuItem";
      this.printToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.printToolStripMenuItem.Text = "Print";
      // 
      // printRenderedImageToolStripMenuItem
      // 
      this.printRenderedImageToolStripMenuItem.Name = "printRenderedImageToolStripMenuItem";
      this.printRenderedImageToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.printRenderedImageToolStripMenuItem.Text = "Rendered Image...";
      this.printRenderedImageToolStripMenuItem.Click += new System.EventHandler(this.printRenderedImageToolStripMenuItem_Click);
      // 
      // toolStripMenuItem4
      // 
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      this.toolStripMenuItem4.Size = new System.Drawing.Size(193, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // designToolStripMenuItem
      // 
      this.designToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.guidelinesToolStripMenuItem,
            this.designOverlayToolStripMenuItem,
            this.rotateToolStripMenuItem});
      this.designToolStripMenuItem.Name = "designToolStripMenuItem";
      this.designToolStripMenuItem.Size = new System.Drawing.Size(55, 22);
      this.designToolStripMenuItem.Text = "Design";
      // 
      // infoToolStripMenuItem
      // 
      this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
      this.infoToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.infoToolStripMenuItem.Text = "Info...";
      this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
      // 
      // guidelinesToolStripMenuItem
      // 
      this.guidelinesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHideGuidelinesToolStripMenuItem,
            this.toolStripMenuItem2,
            this.horizontalGuidelineToolStripMenuItem,
            this.verticalGuidelineToolStripMenuItem,
            this.toolStripMenuItem5,
            this.halvesGuidelinesToolStripMenuItem,
            this.thirdsGuidelinesToolStripMenuItem,
            this.quartersGuidelinesToolStripMenuItem,
            this.toolStripMenuItem6,
            this.clearAllGuidelinesToolStripMenuItem});
      this.guidelinesToolStripMenuItem.Name = "guidelinesToolStripMenuItem";
      this.guidelinesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.guidelinesToolStripMenuItem.Text = "Guidelines";
      // 
      // showHideGuidelinesToolStripMenuItem
      // 
      this.showHideGuidelinesToolStripMenuItem.Name = "showHideGuidelinesToolStripMenuItem";
      this.showHideGuidelinesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.showHideGuidelinesToolStripMenuItem.Text = "Hide";
      this.showHideGuidelinesToolStripMenuItem.Click += new System.EventHandler(this.showHideGuidelinesToolStripMenuItem_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(126, 6);
      // 
      // horizontalGuidelineToolStripMenuItem
      // 
      this.horizontalGuidelineToolStripMenuItem.Name = "horizontalGuidelineToolStripMenuItem";
      this.horizontalGuidelineToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.horizontalGuidelineToolStripMenuItem.Text = "Horizontal";
      this.horizontalGuidelineToolStripMenuItem.Click += new System.EventHandler(this.horizontalGuidelineToolStripMenuItem_Click);
      // 
      // verticalGuidelineToolStripMenuItem
      // 
      this.verticalGuidelineToolStripMenuItem.Name = "verticalGuidelineToolStripMenuItem";
      this.verticalGuidelineToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.verticalGuidelineToolStripMenuItem.Text = "Vertical";
      this.verticalGuidelineToolStripMenuItem.Click += new System.EventHandler(this.verticalGuidelineToolStripMenuItem_Click);
      // 
      // toolStripMenuItem5
      // 
      this.toolStripMenuItem5.Name = "toolStripMenuItem5";
      this.toolStripMenuItem5.Size = new System.Drawing.Size(126, 6);
      // 
      // halvesGuidelinesToolStripMenuItem
      // 
      this.halvesGuidelinesToolStripMenuItem.Name = "halvesGuidelinesToolStripMenuItem";
      this.halvesGuidelinesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.halvesGuidelinesToolStripMenuItem.Text = "Halves";
      this.halvesGuidelinesToolStripMenuItem.Click += new System.EventHandler(this.halvesGuidelinesToolStripMenuItem_Click);
      // 
      // thirdsGuidelinesToolStripMenuItem
      // 
      this.thirdsGuidelinesToolStripMenuItem.Name = "thirdsGuidelinesToolStripMenuItem";
      this.thirdsGuidelinesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.thirdsGuidelinesToolStripMenuItem.Text = "Thirds";
      this.thirdsGuidelinesToolStripMenuItem.Click += new System.EventHandler(this.thirdsGuidelinesToolStripMenuItem_Click);
      // 
      // quartersGuidelinesToolStripMenuItem
      // 
      this.quartersGuidelinesToolStripMenuItem.Name = "quartersGuidelinesToolStripMenuItem";
      this.quartersGuidelinesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.quartersGuidelinesToolStripMenuItem.Text = "Quarters";
      this.quartersGuidelinesToolStripMenuItem.Click += new System.EventHandler(this.quartersGuidelinesToolStripMenuItem_Click);
      // 
      // toolStripMenuItem6
      // 
      this.toolStripMenuItem6.Name = "toolStripMenuItem6";
      this.toolStripMenuItem6.Size = new System.Drawing.Size(126, 6);
      // 
      // clearAllGuidelinesToolStripMenuItem
      // 
      this.clearAllGuidelinesToolStripMenuItem.Name = "clearAllGuidelinesToolStripMenuItem";
      this.clearAllGuidelinesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.clearAllGuidelinesToolStripMenuItem.Text = "Clear All";
      this.clearAllGuidelinesToolStripMenuItem.Click += new System.EventHandler(this.clearAllGuidelinesToolStripMenuItem_Click);
      // 
      // designOverlayToolStripMenuItem
      // 
      this.designOverlayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOverlayToolStripMenuItem,
            this.overlaySeparatorToolStripMenuItem,
            this.showHideOverlayToolStripMenuItem,
            this.rotateToolStripMenuItem1});
      this.designOverlayToolStripMenuItem.Name = "designOverlayToolStripMenuItem";
      this.designOverlayToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.designOverlayToolStripMenuItem.Text = "Overlay";
      // 
      // openOverlayToolStripMenuItem
      // 
      this.openOverlayToolStripMenuItem.Name = "openOverlayToolStripMenuItem";
      this.openOverlayToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.openOverlayToolStripMenuItem.Text = "Open...";
      this.openOverlayToolStripMenuItem.Click += new System.EventHandler(this.openOverlayToolStripMenuItem_Click);
      // 
      // overlaySeparatorToolStripMenuItem
      // 
      this.overlaySeparatorToolStripMenuItem.Name = "overlaySeparatorToolStripMenuItem";
      this.overlaySeparatorToolStripMenuItem.Size = new System.Drawing.Size(135, 6);
      // 
      // showHideOverlayToolStripMenuItem
      // 
      this.showHideOverlayToolStripMenuItem.Name = "showHideOverlayToolStripMenuItem";
      this.showHideOverlayToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
      this.showHideOverlayToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.showHideOverlayToolStripMenuItem.Text = "Hide";
      this.showHideOverlayToolStripMenuItem.Click += new System.EventHandler(this.showHideOverlayToolStripMenuItem_Click);
      // 
      // rotateToolStripMenuItem1
      // 
      this.rotateToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockwiseOverlayToolStripMenuItem,
            this.counterclockwiseOverlayToolStripMenuItem});
      this.rotateToolStripMenuItem1.Name = "rotateToolStripMenuItem1";
      this.rotateToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
      this.rotateToolStripMenuItem1.Text = "Rotate";
      // 
      // clockwiseOverlayToolStripMenuItem
      // 
      this.clockwiseOverlayToolStripMenuItem.Name = "clockwiseOverlayToolStripMenuItem";
      this.clockwiseOverlayToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
      this.clockwiseOverlayToolStripMenuItem.Text = "Clockwise";
      this.clockwiseOverlayToolStripMenuItem.Click += new System.EventHandler(this.clockwiseOverlayToolStripMenuItem_Click);
      // 
      // counterclockwiseOverlayToolStripMenuItem
      // 
      this.counterclockwiseOverlayToolStripMenuItem.Name = "counterclockwiseOverlayToolStripMenuItem";
      this.counterclockwiseOverlayToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
      this.counterclockwiseOverlayToolStripMenuItem.Text = "Counterclockwise";
      this.counterclockwiseOverlayToolStripMenuItem.Click += new System.EventHandler(this.counterclockwiseOverlayToolStripMenuItem_Click);
      // 
      // rotateToolStripMenuItem
      // 
      this.rotateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockwiseToolStripMenuItem,
            this.counterclockwiseToolStripMenuItem});
      this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
      this.rotateToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.rotateToolStripMenuItem.Text = "Rotate";
      // 
      // clockwiseToolStripMenuItem
      // 
      this.clockwiseToolStripMenuItem.Name = "clockwiseToolStripMenuItem";
      this.clockwiseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
      this.clockwiseToolStripMenuItem.Text = "Clockwise";
      this.clockwiseToolStripMenuItem.Click += new System.EventHandler(this.clockwiseToolStripMenuItem_Click);
      // 
      // counterclockwiseToolStripMenuItem
      // 
      this.counterclockwiseToolStripMenuItem.Name = "counterclockwiseToolStripMenuItem";
      this.counterclockwiseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
      this.counterclockwiseToolStripMenuItem.Text = "Counterclockwise";
      this.counterclockwiseToolStripMenuItem.Click += new System.EventHandler(this.counterclockwiseToolStripMenuItem_Click);
      // 
      // colorsToolStripMenuItem
      // 
      this.colorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.countColorsToolStripMenuItem,
            this.replaceInDesignToolStripMenuItem,
            this.colorRingsFromOverlayToolStripMenuItem,
            this.toolStripMenuItem7,
            this.clearAllRingColorsToolStripMenuItem});
      this.colorsToolStripMenuItem.Name = "colorsToolStripMenuItem";
      this.colorsToolStripMenuItem.Size = new System.Drawing.Size(53, 22);
      this.colorsToolStripMenuItem.Text = "Colors";
      // 
      // countColorsToolStripMenuItem
      // 
      this.countColorsToolStripMenuItem.Name = "countColorsToolStripMenuItem";
      this.countColorsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
      this.countColorsToolStripMenuItem.Text = "Count Colors...";
      this.countColorsToolStripMenuItem.Click += new System.EventHandler(this.countColorsToolStripMenuItem_Click);
      // 
      // replaceInDesignToolStripMenuItem
      // 
      this.replaceInDesignToolStripMenuItem.Name = "replaceInDesignToolStripMenuItem";
      this.replaceInDesignToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
      this.replaceInDesignToolStripMenuItem.Text = "Replace in Design...";
      this.replaceInDesignToolStripMenuItem.Click += new System.EventHandler(this.replaceInDesignToolStripMenuItem_Click);
      // 
      // colorRingsFromOverlayToolStripMenuItem
      // 
      this.colorRingsFromOverlayToolStripMenuItem.Name = "colorRingsFromOverlayToolStripMenuItem";
      this.colorRingsFromOverlayToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
      this.colorRingsFromOverlayToolStripMenuItem.Text = "Color Rings from Overlay...";
      this.colorRingsFromOverlayToolStripMenuItem.Click += new System.EventHandler(this.colorRingsFromOverlayToolStripMenuItem_Click);
      // 
      // toolStripMenuItem7
      // 
      this.toolStripMenuItem7.Name = "toolStripMenuItem7";
      this.toolStripMenuItem7.Size = new System.Drawing.Size(213, 6);
      // 
      // clearAllRingColorsToolStripMenuItem
      // 
      this.clearAllRingColorsToolStripMenuItem.Name = "clearAllRingColorsToolStripMenuItem";
      this.clearAllRingColorsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
      this.clearAllRingColorsToolStripMenuItem.Text = "Clear All Ring Colors";
      this.clearAllRingColorsToolStripMenuItem.Click += new System.EventHandler(this.clearAllRingColorsToolStripMenuItem_Click);
      // 
      // windowToolStripMenuItem
      // 
      this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paletteWindowToolStripMenuItem});
      this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
      this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 22);
      this.windowToolStripMenuItem.Text = "Window";
      // 
      // paletteWindowToolStripMenuItem
      // 
      this.paletteWindowToolStripMenuItem.Name = "paletteWindowToolStripMenuItem";
      this.paletteWindowToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
      this.paletteWindowToolStripMenuItem.Text = "Palette";
      this.paletteWindowToolStripMenuItem.Click += new System.EventHandler(this.paletteWindowToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpAboutToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.showHelpToolStripMenuItem,
            this.licenseAgreementToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // helpAboutToolStripMenuItem
      // 
      this.helpAboutToolStripMenuItem.Name = "helpAboutToolStripMenuItem";
      this.helpAboutToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
      this.helpAboutToolStripMenuItem.Text = "About Chainmaille Designer...";
      this.helpAboutToolStripMenuItem.Click += new System.EventHandler(this.helpAboutToolStripMenuItem_Click);
      // 
      // configurationToolStripMenuItem
      // 
      this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
      this.configurationToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
      this.configurationToolStripMenuItem.Text = "Configuration...";
      this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
      // 
      // showHelpToolStripMenuItem
      // 
      this.showHelpToolStripMenuItem.Name = "showHelpToolStripMenuItem";
      this.showHelpToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
      this.showHelpToolStripMenuItem.Text = "Show Help...";
      this.showHelpToolStripMenuItem.Click += new System.EventHandler(this.showHelpToolStripMenuItem_Click);
      // 
      // licenseAgreementToolStripMenuItem
      // 
      this.licenseAgreementToolStripMenuItem.Name = "licenseAgreementToolStripMenuItem";
      this.licenseAgreementToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
      this.licenseAgreementToolStripMenuItem.Text = "License Agreement...";
      this.licenseAgreementToolStripMenuItem.Click += new System.EventHandler(this.licenseAgreementToolStripMenuItem_Click);
      // 
      // renderedImagePictureBox
      // 
      this.renderedImagePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.renderedImagePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.renderedImagePictureBox.Location = new System.Drawing.Point(4, 4);
      this.renderedImagePictureBox.Margin = new System.Windows.Forms.Padding(2);
      this.renderedImagePictureBox.Name = "renderedImagePictureBox";
      this.renderedImagePictureBox.Size = new System.Drawing.Size(689, 340);
      this.renderedImagePictureBox.TabIndex = 1;
      this.renderedImagePictureBox.TabStop = false;
      this.renderedImagePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.renderedImagePictureBox_MouseDown);
      this.renderedImagePictureBox.MouseLeave += new System.EventHandler(this.renderedImagePictureBox_MouseLeave);
      this.renderedImagePictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderedImagePictureBox_MouseMove);
      this.renderedImagePictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.renderedImagePictureBox_MouseUp);
      this.renderedImagePictureBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.renderedImagePictureBox_MouseWheel);
      // 
      // renderedImagePanel
      // 
      this.renderedImagePanel.AutoScroll = true;
      this.renderedImagePanel.Controls.Add(this.zoomedImageVerticalScrollBar);
      this.renderedImagePanel.Controls.Add(this.zoomedImageHorizontalScrollBar);
      this.renderedImagePanel.Controls.Add(this.renderedImagePictureBox);
      this.renderedImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.renderedImagePanel.Location = new System.Drawing.Point(0, 24);
      this.renderedImagePanel.Margin = new System.Windows.Forms.Padding(2);
      this.renderedImagePanel.Name = "renderedImagePanel";
      this.renderedImagePanel.Size = new System.Drawing.Size(710, 365);
      this.renderedImagePanel.TabIndex = 2;
      // 
      // zoomedImageVerticalScrollBar
      // 
      this.zoomedImageVerticalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomedImageVerticalScrollBar.Location = new System.Drawing.Point(693, 4);
      this.zoomedImageVerticalScrollBar.Name = "zoomedImageVerticalScrollBar";
      this.zoomedImageVerticalScrollBar.Size = new System.Drawing.Size(25, 340);
      this.zoomedImageVerticalScrollBar.TabIndex = 3;
      this.zoomedImageVerticalScrollBar.Visible = false;
      this.zoomedImageVerticalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.zoomedImageVerticalScrollBar_Scroll);
      // 
      // zoomedImageHorizontalScrollBar
      // 
      this.zoomedImageHorizontalScrollBar.AllowDrop = true;
      this.zoomedImageHorizontalScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomedImageHorizontalScrollBar.Location = new System.Drawing.Point(0, 345);
      this.zoomedImageHorizontalScrollBar.Name = "zoomedImageHorizontalScrollBar";
      this.zoomedImageHorizontalScrollBar.Size = new System.Drawing.Size(688, 25);
      this.zoomedImageHorizontalScrollBar.TabIndex = 2;
      this.zoomedImageHorizontalScrollBar.Visible = false;
      this.zoomedImageHorizontalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.zoomedImageHorizontalScrollBar_Scroll);
      // 
      // zoomLabel
      // 
      this.zoomLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomLabel.AutoSize = true;
      this.zoomLabel.Location = new System.Drawing.Point(400, 6);
      this.zoomLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.zoomLabel.Name = "zoomLabel";
      this.zoomLabel.Size = new System.Drawing.Size(34, 13);
      this.zoomLabel.TabIndex = 1;
      this.zoomLabel.Text = "Zoom";
      // 
      // zoomTextBox
      // 
      this.zoomTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomTextBox.Location = new System.Drawing.Point(539, 3);
      this.zoomTextBox.Margin = new System.Windows.Forms.Padding(2);
      this.zoomTextBox.Name = "zoomTextBox";
      this.zoomTextBox.Size = new System.Drawing.Size(48, 20);
      this.zoomTextBox.TabIndex = 3;
      this.zoomTextBox.Text = "100";
      this.zoomTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.zoomTextBox.TextChanged += new System.EventHandler(this.zoomTextBox_TextChanged);
      // 
      // zoomResetButton
      // 
      this.zoomResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomResetButton.Location = new System.Drawing.Point(606, 3);
      this.zoomResetButton.Margin = new System.Windows.Forms.Padding(2);
      this.zoomResetButton.Name = "zoomResetButton";
      this.zoomResetButton.Size = new System.Drawing.Size(50, 21);
      this.zoomResetButton.TabIndex = 5;
      this.zoomResetButton.Text = "100%";
      this.zoomResetButton.UseVisualStyleBackColor = true;
      this.zoomResetButton.Click += new System.EventHandler(this.zoomResetButton_Click);
      // 
      // zoomControlPanel
      // 
      this.zoomControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.zoomControlPanel.Controls.Add(this.zoomFitButton);
      this.zoomControlPanel.Controls.Add(this.patternElementIdLabel);
      this.zoomControlPanel.Controls.Add(this.zoomPercentLabel);
      this.zoomControlPanel.Controls.Add(this.zoomTrackBar);
      this.zoomControlPanel.Controls.Add(this.zoomLabel);
      this.zoomControlPanel.Controls.Add(this.zoomResetButton);
      this.zoomControlPanel.Controls.Add(this.zoomTextBox);
      this.zoomControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.zoomControlPanel.Location = new System.Drawing.Point(0, 389);
      this.zoomControlPanel.Margin = new System.Windows.Forms.Padding(2);
      this.zoomControlPanel.Name = "zoomControlPanel";
      this.zoomControlPanel.Size = new System.Drawing.Size(710, 29);
      this.zoomControlPanel.TabIndex = 6;
      // 
      // zoomFitButton
      // 
      this.zoomFitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomFitButton.Location = new System.Drawing.Point(655, 3);
      this.zoomFitButton.Margin = new System.Windows.Forms.Padding(2);
      this.zoomFitButton.Name = "zoomFitButton";
      this.zoomFitButton.Size = new System.Drawing.Size(50, 21);
      this.zoomFitButton.TabIndex = 7;
      this.zoomFitButton.Text = "Fit";
      this.zoomFitButton.UseVisualStyleBackColor = true;
      this.zoomFitButton.Click += new System.EventHandler(this.zoomFitButton_Click);
      // 
      // patternElementIdLabel
      // 
      this.patternElementIdLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.patternElementIdLabel.AutoSize = true;
      this.patternElementIdLabel.Location = new System.Drawing.Point(10, 9);
      this.patternElementIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.patternElementIdLabel.Name = "patternElementIdLabel";
      this.patternElementIdLabel.Size = new System.Drawing.Size(0, 13);
      this.patternElementIdLabel.TabIndex = 6;
      // 
      // zoomPercentLabel
      // 
      this.zoomPercentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomPercentLabel.AutoSize = true;
      this.zoomPercentLabel.Location = new System.Drawing.Point(588, 6);
      this.zoomPercentLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.zoomPercentLabel.Name = "zoomPercentLabel";
      this.zoomPercentLabel.Size = new System.Drawing.Size(15, 13);
      this.zoomPercentLabel.TabIndex = 4;
      this.zoomPercentLabel.Text = "%";
      // 
      // zoomTrackBar
      // 
      this.zoomTrackBar.AllowDrop = true;
      this.zoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomTrackBar.AutoSize = false;
      this.zoomTrackBar.Location = new System.Drawing.Point(436, 3);
      this.zoomTrackBar.Margin = new System.Windows.Forms.Padding(2);
      this.zoomTrackBar.Maximum = 20;
      this.zoomTrackBar.Minimum = -20;
      this.zoomTrackBar.Name = "zoomTrackBar";
      this.zoomTrackBar.Size = new System.Drawing.Size(100, 16);
      this.zoomTrackBar.TabIndex = 2;
      this.zoomTrackBar.TickFrequency = 5;
      this.zoomTrackBar.ValueChanged += new System.EventHandler(this.zoomTrackBar_ValueChanged);
      // 
      // ringFilterGroupBox
      // 
      this.ringFilterGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ringFilterGroupBox.Controls.Add(this.ringFilterRadioButton0);
      this.ringFilterGroupBox.Location = new System.Drawing.Point(4, 197);
      this.ringFilterGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.ringFilterGroupBox.Name = "ringFilterGroupBox";
      this.ringFilterGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.ringFilterGroupBox.Size = new System.Drawing.Size(82, 106);
      this.ringFilterGroupBox.TabIndex = 7;
      this.ringFilterGroupBox.TabStop = false;
      this.ringFilterGroupBox.Text = "Ring Sizes";
      // 
      // ringFilterRadioButton0
      // 
      this.ringFilterRadioButton0.AutoSize = true;
      this.ringFilterRadioButton0.Checked = true;
      this.ringFilterRadioButton0.Location = new System.Drawing.Point(5, 18);
      this.ringFilterRadioButton0.Margin = new System.Windows.Forms.Padding(2);
      this.ringFilterRadioButton0.Name = "ringFilterRadioButton0";
      this.ringFilterRadioButton0.Size = new System.Drawing.Size(66, 17);
      this.ringFilterRadioButton0.TabIndex = 0;
      this.ringFilterRadioButton0.TabStop = true;
      this.ringFilterRadioButton0.Text = "All Rings";
      this.ringFilterRadioButton0.UseVisualStyleBackColor = true;
      // 
      // shapesGroupBox
      // 
      this.shapesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.shapesGroupBox.Controls.Add(this.lineShapeRadioButton);
      this.shapesGroupBox.Controls.Add(this.polygonShapeRadioButton);
      this.shapesGroupBox.Controls.Add(this.ellipseShapeRadioButton);
      this.shapesGroupBox.Controls.Add(this.rectangleShapeRadioButton);
      this.shapesGroupBox.Controls.Add(this.freehandShapeRadioButton);
      this.shapesGroupBox.Location = new System.Drawing.Point(4, 4);
      this.shapesGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.shapesGroupBox.Name = "shapesGroupBox";
      this.shapesGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.shapesGroupBox.Size = new System.Drawing.Size(82, 114);
      this.shapesGroupBox.TabIndex = 8;
      this.shapesGroupBox.TabStop = false;
      this.shapesGroupBox.Text = "Shapes";
      // 
      // lineShapeRadioButton
      // 
      this.lineShapeRadioButton.AutoSize = true;
      this.lineShapeRadioButton.Location = new System.Drawing.Point(5, 91);
      this.lineShapeRadioButton.Margin = new System.Windows.Forms.Padding(2);
      this.lineShapeRadioButton.Name = "lineShapeRadioButton";
      this.lineShapeRadioButton.Size = new System.Drawing.Size(45, 17);
      this.lineShapeRadioButton.TabIndex = 7;
      this.lineShapeRadioButton.TabStop = true;
      this.lineShapeRadioButton.Text = "Line";
      this.lineShapeRadioButton.UseVisualStyleBackColor = true;
      this.lineShapeRadioButton.CheckedChanged += new System.EventHandler(this.ShapeButton_CheckedChanged);
      // 
      // polygonShapeRadioButton
      // 
      this.polygonShapeRadioButton.AutoSize = true;
      this.polygonShapeRadioButton.Location = new System.Drawing.Point(5, 73);
      this.polygonShapeRadioButton.Margin = new System.Windows.Forms.Padding(2);
      this.polygonShapeRadioButton.Name = "polygonShapeRadioButton";
      this.polygonShapeRadioButton.Size = new System.Drawing.Size(63, 17);
      this.polygonShapeRadioButton.TabIndex = 6;
      this.polygonShapeRadioButton.Text = "Polygon";
      this.polygonShapeRadioButton.UseVisualStyleBackColor = true;
      this.polygonShapeRadioButton.CheckedChanged += new System.EventHandler(this.ShapeButton_CheckedChanged);
      // 
      // ellipseShapeRadioButton
      // 
      this.ellipseShapeRadioButton.AutoSize = true;
      this.ellipseShapeRadioButton.Location = new System.Drawing.Point(5, 54);
      this.ellipseShapeRadioButton.Margin = new System.Windows.Forms.Padding(2);
      this.ellipseShapeRadioButton.Name = "ellipseShapeRadioButton";
      this.ellipseShapeRadioButton.Size = new System.Drawing.Size(55, 17);
      this.ellipseShapeRadioButton.TabIndex = 5;
      this.ellipseShapeRadioButton.Text = "Ellipse";
      this.ellipseShapeRadioButton.UseVisualStyleBackColor = true;
      this.ellipseShapeRadioButton.CheckedChanged += new System.EventHandler(this.ShapeButton_CheckedChanged);
      // 
      // rectangleShapeRadioButton
      // 
      this.rectangleShapeRadioButton.AutoSize = true;
      this.rectangleShapeRadioButton.Location = new System.Drawing.Point(5, 37);
      this.rectangleShapeRadioButton.Margin = new System.Windows.Forms.Padding(2);
      this.rectangleShapeRadioButton.Name = "rectangleShapeRadioButton";
      this.rectangleShapeRadioButton.Size = new System.Drawing.Size(74, 17);
      this.rectangleShapeRadioButton.TabIndex = 4;
      this.rectangleShapeRadioButton.Text = "Rectangle";
      this.rectangleShapeRadioButton.UseVisualStyleBackColor = true;
      this.rectangleShapeRadioButton.CheckedChanged += new System.EventHandler(this.ShapeButton_CheckedChanged);
      // 
      // freehandShapeRadioButton
      // 
      this.freehandShapeRadioButton.AutoSize = true;
      this.freehandShapeRadioButton.Checked = true;
      this.freehandShapeRadioButton.Location = new System.Drawing.Point(5, 18);
      this.freehandShapeRadioButton.Margin = new System.Windows.Forms.Padding(2);
      this.freehandShapeRadioButton.Name = "freehandShapeRadioButton";
      this.freehandShapeRadioButton.Size = new System.Drawing.Size(70, 17);
      this.freehandShapeRadioButton.TabIndex = 0;
      this.freehandShapeRadioButton.TabStop = true;
      this.freehandShapeRadioButton.Text = "Freehand";
      this.freehandShapeRadioButton.UseVisualStyleBackColor = true;
      this.freehandShapeRadioButton.CheckedChanged += new System.EventHandler(this.ShapeButton_CheckedChanged);
      // 
      // fillGroupBox
      // 
      this.fillGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.fillGroupBox.Controls.Add(this.hollowFillRadioButton);
      this.fillGroupBox.Controls.Add(this.solidFillRadioButton);
      this.fillGroupBox.Location = new System.Drawing.Point(4, 122);
      this.fillGroupBox.Margin = new System.Windows.Forms.Padding(2);
      this.fillGroupBox.Name = "fillGroupBox";
      this.fillGroupBox.Padding = new System.Windows.Forms.Padding(2);
      this.fillGroupBox.Size = new System.Drawing.Size(82, 61);
      this.fillGroupBox.TabIndex = 9;
      this.fillGroupBox.TabStop = false;
      this.fillGroupBox.Text = "Fill";
      // 
      // hollowFillRadioButton
      // 
      this.hollowFillRadioButton.AutoSize = true;
      this.hollowFillRadioButton.Location = new System.Drawing.Point(5, 37);
      this.hollowFillRadioButton.Margin = new System.Windows.Forms.Padding(2);
      this.hollowFillRadioButton.Name = "hollowFillRadioButton";
      this.hollowFillRadioButton.Size = new System.Drawing.Size(57, 17);
      this.hollowFillRadioButton.TabIndex = 1;
      this.hollowFillRadioButton.Text = "Hollow";
      this.hollowFillRadioButton.UseVisualStyleBackColor = true;
      this.hollowFillRadioButton.CheckedChanged += new System.EventHandler(this.FillButton_CheckedChanged);
      // 
      // solidFillRadioButton
      // 
      this.solidFillRadioButton.AutoSize = true;
      this.solidFillRadioButton.Checked = true;
      this.solidFillRadioButton.Location = new System.Drawing.Point(5, 18);
      this.solidFillRadioButton.Margin = new System.Windows.Forms.Padding(2);
      this.solidFillRadioButton.Name = "solidFillRadioButton";
      this.solidFillRadioButton.Size = new System.Drawing.Size(48, 17);
      this.solidFillRadioButton.TabIndex = 0;
      this.solidFillRadioButton.TabStop = true;
      this.solidFillRadioButton.Text = "Solid";
      this.solidFillRadioButton.UseVisualStyleBackColor = true;
      this.solidFillRadioButton.CheckedChanged += new System.EventHandler(this.FillButton_CheckedChanged);
      // 
      // shapeProgressBar
      // 
      this.shapeProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.shapeProgressBar.Location = new System.Drawing.Point(4, 187);
      this.shapeProgressBar.Margin = new System.Windows.Forms.Padding(2);
      this.shapeProgressBar.Name = "shapeProgressBar";
      this.shapeProgressBar.Size = new System.Drawing.Size(82, 5);
      this.shapeProgressBar.TabIndex = 10;
      // 
      // overlayGroupBox
      // 
      this.overlayGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.overlayGroupBox.Controls.Add(this.overlayOpacityLabel);
      this.overlayGroupBox.Controls.Add(this.overlayOpacityTrackBar);
      this.overlayGroupBox.Controls.Add(this.overlayCheckBox);
      this.overlayGroupBox.Location = new System.Drawing.Point(4, 305);
      this.overlayGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.overlayGroupBox.Name = "overlayGroupBox";
      this.overlayGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.overlayGroupBox.Size = new System.Drawing.Size(82, 57);
      this.overlayGroupBox.TabIndex = 11;
      this.overlayGroupBox.TabStop = false;
      this.overlayGroupBox.Text = "Overlay";
      // 
      // overlayOpacityLabel
      // 
      this.overlayOpacityLabel.AutoSize = true;
      this.overlayOpacityLabel.Location = new System.Drawing.Point(9, 37);
      this.overlayOpacityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.overlayOpacityLabel.Name = "overlayOpacityLabel";
      this.overlayOpacityLabel.Size = new System.Drawing.Size(66, 13);
      this.overlayOpacityLabel.TabIndex = 2;
      this.overlayOpacityLabel.Text = "Opacity 50%";
      // 
      // overlayOpacityTrackBar
      // 
      this.overlayOpacityTrackBar.AutoSize = false;
      this.overlayOpacityTrackBar.Location = new System.Drawing.Point(20, 16);
      this.overlayOpacityTrackBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.overlayOpacityTrackBar.Maximum = 100;
      this.overlayOpacityTrackBar.Name = "overlayOpacityTrackBar";
      this.overlayOpacityTrackBar.Size = new System.Drawing.Size(60, 16);
      this.overlayOpacityTrackBar.TabIndex = 1;
      this.overlayOpacityTrackBar.Value = 50;
      this.overlayOpacityTrackBar.ValueChanged += new System.EventHandler(this.overlayOpacityTrackBar_ValueChanged);
      // 
      // overlayCheckBox
      // 
      this.overlayCheckBox.Location = new System.Drawing.Point(4, 20);
      this.overlayCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.overlayCheckBox.Name = "overlayCheckBox";
      this.overlayCheckBox.Size = new System.Drawing.Size(13, 13);
      this.overlayCheckBox.TabIndex = 0;
      this.overlayCheckBox.UseVisualStyleBackColor = true;
      this.overlayCheckBox.Click += new System.EventHandler(this.overlayCheckBox_Click);
      // 
      // drawingToolPanel
      // 
      this.drawingToolPanel.Controls.Add(this.overlayGroupBox);
      this.drawingToolPanel.Controls.Add(this.ringFilterGroupBox);
      this.drawingToolPanel.Controls.Add(this.shapeProgressBar);
      this.drawingToolPanel.Controls.Add(this.fillGroupBox);
      this.drawingToolPanel.Controls.Add(this.shapesGroupBox);
      this.drawingToolPanel.Dock = System.Windows.Forms.DockStyle.Right;
      this.drawingToolPanel.Location = new System.Drawing.Point(710, 24);
      this.drawingToolPanel.Margin = new System.Windows.Forms.Padding(2);
      this.drawingToolPanel.Name = "drawingToolPanel";
      this.drawingToolPanel.Size = new System.Drawing.Size(90, 394);
      this.drawingToolPanel.TabIndex = 25;
      // 
      // ChainmailleDesignerForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 418);
      this.Controls.Add(this.renderedImagePanel);
      this.Controls.Add(this.zoomControlPanel);
      this.Controls.Add(this.drawingToolPanel);
      this.Controls.Add(this.mainMenuStrip);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.mainMenuStrip;
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "ChainmailleDesignerForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Chainmaille Designer";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChainmailleDesignerForm_FormClosing);
      this.ResizeEnd += new System.EventHandler(this.ChainmailleDesignerForm_ResizeEnd);
      this.Resize += new System.EventHandler(this.ChainmailleDesignerForm_Resize);
      this.mainMenuStrip.ResumeLayout(false);
      this.mainMenuStrip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.renderedImagePictureBox)).EndInit();
      this.renderedImagePanel.ResumeLayout(false);
      this.zoomControlPanel.ResumeLayout(false);
      this.zoomControlPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).EndInit();
      this.ringFilterGroupBox.ResumeLayout(false);
      this.ringFilterGroupBox.PerformLayout();
      this.shapesGroupBox.ResumeLayout(false);
      this.shapesGroupBox.PerformLayout();
      this.fillGroupBox.ResumeLayout(false);
      this.fillGroupBox.PerformLayout();
      this.overlayGroupBox.ResumeLayout(false);
      this.overlayGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.overlayOpacityTrackBar)).EndInit();
      this.drawingToolPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

#endregion

    private System.Windows.Forms.MenuStrip mainMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileOpenToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem fileCloseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveDesignToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveDesignAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveRenderedImageToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
    private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem designToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem colorsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.PictureBox renderedImagePictureBox;
    private System.Windows.Forms.Panel renderedImagePanel;
    private System.Windows.Forms.ToolStripMenuItem helpAboutToolStripMenuItem;
    private System.Windows.Forms.Label zoomLabel;
    private System.Windows.Forms.TextBox zoomTextBox;
    private System.Windows.Forms.Button zoomResetButton;
    private System.Windows.Forms.Panel zoomControlPanel;
    private System.Windows.Forms.TrackBar zoomTrackBar;
    private System.Windows.Forms.Label zoomPercentLabel;
    private System.Windows.Forms.Label patternElementIdLabel;
    private System.Windows.Forms.VScrollBar zoomedImageVerticalScrollBar;
    private System.Windows.Forms.HScrollBar zoomedImageHorizontalScrollBar;
    private System.Windows.Forms.ToolStripMenuItem printRenderedImageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem paletteWindowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem countColorsToolStripMenuItem;
    private System.Windows.Forms.Button zoomFitButton;
    private System.Windows.Forms.ToolStripMenuItem designOverlayToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openOverlayToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator overlaySeparatorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showHideOverlayToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showHelpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem licenseAgreementToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clockwiseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem counterclockwiseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem clockwiseOverlayToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem counterclockwiseOverlayToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem replaceInDesignToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem guidelinesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showHideGuidelinesToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem horizontalGuidelineToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem verticalGuidelineToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
    private System.Windows.Forms.ToolStripMenuItem halvesGuidelinesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem thirdsGuidelinesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem quartersGuidelinesToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
    private System.Windows.Forms.ToolStripMenuItem clearAllGuidelinesToolStripMenuItem;
    private System.Windows.Forms.GroupBox ringFilterGroupBox;
    private System.Windows.Forms.RadioButton ringFilterRadioButton0;
    private System.Windows.Forms.GroupBox shapesGroupBox;
    private System.Windows.Forms.RadioButton polygonShapeRadioButton;
    private System.Windows.Forms.RadioButton ellipseShapeRadioButton;
    private System.Windows.Forms.RadioButton rectangleShapeRadioButton;
    private System.Windows.Forms.RadioButton freehandShapeRadioButton;
    private System.Windows.Forms.RadioButton lineShapeRadioButton;
    private System.Windows.Forms.GroupBox fillGroupBox;
    private System.Windows.Forms.RadioButton hollowFillRadioButton;
    private System.Windows.Forms.RadioButton solidFillRadioButton;
    private System.Windows.Forms.ProgressBar shapeProgressBar;
    private System.Windows.Forms.ToolStripMenuItem colorRingsFromOverlayToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
    private System.Windows.Forms.ToolStripMenuItem clearAllRingColorsToolStripMenuItem;
    private System.Windows.Forms.GroupBox overlayGroupBox;
    private System.Windows.Forms.Label overlayOpacityLabel;
    private System.Windows.Forms.TrackBar overlayOpacityTrackBar;
    private System.Windows.Forms.CheckBox overlayCheckBox;
    private System.Windows.Forms.Panel drawingToolPanel;
  }
}

