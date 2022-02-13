// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: PatternScale.cs


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
using System.Xml;

namespace ChainmailleDesigner
{
  public class PatternScale
  {
    public SizeF UnitSize = new SizeF();
    public Units ReferenceUnits = Units.Inches; 
    public List<RingSize> RingSizes = new List<RingSize>();

    public PatternScale(XmlNode scaleNode)
    {
      ReadFromXml(scaleNode);
    }

    /// <summary>
    /// Textual description of scale, based on the ring sizes,
    /// e.g. "16G 5/16 / 18G 3/16"
    /// </summary>
    public string Description
    {
      get
      {
        string result = string.Empty;

        foreach (RingSize ringSize in RingSizes)
        {
          result += (result.Length > 0 ? " / " : string.Empty) +
            ringSize.Gauge + "G " + ringSize.InsideDiameterInches;
        }

        return result;
      }
    }

    public void ReadFromXml(XmlNode scaleNode)
    {
      if (scaleNode != null)
      {
        XmlNode measureNode =
          scaleNode.SelectSingleNode("Measure");
        if (measureNode != null)
        {
          XmlAttribute hAttribute =
            measureNode.Attributes["unitsHorizontal"];
          XmlAttribute vAttribute =
            measureNode.Attributes["unitsVertical"];
          XmlAttribute refAttribute =
            measureNode.Attributes["referenceUnit"];
          if (hAttribute != null && vAttribute != null &&
              refAttribute != null)
          {
            UnitSize = new SizeF(
              float.Parse(hAttribute.Value),
              float.Parse(vAttribute.Value));
            if (refAttribute.Value == "inch")
            {
              ReferenceUnits = Units.Inches;
            }
            else if (refAttribute.Value == "cm")
            {
              ReferenceUnits = Units.Centimeters;
            }
          }
        }

        XmlNode ringSizesNode =
          scaleNode.SelectSingleNode("RingSizes");
        if (ringSizesNode != null)
        {
          RingSizes.Clear();
          XmlNodeList ringSizeNodes =
            ringSizesNode.SelectNodes("RingSize");
          foreach (XmlNode ringSizeNode in ringSizeNodes)
          {
            RingSize ringSize = new RingSize();
            XmlAttribute ringSizeNameAttribute =
              ringSizeNode.Attributes["name"];
            if (ringSizeNameAttribute != null)
            {
              ringSize.Name = ringSizeNameAttribute.Value;
            }
            XmlAttribute ringSizeIdAttribute =
              ringSizeNode.Attributes["idinches"];
            if (ringSizeIdAttribute != null)
            {
              ringSize.InsideDiameterInches = ringSizeIdAttribute.Value;
            }
            XmlAttribute ringSizeGaugeAttribute =
              ringSizeNode.Attributes["gauge"];
            if (ringSizeGaugeAttribute != null)
            {
              ringSize.Gauge = ringSizeGaugeAttribute.Value;
            }
            XmlAttribute ringSizeArAttribute =
              ringSizeNode.Attributes["ar"];
            if (ringSizeArAttribute != null)
            {
              ringSize.AspectRatio = ringSizeArAttribute.Value;
            }
            RingSizes.Add(ringSize);
          }
        }
      }
    }

    public void WriteToXml(XmlDocument doc, XmlNode parentNode)
    {
      XmlNode scaleNode = doc.CreateElement("Scale");
      parentNode.AppendChild(scaleNode);

      // Measure.
      if (UnitSize.Width != 0 && UnitSize.Height != 0)
      {
        XmlNode measureNode = doc.CreateElement("Measure");
        scaleNode.AppendChild(measureNode);

        XmlAttribute hUnitsAttribute = doc.CreateAttribute("unitsHorizontal");
        hUnitsAttribute.Value = UnitSize.Width.ToString();
        measureNode.Attributes.Append(hUnitsAttribute);

        XmlAttribute vUnitsAttribute = doc.CreateAttribute("unitsVertical");
        vUnitsAttribute.Value = UnitSize.Height.ToString();
        measureNode.Attributes.Append(vUnitsAttribute);

        XmlAttribute referenceUnitAttribute = doc.CreateAttribute("referenceUnit");
        referenceUnitAttribute.Value =
          ReferenceUnits == Units.Inches ? "inch" : "cm";
        measureNode.Attributes.Append(referenceUnitAttribute);
      }

      // Ring sizes.
      if (RingSizes.Count > 0)
      {
        XmlNode ringSizesNode = doc.CreateElement("RingSizes");
        scaleNode.AppendChild(ringSizesNode);
        foreach (RingSize ringSize in RingSizes)
        {
          XmlNode ringSizeNode = doc.CreateElement("RingSize");
          ringSizesNode.AppendChild(ringSizeNode);

          if (!string.IsNullOrEmpty(ringSize.Name))
          {
            XmlAttribute nameAttribute = doc.CreateAttribute("name");
            nameAttribute.Value = ringSize.Name;
            ringSizeNode.Attributes.Append(nameAttribute);
          }

          XmlAttribute idAttribute = doc.CreateAttribute("idinches");
          idAttribute.Value = ringSize.InsideDiameterInches;
          ringSizeNode.Attributes.Append(idAttribute);

          XmlAttribute gaugeAttribute = doc.CreateAttribute("gauge");
          gaugeAttribute.Value = ringSize.Gauge;
          ringSizeNode.Attributes.Append(gaugeAttribute);

          XmlAttribute arAttribute = doc.CreateAttribute("ar");
          arAttribute.Value = ringSize.AspectRatio;
          ringSizeNode.Attributes.Append(arAttribute);
        }
      }
    }

  }
}
