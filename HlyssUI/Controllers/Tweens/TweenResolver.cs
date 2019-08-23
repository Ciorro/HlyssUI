using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Controllers.Tweens
{
    static class TweenResolver
    {
        private static List<Tween> _tweens = new List<Tween>()
        {
            new TweenIn(),
            new TweenOut(),
            new TweenInstant()
        };

        public static Tween GetTween(string name)
        {
            foreach (var tween in _tweens)
            {
                if (tween.Name == name)
                    return tween.Get();
            }

            return null;
        }

        public static void AddTween(Tween tween)
        {
            _tweens.Add(tween);
        }
    }
}
