// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmailleDesign.cs


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
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml;
using ChainmailleDesigner.Features;
using ChainmailleDesigner.Features.CommandHistorySupport;

using LabColor = System.Tuple<double, double, double>;

// Transformation specification for translating colors within the same color
// space. These are scale, offset pairs for each of three components.
using ColorTransform = System.Tuple<System.Tuple<double, double>,
  System.Tuple<double, double>, System.Tuple<double, double>>;

namespace ChainmailleDesigner
{
  /// <summary>
  /// A chainmaille design consists, minimally, of a chainmaille pattern
  /// (i.e. weave), dimensional information (such as number of rows,
  /// columns, or units), wrapping information (i.e. whether the
  /// design is for a sheet or a cylinder, and if the latter whether
  /// the cylinder connects horizontally or vertically), and a color
  /// image that specifies the colors of the rings.  It may optionally
  /// also have a design overlay image (such as a photograph or drawing),
  /// specify one or more specific ring sizes appropriate to the
  /// chainmaille pattern, and reference a specific color palette.
  /// </summary>
  public class ChainmailleDesign : IDisposable
  {
    private ColorImage colorImage = null;
    private ChainmaillePattern chainmaillePattern = null;
    private Bitmap renderedImage = null;
    private string messages = string.Empty;

    private bool hasBeenChanged = false;
    private string designFile = string.Empty;
    private string designFileVersion = "2";
    private string designName = string.Empty;
    private string weaveName = string.Empty;
    private string patternFile = string.Empty;
    private Size sizeInUnits = new Size(1, 1);
    private WrapEnum wrap = WrapEnum.None;
    private PatternScale scale = null;
    // Be a little careful here. The palette name is the name of the palette
    // most recently associated with the design. It is saved with the design
    // when the design is saved, and the palette saved with the design is
    // restored when the design is read in (assuming that it is still
    // available), but the user can create or open different palettes as
    // they work, and those changes will not be reflected in this palette name
    // until the next save of the design.
    private string paletteName = string.Empty;
    private string paletteFile = string.Empty;
    private List<string> hiddenPaletteSectionNames = new List<string>();

    string designedBy = Properties.Settings.Default.DesignerName;
    DateTime designDate = DateTime.UtcNow;
    string designedFor = string.Empty;
    string designDescription = string.Empty;

    private Color backgroundColor =
      Properties.Settings.Default.BackgroundColor;
    private Color outlineColor = Properties.Settings.Default.OutlineColor;
    private float displayRotationDegrees = 0F;
    private Matrix renderedImageToDisplayTransformation = new Matrix();
    private Matrix displayToRenderedImageTransformation = new Matrix();

    // Overlay.
    private string overlayName = string.Empty;
    private string overlayFile = string.Empty;
    private Bitmap overlayImage = null;
    private float overlayTransparency = 0.5F;
    private float overlayXScaleFactor = 1.0F;
    private float overlayYScaleFactor = 1.0F;
    private float overlayRotationDegrees = 0F;
    private Matrix overlayToRenderedImageTransformation = new Matrix();
    private Matrix renderedToOverlayImageTransformation = new Matrix();
    PointF overlayCenterInRenderedImage = new PointF();
    PointF overlayCenterInOverlayImage = new PointF();
    ImageAttributes overlayAttributes = new ImageAttributes();
    private bool overlayIsShowing = true;
    public static List<string> OverlayImageFileExtensions =
      new List<string>() { ".jpg", ".png", ".bmp", ".tif", ".gif" };

    // Guidelines
    SortedSet<int> horizontalGuidelines = new SortedSet<int>();
    SortedSet<int> verticalGuidelines = new SortedSet<int>();
    bool guidelinesAreShowing = true;
    
    // If the design has edge treatment, keep track of the edge offsets in both
    // chainmaille pattern units and the various images.
    //Size edgesSizeInBuildImageTotal = new Size(0, 0);
    //Size edgesSizeInBuildImageTopLeft = new Size(0, 0);
    //Size edgesSizeInBuildImageBottomRight = new Size(0, 0);
    Size edgesSizeInColorImageTotal = new Size(0, 0);
    Size edgesSizeInColorImageTopLeft = new Size(0, 0);
    Size edgesSizeInColorImageBottomRight = new Size(0, 0);
    Size edgesSizeInPatternUnitsTotal = new Size(0, 0);
    Size edgesSizeInPatternUnitsTopLeft = new Size(0, 0);
    Size edgesSizeInPatternUnitsBottomRight = new Size(0, 0);
    Size edgesSizeInRenderedImageTotal = new Size(0, 0);
    Size edgesSizeInRenderedImageTopLeft = new Size(0, 0);
    Size edgesSizeInRenderedImageBottomRight = new Size(0, 0);
    Size edgesSizeVisualTotal = new Size(0, 0);
    Size edgesSizeVisualTopLeft = new Size(0, 0);
    Size edgesSizeVisualBottomRight = new Size(0, 0);
    bool useCornersIfAvailable = true;
    bool useLeftRightEdgesIfAvailable = true;
    bool useTopBottomEdgesIfAvailable = true;

    // If the pattern doesn't have special edge treatment,
    // the rendered image may need margins to accommodate
    // pattern elements with an extent beyond the spacing.
    Size renderingMarginTopLeft = new Size(0, 0);
    Size renderingMarginBottomRight = new Size(0, 0);

    private Dictionary<Color, ImageAttributes> colorMappings =
      new Dictionary<Color, ImageAttributes>();

    double epsilon = 0.01;

    /// <summary>
    /// Constructor for new design.
    /// </summary>
    public ChainmailleDesign()
    {
    }

    /// <summary>
    /// Constructor for new design, specifying various parameters.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="patternFilename"></param>
    /// <param name="size"></param>
    /// <param name="sizeUnits"></param>
    public ChainmailleDesign(string name, string patternFilename, SizeF size,
      DesignSizeUnitsEnum sizeUnits, bool roundSizeUp, WrapEnum designWrap,
      PatternScale designScale)
    {
      designName = name;
      patternFile = patternFilename;
      if (patternFile.StartsWith(
            Properties.Settings.Default.PatternDirectory))
      {
        patternFile = patternFile.Remove(0, Properties.Settings.Default.
          PatternDirectory.Length + 1);
      }
      weaveName = Path.GetFileNameWithoutExtension(patternFile);
      chainmaillePattern = new ChainmaillePattern(patternFile, weaveName);
      // Set the weave to be the current pattern.
      Properties.Settings.Default.CurrentPattern = patternFile;
      Properties.Settings.Default.Save();

      wrap = designWrap;
      scale = designScale;

      DetermineEdgeAllowances();

      // If size units are distance units but no scale information is provided,
      // set size units to be pattern units.
      if (designScale == null &&
          (sizeUnits == DesignSizeUnitsEnum.Inches ||
           sizeUnits == DesignSizeUnitsEnum.Centimeters))
      {
        sizeUnits = DesignSizeUnitsEnum.Units;
      }

      double fractionalSize;
      switch (sizeUnits)
      {
        case DesignSizeUnitsEnum.RowsColumns:
          {
            SizeF bodySize = new SizeF(
              size.Width - edgesSizeVisualTotal.Width,
              size.Height - edgesSizeVisualTotal.Height);

            fractionalSize =
              bodySize.Width / chainmaillePattern.VisualSize.Width;
            sizeInUnits.Width = (int)Math.Floor(
              bodySize.Width / chainmaillePattern.VisualSize.Width);
            if (fractionalSize - sizeInUnits.Width > epsilon && roundSizeUp)
            {
              sizeInUnits.Width++;
            }
            fractionalSize =
              bodySize.Height / chainmaillePattern.VisualSize.Height;
            sizeInUnits.Height = (int)Math.Floor(
              bodySize.Height / chainmaillePattern.VisualSize.Height);
            if (fractionalSize - sizeInUnits.Height > epsilon && roundSizeUp)
            {
              sizeInUnits.Height++;
            }

            sizeInUnits.Width += edgesSizeInPatternUnitsTotal.Width;
            sizeInUnits.Height += edgesSizeInPatternUnitsTotal.Height;
          }
          break;
        case DesignSizeUnitsEnum.Pixels:
          fractionalSize = size.Width / chainmaillePattern.ColorSize.Width;
          sizeInUnits.Width = (int)Math.Floor(
            size.Width / chainmaillePattern.ColorSize.Width);
          if (fractionalSize - sizeInUnits.Width > epsilon && roundSizeUp)
          {
            sizeInUnits.Width++;
          }
          fractionalSize = size.Height / chainmaillePattern.ColorSize.Height;
          sizeInUnits.Height = (int)Math.Floor(
            size.Height / chainmaillePattern.ColorSize.Height);
          if (fractionalSize - sizeInUnits.Height > epsilon && roundSizeUp)
          {
            sizeInUnits.Height++;
          }
          break;
        case DesignSizeUnitsEnum.Inches:
        case DesignSizeUnitsEnum.Centimeters:
          {
            double distanceConversionFactor = 1.0;
            if (sizeUnits == DesignSizeUnitsEnum.Inches &&
                designScale.ReferenceUnits != Units.Inches)
            {
              distanceConversionFactor = UnitConverter.ConvertValue(
                1.0, Units.Inches, designScale.ReferenceUnits);
            }
            else if (sizeUnits == DesignSizeUnitsEnum.Centimeters &&
                     designScale.ReferenceUnits != Units.Centimeters)
            {
              distanceConversionFactor = UnitConverter.ConvertValue(
                1.0, Units.Centimeters, designScale.ReferenceUnits);
            }

            fractionalSize = size.Width * distanceConversionFactor *
              designScale.UnitSize.Width;
            sizeInUnits.Width = (int)Math.Floor(size.Width *
              distanceConversionFactor * designScale.UnitSize.Width);
            if (fractionalSize - sizeInUnits.Width > epsilon && roundSizeUp)
            {
              sizeInUnits.Width++;
            }
            fractionalSize = size.Height * distanceConversionFactor *
              designScale.UnitSize.Height;
            sizeInUnits.Height = (int)Math.Floor(size.Height *
              distanceConversionFactor * designScale.UnitSize.Height);
            if (fractionalSize - sizeInUnits.Height > epsilon && roundSizeUp)
            {
              sizeInUnits.Height++;
            }
          }
          break;
        case DesignSizeUnitsEnum.Units:
        default:
          sizeInUnits.Width = (int)Math.Floor(size.Width);
          if (size.Width - sizeInUnits.Width > epsilon && roundSizeUp)
          {
            sizeInUnits.Width++;
          }
          sizeInUnits.Height = (int)Math.Floor(size.Height);
          if (size.Height - sizeInUnits.Height > epsilon && roundSizeUp)
          {
            sizeInUnits.Height++;
          }
          break;
      }

      Size colorImageSize = new Size(
        sizeInUnits.Width * chainmaillePattern.ColorSize.Width,
        sizeInUnits.Height * chainmaillePattern.ColorSize.Height);
      colorImage = new ColorImage(colorImageSize);

      hasBeenChanged = true;
    }

    /// <summary>
    /// Constructor from a specified design file.
    /// </summary>
    /// <param name="designFilepath"></param>
    public ChainmailleDesign(string designFilepath)
    {
      designFile = designFilepath;
      designName = Path.GetFileNameWithoutExtension(designFile);

      // Later, read the design from the specified file.
      if (Path.GetExtension(designFile) == ".xml")
      {
        // The file is an XML file, so try reading it as a design file.
        string messages = ReadDesignFromXml();
        // Set the weave to be the current pattern.
        Properties.Settings.Default.CurrentPattern = patternFile;
        Properties.Settings.Default.Save();
      }
      else
      {
        // The file is not an XML file, so it must just be an image file.
        // use the image file as a source for the color image.
        colorImage = new ColorImage(designFile);

        // Default the pattern.
        patternFile = Properties.Settings.Default.CurrentPattern;
        weaveName = Path.GetFileNameWithoutExtension(patternFile);
        chainmaillePattern = new ChainmaillePattern(patternFile, weaveName);
        if (!chainmaillePattern.IsInitialized)
        {
          chainmaillePattern = null;
        }
      }

      DetermineEdgeAllowances();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        colorImage?.Dispose();
        chainmaillePattern?.Dispose();
        renderedImage?.Dispose();
        renderedImageToDisplayTransformation?.Dispose();
        displayToRenderedImageTransformation?.Dispose();
        overlayImage?.Dispose();
        overlayToRenderedImageTransformation?.Dispose();
        renderedToOverlayImageTransformation?.Dispose();
        overlayAttributes?.Dispose();
        foreach (ImageAttributes imageAttributes in colorMappings.Values)
        {
          imageAttributes.Dispose();
        }
        colorMappings.Clear();
      }
    }

    public void AddHorizontalGuideline(int position)
    {
      if (!horizontalGuidelines.Contains(position))
      {
        horizontalGuidelines.Add(position);
        hasBeenChanged = true;
      }
    }

    public void AddVerticalGuideline(int position)
    {
      if (!verticalGuidelines.Contains(position))
      {
        verticalGuidelines.Add(position);
        hasBeenChanged = true;
      }
    }

    /// <summary>
    /// Adjust the size of the design by adding or removing units from the
    /// edges.
    /// </summary>
    /// <param name="adjustment"></param>
    public void AdjustSize(SizeAdjustment adjustment)
    {
      if (chainmaillePattern != null &&
          adjustment.Units == DesignSizeUnitsEnum.Units)
      {
        Size upperLeftAdjustment = new Size(
          (int)adjustment.UpperLeftAdjustment.Width *
          chainmaillePattern.ColorSize.Width,
          (int)adjustment.UpperLeftAdjustment.Height *
          chainmaillePattern.ColorSize.Height);
        Size lowerRightAdjustment = new Size(
          (int)adjustment.LowerRightAdjustment.Width *
          chainmaillePattern.ColorSize.Width,
          (int)adjustment.LowerRightAdjustment.Height *
          chainmaillePattern.ColorSize.Height);

        try
        {
          // Create a new color image of the required size.
          ColorImage newColorImage = new ColorImage(new Size(
            colorImage.BitmapImage.Width +
            upperLeftAdjustment.Width +
            lowerRightAdjustment.Width,
            colorImage.BitmapImage.Height +
            upperLeftAdjustment.Height +
            lowerRightAdjustment.Height));

          // Initialize source and destination to the entirety of their
          // respective images.
          Rectangle source = new Rectangle(0, 0,
            colorImage.BitmapImage.Width, colorImage.BitmapImage.Height);
          Rectangle destination = new Rectangle(0, 0,
            newColorImage.BitmapImage.Width, newColorImage.BitmapImage.Height);

          // Clear the new color image.
          Graphics g = Graphics.FromImage(newColorImage.BitmapImage);
          Brush clearBrush = new SolidBrush(
            Properties.Settings.Default.UnspecifiedElementColor);
          g.FillRectangle(clearBrush, destination);
          clearBrush.Dispose();

          // Adjust the relationship between the rectangles.
          if (upperLeftAdjustment.Height >= 0)
          {
            destination.Y += upperLeftAdjustment.Height;
            destination.Height -= upperLeftAdjustment.Height;
          }
          else
          {
            source.Y -= upperLeftAdjustment.Height;
            source.Height += upperLeftAdjustment.Height;
          }
          if (lowerRightAdjustment.Height >= 0)
          {
            destination.Height -= lowerRightAdjustment.Height;
          }
          else
          {
            source.Height += lowerRightAdjustment.Height;
          }
          if (upperLeftAdjustment.Width >= 0)
          {
            destination.X += upperLeftAdjustment.Width;
            destination.Width -= upperLeftAdjustment.Width;
          }
          else
          {
            source.X -= upperLeftAdjustment.Width;
            source.Width += upperLeftAdjustment.Width;
          }
          if (lowerRightAdjustment.Width >= 0)
          {
            destination.Width -= lowerRightAdjustment.Width;
          }
          else
          {
            source.Width += lowerRightAdjustment.Width;
          }

          // Draw the right part of the old color image into the right part of
          // the new color image.
          g.DrawImage(colorImage.BitmapImage, destination, source,
            GraphicsUnit.Pixel);
          g.Dispose();

          // Replace the old color image with the new one.
          ColorImage oldColorImage = colorImage;
          colorImage = newColorImage;
          oldColorImage.Dispose();

          // If top or left edges were changed, adjust the positions of the
          // overlay and guidelines, if any.
          if (adjustment.UpperLeftAdjustment.Width != 0 ||
              adjustment.UpperLeftAdjustment.Height != 0)
          {
            SizeF renderedAdjustment = new SizeF(
              adjustment.UpperLeftAdjustment.Width *
              chainmaillePattern.BasicPatternSet.RenderingSpacing.Width,
              adjustment.UpperLeftAdjustment.Height *
              chainmaillePattern.BasicPatternSet.RenderingSpacing.Height);
            if (overlayImage != null)
            {
              overlayCenterInRenderedImage += renderedAdjustment;
            }
            if (horizontalGuidelines.Count > 0)
            {
              SortedSet<int> adjustedGuidelines = new SortedSet<int>();
              foreach (int guideline in horizontalGuidelines)
              {
                adjustedGuidelines.Add(
                  guideline + (int)renderedAdjustment.Height);
              }
              horizontalGuidelines = adjustedGuidelines;
            }
            if (verticalGuidelines.Count > 0)
            {
              SortedSet<int> adjustedGuidelines = new SortedSet<int>();
              foreach (int guideline in verticalGuidelines)
              {
                adjustedGuidelines.Add(
                  guideline + (int)renderedAdjustment.Width);
              }
              verticalGuidelines = adjustedGuidelines;
            }
          }

          RenderImage();

          hasBeenChanged = true;
        }
        catch { }
      }
    }

