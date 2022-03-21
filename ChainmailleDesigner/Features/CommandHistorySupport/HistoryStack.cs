using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    public class HistoryStack<T>
    {
        private LinkedList<T> dataCollection { get; set; }
        public bool HasItems {  get {  return dataCollection.Count > 0; } }
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

        public T Pop()
        {
            var Top = dataCollection.FirstOrDefault();
            if (dataCollection.Count > 0) { dataCollection.RemoveFirst(); }
            return Top;
        }

        public void Clear()
        {
            dataCollection.Clear();
        }
    }
}
