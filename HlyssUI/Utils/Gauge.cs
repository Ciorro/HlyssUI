using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HlyssUI.Utils
{
    static class Gauge
    {
        private static Dictionary<string, Stopwatch> _measurements = new Dictionary<string, Stopwatch>();
        private static string _lastStarted = string.Empty;

        public static void StartMeasurement(string name, bool restart = false)
        {
            if (_measurements.ContainsKey(name))
            {
                if (restart)
                    _measurements[name].Reset();
                
                _measurements[name].Start();
            }
            else
            {
                _measurements.Add(name, Stopwatch.StartNew());
            }

            _lastStarted = name;
        }

        public static void PauseMeasurement(string name)
        {
            if (_measurements.ContainsKey(name))
                _measurements[name].Stop();
        }

        public static void PauseAndStartNewMeasurement(string name, bool restart = false)
        {
            PauseMeasurement(_lastStarted);
            StartMeasurement(name, restart);
        }

        public static void ResetMeasurement(string name)
        {
            if (_measurements.ContainsKey(name))
                _measurements[name].Reset();
        }

        public static void ResetAllMeasurements()
        {
            foreach (var measurement in _measurements)
            {
                measurement.Value.Reset();
            }
        }

        public static TimeSpan GetElapsedTime(string name)
        {
            if (_measurements.ContainsKey(name))
                return _measurements[name].Elapsed;

            return new TimeSpan(0);
        }

        public static void PrintSummary(params string[] names)
        {
            if (names.Length == 0)
                names = _measurements.Keys.ToArray();

            long totalElapsedTicks = 0;

            for (int i = 0; i < names.Length; i++)
            {
                totalElapsedTicks += _measurements[names[i]].ElapsedTicks;
            }

            for (int i = 0; i < names.Length; i++)
            {
                long elapsedMs = _measurements[names[i]].ElapsedMilliseconds;
                long elapsedTicks = _measurements[names[i]].ElapsedTicks;

                Console.WriteLine($"{names[i]}: {elapsedMs}ms ({((float)elapsedTicks / totalElapsedTicks) * 100}%)");
            }
        }
    }
}
