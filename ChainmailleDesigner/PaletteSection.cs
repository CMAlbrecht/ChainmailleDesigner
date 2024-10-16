// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: PaletteSection.cs


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

using LabColor = System.Tuple<double, double, double>;

namespace ChainmailleDesigner
{
  public class PaletteSection
  {
    string paletteSectionName = string.Empty;
    string paletteSectionAbbreviatedName = string.Empty;
    bool sectionIsHidden = false;
    Dictionary<string, Color> colors = new Dictionary<string, Color>();
    Dictionary<string, LabColor> labColors = new Dictionary<string, LabColor>();

    /// <summary>
    /// Constructor for a new palette section.
    /// </summary>
    public PaletteSection()
    {
    }

    /// <summary>
    /// Constructor from XML node.
    /// </summary>
    /// <param name="sectionNode">XML node defining the section.</param>
    /// <param name="sectionIndex">Used as a name if the node does not specify
    /// a section name.</param>
    public PaletteSection(XmlNode sectionNode, int sectionIndex)
      : this()
    {
      Load(sectionNode, sectionIndex);
    }

    public string AbbreviatedName
    {
      get { return paletteSectionAbbreviatedName; }
      set { paletteSectionAbbreviatedName = value; }
    }

    public void AddColor(Color color, string colorName)
    {
      // Issue #2, Overlapping color names
      if (colors.ContainsKey(colorName))
      {
        string tempColorName;
        var counter = 1;
        do
        {
          tempColorName = colorName + "_" + counter; // Append indexer to make a unique name while preserving what the user typed
          counter++;
        } while (colors.ContainsKey(tempColorName));
        colorName = tempColorName;
      }
      // ~Issue #2

      colors.Add(colorName, color);
      labColors.Add(colorName, ColorUtils.RgbToLab(color));
    }

    public int ColorCount
    {
      get { return colors.Count; }
    }

    public Dictionary<string, Color> Colors
    {
      get { return colors; }
    }

    /// <summary>
    /// Get the closest color in the palette section to the LAB color
    /// provided. A LAB color of 0, 0, 0 is interpreted as a transparent
    /// un-assigned color.
    /// </summary>
    /// <param name="labColor"></param>
    /// <returns>The closest color from the palette section or the transparent
    /// color</returns>
    public Color GetClosestColor(LabColor labColor)
    {
      Color result = Color.Transparent;

      if (labColor.Item1 != 0.0 || labColor.Item2 != 0.0 ||
          labColor.Item3 != 0.0)
      {

        double bestDistance = double.MaxValue;
        double distance;
        foreach (string colorName in colors.Keys)
        {
          LabColor paletteLabColor = LabColors[colorName];
          distance = ColorConverter.ColorDifference(
            labColor, paletteLabColor, 6.0);
          if (distance < bestDistance)
          {
            result = colors[colorName];
            bestDistance = distance;
          }
        }
      }

      return result;
    }

    public bool Hidden
    {
      get { return sectionIsHidden; }
      set { sectionIsHidden = value; }
    }

    public Dictionary<string, LabColor> LabColors
    {
      get { return labColors; }
    }

