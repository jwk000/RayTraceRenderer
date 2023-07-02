namespace tiny
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ClientSize = new Size(800, 800);
            mScene.Render();
            Invalidate();
        }
        Scene mScene = new Scene();

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mScene.mImage != null)
            {
                e.Graphics.DrawImage(mScene.mImage, 0, 0);
            }
        }
    }
}