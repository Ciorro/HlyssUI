namespace HlyssUI.Utils
{
    internal class DeltaTime
    {
        public float Current
        {
            get
            {
                float dt = _deltaTime / 1000f;
                return (dt > 0.016f) ? 0.016f : dt;
            }
        }

        private long _lastMs = System.Environment.TickCount;
        private float _deltaTime = 0;

        public void Update()
        {
            long currentMs = System.Environment.TickCount;
            _deltaTime = currentMs - _lastMs;
            _lastMs = currentMs;
        }
    }
}
