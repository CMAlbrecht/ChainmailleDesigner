// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ChainmailleDesignerConstants.cs


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
using System.ComponentModel;

namespace ChainmailleDesigner
{
  public class ChainmailleDesignerConstants
  {
    public const string rows = "rows";
    public const string columns = "columns";

    public static SortedSet<CornerOrientationEnum>
      rectangularCornerOrientations = new SortedSet<CornerOrientationEnum>()
      {
        CornerOrientationEnum.TopLeft, CornerOrientationEnum.TopRight,
        CornerOrientationEnum.BottomLeft, CornerOrientationEnum.BottomRight
      };

    public static SortedSet<EdgeOrientationEnum>
      rectangularEdgeOrientations = new SortedSet<EdgeOrientationEnum>()
      {
        EdgeOrientationEnum.Top, EdgeOrientationEnum.Bottom,
        EdgeOrientationEnum.Left, EdgeOrientationEnum.Right
      };
  }

  // Enumerations.

  public enum CornerOrientationEnum
  {
    [Description("top left")]
    TopLeft,
    [Description("top right")]
    TopRight,
    [Description("bottom left")]
    BottomLeft,
    [Description("bottom right")]
    BottomRight
  };

  public enum DesignSizeUnitsEnum
  {
    [Description("units")]
    Units,
    [Description("rows & columns")]
    RowsColumns,
    [Description("pixels")]
    Pixels,
    [Description("inches")]
    Inches,
    [Description("centimeters")]
    Centimeters
  };

  public enum DrawingFillMode
  {
    [Description("solid")]
    Solid,
    [Description("hollow")]
    Hollow
  };

  public enum EdgeGeometryEnum
  {
    [Description("none")]
    None,
    [Description("linear")]
    Linear,
    [Description("rectangular")]
    Rectangular,
    [Description("hexagonal")]
    Hexagonal
  };

  public enum EdgeOrientationEnum
  {
    [Description("top")]
    Top,
    [Description("bottom")]
    Bottom,
    [Description("left")]
    Left,
    [Description("right")]
    Right
  };

  // This may end up being the same as design size units, above.
  public enum SizeAdjustmentUnitsEnum
  {
    [Description("units")]
    Units,
    [Description("rows & columns")]
    RowsColumns,
    [Description("pixels")]
    Pixels
  };

  public enum WeaveLinkageEnum
  {
    [Description("chain")]
    Chain,
    [Description("sheet")]
    Sheet
  };

  public enum WrapEnum
  {
    [Description("none")]
    None,
    [Description("horizontal")]
    Horizontal,
    [Description("vertical")]
    Vertical
  };

}
