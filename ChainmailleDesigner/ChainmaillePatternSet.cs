// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmaillePatternSet.cs


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
using System.Xml;

namespace ChainmailleDesigner
{
  /// <summary>
  /// A chainmaille pattern set holds the pattern information for a region of
  /// the pattern. Many patterns will have only one pattern set (the interior
  /// of the pattern), but edges require more. An instance of rectangular edges
  /// for a sheet, for example, typically requires one pattern set for each
  /// edge (top, bottom, left, and right) and one pattern set for each corner.
  /// </summary>
  public class ChainmaillePatternSet : IDisposable
  {
    // Image of just the ring outlines, black on white background.
    private Bitmap outlineImage = null;
    // The individual ring elements.
    private List<ChainmaillePatternElement> patternElements =
      new List<ChainmaillePatternElement>();
    // Bounding box which encompasses all element images.
    private Rectangle boundingBox = new Rectangle(0, 0, 1, 1);
    // Extent of the pattern set in pattern units.
    private Size sizeInUnits = new Size(1, 1);
    // Extent of the pattern set in visual rows and columns.
    private Size visualSize = new Size(1, 1);
    // Physical extent (as distance) in pattern units.
    private SizeF unitExtent = new SizeF(1, 1);
    // Where to start drawing the next pattern set to the right of or below
    // this one.
    private Size renderingSpacing = new Size(0, 0);
    // Whether build offsets were specified for the elements.
    private bool hasBuildOffsets = false;
    // Whether ring size names were specified for the elements.
    private bool hasRingSizeNames = false;

    public ChainmaillePatternSet(Bitmap outline,
      List<ChainmaillePatternElement> elements, Size size, Size spacing)
    {
      outlineImage = outline;
      patternElements = elements;
      sizeInUnits = size;
      renderingSpacing = spacing;

      ComputeBoundingBox();
    }

