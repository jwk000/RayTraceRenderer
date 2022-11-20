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
        public int Specular;//高光反射系数
    }
}
