// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmaillePatternEdges.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System.Collections.Generic;

namespace ChainmailleDesigner
{
  public abstract class ChainmaillePatternEdges
  {
    protected EdgeGeometryEnum geometry = EdgeGeometryEnum.None;
    protected Dictionary<EdgeOrientationEnum, ChainmaillePatternSet>
      edgePatternSets =
      new Dictionary<EdgeOrientationEnum, ChainmaillePatternSet>();
    protected Dictionary<CornerOrientationEnum, ChainmaillePatternSet>
      cornerPatternSets =
      new Dictionary<CornerOrientationEnum, ChainmaillePatternSet>();

    public EdgeGeometryEnum Geometry
    {
      get { return geometry; }
    }

    public ChainmaillePatternSet Corner(CornerOrientationEnum cornerOrientation)
    {
      ChainmaillePatternSet result = null;

      if (cornerPatternSets.ContainsKey(cornerOrientation))
      {
        result = cornerPatternSets[cornerOrientation];
      }

      return result;
    }

    public ChainmaillePatternSet Edge(EdgeOrientationEnum edgeOrientation)
    {
      ChainmaillePatternSet result = null;

      if (edgePatternSets.ContainsKey(edgeOrientation))
      {
        result = edgePatternSets[edgeOrientation];
      }

      return result;
    }

    public bool HasCorner(CornerOrientationEnum cornerOrientation)
    {
      return cornerPatternSets.ContainsKey(cornerOrientation);
    }

    public bool HasEdge(EdgeOrientationEnum edgeOrientation)
    {
      return edgePatternSets.ContainsKey(edgeOrientation);
    }

  }
}
