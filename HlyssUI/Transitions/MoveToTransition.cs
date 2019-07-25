using HlyssUI.Utils;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HlyssUI.Transitions
{
    class MoveToTransition : Transition
    {
        private string _unitX;
        private string _unitY;
        private Vector2f _from;
        private Vector2f _to;

        public MoveToTransition(string x, string y)
        {
            Match matchX = StringDimensionsConverter.DimRegex.Match(x);
            Match matchH = StringDimensionsConverter.DimRegex.Match(y);

            _to = new Vector2f(int.Parse(matchX.Groups[1].Value), int.Parse(matchH.Groups[1].Value));
            _unitX = matchX.Groups[2].Value;
            _unitY = matchH.Groups[2].Value;
        }

        public override void Start()
        {
            base.Start();
            _from = new Vector2f(Engine.Component.Position.X, Engine.Component.Position.Y);
        }

        public override void Update()
        {
            base.Update();

            Vector2i newPosition = new Vector2i();
            newPosition.X = (int)(_from.X + (_to.X - _from.X) * Percentage);
            newPosition.Y = (int)(_from.Y + (_to.Y - _from.Y) * Percentage);

            Engine.Component.Left = $"{newPosition.X}{_unitX}";
            Engine.Component.Top = $"{newPosition.Y}{_unitY}";
        }
    }
}
