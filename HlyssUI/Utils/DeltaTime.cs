namespace HlyssUI.Utils
{
    internal static class DeltaTime
    {
        public static float Current
        {
            get
            {
                float dt = _deltaTime / 1000f;
                return (dt > 0.016f) ? 0.016f : dt;
            }
        }

        private static long _lastMs = System.Environment.TickCount;
        private static float _deltaTime = 0;

        public static void Update()
        {
            long currentMs = System.Environment.TickCount;
            _deltaTime = currentMs - _lastMs;
            _lastMs = currentMs;
        }
    }
}
