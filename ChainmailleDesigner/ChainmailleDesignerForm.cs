// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmailleDesignerForm.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class ChainmailleDesignerForm : Form, IShapeProgressIndicator,
    PaletteFormClientInterface
  {
    private ChainmailleDesign chainmailleDesign = null;
    private PaletteForm paletteForm = null;
    private bool paletteFormIsShowing = false;

    private float zoomFactor = 1.0F;
    private int minimumImagePixels = 12;
    private Brush blankingBrush = new SolidBrush(SystemColors.Control);
    private Pen guidelinePen =
      new Pen(Properties.Settings.Default.GuidelineColor);
    private Pen rubberBandPen =
      new Pen(Properties.Settings.Default.GuidelineColor, 3);
    private Graphics zoomGraphics;
    PointF zoomCenterInRenderedImage = new PointF();
    PointF zoomCenterInZoomedImage = new PointF();
    // The full zoomed image is the entire rendered image,
    // rotated and zoomed for display in the display area.
    PointF fullZoomedUpperLeft = new PointF();
    PointF fullZoomedLowerRight = new PointF();
    SizeF fullZoomedSize = new SizeF();

    private bool scrollBarResponseEnabled = true;
    private bool zoomControlResponseEnabled = true;
    private bool overlayControlResponseEnabled = true;

    private float defaultOverlayTransparency = 0.5F;

    private bool mouseIsDown = false;
    private Keys mouseDownKeys;
    private ChainmaillePatternElementId lastMouseDownElementId = null;
    enum MouseMode
    {
      None, PaintingRings, MovingOverlay, ScalingOverlay, Drawing,
      SettingHorizontalGuideline, SettingVerticalGuideline
    };
    MouseMode mouseMode = MouseMode.None;
    PointF mouseDownPointInDisplayImage = new PointF();
    PointF mouseDownPointInRenderedImage = new PointF();
    float mouseDownOverlayXScaleFactor = 1.0F;
    float mouseDownOverlayYScaleFactor = 1.0F;
    PointF mouseDownOverlayCenterInRenderedImage = new PointF();

    enum DrawingMode { Freehand, Rectangle, Ellipse, Polygon, Line };
    DrawingMode currentDrawingMode = DrawingMode.Freehand;
    DrawingFillMode currentFillMode = DrawingFillMode.Solid;
    int drawingModeStepNr = 0;
    List<PointF> drawingPoints = new List<PointF>();
    Color drawingColor = Color.White;

    // Rotation.
    private const float quarterCircleInDegrees = 90F;

    // Ring size filtering.
    string ringSizeFilter = string.Empty;
    // Note: When the ring size filter is empty, operations affect all ring
    // sizes.
    private const string ringFilterNameBase = "ringFilterRadioButton";

    // Dynamic UI element generation.
    private float xScaleFactor = 1;
    private float yScaleFactor = 1;
    private int groupBoxSpacing = 10;

    public ChainmailleDesignerForm()
    {
      InitializeComponent();

      xScaleFactor = shapesGroupBox.Width / 165F;
      yScaleFactor = shapesGroupBox.Height / 215F;

      paletteForm = new PaletteForm(this);
      ShowPaletteForm(true);
      paletteFormIsShowing = true;
      Top = paletteForm.Top;
      Left = paletteForm.Left + paletteForm.Width;
      ChainmailleDesignerForm_Resize(null, null);

      renderedImagePictureBox.BackgroundImage = new Bitmap(
        renderedImagePictureBox.ClientSize.Width,
        renderedImagePictureBox.ClientSize.Height);
      zoomGraphics = Graphics.FromImage(
        renderedImagePictureBox.BackgroundImage);

      EnableMenuItems();
      EnableRingSizeControls();
      EnableOverlayControls();
      ShowZoomControlValues();
    }

    private void clockwiseToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      // Rotate the design area 90 degrees clockwise.
      RotateDesign(quarterCircleInDegrees);
    }

    private void counterclockwiseToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      // Rotate the design area 90 degrees counterclockwise.
      RotateDesign(-quarterCircleInDegrees);
    }

    private void clockwiseOverlayToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      // Rotate the overlay 90 degrees clockwise.
      RotateOverlay(quarterCircleInDegrees);
    }

    private void counterclockwiseOverlayToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      // Rotate the design area 90 degrees counterclockwise.
      RotateOverlay(-quarterCircleInDegrees);
    }

    private void RotateDesign(float degreesToRotate)
    {
      if (chainmailleDesign != null)
      {
        chainmailleDesign.RotateDesign(degreesToRotate);
        ShowRenderedImageAtZoom();
        RefreshScrollBars();
      }
    }

    private void RotateOverlay(float degreesToRotate)
    {
      if (chainmailleDesign != null)
      {
        chainmailleDesign.RotateOverlay(degreesToRotate);
        ShowRenderedImageAtZoom();
      }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      bool continueWithOperation = true;
      if (chainmailleDesign != null && chainmailleDesign.HasBeenChanged)
      {
        // Ask whether to save.
        DialogResult dialogResult = AskWhetherToSaveDesignChanges();
        if (dialogResult == DialogResult.Yes)
        {
          if (string.IsNullOrEmpty(chainmailleDesign.DesignFile))
          {
            saveDesignAsToolStripMenuItem_Click(sender, e);
          }
          else
          {
            SetPaletteInDesign();
            chainmailleDesign.Save();
          }
        }
        else if (dialogResult == DialogResult.Cancel)
        {
          continueWithOperation = false;
        }
      }

      if (continueWithOperation)
      {
        Close();
      }
    }

    private void fileNewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      NewDesignForm dlg = new NewDesignForm();
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        bool continueWithOperation = true;
        if (chainmailleDesign != null && chainmailleDesign.HasBeenChanged)
        {
          // Ask whether to save.
          DialogResult dialogResult = AskWhetherToSaveDesignChanges();
          if (dialogResult == DialogResult.Yes)
          {
            if (string.IsNullOrEmpty(chainmailleDesign.DesignFile))
            {
              saveDesignAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
              SetPaletteInDesign();
              chainmailleDesign.Save();
            }
          }
          else if (dialogResult == DialogResult.Cancel)
          {
            continueWithOperation = false;
          }
        }
        if (continueWithOperation)
        {
          // Create a new design.
          chainmailleDesign = new ChainmailleDesign(
            dlg.DesignName, dlg.WeaveFile,
            dlg.DesignSize, dlg.DesignSizeUnits, dlg.RoundSizeUp, dlg.Wrap,
            dlg.DesignScale);
          chainmailleDesign.Description = dlg.Description;
          chainmailleDesign.DesignDate = dlg.DesignDate;
          chainmailleDesign.DesignedBy = dlg.DesignedBy;
          chainmailleDesign.DesignedFor = dlg.DesignedFor;

          // Set the colors for rendering.
          chainmailleDesign.BackgroundColorHasChanged(
            paletteForm.BackgroundColor);
          chainmailleDesign.OutlineColorHasChanged(paletteForm.OutlineColor);

          // Render and display the design.
          chainmailleDesign.RenderImage();
          zoomResetButton_Click(null, null);
        }
      }

      EnableMenuItems();
      EnableRingSizeControls();
      EnableOverlayControls();
    }

    private void fileOpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog dlg = new OpenFileDialog();
      dlg.InitialDirectory = Properties.Settings.Default.DesignDirectory;
      dlg.Filter = Properties.Settings.Default.DesignFileFilter;
      dlg.FilterIndex = 1;
      dlg.CheckFileExists = true;
      dlg.CheckPathExists = true;
      dlg.Multiselect = false;
      dlg.Title = "Open Design";
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        bool continueWithOperation = true;
        if (chainmailleDesign != null && chainmailleDesign.HasBeenChanged)
        {
          // Ask whether to save.
          DialogResult dialogResult = AskWhetherToSaveDesignChanges();
          if (dialogResult == DialogResult.Yes)
          {
            if (string.IsNullOrEmpty(chainmailleDesign.DesignFile))
            {
              saveDesignAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
              SetPaletteInDesign();
              chainmailleDesign.Save();
            }
          }
          else if (dialogResult == DialogResult.Cancel)
          {
            continueWithOperation = false;
          }
        }
        if (continueWithOperation)
        {
          // Open the design file.
          chainmailleDesign = new ChainmailleDesign(dlg.FileName);
          string messages = chainmailleDesign.Messages;
          if (!string.IsNullOrEmpty(chainmailleDesign.PaletteFile))
          {
            // The design specified a palette, so try to open it.
            if (!paletteForm.OpenPalette(chainmailleDesign.PaletteFile,
                  chainmailleDesign.PaletteName))
            {
              // Couldn't find (or maybe read) the color palette for the
              // design. Ask whether to locate it.
              if (MessageBox.Show("Since the color palette for the design " +
                "couldn't be read, would you like to specify where it is?",
                "Locate Color Palette?", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
              {
                OpenFileDialog odlg = new OpenFileDialog();
                odlg.InitialDirectory =
                  Properties.Settings.Default.PaletteDirectory;
                odlg.Filter = Properties.Settings.Default.PaletteFileFilter;
                odlg.FilterIndex = 1;
                odlg.CheckFileExists = true;
                odlg.CheckPathExists = true;
                odlg.Multiselect = false;
                odlg.Title = "Choose Color Palette";
                if (odlg.ShowDialog() == DialogResult.OK)
                {
                  if (!paletteForm.OpenPalette(odlg.FileName, null))
                  {
                    MessageBox.Show("Selected file is not usable as a color" +
                      " palette for the design.", "Color Palette Failure",
                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                }
              }
            }
            paletteForm.HiddenSections =
              chainmailleDesign.PaletteSectionsHidden;
            if (paletteForm.Palette != null &&
                paletteForm.Palette.HasBeenInitialized &&
                paletteForm.Palette.PaletteFile !=
                  chainmailleDesign.PaletteFile)
            {
              // The palette file was corrected. Save the corrected palette
              // information in the design.
              SetPaletteInDesign();
            }
         }

          // Set the colors for rendering.
          chainmailleDesign.BackgroundColorHasChanged(
            paletteForm.BackgroundColor);
          chainmailleDesign.OutlineColorHasChanged(paletteForm.OutlineColor);

          // Show messages, if any.
          if (!string.IsNullOrEmpty(messages))
          {
            MessageBox.Show(messages, "Your Attention, Please",
              MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }

          if (chainmailleDesign.ColorImage == null ||
              chainmailleDesign.ColorImage.BitmapImage == null)
          {
            // Couldn't find (or maybe read) the color image for the design.
            // Ask whether to locate it.
            if (MessageBox.Show("Since the color image for the design " +
              "couldn't be read, would you like to specify where it is?",
              "Locate Color Image?", MessageBoxButtons.YesNo,
              MessageBoxIcon.Question) == DialogResult.Yes)
            {
              OpenFileDialog odlg = new OpenFileDialog();
              odlg.InitialDirectory =
                Properties.Settings.Default.DesignDirectory;
              odlg.Filter = Properties.Settings.Default.ColorImageFileFilter;
              odlg.FilterIndex = 1;
              odlg.CheckFileExists = true;
              odlg.CheckPathExists = true;
              odlg.Multiselect = false;
              odlg.Title = "Choose Color Image";
              if (odlg.ShowDialog() == DialogResult.OK)
              {
                try
                {
                  chainmailleDesign.ColorImage = new ColorImage(odlg.FileName);
                }
                catch
                {
                  MessageBox.Show("Selected file is not usable as a color" +
                    " image for the design.", "Color Image Failure",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
              }
            }
          }

          if (!string.IsNullOrEmpty(chainmailleDesign.OverlayFile) &&
              chainmailleDesign.OverlayImage == null)
          {
            // Couldn't find (or maybe read) the overlay image for the design.
            // Ask whether to locate it.
            if (MessageBox.Show("Since the overlay image for the design " +
              "couldn't be read, would you like to specify where it is?",
              "Locate Overlay Image?", MessageBoxButtons.YesNo,
              MessageBoxIcon.Question) == DialogResult.Yes)
            {
              OpenFileDialog odlg = new OpenFileDialog();
              odlg.InitialDirectory =
                Properties.Settings.Default.DesignDirectory;
              odlg.Filter = Properties.Settings.Default.ImageFileFilter;
              odlg.FilterIndex = 1;
              odlg.CheckFileExists = true;
              odlg.CheckPathExists = true;
              odlg.Multiselect = false;
              odlg.Title = "Choose Overlay Image";
              if (odlg.ShowDialog() == DialogResult.OK)
              {
                chainmailleDesign.OverlayFile = odlg.FileName;
                if (chainmailleDesign.OverlayImage == null)
                {
                  MessageBox.Show("Selected file is not usable as an overlay" +
                    " image for the design.", "Overlay Image Failure",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
              }
            }
          }

          // Render and display the design.
          chainmailleDesign.RenderImage();
          // Generate display transformations based on rotations from file.
          // Note: These "add" zero to the values from the file.
          chainmailleDesign.RotateDesign(0F, true);
          chainmailleDesign.RotateOverlay(0F, true);
          // Reset zoom.
          zoomResetButton_Click(null, null);
        }
      }

      if (chainmailleDesign != null)
      {
        showHideGuidelinesToolStripMenuItem.Text =
          chainmailleDesign.ShowGuidelines ? "Hide" : "Show";
        showHideOverlayToolStripMenuItem.Text =
          chainmailleDesign.ShowOverlay ? "Hide" : "Show";
      }

      EnableMenuItems();
      EnableRingSizeControls();
      EnableOverlayControls();
    }

    private DialogResult AskWhetherToSaveDesignChanges()
    {
      string title = "Save Design?";
      string message = "The design has been changed. Do you wish to save " +
        "your changes before proceeding?";
      return MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
    }

    private void fileCloseToolStripMenuItem_Click(object sender, EventArgs e)
    {
      bool continueWithOperation = true;
      if (chainmailleDesign != null && chainmailleDesign.HasBeenChanged)
      {
        // Ask whether to save.
        DialogResult dialogResult = AskWhetherToSaveDesignChanges();
        if (dialogResult == DialogResult.Yes)
        {
          if (string.IsNullOrEmpty(chainmailleDesign.DesignFile))
          {
            saveDesignAsToolStripMenuItem_Click(sender, e);
          }
          else
          {
            SetPaletteInDesign();
            chainmailleDesign.Save();
          }
        }
        else if (dialogResult == DialogResult.Cancel)
        {
          continueWithOperation = false;
        }
      }
      if (continueWithOperation)
      {
        // Clear the zoomed image.
        // Note: Clearing the zoomed image also resets its transformation.
        ClearZoomedImage();
        renderedImagePictureBox.Invalidate();

        // Remove the design.
        chainmailleDesign = null;
      }

      EnableMenuItems();
      EnableRingSizeControls();
      EnableOverlayControls();
    }

    private void SetZoom(float? newZoomFactor,
      PointF? newZoomCenterInZoomedImage = null,
      PointF?  newZoomCenterInRenderedImage = null)
    {
      if (newZoomFactor.HasValue)
      {
        if (chainmailleDesign != null &&
            chainmailleDesign.RenderedImage != null)
        {
          // Limit the lower end of the zoom. We should have at least a few
          // pixels on the screen.
          double nrRenderedPixels = chainmailleDesign.RenderedImage.Width *
            chainmailleDesign.RenderedImage.Height;
          float minZoomFactor = (float)
            Math.Sqrt(minimumImagePixels / nrRenderedPixels);
          zoomFactor = Math.Max(minZoomFactor, newZoomFactor.Value);
        }
        else
        {
          // No rendered image, so no basis on which to limit zoom.
          zoomFactor = newZoomFactor.Value;
        }
      }

      if (newZoomCenterInZoomedImage.HasValue)
      {
        zoomCenterInZoomedImage = newZoomCenterInZoomedImage.Value;
      }

      if (newZoomCenterInRenderedImage.HasValue)
      {
        zoomCenterInRenderedImage = newZoomCenterInRenderedImage.Value;
      }

      if (chainmailleDesign != null &&
          chainmailleDesign.RenderedImage != null)
      {
        // Reset zoom centers to the center of the zoomed image if they are
        // not already there.
        // There should be no effect unless we're responding to a mouse zoom.
        PointF newCenterInZoomedImage = new PointF(
          0.5F * renderedImagePictureBox.BackgroundImage.Size.Width,
          0.5F * renderedImagePictureBox.BackgroundImage.Size.Height);
        if (newCenterInZoomedImage != zoomCenterInZoomedImage)
        {
          // Do this transformation before we change the actual centers,
          // since changing the centers will alter the transformation.
          PointF newCenterInRenderedImage = ZoomedImagePointFToRenderedImage(
            newCenterInZoomedImage);
          // Now change the actual centers.
          zoomCenterInZoomedImage = newCenterInZoomedImage;
          zoomCenterInRenderedImage = newCenterInRenderedImage;
        }
      }
    }

    public void ShowRenderedImageAtZoom(bool suppressInvalidate = false)
    {
      if (chainmailleDesign != null &&
          chainmailleDesign.RenderedImage != null)
      {
        // Determine the extent in the display area that would be needed to
        // display the entire rendered image at the current rotation and zoom.

        // The actual sizes of the display area and the rendered image.
        Size zoomedImageSize = renderedImagePictureBox.BackgroundImage.Size;
        Size renderedImageSize = chainmailleDesign.RenderedImage.Size;
        // The opposite corners of the rendered image.
        PointF renderedImageUpperLeft = new PointF(0, 0);
        PointF renderedImageLowerRight = new PointF(
          renderedImageSize.Width, renderedImageSize.Height);
        // Transform rendered image corners to the zoomed image.
        PointF riulInZoomedImage =
          RenderedImagePointFToZoomedImage(renderedImageUpperLeft);
        PointF rilrInZoomedImage =
          RenderedImagePointFToZoomedImage(renderedImageLowerRight);
        // Because the display rotation means that these are not
        // necessarily at the upper-left and lower-right respectively,
        // account for the possible rotation by using min and max.
        fullZoomedUpperLeft.X =
          Math.Min(riulInZoomedImage.X, rilrInZoomedImage.X);
        fullZoomedUpperLeft.Y =
          Math.Min(riulInZoomedImage.Y, rilrInZoomedImage.Y);
        fullZoomedLowerRight.X =
          Math.Max(riulInZoomedImage.X, rilrInZoomedImage.X);
        fullZoomedLowerRight.Y =
          Math.Max(riulInZoomedImage.Y, rilrInZoomedImage.Y);
        fullZoomedSize.Width =
          fullZoomedLowerRight.X - fullZoomedUpperLeft.X;
        fullZoomedSize.Height =
          fullZoomedLowerRight.Y - fullZoomedUpperLeft.Y;

        // Start with a destination rectangle that can encompass the entire
        // full zoomed image.
        Rectangle destinationRectangle = new Rectangle(
          (int)Math.Round(fullZoomedUpperLeft.X),
          (int)Math.Round(fullZoomedUpperLeft.Y),
          (int)Math.Round(fullZoomedSize.Width),
          (int)Math.Round(fullZoomedSize.Height));

        // Clip the destination rectangle to the available display area.
        // If we do clip, it means that we ought to show the corresponding
        // scroll bar.
        bool showHorizontalScrollBar = false;
        bool showVerticalScrollBar = false;
        if (destinationRectangle.X < 0)
        {
          // The image would spill over the left edge of the display area.
          // Clip the left edge.
          destinationRectangle.Width += destinationRectangle.X;
          destinationRectangle.X = 0;
          showHorizontalScrollBar = true;
        }
        if (destinationRectangle.Y < 0)
        {
          // The image would spill over the top edge of the display area.
          // Clip the top edge.
          destinationRectangle.Height += destinationRectangle.Y;
          destinationRectangle.Y = 0;
          showVerticalScrollBar = true;
        }
        int overflow = destinationRectangle.X + destinationRectangle.Width -
          zoomedImageSize.Width;
        if (overflow > 0)
        {
          // The image would spill over the right edge of the display area.
          // Clip the right edge.
          destinationRectangle.Width -= overflow;
          showHorizontalScrollBar = true;
        }
        overflow = destinationRectangle.Y + destinationRectangle.Height -
          zoomedImageSize.Height;
        if (overflow > 0)
        {
          // The image would spill over the bottom edge of the display area.
          // Clip the bottom edge.
          destinationRectangle.Height -= overflow;
          showVerticalScrollBar = true;
        }

        // Determine the extent in the rendered image that corresponds to the
        // (possibly clipped) destination rectangle.

        // Transform the corners of the destination rectangle to the rendered
        // image.
        PointF drulInRenderedImage = ZoomedImagePointFToRenderedImage(
          new PointF(destinationRectangle.X, destinationRectangle.Y));
        PointF drlrInRenderedImage =
          ZoomedImagePointFToRenderedImage(new PointF(
            destinationRectangle.X + destinationRectangle.Size.Width,
            destinationRectangle.Y + destinationRectangle.Height));
        // Determine the corners of the source rectangle, allowing for
        // display rotation.
        PointF sourceRectangleUpperLeft = new PointF(
          Math.Min(drulInRenderedImage.X, drlrInRenderedImage.X),
          Math.Min(drulInRenderedImage.Y, drlrInRenderedImage.Y));
        PointF sourceRectangleLowerRight = new PointF(
          Math.Max(drulInRenderedImage.X, drlrInRenderedImage.X),
          Math.Max(drulInRenderedImage.Y, drlrInRenderedImage.Y));
        SizeF sourceRectangleSize = new SizeF(
          sourceRectangleLowerRight.X -
          sourceRectangleUpperLeft.X,
          sourceRectangleLowerRight.Y -
          sourceRectangleUpperLeft.Y);
        Rectangle sourceRectangle = new Rectangle(
          (int)Math.Round(sourceRectangleUpperLeft.X),
          (int)Math.Round(sourceRectangleUpperLeft.Y),
          (int)Math.Round(sourceRectangleSize.Width),
          (int)Math.Round(sourceRectangleSize.Height));

        // Clear the zoomed image, so that an old zoomed image doesn't remain
        // behind the new zoomed image.
        // Note: Clearing the zoomed image also resets its transformation.
        ClearZoomedImage();

        // Set the transformation for display graphics.
        zoomGraphics.Transform =
          chainmailleDesign.RenderedImageToDisplayTransformation;

        // Also, now that we've done the clipping, the draw image call needs
        // the destination rectangle to be in un-transformed coordinates.
        PointF drulCounterRotated =
          chainmailleDesign.TransformDisplayPointFToRenderedImage(new PointF(
            destinationRectangle.X, destinationRectangle.Y));
        PointF drlrCounterRotated =
          chainmailleDesign.TransformDisplayPointFToRenderedImage(new PointF(
            destinationRectangle.X + destinationRectangle.Width,
            destinationRectangle.Y + destinationRectangle.Height));
        PointF counterRotatedUpperLeft = new PointF(
          Math.Min(drulCounterRotated.X, drlrCounterRotated.X),
          Math.Min(drulCounterRotated.Y, drlrCounterRotated.Y));
        PointF counterRotatedLowerRight = new PointF(
          Math.Max(drulCounterRotated.X, drlrCounterRotated.X),
          Math.Max(drulCounterRotated.Y, drlrCounterRotated.Y));
        SizeF counterRotatedSize = new SizeF(
          counterRotatedLowerRight.X - counterRotatedUpperLeft.X,
          counterRotatedLowerRight.Y - counterRotatedUpperLeft.Y);
        Rectangle counterRotatedDestinationRectangle = new Rectangle(
          (int)Math.Round(counterRotatedUpperLeft.X),
          (int)Math.Round(counterRotatedUpperLeft.Y),
          (int)Math.Round(counterRotatedSize.Width),
          (int)Math.Round(counterRotatedSize.Height));

        // Draw the zoomed image.
        zoomGraphics.DrawImage(chainmailleDesign.RenderedImage,
          counterRotatedDestinationRectangle, sourceRectangle,
          GraphicsUnit.Pixel);

        // Show the overlay, if any.
        if (chainmailleDesign.OverlayImage != null &&
            chainmailleDesign.ShowOverlay)
        {
          // Further counter-rotate the counter-rotated destination rectangle
          // to account for the overlay transformation.
          PointF[] destinationCorners = new PointF[]
          {
            counterRotatedUpperLeft, counterRotatedLowerRight
          };
          chainmailleDesign.RenderedToOverlayImageTransformation.
            TransformPoints(destinationCorners);
          PointF counterCounterRotatedUpperLeft = new PointF(
            Math.Min(destinationCorners[0].X, destinationCorners[1].X),
            Math.Min(destinationCorners[0].Y, destinationCorners[1].Y));
          PointF counterCounterRotatedLowerRight = new PointF(
            Math.Max(destinationCorners[0].X, destinationCorners[1].X),
            Math.Max(destinationCorners[0].Y, destinationCorners[1].Y));
          SizeF counterCounterRotatedSize = new SizeF(
            counterCounterRotatedLowerRight.X -
            counterCounterRotatedUpperLeft.X,
            counterCounterRotatedLowerRight.Y -
            counterCounterRotatedUpperLeft.Y);
          Rectangle counterCounterRotatedDestinationRectangle = new Rectangle(
            (int)Math.Round(counterCounterRotatedUpperLeft.X),
            (int)Math.Round(counterCounterRotatedUpperLeft.Y),
            (int)Math.Round(counterCounterRotatedSize.Width),
            (int)Math.Round(counterCounterRotatedSize.Height));

          // Determine the extent in the overlay image that corresponds to the
          // extent in the rendered image.
          PointF[] sourceCorners = new PointF[]
          {
            sourceRectangleUpperLeft, sourceRectangleLowerRight
          };
          chainmailleDesign.RenderedToOverlayImageTransformation.
            TransformPoints(sourceCorners);
          PointF overlaySourceUpperLeft = new PointF(
            Math.Min(sourceCorners[0].X, sourceCorners[1].X),
            Math.Min(sourceCorners[0].Y, sourceCorners[1].Y));
          PointF overlaySourceLowerRight = new PointF(
            Math.Max(sourceCorners[0].X, sourceCorners[1].X),
            Math.Max(sourceCorners[0].Y, sourceCorners[1].Y));
          SizeF overlaySourceSize = new SizeF(
            overlaySourceLowerRight.X - overlaySourceUpperLeft.X,
            overlaySourceLowerRight.Y - overlaySourceUpperLeft.Y);
          Rectangle overlaySourceRectangle = new Rectangle(
            (int)Math.Round(overlaySourceUpperLeft.X),
            (int)Math.Round(overlaySourceUpperLeft.Y),
            (int)Math.Round(overlaySourceSize.Width),
            (int)Math.Round(overlaySourceSize.Height));

          // Set the transformation from the overlay, through the rendered
          // image, to the display.
          Matrix overlayToDisplayImageTransformation =
            chainmailleDesign.OverlayToRenderedImageTransformation.Clone();
          overlayToDisplayImageTransformation.Multiply(chainmailleDesign.
            RenderedImageToDisplayTransformation, MatrixOrder.Append);
          zoomGraphics.Transform = overlayToDisplayImageTransformation;
          // Draw the overlay.
          zoomGraphics.DrawImage(chainmailleDesign.OverlayImage,
            counterCounterRotatedDestinationRectangle,
            overlaySourceRectangle.X, overlaySourceRectangle.Y,
            overlaySourceRectangle.Width, overlaySourceRectangle.Height,
            GraphicsUnit.Pixel, chainmailleDesign.OverlayAttributes);
        }

        if (chainmailleDesign.ShowGuidelines)
        {
          zoomGraphics.ResetTransform();
          foreach (int horizontalGuideline
                   in chainmailleDesign.HorizontalGuidelines)
          {
            PointF guidelinePoint = UnzoomedImagePointFToZoomed(
              chainmailleDesign.TransformRenderedImagePointFToDisplay(
              new PointF(0, horizontalGuideline)));
            if (chainmailleDesign.HVReversed)
            {
              if (guidelinePoint.X >= 0 &&
                  guidelinePoint.X < zoomedImageSize.Width)
              {
                zoomGraphics.DrawLine(guidelinePen, guidelinePoint.X, 0,
                  guidelinePoint.X, zoomedImageSize.Height);
              }
            }
            else
            {
              if (guidelinePoint.Y >= 0 &&
                  guidelinePoint.Y < zoomedImageSize.Height)
              {
                zoomGraphics.DrawLine(guidelinePen, 0, guidelinePoint.Y,
                  zoomedImageSize.Width, guidelinePoint.Y);
              }
            }
          }

          foreach (int verticalGuideline
                   in chainmailleDesign.VerticalGuidelines)
          {
            PointF guidelinePoint = UnzoomedImagePointFToZoomed(
              chainmailleDesign.TransformRenderedImagePointFToDisplay(
              new PointF(verticalGuideline, 0)));
            if (chainmailleDesign.HVReversed)
            {
              if (guidelinePoint.Y >= 0 &&
                  guidelinePoint.Y < zoomedImageSize.Height)
              {
                zoomGraphics.DrawLine(guidelinePen, 0, guidelinePoint.Y,
                  zoomedImageSize.Width, guidelinePoint.Y);
              }
            }
            else
            {
              if (guidelinePoint.X >= 0 &&
                  guidelinePoint.X < zoomedImageSize.Width)
              {
                zoomGraphics.DrawLine(guidelinePen, guidelinePoint.X, 0,
                  guidelinePoint.X, zoomedImageSize.Height);
              }
            }
          }
        }

        if (!suppressInvalidate)
        {
          // Invalidate the display area so that its contents will be
          // refreshed.
          renderedImagePictureBox.Invalidate();
        }

        // Set visibility of scroll bars.
        zoomedImageHorizontalScrollBar.Visible = showHorizontalScrollBar;
        zoomedImageVerticalScrollBar.Visible = showVerticalScrollBar;
      }
    }

    private void ShowRubberBand(List<PointF> transformedPoints)
    {
      if (drawingPoints.Count > 0)
      {
        zoomGraphics.ResetTransform();

        if (transformedPoints.Count == 1)
        {
          transformedPoints.Add(new PointF(
            transformedPoints[0].X + 1, transformedPoints[0].Y + 1));
        }
        zoomGraphics.DrawLines(rubberBandPen, transformedPoints.ToArray());

        // Invalidate the display area so that its contents will be refreshed.
        renderedImagePictureBox.Invalidate();
      }
    }

    private void ShowRubberEllipse(RectangleF circleBox, RectangleF ellipseBox)
    {
      if (drawingPoints.Count > 0)
      {
        zoomGraphics.ResetTransform();

        zoomGraphics.DrawEllipse(rubberBandPen, circleBox);
        zoomGraphics.DrawEllipse(rubberBandPen, ellipseBox);

        // Invalidate the display area so that its contents will be refreshed.
        renderedImagePictureBox.Invalidate();
      }
    }

    /// <summary>
    /// Clear the zoomed image by filling it with its background color.
    /// </summary>
    private void ClearZoomedImage()
    {
      // Reset the transformation so that width, height are in the
      // expected directions.
      zoomGraphics.Transform = new Matrix();
      // Fill the entire display area with blankness.
      zoomGraphics.FillRectangle(blankingBrush, new Rectangle(0, 0,
        renderedImagePictureBox.BackgroundImage.Width,
        renderedImagePictureBox.BackgroundImage.Height));
    }

    /// <summary>
    /// Reset the min, max, and value of the horizontal and vertical scroll
    /// bars according to the relationship between the picture box size,
    /// the current zoom factor, the current zoom center, and the size of the
    /// rendered image.
    /// </summary>
    private void RefreshScrollBars()
    {
      if (chainmailleDesign != null &&
          chainmailleDesign.RenderedImage != null)
      {
        // Determine the extent in the display area that would be needed to
        // display the entire rendered image at the current rotation and zoom.

        // The actual sizes of the display area.
        Size zoomedImageSize = renderedImagePictureBox.BackgroundImage.Size;

        // If the left or right edge of the design area is showing, adjust the
        // minimum and/or maximum scroll bar value; otherwise the minimum is
        // zero (the left edge of the full zoomed image) and the maximum is the
        // right edge of the full zoomed image.
        zoomedImageHorizontalScrollBar.Minimum = (int)Math.Floor(Math.Min(
          -fullZoomedUpperLeft.X, 0));
        zoomedImageHorizontalScrollBar.Maximum = (int)Math.Ceiling(Math.Max(
          fullZoomedLowerRight.X, fullZoomedSize.Width));

        // If the top or bottom edge of the design area is showing, adjust the
        // minimum and/or maximum scroll bar value; otherwise the minimum is
        // zero (the top edge of the full zoomed image) and the maximum is the
        // bottom edge of the full zoomed image.
        zoomedImageVerticalScrollBar.Minimum = (int)Math.Floor(Math.Min(
          -fullZoomedUpperLeft.Y, 0));
        zoomedImageVerticalScrollBar.Maximum = (int)Math.Ceiling(Math.Max(
          fullZoomedLowerRight.Y, fullZoomedSize.Height));

        // Compute scroll knob size to correspond to the portion of the full
        // zoomed image that can be visible. Since our units are zoomed image
        // pixels, these are just the sizes of the actual display area,
        // limited by the size of the full zoomed image (i.e. the knob size
        // can't be bigger than the full zoomed image size).
        int newHorizontalKnobSize = (int)Math.Ceiling(Math.Min(
          zoomedImageSize.Width, fullZoomedSize.Width));
        int newVerticalKnobSize = (int)Math.Ceiling(Math.Min(
          zoomedImageSize.Height, fullZoomedSize.Height));

        // If a knob size is becoming larger, we may need to set the
        // corresponding scroll value lower before setting the new knob size,
        // since the value can't be larger than the scroll maximum minus half
        // the knob size.
        int maxHorizontalValue = zoomedImageHorizontalScrollBar.Maximum -
          newHorizontalKnobSize / 2;
        if (zoomedImageHorizontalScrollBar.Value > maxHorizontalValue)
        {
          zoomedImageHorizontalScrollBar.Value = maxHorizontalValue;
        }

        int maxVerticalValue = zoomedImageVerticalScrollBar.Maximum -
          newVerticalKnobSize / 2;
        if (zoomedImageVerticalScrollBar.Value > maxVerticalValue)
        {
          zoomedImageVerticalScrollBar.Value = maxVerticalValue;
        }

        // Now set the new knob sizes.
        zoomedImageHorizontalScrollBar.LargeChange = newHorizontalKnobSize;
        zoomedImageVerticalScrollBar.LargeChange = newVerticalKnobSize;

        // Set scroll values to the position of the upper left of the display
        // (i.e. 0, 0) relative to the upper left of the full zoomed image.
        zoomedImageHorizontalScrollBar.Value =
          Math.Max(Math.Min((int)Math.Floor(
          -fullZoomedUpperLeft.X),
          zoomedImageHorizontalScrollBar.Maximum),
          zoomedImageHorizontalScrollBar.Minimum);
        zoomedImageVerticalScrollBar.Value =
          Math.Max(Math.Min((int)Math.Floor(
          -fullZoomedUpperLeft.Y),
          zoomedImageVerticalScrollBar.Maximum),
          zoomedImageVerticalScrollBar.Minimum);
      }
    }

    private void zoomResetButton_Click(object sender, EventArgs e)
    {
      if (chainmailleDesign != null &&
          chainmailleDesign.RenderedImage != null &&
          renderedImagePictureBox.BackgroundImage != null)
      {
        float newZoomFactor = 1.0F;

        PointF newZoomCenterInZoomedImage = new PointF(
          0.5F * renderedImagePictureBox.BackgroundImage.Width,
          0.5F * renderedImagePictureBox.BackgroundImage.Height);

        PointF newZoomCenterInRenderedImage = new PointF(
          0.5F * chainmailleDesign.RenderedImage.Width,
          0.5F * chainmailleDesign.RenderedImage.Height);

        SetZoom(newZoomFactor, newZoomCenterInZoomedImage,
          newZoomCenterInRenderedImage);

        ShowRenderedImageAtZoom();
        RefreshScrollBars();
        ShowZoomControlValues();
      }
    }

    private void zoomTextBox_TextChanged(object sender, EventArgs e)
    {
      if (zoomControlResponseEnabled)
      {
        try
        {
          SetZoom(0.01F * int.Parse(zoomTextBox.Text));
          ShowRenderedImageAtZoom();
          RefreshScrollBars();
          ShowZoomControlValues();
        }
        catch { }
      }
    }

    private void zoomTrackBar_ValueChanged(object sender, EventArgs e)
    {
      if (zoomControlResponseEnabled)
      {
        try
        {
          PointF zoomedImageCenter = new PointF(
            0.5F * renderedImagePictureBox.BackgroundImage.Width,
            0.5F * renderedImagePictureBox.BackgroundImage.Height);
          SetZoom((float)Math.Pow(10.0, 0.1 * zoomTrackBar.Value),
            zoomedImageCenter,
            ZoomedImagePointFToRenderedImage(zoomedImageCenter));
          ShowRenderedImageAtZoom();
          RefreshScrollBars();
          ShowZoomControlValues();
        }
        catch { }
      }
    }

    private void ShowZoomControlValues()
    {
      zoomControlResponseEnabled = false;

      // Set the control values.
      zoomTextBox.Text = ((int)(Math.Round(100 * zoomFactor))).ToString();
      int trackBarValue = ((int)(Math.Round(10 * Math.Log10(zoomFactor))));
      trackBarValue = Math.Min(Math.Max(zoomTrackBar.Minimum, trackBarValue),
        zoomTrackBar.Maximum);
      zoomTrackBar.Value = trackBarValue;

      zoomControlResponseEnabled = true;
    }

    private Point ZoomedImagePointToRenderedImage(Point zoomedPoint)
    {
      PointF renderedImagePoint =
        ZoomedImagePointFToRenderedImage(zoomedPoint);
      return new Point(
        (int)Math.Round(renderedImagePoint.X),
        (int)Math.Round(renderedImagePoint.Y));
    }

    private PointF ZoomedImagePointFToRenderedImage(PointF zoomedPoint)
    {
      PointF unzoomedPoint = ZoomedImagePointFToUnzoomed(zoomedPoint);
      PointF renderedImagePoint = unzoomedPoint;
      if (chainmailleDesign != null)
      {
        renderedImagePoint = chainmailleDesign.
          TransformDisplayPointFToRenderedImage(unzoomedPoint);
      }
      return renderedImagePoint;
    }

    private PointF ZoomedImagePointFToOverlayImage(PointF zoomedPoint)
    {
      PointF unzoomedPoint = ZoomedImagePointFToUnzoomed(zoomedPoint);
      PointF overlayImagePoint = unzoomedPoint;
      if (chainmailleDesign != null)
      {
        overlayImagePoint = chainmailleDesign.
          TransformDisplayPointFToOverlayImage(unzoomedPoint);
      }
      return overlayImagePoint;
    }

    private Point ZoomedImagePointToUnzoomed(Point zoomedPoint)
    {
      PointF unzoomedPoint = ZoomedImagePointFToUnzoomed(zoomedPoint);
      return new Point(
        (int)Math.Round(unzoomedPoint.X), (int)Math.Round(unzoomedPoint.Y));
    }

    private PointF ZoomedImagePointFToUnzoomed(PointF zoomedPoint)
    {
      PointF zoomCenterInUnzoomedImage = zoomCenterInZoomedImage;
      if (chainmailleDesign != null)
      {
        zoomCenterInUnzoomedImage = chainmailleDesign.
          TransformRenderedImagePointFToDisplay(zoomCenterInRenderedImage);
      }
      return new PointF(
        zoomCenterInUnzoomedImage.X +
          (zoomedPoint.X - zoomCenterInZoomedImage.X) / zoomFactor,
        zoomCenterInUnzoomedImage.Y +
          (zoomedPoint.Y - zoomCenterInZoomedImage.Y) / zoomFactor);
    }

    private PointF UnzoomedImagePointFToZoomed(PointF unzoomedPoint)
    {
      PointF zoomCenterInUnzoomedImage = zoomCenterInZoomedImage;
      if (chainmailleDesign != null)
      {
        zoomCenterInUnzoomedImage = chainmailleDesign.
          TransformRenderedImagePointFToDisplay(zoomCenterInRenderedImage);
      }
      return new PointF(
        zoomCenterInZoomedImage.X +
        zoomFactor * (unzoomedPoint.X - zoomCenterInUnzoomedImage.X),
        zoomCenterInZoomedImage.Y +
        zoomFactor * (unzoomedPoint.Y - zoomCenterInUnzoomedImage.Y));
    }

    private PointF RenderedImagePointFToZoomedImage(PointF renderedimagePoint)
    {
      PointF unzoomedPoint = renderedimagePoint;
      if (chainmailleDesign != null)
      {
        unzoomedPoint = chainmailleDesign.
          TransformRenderedImagePointFToDisplay(renderedimagePoint);
      }
      return UnzoomedImagePointFToZoomed(unzoomedPoint);
    }

    private void ShowPatternElementId(ChainmaillePatternElementId elementId,
      Color color)
    {
      if (elementId != null)
      {
        string text = elementId.Column.ToString() + "   " +
          elementId.Row.ToString() + "   " + elementId.ElementIndex;
        if (color != Color.Transparent && paletteForm.Palette != null)
        {
          // Describe the color too.
          text += "   " + paletteForm.Palette.DescribeColor(color);
        }
        patternElementIdLabel.Text = text;
      }
      else
      {
        patternElementIdLabel.Text = string.Empty;
      }
    }

    private void ShowDrawingStepInstructions()
    {
      string text = string.Empty;

      switch (currentDrawingMode)
      {
        case DrawingMode.Line:
          switch (drawingModeStepNr)
          {
            case 1:
              text = "Click: Start of line.";
              break;
            case 2:
              text = "Click: End of line.";
              break;
          }
          break;
        case DrawingMode.Rectangle:
          switch (drawingModeStepNr)
          {
            case 1:
              text = "Click: Corner of rectangle.";
              break;
            case 2:
              text = "Click: Opposite corner of rectangle.";
              break;
          }
          break;
        case DrawingMode.Ellipse:
          switch (drawingModeStepNr)
          {
            case 1:
              text = "Click: Center of ellipse or circle.";
              break;
            case 2:
              text = "Click: Complete the circle." +
                "     Shift-click: Complete the ellipse.";
              break;
          }
          break;
        case DrawingMode.Polygon:
          switch (drawingModeStepNr)
          {
            case 1:
              text = "Click: First vertex of polygon.";
              break;
            case 2:
              text = "Click: Next vertex of polygon.";
              break;
            case 3:
              text = "Click: Next vertex of polygon." +
                "     Shift-click: Complete the polygon.";
              break;
          }
          break;
      }

      patternElementIdLabel.Text = text;
    }

    private void ShowGuidelineInstructions()
    {
      patternElementIdLabel.Text = "Click: Set guideline position.";
    }

    private void renderedImagePictureBox_MouseLeave(object sender, EventArgs e)
    {
      lastMouseDownElementId = null;
      mouseIsDown = false;
      if (mouseMode != MouseMode.Drawing &&
          mouseMode != MouseMode.SettingHorizontalGuideline &&
          mouseMode != MouseMode.SettingVerticalGuideline)
      {
        ShowPatternElementId(null, Color.Transparent);
      }
    }

    private void renderedImagePictureBox_MouseMove(object sender,
      MouseEventArgs e)
    {
      if (chainmailleDesign != null)
      {
        if (mouseMode == MouseMode.None)
        {
          // The cursor has moved, but is not effecting an action. Just update
          // the element id display to identify the ring under the cursor.
          ChainmaillePatternElement referencedElement;
          ChainmaillePatternElementId elementId =
            chainmailleDesign.PatternElementAtRenderedPoint(
            ZoomedImagePointToRenderedImage(e.Location),
            out referencedElement);
          Color elementColor = chainmailleDesign.GetElementColor(elementId,
            referencedElement);
          ShowPatternElementId(elementId, elementColor);
        }
        else if (mouseMode == MouseMode.PaintingRings)
        {
          // The cursor has moved while we are painting rings. Determine which
          // ring is under the cursor, then paint it with the current color.
          // Also update the element id display.
          ChainmaillePatternElement referencedElement;
          ChainmaillePatternElementId elementId =
            chainmailleDesign.PatternElementAtRenderedPoint(
            ZoomedImagePointToRenderedImage(e.Location),
            out referencedElement);
          if (elementId != null && mouseIsDown &&
              (string.IsNullOrEmpty(ringSizeFilter) ||
               referencedElement.RingSizeName == ringSizeFilter) &&
              (lastMouseDownElementId == null ||
               elementId.Column != lastMouseDownElementId.Column ||
               elementId.Row != lastMouseDownElementId.Row ||
               elementId.ElementIndex != lastMouseDownElementId.ElementIndex))
          {
            // Determine color to apply.
            Color color = ColorForMouseButton(e, mouseDownKeys);
            ShowPatternElementId(elementId, color);

            // Color the element.
            chainmailleDesign.SetElementColor(elementId, color,
              referencedElement);
            chainmailleDesign.RenderPatternElement(elementId,
              referencedElement);
            ShowRenderedImageAtZoom();

            lastMouseDownElementId = elementId;
          }
          else
          {
            Color elementColor = chainmailleDesign.GetElementColor(elementId,
              referencedElement);
            ShowPatternElementId(elementId, elementColor);
          }
        }
        else if (mouseMode == MouseMode.MovingOverlay)
        {
          // The cursor has moved while we are moving the overlay.
          // Determine the new position for the overlay center relative to
          // the rendered image. We must transform cursor movement on the
          // display to to movement on the rendered image, i.e. to take
          // account of the design rotation and zoom factor.
          PointF currentPointInRenderedImage =
            ZoomedImagePointFToRenderedImage(e.Location);
          chainmailleDesign.OverlayCenterInRenderedImage = new PointF(
            mouseDownOverlayCenterInRenderedImage.X +
            currentPointInRenderedImage.X - mouseDownPointInRenderedImage.X,
            mouseDownOverlayCenterInRenderedImage.Y +
            currentPointInRenderedImage.Y - mouseDownPointInRenderedImage.Y);
          ShowRenderedImageAtZoom();
        }
        else if (mouseMode == MouseMode.ScalingOverlay)
        {
          // Scaling is done relative to the overlay center, in the overlay's
          // coordinate space.
          PointF currentPointInDisplayImage = e.Location;
          PointF scaleAdjustment = new PointF(
            currentPointInDisplayImage.X / mouseDownPointInDisplayImage.X,
            currentPointInDisplayImage.Y / mouseDownPointInDisplayImage.Y);
          Matrix rotationMatrix = new Matrix();
          // Counter-rotate for combined display and overlay rotations.
          rotationMatrix.Rotate(-MathUtils.NormalizeDegrees(
            chainmailleDesign.DisplayRotationDegrees +
            chainmailleDesign.OverlayRotationDegrees));
          PointF[] scaleAdjustmentArray = new PointF[] { scaleAdjustment };
          rotationMatrix.TransformVectors(scaleAdjustmentArray);
          PointF adjustedOverlayScaleFactors = new PointF(
            mouseDownOverlayXScaleFactor * Math.Abs(scaleAdjustmentArray[0].X),
            mouseDownOverlayYScaleFactor * Math.Abs(scaleAdjustmentArray[0].Y));
          chainmailleDesign.OverlayScaleFactors = adjustedOverlayScaleFactors;
          ShowRenderedImageAtZoom();
        }
        else if (mouseMode == MouseMode.Drawing)
        {
          if (drawingPoints.Count > 0)
          {
            // Redraw the underlying image, but don't show it yet.
            ShowRenderedImageAtZoom(true);

            // Draw the rubber band line(s).
            if (currentDrawingMode == DrawingMode.Line)
            {
              List<PointF> transformedPoints = new List<PointF>();
              transformedPoints.Add(UnzoomedImagePointFToZoomed(
                chainmailleDesign.TransformRenderedImagePointFToDisplay(
                drawingPoints[0])));
              transformedPoints.Add(e.Location);
              ShowRubberBand(transformedPoints);
            }
            else if (currentDrawingMode == DrawingMode.Rectangle)
            {
              List<PointF> transformedPoints = new List<PointF>();
              transformedPoints.Add(UnzoomedImagePointFToZoomed(
                chainmailleDesign.TransformRenderedImagePointFToDisplay(
                drawingPoints[0])));
              transformedPoints.Add(new PointF(transformedPoints[0].X, e.Location.Y));
              transformedPoints.Add(new PointF(e.Location.X, e.Location.Y));
              transformedPoints.Add(new PointF(e.Location.X, transformedPoints[0].Y));
              transformedPoints.Add(transformedPoints[0]);
              ShowRubberBand(transformedPoints);
            }
            else if (currentDrawingMode == DrawingMode.Polygon)
            {
              List<PointF> transformedPoints = new List<PointF>();
              foreach (PointF drawingPoint in drawingPoints)
              {
                transformedPoints.Add(UnzoomedImagePointFToZoomed(
                  chainmailleDesign.TransformRenderedImagePointFToDisplay(
                  drawingPoint)));
              }
              transformedPoints.Add(new PointF(e.Location.X, e.Location.Y));
              transformedPoints.Add(transformedPoints[0]);
              ShowRubberBand(transformedPoints);
            }
            else if (currentDrawingMode == DrawingMode.Ellipse)
            {
              PointF center = UnzoomedImagePointFToZoomed(
                chainmailleDesign.TransformRenderedImagePointFToDisplay(
                drawingPoints[0]));
              SizeF halfSize = new SizeF(
                Math.Abs(e.Location.X - center.X),
                Math.Abs(e.Location.Y - center.Y));
              float radius = MathUtils.RootSumSquares(
                halfSize.Width, halfSize.Height);
              RectangleF circleBox = new RectangleF(
                center.X - radius, center.Y - radius,
                2.0F * radius, 2.0F * radius);
              RectangleF ellipseBox = new RectangleF(
                center.X - halfSize.Width,
                center.Y - halfSize.Height,
                2.0F * halfSize.Width, 2.0F * halfSize.Height);
              ShowRubberEllipse(circleBox, ellipseBox);
            }
          }
        }
      }
    }

    private void renderedImagePictureBox_MouseWheel(object sender,
      MouseEventArgs e)
    {
      Keys keys = Control.ModifierKeys;
      if (keys == Keys.None)
      {
        SetZoom(zoomFactor * (1.0F + 0.0005F * e.Delta), e.Location,
          ZoomedImagePointFToRenderedImage(e.Location));
        ShowRenderedImageAtZoom();
        ShowZoomControlValues();
        RefreshScrollBars();
      }
      else if (keys == Keys.Shift)
      {
        float scaleScale = 1.0F + 0.0005F * e.Delta;
        chainmailleDesign.OverlayXScaleFactor *= scaleScale;
        chainmailleDesign.OverlayYScaleFactor *= scaleScale;
        ShowRenderedImageAtZoom();
        RefreshScrollBars();
      }
    }

    private void renderedImagePictureBox_MouseDown(object sender,
      MouseEventArgs e)
    {
      mouseIsDown = true;
      mouseDownKeys = Control.ModifierKeys;
      if (mouseMode == MouseMode.Drawing)
      {
        Drawing_MouseDown(e, mouseDownKeys);
      }
      else if (mouseMode == MouseMode.SettingHorizontalGuideline)
      {
        mouseDownPointInRenderedImage =
          ZoomedImagePointFToRenderedImage(e.Location);
        SetHorizontalGuideline(mouseDownPointInRenderedImage);
        mouseMode = MouseMode.None;
      }
      else if (mouseMode == MouseMode.SettingVerticalGuideline)
      {
        mouseDownPointInRenderedImage =
          ZoomedImagePointFToRenderedImage(e.Location);
        SetVerticalGuideline(mouseDownPointInRenderedImage);
        mouseMode = MouseMode.None;
      }
      else
      {
        mouseMode = MouseMode.None;
        if (chainmailleDesign != null)
        {
          if (mouseDownKeys == Keys.Shift)
          {
            // Begin moving the overlay relative to the rendered image.
            mouseMode = MouseMode.MovingOverlay;
            mouseDownPointInRenderedImage =
              ZoomedImagePointFToRenderedImage(e.Location);
            mouseDownOverlayCenterInRenderedImage.X =
              chainmailleDesign.OverlayCenterInRenderedImage.X;
            mouseDownOverlayCenterInRenderedImage.Y =
              chainmailleDesign.OverlayCenterInRenderedImage.Y;
          }
          else if (mouseDownKeys == Keys.Control)
          {
            // Begin scaling the overlay relative to the overlay center.
            mouseMode = MouseMode.ScalingOverlay;
            mouseDownPointInDisplayImage = e.Location;
            mouseDownOverlayXScaleFactor =
              chainmailleDesign.OverlayXScaleFactor;
            mouseDownOverlayYScaleFactor =
              chainmailleDesign.OverlayYScaleFactor;
          }
          else if (mouseDownKeys == Keys.None || mouseDownKeys == Keys.Alt)
          {
            mouseMode = MouseMode.PaintingRings;

            // Determine the element to color.
            ChainmaillePatternElement referencedElement;
            ChainmaillePatternElementId elementId =
              chainmailleDesign.PatternElementAtRenderedPoint(
              ZoomedImagePointToRenderedImage(e.Location),
              out referencedElement);
            if (elementId != null &&
                (string.IsNullOrEmpty(ringSizeFilter) ||
                 referencedElement.RingSizeName == ringSizeFilter))
            {
              // The mouse is down over an element.
              lastMouseDownElementId = elementId;

              // Determine color to apply.
              Color color = ColorForMouseButton(e, mouseDownKeys);

              // Color the element.
              chainmailleDesign.SetElementColor(elementId, color,
                referencedElement);
              chainmailleDesign.RenderPatternElement(elementId,
                referencedElement);
              ShowRenderedImageAtZoom();
            }
          }
        }
      }
    }

    private Color ColorForMouseButton(MouseEventArgs e, Keys keys)
    {
      // Determine color to apply. The middle button will always use the
      // "unspecified element" color.
      Color result = Properties.Settings.Default.UnspecifiedElementColor;
      if (keys != Keys.Alt)
      {
        if (e.Button == MouseButtons.Left)
        {
          // Set the color to the selected left button color in the palette.
          result = paletteForm.LeftButtonColor;
        }
        else if (e.Button == MouseButtons.Right)
        {
          // Set the color to the selected right button color in the palette.
          result = paletteForm.RightButtonColor;
        }
      }

      return result;
    }

    private void renderedImagePictureBox_MouseUp(object sender,
      MouseEventArgs e)
    {
      lastMouseDownElementId = null;
      mouseIsDown = false;
      if (mouseMode != MouseMode.Drawing)
      {
        mouseMode = MouseMode.None;
      }
    }

    private void ChainmailleDesignerForm_ResizeEnd(object sender, EventArgs e)
    {
      // Make a new zoomed image and graphics, and dispose of the old ones.
      Image oldImage = renderedImagePictureBox.BackgroundImage;
      renderedImagePictureBox.BackgroundImage = new Bitmap(
        renderedImagePictureBox.ClientSize.Width,
        renderedImagePictureBox.ClientSize.Height);
      if (oldImage != null)
      {
        oldImage.Dispose();
      }
      if (zoomGraphics != null)
      {
        zoomGraphics.Dispose();
      }
      zoomGraphics = Graphics.FromImage(
        renderedImagePictureBox.BackgroundImage);
      SetZoom(null, new PointF(
        0.5F * renderedImagePictureBox.BackgroundImage.Width,
        0.5F * renderedImagePictureBox.BackgroundImage.Height));
      ShowRenderedImageAtZoom();
      RefreshScrollBars();
    }

    private void ChainmailleDesignerForm_FormClosing(object sender,
      FormClosingEventArgs e)
    {
      bool continueWithOperation = true;
      if (chainmailleDesign != null && chainmailleDesign.HasBeenChanged)
      {
        // Ask whether to save.
        DialogResult dialogResult = AskWhetherToSaveDesignChanges();
        if (dialogResult == DialogResult.Yes)
        {
          if (string.IsNullOrEmpty(chainmailleDesign.DesignFile))
          {
            saveDesignAsToolStripMenuItem_Click(sender, e);
          }
          else
          {
            SetPaletteInDesign();
            chainmailleDesign.Save();
          }
        }
        else if (dialogResult == DialogResult.Cancel)
        {
          continueWithOperation = false;
        }
      }
      if (continueWithOperation)
      {
        continueWithOperation = paletteForm.TryClose();
      }
      e.Cancel = !continueWithOperation;
      if (continueWithOperation)
      {
        // Remove the design.
        chainmailleDesign = null;

        // Dispose of the image in the picture box.
        zoomGraphics.Dispose();
        renderedImagePictureBox.BackgroundImage.Dispose();
        renderedImagePictureBox.BackgroundImage = null;
      }

      EnableMenuItems();
      EnableRingSizeControls();
      EnableOverlayControls();
    }

    private void zoomedImageHorizontalScrollBar_Scroll(object sender,
      ScrollEventArgs e)
    {
      if (scrollBarResponseEnabled)
      {
        float newZoomCenterX = zoomCenterInZoomedImage.X +
          zoomedImageHorizontalScrollBar.Value + fullZoomedUpperLeft.X;
        SetZoom(null, null, ZoomedImagePointFToRenderedImage(new PointF(
          newZoomCenterX, zoomCenterInZoomedImage.Y)));
        ShowRenderedImageAtZoom();
      }
    }

    private void zoomedImageVerticalScrollBar_Scroll(object sender,
      ScrollEventArgs e)
    {
      if (scrollBarResponseEnabled)
      {
        float newZoomCenterY = zoomCenterInZoomedImage.Y +
          zoomedImageVerticalScrollBar.Value + fullZoomedUpperLeft.Y;
        SetZoom(null, null, ZoomedImagePointFToRenderedImage(new PointF(
          zoomCenterInZoomedImage.X, newZoomCenterY)));
        ShowRenderedImageAtZoom();
      }
    }

    private void SetPaletteInDesign()
    {
      chainmailleDesign.PaletteFile = paletteForm.Palette != null ?
        paletteForm.Palette.PaletteFile : string.Empty;
      chainmailleDesign.PaletteName = paletteForm.Palette != null ?
        paletteForm.Palette.Name : string.Empty;
      chainmailleDesign.PaletteSectionsHidden = paletteForm.Palette != null ?
        paletteForm.Palette.HiddenSections : new List<string>();
    }

    private void saveDesignToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (chainmailleDesign != null)
      {
        if (string.IsNullOrEmpty(chainmailleDesign.DesignFile))
        {
          saveDesignAsToolStripMenuItem_Click(sender, e);
        }
        else
        {
          SetPaletteInDesign();
          chainmailleDesign.Save();
        }
      }
      else
      {
        MessageBox.Show(this,
          "No design is currently defined, so the design can't be saved.",
          "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      EnableMenuItems();
    }

    private void saveDesignAsToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      if (chainmailleDesign != null)
      {
        SaveFileDialog dlg = new SaveFileDialog();
        dlg.InitialDirectory = Properties.Settings.Default.DesignDirectory;
        dlg.Filter = Properties.Settings.Default.DesignFileOutFilter;
        dlg.FilterIndex = 1;
        dlg.FileName = chainmailleDesign.DesignName;
        dlg.Title = "Save Design";
        dlg.FileName = chainmailleDesign.DesignName;
        if (dlg.ShowDialog() == DialogResult.OK)
        {
          chainmailleDesign.DesignFile = dlg.FileName;
          SetPaletteInDesign();
          chainmailleDesign.Save();
        }
      }
      else
      {
        MessageBox.Show(this,
          "No design is currently defined, so the design can't be saved.",
          "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      EnableMenuItems();
    }

    private void printRenderedImageToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      if (chainmailleDesign != null && chainmailleDesign.RenderedImage != null)
      {
        // Since we will print the transformed rendered image, base the
        // portrait / landscape default on the transformed image dimensions.
        Size renderedImageSize =
          chainmailleDesign.TransformedRenderedImageSize;
        // Set up the print document.
        PrintDocument printDocument = new PrintDocument();
        printDocument.DocumentName = chainmailleDesign.DesignName;
        printDocument.DefaultPageSettings.Landscape =
          renderedImageSize.Width > renderedImageSize.Height;
        printDocument.PrintPage +=
          new PrintPageEventHandler(PrintRenderedImage);

        // Show the print dialog.
        PrintDialog printDialog = new PrintDialog();
        printDialog.Document = printDocument;
        if (printDialog.ShowDialog() == DialogResult.OK)
        {
          // Do the printing.
          printDocument.Print();
        }
      }
      else
      {
        MessageBox.Show(this, "No design is currently defined, " +
          "so the rendered design can't be printed.", "Nothing to Print",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }

    private void PrintRenderedImage(object sender, PrintPageEventArgs e)
    {
      // Apply display transformation before printing.
      Bitmap transformedRenderedImage =
        chainmailleDesign.TransformedRenderedImage;
      if (transformedRenderedImage != null)
      {
        // Draw the image to fit the page.
        // (The user has already had the opportunity to set paper size,
        // portrait / landscape, etc.)
        Rectangle pageRectangle = e.MarginBounds;
        Size renderedImageSize = transformedRenderedImage.Size;
        double zoom = Math.Min(
          1.0 * pageRectangle.Width / renderedImageSize.Width,
          1.0 * pageRectangle.Height / renderedImageSize.Height);
        Size printSize = new Size(
          (int)(zoom * renderedImageSize.Width),
          (int)(zoom * renderedImageSize.Height));
        Rectangle destinationRectangle = new Rectangle(
          pageRectangle.X + (pageRectangle.Width - printSize.Width) / 2,
          pageRectangle.Y + (pageRectangle.Height - printSize.Height) / 2,
          printSize.Width, printSize.Height);
        e.Graphics.DrawImage(transformedRenderedImage,
          destinationRectangle);
        // Dispose of the transformed image.
        transformedRenderedImage.Dispose();
      }
    }

    private void saveRenderedImageToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      if (chainmailleDesign != null)
      {
        SaveFileDialog dlg = new SaveFileDialog();
        dlg.InitialDirectory = Properties.Settings.Default.DesignDirectory;
        dlg.Filter = Properties.Settings.Default.ImageFileFilter;
        dlg.FilterIndex = 5;
        dlg.FileName = chainmailleDesign.DesignName + " - Rendered";
        dlg.Title = "Save Rendered Image";
        if (dlg.ShowDialog() == DialogResult.OK)
        {
          chainmailleDesign.SaveRenderedImage(dlg.FileName);
        }
      }
      else
      {
        MessageBox.Show(this, "No design is currently defined, " +
          "so the rendered design can't be saved.", "Nothing to Save",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }

    public void BackgroundColorHasChanged(Color newColor)
    {
      Properties.Settings.Default.BackgroundColor = newColor;
      Properties.Settings.Default.Save();
      if (chainmailleDesign != null)
      {
        chainmailleDesign.BackgroundColorHasChanged(newColor);
        ShowRenderedImageAtZoom();
      }
    }

    public void OutlineColorHasChanged(Color newColor)
    {
      Properties.Settings.Default.OutlineColor = newColor;
      Properties.Settings.Default.Save();
      if (chainmailleDesign != null)
      {
        chainmailleDesign.OutlineColorHasChanged(newColor);
        ShowRenderedImageAtZoom();
      }
    }

    public void ShowPaletteForm(bool show)
    {
      if (show)
      {
        paletteForm.Show();
      }
      else
      {
        paletteForm.Hide();
      }
      paletteFormIsShowing = show;
      paletteWindowToolStripMenuItem.Text =
        paletteFormIsShowing ? "Hide Palette" : "Show Palette";
    }

    private void paletteWindowToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      ShowPaletteForm(!paletteFormIsShowing);
      // Regain focus. If palette was shown, it will have been given focus.
      Focus();
    }

    private void helpAboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ChainmailleDesignerAboutBox aboutBox = new ChainmailleDesignerAboutBox();
      aboutBox.Show();
    }

    private void countColorsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Palette palette = paletteForm.Palette;
      if (chainmailleDesign != null && chainmailleDesign.ColorImage != null &&
          chainmailleDesign.ColorImage.BitmapImage != null)
      {
        ColorCounter colorCounter = new ColorCounter(
          chainmailleDesign, palette);
        ColorReportForm colorReportForm = new ColorReportForm();
        colorReportForm.DesignName = chainmailleDesign.DesignName;
        colorReportForm.Show();
        colorReportForm.ColorCounter = colorCounter;
        colorReportForm.Focus();
      }
    }

    private void EnableMenuItems()
    {
      bool haveADesign = chainmailleDesign != null;
      bool designHasAFilename = haveADesign &&
        !string.IsNullOrEmpty(chainmailleDesign.DesignFile);
      bool designHasAnOverlay = haveADesign &&
        chainmailleDesign.OverlayImage != null;

      fileCloseToolStripMenuItem.Enabled = haveADesign;
      saveDesignToolStripMenuItem.Enabled = designHasAFilename;
      saveDesignAsToolStripMenuItem.Enabled = haveADesign;
      saveRenderedImageToolStripMenuItem.Enabled = haveADesign;
      printRenderedImageToolStripMenuItem.Enabled = haveADesign;
      printToolStripMenuItem.Enabled = haveADesign;
      designToolStripMenuItem.Enabled = haveADesign;
      colorsToolStripMenuItem.Enabled = haveADesign;
      colorRingsFromOverlayToolStripMenuItem.Enabled = designHasAnOverlay;
      rotateToolStripMenuItem1.Enabled = designHasAnOverlay;
      showHideOverlayToolStripMenuItem.Enabled = designHasAnOverlay;

      shapesGroupBox.Enabled = haveADesign;
      fillGroupBox.Enabled = haveADesign;
      ringFilterGroupBox.Enabled = haveADesign;
    }

    private void EnableOverlayControls()
    {
      overlayGroupBox.Visible = chainmailleDesign != null &&
        chainmailleDesign.OverlayImage != null;
      if (overlayGroupBox.Visible)
      {
        if (ringFilterGroupBox.Visible)
        {
          // Position below the ring filter box.
          overlayGroupBox.Location = new Point(overlayGroupBox.Location.X,
            ringFilterGroupBox.Location.Y + ringFilterGroupBox.Size.Height +
            (int)(yScaleFactor * groupBoxSpacing));
        }
        else
        {
          // Position below the progress bar.
          overlayGroupBox.Location = new Point(overlayGroupBox.Location.X,
            shapeProgressBar.Location.Y + shapeProgressBar.Size.Height +
            (int)(yScaleFactor * groupBoxSpacing));
        }
        overlayControlResponseEnabled = false;
        overlayCheckBox.Checked = chainmailleDesign.ShowOverlay;
        overlayOpacityTrackBar.Value = 100 -
          (int)(chainmailleDesign.OverlayTransparency * 100F + 0.5F);
        overlayOpacityLabel.Text =
          "Opacity " + overlayOpacityTrackBar.Value + "%";
        overlayControlResponseEnabled = true;
      }
    }

    private void EnableRingSizeControls()
    {
      bool showRingSizeControls = false;

      // Clear the ring size controls.
      ringFilterGroupBox.Controls.Clear();
      ringSizeFilter = string.Empty;
      // Add the control for all ring sizes.
      int sizeIndex = 0;
      AddRingFilterButton(sizeIndex++, "All Rings", true);
      ringFilterRadioButton0.Checked = true;

      if (chainmailleDesign != null &&
          chainmailleDesign.ChainmailPattern != null &&
          chainmailleDesign.ChainmailPattern.HasRingSizeNames &&
          chainmailleDesign.ChainmailPattern.HasScales)
      {
        PatternScale firstScale = chainmailleDesign.ChainmailPattern.Scales[0];
        if (firstScale.RingSizes.Count > 1)
        {
          showRingSizeControls = true;
          foreach (RingSize ringSize in firstScale.RingSizes)
          {
            if (!string.IsNullOrEmpty(ringSize.Name))
            {
              AddRingFilterButton(sizeIndex++, ringSize.Name);
            }
          }
        }
      }

      ringFilterGroupBox.Visible = showRingSizeControls;
    }

    private void AddRingFilterButton(int filterIndex, string filterName,
      bool isDefault = false)
    {
      RadioButton filterRadioButton = new RadioButton();
      filterRadioButton.Location = new Point(
      (int)(10 * xScaleFactor),
      (int)((35 + 35 * filterIndex) * yScaleFactor));
      filterRadioButton.Name = ringFilterNameBase + filterIndex;
      filterRadioButton.TabIndex = filterIndex;
      filterRadioButton.AutoSize = true;
      filterRadioButton.Text = filterName;
      filterRadioButton.Checked = isDefault;
      filterRadioButton.CheckedChanged +=
        new System.EventHandler(RingFilterButton_CheckedChanged);
      ringFilterGroupBox.Controls.Add(filterRadioButton);
    }

    private void RingFilterButton_CheckedChanged(object sender, EventArgs e)
    {
      // For each action, one button is checked, while another is unchecked.
      // We only want to act on the checked button.
      RadioButton radioButton = sender as RadioButton;
      if (radioButton.Checked)
      {
        ringSizeFilter = radioButton.Text == "All Rings" ?
          string.Empty : radioButton.Text;
      }
    }

    private void ShapeButton_CheckedChanged(object sender, EventArgs e)
    {
      // For each action, one button is checked, while another is unchecked.
      // We only want to act on the checked button.
      RadioButton radioButton = sender as RadioButton;
      if (radioButton.Checked)
      {
        currentDrawingMode = EnumUtils.ToEnum<DrawingMode>(radioButton.Text);
        if (currentDrawingMode != DrawingMode.Freehand)
        {
          drawingModeStepNr = 1;
          ShowDrawingStepInstructions();
          mouseMode = MouseMode.Drawing;
        }
        else
        {
          drawingModeStepNr = 0;
          ShowDrawingStepInstructions();
          mouseMode = MouseMode.None;
        }
      }
    }

    private void Drawing_MouseDown(MouseEventArgs e, Keys mouseKeys)
    {
      bool doneDrawing = false;

      if (drawingModeStepNr == 1)
      {
        drawingColor = ColorForMouseButton(e, mouseDownKeys);
      }

      switch (currentDrawingMode)
      {
        case DrawingMode.Line:
          doneDrawing = NextLineDrawingStep(e);
          break;
        case DrawingMode.Rectangle:
          doneDrawing = NextRectangleDrawingStep(e);
          break;
        case DrawingMode.Ellipse:
          doneDrawing = NextEllipseDrawingStep(e, mouseDownKeys);
          break;
        case DrawingMode.Polygon:
          doneDrawing = NextPolygonDrawingStep(e, mouseDownKeys);
          break;
      }

      if (doneDrawing)
      {
        mouseMode = MouseMode.None;
        freehandShapeRadioButton.Checked = true;
        drawingPoints.Clear();
        ShowRenderedImageAtZoom();
        ShapeProgressValue = 0;
      }
      else
      {
        drawingModeStepNr++;
        ShowDrawingStepInstructions();
      }
    }

    private bool NextEllipseDrawingStep(MouseEventArgs e, Keys mouseDownKeys)
    {
      bool doneDrawing = false;

      switch (drawingModeStepNr)
      {
        case 1:
          drawingPoints = new List<PointF>()
            { ZoomedImagePointFToRenderedImage(e.Location) };
          break;
        case 2:
          drawingPoints.Add(
            ZoomedImagePointFToRenderedImage(e.Location));
          if (mouseDownKeys == Keys.Shift)
          {
            chainmailleDesign.DrawEllipse(drawingPoints, drawingColor,
              currentFillMode, ringSizeFilter, this);
          }
          else
          {
            chainmailleDesign.DrawCircle(drawingPoints, drawingColor,
              currentFillMode, ringSizeFilter, this);
          }
          doneDrawing = true;
          break;
      }

      return doneDrawing;
    }

    private bool NextLineDrawingStep(MouseEventArgs e)
    {
      bool doneDrawing = false;

      switch (drawingModeStepNr)
      {
        case 1:
          drawingPoints = new List<PointF>()
            { ZoomedImagePointFToRenderedImage(e.Location) };
          break;
        case 2:
          drawingPoints.Add(
            ZoomedImagePointFToRenderedImage(e.Location));
          chainmailleDesign.DrawLine(drawingPoints, drawingColor,
            currentFillMode, ringSizeFilter, this);
          doneDrawing = true;
          break;
      }

      return doneDrawing;
    }

    private bool NextPolygonDrawingStep(MouseEventArgs e, Keys mouseDownKeys)
    {
      bool doneDrawing = false;

      switch (drawingModeStepNr)
      {
        case 1:
          drawingPoints = new List<PointF>()
            { ZoomedImagePointFToRenderedImage(e.Location) };
          break;
        case 2:
          drawingPoints.Add(
            ZoomedImagePointFToRenderedImage(e.Location));
          break;
        case 3:
          drawingPoints.Add(
            ZoomedImagePointFToRenderedImage(e.Location));
          if (mouseDownKeys != Keys.Shift)
          {
            // Stay on the same step.
            drawingModeStepNr--;
          }
          else
          {
            // Complete the polygon.
            chainmailleDesign.DrawPolygon(drawingPoints, drawingColor,
              currentFillMode, ringSizeFilter, this);
            doneDrawing = true;
          }
          break;
      }

      return doneDrawing;
    }

    private bool NextRectangleDrawingStep(MouseEventArgs e)
    {
      bool doneDrawing = false;

      switch (drawingModeStepNr)
      {
        case 1:
          drawingPoints = new List<PointF>()
            { ZoomedImagePointFToRenderedImage(e.Location) };
          break;
        case 2:
          drawingPoints.Add(
            ZoomedImagePointFToRenderedImage(e.Location));
          chainmailleDesign.DrawRectangle(drawingPoints, drawingColor,
            currentFillMode, ringSizeFilter, this);
          doneDrawing = true;
          break;
      }

      return doneDrawing;
    }
    public int ShapeProgressScale
    {
      set { shapeProgressBar.Maximum = value; }
    }

    public int ShapeProgressValue
    {
      set { shapeProgressBar.Value = value; }
    }

    private void FillButton_CheckedChanged(object sender, EventArgs e)
    {
      // For each action, one button is checked, while another is unchecked.
      // We only want to act on the checked button.
      RadioButton radioButton = sender as RadioButton;
      if (radioButton.Checked)
      {
        currentFillMode = EnumUtils.ToEnum<DrawingFillMode>(radioButton.Text);
      }
    }

    private void zoomFitButton_Click(object sender, EventArgs e)
    {
      if (chainmailleDesign != null &&
          chainmailleDesign.RenderedImage != null &&
          renderedImagePictureBox.BackgroundImage != null)
      {
        // Base fit zoom factor on the rotated rendered image.
        Size renderedImageSize =
          chainmailleDesign.TransformedRenderedImageSize;

        float newZoomFactor = Math.Min(
          1.0F * renderedImagePictureBox.BackgroundImage.Width /
          renderedImageSize.Width,
          1.0F * renderedImagePictureBox.BackgroundImage.Height /
          renderedImageSize.Height);

        PointF newZoomCenterInZoomedImage = new PointF(
          0.5F * renderedImagePictureBox.BackgroundImage.Width,
          0.5F * renderedImagePictureBox.BackgroundImage.Height);

        PointF newZoomCenterInRenderedImage = new PointF(
          0.5F * chainmailleDesign.RenderedImage.Width,
          0.5F * chainmailleDesign.RenderedImage.Height);

        SetZoom(newZoomFactor, newZoomCenterInZoomedImage,
          newZoomCenterInRenderedImage);

        ShowRenderedImageAtZoom();
        RefreshScrollBars();
        ShowZoomControlValues();
      }
    }

    private void openOverlayToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (chainmailleDesign != null)
      {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.InitialDirectory = Properties.Settings.Default.DesignDirectory;
        dlg.Filter = Properties.Settings.Default.ImageFileFilter;
        dlg.FilterIndex = 1;
        dlg.CheckFileExists = true;
        dlg.CheckPathExists = true;
        dlg.Multiselect = false;
        dlg.Title = "Open Overlay Image";
        if (dlg.ShowDialog() == DialogResult.OK)
        {
          // Set the overlay file for the design.
          chainmailleDesign.OverlayFile = dlg.FileName;
          chainmailleDesign.OverlayTransparency = defaultOverlayTransparency;
          chainmailleDesign.ShowOverlay = true;
          showHideOverlayToolStripMenuItem.Text =
            chainmailleDesign.ShowOverlay ? "Hide" : "Show";

          if (chainmailleDesign.OverlayImage != null &&
              chainmailleDesign.RenderedImage != null)
          {
            // Initially center the overlay in the rendered image and scale it
            // to fit over the transformed rendered image without distortion.
            Size transformedRenderedImageSize =
              chainmailleDesign.TransformedRenderedImageSize;
            chainmailleDesign.OverlayCenterInRenderedImage = new PointF(
              0.5F * chainmailleDesign.RenderedImage.Width,
              0.5F * chainmailleDesign.RenderedImage.Height);
            float overlayZoomFactor = Math.Min(
              1.0F * transformedRenderedImageSize.Width /
              chainmailleDesign.OverlayImage.Width,
              1.0F * transformedRenderedImageSize.Height /
              chainmailleDesign.OverlayImage.Height);
            chainmailleDesign.OverlayXScaleFactor = overlayZoomFactor;
            chainmailleDesign.OverlayYScaleFactor = overlayZoomFactor;
            // Set up the overlay rotation transformation.
            chainmailleDesign.RotateOverlay(0F, true);
          }

          // Display the design with the overlay.
          ShowRenderedImageAtZoom();
        }

        EnableMenuItems();
        EnableOverlayControls();
      }
    }

    private void infoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (chainmailleDesign != null)
      {
        DesignInfoForm dlg = new DesignInfoForm(chainmailleDesign, this);
        if (dlg.ShowDialog() == DialogResult.OK)
        {
          // Change design info.
          chainmailleDesign.DesignName = dlg.DesignName;
          chainmailleDesign.WeaveName = dlg.WeaveName;
          chainmailleDesign.Wrap = dlg.Wrap;
          chainmailleDesign.DesignedFor = dlg.DesignedFor;
          chainmailleDesign.DesignDate = dlg.DesignDate;
          chainmailleDesign.DesignedBy = dlg.DesignedBy;
          chainmailleDesign.Description = dlg.Description;
          if (!string.IsNullOrEmpty(dlg.WeaveFile))
          {
            chainmailleDesign.ApplyPattern(dlg.WeaveFile);
          }
          // Do only after weave is changed.
          chainmailleDesign.Scale = dlg.PatternScale;
          SizeAdjustment sizeAdjustment = dlg.SizeAdjustment;
          if (sizeAdjustment.IsSignificant)
          {
            chainmailleDesign.AdjustSize(sizeAdjustment);
          }
          EnableRingSizeControls();
          EnableOverlayControls();
          chainmailleDesign.RenderImage();
          ShowRenderedImageAtZoom();
        }
      }
    }

    private void configurationToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      ConfigurationForm dlg = new ConfigurationForm();
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        // Do something.
      }
    }

    private void showHelpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Help.ShowHelp(this, Properties.Settings.Default.HelpUrl);
    }

    private void licenseAgreementToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      DocumentDisplayForm dlg = new DocumentDisplayForm();
      dlg.Text = "License Agreement";
      dlg.DocumentFileName = Properties.Settings.Default.LicenseFileName;
      dlg.Show();
    }

    private void showHideOverlayToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      if (overlayControlResponseEnabled)
      {
        chainmailleDesign.ShowOverlay = !chainmailleDesign.ShowOverlay;
        showHideOverlayToolStripMenuItem.Text =
          chainmailleDesign.ShowOverlay ? "Hide" : "Show";
        overlayCheckBox.Checked = chainmailleDesign.ShowOverlay;
        ShowRenderedImageAtZoom();
      }
    }

    FormWindowState lastWindowState = FormWindowState.Minimized;
    private void ChainmailleDesignerForm_Resize(object sender, EventArgs e)
    {
      if (WindowState != lastWindowState)
      {
        if (WindowState == FormWindowState.Maximized ||
            WindowState == FormWindowState.Normal)
        {
          // Resize end doesn't get called by maximize / restore,
          // so call it explicitly here.
          ChainmailleDesignerForm_ResizeEnd(sender, e);
        }
        lastWindowState = WindowState;
      }
    }

    private void replaceInDesignToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      ColorReplacementForm dlg = new ColorReplacementForm(chainmailleDesign,
        paletteForm.Palette);
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        chainmailleDesign.ReplaceColorsInDesign(dlg.ColorReplacements,
          dlg.RingFilterString);
        ShowRenderedImageAtZoom();
      }
    }

    private void showHideGuidelinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      chainmailleDesign.ShowGuidelines = !chainmailleDesign.ShowGuidelines;
      showHideGuidelinesToolStripMenuItem.Text =
        chainmailleDesign.ShowGuidelines ? "Hide" : "Show";
      ShowRenderedImageAtZoom();
    }

    private void horizontalGuidelineToolStripMenuItem_Click(object sender, EventArgs e)
    {
      freehandShapeRadioButton.Checked = true;
      mouseMode = MouseMode.SettingHorizontalGuideline;
      ShowGuidelineInstructions();
    }

    private void verticalGuidelineToolStripMenuItem_Click(object sender, EventArgs e)
    {
      freehandShapeRadioButton.Checked = true;
      mouseMode = MouseMode.SettingVerticalGuideline;
      ShowGuidelineInstructions();
    }

    private void halvesGuidelinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SetEquallySpacedGuidelines(2);
    }

    private void thirdsGuidelinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SetEquallySpacedGuidelines(3);
    }

    private void quartersGuidelinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SetEquallySpacedGuidelines(4);
    }

    private void SetEquallySpacedGuidelines(int divisor)
    {
      if (chainmailleDesign != null)
      {
        Size sizeIncrement = new Size(
          chainmailleDesign.RenderedImage.Size.Width / divisor,
          chainmailleDesign.RenderedImage.Size.Height / divisor);
        Point guidelinePoint = new Point(
          sizeIncrement.Width, sizeIncrement.Height);
        for (int i = 1; i < divisor; i++)
        {
          chainmailleDesign.AddHorizontalGuideline(guidelinePoint.Y);
          chainmailleDesign.AddVerticalGuideline(guidelinePoint.X);
          guidelinePoint += sizeIncrement;
        }

        ShowRenderedImageAtZoom();
      }
    }

    private void SetHorizontalGuideline(PointF pointInRenderedImage)
    {
      if (chainmailleDesign.HVReversed)
      {
        chainmailleDesign.AddVerticalGuideline((int)pointInRenderedImage.X);
      }
      else
      {
        chainmailleDesign.AddHorizontalGuideline((int)pointInRenderedImage.Y);
      }

      ShowRenderedImageAtZoom();
    }

    private void SetVerticalGuideline(PointF pointInRenderedImage)
    {
      if (chainmailleDesign.HVReversed)
      {
        chainmailleDesign.AddHorizontalGuideline((int)pointInRenderedImage.Y);
      }
      else
      {
        chainmailleDesign.AddVerticalGuideline((int)pointInRenderedImage.X);
      }

      ShowRenderedImageAtZoom();
    }

    private void clearAllGuidelinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      chainmailleDesign.ClearAllGuidelines();
      showHideGuidelinesToolStripMenuItem.Text =
        chainmailleDesign.ShowGuidelines ? "Hide" : "Show";
      ShowRenderedImageAtZoom();
    }

    private void colorRingsFromOverlayToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ColorFromOverlayForm dlg = new ColorFromOverlayForm(chainmailleDesign,
        paletteForm.Palette);
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        chainmailleDesign.ColorDesignFromOverlay(
          dlg.PaletteSection, dlg.RingFilterString, this);
        ShowRenderedImageAtZoom();
      }
    }

    private void clearAllRingColorsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      chainmailleDesign.ClearAllRingColors();
      ShowRenderedImageAtZoom();
    }

    private void overlayCheckBox_Click(object sender, EventArgs e)
    {
      showHideOverlayToolStripMenuItem_Click(sender, e);
    }

    private void overlayOpacityTrackBar_ValueChanged(object sender, EventArgs e)
    {
      if (overlayControlResponseEnabled)
      {
        chainmailleDesign.OverlayTransparency =
          (100F - overlayOpacityTrackBar.Value) / 100F;
        overlayOpacityLabel.Text =
          "Opacity " + overlayOpacityTrackBar.Value + "%";
        ShowRenderedImageAtZoom();
      }
    }
  }
}
