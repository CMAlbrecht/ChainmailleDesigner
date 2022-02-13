// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: KnownColors.cs


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

namespace ChainmailleDesigner
{
  public class KnownColors
  {
    private static Dictionary<KnownColor, Color> knownColors =
      new Dictionary<KnownColor, Color>();

    static KnownColors()
    {
      // The full known colors enumeration includes system colors like Menu
      // and ActiveBorder; we don't want those.
      foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
      {
        if (knownColor != 0)
        {
          Color color = Color.FromKnownColor(knownColor);
          if (!color.IsSystemColor)
          {
            knownColors.Add(knownColor, color);
          }
        }
      }
    }

    /// <summary>
    /// Color.ToKnownColor doesn't work if the color wasn't originally
    /// created from a known color (how stupid is that??), so compare
    /// the specified color with colors created from the known colors
    /// to find a match.
    /// Yes, there are more efficient ways to search a color space.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static KnownColor MatchingColor(Color color)
    {
      KnownColor result = 0;

      foreach (KnownColor knownColor in knownColors.Keys)
      {
        if (color.R == knownColors[knownColor].R &&
            color.G == knownColors[knownColor].G &&
            color.B == knownColors[knownColor].B)
        {
          result = knownColor;
          break;
        }
      }

      return result;
    }

  }
}
