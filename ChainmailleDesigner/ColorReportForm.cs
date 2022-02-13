// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorReportForm.cs


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
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class ColorReportForm : Form
  {
    ColorCounter colorCounter = null;
    string designName = string.Empty;

    public ColorReportForm()
    {
      InitializeComponent();
      Left += 1;
    }

    private Label BuildColorCountLabel(string sectionName, string colorName,
      int colorCount, Point position, Size size)
    {
      Label label = new Label();
      label.BorderStyle = BorderStyle.None;
      label.Location = position;
      label.Name = "ColorCount_" + (sectionName + colorName).Replace(" ", "").
        Replace(".", "") + "Label";
      label.Size = size;
      label.Text = colorCount.ToString();
      label.TextAlign = ContentAlignment.MiddleRight;

      return label;
    }

    private Label BuildColorNameLabel(string sectionName, string colorName,
      Point position, Size size)
    {
      Label label = new Label();
      label.BorderStyle = BorderStyle.None;
      label.Location = position;
      label.Name = "Color_" + (sectionName + colorName).Replace(" ", "").
        Replace(".", "") + "Label";
      label.Size = size;
      label.Text = colorName;
      label.TextAlign = ContentAlignment.MiddleLeft;

      return label;
    }

    private Panel BuildColorSamplePanel(Color color, string sectionName,
      string colorName, Point position, Size size)
    {
      Panel panel = new Panel();
      panel.BackColor = color;
      panel.BorderStyle = BorderStyle.FixedSingle;
      panel.Location = position;
      panel.Name = "Color_" + (sectionName + colorName).Replace(" ", "").
        Replace(".", "") + "Panel";
      panel.Size = size;
      panel.Text = string.Empty;

      return panel;
    }

    private Label BuildSectionLabel(string sectionName, Point position,
      Size size)
    {
      Label label = new Label();
      label.BorderStyle = BorderStyle.FixedSingle;
      label.Location = position;
      label.Name = "Section_" + sectionName.Replace(" ", "").Replace(".", "") + "Label";
      label.Size = size;
      label.Text = sectionName;
      label.TextAlign = ContentAlignment.MiddleCenter;

      return label;
    }

    public ColorCounter ColorCounter
    {
      get { return colorCounter; }
      set
      {
        colorCounter = value;
        InitializeReportPanel();
      }
    }

    public string DesignName
    {
      get { return designName; }
      set { designName = value; }
    }

    private void InitializeReportPanel()
    {
      if (colorCounter != null)
      {
        // Palette section name (or blank) => Color name => count, color.
        SortedDictionary<string, SortedDictionary<string, Tuple<int, Color>>>
          colorCountByColorName = colorCounter.ColorCountByColorName;

        // Determine how big the report is going to be.

        // Determine the size required for the longest color name text.
        Font textFont = SystemFonts.DefaultFont;
        Font boldFont = new Font(textFont, FontStyle.Bold);
        float maxTextWidth = 0, maxTextHeight = 0;
        SizeF textSize;
        // Vertical margins totaling 1 line (1/2 + 1/2),
        // 1 line for column headers, 1 line for total count;
        int nrTextLines = 3;
        Graphics g = CreateGraphics();
        foreach (string sectionName in colorCountByColorName.Keys)
        {
          // 1 line for section name.
          nrTextLines++;
          SortedDictionary<string, Tuple<int, Color>> colorEntries =
            colorCountByColorName[sectionName];
          foreach (string colorName in colorEntries.Keys)
          {
            // 1 line for color count
            nrTextLines++;
            textSize = g.MeasureString(colorName, textFont);
            maxTextWidth = Math.Max(maxTextWidth, textSize.Width);
            maxTextHeight = Math.Max(maxTextHeight, textSize.Height);
          }
          // 1 blank line after each section.
          nrTextLines++;
        }
        int textHeight = (int)(0.999F + maxTextHeight);
        int halfHeight = textHeight / 2;
        int verticalLineSpacing = textHeight + 2;
        Size colorNameSize = new Size(
          (int)(0.999F + maxTextWidth), textHeight);

        // Determine the size required for the counts.
        textSize = g.MeasureString("Count", boldFont);
        SizeF countSize = g.MeasureString(colorCounter.TotalCount.ToString(),
          textFont) ;
        // Microsoft seems to be pretty terrible about measuring strings of
        // digits, so here we add a text height to the "measured" width.
        // Otherwise the last digit disappears and counts seem smaller.
        countSize.Width += textHeight;
        maxTextWidth = Math.Max(countSize.Width, textSize.Width);
        maxTextHeight = Math.Max(countSize.Height, textSize.Height);
        Size colorCountSize = new Size(
          (int)(Math.Ceiling(maxTextWidth)), textHeight);

        Size desiredSize = new Size(
          2 * textHeight + colorNameSize.Width + colorCountSize.Width +
          textHeight, nrTextLines * verticalLineSpacing);

        // Resize the panel to accommodate the report.
        colorReportPanel.Size = desiredSize;

        // Prepare to build the controls to present the color report.
        Point colorSamplePosition = new Point(halfHeight, halfHeight);
        Size colorSampleSize = new Size(textHeight, textHeight);
        Point colorNamePosition = new Point(2 * textHeight, halfHeight);
        Point colorCountPosition = new Point(2 * textHeight +
          colorNameSize.Width, halfHeight);
        Size sectionNameSize = new Size(colorNameSize.Width + textHeight +
          colorCountSize.Width, textHeight);

        // Build the column headers.
        Label header = BuildColorNameLabel("", "Color", colorNamePosition,
          colorNameSize);
        header.Font = new Font(header.Font, FontStyle.Underline);
        colorReportPanel.Controls.Add(header);
        header = BuildColorNameLabel("", "Count", colorCountPosition,
          colorCountSize);
        header.TextAlign = ContentAlignment.MiddleRight;
        header.Font = new Font(header.Font, FontStyle.Underline);
        colorReportPanel.Controls.Add(header);
        colorSamplePosition.Y += verticalLineSpacing;
        colorNamePosition.Y += verticalLineSpacing;
        colorCountPosition.Y += verticalLineSpacing;

        foreach (string sectionName in colorCountByColorName.Keys)
        {
          if (!string.IsNullOrEmpty(sectionName))
          {
            // Build the palette section heading.
            colorReportPanel.Controls.Add(BuildSectionLabel(sectionName,
              colorNamePosition, sectionNameSize));
          }
          colorSamplePosition.Y += verticalLineSpacing;
          colorNamePosition.Y += verticalLineSpacing;
          colorCountPosition.Y += verticalLineSpacing;

          SortedDictionary<string, Tuple<int, Color>> colorEntries =
            colorCountByColorName[sectionName];
          foreach (string colorName in colorEntries.Keys)
          {
            // Build a color sample, color name, and color count.
            Tuple<int, Color> colorEntry = colorEntries[colorName];
            colorReportPanel.Controls.Add(BuildColorSamplePanel(
              colorEntry.Item2, sectionName, colorName, colorSamplePosition,
              colorSampleSize));
            colorReportPanel.Controls.Add(BuildColorNameLabel(sectionName,
              colorName, colorNamePosition, colorNameSize));
            colorReportPanel.Controls.Add(BuildColorCountLabel(sectionName,
              colorName, colorEntry.Item1, colorCountPosition,
              colorCountSize));
            colorSamplePosition.Y += verticalLineSpacing;
            colorNamePosition.Y += verticalLineSpacing;
            colorCountPosition.Y += verticalLineSpacing;
          }

          // Skip the blank line at the end of the section.
          colorSamplePosition.Y += verticalLineSpacing;
          colorNamePosition.Y += verticalLineSpacing;
          colorCountPosition.Y += verticalLineSpacing;
        }

        g.Dispose();

        // Build the total count.
        colorReportPanel.Controls.Add(BuildColorNameLabel("", "total count",
          colorNamePosition, colorNameSize));
        colorReportPanel.Controls.Add(BuildColorCountLabel("", "total count",
          colorCounter.TotalCount, colorCountPosition, colorCountSize));
        colorSamplePosition.Y += verticalLineSpacing;
        colorNamePosition.Y += verticalLineSpacing;
        colorCountPosition.Y += verticalLineSpacing;

        // Determine current margins on the client area.
        Size margins = new Size(
          Width - ClientSize.Width, Height - ClientSize.Height);

        margins.Height += 32;
        // Size the window to the panel, plus margins, but don't make it
        // narrower or the menu won't be visible.
        Size newSize = new Size(
          Math.Max(Width, colorReportPanel.Width + margins.Width),
          colorReportPanel.Height + margins.Height);
          //Math.Max(Height, colorReportPanel.Height + margins.Height));
        MaximumSize = newSize;
        Size = newSize;
        colorReportPanel.Invalidate();
      }
    }

    int nextReportLine = 0;
    List<string> reportLines = new List<string>();
    Font reportFont = new Font(FontFamily.GenericMonospace, 12);
    Brush textBrush = new SolidBrush(Color.Black);
    int lineMaxCharacters = 0;

    /// <summary>
    /// Print the color report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void colorReportPrintToolStripMenuItem_Click(object sender,
      EventArgs e)
    {
      if (colorCounter != null)
      {
        // Break the report into lines.
        reportLines = colorCounter.Report.Split(new string[] { "\r\n" },
          StringSplitOptions.None).ToList();
        nextReportLine = 0;

        // Set up the print document.
        PrintDocument printDocument = new PrintDocument();
        printDocument.DocumentName = "Color Report";
        printDocument.DefaultPageSettings.Landscape = false;
        printDocument.PrintPage += new PrintPageEventHandler(PrintReportPage);

        // Show the print dialog.
        PrintDialog printDialog = new PrintDialog();
        printDialog.Document = printDocument;
        if (printDialog.ShowDialog() == DialogResult.OK)
        {
          // Do the printing.
          printDocument.Print();
        }
      }
    }

    private void PrintReportPage(object sender, PrintPageEventArgs e)
    {
      Rectangle pageRectangle = e.MarginBounds;
      PointF textPosition = new PointF(pageRectangle.X, pageRectangle.Y);
      // We compute text height here to preserve blank lines, since the
      // measured size of a blank line is 0, 0.
      float textHeight = e.Graphics.MeasureString("Ag", reportFont).Height;

      while (nextReportLine < reportLines.Count)
      {
        string reportLine = reportLines[nextReportLine];
        SizeF lineSize = e.Graphics.MeasureString(reportLine, reportFont);
        if (textPosition.Y + textHeight <= pageRectangle.Bottom)
        {
          if (lineSize.Width < pageRectangle.Width)
          {
            // The line doesn't have to be split, so output it as is.
            e.Graphics.DrawString(reportLine, reportFont, textBrush,
              textPosition);
            textPosition.Y += textHeight;
            nextReportLine++;
          }
          else
          {
            // The line is too long and will need to be split.
            if (lineMaxCharacters == 0)
            {
              // Since this is a monospaced font, we can now compute the
              // maximum number of characters that will fit on a line,
              // given the selected paper size, orientation, margins, etc.
              lineMaxCharacters = (int)(reportLine.Length *
                pageRectangle.Width / lineSize.Width);
            }
            // Long lines are almost certainly lines containing multiple
            // color counts in the run length encoding part of the report,
            // so the double-space between color counts is a good place to
            // split a line.
            string separator = "  ";

            int position = 0;
            List<string> lineSplits = new List<string>();

            // Note: LastIndex starts at a position and proceeds backwards
            // for the specified number of characters.
            while (position < reportLine.Length - lineMaxCharacters)
            {
              int splitAt = reportLine.LastIndexOf(separator,
                position + lineMaxCharacters, lineMaxCharacters);
              if (splitAt > 0)
              {
                lineSplits.Add(reportLine.Substring(position,
                  splitAt - position));
              }
              position = splitAt + separator.Length;
            }
            if (position < reportLine.Length)
            {
              lineSplits.Add(reportLine.Substring(position));
            }

            if (textPosition.Y + lineSplits.Count * textHeight <
                  pageRectangle.Bottom )
            {
              // All the splits of the line will fit vertically,
              // so output them now, and increment the line counter.
              foreach (string lineSplit in lineSplits)
              {
                e.Graphics.DrawString(lineSplit, reportFont, textBrush, textPosition);
                textPosition.Y += textHeight;
              }
              nextReportLine++;
            }
            else
            {
              // The line won't fit; we are out of vertical space on this page.
              break;
            }
          }
        }
        else
        {
          // We are out of vertical space on this page.
          break;
        }
      }

      e.HasMorePages = nextReportLine < reportLines.Count;
    }

    /// <summary>
    /// Save the color report to a text file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void colorReportSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (colorCounter != null)
      {
        SaveFileDialog dlg = new SaveFileDialog();
        dlg.InitialDirectory = Properties.Settings.Default.DesignDirectory;
        dlg.Filter = Properties.Settings.Default.ReportFileFilter;
        dlg.FilterIndex = 2;
        dlg.FileName = designName + " - Color Report";
        dlg.Title = "Save Color Report";
        if (dlg.ShowDialog() == DialogResult.OK)
        {
          colorCounter.SaveReport(dlg.FileName);
        }
      }
    }

    // The window does not honor the MaximumSize property.
    // By intercepting a few messages, correct behavior can be attained.
    // Thanks to Zach Johnson, "Overcome OS Imposed Windows Form Minimum Size
    // Limit".
    private const int MW_WINDOWPOSCHANGING = 0x0046;
    private const int WM_GETMINMAXINFO = 0x0024;
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == MW_WINDOWPOSCHANGING)
      {
        WindowPos windowPos = (WindowPos)m.GetLParam(typeof(WindowPos));

        // Make changes.
        windowPos.width = Math.Max(MinimumSize.Width,
          Math.Min(windowPos.width, MaximumSize.Width));
        windowPos.height = Math.Max(MinimumSize.Height,
          Math.Min(windowPos.height, MaximumSize.Height));

        // Marshal the changes back to the message.
        Marshal.StructureToPtr(windowPos, m.LParam, true);
      }

      // Do the usual processing.
      base.WndProc(ref m);

      // Make changes to WM_GETMINMAXINFO after it has been handled by the underlying
      // WndProc, so we only need to repopulate the minimum size constraints
      if (m.Msg == WM_GETMINMAXINFO)
      {
        MinMaxInfo minMaxInfo = (MinMaxInfo)m.GetLParam(typeof(MinMaxInfo));
        minMaxInfo.ptMinTrackSize.x = MinimumSize.Width;
        minMaxInfo.ptMinTrackSize.y = MinimumSize.Height;
        minMaxInfo.ptMaxTrackSize.x = MaximumSize.Width;
        minMaxInfo.ptMaxTrackSize.y = MaximumSize.Height;

        // Marshal the changes back to the message.
        Marshal.StructureToPtr(minMaxInfo, m.LParam, true);
      }
    }

    struct WindowPos
    {
      public IntPtr hwnd;
      public IntPtr hwndInsertAfter;
      public int x;
      public int y;
      public int width;
      public int height;
      public uint flags;
    }

    struct POINT
    {
      public int x;
      public int y;
    }

    struct MinMaxInfo
    {
      public POINT ptReserved;
      public POINT ptMaxSize;
      public POINT ptMaxPosition;
      public POINT ptMinTrackSize;
      public POINT ptMaxTrackSize;
    }

  }
}
