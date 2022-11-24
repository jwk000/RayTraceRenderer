using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceRenderer
{
    internal class Timer
    {
        public static float Delta;

        static Stopwatch sw = new Stopwatch();
        public static void Update()
        {
            Delta = sw.ElapsedMilliseconds / 1000f;
            sw.Restart();
        }
    }
}
