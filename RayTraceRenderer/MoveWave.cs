using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceRenderer
{
    internal class MoveWave : AComponent
    {
        Transform mTransform = null;
        public float Speed = 0.1f;//弧度每秒
        public float Angle = 0;
        public float fixSize = 0.5f;
        public float ZOffset = 8;
        public override void Start()
        {
            base.Start();
            mTransform = gameObject.GetComponent<Transform>();
            mTransform.Position.Z = ZOffset+ MathF.Sin(Angle)* fixSize;
        }

        public override void Update()
        {
            base.Update();
            Angle += Speed;
            mTransform.Position.Z = ZOffset+ MathF.Sin(Angle)* fixSize;

        }
    }
}
