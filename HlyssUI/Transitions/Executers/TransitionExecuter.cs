using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Transitions.Executers
{
    abstract class TransitionExecuter
    {
        public string Name { get; private set; }

        public TransitionExecuter(string name)
        {
            Name = name;
        }

        public abstract Transition GetTransition(string transitionStr);
    }
}
