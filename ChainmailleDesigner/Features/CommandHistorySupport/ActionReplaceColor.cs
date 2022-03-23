using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainmailleDesigner.Features.CommandHistorySupport
{
    internal class ActionReplaceColor : IAction
    {
        protected ChainmailleDesign CD { get; set; }
        protected Dictionary<Color, Color> ColorReplacements {  get; set; }
        protected Dictionary<Color, Color> ColorReplacementsReverse { get; set; }
        protected string RingFilter { get; set; }
        
        public ActionReplaceColor(ChainmailleDesign cD,
            Dictionary<Color, Color> colorReplacements,
            string ringFilter)
        {
            CD = cD;
            ColorReplacements = colorReplacements;
            RingFilter = ringFilter;

            ColorReplacementsReverse = new Dictionary<Color, Color>();
            foreach (KeyValuePair<Color, Color> pair in ColorReplacements)
            {
                ColorReplacementsReverse[pair.Value] = pair.Key;
            }
        }

        public void Undo()
        {
            CD.ReplaceColorsInDesign(ColorReplacementsReverse, RingFilter, true);
        }

        public void Redo()
        {
            CD.ReplaceColorsInDesign(ColorReplacements, RingFilter, true);
        }
    }
}