    public ChainmaillePatternSet(XmlNode topNode, string patternFolder)
    {
      XmlAttribute vUnitsAttribute = topNode.Attributes["verticalUnits"];
      if (vUnitsAttribute != null)
      {
        int vUnits;
        if (int.TryParse(vUnitsAttribute.Value, out vUnits))
        {
          sizeInUnits.Height = vUnits;
        }
      }

      XmlAttribute hUnitsAttribute = topNode.Attributes["horizontalUnits"];
      if (hUnitsAttribute != null)
      {
        int hUnits;
        if (int.TryParse(hUnitsAttribute.Value, out hUnits))
        {
          sizeInUnits.Width = hUnits;
        }
      }

      XmlAttribute rowsAttribute = topNode.Attributes["rows"];
      if (rowsAttribute != null)
      {
        int rows;
        if (int.TryParse(rowsAttribute.Value, out rows))
        {
          visualSize.Height = rows;
        }
      }

      XmlAttribute columnsAttribute = topNode.Attributes["columns"];
      if (columnsAttribute != null)
      {
        int columns;
        if (int.TryParse(columnsAttribute.Value, out columns))
        {
          visualSize.Width = columns;
        }
      }

      unitExtent.Height = sizeInUnits.Height;
      XmlAttribute verticalExtentAttribute =
        topNode.Attributes["verticalUnitsExtent"];
      if (verticalExtentAttribute != null)
      {
        float extent;
        if (float.TryParse(verticalExtentAttribute.Value, out extent))
        {
          unitExtent.Height = extent;
        }
      }

      unitExtent.Width = sizeInUnits.Width;
      XmlAttribute horizontalExtentAttribute =
        topNode.Attributes["horizontalUnitsExtent"];
      if (horizontalExtentAttribute != null)
      {
        float extent;
        if (float.TryParse(horizontalExtentAttribute.Value, out extent))
        {
          unitExtent.Width = extent;
        }
      }

      XmlNode outlineImageNode =
        topNode.SelectSingleNode("OutlineImage");
      if (outlineImageNode != null)
      {
        XmlAttribute fileAttribute = outlineImageNode.Attributes["file"];
        if (fileAttribute != null)
        {
          outlineImage = new Bitmap(patternFolder + '\\' +
            fileAttribute.Value);
          renderingSpacing = new Size(outlineImage.Width, outlineImage.Height);
        }
      }

      XmlNode patternSpacingNode =
        topNode.SelectSingleNode("PatternSpacing");
      if (patternSpacingNode != null)
      {
        XmlAttribute xAttribute = patternSpacingNode.Attributes["x"];
        XmlAttribute yAttribute = patternSpacingNode.Attributes["y"];
        if (xAttribute != null && yAttribute != null)
        {
          int horizontalSpacing = int.Parse(xAttribute.Value);
          int verticalSpacing = int.Parse(yAttribute.Value);
          renderingSpacing = new Size(horizontalSpacing, verticalSpacing);
        }
      }

      XmlNode elementsNode =
        topNode.SelectSingleNode("PatternElements");
      if (elementsNode != null)
      {
        XmlNodeList elementNodes = elementsNode.SelectNodes("PatternElement");
        foreach (XmlNode elementNode in elementNodes)
        {
          XmlAttribute indexAttribute = elementNode.Attributes["index"];
          XmlAttribute ringSizeAttribute = elementNode.Attributes["ringSize"];
          XmlAttribute fileAttribute = elementNode.Attributes["file"];
          if (indexAttribute != null && fileAttribute != null)
          {
            int elementIndex = int.Parse(indexAttribute.Value);
            string elementRingSize = string.Empty;
            if (ringSizeAttribute != null)
            {
              elementRingSize = ringSizeAttribute.Value;
              hasRingSizeNames = true;
            }
            string elementImageFile = patternFolder + '\\' +
              fileAttribute.Value;
            Point elementImageOffset = new Point(0, 0);
            Point elementColorOffset = new Point(0, 0);
            Point elementBuildOffset = new Point(0, 0);

            XmlNode imageOffsetNode =
              elementNode.SelectSingleNode("ImageOffset");
            if (imageOffsetNode != null)
            {
              XmlAttribute xAttribute = imageOffsetNode.Attributes["x"];
              XmlAttribute yAttribute = imageOffsetNode.Attributes["y"];
              if (xAttribute != null && yAttribute != null)
              {
                elementImageOffset.X = int.Parse(xAttribute.Value);
                elementImageOffset.Y = int.Parse(yAttribute.Value);
              }
            }
            XmlNode colorOffsetNode =
              elementNode.SelectSingleNode("ColorOffset");
            if (colorOffsetNode != null)
            {
              XmlAttribute xAttribute = colorOffsetNode.Attributes["x"];
              XmlAttribute yAttribute = colorOffsetNode.Attributes["y"];
              if (xAttribute != null && yAttribute != null)
              {
                elementColorOffset.X = int.Parse(xAttribute.Value);
                elementColorOffset.Y = int.Parse(yAttribute.Value);
              }
            }

            XmlNode buildOffsetNode =
              elementNode.SelectSingleNode("BuildOffset");
            if (buildOffsetNode != null)
            {
              XmlAttribute xAttribute = buildOffsetNode.Attributes["x"];
              XmlAttribute yAttribute = buildOffsetNode.Attributes["y"];
              if (xAttribute != null && yAttribute != null)
              {
                elementBuildOffset.X = int.Parse(xAttribute.Value);
                elementBuildOffset.Y = int.Parse(yAttribute.Value);
                hasBuildOffsets = true;
              }
            }
            patternElements.Add(
              new ChainmaillePatternElement(elementIndex, elementImageFile,
              elementImageOffset, elementColorOffset, elementBuildOffset,
              elementRingSize));
          }
        }

        ComputeBoundingBox();
      }
    }

    public Rectangle BoundingBox
    {
      get { return boundingBox; }
    }

    private void ComputeBoundingBox()
    {
      if (patternElements.Count >= 1)
      {
        int x, y;
        int xMin = patternElements[0].PatternOffset.X;
        int yMin = patternElements[0].PatternOffset.Y;
        int xMax = xMin + patternElements[0].Image.Width;
        int yMax = yMin + patternElements[0].Image.Height;
        for (int e = 1; e < patternElements.Count; e++)
        {
          x = patternElements[e].PatternOffset.X;
          y = patternElements[e].PatternOffset.Y;
          xMin = Math.Min(x, xMin);
          yMin = Math.Min(y, yMin);
          x += patternElements[e].Image.Width;
          y += patternElements[e].Image.Height;
          xMax = Math.Max(x, xMax);
          yMax = Math.Max(y, yMax);
        }
        boundingBox = new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
      }
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
        outlineImage?.Dispose();
      }
    }

    public bool HasBuildOffsets
    {
      get { return hasBuildOffsets; }
    }

    public bool HasRingSizeNames
    {
      get { return hasRingSizeNames; }
    }

    public Bitmap OutlineImage
    {
      get { return outlineImage; }
    }

    public List<ChainmaillePatternElement> PatternElements
    {
      get { return patternElements; }
    }

    public Size RenderingSpacing
    {
      get { return renderingSpacing; }
    }

    public Size SizeInUnits
    {
      get { return sizeInUnits; }
    }

    public SizeF UnitExtent
    {
      get { return unitExtent; }
    }

    public Size VisualSize
    {
      get { return visualSize; }
    }

  }
}
