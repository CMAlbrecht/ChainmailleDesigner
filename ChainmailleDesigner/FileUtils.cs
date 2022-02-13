// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: FileUtils.cs


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
using System.IO;

namespace ChainmailleDesigner
{
  public static class FileUtils
  {
    public static string FindSimilarDirectory(string originalName,
      List<string> alternativeDirectories)
    {
      string result = string.Empty;

      // The original filepath is no longer valid.
      // Search for the file by name.
      string trialDirectory = string.Empty;
      string trialFilepath = string.Empty;
      for (int trial = 0; trial < alternativeDirectories.Count; trial++)
      {
        // Check one of the alternative directories.
        trialDirectory = alternativeDirectories[trial];
        DirectoryInfo directoryInfo = new DirectoryInfo(trialDirectory);
        if (directoryInfo.Exists)
        {
          List<DirectoryInfo> directories =
            new List<DirectoryInfo>(directoryInfo.EnumerateDirectories(
              originalName, SearchOption.AllDirectories));
          if (directories.Count > 0)
          {
            result = directories[0].FullName;
          }
        }
        if (!string.IsNullOrEmpty(result))
        {
          break;
        }
      }

      return result;
    }

    public static string FindSimilarFile(string originalFilepath,
      List<string> alternativeDirectories, List<string> alternativeExtensions)
    {
      string result = string.Empty;

      if (!new FileInfo(originalFilepath).Exists)
      {
        // The original filepath is no longer valid.
        // Search for the file by name.
        string originalName =
          Path.GetFileNameWithoutExtension(originalFilepath);
        string originalExtension = Path.GetExtension(originalFilepath);
        string trialDirectory = string.Empty;
        string trialFilepath = string.Empty;
        for (int trial = 0; trial <= alternativeDirectories.Count; trial++)
        {
          if (trial == 0)
          {
            // First check the directory given by the original filepath.
            trialDirectory = Path.GetDirectoryName(originalFilepath);
          }
          else
          {
            // Check one of the alternative directories.
            trialDirectory = alternativeDirectories[trial - 1];
          }
          DirectoryInfo directoryInfo = new DirectoryInfo(trialDirectory);
          if (directoryInfo.Exists)
          {
            // Check the file name with the original extension.
            trialFilepath = trialDirectory + "\\" + originalName +
              originalExtension;
            if (new FileInfo(trialFilepath).Exists)
            {
              result = trialFilepath;
            }
            else
            {
              // Check for the same name but a different extension.
              foreach (FileInfo fileInfo
                       in directoryInfo.EnumerateFiles(originalName + ".*"))
              {
                if (alternativeExtensions.Contains(fileInfo.Extension))
                {
                  result = fileInfo.FullName;
                  break;
                }
              }
            }
          }
          if (!string.IsNullOrEmpty(result))
          {
            break;
          }
        }
      }

      return result;
    }

  }
}
