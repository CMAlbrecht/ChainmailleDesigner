// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorCounter.cs


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
using System.Linq;
using System.Text;

namespace ChainmailleDesigner
{
  public class ColorCounter
  {
    private ChainmailleDesign chainmailleDesign = null;
    private Bitmap colorImage = null;
    private Palette palette = null;

    // Color => section name, color name.
    private Dictionary<Color, Tuple<string, string>> colorNames =
      new Dictionary<Color, Tuple<string, string>>();
    // Color => number of rings of that color
    private Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();
    // Total number of non-transparent ring colors.
    private int totalCount = 0;
    // Palette section name (or blank) => Color name (or RGB triple) => count.
    private SortedDictionary<string,
      SortedDictionary<string, Tuple<int, Color>>>
      colorCountByColorName = new SortedDictionary<string,
        SortedDictionary<string, Tuple<int, Color>>>();
    // Run length encoding of each row.
    // row number => list of color, count.
    private SortedDictionary<int, List<Tuple<Color, int>>>
      rowRunLengthEncodings =
      new SortedDictionary<int, List<Tuple<Color, int>>>();
    // Whether any rings are transparent.
    private bool someRingsAreTransparent = false;

    string report = string.Empty;

    public ColorCounter(ChainmailleDesign design, Palette designPalette)
    {
      chainmailleDesign = design;
      palette = designPalette;
      if (chainmailleDesign != null)
      {
        colorImage = chainmailleDesign.ColorImage.BitmapImage;
        if (colorImage != null)
        {
          Graphics g = Graphics.FromImage(colorImage);
          CountTheColors(g);
          g.Dispose();
        }
      }
    }

    public SortedDictionary<string, SortedDictionary<string,
      Tuple<int, Color>>> ColorCountByColorName
    {
      get { return colorCountByColorName; }
    }

