using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTraceRenderer
{
    interface IShape
    {
        List<float> IntersectRay(Vector3 O, Vector3 D);

    }

    abstract class AShape :AComponent, IShape
    {
        public abstract List<float> IntersectRay(Vector3 O, Vector3 D);
    }
}
