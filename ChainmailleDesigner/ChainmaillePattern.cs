// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmaillePattern.cs


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
using System.IO;
using System.Xml;

namespace ChainmailleDesigner
{
  public class ChainmaillePattern : IDisposable
  {
    private bool isInitialized = false;
    private string name = string.Empty;
    private string description = string.Empty;
    private string patternDirectory = string.Empty;
    // Image of the entire pattern, suitable for display as an example of the
    // weave.
    private Bitmap patternImage = null;
    // The basic pattern set represents the set of repeating elements that
    // make up the basic pattern (exclusive of special edge treatment).
    ChainmaillePatternSet bodyPatternSet = null;
    private int evenRowHorizontalOffset = 0;
    private int evenRowVerticalOffset = 0;
    // Horizontal and vertical extent of a pattern unit in visual rows and
    // columns of the assembled pattern.
    private Size visualSize = new Size(1, 1);
    // Horizontal and vertical extent of the block of pixels in the
    // color image that correspond to a single instance of the basic pattern.
    // (Some patterns use more than one row of color data, e.g. two rows
    // of 7 pixels each for Japanese 12-in-1.)
    private Size colorSize = new Size(1, 1);
    // Whether the pattern is for a sheet or a chain.
    WeaveLinkageEnum linkage = WeaveLinkageEnum.Sheet;
    // Scales relate pattern units to distance measures.
    List<PatternScale> patternScales = new List<PatternScale>();
    // These classes hold the pattern elements for special edge treatment for
    // various edge geometries (rectangular, hexagonal, etc.)
    Dictionary<EdgeGeometryEnum, ChainmaillePatternEdges> edgePatterns =
      new Dictionary<EdgeGeometryEnum, ChainmaillePatternEdges>();

    public ChainmaillePattern(string patternFile, string weaveName = null)
    {
      name = Path.GetFileNameWithoutExtension(patternFile);
      patternDirectory = patternFile;
      if (Path.GetExtension(patternFile) != string.Empty)
      {
        // We were given an actual file; the folder is one up from that.
        patternDirectory = Path.GetDirectoryName(patternFile);
      }
      if (!Path.IsPathRooted(patternDirectory))
      {
        // We don't have a full path, so append the path to the default
        // pattern directory.
        patternDirectory = Properties.Settings.Default.PatternDirectory +
          '\\' + patternDirectory;
      }
      DirectoryInfo directoryInfo = new DirectoryInfo(patternDirectory);
      if (!directoryInfo.Exists)
      {
        // Try to find the weave by name.
        string alternativeDirectory = FileUtils.FindSimilarDirectory(name,
          new List<string>() { Properties.Settings.Default.PatternDirectory });
        if (!string.IsNullOrEmpty(alternativeDirectory))
        {
          patternDirectory = alternativeDirectory;
        }
      }

      // Newer XML pattern description file.
      string xmlFileName = patternDirectory + '\\' + name + ".xml";
      // Older IGP-style ini file pattern description.
      string iniFileName = patternDirectory + '\\' + name + ".ini";
      if (new FileInfo(xmlFileName).Exists)
      {
        InitializeFromXmlFile(xmlFileName, patternDirectory);
      }
      else if (new FileInfo(iniFileName).Exists)
      {
        InitializeFromIniFile(iniFileName, patternDirectory);
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
        patternImage?.Dispose();
        bodyPatternSet?.Dispose();
      }
    }

