using HlyssUI.Themes;
using SFML.Graphics;

namespace HlyssUI.Transitions.Executers
{
    class ColorTransitionExecuter : TransitionExecuter
    {
        public ColorTransitionExecuter() : base("color")
        {
        }

        public override Transition GetTransition(string transitionStr)
        {
            try
            {
                string[] elements = transitionStr.ToLower().Split(' ');
                string colorFrom = elements[0];
                Color destColor = Theme.GetColor(elements[2]);

                if (elements.Length >= 5)
                {
                    if (elements[3] == "darken")
                        destColor = Style.GetDarker(destColor, byte.Parse(elements[4]));
                    else if (elements[3] == "lighten")
                        destColor = Style.GetLighter(destColor, byte.Parse(elements[4]));
                }

                return new ColorTransition(destColor, colorFrom);
            }
            catch
            {
                throw new TransitionInvalidException();
            }
        }
    }
}
