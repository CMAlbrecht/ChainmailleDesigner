using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    /// <summary>
    /// Managed stack of items to be used for undo/redo items
    /// Will honor the History Limit configuration setting when instructed (only needed for undo stack as the redo stack is only filled from the undo stack)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HistoryStack<T>
    {
        private LinkedList<T> dataCollection { get; set; }
        public bool HasItems {  get {  return dataCollection.Count > 0; } }
        /// <summary>
        /// Flags if the stack has 0 or 1 items for when to trigger the UI enabled status change
        /// </summary>
        public bool EnableDisableMenuItems {  get { return dataCollection.Count <= 1; } }
        private int HistoryLimit
        {
            get
            {
                return Properties.Settings.Default.HistoryLimit;
            }
        }

        public HistoryStack()
        {
            dataCollection = new LinkedList<T>();
        }

        /// <summary>
        /// Add an item top the stack
        /// </summary>
        /// <param name="item"></param>
        /// <param name="limitQueue"></param>
        public void Push(T item, bool limitQueue = false)
        {
            dataCollection.AddFirst(item);

            if (limitQueue && dataCollection.Count > HistoryLimit)
            {
                do
                {
                    dataCollection.RemoveLast();
                }
                while (dataCollection.Count > HistoryLimit);
            }
        }

        /// <summary>
        /// Remove an item from the stack and return it
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            var Top = dataCollection.FirstOrDefault();
            if (dataCollection.Count > 0) { dataCollection.RemoveFirst(); }
            return Top;
        }

        /// <summary>
        /// Empty the stack
        /// </summary>
        public void Clear()
        {
            dataCollection.Clear();
        }
    }
}
