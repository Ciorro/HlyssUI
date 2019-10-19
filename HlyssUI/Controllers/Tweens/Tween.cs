namespace HlyssUI.Controllers.Tweens
{
    abstract class Tween
    {
        public delegate void FinishHandler();
        public FinishHandler OnFinish;

        public readonly string Name;
        public float Duration { get; set; } = 0.5f;

        protected float progress;
        protected float timePassed = 0;

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
        public abstract void Update();

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
