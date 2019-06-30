namespace HlyssUI.Utils
{
    internal static class DeltaTime
    {
        public static float Current
        {
            get
            {
                return _deltaTime / 1000f;
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
