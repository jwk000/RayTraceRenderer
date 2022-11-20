using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTraceRenderer
{
    internal class Transform:AComponent
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
    }
}
