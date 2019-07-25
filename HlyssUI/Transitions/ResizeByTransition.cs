using HlyssUI.Utils;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

//TODO: Resize by

namespace HlyssUI.Transitions
{
    class ResizeByTransition : Transition
    {
        private string _unitW;
        private string _unitH;
        private Vector2f _from;
        private Vector2f _to;

        public ResizeByTransition(string w, string h)
        {
            Match matchW = StringDimensionsConverter.DimRegex.Match(w);
            Match matchH = StringDimensionsConverter.DimRegex.Match(h);

            _to = new Vector2f(int.Parse(matchW.Groups[1].Value), int.Parse(matchH.Groups[1].Value));
            _unitW = matchW.Groups[2].Value;
            _unitH = matchH.Groups[2].Value;
        }

        public override void Start()
        {
            base.Start();
            _from = new Vector2f(Engine.Component.Size.X, Engine.Component.Size.Y);
            _to += _from;
        }

        public override void Update()
        {
            base.Update();

            Vector2i newSize = new Vector2i();
            newSize.X = (int)(_from.X + (_to.X - _from.X) * Percentage);
            newSize.Y = (int)(_from.Y + (_to.Y - _from.Y) * Percentage);

            Engine.Component.Width = $"{newSize.X}{_unitW}";
            Engine.Component.Height = $"{newSize.Y}{_unitH}";
        }
    }
}
