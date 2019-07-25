using HlyssUI.Components;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Transitions
{
    public class TransitionEngine
    {
        private enum UpdateMode
        {
            Simultaneous, Sequential
        }

        private UpdateMode _mode = UpdateMode.Simultaneous;
        private List<Transition> _transitions = new List<Transition>();

        public Component Component;

        public void RunSequence(params Transition[] transitions)
        {
            _transitions.Clear();
            _transitions.AddRange(transitions);

            _mode = UpdateMode.Sequential;
        }

        public void RunAll(params Transition[] transitions)
        {
            _transitions.Clear();
            _transitions.AddRange(transitions);

            _mode = UpdateMode.Simultaneous;
        }

        public void Update()
        {
            switch (_mode)
            {
                case UpdateMode.Simultaneous:
                    updateAll();
                    break;
                case UpdateMode.Sequential:
                    updateTop();
                    break;
            }
        }

        private void updateTop()
        {
            Transition transition = _transitions.Last();

            if (!transition.IsFinished)
            {
                if (!transition.IsRunning)
                {
                    transition.Engine = this;
                    transition.Start();
                }

                transition.Update();
            }
            else
            {
                _transitions.Remove(transition);
            }
        }

        private void updateAll()
        {
            for (int i = 0; i < _transitions.Count; i++)
            {
                if (!_transitions[i].IsFinished)
                {
                    if (!_transitions[i].IsRunning)
                    {
                        _transitions[i].Engine = this;
                        _transitions[i].Start();
                    }

                    _transitions[i].Update();
                }
                else
                {
                    _transitions.Remove(_transitions[i]);
                }
            }
        }
    }
}
