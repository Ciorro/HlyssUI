using HlyssUI.Utils;

namespace HlyssUI.Controllers.Tweens
{
    abstract class Tween
    {
        public delegate void FinishHandler();
        public FinishHandler OnFinish;

        public readonly string Name;
        public float Duration { get; set; } = 0.3f;

        protected float progress;
        protected float timePassed = 0;
        protected DeltaTime _deltaTime = new DeltaTime();

        protected const byte Power = 6;

        public float Percentage
        {
            get
            {
                return progress;
            }
        }

        public bool IsRunning { get; set; }
        public bool IsFinished { get; private set; }

        public Tween(string name) => Name = name;

        public abstract Tween Get();

        public virtual void Update()
        {
            _deltaTime.Update();
        }

        public virtual void Start()
        {
            IsRunning = true;
            IsFinished = false;
            progress = 0;
            timePassed = 0;
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