    private void CountTheColors(Graphics g)
    {
      if (palette != null)
      {
        // Collect the color name information.
        foreach (PaletteSection section in palette.Sections)
        {
          Dictionary<string, Color> colors = section.Colors;
          foreach (string colorName in colors.Keys)
          {
            Color color = colors[colorName];
            string colorId = colorName;
            if (colorName.StartsWith("Color "))
            {
              // The name is just a color number, so substitute the RGB triple.
              colorId = string.Format("{0,3} {1,3} {2,3}",
                color.R, color.G, color.B);
              Color nearestColor = g.GetNearestColor(color);
              KnownColor knownColor = KnownColors.MatchingColor(nearestColor);
              if (knownColor != 0)
              {
                colorId = knownColor.ToString();
              }
            }
            colorNames.Add(color,
              new Tuple<string, string>(section.Name, colorId));
          }
        }
      }

      SortedDictionary<int, SortedDictionary<int, Color>> buildImage;
      if (chainmailleDesign.ChainmailPattern.HasBuildOffsets)
      {
        buildImage = chainmailleDesign.ConstructBuildImage();
      }
      else
      {
        // Just put the color image into build image form.
        buildImage = new SortedDictionary<int, SortedDictionary<int, Color>>();
        for (int y = 0; y < colorImage.Height; y++)
        {
          buildImage.Add(y, new SortedDictionary<int, Color>());
          for (int x = 0; x < colorImage.Width; x++)
          {
            buildImage[y].Add(x, colorImage.GetPixel(x, y));
          }
        }
      }

      // Count the instances of each color, building up the run length encoding
      // for each row as we go along.
      Color? previousColor = null;
      int consecutiveCount = 0;
      rowRunLengthEncodings.Clear();
      foreach (int y in buildImage.Keys)
      {
        previousColor = null;
        consecutiveCount = 0;
        List<Tuple<Color, int>> rowRunLengthEncoding =
          new List<Tuple<Color, int>>();

        foreach (int x in buildImage[y].Keys)
        {
          Color color = buildImage[y][x];
          // Ignore transparent colors. These are used to indicate rings that
          // are not part of the design.
          if (color.A == 255)
          {
            // Color equality tests don't work, unfortunately, so we'll have to
            // go through the dictionary keys comparing components. I could
            // define an equality operator or comparator for notational
            // convenience, but that wouldn't reduce what needs to be done
            // in the inner loop here.
            bool colorWasCounted = false;
            foreach (Color countedColor in colorCounts.Keys)
            {
              if (color.R == countedColor.R &&
                  color.G == countedColor.G &&
                  color.B == countedColor.B)
              {
                colorCounts[countedColor]++;
                colorWasCounted = true;
                if (previousColor.HasValue &&
                    color.R == previousColor.Value.R &&
                    color.G == previousColor.Value.G &&
                    color.B == previousColor.Value.B)
                {
                  consecutiveCount++;
                }
                else
                {
                  if (consecutiveCount > 0 && previousColor.HasValue)
                  {
                    // End the previous run.
                    rowRunLengthEncoding.Add(new Tuple<Color, int>(
                      previousColor.Value, consecutiveCount));
                  }
                  // Begin the new run.
                  previousColor = countedColor;
                  consecutiveCount = 1;
                }
                break;
              }
            }
            if (!colorWasCounted)
            {
              colorCounts.Add(color, 1);
              if (previousColor.HasValue &&
                  color.R == previousColor.Value.R &&
                  color.G == previousColor.Value.G &&
                  color.B == previousColor.Value.B)
              {
                consecutiveCount++;
              }
              else
              {
                if (consecutiveCount > 0 && previousColor.HasValue)
                {
                  // End the previous run.
                  rowRunLengthEncoding.Add(new Tuple<Color, int>(
                    previousColor.Value, consecutiveCount));
                }
                // Begin the new run.
                previousColor = color;
                consecutiveCount = 1;
              }
            }

            totalCount++;
          }
          else
          {
            someRingsAreTransparent = true;
          }
        }
        if (consecutiveCount > 0 && previousColor.HasValue)
        {
          // End the previous run.
          rowRunLengthEncoding.Add(new Tuple<Color, int>(
            previousColor.Value, consecutiveCount));
        }
        // End the row.
        rowRunLengthEncodings.Add(y + 1, rowRunLengthEncoding);
      }

      // Determine the color names of the counted colors.
      // Determine the maximum color name length amongst the named colors
      // present in the count while simultaneously building up the data that
      // we will need for our report.
      int longestCountNrDigits = totalCount > 0 ?
        (int)(0.999999 + Math.Log10(totalCount)) : 0;
      int longestColorNameLength = 11; // Length of "total count"
      foreach (Color countedColor in colorCounts.Keys)
      {
        // Does this color have a name?
        bool colorWasInDictionary = false;
        foreach (Color namedColor in colorNames.Keys)
        {
          if (countedColor.R == namedColor.R &&
              countedColor.G == namedColor.G &&
              countedColor.B == namedColor.B)
          {
            longestColorNameLength = Math.Max(longestColorNameLength,
              colorNames[namedColor].Item2.Length);
            colorWasInDictionary = true;
            if (!colorCountByColorName.ContainsKey(colorNames[namedColor].Item1))
            {
              // Add the palette section name.
              colorCountByColorName.Add(colorNames[namedColor].Item1,
                new SortedDictionary<string, Tuple<int, Color>>());
            }
            colorCountByColorName[colorNames[namedColor].Item1].Add(
              colorNames[namedColor].Item2,
              new Tuple<int, Color>(colorCounts[countedColor], countedColor));
            break;
          }
        }
        if (!colorWasInDictionary)
        {
          if (!colorCountByColorName.ContainsKey(string.Empty))
          {
            // Add the (empty) palette section name.
            colorCountByColorName.Add(string.Empty,
              new SortedDictionary<string, Tuple<int, Color>>());
          }
          string colorName = string.Format("{0,3} {1,3} {2,3}",
            countedColor.R, countedColor.G, countedColor.B);
          Color nearestColor = g.GetNearestColor(countedColor);
          KnownColor knownColor = KnownColors.MatchingColor(nearestColor);
          if (knownColor != 0)
          {
            colorName = knownColor.ToString();
          }
          longestColorNameLength = Math.Max(longestColorNameLength,
            colorName.Length);
          colorCountByColorName[string.Empty].Add(colorName,
            new Tuple<int, Color>(colorCounts[countedColor], countedColor));
        }
      }

      // Determine whether there are any color names that are drawn from more
      // than one section. These will need to have their section's abbreviated
      // name appended to them so that they are properly disambiguated.
      // color name => list of palette sections using that color name
      Dictionary<string, List<string>> paletteSectionsByColorName =
        new Dictionary<string, List<string>>();
      List<string> colorNamesEncountered = new List<string>();
      foreach (string sectionName in colorCountByColorName.Keys)
      {
        foreach (string colorName in colorCountByColorName[sectionName].Keys)
        {
          if (!paletteSectionsByColorName.ContainsKey(colorName))
          {
            paletteSectionsByColorName.Add(colorName,
              new List<string>() { sectionName });
          }
          else if (!paletteSectionsByColorName[colorName].Contains(
                     sectionName))
          {
            paletteSectionsByColorName[colorName].Add(sectionName);
          }
        }
      }
      List<string> colorNamesToDisambiguate = new List<string>();
      // section name => section abbreviated name
      Dictionary<string, string> sectionAbbreviationsByName =
        new Dictionary<string, string>();
      foreach (string colorName in paletteSectionsByColorName.Keys)
      {
        if (paletteSectionsByColorName[colorName].Count > 1)
        {
          colorNamesToDisambiguate.Add(colorName);
          foreach (string sectionName in paletteSectionsByColorName[colorName])
          {
            if (!sectionAbbreviationsByName.ContainsKey(sectionName))
            {
              sectionAbbreviationsByName.Add(sectionName,
                "(" + palette.Section(sectionName).AbbreviatedName + ")");
            }
          }
        }
      }
      bool colorNameDisambiguationNeeded = colorNamesToDisambiguate.Count > 0;

      // Transform the row length encoding to use color names.
      // row number => list of (color name, ring count)
      SortedDictionary<int, List<Tuple<string, int>>>
        rowRunLengthEncodingsWithColorNames =
        new SortedDictionary<int, List<Tuple<string, int>>>();
      int largestRLECount = 0;
      foreach (int row in rowRunLengthEncodings.Keys)
      {
        rowRunLengthEncodingsWithColorNames.Add(row,
          new List<Tuple<string, int>>());
        foreach (Tuple<Color, int> run in rowRunLengthEncodings[row])
        {
          // Does this color have a name?
          bool colorWasInDictionary = false;
          foreach (Color namedColor in colorNames.Keys)
          {
            if (run.Item1.R == namedColor.R &&
                run.Item1.G == namedColor.G &&
                run.Item1.B == namedColor.B)
            {
              rowRunLengthEncodingsWithColorNames[row].Add(
                new Tuple<string, int>(colorNames[namedColor].Item2 +
                  (colorNamesToDisambiguate.Contains(
                     colorNames[namedColor].Item2) &&
                   sectionAbbreviationsByName.ContainsKey(
                     colorNames[namedColor].Item1) ?
                     sectionAbbreviationsByName[colorNames[namedColor].Item1] :
                     string.Empty),
                  run.Item2));
              largestRLECount = Math.Max(largestRLECount, run.Item2);
              colorWasInDictionary = true;
            }
          }
          if (!colorWasInDictionary)
          {
            string colorName = string.Format("r{0}g{1}b{2}",
              run.Item1.R, run.Item1.G, run.Item1.B);
            Color nearestColor = g.GetNearestColor(run.Item1);
            KnownColor knownColor = KnownColors.MatchingColor(nearestColor);
            if (knownColor != 0)
            {
              colorName = knownColor.ToString();
            }
            rowRunLengthEncodingsWithColorNames[row].Add(
              new Tuple<string, int>(colorName, run.Item2));
            largestRLECount = Math.Max(largestRLECount, run.Item2);
          }
        }
      }

      // Write the report.
      string lineFormat = "{0,-" + longestColorNameLength.ToString() +
        "}  {1," + longestCountNrDigits.ToString() + "}";
      StringBuilder reportWriter = new StringBuilder();
      // Write the column headings.
      reportWriter.AppendLine(string.Format(lineFormat, "Color", "Count"));
      reportWriter.AppendLine(string.Format(lineFormat, "=====", "====="));
      foreach (string sectionName in colorCountByColorName.Keys)
      {
        // Write the section name.
        reportWriter.AppendLine();
        if (sectionName != string.Empty)
        {
          reportWriter.AppendLine("--- " + sectionName + 
            (colorNameDisambiguationNeeded &&
             sectionAbbreviationsByName.ContainsKey(sectionName) ?
             " " + sectionAbbreviationsByName[sectionName] :
             string.Empty) + " ---");
        }
        // Write the names and counts of colors in this section.
        foreach (string colorName in colorCountByColorName[sectionName].Keys)
        {
          reportWriter.AppendLine(string.Format(lineFormat, colorName,
            colorCountByColorName[sectionName][colorName].Item1));
        }
      }
      // Write the total count.
      reportWriter.AppendLine();
      reportWriter.AppendLine(string.Format(lineFormat,
        "total count", totalCount));

      // Write the row length encodings for each row.
      reportWriter.AppendLine();
      reportWriter.AppendLine();

      int allRowsOffset = 1 - rowRunLengthEncodingsWithColorNames.Keys.First();
      int largestRowNumberNrDigits = (int)(0.999999 + Math.Log10(
        rowRunLengthEncodingsWithColorNames.Count));
      int largestRLECountNrDigits = (int)(0.999999 + Math.Log10(
        largestRLECount));
      string rowNumberFormat =
        "  row {0," + largestRowNumberNrDigits.ToString() + "}:";
      string colorCountFormat = "  {0}:{1}";
      foreach (int row in rowRunLengthEncodingsWithColorNames.Keys)
      {
        // Write the row number.
        reportWriter.Append(string.Format(rowNumberFormat,
          row + allRowsOffset));
        foreach (Tuple<string, int> run
                 in rowRunLengthEncodingsWithColorNames[row])
        {
          // Write the color name and count.
          reportWriter.Append(string.Format(colorCountFormat,
            run.Item1, run.Item2));
        }
        // End the row.
        reportWriter.AppendLine();
      }

      // Output as a string.
      report = reportWriter.ToString();
    }

    public string Report
    {
      get { return report; }
    }

    public void SaveReport(string filename)
    {
      File.WriteAllText(filename, report);
    }

    public bool SomeRingsAreTransparent
    {
      get { return someRingsAreTransparent; }
    }

    public int TotalCount
    {
      get { return totalCount; }
    }

  }
}
