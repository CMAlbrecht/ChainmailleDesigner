using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainmailleDesigner.Features.CommandHistorySupport;

namespace ChainmailleDesigner.Features
{
    /// <summary>
    /// Architecture inspired by an article from Nasi Jofche
    /// https://betterprogramming.pub/utilizing-the-command-pattern-to-support-undo-redo-and-history-of-operations-b28fa9d58910
    /// 
    /// Undo/Redo system for Chainmaille Designer
    /// </summary>
    public class CommandHistory
    {
        public delegate void HistoryChangedEventHandler(object source, EventArgs args);
        public event HistoryChangedEventHandler HistoryChanged;

        public class HistoryStatus : EventArgs
        {
            public bool HasUndoAvailable { get; set; }
            public bool HasRedoAvailable { get; set; }
        }

        public static CommandHistory instance { get; private set; }

        private HistoryStack<List<IAction>> stackUndo { get; set; }
        private HistoryStack<List<IAction>> stackRedo { get; set; }

        public static bool HasUndoAvailable { get { return instance.stackUndo.HasItems; } }
        public static bool HasRedoAvailable { get { return instance.stackRedo.HasItems; } }

        static CommandHistory()
        {
            instance = new CommandHistory();
        }

        private CommandHistory()
        {
            stackUndo = new HistoryStack<List<IAction>>();
            stackRedo = new HistoryStack<List<IAction>>();
        }

        public static void Executed(IAction newAction)
        {
            if (newAction != null)
            {
                var SingleActionList = new List<IAction>();
                SingleActionList.Add(newAction);
                instance.stackUndo.Push(SingleActionList, limitQueue: true);
                instance.ClearReverse();
            }
        }

        public static void Executed(List<IAction> newActionGroup)
        {
            if (newActionGroup != null && newActionGroup.Count > 0)
            {
                instance.stackUndo.Push(newActionGroup, limitQueue: true);
                instance.ClearReverse();
            }
        }

        public static void Undo()
        {
            var UndoActions = instance.stackUndo.Pop();
            if (UndoActions != null)
            {
                instance.stackRedo.Push(UndoActions);

                foreach (var undoAction in UndoActions) { undoAction.Undo(); }

                instance.TriggerEvent();
            }
        }

        public static void Redo()
        {
            var RedoActions = instance.stackRedo.Pop();
            if (RedoActions != null)
            {
                instance.stackUndo.Push(RedoActions);

                foreach (var redoAction in RedoActions) { redoAction.Redo(); }

                instance.TriggerEvent();
            }
        }

        private void TriggerEvent()
        {
            if (HistoryChanged != null && (stackUndo.EnableDisableMenuItems || stackRedo.EnableDisableMenuItems))
            {
                var HistoryEventArgs = new HistoryStatus() { HasUndoAvailable = HasUndoAvailable, HasRedoAvailable = HasRedoAvailable };
                HistoryChanged(this, HistoryEventArgs);
            }
        }

        private void ClearReverse()
        {
            if (stackUndo.HasItems)
            {
                stackRedo.Clear();
                TriggerEvent();
            }
        }
    }
}
