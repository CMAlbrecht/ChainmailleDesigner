using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    internal class ActionRotateOverlay : IAction
    {
        protected ChainmailleDesignerForm CDF { get; set; }
        protected float DegreesRotated { get; set; }

        public ActionRotateOverlay(ChainmailleDesignerForm cDF,
            float degreesRotated)
        {
            CDF = cDF;
            DegreesRotated = degreesRotated;
        }

        public void Undo()
        {
            CDF.RotateOverlay(-DegreesRotated, true);
        }

        public void Redo()
        {
            CDF.RotateOverlay(DegreesRotated, true);
        }
    }
}
