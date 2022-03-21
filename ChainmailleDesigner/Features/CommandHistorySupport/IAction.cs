using System;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    public interface IAction
    {
        void Undo();

        void Redo();
    }
}
