﻿using System;
using System.Collections.Generic;
using System.Text;
using HlyssUI.Components;
using HlyssUI.Layout;

namespace HlyssUI.Controllers
{
    class PaddingController : Controller
    {
        private Spacing _from;

        public PaddingController(Component component) : base(component)
        {
        }

        public override void OnValueChanged()
        {
            tween.Start();
            _from = component.Paddings;
        }

        public override bool Update()
        {
            if (!tween.IsRunning)
                return false;

            tween.Update();

            int left = (int)(_from.Left + (component.TargetPaddings.Left - _from.Left) * tween.Percentage);
            int right = (int)(_from.Right + (component.TargetPaddings.Right - _from.Right) * tween.Percentage);
            int top = (int)(_from.Top + (component.TargetPaddings.Top - _from.Top) * tween.Percentage);
            int bottom = (int)(_from.Bottom + (component.TargetPaddings.Bottom - _from.Bottom) * tween.Percentage);

            component.Paddings = new Spacing(left, right, top, bottom);

            return tween.IsRunning;
        }
    }
}