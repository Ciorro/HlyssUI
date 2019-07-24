using HlyssUI.Components;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Transitions
{
    class SizeTransition : Transition
    {
        private string _unit;
        private Vector2f _from;
        private Vector2f _to;
        private Component _component;

        public SizeTransition(int fromW, int fromH, int toW, int toH, string unit, Component component)
        {
            _from = new Vector2f(fromW, fromH);
            _to = new Vector2f(toW, toH);
            _unit = unit;
            _component = component;
        }

        public override void Update()
        {
            base.Update();

            Vector2i newSize = new Vector2i();
            newSize.X = (int)(_from.X + (_to.X - _from.X) * Percentage);
            newSize.Y = (int)(_from.Y + (_to.Y - _from.Y) * Percentage);

            _component.Width = $"{newSize.X}{_unit}";
            _component.Height = $"{newSize.Y}{_unit}";
        }
    }
}
