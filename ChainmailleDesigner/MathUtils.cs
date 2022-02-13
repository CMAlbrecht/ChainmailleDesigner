// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: MathUtils.cs


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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainmailleDesigner
{
  public static class MathUtils
  {
    /// <summary>
    /// Returns the cube of the input parameter.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static double Cube(double x)
    {
      return x * x * x;
    }

    public static float NormalizeDegrees(float degrees)
    {
      return degrees -= 360F *
        ((degrees >= 0F ? 0 : -1) + (int)(degrees / 360F));
    }

    public static double NormalizeDegrees(double x)
    {
      double result = x;
      if (result < 0.0)
      {
        while (result <= -UnitConverter.DegreesPerCycle)
        {
          result += UnitConverter.DegreesPerCycle;
        }
      }
      else
      {
        while (result >= UnitConverter.DegreesPerCycle)
        {
          result -= UnitConverter.DegreesPerCycle;
        }
      }

      return result;
    }

    public static double RootSumSquares(double x, double y)
    {
      return (float)Math.Sqrt(x * x + y * y);
    }

    public static float RootSumSquares(float x, float y)
    {
      return (float)Math.Sqrt(x * x + y * y);
    }

    /// <summary>
    /// Returns the square of the input parameter.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static double Square(double x)
    {
      return x * x;
    }

  }
}