    private void InitializeFromIniFile(string iniFileName,
      string patternFolder)
    {
      IniFile iniFile = new IniFile(iniFileName);

      description = iniFile.Read("Description", "General");
      string patternImageFile = patternFolder + '\\' +
        iniFile.Read("Filename", "General");
      patternImage = new Bitmap(patternImageFile);
      string outlineImageFile = patternFolder + '\\' +
        iniFile.Read("OutlineFilename", "General");
      Bitmap outlineImage = new Bitmap(outlineImageFile);
      int horizontalSpacing = int.Parse(iniFile.Read("XRepeat", "General"));
      int verticalSpacing = int.Parse(iniFile.Read("YRepeat", "General"));
      Size renderingSpacing = new Size(horizontalSpacing, verticalSpacing);
      evenRowHorizontalOffset = int.Parse(iniFile.Read("NextRowXOffset",
        "General"));
      evenRowVerticalOffset = int.Parse(iniFile.Read("NextRowYOffset",
        "General"));
      if (evenRowVerticalOffset > 0)
      {
        evenRowVerticalOffset -= verticalSpacing;
      }
      Size sizeInUnits = new Size(1, 1);
      colorSize = new Size(
        int.Parse(iniFile.Read("xReadWrite", "General")),
        int.Parse(iniFile.Read("yReadWrite", "General")));

      int nrElements = int.Parse(iniFile.Read("NumSubTiles", "General"));
      List<ChainmaillePatternElement> patternElements =
        new List<ChainmaillePatternElement>();
      for (int elementIndex = 0; elementIndex < nrElements; elementIndex++)
      {
        string sectionName = "Subtile" + (elementIndex + 1).ToString("000");
        string elementImageFile =
          patternFolder + '\\' + iniFile.Read("Filename", sectionName);
        patternElements.Add(
          new ChainmaillePatternElement(elementIndex, elementImageFile,
          new Point(int.Parse(iniFile.Read("PatternXOffset", sectionName)),
          int.Parse(iniFile.Read("PatternYOffset", sectionName))),
          new Point(int.Parse(iniFile.Read("ReadWriteXOffset", sectionName)),
          int.Parse(iniFile.Read("ReadWriteYOffset", sectionName))),
          new Point(elementIndex, 0),  // Very dubious; what to do?
          string.Empty));
      }

      bodyPatternSet = new ChainmaillePatternSet(outlineImage, patternElements,
        sizeInUnits, renderingSpacing);

      isInitialized = true;
    }

    private void InitializeFromXmlFile(string xmlFileName,
      string patternFolder)
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(File.ReadAllText(xmlFileName));

