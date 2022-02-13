// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: PaletteFormClientInterface.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System.Drawing;

namespace ChainmailleDesigner
{
  interface PaletteFormClientInterface
  {
    /// <summary>
    /// The client implements this interface to react to a change in the
    /// background color selected in the palette form.
    /// </summary>
    /// <param name="newColor"></param>
    void BackgroundColorHasChanged(Color newColor);

    /// <summary>
    /// The client implements this interface to react to a change in the
    /// outline color selected in the palette form.
    /// </summary>
    /// <param name="newColor"></param>
    void OutlineColorHasChanged(Color newColor);

    /// <summary>
    /// The client implements this interface to show or hide the palette form.
    /// </summary>
    void ShowPaletteForm(bool show);

  }
}
