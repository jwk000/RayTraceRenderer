﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTraceRenderer
{
    internal class Scene
    {
        //场景里的物体
        public List<GameObject> mObjects = new List<GameObject>();
        //摄像机
        public Camera mCamera;


        public Bitmap mImage;

        public Scene()
        {
            mCamera = new Camera(this);
        }


        public void AddObject(GameObject obj)
        {
            mObjects.Add(obj);
        }

        //渲染物体
        public void Render()
        {
            mCamera.Render();
            mImage = mCamera.Image;
            SaveImage();
        }

        //保存图片
        void SaveImage()
        {
            //图片名字格式：年-月-日-时-分-秒
            string name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            mImage.Save($"image/{name}.png");
        }

        //P是物体表面接收光照的点，N是法线
        float ComputeLighting(Vector3 P, Vector3 N, Vector3 V, int s)
        {
            float ret = 0;
            Vector3 L = Vector3.Zero;
            float tmin = 0.001f;
            float tmax = 1;
            foreach (var obj in mObjects)
            {
                ILight light = obj.GetComponent<ALight>();
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
                            Vector3 pos = obj.GetComponent<Transform>().Position;//点光源需要算一下光的方向
                            L = pos - P; //光的方向定义为从光源朝向P的方向
                            tmax = 1; //从P到点光源的射线最多延伸到点光源就停下
                        }
                        else if (light.GetType() == typeof(DirectionLight))
                        {
                            L = (light as DirectionLight).Direction;
                            tmax = float.MaxValue;//从P发出的和L方向相反的射线，距离无限远
                        }

                        //阴影检测
                        var shadow_o = ClosestIntersection(P, L, tmin, tmax, out float closest_t);
                        if (shadow_o != null)
                        {
                            continue; //阴影里的点不受光照
                        }

                        //漫反射
                        float nl = Vector3.Dot(N, L);// 光的方向L和法线N的夹角决定最终的光强
                        if (nl > 0)
                        {
                            ret += light.Intensity * nl / (N.Length() * L.Length());
                        }

                        //镜面反射
                        if (s != -1)
                        {
                            Vector3 R = 2 * N * Vector3.Dot(N, L) - L;//反射光线
                            float rv = Vector3.Dot(R, V);
                            if (rv > 0)
                            {
                                ret += light.Intensity * MathF.Pow(rv / (R.Length() * V.Length()), s);
                            }
                        }
                    }
                }
            }

            return MathF.Min(1, ret);
        }

        public Color TraceRay(Vector3 O, Vector3 D, float tmin, float tmax, int depth)
        {
            GameObject closest_o = ClosestIntersection(O, D, tmin, tmax, out float closest_t);

            if (closest_o == null)
            {
                return mCamera.BGColor;
            }

            Material mat = closest_o.GetComponent<Material>();
            Vector3 P = O + closest_t * D; //相机射线和物体的交点
            Vector3 N = P - closest_o.GetComponent<Transform>().Position;//P点的法线
            N = Vector3.Normalize(N);//归一化
            float I = ComputeLighting(P, N, -D, mat.Specular);
            Color c = mat.Color;
            Color localcolor = Color.FromArgb((int)(c.R * I), (int)(c.G * I), (int)(c.B * I));

            float r = mat.Reflective;
            if (depth <= 0 || r <= 0)
            {
                return localcolor;
            }

            Vector3 R = ReflectRay(-D, N);
            Color reflect_color = TraceRay(P, R, 0.001f, float.MaxValue, depth - 1);
            Color cc = Color.FromArgb(
                (int)(localcolor.R * (1 - r) + reflect_color.R * r), 
                (int)(localcolor.G * (1 - r) + reflect_color.G * r),
                (int)(localcolor.B * (1 - r) + reflect_color.B * r));

            return cc;
        }

        GameObject ClosestIntersection(Vector3 O, Vector3 D, float tmin, float tmax, out float closest_t)
        {
            closest_t = tmax;
            GameObject closest_o = null;

            foreach (var obj in mObjects)
            {
                IShape shap = obj.GetComponent<AShape>();
                if (shap != null)
                {
                    var list = shap.IntersectRay(O, D);
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

        //R是物体表面反射的光线，N是
        Vector3 ReflectRay(Vector3 R, Vector3 N)
        {
            return 2 * N * Vector3.Dot(N, R) - R;
        }

        public void Awake()
        {
            foreach (var obj in mObjects)
            {
                obj.Awake();
            }
        }


        public void Start()
        {
            foreach (var obj in mObjects)
            {
                obj.Start();
            }
        }


        public void Update()
        {
            foreach (var obj in mObjects)
            {
                obj.Update();
            }

            Render();
        }

        public void Destroy()
        {
            foreach (var obj in mObjects)
            {
                obj.Destroy();
            }
        }
    }
}
