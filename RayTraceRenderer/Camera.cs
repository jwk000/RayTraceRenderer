using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTraceRenderer
{
    internal class Camera
    {
        public const int Cw = 600;//画布宽度
        public const int Ch = 600;//画布高度
        public const int Vw = 2;//视口宽度
        public const int Vh = 2;//视口高度
        public const int d = 1;//画布和视口的距离
        public const int f = 999999;
        Vector3 O = Vector3.Zero;//相机原点

        public Bitmap Image = new Bitmap(Cw, Ch);
        Scene scene;

        public Color BGColor = Color.LightSkyBlue; //背景色

        public Camera(Scene s)
        {
            scene = s;
        }
        //画布像素C对应的视口坐标V
        Vector3 CanvasToViewPort(float Cx, float Cy)
        {
            float x = Cx * Vw * 1.0f / Cw;
            float y = Cy * Vh * 1.0f / Ch;
            Vector3 v = new Vector3(x, y, d);
            return v;
        }


        public void Render()
        {
            for (int x = -Cw / 2; x < Cw / 2; x += 1)
            {
                for (int y = -Ch / 2; y < Ch / 2; y += 1)
                {
                    Vector3 D = CanvasToViewPort(x, y);
                    Color c = scene.TraceRay(O, D, d, f,1);
                    SetPixel(x, y, c);
                }
            }
        }



        void SetPixel(int x, int y, Color color)
        {
            x = Cw / 2 + x;
            y = Ch / 2 - y;
            if (x < 0 || x >= Cw || y < 0 || y >= Ch)
            {
                return;
            }
            Image.SetPixel(x, y, color);
        }
    }
}
