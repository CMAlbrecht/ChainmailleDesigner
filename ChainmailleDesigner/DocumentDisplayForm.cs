// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: DocumentDisplayForm.cs


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
using System.Windows.Forms;

namespace ChainmailleDesigner
{
  public partial class DocumentDisplayForm : Form
  {
    public DocumentDisplayForm()
    {
      InitializeComponent();
    }

    public string DocumentFileName
    {
      set
      {
        documentRichTextBox.LoadFile(value);
      }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      Close();
    }

  }
}