    private void Load(XmlNode sectionNode, int sectionIndex)
    {
      if (sectionNode != null)
      {
        // The name of the palette section.
        XmlAttribute nameAttribute = sectionNode.Attributes["name"];
        if (nameAttribute != null && nameAttribute.Value != string.Empty)
        {
          paletteSectionName = nameAttribute.Value;
        }
        else
        {
          paletteSectionName = "Section " + sectionIndex.ToString();
        }

        // The abbreviated name of the palette section.
        // Note: This name is used where space is at a premium, such as in the
        // row-by-row color report.
        XmlAttribute abbreviationAttribute =
          sectionNode.Attributes["abbreviatedName"];
        if (abbreviationAttribute != null &&
            abbreviationAttribute.Value != string.Empty)
        {
          paletteSectionAbbreviatedName = abbreviationAttribute.Value;
        }
        else
        {
          paletteSectionAbbreviatedName = "S" + sectionIndex.ToString();
        }

        // The colors in the palette section.
        XmlNodeList colorNodes = sectionNode.SelectNodes("Color");
        if (colorNodes != null)
        {
          int colorIndex = 1;
          foreach (XmlNode colorNode in colorNodes)
          {
            // The name of the color.
            string colorName = "Color " + colorIndex.ToString();
            XmlAttribute colorNameAttribute = colorNode.Attributes["name"];
            if (colorNameAttribute != null &&
                colorNameAttribute.Value != string.Empty)
            {
              colorName = colorNameAttribute.Value;
            }

            Color? color = null;
            XmlAttribute rgbAttribute = colorNode.Attributes["rgb"];
            if (rgbAttribute != null)
            {
              string rgbString = rgbAttribute.Value;
              string[] rgbStrings = rgbString.Split(' ');
              if (rgbStrings.Length == 3)
              {
                color = Color.FromArgb(int.Parse(rgbStrings[0]),
                  int.Parse(rgbStrings[1]), int.Parse(rgbStrings[2]));
              }
            }
            else
            {
              XmlAttribute hslAttribute = colorNode.Attributes["hsl"];
              if (hslAttribute != null)
              {
                string hslString = hslAttribute.Value;
                string[] hslStrings = hslString.Split(' ');
                if (hslStrings.Length == 3)
                {
                  Tuple<int, int, int> hsl = new Tuple<int, int, int>(
                    int.Parse(hslStrings[0]), int.Parse(hslStrings[1]),
                    int.Parse(hslStrings[2]));
                  Tuple<int, int, int> rgb = ColorConverter.HslToRgb(hsl);
                  color = Color.FromArgb(rgb.Item1, rgb.Item2, rgb.Item3);
                }
              }
            }

            if (color.HasValue)
            {
              AddColor(color.Value, colorName);
            }

            colorIndex++;
          }
        }
      }
    }

    public string Name
    {
      get { return paletteSectionName; }
      set { paletteSectionName = value; }
    }

    public void Save(XmlNode parentNode, XmlDocument doc)
    {
      XmlNode sectionNode = doc.CreateElement("PaletteSection");
      parentNode.AppendChild(sectionNode);

      XmlAttribute sectionNameAttribute = doc.CreateAttribute("name");
      sectionNameAttribute.Value = paletteSectionName;
      sectionNode.Attributes.Append(sectionNameAttribute);

      XmlAttribute sectionAbbreviationAttribute =
        doc.CreateAttribute("abbreviatedName");
      sectionAbbreviationAttribute.Value = paletteSectionAbbreviatedName;
      sectionNode.Attributes.Append(sectionAbbreviationAttribute);

      foreach (string colorName in colors.Keys)
      {
        Color color = colors[colorName];
        XmlNode colorNode = doc.CreateElement("Color");
        sectionNode.AppendChild(colorNode);

        XmlAttribute colorNameAttribute = doc.CreateAttribute("name");
        colorNameAttribute.Value = colorName;
        colorNode.Attributes.Append(colorNameAttribute);

        Tuple<int, int, int> rgb =
          new Tuple<int, int, int>(color.R, color.G, color.B);
        Tuple<int, int, int> hsl = ColorConverter.RgbToHsl(rgb);
        XmlAttribute hslAttribute = doc.CreateAttribute("hsl");
        hslAttribute.Value = hsl.Item1.ToString() + " " +
          hsl.Item2.ToString() + " " + hsl.Item3.ToString();
        colorNode.Attributes.Append(hslAttribute);
        XmlAttribute rgbAttribute = doc.CreateAttribute("rgb");
        rgbAttribute.Value = rgb.Item1.ToString() + " " +
          rgb.Item2.ToString() + " " + rgb.Item3.ToString();
        colorNode.Attributes.Append(rgbAttribute);
      }
    }

  }
}
