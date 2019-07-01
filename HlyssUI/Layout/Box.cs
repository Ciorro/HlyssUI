using HlyssUI.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Layout
{
    public class Box : Component
    {
        public Box(Gui gui) : base(gui)
        {
            DisableClipping = true;
        }

        public override void Update()
        {
            base.Update();

            foreach (var child in Children)
            {
                if(child.NeedsRefresh)
                {
                    NeedsRefresh = true;
                    break;
                }
            }
        }

        public override void OnRefresh()
        {
            Width = $"0px";
            Height = $"0px";

            int maxX = 0, maxY = 0;

            foreach (var child in Children)
            {
                if (child.X + child.W + child.Mr + child.Ml > maxX)
                    maxX = child.X + child.W + child.Mr + child.Ml;
                if (child.Y + child.H + child.Mb + child.Mt > maxY)
                    maxY = child.Y + child.H + child.Mb + child.Mt;
            }

            Width = $"{maxX + Pl + Pr}px";
            Height = $"{maxY + Pt + Pb}px";

            base.OnRefresh();
        }
    }
}
