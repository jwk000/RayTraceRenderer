using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTraceRenderer
{

    //球体
    internal class Sphare : AShape, IShape
    {
        public Vector3 Center { get { return gameObject.GetComponent<Transform>().Position; } }
        public float Radius;

        //球和射线的交点
        public override List<float> IntersectRay(Vector3 O,Vector3 D)
        {
            List<float> ret = new List<float>();
            Vector3 CO = O - Center;

            float a = Vector3.Dot(D, D);
            float b = 2 * Vector3.Dot(CO, D);
            float c = Vector3.Dot(CO, CO) - Radius * Radius;

            float t = b * b - 4 * a * c;
            if (t < 0)
            {
                return ret;
            }

            float t1 = (-b + MathF.Sqrt(t)) / (2 * a);
            float t2 = (-b - MathF.Sqrt(t)) / (2 * a);

            ret.Add(t1);
            ret.Add(t2);
            return ret;
        }


    }
}
