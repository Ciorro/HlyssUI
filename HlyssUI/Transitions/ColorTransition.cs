using HlyssUI.Components;
using HlyssUI.Utils;
using SFML.Graphics;
using System;

namespace HlyssUI.Transitions
{
    class ColorTransition:Transition
    {
        private Color _from;
        private Color _to;
        private string _name;
        private Component _component;

        public ColorTransition(Color from, Color to, string colorName, Component component)
        {
            _from = from;
            _to = to;
            _name = colorName;
            _component = component;
        }

        public override void Update()
        {
            base.Update();
            _component.Style[_name] = calcColor();
        }

        private Color calcColor()
        {
            byte r = (byte)(_from.R + ((float)_to.R - _from.R) * Percentage);
            byte g = (byte)(_from.G + ((float)_to.G - _from.G) * Percentage);
            byte b = (byte)(_from.B + ((float)_to.B - _from.B) * Percentage);
            byte a = (byte)(_from.A + ((float)_to.A - _from.A) * Percentage);

            return new Color(r, g, b, a);
        }
    }
}
