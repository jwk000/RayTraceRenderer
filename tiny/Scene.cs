﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace tiny
{
    class Camera
    {
        public const int Cw = 800;//画布宽度
        public const int Ch = 800;//画布高度
        public const int d = 400;//相机看到的最近距离，画布和相机的距离
        public const int f = 9999; //相机看到的最远距离
        public Color BGColor = Color.SkyBlue; //背景色
        public Vector3 O = Vector3.Zero;//相机原点
        public Bitmap Image = new Bitmap(Cw, Ch);//输出画布
        Scene scene;

        public Camera(Scene s)
        {
            scene = s;
        }

        public void Render()
        {
            for (int x = -Cw / 2; x < Cw / 2; x += 1)
            {
                for (int y = -Ch / 2; y < Ch / 2; y += 1)
                {
                    Vector3 D = new Vector3(x, y, d);
                    Color c = scene.TraceRay(O, D, 1, int.MaxValue, 8, false);
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

    abstract class GameObject
    {
        //O是相机原点 D是视口坐标
        public abstract List<float> IntersectRay(Vector3 O, Vector3 D);
        public abstract Vector3 GetNormal(Vector3 P);
        public Material Material { get; set; }
    }
    //球体
    class Sphare : GameObject
    {
        public Vector3 Center;
        public float Radius;

        //球和射线的交点
        //球面方程 (P-C)*(P-C)=R^2 射线方程 P=O+tD
        public override List<float> IntersectRay(Vector3 O, Vector3 D)
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
        //球面上一点的法线
        public override Vector3 GetNormal(Vector3 P)
        {
            return P - Center;
        }
    }
    //平面
    class Plane : GameObject
    {
        public float K;//N*P=K
        public Vector3 Normal;

        //平面和射线交点 
        //射线方程 O+tD=P 平面方程 N*P=K
        //N*O+t(N*D)=K --> t=(K-N*O)/(N*D)
        public override List<float> IntersectRay(Vector3 O, Vector3 D)
        {
            List<float> ret = new List<float>();
            float u = Vector3.Dot(D, Normal);
            if (u == 0)
            {
                return ret;
            }
            float t = (K - Vector3.Dot(O, Normal)) / u;
            if (t < 0)
            {
                return ret;
            }

            ret.Add(t);
            return ret;
        }
        //平面上一点的法线
        public override Vector3 GetNormal(Vector3 P)
        {
            return Normal;
        }
    }

    //光
    class Light
    {
        public float Intensity;//光强
    }
    //环境光
    class AmbientLight : Light
    {

    }
    //点光源
    class PointLight : Light
    {
        public Vector3 Pos;
    }
    //方向光
    class DirectionLight : Light
    {
        public Vector3 Dir;
    }
    //材质
    class Material
    {
        public Color Color;
        public int Specular;//高光系数
        public float Reflective;//反射系数 0没有反射 1完全反射
        public float Refractive;//透光率，折射系数 0没有折射 1完全折射
        public float Refraction;//折射率 真空1 水 1.33 玻璃1.5

        public virtual Color GetColor(Vector3 P)
        {
            return Color;
        }
    }

    class PlaneMaterial : Material
    {
        public int GridSize = 50;
        public Color ColorB = Color.Black;
        public override Color GetColor(Vector3 P)
        {
            int x = GridSize * 100 + (int)P.X;
            int z = GridSize * 100 + (int)P.Z;
            if (x / GridSize % 2 + z / GridSize % 2 == 1)
            {
                return ColorB;
            }
            return Color;
        }
    }
    //场景
    class Scene
    {
        public List<GameObject> mObjects = new List<GameObject>();
        public List<Light> mLights = new List<Light>();
        public Camera mCamera;
        public Bitmap mImage => mCamera.Image;
        Random mRandor = new Random();
        public Scene()
        {
            mCamera = new Camera(this);
            {
                AmbientLight light = new AmbientLight();
                light.Intensity = 0.2f;
                mLights.Add(light);
            }
            {
                PointLight light = new PointLight();
                light.Intensity = 0.6f;
                light.Pos = new Vector3(600, 400, 100);
                mLights.Add(light);
            }
            {
                DirectionLight light = new DirectionLight();
                light.Intensity = 0.2f;
                light.Dir = new Vector3(1, -1, 1);
                mLights.Add(light);
            }
            {//红球只有漫反射高光
                Sphare obj = new Sphare();
                obj.Radius = 150;
                obj.Center = new Vector3(200, -100, 800);

                Material mat = new Material();
                mat.Color = Color.Red;
                mat.Specular = 1000;
                mat.Reflective = 0f;
                mat.Refraction = 1f;
                mat.Refractive = 0f;
                obj.Material = mat;
                mObjects.Add(obj);
            }
            {//蓝球有反射
                Sphare obj = new Sphare();
                obj.Radius = 200;
                obj.Center = new Vector3(-200, -100, 800);

                Material mat = new Material();
                mat.Color = Color.LightBlue;
                mat.Specular = 500;
                mat.Reflective = 0.5f;
                mat.Refraction = 0f;
                mat.Refractive = 0;
                obj.Material = mat;
                mObjects.Add(obj);
            }
            {//白球有折射
                Sphare obj = new Sphare();
                obj.Radius = 100;
                obj.Center = new Vector3(50, -180, 600);

                Material mat = new Material();
                mat.Color = Color.White;
                mat.Specular = 1000;
                mat.Reflective = 0.1f;
                mat.Refraction = 1.8f;
                mat.Refractive = 1f;
                obj.Material = mat;
                mObjects.Add(obj);
            }
            {//黑白棋盘平面有反射
                Plane obj = new Plane();
                obj.K = -300;
                obj.Normal = new Vector3(0, 1, 0);

                PlaneMaterial mat = new PlaneMaterial();
                mat.Color = Color.Yellow;
                mat.ColorB = Color.Black;
                mat.Specular = 1000;
                mat.Reflective = 0.2f;
                mat.Refraction = 0;
                mat.Refractive = 0;
                obj.Material = mat;
                mObjects.Add(obj);
            }
        }

        //渲染
        public void Render()
        {
            mCamera.Render();
        }

        //P是物体表面接收光照的点，N是法线
        float ComputeLighting(Vector3 P, Vector3 N, Vector3 V, Material mat)
        {
            float ret = 0;
            Vector3 L = Vector3.Zero;
            float tmin = 0.001f;
            float tmax = 1;
            foreach (var light in mLights) //处理所有光源
            {
                if (light != null)
                {
                    if (light.GetType() == typeof(AmbientLight))
                    {
                        ret += light.Intensity; //环境光直接加光强
                    }
                    else
                    {
                        if (light.GetType() == typeof(PointLight))
                        {
                            Vector3 pos = (light as PointLight).Pos;//点光源需要算一下光的方向
                            L = pos - P; //光的方向定义为从光源朝向P的方向
                            tmax = 1; //从P到点光源的射线最多延伸到点光源就停下
                        }
                        else if (light.GetType() == typeof(DirectionLight))
                        {
                            L = (light as DirectionLight).Dir;
                            tmax = float.MaxValue;//从P发出的和L方向相反的射线，距离无限远
                        }

                        //阴影检测
                        var shadow_o = ClosestIntersection(P, L, tmin, tmax, out float closest_t);
                        if (shadow_o != null && shadow_o.Material.Refractive == 0)//忽略透明物体
                        {
                            continue; //阴影里的点不受光照
                        }

                        //漫反射
                        float nl = Vector3.Dot(N, L);// 光的方向L和法线N的夹角决定最终的光强
                        if (nl > 0)
                        {
                            ret += light.Intensity * nl / (N.Length() * L.Length());
                        }

                        //高光
                        if (mat.Specular != -1)
                        {
                            Vector3 R = 2 * N * Vector3.Dot(N, L) - L;//反射光线
                            float rv = Vector3.Dot(R, V);
                            if (rv > 0)
                            {
                                ret += light.Intensity * MathF.Pow(rv / (R.Length() * V.Length()), mat.Specular);
                            }
                        }
                    }
                }
            }

            return MathF.Min(1, ret);
        }

        //depth是递归深度
        //inner是在物体内部
        public Color TraceRay(Vector3 O, Vector3 D, float tmin, float tmax, int depth, bool inner)
        {
            GameObject closest_o = ClosestIntersection(O, D, tmin, tmax, out float closest_t);

            if (closest_o == null)
            {
                return mCamera.BGColor;
            }

            Material mat = closest_o.Material;
            Vector3 P = O + closest_t * D; //相机射线和物体的交点
            Vector3 N = closest_o.GetNormal(P);//P点的法线
            N = Vector3.Normalize(N);//归一化
            Vector3 V = -Vector3.Normalize(D);
            float eta = 1 / mat.Refraction;
            if (inner)
            {
                N = -N;
                eta = mat.Refraction;
            }

            Color localcolor = Color.Black;
            if (!inner)
            {
                float I = ComputeLighting(P, N, V, mat);
                Color c = mat.GetColor(P);
                localcolor = Color.FromArgb((int)(c.R * I), (int)(c.G * I), (int)(c.B * I));
            }
            if (depth <= 0)
            {
                return localcolor;
            }

            //反射光
            Color reflect_color = Color.Black;
            float r = mat.Reflective;

            //折射光
            Color refract_color = Color.Black;
            float t = mat.Refractive;

            if (t > 0)
            {
                float s = schlick(V, N, eta);//反射率和折射率和观察角度有关

                r = t * s;
                t = t * (1 - s);


                Vector3 F = RefractRay(V, N, eta);
                if (F != Vector3.Zero) //需要判断有没有发生折射
                {
                    refract_color = TraceRay(P, F, 0.001f, float.MaxValue, depth - 1, !inner);
                }
            }

            if (r > 0)
            {
                Vector3 R = ReflectRay(V, N);
                reflect_color = TraceRay(P, R, 0.001f, float.MaxValue, depth - 1, inner);
            }
            float v = MathF.Max(0, 1 - r - t);
            float cr = localcolor.R * v + refract_color.R * t + reflect_color.R * r;
            float cg = localcolor.G * v + refract_color.G * t + reflect_color.G * r;
            float cb = localcolor.B * v + refract_color.B * t + reflect_color.B * r;
            Color cc = Color.FromArgb((int)MathF.Min(255, cr), (int)MathF.Min(255, cg), (int)MathF.Min(255, cb));

            return cc;
        }

        GameObject ClosestIntersection(Vector3 O, Vector3 D, float tmin, float tmax, out float closest_t)
        {
            closest_t = tmax;
            GameObject closest_o = null;

            foreach (var obj in mObjects)
            {
                if (obj != null)
                {
                    var list = obj.IntersectRay(O, D);
                    if (list.Count > 0)
                    {
                        foreach (float t in list)
                        {
                            if (t > tmin && t < tmax && t < closest_t)
                            {
                                closest_t = t;
                                closest_o = obj;
                            }
                        }
                    }
                }
            }
            return closest_o;
        }

        //V是观察者视线方向，N是法线，返回反射光方向
        Vector3 ReflectRay(Vector3 V, Vector3 N)
        {
            return 2 * N * Vector3.Dot(N, V) - V;
        }

        //V是视线方向，N是法线，r是折射率，返回折射后的射线方向
        // eta_a*sin_a=eta_b*sin_b
        Vector3 RefractRay(Vector3 V, Vector3 N, float eta)
        {
            float cos_a = Vector3.Dot(V, N);
            float sin_2_a = 1 - cos_a * cos_a;
            float cos_2_b = 1 - (sin_2_a) * (eta * eta);
            if (cos_2_b < 0)
            {
                return Vector3.Zero;
            }
            Vector3 Rp = eta * (cos_a * N - V);
            Vector3 Rn = MathF.Sqrt(cos_2_b) * (-N);
            Vector3 R = Rp + Rn;
            return R;
        }

        //fresnel方程近似，schlick方程
        //计算光线的反射光线能量占入射光线能量的比例，反射率（根据能量守恒，透射率=1-反射率）
        float schlick(Vector3 V, Vector3 N, float eta)
        {
            V = Vector3.Normalize(V);
            N = Vector3.Normalize(N);
            float f0 = (eta - 1.0f) / (eta + 1.0f);
            f0 *= f0;
            float cos_theta = Vector3.Dot(V, N);
            if (cos_theta < 0)
            {
                return 1;
            }
            if (eta > 1.0)
            {
                float cos2_t = 1.0f - eta * eta * (1.0f - cos_theta * cos_theta);
                if (cos2_t < 0.0) return 1.0f;
                cos_theta = MathF.Sqrt(cos2_t);
            }

            float x = 1.0f - cos_theta;

            float x2 = x * x;
            float x5 = x2 * x2 * x;

            return f0 + (1.0f - f0) * x5;
        }

    }
}
