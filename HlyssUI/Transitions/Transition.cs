using HlyssUI.Components;

namespace HlyssUI.Transitions
{
    public abstract class Transition
    {
        public delegate void FinishHandler();
        FinishHandler OnFinish;

        protected Component component;

        public bool IsFinished { get; private set; }

        public Transition(Component component)
        {
            this.component = component;
        }

        public abstract void Update();

        protected void Finish()
        {
            OnFinish?.Invoke();
            OnFinish = null;

            IsFinished = true;
        }
    }
}
