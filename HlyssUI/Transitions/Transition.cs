using HlyssUI.Components;
using HlyssUI.Utils;
using System;

namespace HlyssUI.Transitions
{
    public abstract class Transition
    {
        public delegate void FinishHandler();
        public FinishHandler OnFinish;

        private float _progress;

        protected float Percentage
        {
            get
            {
                return _progress / 100f;
            }
        }

        public TransitionEngine Engine;

        public bool IsRunning { get; set; }
        public bool IsFinished { get; private set; }

        public virtual void Update()
        {
            float offset = Math.Abs(100 - _progress);
            float length = offset;

            if (Math.Abs(length) < 1)
            {
                _progress = 100;
                Finish();
                return;
            }

            offset *= DeltaTime.Current * 15;
            _progress += offset;
        }

        public virtual void Start()
        {
            IsRunning = true;
        }

        protected void Finish()
        {
            OnFinish?.Invoke();
            OnFinish = null;

            IsRunning = false;
            IsFinished = true;
        }
    }
}
