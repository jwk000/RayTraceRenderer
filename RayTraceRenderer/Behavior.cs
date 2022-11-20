using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceRenderer
{


    internal class Behavior
    {
        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Destroy() { }

    }
}
