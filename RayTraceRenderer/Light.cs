using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTraceRenderer
{
    internal interface ILight
    {
        float Intensity { get; set; }
    }

    internal class ALight : AComponent,ILight
    {
        public float Intensity { get; set; }
    }
    internal class AmbientLight :ALight
    {
       
    }

    internal class PointLight : ALight
    {
       
    }

    internal class DirectionLight:ALight
    {
        public Vector3 Direction;
    }
}
