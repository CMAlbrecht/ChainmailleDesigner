// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: IniFile.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ChainmailleDesigner
{
  public class IniFile
  {
    string Path;
    string EXE = Assembly.GetExecutingAssembly().GetName().Name;

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern int WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

    public IniFile(string IniPath = null)
    {
      Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
    }

    public string Read(string Key, string Section = null)
    {
      var RetVal = new StringBuilder(255);
      GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
      return RetVal.ToString();
    }

    public void Write(string Key, string Value, string Section = null)
    {
      WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
    }

    public void DeleteKey(string Key, string Section = null)
    {
      Write(Key, null, Section ?? EXE);
    }

    public void DeleteSection(string Section = null)
    {
      Write(null, null, Section ?? EXE);
    }

    public bool KeyExists(string Key, string Section = null)
    {
      return Read(Key, Section).Length > 0;
    }

  }
}
