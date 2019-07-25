using HlyssUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Transitions.Executers
{
    class SizeTransitionExecuter : TransitionExecuter
    {
        public SizeTransitionExecuter() : base("size")
        {
        }

        public override Transition GetTransition(string transitionStr)
        {
            try
            {
                string[] elements = transitionStr.ToLower().Split(' ');

                if (!StringDimensionsConverter.DimRegex.IsMatch(elements[1]) || !StringDimensionsConverter.DimRegex.IsMatch(elements[2]))
                    throw new TransitionInvalidException();

                if(elements[0] == "to")
                {
                    return new ResizeToTransition(elements[1], elements[2]);
                }
                else if (elements[0] == "by")
                {
                    return new ResizeByTransition(elements[1], elements[2]);
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
