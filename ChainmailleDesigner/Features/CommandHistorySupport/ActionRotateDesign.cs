using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    internal class ActionRotateDesign : IAction
    {
        protected ChainmailleDesignerForm CDF { get; set; }
        protected float DegreesRotated { get; set; }

        public ActionRotateDesign(ChainmailleDesignerForm cDF,
            float degreesRotated)
        {
            CDF = cDF;
            DegreesRotated = degreesRotated;
        }

        public void Undo()
        {
            CDF.RotateDesign(-DegreesRotated, true);
        }

        public void Redo()
        {
            CDF.RotateDesign(DegreesRotated, true);
        }
    }
}
