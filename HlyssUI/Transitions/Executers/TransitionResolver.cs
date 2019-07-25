using HlyssUI.Utils;
using System.Collections.Generic;

namespace HlyssUI.Transitions.Executers
{
    static class TransitionResolver
    {
        private static List<TransitionExecuter> _executers = new List<TransitionExecuter>()
        {
            new ColorTransitionExecuter(),
            new SizeTransitionExecuter()
        };

        public static Transition GetTransition(string transitionStr)
        {
            string name = transitionStr.Split(':')[0];

            foreach (var exec in _executers)
            {
                try
                {
                    if (exec.Name == name)
                        return exec.GetTransition(transitionStr.Remove(0, name.Length + 1).Trim(' '));
                }
                catch (TransitionInvalidException)
                {
                    Logger.Log($"Transition \"{transitionStr}\" was invalid.");
                }
            }

            return null;
            //TODO: Throw exception?
        }

        public static void AddExecuter(TransitionExecuter executer)
        {
            _executers.Add(executer);
        }
    }
}
