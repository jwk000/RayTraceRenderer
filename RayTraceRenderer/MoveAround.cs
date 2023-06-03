using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceRenderer
{
    internal class MoveAround:AComponent
    {
        Transform mTransform = null;
        float mSpeed = 0.2f;//弧度每秒
        //float mRaduis = 2f;

        public override void Start()
        {
            mTransform = gameObject.GetComponent<Transform>();
        }
        public override void Update()
        {
            float theata = mSpeed ;
            //围绕中心点旋转
            mTransform.Position.X = mTransform.Position.X * MathF.Cos(theata) - mTransform.Position.Z * MathF.Sin(theata);
            mTransform.Position.Z = mTransform.Position.X * MathF.Sin(theata) + mTransform.Position.Z * MathF.Cos(theata);
        }
    }
}
