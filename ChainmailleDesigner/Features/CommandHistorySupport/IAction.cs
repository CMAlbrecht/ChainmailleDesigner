using System;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    /// <summary>
    /// Template for an item that can exist on the undo/redo stack
    /// </summary>
    public interface IAction
    {
        void Undo();

        void Redo();
    }
}
