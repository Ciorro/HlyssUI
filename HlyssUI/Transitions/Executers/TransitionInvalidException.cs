using System;

namespace HlyssUI.Transitions.Executers
{
    class TransitionInvalidException : Exception
    {
        public TransitionInvalidException()
        {
        }

        public TransitionInvalidException(string message) : base(message)
        {
        }
    }
}
