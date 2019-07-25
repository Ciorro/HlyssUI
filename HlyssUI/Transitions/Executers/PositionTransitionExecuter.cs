using HlyssUI.Utils;

namespace HlyssUI.Transitions.Executers
{
    class PositionTransitionExecuter : TransitionExecuter
    {
        public PositionTransitionExecuter() : base("position")
        {
        }

        public override Transition GetTransition(string transitionStr)
        {
            try
            {
                string[] elements = transitionStr.ToLower().Split(' ');

                if (!StringDimensionsConverter.DimRegex.IsMatch(elements[1]) || !StringDimensionsConverter.DimRegex.IsMatch(elements[2]))
                    throw new TransitionInvalidException();

                if (elements[0] == "to")
                {
                    return new MoveToTransition(elements[1], elements[2]);
                }
                else if (elements[0] == "by")
                {
                    return new MoveByTransition(elements[1], elements[2]);
                }
                else
                    throw new TransitionInvalidException();
            }
            catch
            {
                throw new TransitionInvalidException();
            }
        }
    }
}