    public void ApplyPattern(string filePath)
    {
      string directoryPath = Path.GetDirectoryName(filePath);
      if (directoryPath.StartsWith(
            Properties.Settings.Default.PatternDirectory))
      {
        patternFile = directoryPath.Remove(0, Properties.Settings.Default.
          PatternDirectory.Length + 1);
        weaveName = Path.GetFileNameWithoutExtension(patternFile);
        chainmaillePattern = new ChainmaillePattern(patternFile, weaveName);
        hasBeenChanged = true;
        // Set the weave to be the current pattern.
        Properties.Settings.Default.CurrentPattern = patternFile;
        Properties.Settings.Default.Save();

        DetermineEdgeAllowances();
      }
    }

    public void BackgroundColorHasChanged(Color newBackgroundColor)
    {
      backgroundColor = newBackgroundColor;
      if (renderedImage != null)
      {
        Graphics g = Graphics.FromImage(renderedImage);
        RenderBackground(renderedImage, g);
        RenderOutlines(renderedImage, g);
        RenderAllPatternElements(renderedImage, g);
        g.Dispose();
      }
    }

    public ChainmaillePattern ChainmailPattern
    {
      get { return chainmaillePattern; }
    }

    public void ClearAllGuidelines()
    {
      if (horizontalGuidelines.Count > 0 || verticalGuidelines.Count > 0)
      {
        hasBeenChanged = true;
      }
      horizontalGuidelines.Clear();
      verticalGuidelines.Clear();
      guidelinesAreShowing = true;
    }

    public void ClearAllRingColors()
    {
      if (colorImage != null && colorImage.BitmapImage != null)
      {
        Graphics g = Graphics.FromImage(colorImage.BitmapImage);
        g.Clear(Color.Transparent);
        g.Dispose();
        hasBeenChanged = true;
        RenderImage();
      }
    }

    /// <summary>
    /// Using the colors from the overlay image, determine the ring colors
    /// for the design, given a palette section. Each ring is assigned the
    /// color from the palette section that is closest to the average color
    /// of the overlay pixels found within the outline of that ring.
    /// </summary>
    public void ColorDesignFromOverlay(PaletteSection paletteSection,
      string ringFilter, IShapeProgressIndicator progress = null,
      ColorTransform colorTransform = null,
      ColorImage alternateColorImage = null)
    {
      if (chainmaillePattern != null && colorImage != null &&
          paletteSection != null && paletteSection.ColorCount > 0 &&
          overlayImage != null)
      {
        // Create an empty color image for the design, based upon the
        // dimensions and pixel format of the old.
        Bitmap newColorImage = new Bitmap(colorImage.BitmapImage);

        // Set up for computing Lab color averages.
        LabImage colorSums = new LabImage(
          colorImage.Width, colorImage.Height);
        Dictionary<int, Dictionary<int, int>> colorCounts =
          new Dictionary<int, Dictionary<int, int>>();
        for (int cx = 0; cx < colorImage.Width; cx++)
        {
          colorCounts.Add(cx, new Dictionary<int, int>());
          for (int cy = 0; cy < colorImage.Height; cy++)
          {
            colorCounts[cx].Add(cy, 0);
          }
        }

        // Dictionary to speed ring lookups.
        Dictionary<int, Dictionary<int, Tuple<ChainmaillePatternElementId,
          ChainmaillePatternElement>>> ringLookup =
          new Dictionary<int, Dictionary<int,
          Tuple<ChainmaillePatternElementId, ChainmaillePatternElement>>>();

        // Progress bar scaling.
        if (progress != null)
        {
          progress.ShapeProgressScale = overlayImage.Height;
        }

        // For each pixel in the overlay image.
        LabColor overlayColor;
        for (int oy = 0; oy < overlayImage.Height; oy++)
        {
          // Progress bar progress.
          if (progress != null)
          {
            progress.ShapeProgressValue = oy;
          }

          for (int ox = 0; ox < overlayImage.Width; ox++)
          {
            overlayColor = ColorUtils.RgbToLab(overlayImage.GetPixel(ox, oy));

            // Determine where the overlay pixel falls in the rendered image.
            PointF oPointF = new PointF(ox, oy);
            PointF rPointF =
              TransformOverlayImagePointFToRenderedImage(oPointF);
            Point rPoint = new Point(
              (int)(rPointF.X + 0.5), (int)(rPointF.Y + 0.5));

            // Determine which ring corresponds to the rendered image pixel.
            ChainmaillePatternElement element;
            ChainmaillePatternElementId elementId;
            if (ringLookup.ContainsKey(rPoint.X) &&
                ringLookup[rPoint.X].ContainsKey(rPoint.Y))
            {
              elementId = ringLookup[rPoint.X][rPoint.Y].Item1;
              element = ringLookup[rPoint.X][rPoint.Y].Item2;
            }
            else
            {
              elementId = PatternElementAtRenderedPoint(rPoint, out element);
              if (!ringLookup.ContainsKey(rPoint.X))
              {
                ringLookup.Add(rPoint.X, new Dictionary<int,
                  Tuple<ChainmaillePatternElementId,
                  ChainmaillePatternElement>>());
              }
              ringLookup[rPoint.X].Add(rPoint.Y,
                new Tuple<ChainmaillePatternElementId,
                ChainmaillePatternElement>(elementId, element));
            }
            if (elementId != null && (string.IsNullOrEmpty(ringFilter) ||
              element.RingSizeName == ringFilter))
            {
              // Determine which pixel of the color image corresponds to the
              // ring.
              Point? cPoint = ElementToPointInColorImage(elementId, element);
              if (cPoint.HasValue)
              {
                // Add the overlay pixel color information to the running
                // color sum for that pixel of the color image.
                int cx = cPoint.Value.X;
                int cy = cPoint.Value.Y;
                LabColor oldSum = colorSums.GetPixel(cx, cy);
                colorSums.SetPixel(cx, cy, new LabColor(
                  oldSum.Item1 + overlayColor.Item1,
                  oldSum.Item2 + overlayColor.Item2,
                  oldSum.Item3 + overlayColor.Item3));
                colorCounts[cx][cy]++;
              }
            }
          }
        }

        // For each pixel of the color image.
        for (int cy = 0; cy < newColorImage.Height; cy++)
        {
          for (int cx = 0; cx < newColorImage.Width; cx++)
          {
            int colorCount = colorCounts[cx][cy];
            if (colorCount > 0)
            {
              // Compute the average overlay color.
              LabColor colorSum = colorSums.GetPixel(cx, cy);
              LabColor averageColor = new LabColor(colorSum.Item1 / colorCount,
                colorSum.Item2 / colorCount, colorSum.Item3 / colorCount);

              // If a color transform is specified, apply it.
              if (colorTransform != null)
              {
                averageColor =
                  ColorConverter.TransformLabColor(averageColor, colorTransform);
              }

              // Determine the closest palette section color and set the ring
              // color to the closest palette section color.
              newColorImage.SetPixel(cx, cy,
                paletteSection.GetClosestColor(averageColor));
            }
          }
        }

        if (alternateColorImage != null)
        {
          // Don't assign the color image to the design; we're probably using
          // it for a preview.
          alternateColorImage.BitmapImage = newColorImage;
        }
        else
        {
          // Assign the new color image to the design and redraw the design.
          colorImage.BitmapImage = newColorImage;
          hasBeenChanged = true;

          RenderImage();
        }

        if (progress != null)
        {
          progress.ShapeProgressValue = 0;
        }
      }
    }

    public ColorImage ColorImage
    {
      get { return colorImage; }
      set { colorImage = value; }
    }

    /// <summary>
    /// Remaps the element color of a pattern element image (always black)
    /// to another color (e.g. ring color, outline color, etc.) for rendering,
    /// and the background of the pattern element image (always white) to
    /// transparent.
    /// Builds a dictionary of color mappings, so that mappings can be reused.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private ImageAttributes ColorMapping(Color color)
    {
      if (!colorMappings.ContainsKey(color))
      {
        ImageAttributes imageAttributes = new ImageAttributes();
        ColorMap[] colorMap = new ColorMap[2];
        colorMap[0] = new ColorMap();
        colorMap[0].OldColor = Color.Black;
        colorMap[0].NewColor = color;
        colorMap[1] = new ColorMap();
        colorMap[1].OldColor = Color.White;
        colorMap[1].NewColor = Color.Transparent;
        imageAttributes.SetRemapTable(colorMap);
        colorMappings.Add(color, imageAttributes);
      }

      return colorMappings[color];
    }

    private Size? ComputeRenderedImageSize()
    {
      Size? size = null;

      if (colorImage != null && chainmaillePattern != null)
      {
        sizeInUnits = new Size(
          (int)(Math.Ceiling(1.0 * colorImage.Size.Width /
          chainmaillePattern.ColorSize.Width)),
          (int)(Math.Ceiling(1.0 * colorImage.Size.Height /
          chainmaillePattern.ColorSize.Height))
        );

        Rectangle boundingBox = chainmaillePattern.BoundingBox;
        renderingMarginTopLeft = new Size(0, 0);
        renderingMarginBottomRight = new Size(0, 0);
        if (wrap != WrapEnum.Horizontal &&
            edgesSizeInRenderedImageTopLeft.Width == 0)
        {
          // There is no special left edge treatment.
          // Allow for pattern extent beyond spacing.
          renderingMarginTopLeft.Width = Math.Max(0, -boundingBox.X);
        }
        if (wrap != WrapEnum.Vertical &&
            edgesSizeInRenderedImageTopLeft.Height == 0)
        {
          // There is no special top edge treatment.
          // Allow for pattern extent beyond spacing.
          renderingMarginTopLeft.Height = Math.Max(0, -boundingBox.Y);
        }
        if (wrap != WrapEnum.Horizontal &&
            edgesSizeInRenderedImageBottomRight.Width == 0)
        {
          // There is no special right edge treatment.
          // Allow for pattern extent beyond spacing.
          renderingMarginBottomRight.Width = Math.Max(0,
            chainmaillePattern.EvenRowHorizontalOffset +
            Math.Max(0, boundingBox.X) +
            chainmaillePattern.BoundingBox.Width -
            renderingMarginTopLeft.Width -
            chainmaillePattern.BasicPatternSet.RenderingSpacing.Width);
        }
        if (wrap != WrapEnum.Vertical &&
            edgesSizeInRenderedImageBottomRight.Height == 0)
        {
          // There is no special bottom edge treatment.
          // Allow for pattern extent beyond spacing.
          renderingMarginBottomRight.Height = Math.Max(0,
            chainmaillePattern.EvenRowVerticalOffset +
            Math.Max(0, boundingBox.Y) +
            chainmaillePattern.BoundingBox.Height -
            renderingMarginTopLeft.Height - 
            chainmaillePattern.BasicPatternSet.RenderingSpacing.Height);
        }

        size = new Size(
          chainmaillePattern.HorizontalSpacing *
            (sizeInUnits.Width - edgesSizeInPatternUnitsTotal.Width) +
            edgesSizeInRenderedImageTotal.Width +
            renderingMarginTopLeft.Width + renderingMarginBottomRight.Width,
          chainmaillePattern.VerticalSpacing *
            (sizeInUnits.Height - edgesSizeInPatternUnitsTotal.Height) +
            edgesSizeInRenderedImageTotal.Height +
            renderingMarginTopLeft.Height + renderingMarginBottomRight.Height);
      }

      return size;
    }

