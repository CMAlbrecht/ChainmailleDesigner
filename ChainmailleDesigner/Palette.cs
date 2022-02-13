// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: Palette.cs


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
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace ChainmailleDesigner
{
  public class Palette
  {
    private string paletteFile = string.Empty;
    private string paletteFileVersion = "2";
    private string paletteName = string.Empty;
    private string paletteDescription = string.Empty;
    private Dictionary<string, PaletteSection> sections =
      new Dictionary<string, PaletteSection>();
    private bool hasBeenChanged = false;
    private bool hasBeenInitialized = false;

    /// <summary>
    /// Constructor for a new palette.
    /// </summary>
    public Palette()
    {
    }

    /// <summary>
    /// Constructor from a palette file specified in XML.
    /// </summary>
    /// <param name="paletteFilePath"></param>
    /// <param name="paletteFileName"></param>
    public Palette(string paletteFilePath, string paletteFileName)
    {
      paletteFile = paletteFilePath;
      paletteName = paletteFileName;
      if (new FileInfo(paletteFile).Exists)
      {
        if (string.IsNullOrEmpty(paletteName))
        {
          paletteName = Path.GetFileNameWithoutExtension(paletteFile);
        }
        LoadFromFile();
      }
      else if (!string.IsNullOrEmpty(paletteName))
      {
        paletteFile = Properties.Settings.Default.PaletteDirectory +
          "\\" + paletteName + ".xml";
        if (new FileInfo(paletteFile).Exists)
        {
          LoadFromFile();
        }
      }
    }

    public void AddSection(PaletteSection section)
    {
      sections.Add(section.Name, section);
      hasBeenChanged = true;
    }

    public void ClearSections()
    {
      sections.Clear();
    }

    public string DescribeColor(Color color)
    {
      string colorDescription = string.Empty;

      foreach (PaletteSection section in sections.Values)
      {
        if (!section.Hidden)
        {
          foreach (string colorName in section.Colors.Keys)
          {
            Color sectionColor = section.Colors[colorName];
            if (sectionColor.R == color.R &&
                sectionColor.G == color.G &&
                sectionColor.B == color.B)
            {
              colorDescription = colorName;
              break;
            }
          }
          if (!string.IsNullOrEmpty(colorDescription))
          {
            break;
          }
        }
      }

      return colorDescription;
    }

    public string Description
    {
      get { return paletteDescription; }
      set { paletteDescription = value; }
    }

    public bool HasBeenChanged
    {
      get { return hasBeenChanged; }
      set { hasBeenChanged = value; }
    }

    public bool HasBeenInitialized
    {
      get { return hasBeenInitialized; }
    }

    public List<string> HiddenSections
    {
      get
      {
        List<string> result = new List<string>();

        foreach (PaletteSection section in sections.Values)
        {
          if (section.Hidden)
          {
            result.Add(section.Name);
          }
        }

        return result;
      }
      set
      {
        foreach (PaletteSection section in sections.Values)
        {
          section.Hidden = value.Contains(section.Name);
        }
      }
    }

    /// <summary>
    /// Initializes the palette from the contents of an XML palette file.
    /// </summary>
    private void LoadFromFile()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(paletteFile);
      XmlNode paletteNode = doc.SelectSingleNode("Palette");
      if (paletteNode != null)
      {
        // The name of this palette.
        XmlAttribute nameAttribute = paletteNode.Attributes["name"];
        if (nameAttribute != null)
        {
          paletteName = nameAttribute.Value;
        }
        else
        {
          paletteName = Path.GetFileNameWithoutExtension(paletteFile);
        }

        XmlNode descriptionNode = paletteNode.SelectSingleNode("Description");
        if (descriptionNode != null)
        {
          paletteDescription = descriptionNode.InnerText;
        }

        // Sections of this palette.
        XmlNodeList sectionNodes = paletteNode.SelectNodes("PaletteSection");
        if (sectionNodes != null)
        {
          int sectionIndex = 1;
          foreach (XmlNode sectionNode in sectionNodes)
          {
            PaletteSection section =
              new PaletteSection(sectionNode, sectionIndex);
            sections.Add(section.Name, section);
            sectionIndex++;
          }
        }

        hasBeenInitialized = true;
        hasBeenChanged = false;
      }
    }

    public string Name
    {
      get { return paletteName; }
      set { paletteName = value; }
    }

    public string PaletteFile
    {
      get { return paletteFile; }
      set { paletteFile = value; }
    }

    public void Save()
    {
      XmlDocument doc = new XmlDocument();
      doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

      XmlNode paletteNode = doc.CreateElement("Palette");
      doc.AppendChild(paletteNode);

      XmlAttribute paletteNameAttribute = doc.CreateAttribute("name");
      paletteNameAttribute.Value = Name;
      paletteNode.Attributes.Append(paletteNameAttribute);

      XmlAttribute paletteVersionAttribute = doc.CreateAttribute("version");
      paletteVersionAttribute.Value = paletteFileVersion;
      paletteNode.Attributes.Append(paletteVersionAttribute);

      if (!string.IsNullOrEmpty(Description))
      {
        XmlNode descriptionNode = doc.CreateElement("Description");
        descriptionNode.InnerText = Description;
        paletteNode.AppendChild(descriptionNode);
      }

      foreach (PaletteSection section in sections.Values)
      {
        section.Save(paletteNode, doc);
      }

      XmlTextWriter writer = new XmlTextWriter(paletteFile, Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      doc.Save(writer);
      writer.Dispose();

      hasBeenChanged = false;
    }

    public PaletteSection Section(string sectionName)
    {
      PaletteSection result = null;

      if (sections.ContainsKey(sectionName))
      {
        result = sections[sectionName];
      }

      return result;
    }

    public List<PaletteSection> Sections
    {
      get
      {
        return new List<PaletteSection>(sections.Values);
      }
    }

  }
}
