// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: WeaveSelectionForm.cs


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
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ChainmailleDesigner
{
  public partial class WeaveSelectionForm : Form
  {
    private const int highestWeaveXmlVersionUnderstood = 2;
    private string selectedWeaveFile = string.Empty;
    private string selectedWeaveName = string.Empty;
    private Rectangle galleryImageRectangle = new Rectangle(0, 0, 100, 50);
    private PixelFormat galleryImageFormat = PixelFormat.Format24bppRgb;

    public WeaveSelectionForm()
    {
      InitializeComponent();

      InitializeWeaveLists();
    }

    Image CreateGalleryImage(string filename)
    {
      Image result = new Bitmap(galleryImageRectangle.Width,
        galleryImageRectangle.Height, galleryImageFormat);

      try
      {
        Image brushImage = Image.FromFile(filename);
        TextureBrush imageBrush = new TextureBrush(brushImage);
        Graphics g = Graphics.FromImage(result);
        g.FillRectangle(imageBrush, galleryImageRectangle);
        g.Dispose();
        imageBrush.Dispose();
        brushImage.Dispose();
      }
      catch { }

      return result;
    }

    private void InitializeWeaveLists()
    {
      List<FileInfo> possibleWeaveFiles = new List<FileInfo>();
      DirectoryInfo directory = new DirectoryInfo(
        Properties.Settings.Default.PatternDirectory);
      if (directory.Exists)
      {
        possibleWeaveFiles.AddRange(directory.EnumerateFiles(
          "*.xml", SearchOption.AllDirectories));
        possibleWeaveFiles.AddRange(directory.EnumerateFiles(
          "*.ini", SearchOption.AllDirectories));

        ImageList weaveImageList = new ImageList();
        weaveImageList.ImageSize = new Size(100, 50);
        weaveImageList.ColorDepth = ColorDepth.Depth24Bit;
        int imageIndex = 0;
        string imageName;
        int? useImageIndex;
        Rectangle imageRectangle = new Rectangle(0, 0,
          weaveImageList.ImageSize.Width, weaveImageList.ImageSize.Height);

        // Each list entry has a list of column texts followed by the image index
        // into the image list for the gallery.
        List<Tuple<List<string>, int?>> weaveFiles =
          new List<Tuple<List<string>, int?>>();

        foreach (FileInfo fileInfo in possibleWeaveFiles)
        {
          if (fileInfo.Extension == ".ini")
          {
            try
            {
              IniFile iniFile = new IniFile(fileInfo.FullName);
              if (iniFile.KeyExists("Filename", "Subtile001"))
              {
                List<string> detailColumns = new List<string>();
                detailColumns.Add(Path.GetFileNameWithoutExtension(
                  fileInfo.FullName));
                string description = iniFile.Read("Description", "General");
                string scalesText = string.Empty;
                if (description.Contains(" - "))
                {
                  string[] split = description.Split(new string[] { " - " },
                    StringSplitOptions.None);
                  description = split[0];
                  scalesText = split[1];
                }
                detailColumns.Add(description);
                detailColumns.Add(scalesText);
                detailColumns.Add(fileInfo.FullName);
                useImageIndex = null;
                try
                {
                  imageName = fileInfo.DirectoryName + "\\" +
                    iniFile.Read("OutlineFilename", "General");
                  weaveImageList.Images.Add(CreateGalleryImage(imageName));
                  useImageIndex = imageIndex++;
                }
                catch { }

                weaveFiles.Add(new Tuple<List<string>, int?>(
                  detailColumns, useImageIndex));
              }
            }
            catch { }
          }
          else if (fileInfo.Extension == ".xml")
          {
            try
            {
              XmlDocument doc = new XmlDocument();
              doc.LoadXml(File.ReadAllText(fileInfo.FullName));
              XmlNode weaveNode = doc.SelectSingleNode("ChainmailleWeave");
              if (weaveNode != null)
              {
                XmlAttribute versionAttribute = weaveNode.Attributes["version"];
                if (versionAttribute != null)
                {
                  int version = int.Parse(versionAttribute.Value);
                  if (version <= highestWeaveXmlVersionUnderstood)
                  {
                    List<string> detailColumns = new List<string>();

                    XmlAttribute nameAttribute = weaveNode.Attributes["name"];
                    if (nameAttribute != null)
                    {
                      detailColumns.Add(nameAttribute.Value);
                    }
                    else
                    {
                      detailColumns.Add(string.Empty);
                    }

                    XmlNode descriptionNode = weaveNode.SelectSingleNode("Description");
                    if (descriptionNode != null)
                    {
                      detailColumns.Add(descriptionNode.InnerText);
                    }
                    else
                    {
                      detailColumns.Add(string.Empty);
                    }

                    string scalesText = string.Empty;
                    XmlNode scalesNode = weaveNode.SelectSingleNode("Scales");
                    if (scalesNode != null)
                    {
                      if (scalesNode != null)
                      {
                        XmlNodeList scaleNodes = scalesNode.SelectNodes("Scale");
                        foreach (XmlNode scaleNode in scaleNodes)
                        {
                          PatternScale patternScale =
                            new PatternScale(scaleNode);
                          scalesText +=
                            (scalesText.Length > 0 ? "; " : string.Empty) +
                            patternScale.Description;
                        }
                      }
                    }
                    detailColumns.Add(scalesText);

                    detailColumns.Add(fileInfo.FullName);

                    useImageIndex = null;
                    XmlNode bodyNode = weaveNode.SelectSingleNode("Body");
                    if (bodyNode != null)
                    {
                      XmlNode imageNode = bodyNode.SelectSingleNode("OutlineImage");
                      if (imageNode != null)
                      {
                        XmlAttribute fileAttribute = imageNode.Attributes["file"];
                        if (fileAttribute != null)
                        {
                          try
                          {
                            imageName = fileInfo.DirectoryName + "\\" +
                              fileAttribute.Value;
                            weaveImageList.Images.Add(CreateGalleryImage(imageName));
                            useImageIndex = imageIndex++;
                          }
                          catch { }
                        }
                      }
                    }

                    weaveFiles.Add(new Tuple<List<string>, int?>(
                      detailColumns, useImageIndex));
                  }
                }
              }
            }
            catch { }
          }
        }

        weaveGalleryListView.LargeImageList = weaveImageList;
        foreach (Tuple<List<string>, int?> weaveFile in weaveFiles)
        {
          weaveListView.Items.Add(new ListViewItem(weaveFile.Item1.ToArray()));
          if (weaveFile.Item2 != null)
          {
#if false
          ListViewItem item = new ListViewItem();
          List<ListViewItem.ListViewSubItem> subItems =
            new List<ListViewItem.ListViewSubItem>();
          ListViewItem.ListViewSubItem subitem =
            new ListViewItem.ListViewSubItem();
          subitem.Name = weaveGalleryNameColumnHeader.Name;
          subitem.Text = weaveFile.Item1[0];
          subItems.Add(subitem);
          subitem = new ListViewItem.ListViewSubItem();
          subitem.Name = weaveGalleryDescriptionColumnHeader.Name;
          subitem.Text = weaveFile.Item1[1];
          subItems.Add(subitem);
          subitem = new ListViewItem.ListViewSubItem();
          subitem.Name = weaveGalleryRingsColumnHeader.Name;
          subitem.Text = weaveFile.Item1[2];
          subItems.Add(subitem);
          subitem = new ListViewItem.ListViewSubItem();
          subitem.Name = weaveGalleryFileColumnHeader.Name;
          subitem.Text = weaveFile.Item1[3];
          subItems.Add(subitem);
          weaveGalleryListView.Items.Add(new ListViewItem(subItems.ToArray(),
            weaveFile.Item2.Value));
#else
            weaveGalleryListView.Items.Add(new ListViewItem(
              weaveFile.Item1.ToArray(), weaveFile.Item2.Value));
#endif
          }
        }
      }
      else
      {
        string message = "The configured directory for the weave pattern " +
          "files does not exist or can't be accessed. Select Help > " +
          "Configuration to reconfigure the directory.";
        MessageBox.Show(message, "Your Attention, Please",
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      if (tabControl.SelectedTab == weaveListTabPage)
      {
        if (weaveListView.SelectedItems.Count > 0)
        {
          selectedWeaveName = weaveListView.SelectedItems[0].SubItems[0].Text;
          selectedWeaveFile = weaveListView.SelectedItems[0].SubItems[3].Text;
        }
      }
      else if (tabControl.SelectedTab == weaveGalleryTabPage)
      {
        if (weaveGalleryListView.SelectedItems.Count > 0)
        {
          selectedWeaveName = weaveGalleryListView.SelectedItems[0].SubItems[0].Text;
          selectedWeaveFile = weaveGalleryListView.SelectedItems[0].SubItems[3].Text;
        }
      }
    }

    public string SelectedWeaveFile
    {
      get { return selectedWeaveFile; }
    }

    public string SelectedWeaveName
    {
      get { return selectedWeaveName; }
    }

    private void weaveListView_DoubleClick(object sender, EventArgs e)
    {
      okButton_Click(sender, e);
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}
