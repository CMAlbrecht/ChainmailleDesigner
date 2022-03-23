using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    internal class ActionRingColorChange : IAction
    {
        protected ChainmailleDesign CD {  get; set; }
        protected ChainmaillePatternElementId ElementId { get; set; }
        protected Color RingColor { get; set; }
        protected ChainmaillePatternElement ReferencedElement { get; set; }
        protected Color OldRingColor { get; set; }

        public ActionRingColorChange(ChainmailleDesign cD,
            ChainmaillePatternElementId elementId,
            Color ringColor,
            ChainmaillePatternElement referncedElement,
            Color oldRingColor)
        {
            CD = cD;
            ElementId = elementId;
            RingColor = ringColor;
            ReferencedElement = referncedElement;
            OldRingColor = oldRingColor;
        }

        public void Undo()
        {
            CD.SetElementColor(ElementId, OldRingColor, ReferencedElement, true);
            CD.RenderPatternElement(ElementId, ReferencedElement);
        }

        public void Redo()
        {
            CD.SetElementColor(ElementId, RingColor, ReferencedElement, true);
            CD.RenderPatternElement(ElementId, ReferencedElement);
        }
    }
}
