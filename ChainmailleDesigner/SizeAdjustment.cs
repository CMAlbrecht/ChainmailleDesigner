// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: SizeAdjustment.cs


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
using System.Drawing;

namespace ChainmailleDesigner
{
  public class SizeAdjustment
  {
    public SizeF UpperLeftAdjustment;
    public SizeF LowerRightAdjustment;
    public DesignSizeUnitsEnum Units;

    public SizeAdjustment(SizeF upperLeft, SizeF lowerRight,
      DesignSizeUnitsEnum adjustmentUnits)
    {
      UpperLeftAdjustment = new SizeF(upperLeft);
      LowerRightAdjustment = new SizeF(lowerRight);
      Units = adjustmentUnits;
    }

    public SizeAdjustment(Size upperLeft, Size lowerRight,
      DesignSizeUnitsEnum adjustmentUnits)
    {
      UpperLeftAdjustment = new SizeF(upperLeft);
      LowerRightAdjustment = new SizeF(lowerRight);
      Units = adjustmentUnits;
    }

    public SizeAdjustment(float top, float bottom, float left, float right,
      DesignSizeUnitsEnum adjustmentUnits)
    {
      UpperLeftAdjustment = new SizeF(left, top);
      LowerRightAdjustment = new SizeF(right, bottom);
      Units = adjustmentUnits;
    }

    public SizeAdjustment(SizeAdjustment that)
    {
      UpperLeftAdjustment = new SizeF(that.UpperLeftAdjustment);
      LowerRightAdjustment = new SizeF(that.LowerRightAdjustment);
      Units = that.Units;
    }

    float epsilon = 0.05F;
    public bool IsSignificant
    {
      get
      {
        return
          Math.Abs(UpperLeftAdjustment.Width) > epsilon ||
          Math.Abs(UpperLeftAdjustment.Height) > epsilon ||
          Math.Abs(LowerRightAdjustment.Width) > epsilon ||
          Math.Abs(LowerRightAdjustment.Height) > epsilon;
      }
    }

  }
}
