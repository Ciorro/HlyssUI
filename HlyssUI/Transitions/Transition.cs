using HlyssUI.Components;

namespace HlyssUI.Transitions
{
    public abstract class Transition<T>
    {
        public delegate void FinishHandler();
        FinishHandler OnFinish;

        protected T value;

        public bool IsFinished { get; private set; }

        public Transition(T value)
        {
            this.value = value;
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