    /// <summary>
    /// Rearrange the color information into rows and columns as they will be
    /// built. The process is like rendering, with account taken of edges and
    /// corners, but uses the build offset information from the weave pattern
    /// definition.
    /// </summary>
    /// <returns>row->column->color</returns>
    public SortedDictionary<int, SortedDictionary<int, Color>>
      ConstructBuildImage()
    {
      SortedDictionary<int, SortedDictionary<int, Color>> buildImage =
        new SortedDictionary<int, SortedDictionary<int, Color>>();
      Size expectedBuildImageSize = new Size(
        (sizeInUnits.Width - edgesSizeInPatternUnitsTotal.Width) *
        chainmaillePattern.VisualSize.Width + edgesSizeVisualTotal.Width,
        (sizeInUnits.Height - edgesSizeInPatternUnitsTotal.Height) *
        chainmaillePattern.VisualSize.Height + edgesSizeVisualTotal.Height);

      if (chainmaillePattern != null && colorImage != null &&
            chainmaillePattern.PatternElements.Count > 0)
      {
        int cx, cy, cxBase, cyBase;
        int bx, by, bxBase, byBase;
        ChainmaillePatternElement element;
        Color elementColor;
        ChainmaillePatternSet patternSet = chainmaillePattern.BasicPatternSet;
        Size imageSizeInUnits = chainmaillePattern.SizeInUnits;

        if (chainmaillePattern.Linkage == WeaveLinkageEnum.Sheet &&
            chainmaillePattern.HasEdges(EdgeGeometryEnum.Rectangular))
        {
          int bottomRowMarginInUnits =
            Math.Max(1, edgesSizeInPatternUnitsBottomRight.Height);
          int rightColumnMarginInUnits =
            Math.Max(1, edgesSizeInPatternUnitsBottomRight.Width);

          byBase = 0;
          cyBase = 0;
          for (int r = 0; r < sizeInUnits.Height;)
          {
            bool isLastRow =
              sizeInUnits.Height - r <= bottomRowMarginInUnits;
            bxBase= 0;
            cxBase = 0;
            for (int c = 0; c < sizeInUnits.Width;)
            {
              bool isLastColumn =
                sizeInUnits.Width - c <= rightColumnMarginInUnits;
              // Subsequently, we test whether we need to draw a corner or edge
              // image instead of the normal image, so here we default the
              // image to draw the normal image.
              patternSet = null;
              if (r == 0)
              {
                // Top row; draw corners and/or edges as needed.
                if (c == 0 && useCornersIfAvailable &&
                    chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopLeft))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopLeft);
                }
                else if (isLastColumn && useCornersIfAvailable &&
                          chainmaillePattern.HasCorner(
                            EdgeGeometryEnum.Rectangular,
                            CornerOrientationEnum.TopRight))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopRight);
                }
                else if (useTopBottomEdgesIfAvailable &&
                          chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                          EdgeOrientationEnum.Top))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Top);
                }
                else if (c == 0 && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }
              else if (isLastRow)
              {
                // Bottom row; draw corners and/or edges as needed.
                if (c == 0 && useCornersIfAvailable &&
                    chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomLeft))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomLeft);
                }
                else if (isLastColumn && useCornersIfAvailable &&
                          chainmaillePattern.HasCorner(
                            EdgeGeometryEnum.Rectangular,
                            CornerOrientationEnum.BottomRight))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomRight);
                }
                else if (useTopBottomEdgesIfAvailable &&
                          chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                          EdgeOrientationEnum.Bottom))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Bottom);
                }
                else if (c == 0 && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }
              else
              {
                // A middle row; draw left/right edges as needed.
                if (c == 0 && useLeftRightEdgesIfAvailable &&
                    chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                          chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                          EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }

              if (patternSet == null || patternSet.OutlineImage == null)
              {
                patternSet = chainmaillePattern.BasicPatternSet;
              }
              imageSizeInUnits = patternSet.SizeInUnits;

              for (int e = 0; e < patternSet.PatternElements.Count; e++)
              {
                element = patternSet.PatternElements[e];
                if (element.Image != null)
                {
                  cx = cxBase + element.ColorOffset.X;
                  cy = cyBase + element.ColorOffset.Y;
                  elementColor = colorImage.ColorAt(cx, cy);

                  bx = bxBase + element.BuildOffset.X;
                  by = byBase + element.BuildOffset.Y;

                  // Handle wrap, if any.
                  if (wrap == WrapEnum.Horizontal)
                  {
                    bx += bx < 0 ? expectedBuildImageSize.Width :
                      (bx >= expectedBuildImageSize.Width ?
                      -expectedBuildImageSize.Width : 0);
                  }
                  else if (wrap == WrapEnum.Vertical)
                  {
                    by += by < 0 ? expectedBuildImageSize.Height :
                      (by >= expectedBuildImageSize.Height ?
                      -expectedBuildImageSize.Height : 0);
                  }

                  if (!buildImage.ContainsKey(by))
                  {
                    buildImage.Add(by, new SortedDictionary<int, Color>());
                  }
                  if (!buildImage[by].ContainsKey(bx))
                  {
                    buildImage[by].Add(bx, elementColor);
                  }
                  else
                  {
                    // Oughtn't happen; should we mention it?
                    buildImage[by][bx] = elementColor;
                  }
                }
              }

              bxBase += patternSet.SizeInUnits.Width *
                chainmaillePattern.VisualSize.Width;
              cxBase += chainmaillePattern.ColorSize.Width *
                patternSet.SizeInUnits.Width;
              c += imageSizeInUnits.Width;
            }

            byBase += patternSet.SizeInUnits.Height *
              chainmaillePattern.VisualSize.Height;
            cyBase += chainmaillePattern.ColorSize.Height *
              patternSet.SizeInUnits.Height;
            r += imageSizeInUnits.Height;
          }
        }
        else
        {
          byBase = 0;
          cyBase = 0;
          for (int r = 0; r < sizeInUnits.Height;)
          {
            bxBase = 0;
            cxBase = 0;
            for (int c = 0; c < sizeInUnits.Width;)
            {
              for (int e = 0; e < chainmaillePattern.PatternElements.Count; e++)
              {
                element = chainmaillePattern.PatternElements[e];
                if (element.Image != null)
                {
                  cx = cxBase + element.ColorOffset.X;
                  cy = cyBase + element.ColorOffset.Y;
                  elementColor = colorImage.ColorAt(cx, cy);

                  bx = bxBase + element.BuildOffset.X;
                  by = byBase + element.BuildOffset.Y;

                  // Handle wrap, if any.
                  if (wrap == WrapEnum.Horizontal)
                  {
                    bx += bx < 0 ? expectedBuildImageSize.Width :
                      (bx >= expectedBuildImageSize.Width ?
                      -expectedBuildImageSize.Width : 0);
                  }
                  else if (wrap == WrapEnum.Vertical)
                  {
                    by += by < 0 ? expectedBuildImageSize.Height :
                      (by >= expectedBuildImageSize.Height ?
                      -expectedBuildImageSize.Height : 0);
                  }

                  if (!buildImage.ContainsKey(by))
                  {
                    buildImage.Add(by, new SortedDictionary<int, Color>());
                  }
                  if (!buildImage[by].ContainsKey(bx))
                  {
                    buildImage[by].Add(bx, elementColor);
                  }
                  else
                  {
                    // Oughtn't happen; should we mention it?
                    buildImage[by][bx] = elementColor;
                  }
                }
              }

              bxBase += patternSet.SizeInUnits.Width *
                chainmaillePattern.VisualSize.Width;
              cxBase += chainmaillePattern.ColorSize.Width *
                patternSet.SizeInUnits.Height;
              c += imageSizeInUnits.Width;
            }

            byBase += patternSet.SizeInUnits.Height *
              chainmaillePattern.VisualSize.Height;
            cyBase += chainmaillePattern.ColorSize.Height *
              patternSet.SizeInUnits.Height;
            r += imageSizeInUnits.Height;
          }
        }
      }

      return buildImage;
    }

    public string Description
    {
      get { return designDescription; }
      set
      {
        if (designDescription != value)

        {
          designDescription = value;
          hasBeenChanged = true;
        }
      }
    }

    public DateTime DesignDate
    {
      get { return designDate; }
      set
      {
        if (designDate != value)
        {
          designDate = value;
          hasBeenChanged = true;
        }
      }
    }

    public string DesignedBy
    {
      get { return designedBy; }
      set
      {
        if (designedBy != value)
        {
          designedBy = value;
          hasBeenChanged = true;
        }
      }
    }

    public string DesignedFor
    {
      get { return designedFor; }
      set
      {
        if (designedFor != value)
        {
          designedFor = value;
          hasBeenChanged = true;
        }
      }
    }

    public string DesignFile
    {
      get { return designFile; }
      set
      {
        designFile = value;
        designName = Path.GetFileNameWithoutExtension(designFile);
      }
    }

    public string DesignName
    {
      get { return designName; }
      set
      {
        if (designName != value)
        {
          designName = value;
          hasBeenChanged = true;
        }
      }
    }

    /// <summary>
    /// Determine allowances for edges of the design.
    /// </summary>
    private void DetermineEdgeAllowances()
    {
      // Reset the allowances.
      //edgesSizeInBuildImageTotal = new Size(0, 0);
      //edgesSizeInBuildImageTopLeft = new Size(0, 0);
      //edgesSizeInBuildImageBottomRight = new Size(0, 0);
      edgesSizeInColorImageTotal = new Size(0, 0);
      edgesSizeInColorImageTopLeft = new Size(0, 0);
      edgesSizeInColorImageBottomRight = new Size(0, 0);
      edgesSizeInPatternUnitsTotal = new Size(0, 0);
      edgesSizeInPatternUnitsTopLeft = new Size(0, 0);
      edgesSizeInPatternUnitsBottomRight = new Size(0, 0);
      edgesSizeInRenderedImageTotal = new Size(0, 0);
      edgesSizeInRenderedImageTopLeft = new Size(0, 0);
      edgesSizeInRenderedImageBottomRight = new Size(0, 0);
      edgesSizeVisualTotal = new Size(0, 0);
      edgesSizeVisualTopLeft = new Size(0, 0);
      edgesSizeVisualBottomRight = new Size(0, 0);

      useCornersIfAvailable = wrap == WrapEnum.None;
      useTopBottomEdgesIfAvailable = wrap != WrapEnum.Vertical;
      useLeftRightEdgesIfAvailable = wrap != WrapEnum.Horizontal;

      // Note: For now, the code assumes that if the design uses a pattern
      // with sheet linkage and rectangular edges, the design has
      // rectangular edges.
      if (chainmaillePattern != null &&
          chainmaillePattern.Linkage == WeaveLinkageEnum.Sheet &&
          chainmaillePattern.HasEdges(EdgeGeometryEnum.Rectangular))
      {
        ChainmaillePatternSet edgeSet;
        if (useTopBottomEdgesIfAvailable)
        {
          if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Top))
          {
            edgeSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
              EdgeOrientationEnum.Top);
            edgesSizeInColorImageTopLeft.Height =
              edgeSet.SizeInUnits.Height * chainmaillePattern.ColorSize.Height;
            edgesSizeInColorImageTotal.Height +=
              edgesSizeInColorImageTopLeft.Height;
            edgesSizeInPatternUnitsTopLeft.Height = edgeSet.SizeInUnits.Height;
            edgesSizeInPatternUnitsTotal.Height += edgeSet.SizeInUnits.Height;
            edgesSizeInRenderedImageTopLeft.Height = edgeSet.RenderingSpacing.Height;
            edgesSizeInRenderedImageTotal.Height += edgeSet.RenderingSpacing.Height;
            edgesSizeVisualTopLeft.Height = edgeSet.VisualSize.Height;
            edgesSizeVisualTotal.Height += edgeSet.VisualSize.Height;
          }
          if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Bottom))
          {
            edgeSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
              EdgeOrientationEnum.Bottom);
            edgesSizeInColorImageBottomRight.Height =
              edgeSet.SizeInUnits.Height * chainmaillePattern.ColorSize.Height;
            edgesSizeInColorImageTotal.Height +=
              edgesSizeInColorImageBottomRight.Height;
            edgesSizeInPatternUnitsBottomRight.Height = edgeSet.SizeInUnits.Height;
            edgesSizeInPatternUnitsTotal.Height += edgeSet.SizeInUnits.Height;
            edgesSizeInRenderedImageBottomRight.Height = edgeSet.RenderingSpacing.Height;
            edgesSizeInRenderedImageTotal.Height += edgeSet.RenderingSpacing.Height;
            edgesSizeVisualBottomRight.Height = edgeSet.VisualSize.Height;
            edgesSizeVisualTotal.Height += edgeSet.VisualSize.Height;
          }
        }

        if (useLeftRightEdgesIfAvailable)
        {
          if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Left))
          {
            edgeSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
              EdgeOrientationEnum.Left);
            edgesSizeInColorImageTopLeft.Width =
              edgeSet.SizeInUnits.Width * chainmaillePattern.ColorSize.Width;
            edgesSizeInColorImageTotal.Width +=
              edgesSizeInColorImageTopLeft.Width;
            edgesSizeInPatternUnitsTopLeft.Width = edgeSet.SizeInUnits.Width;
            edgesSizeInPatternUnitsTotal.Width += edgeSet.SizeInUnits.Width;
            edgesSizeInRenderedImageTopLeft.Width = edgeSet.RenderingSpacing.Width;
            edgesSizeInRenderedImageTotal.Width += edgeSet.RenderingSpacing.Width;
            edgesSizeVisualTopLeft.Width = edgeSet.VisualSize.Width;
            edgesSizeVisualTotal.Width += edgeSet.VisualSize.Width;
          }
          if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Right))
          {
            edgeSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
              EdgeOrientationEnum.Right);
            edgesSizeInColorImageBottomRight.Width =
              edgeSet.SizeInUnits.Width * chainmaillePattern.ColorSize.Width;
            edgesSizeInColorImageTotal.Width +=
              edgesSizeInColorImageBottomRight.Width;
            edgesSizeInPatternUnitsBottomRight.Width = edgeSet.SizeInUnits.Width;
            edgesSizeInPatternUnitsTotal.Width += edgeSet.SizeInUnits.Width;
            edgesSizeInRenderedImageBottomRight.Width = edgeSet.RenderingSpacing.Width;
            edgesSizeInRenderedImageTotal.Width += edgeSet.RenderingSpacing.Width;
            edgesSizeVisualBottomRight.Width = edgeSet.VisualSize.Width;
            edgesSizeVisualTotal.Width += edgeSet.VisualSize.Width;
          }
        }
      }
    }

    public float DisplayRotationDegrees
    {
      get { return displayRotationDegrees; }
    }

    public void DrawCircle(List<PointF> drawingPoints, Color drawingColor,
      DrawingFillMode fillMode, string ringSizeFilter,
      IShapeProgressIndicator progress = null)
    {
      if (drawingPoints.Count == 2)
      {
        PointF center = drawingPoints[0];
        PointF edge = drawingPoints[1];
        SizeF halfSize = new SizeF(
          Math.Abs(edge.X - center.X),
          Math.Abs(edge.Y - center.Y));
        float radius = MathUtils.RootSumSquares(
          halfSize.Width, halfSize.Height);
        RectangleF box = new RectangleF(
          center.X - radius, center.Y - radius,
          2.0F * radius, 2.0F * radius);

        GraphicsPath path = new GraphicsPath();
        path.AddEllipse(box);
        Region region = new Region(path);

        if (fillMode == DrawingFillMode.Hollow)
        {
          // Reduce the size of the box and exclude the resulting ellipse from
          // the region.
          box.Inflate(-2, -2);
          GraphicsPath exclusionPath = new GraphicsPath();
          exclusionPath.AddEllipse(box);
          Region exclusionRegion = new Region(exclusionPath);
          region.Exclude(exclusionRegion);
        }

        // Draw the shape.
        RectangleF[] scans = region.GetRegionScans(new Matrix());
        int progressValue = 1;
        if (progress != null)
        {
          progress.ShapeProgressScale = scans.Length;
        }
        foreach (RectangleF scan in scans)
        {
          if (progress != null)
          {
            progress.ShapeProgressValue = progressValue++;
          }
          DrawRectangle(new List<PointF>()
            { new PointF(scan.Left, scan.Top),
              new PointF(scan.Right, scan.Bottom) },
            drawingColor, DrawingFillMode.Solid, ringSizeFilter);
        }
      }
    }

    public void DrawEllipse(List<PointF> drawingPoints, Color drawingColor,
      DrawingFillMode fillMode, string ringSizeFilter,
      IShapeProgressIndicator progress = null)
    {
      if (drawingPoints.Count == 2)
      {
        PointF center = drawingPoints[0];
        PointF edge = drawingPoints[1];
        SizeF halfSize = new SizeF(
          Math.Abs(edge.X - center.X),
          Math.Abs(edge.Y - center.Y));
        RectangleF box = new RectangleF(
          center.X - halfSize.Width,
          center.Y - halfSize.Height,
          2.0F * halfSize.Width, 2.0F * halfSize.Height);

        GraphicsPath path = new GraphicsPath();
        path.AddEllipse(box);
        Region region = new Region(path);

        if (fillMode == DrawingFillMode.Hollow)
        {
          // Reduce the size of the box and exclude the resulting ellipse from
          // the region.
          box.Inflate(-2, -2);
          GraphicsPath exclusionPath = new GraphicsPath();
          exclusionPath.AddEllipse(box);
          Region exclusionRegion = new Region(exclusionPath);
          region.Exclude(exclusionRegion);
        }

        // Draw the shape.
        RectangleF[] scans = region.GetRegionScans(new Matrix());
        int progressValue = 1;
        if (progress != null)
        {
          progress.ShapeProgressScale = scans.Length;
        }
        foreach (RectangleF scan in scans)
        {
          if (progress != null)
          {
            progress.ShapeProgressValue = progressValue++;
          }
          DrawRectangle(new List<PointF>()
            { new PointF(scan.Left, scan.Top),
              new PointF(scan.Right, scan.Bottom) },
            drawingColor, DrawingFillMode.Solid, ringSizeFilter);
        }
      }
    }

    public void DrawLine(List<PointF> drawingPoints, Color drawingColor,
      DrawingFillMode fillMode, string ringSizeFilter,
      IShapeProgressIndicator progress = null)
    {
      if (drawingPoints.Count == 2)
      {
        // Identify all of the rings that lie along the line.
        // If a ring filter is active, identify only those rings that pass
        // the filter.
        // This is basically Bresenham's algorithm with ring identification,
        // so traversal is always in the positive direction along the axis
        // of greatest change.
        List<Tuple<ChainmaillePatternElementId, ChainmaillePatternElement>>
          elementIds = new List<Tuple<ChainmaillePatternElementId,
          ChainmaillePatternElement>>();
        ChainmaillePatternElementId elementId;
        ChainmaillePatternElementId previousElementId = null;
        ChainmaillePatternElement referencedElement;
        Point point = new Point();
        int end;
        int step;
        int absDx = Math.Abs((int)(drawingPoints[0].X - drawingPoints[1].X));
        int absDy = Math.Abs((int)(drawingPoints[0].Y - drawingPoints[1].Y));
        bool traverseX = absDx >= absDy;
        int p = traverseX ? 2 * absDy - absDx : 2 * absDx - absDy;
        int c1 = traverseX ? 2 * absDy : 2 * absDx;
        int c2 = traverseX ? 2 * (absDy - absDx) : 2 * (absDx - absDy);
        if (traverseX ? drawingPoints[0].X <= drawingPoints[1].X :
            drawingPoints[0].Y <= drawingPoints[1].Y)
        {
          // Begin at the first point.
          point.X = (int)drawingPoints[0].X;
          point.Y = (int)drawingPoints[0].Y;
          end = traverseX ? (int)drawingPoints[1].X :
            (int)drawingPoints[1].Y;
          step = traverseX ?
            (drawingPoints[0].Y <= drawingPoints[1].Y ? 1 : -1) :
            (drawingPoints[0].X <= drawingPoints[1].X ? 1 : -1);
        }
        else
        {
          // Begin at the second point.
          point.X = (int)drawingPoints[1].X;
          point.Y = (int)drawingPoints[1].Y;
          end = traverseX ? (int)drawingPoints[0].X :
            (int)drawingPoints[0].Y;
          step = traverseX ?
            (drawingPoints[1].Y <= drawingPoints[0].Y ? 1 : -1) :
            (drawingPoints[1].X <= drawingPoints[0].X ? 1 : -1);
        }
        elementId = PatternElementAtRenderedPoint(point,
          out referencedElement);
        if (elementId != null &&
            (string.IsNullOrEmpty(ringSizeFilter) ||
             referencedElement.RingSizeName == ringSizeFilter) &&
            (previousElementId == null ||
             elementId.Row != previousElementId.Row ||
             elementId.Column != previousElementId.Column ||
             elementId.ElementIndex != previousElementId.ElementIndex))
        {
          elementIds.Add(new Tuple<ChainmaillePatternElementId,
            ChainmaillePatternElement>(elementId, referencedElement));
          previousElementId = elementId;
        }
        while (traverseX ? point.X <= end : point.Y <= end)
        {
          if (traverseX)
          {
            point.X++;
          }
          else
          {
            point.Y++;
          }
          if (p < 0)
          {
            p += c1;
          }
          else
          {
            if (traverseX)
            {
              point.Y += step;
            }
            else
            {
              point.X += step;
            }
            p += c2;
          }
          // test for element.
          elementId = PatternElementAtRenderedPoint(point,
            out referencedElement);
          if (elementId != null &&
              (string.IsNullOrEmpty(ringSizeFilter) ||
               referencedElement.RingSizeName == ringSizeFilter) &&
              (previousElementId == null ||
               elementId.Row != previousElementId.Row ||
               elementId.Column != previousElementId.Column ||
               elementId.ElementIndex != previousElementId.ElementIndex))
          {
            elementIds.Add(new Tuple<ChainmaillePatternElementId,
              ChainmaillePatternElement> (elementId, referencedElement));
            previousElementId = elementId;
          }
        }

        int progressValue = 1;
        if (progress != null)
        {
          progress.ShapeProgressScale = elementIds.Count;
        }
        foreach (Tuple<ChainmaillePatternElementId,
                 ChainmaillePatternElement> element in elementIds)
        {
          if (progress != null)
          {
            progress.ShapeProgressValue = progressValue++;
          }
          SetElementColor(element.Item1, drawingColor, element.Item2);
          RenderPatternElement(element.Item1, element.Item2);
        }
      }
    }

    public void DrawPolygon(List<PointF> drawingPoints, Color drawingColor,
      DrawingFillMode fillMode, string ringSizeFilter,
      IShapeProgressIndicator progress = null)
    {
      if (drawingPoints.Count >= 3)
      {
        if (fillMode == DrawingFillMode.Hollow)
        {
          // Build the perimeter lines.
          List<List<PointF>> lines = new List<List<PointF>>();
          for (int i = 0; i < drawingPoints.Count - 1; i++)
          {
            lines.Add(new List<PointF>()
            { drawingPoints[i], drawingPoints[i + 1] });
          }
          lines.Add(new List<PointF>()
          { drawingPoints[drawingPoints.Count - 1], drawingPoints[0] });

          // Draw the perimeter lines.
          int progressValue = 1;
          if (progress != null)
          {
            progress.ShapeProgressScale = lines.Count;
          }
          foreach (List<PointF> line in lines)
          {
            if (progress != null)
            {
              progress.ShapeProgressValue = progressValue++;
            }
            DrawLine(line, drawingColor, DrawingFillMode.Solid,
              ringSizeFilter);
          }
        }
        else
        {
          // Draw the filled shape.
          GraphicsPath path = new GraphicsPath();
          path.AddPolygon(drawingPoints.ToArray());
          Region region = new Region(path);
          RectangleF[] scans = region.GetRegionScans(new Matrix());
          int progressValue = 1;
          if (progress != null)
          {
            progress.ShapeProgressScale = scans.Length;
          }
          foreach (RectangleF scan in scans)
          {
            if (progress != null)
            {
              progress.ShapeProgressValue = progressValue++;
            }
            DrawRectangle(new List<PointF>()
              { new PointF(scan.Left, scan.Top),
                new PointF(scan.Right, scan.Bottom) },
              drawingColor, DrawingFillMode.Solid, ringSizeFilter);
          }
        }
      }
    }

    public void DrawRectangle(List<PointF> drawingPoints, Color drawingColor,
      DrawingFillMode fillMode, string ringSizeFilter,
      IShapeProgressIndicator progress = null)
    {
      if (drawingPoints.Count == 2)
      {
        if (fillMode == DrawingFillMode.Hollow)
        {
          // Build the perimeter lines.
          List<List<PointF>> lines = new List<List<PointF>>();
          PointF start = drawingPoints[0];
          PointF end = new PointF(drawingPoints[0].X, drawingPoints[1].Y);
          lines.Add(new List<PointF>() { start, end });
          start = end;
          end = drawingPoints[1];
          lines.Add(new List<PointF>() { start, end });
          start = end;
          end = new PointF(drawingPoints[1].X, drawingPoints[0].Y);
          lines.Add(new List<PointF>() { start, end });
          start = end;
          end = drawingPoints[0];
          lines.Add(new List<PointF>() { start, end });

          // Draw the perimeter lines.
          int progressValue = 1;
          if (progress != null)
          {
            progress.ShapeProgressScale = lines.Count;
          }
          foreach (List<PointF> line in lines)
          {
            if (progress != null)
            {
              progress.ShapeProgressValue = progressValue++;
            }
            DrawLine(line, drawingColor, DrawingFillMode.Solid,
              ringSizeFilter);
          }
        }
        else
        {
          // Draw the filled shape.
          int xMin = (int)Math.Min(drawingPoints[0].X, drawingPoints[1].X);
          int xMax = (int)Math.Max(drawingPoints[0].X, drawingPoints[1].X);
          int yMin = (int)Math.Min(drawingPoints[0].Y, drawingPoints[1].Y);
          int yMax = (int)Math.Max(drawingPoints[0].Y, drawingPoints[1].Y);
          int progressValue = 1;
          if (progress != null)
          {
            progress.ShapeProgressScale = xMax - xMin + 1;
          }
          for (int x = xMin; x <= xMax; x++)
          {
            if (progress != null)
            {
              progress.ShapeProgressValue = progressValue++;
            }
            DrawLine(new List<PointF>()
              { new PointF(x, yMin), new PointF(x, yMax) },
              drawingColor, DrawingFillMode.Solid, ringSizeFilter);
          }
        }
      }
    }

    private bool ElementIsInColorImage(ChainmaillePatternElementId elementId,
      ChainmaillePatternElement element)
    {
      return ElementToPointInColorImage(elementId, element).HasValue;
    }

    private Point? ElementToPointInColorImage(
      ChainmaillePatternElementId elementId, ChainmaillePatternElement element)
    {
      Point? result = null;

      if (elementId != null && colorImage != null &&
          colorImage.BitmapImage != null && chainmaillePattern != null &&
          chainmaillePattern.PatternElements != null &&
          elementId.Column > 0 && elementId.Column <= sizeInUnits.Width &&
          elementId.Row > 0 && elementId.Row <= sizeInUnits.Height &&
          elementId.ElementIndex > 0)
      {
        int c = elementId.Column - 1;
        int r = elementId.Row - 1;
        int cxBase = c < edgesSizeInPatternUnitsTopLeft.Width ? 0 :
          edgesSizeInColorImageTopLeft.Width +
          (c - edgesSizeInPatternUnitsTopLeft.Width) *
          chainmaillePattern.ColorSize.Width;
        int cyBase = r < edgesSizeInPatternUnitsTopLeft.Height ? 0 :
          edgesSizeInColorImageTopLeft.Height +
          (r - edgesSizeInPatternUnitsTopLeft.Height) *
          chainmaillePattern.ColorSize.Height;
        int cx = cxBase + element.ColorOffset.X;
        int cy = cyBase + element.ColorOffset.Y;
        if (cx >= 0 && cx < colorImage.BitmapImage.Width &&
            cy >= 0 && cy < colorImage.BitmapImage.Height)
        {
          result = new Point(cx, cy);
        }
      }

      return result;
    }

    public Color GetElementColor(ChainmaillePatternElementId elementId,
      ChainmaillePatternElement referencedElement)
    {
      Color color = Color.Transparent;

      Point? cPoint = ElementToPointInColorImage(elementId, referencedElement);
      if (cPoint.HasValue)
      {
        color = colorImage.BitmapImage.GetPixel(
          cPoint.Value.X, cPoint.Value.Y);
      }

      return color;
    }

    /// <summary>
    /// Whether the design has been changed since it was last retrieved from
    /// its design file. A new design starts out as changed.
    /// </summary>
    public bool HasBeenChanged
    {
      get { return hasBeenChanged; }
    }

    public SortedSet<int> HorizontalGuidelines
    {
      get { return horizontalGuidelines; }
    }

    public bool HVReversed
    {
      get
      {
        return ((((int)displayRotationDegrees) + 45) / 90) % 2 == 1;
      }
    }

    public string Messages
    {
      get { return messages; }
    }

    public void OutlineColorHasChanged(Color newOutlineColor)
    {
      outlineColor = newOutlineColor;
      if (renderedImage != null)
      {
        Graphics g = Graphics.FromImage(renderedImage);
        RenderOutlines(renderedImage, g);
        g.Dispose();
      }
    }

    public ImageAttributes OverlayAttributes
    {
      get { return overlayAttributes; }
    }

    public PointF OverlayCenterInOverlayImage
    {
      get { return overlayCenterInOverlayImage; }
    }

    public PointF OverlayCenterInRenderedImage
    {
      get { return overlayCenterInRenderedImage; }
      set
      {
        if (overlayCenterInRenderedImage.X != value.X ||
            overlayCenterInRenderedImage.Y != value.Y)
        {
          overlayCenterInRenderedImage = value;
          hasBeenChanged = true;
          RecomputeOverlayTransformation();
        }
      }
    }

    /// <summary>
    /// The overlay image of the design.
    /// </summary>
    public string OverlayFile
    {
      get { return overlayFile; }
      set
      {
        if (value != overlayFile)
        {
          hasBeenChanged = true;
          // Also reset the overlay rotation.
          overlayRotationDegrees = 0F;
        }
        overlayFile = value;

        try
        {
          // Note: bitmap images can be created from files of the BMP, GIF,
          // EXIF, JPG, PNG, and TIFF formats, although not all of these will
          // be appropriate for use as a color image.
          Bitmap rawBitmapImage = new Bitmap(overlayFile);

          // When we create a bitmap from the specified file, the file may or
          // may not have had the right pixel format. We want a pixel format
          // that permits transparency as well as a decent color range.
          if (rawBitmapImage.PixelFormat == PixelFormat.Format32bppArgb)
          {
            // The image from file is the right format, so use it.
            overlayImage = rawBitmapImage;
          }
          else
          {
            // The image from file is not in the format that we need.
            // Create the overlay image bitmap to be the same size as the image
            // from file, but in 32bppARGB format, then draw the image from
            // file into the overlay image bitmap.
            overlayImage = new Bitmap(rawBitmapImage.Width,
              rawBitmapImage.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(overlayImage);
            g.DrawImage(rawBitmapImage, new Rectangle(
              0, 0, rawBitmapImage.Width, rawBitmapImage.Height));
            g.Dispose();
          }

          overlayCenterInOverlayImage.X = 0.5F * overlayImage.Width;
          overlayCenterInOverlayImage.Y = 0.5F * overlayImage.Height;
        }
        catch { }
      }
    }

    /// <summary>
    /// The overlay image of the design.
    /// </summary>
    public Bitmap OverlayImage
    {
      get { return overlayImage; }
    }

    public float OverlayRotationDegrees
    {
      get { return overlayRotationDegrees; }
    }

    public float OverlayTransparency
    {
      get { return overlayTransparency; }
      set
      {
        if (overlayTransparency != value)
        {
          overlayTransparency = value;
          hasBeenChanged = true;
        }
        overlayAttributes = TransparencyMapping(overlayTransparency);
      }
    }

    public PointF OverlayScaleFactors
    {
      get { return new PointF(overlayXScaleFactor, overlayYScaleFactor); }
      set
      {
        if (overlayXScaleFactor != value.X || overlayYScaleFactor != value.Y)
        {
          overlayXScaleFactor = value.X;
          overlayYScaleFactor = value.Y;
          hasBeenChanged = true;
          RecomputeOverlayTransformation();
        }
      }
    }

    public float OverlayXScaleFactor
    {
      get { return overlayXScaleFactor; }
      set
      {
        if (overlayXScaleFactor != value)
        {
          overlayXScaleFactor = value;
          hasBeenChanged = true;
          RecomputeOverlayTransformation();
        }
      }
    }

    public float OverlayYScaleFactor
    {
      get { return overlayYScaleFactor; }
      set
      {
        if (overlayYScaleFactor != value)
        {
          overlayYScaleFactor = value;
          hasBeenChanged = true;
          RecomputeOverlayTransformation();
        }
      }
    }

    public string PaletteFile
    {
      get { return paletteFile; }
      set
      {
        if (paletteFile != value)
        {
          paletteFile = value;
          hasBeenChanged = true;
        }
      }
    }

    public string PaletteName
    {
      get { return paletteName; }
      set
      {
        if (paletteName != value)
        {
          paletteName = value;
          hasBeenChanged = true;
        }
      }
    }

    public List<string> PaletteSectionsHidden
    {
      get { return hiddenPaletteSectionNames; }
      set
      {
        hiddenPaletteSectionNames = value;
      }
    }

    /// <summary>
    /// Determine which pattern element lies under the specified point in the
    /// rendered image; null if none (e.g. outline or background).
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public ChainmaillePatternElementId PatternElementAtRenderedPoint(
      Point point, out ChainmaillePatternElement referencedElement)
    {
      ChainmaillePatternElementId elementId = null;
      int x = point.X;
      int y = point.Y;
      int rxBase, ryBase;
      referencedElement = null;

      if (renderedImage != null && x >= 0 && y >= 0 &&
          x < renderedImage.Width && y < renderedImage.Height)
      {
        // Alternate point to test in case the ring we want wraps around.
        int aX = x;
        int aY = y;
        if (wrap == WrapEnum.Horizontal)
        {
          aX += (x < renderedImage.Width / 2 ?
            renderedImage.Width : -renderedImage.Width);
        }
        else if (wrap == WrapEnum.Vertical)
        {
          aY += (y < renderedImage.Height / 2 ?
            renderedImage.Height : -renderedImage.Height);
        }

        ChainmaillePatternSet patternSet;
        if (chainmaillePattern.HasEdges(EdgeGeometryEnum.Rectangular))
        {
          // Check for and assign elementId if part of corner or edge.
          int r, c;
          if (useCornersIfAvailable)
          {
            // Check top left corner.
            if (chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
              CornerOrientationEnum.TopLeft))
            {
              patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular, CornerOrientationEnum.TopLeft);
              r = 0;
              c = 0;
              rxBase = renderingMarginTopLeft.Width;
              ryBase = renderingMarginTopLeft.Height;
              if (TestPointAgainstBoundingBoxOfPatternSet(
                  rxBase, ryBase, x, y, aX, aY, patternSet))
              {
                // Test the elements.
                TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                  ryBase, patternSet, ref elementId, ref referencedElement);
                if (wrap != WrapEnum.None && elementId == null)
                {
                  TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                }
              }
            }
            // Check top right corner.
            if (elementId == null && chainmaillePattern.HasCorner(
              EdgeGeometryEnum.Rectangular, CornerOrientationEnum.TopRight))
            {
              patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular, CornerOrientationEnum.TopRight);
              r = 0;
              c = SizeInUnits.Width - patternSet.SizeInUnits.Width;
              rxBase = renderingMarginTopLeft.Width +
                edgesSizeInRenderedImageTopLeft.Width +
                (c - edgesSizeInPatternUnitsTopLeft.Width) *
                chainmaillePattern.HorizontalSpacing;
              ryBase = renderingMarginTopLeft.Height;
              if (TestPointAgainstBoundingBoxOfPatternSet(
                  rxBase, ryBase, x, y, aX, aY, patternSet))
              {
                // Test the elements.
                TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                  ryBase, patternSet, ref elementId, ref referencedElement);
                if (wrap != WrapEnum.None && elementId == null)
                {
                  TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                }
              }
            }
            // Check bottom left corner.
            if (elementId == null && chainmaillePattern.HasCorner(
              EdgeGeometryEnum.Rectangular, CornerOrientationEnum.BottomLeft))
            {
              patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular, CornerOrientationEnum.BottomLeft);
              r = SizeInUnits.Height - patternSet.SizeInUnits.Height;
              c = 0;
              rxBase = renderingMarginTopLeft.Width +
                (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
              ryBase = renderingMarginTopLeft.Height +
                edgesSizeInRenderedImageTopLeft.Height +
                (r - edgesSizeInPatternUnitsTopLeft.Height) *
                chainmaillePattern.VerticalSpacing +
                (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              if (TestPointAgainstBoundingBoxOfPatternSet(
                  rxBase, ryBase, x, y, aX, aY, patternSet))
              {
                // Test the elements.
                TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                  ryBase, patternSet, ref elementId, ref referencedElement);
                if (wrap != WrapEnum.None && elementId == null)
                {
                  TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                }
              }
            }
            // Check bottom right corner.
            if (elementId == null && chainmaillePattern.HasCorner(
              EdgeGeometryEnum.Rectangular, CornerOrientationEnum.BottomRight))
            {
              patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular, CornerOrientationEnum.BottomRight);
              r = SizeInUnits.Height - patternSet.SizeInUnits.Height;
              c = SizeInUnits.Width - patternSet.SizeInUnits.Width;
              rxBase = renderingMarginTopLeft.Width +
                edgesSizeInRenderedImageTopLeft.Width +
                (c - edgesSizeInPatternUnitsTopLeft.Width) *
                chainmaillePattern.HorizontalSpacing +
                (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
              ryBase = renderingMarginTopLeft.Height +
                edgesSizeInRenderedImageTopLeft.Height +
                (r - edgesSizeInPatternUnitsTopLeft.Height) *
                chainmaillePattern.VerticalSpacing +
                (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              if (TestPointAgainstBoundingBoxOfPatternSet(
                  rxBase, ryBase, x, y, aX, aY, patternSet))
              {
                // Test the elements.
                TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                  ryBase, patternSet, ref elementId, ref referencedElement);
                if (wrap != WrapEnum.None && elementId == null)
                {
                  TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                }
              }
            }
          }

          if (useLeftRightEdgesIfAvailable)
          {
            // Check left edge.
            if (elementId == null && chainmaillePattern.HasEdge(
              EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Left))
            {
              patternSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Left);
              c = 0;
              for (r = edgesSizeInPatternUnitsTopLeft.Height;
                   r < sizeInUnits.Height -
                     edgesSizeInPatternUnitsBottomRight.Height &&
                     elementId == null; r++)
              {
                rxBase = renderingMarginTopLeft.Width +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                ryBase = renderingMarginTopLeft.Height +
                  edgesSizeInRenderedImageTopLeft.Height +
                  (r - edgesSizeInPatternUnitsTopLeft.Height) *
                  chainmaillePattern.VerticalSpacing +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
                if (TestPointAgainstBoundingBoxOfPatternSet(
                    rxBase, ryBase, x, y, aX, aY, patternSet))
                {
                  // Test the elements.
                  TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                  if (wrap != WrapEnum.None && elementId == null)
                  {
                    TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                      ryBase, patternSet, ref elementId, ref referencedElement);
                  }
                }
              }
            }

            // Check right edge.
            if (elementId == null && chainmaillePattern.HasEdge(
              EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Right))
            {
              patternSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Right);
              c = sizeInUnits.Width - patternSet.SizeInUnits.Width;
              for (r = edgesSizeInPatternUnitsTopLeft.Height;
                   r < sizeInUnits.Height -
                     edgesSizeInPatternUnitsBottomRight.Height &&
                     elementId == null; r++)
              {
                rxBase = renderingMarginTopLeft.Width +
                  edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                ryBase = renderingMarginTopLeft.Height +
                  edgesSizeInRenderedImageTopLeft.Height +
                  (r - edgesSizeInPatternUnitsTopLeft.Height) *
                  chainmaillePattern.VerticalSpacing +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
                if (TestPointAgainstBoundingBoxOfPatternSet(
                    rxBase, ryBase, x, y, aX, aY, patternSet))
                {
                  // Test the elements.
                  TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                  if (wrap != WrapEnum.None && elementId == null)
                  {
                    TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                      ryBase, patternSet, ref elementId, ref referencedElement);
                  }
                }
              }
            }
          }

          if (useTopBottomEdgesIfAvailable)
          {
            // Check top edge.
            if (elementId == null && chainmaillePattern.HasEdge(
              EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Top))
            {
              patternSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Top);
              r = 0;
              ryBase = renderingMarginTopLeft.Height;
              for (c = edgesSizeInPatternUnitsTopLeft.Width;
                   c < sizeInUnits.Width -
                     edgesSizeInPatternUnitsBottomRight.Width &&
                     elementId == null; c++)
              {
                rxBase = renderingMarginTopLeft.Width +
                  edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing;
                if (TestPointAgainstBoundingBoxOfPatternSet(
                    rxBase, ryBase, x, y, aX, aY, patternSet))
                {
                  // Test the elements.
                  TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                  if (wrap != WrapEnum.None && elementId == null)
                  {
                    TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                      ryBase, patternSet, ref elementId, ref referencedElement);
                  }
                }
              }
            }

            // Check bottom edge.
            if (elementId == null && chainmaillePattern.HasEdge(
              EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Bottom))
            {
              patternSet = chainmaillePattern.Edge(EdgeGeometryEnum.Rectangular,
                EdgeOrientationEnum.Bottom);
              r = sizeInUnits.Height - patternSet.SizeInUnits.Height;
              ryBase = renderingMarginTopLeft.Height +
                edgesSizeInRenderedImageTopLeft.Height +
                (r - edgesSizeInPatternUnitsTopLeft.Height) *
                chainmaillePattern.VerticalSpacing +
                (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              for (c = edgesSizeInPatternUnitsTopLeft.Width;
                    c < sizeInUnits.Width -
                      edgesSizeInPatternUnitsBottomRight.Width &&
                      elementId == null; c++)
              {
                rxBase = renderingMarginTopLeft.Width +
                  edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                if (TestPointAgainstBoundingBoxOfPatternSet(
                    rxBase, ryBase, x, y, aX, aY, patternSet))
                {
                  // Test the elements.
                  TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                  if (wrap != WrapEnum.None && elementId == null)
                  {
                    TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                      ryBase, patternSet, ref elementId, ref referencedElement);
                  }
                }
              }
            }
          }
        }

        if (elementId == null)
        {
          patternSet = chainmaillePattern.BasicPatternSet;
          for (int r = edgesSizeInPatternUnitsTopLeft.Height;
               r < sizeInUnits.Height -
               edgesSizeInPatternUnitsBottomRight.Height &&
               elementId == null; r++)
          {
            ryBase = renderingMarginTopLeft.Height +
              edgesSizeInRenderedImageTopLeft.Height +
              (r - edgesSizeInPatternUnitsTopLeft.Height) *
              chainmaillePattern.VerticalSpacing +
              (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
            // Test whether y is in range for this row; if not, no need to
            // loop through the columns.
            for (int c = edgesSizeInPatternUnitsTopLeft.Width;
                  c < sizeInUnits.Width -
                  edgesSizeInPatternUnitsBottomRight.Width &&
                  elementId == null; c++)
            {
              rxBase = renderingMarginTopLeft.Width +
                edgesSizeInRenderedImageTopLeft.Width +
                (c - edgesSizeInPatternUnitsTopLeft.Width) *
                chainmaillePattern.HorizontalSpacing +
                (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
              if (TestPointAgainstBoundingBoxOfPatternSet(
                  rxBase, ryBase, x, y, aX, aY, patternSet))
              {
                TestPointAgainstElementsOfPatternSet(r, c, x, y, rxBase,
                  ryBase, patternSet, ref elementId, ref referencedElement);
                if (wrap != WrapEnum.None && elementId == null)
                {
                  TestPointAgainstElementsOfPatternSet(r, c, aX, aY, rxBase,
                    ryBase, patternSet, ref elementId, ref referencedElement);
                }
              }
            }
          }
        }
      }

      return elementId;
    }

    /// <summary>
    /// Reads the design from the established XML file.
    /// </summary>
    /// <returns>error messages, or empty string if no errors.</returns>
    private string ReadDesignFromXml()
    {
      messages = string.Empty;

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(File.ReadAllText(designFile));

        XmlNode designNode = doc.SelectSingleNode("ChainmailleDesign");
        if (designNode == null)
        {
          // Try old-style node.
          designNode = doc.SelectSingleNode("ChainmailDesign");
        }
        if (designNode != null)
        {
          XmlAttribute designNameAttribute = designNode.Attributes["name"];
          if (designNameAttribute != null)
          {
            designName = designNameAttribute.Value;
          }

          // Information about the design.
          XmlNode designedByNode = designNode.SelectSingleNode("DesignedBy");
          if (designedByNode != null)
          {
            designedBy = designedByNode.InnerText;
          }
          XmlNode dateNode = designNode.SelectSingleNode("Date");
          if (dateNode != null)
          {
            designDate = DateTime.Parse(dateNode.InnerText);
          }
          XmlNode designedForNode = designNode.SelectSingleNode("DesignedFor");
          if (designedForNode != null)
          {
            designedFor = designedForNode.InnerText;
          }
          XmlNode descriptionNode = designNode.SelectSingleNode("Description");
          if (descriptionNode != null)
          {
            designDescription = descriptionNode.InnerText;
          }

          // Palette.
          XmlNode paletteNode = designNode.SelectSingleNode("Palette");
          if (paletteNode != null)
          {
            XmlAttribute paletteNameAttribute = paletteNode.Attributes["name"];
            if (paletteNameAttribute != null)
            {
              paletteName = paletteNameAttribute.Value;
            }
            XmlAttribute paletteFileAttribute = paletteNode.Attributes["file"];
            if (paletteFileAttribute != null)
            {
              paletteFile = paletteFileAttribute.Value;
            }
            hiddenPaletteSectionNames = new List<string>();
            XmlNodeList hiddenSectionNodes =
              paletteNode.SelectNodes("HiddenSection");
            foreach (XmlNode hiddenSectionNode in hiddenSectionNodes)
            {
              XmlAttribute hiddenSectionNameAttribute =
                hiddenSectionNode.Attributes["name"];
              if (hiddenSectionNameAttribute != null)
              {
                hiddenPaletteSectionNames.Add(
                  hiddenSectionNameAttribute.Value);
              }
            }
          }

          // Display rotation.
          XmlNode displayRotationNode =
            designNode.SelectSingleNode("DisplayRotation");
          if (displayRotationNode != null)
          {
            XmlAttribute degreesAttribute =
              displayRotationNode.Attributes["degrees"];
            if (degreesAttribute != null)
            {
              displayRotationDegrees = float.Parse(degreesAttribute.Value);
            }
          }

          bool foundADesign = false;
          XmlNodeList sectionNodes = designNode.SelectNodes("DesignSection");
          foreach (XmlNode sectionNode in sectionNodes)
          {
            // For now, only process the first node.
            if (!foundADesign)
            {
              patternFile = Properties.Settings.Default.CurrentPattern;
              XmlNode patternNode =
                sectionNode.SelectSingleNode("ChainmaillePattern");
              if (patternNode == null)
              {
                // Try old-style node.
                patternNode =
                  sectionNode.SelectSingleNode("ChainmailPattern");
              }
              if (patternNode != null)
              {
                XmlAttribute patternNameAttribute =
                  patternNode.Attributes["name"];
                if (patternNameAttribute != null)
                {
                  weaveName = patternNameAttribute.Value;
                }
                XmlAttribute patternFileAttribute =
                  patternNode.Attributes["file"];
                if (patternFileAttribute != null)
                {
                  patternFile = patternFileAttribute.Value;
                }
              }
              if (string.IsNullOrEmpty(weaveName))
              {
                weaveName = Path.GetFileNameWithoutExtension(patternFile);
              }
              chainmaillePattern = new ChainmaillePattern(patternFile, weaveName);
              if (!chainmaillePattern.IsInitialized)
              {
                chainmaillePattern = null;
                if (Path.IsPathRooted(patternFile))
                {
                  // We were handed the full path.
                  messages += "The chainmaille pattern file " + patternFile +
                    " for the " + weaveName + " weave either could not be" +
                    " found or was unreadable. The filepath was fully" +
                    " specified in the design file, but can be changed by" +
                    " selecting an equivalent weave from the design" +
                    " information dialog (Design->Info from the menu) .";
                }
                else
                {
                  // Not a full path, so we were looking in the configured
                  // directory.
                  messages += "The chainmaille pattern file " + patternFile +
                    " for the " + weaveName + " weave either could not be" +
                    " found in the configured Weave Pattern Files directory" +
                    " or was unreadable. You may wish either to move the" +
                    " pattern files under the " +
                    Properties.Settings.Default.PatternDirectory +
                    " directory or change the directory configuration" +
                    " (Help->Configuration from the menu). " +
                    "Alternatively, you can select an equivalent weave" +
                    " for the design (Design->Info from the menu). ";
                }
              }
              else if (chainmaillePattern.PatternDirectory != patternFile)
              {
                // We found the pattern, but it wasn't where we were told.
                patternFile = chainmaillePattern.PatternDirectory;
                hasBeenChanged = true;
              }

              XmlNode designSizeNode =
                sectionNode.SelectSingleNode("DesignSize");
              if (designSizeNode != null)
              {
                XmlAttribute rowsAttribute =
                  designSizeNode.Attributes["rows"];
                if (rowsAttribute != null)
                {
                  sizeInUnits.Height = int.Parse(rowsAttribute.Value);
                }
                XmlAttribute columnsAttribute =
                  designSizeNode.Attributes["columns"];
                if (columnsAttribute != null)
                {
                  sizeInUnits.Width = int.Parse(columnsAttribute.Value);
                }
                XmlAttribute wrapAttribute =
                  designSizeNode.Attributes["wrap"];
                if (wrapAttribute != null)
                {
                  wrap = EnumUtils.ToEnumFromDescription<WrapEnum>(
                    wrapAttribute.Value);
                }
              }

              XmlNode scaleNode =
                sectionNode.SelectSingleNode("Scale");
              if (scaleNode != null)
              {
                scale = new PatternScale(scaleNode);
              }

              XmlNode colorImageNode =
                sectionNode.SelectSingleNode("ColorImage");
              if (colorImageNode != null)
              {
                string colorImageName = string.Empty;
                string colorImageFile = string.Empty;
                XmlAttribute colorImageNameAttribute =
                  colorImageNode.Attributes["name"];
                if (colorImageNameAttribute != null)
                {
                  colorImageName = colorImageNameAttribute.Value;
                }
                XmlAttribute colorImageFileAttribute =
                  colorImageNode.Attributes["file"];
                if (colorImageFileAttribute != null)
                {
                  colorImageFile = colorImageFileAttribute.Value;
                }
                try
                {
                  colorImage = new ColorImage(colorImageFile);
                }
                catch
                {
                  // The file path was not valid. Try to find the file
                  // elsewhere.
                  string alternativeFile = FileUtils.FindSimilarFile(
                    colorImageFile, new List<string>()
                    {
                      Path.GetDirectoryName(designFile),
                      Properties.Settings.Default.DesignDirectory
                    }, ColorImage.ColorImageFileExtensions);
                  if (!string.IsNullOrEmpty(alternativeFile))
                  {
                    try
                    {
                      colorImage = new ColorImage(alternativeFile);
                      colorImageFile = alternativeFile;
                      hasBeenChanged = true;
                    }
                    catch
                    {
                      messages += "Could not read color image from " +
                        colorImageFile + ". ";
                    }
                  }
                  else
                  {
                    messages += "Could not read color image from " +
                      colorImageFile + ". ";
                  }
                }
              }

              XmlNode overlayNode =
                sectionNode.SelectSingleNode("Overlay");
              if (overlayNode != null)
              {
                XmlAttribute overlayImageNameAttribute =
                  overlayNode.Attributes["name"];
                if (overlayImageNameAttribute != null)
                {
                  overlayName = overlayImageNameAttribute.Value;
                }
                XmlAttribute overlayImageFileAttribute =
                  overlayNode.Attributes["file"];
                if (overlayImageFileAttribute != null)
                {
                  overlayFile = overlayImageFileAttribute.Value;
                }
                OverlayFile = overlayFile;
                if (!string.IsNullOrEmpty(overlayFile) && overlayImage == null)
                {
                  // The file path was not valid. Try to find the file
                  // elsewhere.
                  string originalOverlayFile = overlayFile;
                  string alternativeFile = FileUtils.FindSimilarFile(
                    overlayFile, new List<string>()
                    {
                      Path.GetDirectoryName(designFile),
                      Properties.Settings.Default.DesignDirectory
                    }, OverlayImageFileExtensions);
                  if (!string.IsNullOrEmpty(alternativeFile))
                  {
                    OverlayFile = alternativeFile;
                  }
                  if (overlayImage == null)
                  {
                    messages += "Could not read overlay image from " +
                      originalOverlayFile + ". ";
                  }
                }
                overlayIsShowing = true;
                XmlAttribute overlayShownAttribute =
                  overlayNode.Attributes["shown"];
                if (overlayShownAttribute != null)
                {
                  overlayIsShowing = overlayShownAttribute.Value != "false";
                }

                XmlNode transparencyNode =
                  overlayNode.SelectSingleNode("Transparency");
                if (transparencyNode != null)
                {
                  XmlAttribute transparencyValueAttribute =
                    transparencyNode.Attributes["value"];
                  if (transparencyValueAttribute != null)
                  {
                    // Pre-set the private value. We don't want the
                    // transparency to set the "changed" flag, but we do
                    // want setting of the public property to set up the
                    // transparency mapping.
                    overlayTransparency =
                      float.Parse(transparencyValueAttribute.Value);
                    OverlayTransparency = overlayTransparency;
                  }
                }

                XmlNode overlayCenterNode =
                  overlayNode.SelectSingleNode("Center");
                if (overlayCenterNode != null)
                {
                  XmlAttribute centerReferenceAttribute =
                    overlayCenterNode.Attributes["reference"];
                  if (centerReferenceAttribute != null)
                  {
                    if (centerReferenceAttribute.Value == "rendered image")
                    {
                      XmlAttribute centerXAttribute =
                        overlayCenterNode.Attributes["x"];
                      XmlAttribute centerYAttribute =
                        overlayCenterNode.Attributes["y"];
                      if (centerXAttribute != null && centerYAttribute != null)
                      {
                        overlayCenterInRenderedImage.X =
                          float.Parse(centerXAttribute.Value);
                        overlayCenterInRenderedImage.Y =
                          float.Parse(centerYAttribute.Value);
                      }
                    }
                  }
                }

                XmlNode overlayScaleNode =
                  overlayNode.SelectSingleNode("Scale");
                if (overlayScaleNode != null)
                {
                  XmlAttribute scaleReferenceAttribute =
                    overlayScaleNode.Attributes["reference"];
                  if (scaleReferenceAttribute != null)
                  {
                    if (scaleReferenceAttribute.Value == "rendered image")
                    {
                      XmlAttribute scaleXAttribute =
                        overlayScaleNode.Attributes["x"];
                      XmlAttribute scaleYAttribute =
                        overlayScaleNode.Attributes["y"];
                      if (scaleXAttribute != null && scaleYAttribute != null)
                      {
                        overlayXScaleFactor =
                          float.Parse(scaleXAttribute.Value);
                        overlayYScaleFactor =
                          float.Parse(scaleYAttribute.Value);
                      }
                    }
                  }
                }

                XmlNode overlayRotationNode =
                  overlayNode.SelectSingleNode("Rotation");
                if (overlayRotationNode != null)
                {
                  XmlAttribute overlayRotationAttribute =
                    overlayRotationNode.Attributes["degrees"];
                  if (overlayRotationAttribute != null)
                  {
                    overlayRotationDegrees =
                      float.Parse(overlayRotationAttribute.Value);
                  }
                }
              }

              // Guidelines.
              horizontalGuidelines = new SortedSet<int>();
              verticalGuidelines = new SortedSet<int>();
              guidelinesAreShowing = true;
              XmlNode guidelinesNode =
                designNode.SelectSingleNode("Guidelines");
              if (guidelinesNode != null)
              {
                XmlAttribute guidelinesShownAttribute =
                  guidelinesNode.Attributes["shown"];
                if (guidelinesShownAttribute != null)
                {
                  guidelinesAreShowing =
                    guidelinesShownAttribute.Value != "false";
                }
                XmlNodeList horizontalGuidelineNodes =
                  guidelinesNode.SelectNodes("HorizontalGuideline");
                foreach (XmlNode guidelineNode in horizontalGuidelineNodes)
                {
                  XmlAttribute guidelinePositionAttribute =
                    guidelineNode.Attributes["position"];
                  if (guidelinePositionAttribute != null)
                  {
                    horizontalGuidelines.Add(
                      int.Parse(guidelinePositionAttribute.Value));
                  }
                }
                XmlNodeList verticalGuidelineNodes =
                  guidelinesNode.SelectNodes("VerticalGuideline");
                foreach (XmlNode guidelineNode in verticalGuidelineNodes)
                {
                  XmlAttribute guidelinePositionAttribute =
                    guidelineNode.Attributes["position"];
                  if (guidelinePositionAttribute != null)
                  {
                    verticalGuidelines.Add(
                      int.Parse(guidelinePositionAttribute.Value));
                  }
                }
              }

              foundADesign = true;
            }
          }
        }
        else
        {
          messages += "Although the file " + designFile + "appears to be" +
            " an XML file, it is not a chainmaille design file. ";
        }

      }
      catch
      {
        messages += "The file " + designFile +
          " is not a well-formed XML chainmaille design file. ";
      }

      return messages;
    }

    private void RenderAllPatternElements(Bitmap rendering, Graphics g,
      Dictionary<Color, Color> colorReplacements = null,
      string ringFilter = null, bool pushBackToColorImage = false,
      ColorImage alternateColorImage = null)
    {
      // Note: The alternateColorImage, if supplied, must be of the same size
      // as the color image of the design.
      if (rendering != null && g != null && chainmaillePattern != null &&
          chainmaillePattern.PatternElements.Count > 0)
      {
        int cx, cy, cxBase, cyBase;
        int rx, ry, rxBase, ryBase, rxBaseOffset, ryBaseOffset;
        ChainmaillePatternElement element;
        Color elementColor;
        ChainmaillePatternSet patternSet = chainmaillePattern.BasicPatternSet;
        Size imageSizeInUnits = chainmaillePattern.SizeInUnits;

        if (chainmaillePattern.Linkage == WeaveLinkageEnum.Sheet &&
            chainmaillePattern.HasEdges(EdgeGeometryEnum.Rectangular))
        {
          int bottomRowMarginInUnits =
            Math.Max(1, edgesSizeInPatternUnitsBottomRight.Height);
          int rightColumnMarginInUnits =
            Math.Max(1, edgesSizeInPatternUnitsBottomRight.Width);

          ryBaseOffset = 0;
          for (int r = 0; r < sizeInUnits.Height;)
          {
            cyBase = r * chainmaillePattern.ColorSize.Height;
            ryBase = ryBaseOffset +
              (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
            bool isLastRow =
              sizeInUnits.Height - r <= bottomRowMarginInUnits;
            rxBaseOffset = 0;
            for (int c = 0; c < sizeInUnits.Width;)
            {
              cxBase = c * chainmaillePattern.ColorSize.Width;
              rxBase = rxBaseOffset +
                (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
              bool isLastColumn =
                sizeInUnits.Width - c <= rightColumnMarginInUnits;
              // Subsequently, we test whether we need to draw a corner or edge
              // image instead of the normal image, so here we default the
              // image to draw the normal image.
              patternSet = null;
              imageSizeInUnits = chainmaillePattern.SizeInUnits;
              if (r == 0)
              {
                // Top row; draw corners and/or edges as needed.
                if (c == 0 && useCornersIfAvailable &&
                    chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopLeft))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopLeft);
                }
                else if (isLastColumn && useCornersIfAvailable &&
                         chainmaillePattern.HasCorner(
                           EdgeGeometryEnum.Rectangular,
                           CornerOrientationEnum.TopRight))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopRight);
                }
                else if (useTopBottomEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Top))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Top);
                }
                else if (c == 0 && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }
              else if (isLastRow)
              {
                // Bottom row; draw corners and/or edges as needed.
                if (c == 0 && useCornersIfAvailable &&
                    chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomLeft))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomLeft);
                }
                else if (isLastColumn && useCornersIfAvailable &&
                         chainmaillePattern.HasCorner(
                           EdgeGeometryEnum.Rectangular,
                           CornerOrientationEnum.BottomRight))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomRight);
                }
                else if (useTopBottomEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Bottom))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Bottom);
                }
                else if (c == 0 && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }
              else
              {
                // A middle row; draw left/right edges as needed.
                if (c == 0 && useLeftRightEdgesIfAvailable &&
                    chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }

              if (patternSet == null || patternSet.OutlineImage == null)
              {
                patternSet = chainmaillePattern.BasicPatternSet;
              }
              imageSizeInUnits = patternSet.SizeInUnits;


              for (int e = 0; e < patternSet.PatternElements.Count; e++)
              {
                element = patternSet.PatternElements[e];
                if (element.Image != null)
                {
                  cx = cxBase + element.ColorOffset.X;
                  cy = cyBase + element.ColorOffset.Y;
                  if (alternateColorImage != null)
                  {
                    // Color the rendering per the alternate colors supplied.
                    elementColor = alternateColorImage.ColorAt(cx, cy);
                  }
                  else
                  {
                    // Color the rendering using the design colors.
                    elementColor = colorImage.ColorAt(cx, cy);
                  }

                  // Check for rendering in alternate colors.
                  if (colorReplacements != null)
                  {
                    if ((string.IsNullOrEmpty(ringFilter) ||
                         element.RingSizeName == ringFilter) &&
                        colorReplacements.ContainsKey(elementColor))
                    {
                      elementColor = colorReplacements[elementColor];
                      if (pushBackToColorImage)
                      {
                        colorImage.BitmapImage.SetPixel(cx, cy, elementColor);
                      }
                    }
                  }

                  rx = rxBase + element.PatternOffset.X;
                  ry = ryBase + element.PatternOffset.Y;
                  Rectangle destinationRectangle = new Rectangle(rx, ry,
                    element.Image.Width, element.Image.Height);
                  g.DrawImage(element.Image, destinationRectangle, 0, 0,
                    element.Image.Width, element.Image.Height,
                    GraphicsUnit.Pixel, ColorMapping(elementColor));
                  // Check for whether the element is wrapped. If so, also draw the
                  // wrapped portion of the element.
                  if (wrap == WrapEnum.Horizontal)
                  {
                    if (destinationRectangle.X < 0)
                    {
                      destinationRectangle.X += rendering.Width;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                    else if (destinationRectangle.X + destinationRectangle.Width >
                        rendering.Width)
                    {
                      destinationRectangle.X -= rendering.Width;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                  }
                  else if (wrap == WrapEnum.Vertical)
                  {
                    if (destinationRectangle.Y < 0)
                    {
                      destinationRectangle.Y += rendering.Height;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                    else if (destinationRectangle.Y + destinationRectangle.Height >
                             rendering.Height)
                    {
                      destinationRectangle.Y -= rendering.Height;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                  }
                }
              }

              rxBaseOffset += patternSet.RenderingSpacing.Width;
              c += imageSizeInUnits.Width;
            }

            ryBaseOffset += patternSet.RenderingSpacing.Height;
            r += imageSizeInUnits.Height;
          }
        }
        else
        {
          for (int r = 0; r < sizeInUnits.Height; r++)
          {
            cyBase = r * chainmaillePattern.ColorSize.Height;
            ryBase = r * chainmaillePattern.VerticalSpacing +
              (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0) +
              renderingMarginTopLeft.Height;
            for (int c = 0; c < sizeInUnits.Width; c++)
            {
              cxBase = c * chainmaillePattern.ColorSize.Width;
              rxBase = c * chainmaillePattern.HorizontalSpacing +
                (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0) +
                renderingMarginTopLeft.Width;
              for (int e = 0; e < chainmaillePattern.PatternElements.Count; e++)
              {
                element = chainmaillePattern.PatternElements[e];
                if (element.Image != null)
                {
                  cx = cxBase + element.ColorOffset.X;
                  cy = cyBase + element.ColorOffset.Y;
                  elementColor = colorImage.ColorAt(cx, cy);

                  // Check for rendering in alternate colors.
                  if (colorReplacements != null)
                  {
                    if ((string.IsNullOrEmpty(ringFilter) ||
                         element.RingSizeName == ringFilter) &&
                        colorReplacements.ContainsKey(elementColor))
                    {
                      elementColor = colorReplacements[elementColor];
                      if (pushBackToColorImage)
                      {
                        colorImage.BitmapImage.SetPixel(cx, cy, elementColor);
                      }
                    }
                  }

                  rx = rxBase + element.PatternOffset.X;
                  ry = ryBase + element.PatternOffset.Y;
                  Rectangle destinationRectangle = new Rectangle(rx, ry,
                    element.Image.Width, element.Image.Height);
                  g.DrawImage(element.Image, destinationRectangle, 0, 0,
                    element.Image.Width, element.Image.Height,
                    GraphicsUnit.Pixel, ColorMapping(elementColor));
                  // Check for whether the element is wrapped. If so, also draw the
                  // wrapped portion of the element.
                  if (wrap == WrapEnum.Horizontal)
                  {
                    if (destinationRectangle.X < 0)
                    {
                      destinationRectangle.X += rendering.Width;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                    else if (destinationRectangle.X + destinationRectangle.Width >
                        rendering.Width)
                    {
                      destinationRectangle.X -= rendering.Width;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                  }
                  else if (wrap == WrapEnum.Vertical)
                  {
                    if (destinationRectangle.Y < 0)
                    {
                      destinationRectangle.Y += rendering.Height;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                    else if (destinationRectangle.Y + destinationRectangle.Height >
                             rendering.Height)
                    {
                      destinationRectangle.Y -= rendering.Height;
                      g.DrawImage(element.Image, destinationRectangle, 0, 0,
                        element.Image.Width, element.Image.Height,
                        GraphicsUnit.Pixel, ColorMapping(elementColor));
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Paint the entire background of the rendered image in the background
    /// color.
    /// </summary>
    /// <param name="rendering"></param>
    /// <param name="g"></param>
    private void RenderBackground(Bitmap rendering, Graphics g)
    {
      if (rendering != null && g != null)
      {
        g.Clear(backgroundColor);
      }
    }

    /// <summary>
    /// The rendered image of the design.
    /// </summary>
    public Bitmap RenderedImage
    {
      get { return renderedImage; }
    }

    /// <summary>
    /// The rendered image of the design, transformed per the display
    /// transformation. This is a new bitmap and should be properly
    /// disposed by the caller.
    /// </summary>
    public Bitmap TransformedRenderedImage
    {
      get
      {
        return TransformRenderedImageForDisplay(renderedImage);
      }
    }

    public Size TransformedRenderedImageSize
    {
      get
      {
        Size transformedSize = new Size();
        if (renderedImage != null)
        {
          Rectangle renderedImageRectangle = new Rectangle(
            0, 0, renderedImage.Width, renderedImage.Height);
          PointF renderedImageUpperLeft = new PointF(0, 0);
          PointF renderedImageLowerRight = new PointF(
            renderedImage.Width, renderedImage.Height);
          PointF transformedUpperLeft =
            TransformRenderedImagePointFToDisplay(renderedImageUpperLeft);
          PointF transformedLowerRight =
            TransformRenderedImagePointFToDisplay(renderedImageLowerRight);
          transformedSize = new Size(
            (int)Math.Round(
              Math.Max(transformedUpperLeft.X, transformedLowerRight.X) -
              Math.Min(transformedUpperLeft.X, transformedLowerRight.X)),
            (int)Math.Round(
              Math.Max(transformedUpperLeft.Y, transformedLowerRight.Y) -
              Math.Min(transformedUpperLeft.Y, transformedLowerRight.Y)));
        }

        return transformedSize;
      }
    }

    public Bitmap TransformRenderedImageForDisplay(Bitmap renderedImage)
    {
      Bitmap transformedRenderedImage = null;
      if (renderedImage != null)
      {
        // Determine the size of the transformed image.
        Size transformedSize = TransformedRenderedImageSize;

        // Create the image and its graphics.
        transformedRenderedImage = new Bitmap(
          transformedSize.Width, transformedSize.Height);
        Graphics g = Graphics.FromImage(transformedRenderedImage);

        // Set the transformation for display graphics.
        g.Transform = RenderedImageToDisplayTransformation;
        // Draw the transformed image.
        Rectangle renderedImageRectangle = new Rectangle(
          0, 0, renderedImage.Width, renderedImage.Height);
        g.DrawImage(renderedImage, renderedImageRectangle,
          renderedImageRectangle, GraphicsUnit.Pixel);
        // Dispose of the graphics.
        g.Dispose();
      }

      return transformedRenderedImage;
    }

    public Matrix DisplayToRenderedImageTransformation
    {
      get { return displayToRenderedImageTransformation.Clone(); }
    }

    public Matrix RenderedImageToDisplayTransformation
    {
      get { return renderedImageToDisplayTransformation.Clone(); }
    }

    /// <summary>
    /// Render the design into a displayable image.
    /// </summary>
    /// <returns></returns>
    public Bitmap RenderImage(bool isAlternativeRendering = false,
      Dictionary<Color, Color> colorReplacements = null,
      string ringFilter = null, bool pushBackToColorImage = false,
      ColorImage alternateColorImage = null)
    { 
      Bitmap rendering = null;
      Size? size = ComputeRenderedImageSize();
      if (size.HasValue)
      {
        Point origin = new Point(0, 0);
        Rectangle rect = new Rectangle(origin, size.Value);
        rendering = new Bitmap(rect.Width, rect.Height);
        Graphics g = Graphics.FromImage(rendering);
        RenderBackground(rendering, g);
        RenderOutlines(rendering, g);
        RenderAllPatternElements(rendering, g, colorReplacements, ringFilter,
          pushBackToColorImage, alternateColorImage);
        g.Dispose();
      }

      if (rendering != null && !isAlternativeRendering)
      {
        renderedImage = rendering;
      }

      return rendering;
    }

    /// <summary>
    /// Draw the pattern element outlines on the rendered image.
    /// </summary>
    /// <param name="rendering"></param>
    /// <param name="g"></param>
    private void RenderOutlines(Bitmap rendering, Graphics g)
    {
      if (rendering != null && g != null && chainmaillePattern != null &&
          chainmaillePattern.OutlineImage != null)
      {
        int x, y;
        Bitmap outlineImage = chainmaillePattern.OutlineImage;
        ChainmaillePatternSet patternSet;
        Size imageSizeInUnits = chainmaillePattern.SizeInUnits;

        if (chainmaillePattern.Linkage == WeaveLinkageEnum.Sheet &&
            chainmaillePattern.HasEdges(EdgeGeometryEnum.Rectangular))
        {
          int bottomRowMarginInUnits =
            Math.Max(1, edgesSizeInPatternUnitsBottomRight.Height);
          int rightColumnMarginInUnits =
            Math.Max(1, edgesSizeInPatternUnitsBottomRight.Width);

          y = 0;
          for (int r = 0; r < sizeInUnits.Height;)
          {
            x = 0;
            bool isLastRow =
              sizeInUnits.Height - r <= bottomRowMarginInUnits;
            for (int c = 0; c < sizeInUnits.Width;)
            {
              bool isLastColumn =
                sizeInUnits.Width - c <= rightColumnMarginInUnits;

              // Subsequently, we test whether we need to draw a corner or edge
              // pattern instead of the normal pattern, so here we default the
              // pattern to null to indicate no special corner or edge
              // treatment.
              patternSet = null;

              if (r == 0)
              {
                // Top row; draw corners and/or edges as needed.
                if (c == 0 && useCornersIfAvailable &&
                    chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopLeft))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopLeft);
                }
                else if (isLastColumn && useCornersIfAvailable &&
                         chainmaillePattern.HasCorner(
                           EdgeGeometryEnum.Rectangular,
                           CornerOrientationEnum.TopRight))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.TopRight);
                }
                else if (useTopBottomEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Top))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Top);
                }
                else if (c ==0 && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }
              else if (isLastRow)
              {
                // Bottom row; draw corners and/or edges as needed.
                if (c == 0 && useCornersIfAvailable &&
                    chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomLeft))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomLeft);
                }
                else if (isLastColumn && useCornersIfAvailable &&
                         chainmaillePattern.HasCorner(
                           EdgeGeometryEnum.Rectangular,
                           CornerOrientationEnum.BottomRight))
                {
                  patternSet = chainmaillePattern.Corner(
                    EdgeGeometryEnum.Rectangular,
                    CornerOrientationEnum.BottomRight);
                }
                else if (useTopBottomEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Bottom))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Bottom);
                }
                else if (c == 0 && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }
              else
              {
                // A middle row; draw left/right edges as needed.
                if (c == 0 && useLeftRightEdgesIfAvailable &&
                    chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left);
                }
                else if (isLastColumn && useLeftRightEdgesIfAvailable &&
                         chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                         EdgeOrientationEnum.Right))
                {
                  patternSet = chainmaillePattern.Edge(
                    EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right);
                }
                else
                {
                  // Normal non-edge draw.
                }
              }

              if (patternSet == null || patternSet.OutlineImage == null)
              {
                patternSet = chainmaillePattern.BasicPatternSet;
              }
              outlineImage = patternSet.OutlineImage;
              imageSizeInUnits = patternSet.SizeInUnits;

              Rectangle destinationRectangle = new Rectangle(x, y,
                outlineImage.Width, outlineImage.Height);
              g.DrawImage(outlineImage, destinationRectangle,
                0, 0, outlineImage.Width, outlineImage.Height,
                GraphicsUnit.Pixel, ColorMapping(outlineColor));

              x += outlineImage.Width;
              c += imageSizeInUnits.Width;
            }

            y += outlineImage.Height;
            r += imageSizeInUnits.Height;
          }
        }
        else
        {
          // One extra iteration left and top plus two extra iterations right
          // and bottom to fill rendering margin, if any.  This accommodates
          // the wierd half-Persian patterns with negative offsets and pattern
          // extents that go beyond the rendering size.
          for (int r = -imageSizeInUnits.Height;
                 r < sizeInUnits.Height + 2 * imageSizeInUnits.Height;
                 r += imageSizeInUnits.Height)
          {
            y = r * outlineImage.Height + renderingMarginTopLeft.Height;
            for (int c = -imageSizeInUnits.Width;
                   c < sizeInUnits.Width + 2 * imageSizeInUnits.Width;
                   c += imageSizeInUnits.Width)
            {
              x = c * outlineImage.Width + renderingMarginTopLeft.Width;
              Rectangle destinationRectangle = new Rectangle(x, y,
                outlineImage.Width, outlineImage.Height);
              g.DrawImage(outlineImage, destinationRectangle,
                0, 0, outlineImage.Width, outlineImage.Height,
                GraphicsUnit.Pixel, ColorMapping(outlineColor));
            }
          }
        }
      }
    }

    /// <summary>
    /// Render a single pattern element.
    /// </summary>
    /// <param name="elementId"></param>
    public void RenderPatternElement(ChainmaillePatternElementId elementId,
      ChainmaillePatternElement referencedElement)
    {
      Point? cPoint = ElementToPointInColorImage(elementId, referencedElement);
      if (cPoint.HasValue && renderedImage != null)
      {
        Color elementColor = colorImage.ColorAt(cPoint.Value.X,
          cPoint.Value.Y);
        if (elementColor.A != 255)
        {
          elementColor = backgroundColor;
        }
        int r = elementId.Row - 1;
        int c = elementId.Column - 1;
        int e = elementId.ElementIndex - 1;

        ChainmaillePatternElement element = null;
        int rx = 0;
        int ry = 0;

        if (chainmaillePattern.HasEdges(EdgeGeometryEnum.Rectangular))
        {
          // Check for and set element, rx, & ry if part of corner or edge.
          if (useCornersIfAvailable)
          {
            if (chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                  CornerOrientationEnum.TopLeft))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular, CornerOrientationEnum.TopLeft);
              if (r == 0 && c == 0)
              {
                element = patternSet.PatternElements[e];
                rx = element.PatternOffset.X;
                ry = element.PatternOffset.Y;
              }
            }
            if (chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                  CornerOrientationEnum.TopRight))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular, CornerOrientationEnum.TopRight);
              if (r == 0 &&
                  c == SizeInUnits.Width - patternSet.SizeInUnits.Width)
              {
                element = patternSet.PatternElements[e];
                rx = edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing +
                  element.PatternOffset.X;
                ry = element.PatternOffset.Y;
              }
            }
            if (chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                  CornerOrientationEnum.BottomLeft))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular,
                CornerOrientationEnum.BottomLeft);
              if (r == SizeInUnits.Height - patternSet.SizeInUnits.Height &&
                  c == 0)
              {
                element = patternSet.PatternElements[e];
                rx = element.PatternOffset.X +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                ry = edgesSizeInRenderedImageTopLeft.Height +
                  (r - edgesSizeInPatternUnitsTopLeft.Height) *
                  chainmaillePattern.VerticalSpacing +
                  element.PatternOffset.Y +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              }
            }
            if (chainmaillePattern.HasCorner(EdgeGeometryEnum.Rectangular,
                  CornerOrientationEnum.BottomRight))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Corner(
                EdgeGeometryEnum.Rectangular,
                CornerOrientationEnum.BottomRight);
              if (r == SizeInUnits.Height - patternSet.SizeInUnits.Height &&
                  c == SizeInUnits.Width - patternSet.SizeInUnits.Width)
              {
                element = patternSet.PatternElements[e];
                rx = edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing +
                  element.PatternOffset.X +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                ry = edgesSizeInRenderedImageTopLeft.Height +
                  (r - edgesSizeInPatternUnitsTopLeft.Height) *
                  chainmaillePattern.VerticalSpacing +
                  element.PatternOffset.Y +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              }
            }
          }
          if (useLeftRightEdgesIfAvailable)
          {
            if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Left))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Edge(
                EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Left);
              if (c == 0 && r >= edgesSizeInPatternUnitsTopLeft.Height &&
                  r < sizeInUnits.Height -
                    edgesSizeInPatternUnitsBottomRight.Height)
              {
                element = patternSet.PatternElements[e];
                rx = edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing +
                  element.PatternOffset.X +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                ry = edgesSizeInRenderedImageTopLeft.Height +
                  (r - edgesSizeInPatternUnitsTopLeft.Height) *
                  chainmaillePattern.VerticalSpacing +
                  element.PatternOffset.Y +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              }
            }
            if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Right))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Edge(
                EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Right);
              if (c == sizeInUnits.Width - patternSet.SizeInUnits.Width &&
                  r >= edgesSizeInPatternUnitsTopLeft.Height &&
                  r < sizeInUnits.Height -
                    edgesSizeInPatternUnitsBottomRight.Height)
              {
                element = patternSet.PatternElements[e];
                rx = edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft .Width) *
                  chainmaillePattern.HorizontalSpacing +
                  element.PatternOffset.X +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                ry = edgesSizeInRenderedImageTopLeft.Height +
                  (r - edgesSizeInPatternUnitsTopLeft.Height) *
                  chainmaillePattern.VerticalSpacing +
                  element.PatternOffset.Y +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              }
            }
          }
          if (useTopBottomEdgesIfAvailable)
          {
            if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Top))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Edge(
                EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Top);
              if (r == 0 && c >= edgesSizeInPatternUnitsTopLeft.Width &&
                  c < sizeInUnits.Width -
                    edgesSizeInPatternUnitsBottomRight.Width)
              {
                element = patternSet.PatternElements[e];
                rx = edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing +
                  element.PatternOffset.X;
                ry = element.PatternOffset.Y;
              }
            }
            if (chainmaillePattern.HasEdge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Bottom))
            {
              ChainmaillePatternSet patternSet = chainmaillePattern.Edge(
                EdgeGeometryEnum.Rectangular, EdgeOrientationEnum.Bottom);
              if (r == sizeInUnits.Height - patternSet.SizeInUnits.Height &&
                  c >= edgesSizeInPatternUnitsTopLeft.Width &&
                  c < sizeInUnits.Width -
                    edgesSizeInPatternUnitsBottomRight.Width)
              {
                element = patternSet.PatternElements[e];
                rx = edgesSizeInRenderedImageTopLeft.Width +
                  (c - edgesSizeInPatternUnitsTopLeft.Width) *
                  chainmaillePattern.HorizontalSpacing +
                  element.PatternOffset.X +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0);
                ry = edgesSizeInRenderedImageTopLeft.Height +
                  (r - edgesSizeInPatternUnitsTopLeft.Height) *
                  chainmaillePattern.VerticalSpacing +
                  element.PatternOffset.Y +
                  (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0);
              }
            }
          }
        }

        if (element == null)
        {
          element = chainmaillePattern.PatternElements[e];
          rx = edgesSizeInRenderedImageTopLeft.Width +
            (c - edgesSizeInPatternUnitsTopLeft.Width) *
            chainmaillePattern.HorizontalSpacing +
            element.PatternOffset.X +
            (r % 2 == 1 ? chainmaillePattern.EvenRowHorizontalOffset : 0) +
            renderingMarginTopLeft.Width;
          ry = edgesSizeInRenderedImageTopLeft.Height +
            (r - edgesSizeInPatternUnitsTopLeft.Height) *
            chainmaillePattern.VerticalSpacing +
            element.PatternOffset.Y +
            (r % 2 == 1 ? chainmaillePattern.EvenRowVerticalOffset : 0) +
            renderingMarginTopLeft.Height;
        }

        Rectangle destinationRectangle = new Rectangle(rx, ry,
          element.Image.Width, element.Image.Height);
        Graphics g = Graphics.FromImage(renderedImage);
        g.DrawImage(element.Image, destinationRectangle, 0, 0,
          element.Image.Width, element.Image.Height,
          GraphicsUnit.Pixel, ColorMapping(elementColor));
        // Check for whether the element is wrapped. If so, also draw the
        // wrapped portion of the element.
        if (wrap == WrapEnum.Horizontal)
        {
          if (destinationRectangle.X < 0)
          {
            destinationRectangle.X += renderedImage.Width;
            g.DrawImage(element.Image, destinationRectangle, 0, 0,
              element.Image.Width, element.Image.Height,
              GraphicsUnit.Pixel, ColorMapping(elementColor));
          }
          else if (destinationRectangle.X + destinationRectangle.Width >
              renderedImage.Width)
          {
            destinationRectangle.X -= renderedImage.Width;
            g.DrawImage(element.Image, destinationRectangle, 0, 0,
              element.Image.Width, element.Image.Height,
              GraphicsUnit.Pixel, ColorMapping(elementColor));
          }
        }
        else if (wrap == WrapEnum.Vertical)
        {
          if (destinationRectangle.Y < 0)
          {
            destinationRectangle.Y += renderedImage.Height;
            g.DrawImage(element.Image, destinationRectangle, 0, 0,
              element.Image.Width, element.Image.Height,
              GraphicsUnit.Pixel, ColorMapping(elementColor));
          }
          else if (destinationRectangle.Y + destinationRectangle.Height >
                   renderedImage.Height)
          {
            destinationRectangle.Y -= renderedImage.Height;
            g.DrawImage(element.Image, destinationRectangle, 0, 0,
              element.Image.Width, element.Image.Height,
              GraphicsUnit.Pixel, ColorMapping(elementColor));
          }
        }

        g.Dispose();
      }
    }

    public void ReplaceColorsInDesign(
      Dictionary<Color, Color> colorReplacements, string ringFilter, bool calledFromCommandHistory = false)
    {
      if (colorReplacements.Count > 0 && colorImage != null)
      {
        if (!calledFromCommandHistory)
        {
          var SaveAction = new ActionReplaceColor(this, colorReplacements, ringFilter);
          CommandHistory.Executed(SaveAction);
        }

        // Build the color map.
        ImageAttributes imageAttributes = new ImageAttributes();
        ColorMap[] colorMap = new ColorMap[colorReplacements.Count];
        int colorMapIndex = 0;
        foreach (Color designColor in colorReplacements.Keys)
        {
          colorMap[colorMapIndex] = new ColorMap();
          colorMap[colorMapIndex].OldColor = designColor;
          colorMap[colorMapIndex].NewColor = colorReplacements[designColor];
          colorMapIndex++;
        }
        imageAttributes.SetRemapTable(colorMap);

        if (string.IsNullOrEmpty(ringFilter))
        {
          // Apply the color map to the color image.
          Bitmap oldColorImage = colorImage.BitmapImage;
          Bitmap newColorImage = new Bitmap(oldColorImage.Width, oldColorImage.Height);
          Graphics g = Graphics.FromImage(newColorImage);
          g.Clear(Properties.Settings.Default.UnspecifiedElementColor);
          g.DrawImage(oldColorImage,
            new Rectangle(new Point(0, 0), newColorImage.Size),
            0, 0, oldColorImage.Width, oldColorImage.Height,
            GraphicsUnit.Pixel, imageAttributes);
          g.Dispose();

          // Apply the new color image.
          colorImage.BitmapImage = newColorImage;
          RenderImage();
        }
        else
        {
          // We selectively apply the color map, based on attributes of the
          // corresponding rings. We'll utilize the rendering logic, since it
          // already maps pattern elements (i.e. rings) to colors.
          RenderImage(false, colorReplacements, ringFilter, true);
        }

        hasBeenChanged = true;
      }
    }

    public void RotateDesign(float degreesToRotate,
      bool suppressChange = false)
    {
      if (renderedImage != null)
      {
        displayRotationDegrees = MathUtils.NormalizeDegrees(
          displayRotationDegrees + degreesToRotate);

        // Apply the rotation about the center of the rendered image.

        // Start with a new transformation matrix.
        Matrix rotationMatrix = new Matrix();
        // Translate to the center of the rendered image.
        rotationMatrix.Translate(-0.5F * renderedImage.Width,
          -0.5F * renderedImage.Height, MatrixOrder.Append);
        // Apply the rotation.
        rotationMatrix.Rotate(displayRotationDegrees, MatrixOrder.Append);
        // Calculate where to put the origin in the rotated image.
        PointF[] cornerArray = new PointF[]
        {
          new PointF(0, 0),
          new PointF(renderedImage.Width, renderedImage.Height)
        };
        rotationMatrix.TransformPoints(cornerArray);
        float xOffset = -Math.Min(cornerArray[0].X, cornerArray[1].X);
        float yOffset = -Math.Min(cornerArray[0].Y, cornerArray[1].Y);
        // Translate back to the new origin.
        rotationMatrix.Translate(xOffset, yOffset, MatrixOrder.Append);
        renderedImageToDisplayTransformation = rotationMatrix;
        // Compute the inverse transformation.
        displayToRenderedImageTransformation =
          renderedImageToDisplayTransformation.Clone();
        displayToRenderedImageTransformation.Invert();

        if (!suppressChange)
        {
          hasBeenChanged = true;
        }
      }
    }

    public void RotateOverlay(float degreesToRotate,
      bool suppressChange = false)
    {
      if (overlayImage != null)
      {
        overlayRotationDegrees = MathUtils.NormalizeDegrees(
          overlayRotationDegrees + degreesToRotate);

        RecomputeOverlayTransformation();

        if (!suppressChange)
        {
          hasBeenChanged = true;
        }
      }
    }

    private void RecomputeOverlayTransformation()
    {
      if (overlayImage != null)
      {
        // Scale the overlay from the center of the overlay image.
        // Apply the rotation about the center of the overlay image.
        // Re-reference the rotated image to the rendered image.

        // Start with a new transformation matrix.
        Matrix transformationMatrix = new Matrix();
        // Translate to the center of the overlay image.
        transformationMatrix.Translate(-0.5F * overlayImage.Width,
          -0.5F * overlayImage.Height, MatrixOrder.Append);
        // Scale the overlay image from its center.
        transformationMatrix.Scale(overlayXScaleFactor,
          overlayYScaleFactor, MatrixOrder.Append);
        // Apply the rotation about the image center.
        transformationMatrix.Rotate(overlayRotationDegrees,
          MatrixOrder.Append);
        // Translate back to the origin of the *rendered* image.
        transformationMatrix.Translate(overlayCenterInRenderedImage.X,
          overlayCenterInRenderedImage.Y, MatrixOrder.Append);
        overlayToRenderedImageTransformation = transformationMatrix;
        // Compute the inverse transformation.
        renderedToOverlayImageTransformation =
          overlayToRenderedImageTransformation.Clone();
        renderedToOverlayImageTransformation.Invert();
      }
    }

    public Matrix OverlayToRenderedImageTransformation
    {
      get { return overlayToRenderedImageTransformation; }
    }

    public Matrix RenderedToOverlayImageTransformation
    {
      get { return renderedToOverlayImageTransformation; }
    }

    public void Save()
    {
      if (!string.IsNullOrEmpty(designFile))
      {
        try
        {
          // If the design file was not XML (i.e. it might just have been a
          // color map from IGP) change the design file to have the xml
          // extension.
          string extension = Path.GetExtension(designFile);
          if (Path.GetExtension(designFile) != ".xml")
          {
            designFile = Path.ChangeExtension(designFile, ".xml");
          }

          // Ensure the directory exists, creating it if necessary.
          string directoryName = Path.GetDirectoryName(designFile);
          if (!string.IsNullOrEmpty(directoryName))
          {
            // This does nothing if the directory already exists, and can
            // create an entire heirarchy if necessary.
            Directory.CreateDirectory(directoryName);
          }

          // Save the design into the design file,
          // color image into its file, etc.

          XmlDocument doc = new XmlDocument();
          doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

          XmlNode designNode = doc.CreateElement("ChainmailleDesign");
          doc.AppendChild(designNode);

          XmlAttribute designNameAttribute = doc.CreateAttribute("name");
          designNameAttribute.Value = designName;
          designNode.Attributes.Append(designNameAttribute);

          XmlAttribute fileVersionAttribute = doc.CreateAttribute("version");
          fileVersionAttribute.Value = designFileVersion;
          designNode.Attributes.Append(fileVersionAttribute);

          // Information about the design.
          XmlNode designedByNode = doc.CreateElement("DesignedBy");
          designNode.AppendChild(designedByNode);
          designedByNode.InnerText = designedBy;
          XmlNode dateNode = doc.CreateElement("Date");
          designNode.AppendChild(dateNode);
          dateNode.InnerText = designDate.ToString();
          XmlNode designedForNode = doc.CreateElement("DesignedFor");
          designNode.AppendChild(designedForNode);
          designedForNode.InnerText = designedFor;
          XmlNode descriptionNode = doc.CreateElement("Description");
          designNode.AppendChild(descriptionNode);
          descriptionNode.InnerText = designDescription;

          // Palette.
          XmlNode paletteNode = doc.CreateElement("Palette");
          designNode.AppendChild(paletteNode);
          XmlAttribute paletteNameAttribute = doc.CreateAttribute("name");
          paletteNameAttribute.Value = paletteName;
          paletteNode.Attributes.Append(paletteNameAttribute);
          XmlAttribute paletteFileAttribute = doc.CreateAttribute("file");
          paletteFileAttribute.Value = paletteFile;
          paletteNode.Attributes.Append(paletteFileAttribute);
          foreach (string hiddenSectionName in hiddenPaletteSectionNames)
          {
            XmlNode hiddenSectionNode = doc.CreateElement("HiddenSection");
            paletteNode.AppendChild(hiddenSectionNode);
            XmlAttribute hiddenSectionNameAttribute =
              doc.CreateAttribute("name");
            hiddenSectionNameAttribute.Value = hiddenSectionName;
            hiddenSectionNode.Attributes.Append(hiddenSectionNameAttribute);
          }

          // Display rotation.
          float[] transformationElements =
            renderedImageToDisplayTransformation.Elements;
          XmlNode rotationNode = doc.CreateElement("DisplayRotation");
          designNode.AppendChild(rotationNode);
          XmlAttribute degreesAttribute = doc.CreateAttribute("degrees");
          degreesAttribute.Value = displayRotationDegrees.ToString();
          rotationNode.Attributes.Append(degreesAttribute);

          // For now, there is only one design section. Later there might be
          // more, e.g. one for each panel of a mantle.
          XmlNode designSectionNode = doc.CreateElement("DesignSection");
          designNode.AppendChild(designSectionNode);

          // The chainmail pattern.
          XmlNode patternNode = doc.CreateElement("ChainmaillePattern");
          designSectionNode.AppendChild(patternNode);

          // For now, name and file are the same. Later they will be different.
          XmlAttribute patternNameAttribute = doc.CreateAttribute("name");
          patternNameAttribute.Value =
            Path.GetFileNameWithoutExtension(patternFile);
          patternNode.Attributes.Append(patternNameAttribute);

          XmlAttribute patternFileAttribute = doc.CreateAttribute("file");
          patternFileAttribute.Value = patternFile;
          patternNode.Attributes.Append(patternFileAttribute);

          // The size of the design section.
          XmlNode designSizeNode = doc.CreateElement("DesignSize");
          designSectionNode.AppendChild(designSizeNode);

          // For now, rows and columns are from the size in units.
          // Later, a unit may encompass more than one row or column.
          XmlAttribute designRowsAttribute = doc.CreateAttribute("rows");
          designRowsAttribute.Value = sizeInUnits.Height.ToString();
          designSizeNode.Attributes.Append(designRowsAttribute);

          XmlAttribute designColumnsAttribute = doc.CreateAttribute("columns");
          designColumnsAttribute.Value = sizeInUnits.Width.ToString();
          designSizeNode.Attributes.Append(designColumnsAttribute);

          // Wrap.
          XmlAttribute designWrapAttribute = doc.CreateAttribute("wrap");
          designWrapAttribute.Value = wrap.GetDescription();
          designSizeNode.Attributes.Append(designWrapAttribute);

          // Scale.
          if (scale != null)
          {
            scale.WriteToXml(doc, designSectionNode);
          }

          // Save the color image into a file.
          string colorImageFile = Path.ChangeExtension(designFile, ".png");
          FileStream colorImageStream = new FileStream(colorImageFile,
            FileMode.OpenOrCreate);
          colorImage.BitmapImage.Save(colorImageStream, ImageFormat.Png);
          colorImageStream.Flush();
          colorImageStream.Dispose();

          // Record the color image file in the design.
          XmlNode colorImageNode = doc.CreateElement("ColorImage");
          designSectionNode.AppendChild(colorImageNode);

          XmlAttribute colorMapNameAttribute = doc.CreateAttribute("name");
          colorMapNameAttribute.Value =
            Path.GetFileNameWithoutExtension(colorImageFile);
          colorImageNode.Attributes.Append(colorMapNameAttribute);

          XmlAttribute colorMapFileAttribute = doc.CreateAttribute("file");
          colorMapFileAttribute.Value = colorImageFile;
          colorImageNode.Attributes.Append(colorMapFileAttribute);

          // Overlay
          if (!string.IsNullOrEmpty(overlayFile))
          {
            // Overlay file.
            XmlNode overlayNode = doc.CreateElement("Overlay");
            designSectionNode.AppendChild(overlayNode);

            XmlAttribute overlayNameAttribute = doc.CreateAttribute("name");
            overlayNameAttribute.Value =
              Path.GetFileNameWithoutExtension(overlayFile);
            overlayNode.Attributes.Append(overlayNameAttribute);

            XmlAttribute overlayFileAttribute = doc.CreateAttribute("file");
            overlayFileAttribute.Value = overlayFile;
            overlayNode.Attributes.Append(overlayFileAttribute);

            XmlAttribute overlayShownAttribute = doc.CreateAttribute("shown");
            overlayShownAttribute.Value = overlayIsShowing ? "true" : "false";
            overlayNode.Attributes.Append(overlayShownAttribute);

            // Transparency.
            XmlNode transparencyNode = doc.CreateElement("Transparency");
            overlayNode.AppendChild(transparencyNode);

            XmlAttribute transparencyValueAttribute =
              doc.CreateAttribute("value");
            transparencyValueAttribute.Value = overlayTransparency.ToString();
            transparencyNode.Attributes.Append(transparencyValueAttribute);

            // Center.
            XmlNode centerNode = doc.CreateElement("Center");
            overlayNode.AppendChild(centerNode);

            XmlAttribute centerXAttribute = doc.CreateAttribute("x");
            centerXAttribute.Value = overlayCenterInRenderedImage.X.ToString();
            centerNode.Attributes.Append(centerXAttribute);

            XmlAttribute centerYAttribute = doc.CreateAttribute("y");
            centerYAttribute.Value = overlayCenterInRenderedImage.Y.ToString();
            centerNode.Attributes.Append(centerYAttribute);

            XmlAttribute referenceAttribute = doc.CreateAttribute("reference");
            referenceAttribute.Value = "rendered image";
            centerNode.Attributes.Append(referenceAttribute);

            // Scale.
            XmlNode scaleNode = doc.CreateElement("Scale");
            overlayNode.AppendChild(scaleNode);

            XmlAttribute scaleXAttribute = doc.CreateAttribute("x");
            scaleXAttribute.Value = overlayXScaleFactor.ToString();
            scaleNode.Attributes.Append(scaleXAttribute);

            XmlAttribute scaleYAttribute = doc.CreateAttribute("y");
            scaleYAttribute.Value = overlayYScaleFactor.ToString();
            scaleNode.Attributes.Append(scaleYAttribute);

            referenceAttribute = doc.CreateAttribute("reference");
            referenceAttribute.Value = "rendered image";
            scaleNode.Attributes.Append(referenceAttribute);

            // Rotation.
            XmlNode overlayRotationNode = doc.CreateElement("Rotation");
            overlayNode.AppendChild(overlayRotationNode);

            XmlAttribute overlayRotationAttribute =
              doc.CreateAttribute("degrees");
            overlayRotationAttribute.Value = overlayRotationDegrees.ToString();
            overlayRotationNode.Attributes.Append(overlayRotationAttribute);
          }

          if (horizontalGuidelines.Count > 0 || verticalGuidelines.Count > 0)
          {
            // Guidelines.
            XmlNode guidelinesNode = doc.CreateElement("Guidelines");
            designNode.AppendChild(guidelinesNode);
            XmlAttribute guidelinesShownAttribute =
              doc.CreateAttribute("shown");
            guidelinesShownAttribute.Value =
              guidelinesAreShowing ? "true" : "false";
            guidelinesNode.Attributes.Append(guidelinesShownAttribute);
            foreach (int horizontalGuideline in horizontalGuidelines)
            {
              XmlNode guidelineNode = doc.CreateElement("HorizontalGuideline");
              guidelinesNode.AppendChild(guidelineNode);
              XmlAttribute guidelinePositionAttribute =
                doc.CreateAttribute("position");
              guidelinePositionAttribute.Value =
                horizontalGuideline.ToString();
              guidelineNode.Attributes.Append(guidelinePositionAttribute);
            }
            foreach (int verticalGuideline in verticalGuidelines)
            {
              XmlNode guidelineNode = doc.CreateElement("VerticalGuideline");
              guidelinesNode.AppendChild(guidelineNode);
              XmlAttribute guidelinePositionAttribute =
                doc.CreateAttribute("position");
              guidelinePositionAttribute.Value = verticalGuideline.ToString();
              guidelineNode.Attributes.Append(guidelinePositionAttribute);
            }
          }

          XmlTextWriter writer = new XmlTextWriter(designFile, Encoding.UTF8);
          writer.Formatting = Formatting.Indented;
          doc.Save(writer);
          writer.Dispose();

          try
          {
            XmlDocument rdoc = new XmlDocument();
            rdoc.LoadXml(File.ReadAllText(designFile));
          }
          catch
          {
            try
            {
              // Sometimes the final closing tag gets messed up on file output.
              // The document is fine, but either the writer or stream is
              // messed up.  For now, just try again.
              writer = new XmlTextWriter(designFile, Encoding.UTF8);
              writer.Formatting = Formatting.Indented;
              doc.WriteContentTo(writer);
              writer.Dispose();

              XmlDocument rdoc = new XmlDocument();
              rdoc.LoadXml(File.ReadAllText(designFile));
            }
            catch
            {

            }
          }


          hasBeenChanged = false;
        }
        catch (Exception ex)
        {
        }
      }
    }

    public void SaveRenderedImage(string renderedImageFile)
    {
      if (renderedImage != null)
      {
        if (renderedImageToDisplayTransformation.IsIdentity)
        {
          // No transformation is in effect. Save the rendered image.
          renderedImage.Save(renderedImageFile);
        }
        else
        {
          // Apply display transformation before saving.
          Bitmap transformedRenderedImage = TransformedRenderedImage;
          // Save the transformed image.
          transformedRenderedImage.Save(renderedImageFile);
          // Dispose of the transformed image.
          transformedRenderedImage.Dispose();
        }
      }
    }

    public PatternScale Scale
    {
      get { return scale; }
      set { scale = value; }
    }

    public void SetElementColor(ChainmaillePatternElementId elementId,
      Color color, ChainmaillePatternElement referencedElement, bool calledFromCommandHistory=false)
    {
      Point? cPoint = ElementToPointInColorImage(elementId, referencedElement);
      if (cPoint.HasValue)
      {
        var OldColor = colorImage.BitmapImage.GetPixel(cPoint.Value.X, cPoint.Value.Y);
        if (color.ToArgb() == OldColor.ToArgb()) { return; } // If the point hasn't changed, exit

        if (!calledFromCommandHistory)
        {
          var SaveAction = new ActionRingColorChange(this, elementId, color, referencedElement, OldColor);
          CommandHistory.Executed(SaveAction);
        }

        colorImage.BitmapImage.SetPixel(cPoint.Value.X, cPoint.Value.Y, color);
        hasBeenChanged = true;
      }
    }

    public bool ShowGuidelines
    {
      get { return guidelinesAreShowing; }
      set
      {
        if (value != guidelinesAreShowing)
        {
          guidelinesAreShowing = value;
          hasBeenChanged = true;
        }
      }
    }

    public bool ShowOverlay
    {
      get { return overlayIsShowing; }
      set
      {
        if (value != overlayIsShowing)
        {
          overlayIsShowing = value;
          hasBeenChanged = true;
        }
      }
    }

    public Size SizeInUnits
    {
      get { return sizeInUnits; }
      set
      {
        if (sizeInUnits.Width != value.Width ||
            sizeInUnits.Height != value.Height)
        {
          sizeInUnits = value;
          hasBeenChanged = true;
        }
      }
    }

    private bool TestPointAgainstBoundingBoxOfPatternSet(
      int rxBase, int ryBase, int x, int y, int aX, int aY,
      ChainmaillePatternSet patternSet)
    {
      Rectangle boundingBox = new Rectangle(patternSet.BoundingBox.Location,
        patternSet.BoundingBox.Size);
      boundingBox.X += rxBase;
      boundingBox.Y += ryBase;
      return boundingBox.Contains(x, y) || boundingBox.Contains(aX, aY);
    }

    private bool TestPointAgainstElementsOfPatternSet(int r, int c,
      int x, int y, int rxBase, int ryBase, ChainmaillePatternSet patternSet,
      ref ChainmaillePatternElementId elementId,
      ref ChainmaillePatternElement referencedElement)
    {
      bool result = false;

      int ex, ey;
      for (int e = 0; e < patternSet.PatternElements.Count &&
        elementId == null; e++)
      {
        ChainmaillePatternElement element =
          patternSet.PatternElements[e];
        ex = x - rxBase - element.PatternOffset.X;
        ey = y - ryBase - element.PatternOffset.Y;
        if (ex >= 0 && ex < element.Image.Width &&
            ey >= 0 && ey < element.Image.Height)
        {
          // Strangely, testing the pixel for equality with
          // Color.Black fails even when the pixel is Color.Black,
          // so here we test the color componenets, except that we
          // don't care about alpha.
          Color pixel = element.Image.GetPixel(ex, ey);
          if (pixel.R == Color.Black.R &&
              pixel.G == Color.Black.G &&
              pixel.B == Color.Black.B)
          {
            // As a final test, check whether there is a
            // corresponding pixel in the color image. If a
            // row or column ends with only part of a pattern,
            // some elements will not be part of the design.
            ChainmaillePatternElementId candidateElement =
              new ChainmaillePatternElementId()
              {
                Column = c + 1,
                Row = r + 1,
                ElementIndex = e + 1
              };
            if (ElementIsInColorImage(candidateElement, element))
            {
              elementId = candidateElement;
              referencedElement = element;
            }
          }
        }
      }

      return result;
    }

    public PointF TransformDisplayPointFToRenderedImage(PointF point)
    {
      PointF[] pointArray = new PointF[] { point };
      displayToRenderedImageTransformation.TransformPoints(pointArray);
      return pointArray[0];
    }

    public PointF TransformDisplayPointFToOverlayImage(PointF point)
    {
      PointF[] pointArray = new PointF[] { point };
      displayToRenderedImageTransformation.TransformPoints(pointArray);
      renderedToOverlayImageTransformation.TransformPoints(pointArray);
      return pointArray[0];
    }

    public PointF TransformOverlayImagePointFToRenderedImage(PointF point)
    {
      PointF[] pointArray = new PointF[] { point };
      overlayToRenderedImageTransformation.TransformPoints(pointArray);
      return pointArray[0];
    }

    public PointF TransformRenderedImagePointFToDisplay(PointF point)
    {
      PointF[] pointArray = new PointF[] { point };
      renderedImageToDisplayTransformation.TransformPoints(pointArray);
      return pointArray[0];
    }

    private ImageAttributes TransparencyMapping(float transparency)
    {
      ColorMatrix colorMatrix = new ColorMatrix();
      // Opacity is element 3,3 (4th row, 4th column) of the color
      // transformation matrix.
      colorMatrix.Matrix33 = 1F - transparency;
      ImageAttributes imageAttributes = new ImageAttributes();
      imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
        ColorAdjustType.Bitmap);

      return imageAttributes;
    }

    public SortedSet<int> VerticalGuidelines
    {
      get { return verticalGuidelines; }
    }

    public string WeaveName
    {
      get { return weaveName; }
      set
      {
        if (weaveName != value)
        {
          chainmaillePattern = new ChainmaillePattern(null, value);
          hasBeenChanged = true;
        }
      }
    }

    public WrapEnum Wrap
    {
      get { return wrap; }
      set
      {
        wrap = value;
        DetermineEdgeAllowances();
      }
    }

  }
}
