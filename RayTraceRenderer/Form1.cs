using System.Numerics;

namespace RayTraceRenderer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitScene2();

            mScene.Awake();
            mScene.Start();

            this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;
            this.timer.Interval = 1000;
            this.timer.Tick += Update;
            this.timer.Start();

        }

        private void Update(object? sender, EventArgs e)
        {
            mScene.Update();
            Invalidate();
        }

        Scene mScene = new Scene();
        void InitScene()
        {
            {//环境光
                GameObject go = new GameObject();
                AmbientLight light = new AmbientLight();
                light.Intensity = 0.2f;

                Transform tr = new Transform();
                tr.Position = Vector3.Zero;

                go.AddComponent(light);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }
            {//点光源
                GameObject go = new GameObject();
                PointLight light = new PointLight();
                light.Intensity = 0.6f;

                Transform tr = new Transform();
                tr.Position = new Vector3(2, 1, 0);

                go.AddComponent(light);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }
            {//平行光
                GameObject go = new GameObject();
                DirectionLight light = new DirectionLight();
                light.Intensity = 0.2f;
                light.Direction = new Vector3(1, 4, 4);

                Transform tr = new Transform();
                tr.Position = Vector3.Zero;

                go.AddComponent(light);
                go.AddComponent(tr);
                mScene.AddObject(go);

            }
            {//红球
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1;

                Material mat = new Material();
                mat.Color = Color.Red;
                mat.Specular = 500;
                mat.Reflective = 0f;

                Transform tr = new Transform();
                tr.Position = new Vector3(0, -1, 3);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }

            {//蓝球
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1;

                Material mat = new Material();
                mat.Color = Color.LightBlue;
                mat.Specular = 500;
                mat.Reflective = 0.1f;

                Transform tr = new Transform();
                tr.Position = new Vector3(2, 0, 4);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                //go.AddComponent(new MoveAround());
                mScene.AddObject(go);
            }

            {//绿球
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1;

                Material mat = new Material();
                mat.Color = Color.LightGreen;
                mat.Specular = 1000;
                mat.Reflective = 0.1f;

                Transform tr = new Transform();
                tr.Position = new Vector3(-2, 0, 4);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }

            {//黄球
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1000;

                Material mat = new Material();
                mat.Color = Color.Yellow;
                mat.Specular = 1000;
                mat.Reflective = 0.5f;

                Transform tr = new Transform();
                tr.Position = new Vector3(0, -1001.5f, 0);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }


        }

        void InitScene2()
        {
            {//环境光
                GameObject go = new GameObject();
                AmbientLight light = new AmbientLight();
                light.Intensity = 0.4f;

                Transform tr = new Transform();
                tr.Position = Vector3.Zero;

                go.AddComponent(light);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }

            {//平行光
                GameObject go = new GameObject();
                DirectionLight light = new DirectionLight();
                light.Intensity = 0.6f;
                light.Direction = new Vector3(0, 4, -3);

                Transform tr = new Transform();
                tr.Position = Vector3.Zero;

                go.AddComponent(light);
                go.AddComponent(tr);
                mScene.AddObject(go);

            }
            {//球
                int count = 5;
                for (int x = 0; x < count; x++)
                {
                    for (int y = 0; y < count; y++)
                    {
                        int r = Math.Max(Math.Abs(x - count / 2), Math.Abs(y - count / 2));
                        CreatBall(Color.Yellow, new Vector3(x - count / 2, y - count / 2, 0), 0.45f, r);
                    }
                }
            }
        }

        private void CreatBall(Color color, Vector3 pos, float radius, float angle)
        {
            GameObject go = new GameObject();
            Sphare shap = new Sphare();
            shap.Radius = radius;

            Material mat = new Material();
            mat.Color = color;
            mat.Specular = 500;
            mat.Reflective = 0f;

            Transform tr = new Transform();
            tr.Position = pos;

            MoveWave mw = new MoveWave();
            mw.Angle = angle;

            go.AddComponent(shap);
            go.AddComponent(mat);
            go.AddComponent(tr);
            go.AddComponent(mw);
            mScene.AddObject(go);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mScene.mImage != null)
            {
                e.Graphics.DrawImage(mScene.mImage, 0, 0);
            }
        }

        //输入事件
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.A)
            {

            }
            else if (e.KeyCode == Keys.S)
            {

            }
            else if (e.KeyCode == Keys.D)
            {

            }
            else if (e.KeyCode == Keys.W)
            {

            }
        }

        //使用鼠标旋转相机
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        //使用滚轮缩放fov
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }

    }
}