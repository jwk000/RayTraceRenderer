using System.Numerics;

namespace RayTraceRenderer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitScene();

            this.DoubleBuffered = true;
            this.ClientSize = new Size(600, 600);
            this.timer.Interval = 33; //帧率30帧
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
            {
                GameObject go = new GameObject();
                AmbientLight light = new AmbientLight();
                light.Intensity = 0.2f;

                Transform tr = new Transform();
                tr.Position = Vector3.Zero;

                go.AddComponent(light);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }
            {
                GameObject go = new GameObject();
                PointLight light = new PointLight();
                light.Intensity = 0.6f;

                Transform tr = new Transform();
                tr.Position = new Vector3(2, 1, 0);

                go.AddComponent(light);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }
            {
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
            {
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1;

                Material mat = new Material();
                mat.Color = Color.Red;
                mat.Specular = 500;

                Transform tr = new Transform();
                tr.Position = new Vector3(0, -1, 3);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }

            {
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1;

                Material mat = new Material();
                mat.Color = Color.LightBlue;
                mat.Specular = 500;

                Transform tr = new Transform();
                tr.Position = new Vector3(2, 0, 4);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }

            {
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1;

                Material mat = new Material();
                mat.Color = Color.LightGreen;
                mat.Specular = 1000;

                Transform tr = new Transform();
                tr.Position = new Vector3(-2, 0, 4);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }

            {
                GameObject go = new GameObject();
                Sphare shap = new Sphare();
                shap.Radius = 1000;

                Material mat = new Material();
                mat.Color = Color.Yellow;
                mat.Specular = 1000;

                Transform tr = new Transform();
                tr.Position = new Vector3(0, -1001, 0);

                go.AddComponent(shap);
                go.AddComponent(mat);
                go.AddComponent(tr);
                mScene.AddObject(go);
            }
            mScene.Awake();
            mScene.Start();

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