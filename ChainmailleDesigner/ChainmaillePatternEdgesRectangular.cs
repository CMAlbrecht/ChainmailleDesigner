// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmaillePatternEdgesRectangular.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System.Xml;

namespace ChainmailleDesigner
{
  public class ChainmaillePatternEdgesRectangular : ChainmaillePatternEdges
  {
    public ChainmaillePatternEdgesRectangular()
    {
      geometry = EdgeGeometryEnum.Rectangular;
    }


    public ChainmaillePatternEdgesRectangular(XmlNode topNode,
      string patternFolder)
      : this()
    {
      XmlNodeList edgeNodes = topNode.SelectNodes("Edge");
      foreach (XmlNode edgeNode in edgeNodes)
      {
        XmlAttribute orientationAttribute =
          edgeNode.Attributes["orientation"];
        if (orientationAttribute != null)
        {
          EdgeOrientationEnum? edgeOrientation =
            EnumUtils.ToNullableEnumFromDescription<EdgeOrientationEnum>(
              orientationAttribute.Value);
          if (edgeOrientation.HasValue && ChainmailleDesignerConstants.
                rectangularEdgeOrientations.Contains(edgeOrientation.Value))
          {
            edgePatternSets.Add(edgeOrientation.Value,
              new ChainmaillePatternSet(edgeNode, patternFolder));
          }
        }
      }

      XmlNode cornersNode = topNode.SelectSingleNode("Corners");
      if (cornersNode != null)
      {
        XmlNodeList cornerNodes = cornersNode.SelectNodes("Corner");
        foreach (XmlNode cornerNode in cornerNodes)
        {
          XmlAttribute orientationAttribute =
            cornerNode.Attributes["orientation"];
          if (orientationAttribute != null)
          {
            CornerOrientationEnum? cornerOrientation =
              EnumUtils.ToNullableEnumFromDescription<CornerOrientationEnum>(
                orientationAttribute.Value);
            if (cornerOrientation.HasValue && ChainmailleDesignerConstants.
                  rectangularCornerOrientations.Contains(
                  cornerOrientation.Value))
            {
              cornerPatternSets.Add(cornerOrientation.Value,
                new ChainmaillePatternSet(cornerNode, patternFolder));
            }
          }
        }
      }
    }

  }
}
