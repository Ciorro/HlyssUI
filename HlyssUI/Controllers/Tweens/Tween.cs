using HlyssUI.Utils;
using System;

namespace HlyssUI.Controllers.Tweens
{
    abstract class Tween
    {
        public delegate void FinishHandler();
        public FinishHandler OnFinish;

        public readonly string Name;
        public float Speed { get; set; } = 20f;

        protected float progress;

        public float Percentage
        {
            get
            {
                return progress / 100f;
            }
        }

        public bool IsRunning { get; set; }
        public bool IsFinished { get; private set; }

        public Tween(string name) => Name = name;

        public abstract Tween Get();
        public abstract void Update();

        public virtual void Start()
        {
            IsRunning = true;
            IsFinished = false;
            progress = 0;
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
