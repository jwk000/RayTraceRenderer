using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceRenderer
{
    internal class Material:AComponent
    {
        public Color Color;
        public int Specular;//高光系数
        public float Reflective;//反射系数 0没有反射 1完全反射
    }
}