        XmlNode weaveNode = doc.SelectSingleNode("ChainmailleWeave");
        if (weaveNode != null)
        {
          int designFileVersionNr = 1;
          XmlAttribute designFileVersionAttribute =
            weaveNode.Attributes["version"];
          if (designFileVersionAttribute != null)
          {
            designFileVersionNr = int.Parse(designFileVersionAttribute.Value);
          }
          XmlAttribute designNameAttribute = weaveNode.Attributes["name"];
          if (designNameAttribute != null)
          {
            name = designNameAttribute.Value;
          }

          XmlNode patternImageNode =
          weaveNode.SelectSingleNode("PatternImage");
          if (patternImageNode != null)
          {
            XmlAttribute fileAttribute = patternImageNode.Attributes["file"];
            if (fileAttribute != null)
            {
              patternImage = new Bitmap(patternFolder + '\\' +
                fileAttribute.Value);
            }
          }

          XmlNode visualSizeNode =
            weaveNode.SelectSingleNode("UnitVisualSize");
          if (visualSizeNode != null)
          {
            XmlAttribute columnsAttribute =
              visualSizeNode.Attributes["columns"];
            XmlAttribute rowsAttribute = visualSizeNode.Attributes["rows"];
            if (columnsAttribute != null && rowsAttribute != null)
            {
              visualSize = new Size(int.Parse(columnsAttribute.Value),
                int.Parse(rowsAttribute.Value));
            }
          }

          XmlNode colorSizeNode =
            weaveNode.SelectSingleNode("UnitColorSize");
          if (colorSizeNode != null)
          {
            XmlAttribute xAttribute = colorSizeNode.Attributes["x"];
            XmlAttribute yAttribute = colorSizeNode.Attributes["y"];
            if (xAttribute != null && yAttribute != null)
            {
              colorSize = new Size(int.Parse(xAttribute.Value),
                int.Parse(yAttribute.Value));
            }
          }

          XmlNode evenRowOffsetNode =
            weaveNode.SelectSingleNode("EvenRowOffset");
          if (evenRowOffsetNode != null)
          {
            XmlAttribute xAttribute = evenRowOffsetNode.Attributes["x"];
            XmlAttribute yAttribute = evenRowOffsetNode.Attributes["y"];
            if (xAttribute != null && yAttribute != null)
            {
              evenRowHorizontalOffset = int.Parse(xAttribute.Value);
              evenRowVerticalOffset = int.Parse(yAttribute.Value);
            }
          }

          XmlNode linkageNode =
            weaveNode.SelectSingleNode("Linkage");
          if (linkageNode != null)
          {
            XmlAttribute valueAttribute = linkageNode.Attributes["value"];
            if (valueAttribute != null)
            {
              linkage = EnumUtils.ToEnumFromDescription<WeaveLinkageEnum>(
                valueAttribute.Value);
            }
          }

          XmlNode bodyNode =
            weaveNode.SelectSingleNode("Body");
          if (bodyNode != null)
          {
            bodyPatternSet = new ChainmaillePatternSet(bodyNode, patternFolder);
          }

          XmlNodeList edgesNodes = weaveNode.SelectNodes("Edges");
          foreach (XmlNode edgesNode in edgesNodes)
          {
            XmlAttribute geometryAttribute =
              edgesNode.Attributes["geometry"];
            if (geometryAttribute != null)
            {
              if (geometryAttribute.Value == "rectangular")
              {
                edgePatterns.Add(EdgeGeometryEnum.Rectangular,
                  new ChainmaillePatternEdgesRectangular(edgesNode,
                  patternFolder));
              }
            }
          }

          XmlNode scalesNode =
            weaveNode.SelectSingleNode("Scales");
          if (scalesNode != null)
          {
            XmlNodeList scaleNodes = scalesNode.SelectNodes("Scale");
            foreach (XmlNode scaleNode in scaleNodes)
            {
              PatternScale patternScale = new PatternScale(scaleNode);
              patternScales.Add(patternScale);
            }
          }

          isInitialized = true;
        }
      }
      catch (Exception ex)
      {
      }
    }

    public ChainmaillePatternSet BasicPatternSet
    {
      get { return bodyPatternSet; }
    }

    public Rectangle BoundingBox
    {
      get { return bodyPatternSet.BoundingBox; }
    }

    public Size ColorSize
    {
      get { return colorSize; }
    }

    public SizeAdjustment ConvertAdjustmentUnits(SizeAdjustment adjustment,
      DesignSizeUnitsEnum toUnits, PatternScale scale)
    {
      SizeAdjustment result = new SizeAdjustment(adjustment);

      if (toUnits != adjustment.Units)
      {
        SizeF fromFactor = SizeFactor(adjustment.Units, scale);
        SizeF toFactor = SizeFactor(toUnits, scale);

        result.UpperLeftAdjustment.Width *=
          toFactor.Width / fromFactor.Width;
        result.UpperLeftAdjustment.Height *=
          toFactor.Height / fromFactor.Height;
        result.LowerRightAdjustment.Width *=
          toFactor.Width / fromFactor.Width;
        result.LowerRightAdjustment.Height *=
          toFactor.Height / fromFactor.Height;

        result.Units = toUnits;
      }

      return result;
    }

    public SizeF ConvertSizeUnits(Size size, DesignSizeUnitsEnum toUnits,
      PatternScale scale, WrapEnum wrap)
    {
      // For now, we assume conversion FROM pattern units into some other
      // units. Later we may want to convert from other units.

      SizeF result = new SizeF(size);

      if (toUnits != DesignSizeUnitsEnum.Units)
      {
        SizeF toFactor = SizeFactor(toUnits, scale);
        Size edgeAllowanceIn = new Size(0, 0);
        SizeF edgeAllowanceOut = new SizeF(0, 0);

        if (toUnits != DesignSizeUnitsEnum.Pixels)
        {
          // Account for edges, if available.
          if (HasEdges(EdgeGeometryEnum.Rectangular))
          {
            if (wrap != WrapEnum.Horizontal)
            {
              if (HasEdge(EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Left))
              {
                ChainmaillePatternSet edge = Edge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Left);
                edgeAllowanceIn.Width += edge.SizeInUnits.Width;
                if (toUnits == DesignSizeUnitsEnum.RowsColumns)
                {
                  edgeAllowanceOut.Width += edge.VisualSize.Width;
                }
                else
                {
                  edgeAllowanceOut.Width += edge.UnitExtent.Width;
                }
              }
              if (HasEdge(EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Right))
              {
                ChainmaillePatternSet edge = Edge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Right);
                edgeAllowanceIn.Width += edge.SizeInUnits.Width;
                if (toUnits == DesignSizeUnitsEnum.RowsColumns)
                {
                  edgeAllowanceOut.Width += edge.VisualSize.Width;
                }
                else
                {
                  edgeAllowanceOut.Width += edge.UnitExtent.Width;
                }
              }
            }
            if (wrap != WrapEnum.Vertical)
            {
              if (HasEdge(EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Top))
              {
                ChainmaillePatternSet edge = Edge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Top);
                edgeAllowanceIn.Height += edge.SizeInUnits.Height;
                if (toUnits == DesignSizeUnitsEnum.RowsColumns)
                {
                  edgeAllowanceOut.Height += edge.VisualSize.Height;
                }
                else
                {
                  edgeAllowanceOut.Height += edge.UnitExtent.Height;
                }
              }
              if (HasEdge(EdgeGeometryEnum.Rectangular,
                    EdgeOrientationEnum.Bottom))
              {
                ChainmaillePatternSet edge = Edge(EdgeGeometryEnum.Rectangular,
                  EdgeOrientationEnum.Bottom);
                edgeAllowanceIn.Height += edge.SizeInUnits.Height;
                if (toUnits == DesignSizeUnitsEnum.RowsColumns)
                {
                  edgeAllowanceOut.Height += edge.VisualSize.Height;
                }
                else
                {
                  edgeAllowanceOut.Height += edge.UnitExtent.Height;
                }
              }
            }
          }
        }

        switch (toUnits)
        {
          case DesignSizeUnitsEnum.Pixels:
            result.Width *= toFactor.Width;
            result.Height *= toFactor.Height;
            break;
          case DesignSizeUnitsEnum.RowsColumns:
            result.Width = (result.Width - edgeAllowanceIn.Width) *
              toFactor.Width + edgeAllowanceOut.Width;
            result.Height = (result.Height - edgeAllowanceIn.Height) *
              toFactor.Height + edgeAllowanceOut.Height;
            break;
          case DesignSizeUnitsEnum.Inches:
          case DesignSizeUnitsEnum.Centimeters:
            result.Width = (result.Width - edgeAllowanceIn.Width +
              edgeAllowanceOut.Width) * toFactor.Width;
            result.Height = (result.Height - edgeAllowanceIn.Height +
              edgeAllowanceOut.Height) * toFactor.Height;
            break;
          default:
            break;
        }
      }

      return result;
    }

    public ChainmaillePatternSet Corner(EdgeGeometryEnum edgeGeometry,
      CornerOrientationEnum cornerOrientation)
    {
      ChainmaillePatternSet result = null;

      if (edgePatterns.ContainsKey(edgeGeometry))
      {
        result = edgePatterns[edgeGeometry].Corner(cornerOrientation);
      }

      return result;
    }

    public ChainmaillePatternSet Edge(EdgeGeometryEnum edgeGeometry,
      EdgeOrientationEnum edgeOrientation)
    {
      ChainmaillePatternSet result = null;

      if (edgePatterns.ContainsKey(edgeGeometry))
      {
        result = edgePatterns[edgeGeometry].Edge(edgeOrientation);
      }

      return result;
    }

    public int EvenRowHorizontalOffset
    {
      get { return evenRowHorizontalOffset; }
    }

    public int EvenRowVerticalOffset
    {
      get { return evenRowVerticalOffset; }
    }

    public bool HasBuildOffsets
    {
      get { return bodyPatternSet.HasBuildOffsets; }
    }

    public bool HasRingSizeNames
    {
      get { return bodyPatternSet.HasRingSizeNames; }
    }

    public bool HasCorner(EdgeGeometryEnum edgeGeometry,
      CornerOrientationEnum cornerOrientation)
    {
      bool result = false;

      if (edgePatterns.ContainsKey(edgeGeometry))
      {
        result = edgePatterns[edgeGeometry].HasCorner(cornerOrientation);
      }

      return result;
    }

    public bool HasEdge(EdgeGeometryEnum edgeGeometry,
      EdgeOrientationEnum edgeOrientation)
    {
      bool result = false;

      if (edgePatterns.ContainsKey(edgeGeometry))
      {
        result = edgePatterns[edgeGeometry].HasEdge(edgeOrientation);
      }

      return result;
    }

    public bool HasEdges(EdgeGeometryEnum edgeGeometry)
    {
      return edgePatterns.ContainsKey(edgeGeometry);
    }

    public bool HasScales
    {
      get { return patternScales.Count > 0; }
    }

    public int HorizontalSpacing
    {
      get { return bodyPatternSet.RenderingSpacing.Width; }
    }

    public bool IsInitialized
    {
      get { return isInitialized; }
    }

    public WeaveLinkageEnum Linkage
    {
      get { return linkage; }
    }

    public Bitmap OutlineImage
    {
      get { return bodyPatternSet.OutlineImage; }
    }

    public string PatternDirectory
    {
      get
      {
        string result = patternDirectory;

        if (result.StartsWith(
              Properties.Settings.Default.PatternDirectory))
        {
          result = result.Substring(
            Properties.Settings.Default.PatternDirectory.Length);
        }

        return result;
      }
    }

    public List<ChainmaillePatternElement> PatternElements
    {
      get { return bodyPatternSet.PatternElements; }
    }

    public PatternScale Scale(string scaleDescription)
    {
      PatternScale result = null;

      foreach (PatternScale patternScale in patternScales)
      {
        if (patternScale.Description == scaleDescription)
        {
          result = patternScale;
          break;
        }
      }

      return result;
    }

    public List<PatternScale> Scales
    {
      get { return patternScales; }
    }

    private SizeF SizeFactor(DesignSizeUnitsEnum units, PatternScale scale)
    {
      float hFactor;
      float vFactor;
      float dFactor = 1.0F; // distance unit conversion

      switch (units)
      {
        case DesignSizeUnitsEnum.Pixels:
          hFactor = colorSize.Width;
          vFactor = colorSize.Height;
          break;
        case DesignSizeUnitsEnum.RowsColumns:
          hFactor = visualSize.Width;
          vFactor = visualSize.Height;
          break;
        case DesignSizeUnitsEnum.Inches:
        case DesignSizeUnitsEnum.Centimeters:
          hFactor = 1.0F / scale.UnitSize.Width;
          vFactor = 1.0F / scale.UnitSize.Height;
          if (units == DesignSizeUnitsEnum.Inches &&
              scale.ReferenceUnits != Units.Inches)
          {
            dFactor = (float)UnitConverter.ConvertValue(1.0,
              scale.ReferenceUnits, Units.Inches);
          }
          else if (units == DesignSizeUnitsEnum.Centimeters &&
              scale.ReferenceUnits != Units.Centimeters)
          {
            dFactor = (float)UnitConverter.ConvertValue(1.0,
              scale.ReferenceUnits, Units.Centimeters);
          }
          break;
        default:
          hFactor = 1;
          vFactor = 1;
          break;
      }

      return new SizeF(hFactor * dFactor, vFactor * dFactor);
    }

    public Size SizeInUnits
    {
      get { return bodyPatternSet.SizeInUnits; }
    }

    public int VerticalSpacing
    {
      get { return bodyPatternSet.RenderingSpacing.Height; }
    }

    public Size VisualSize
    {
      get { return visualSize; }
    }

  }
}
