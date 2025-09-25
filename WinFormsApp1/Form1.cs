
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Image? mainPic;
        private List<SnowFlake> snowFlakesList = new();
        private System.Windows.Forms.Timer? timer;
        private Image snowFlakeImage;
        private Random random;
        private float baseSpeed = 1.0f;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            snowFlakeImage = Properties.Resources.SnowFlake;
            CreateSnowFlakes(150);
            InitTimer();
            DoubleBuffered = true;
            mainPic = PickMainPicture();
        }

        private void InitTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            foreach (var flake in snowFlakesList)
            {
                flake.Y += baseSpeed;
                flake.X += Convert.ToSingle((Math.Sin(flake.Y * 0.01) * 0.3));
                if (flake.Y > Height)
                {
                    flake.Y = random.Next(-50, -10);
                    flake.X = random.Next(Width);
                }
            }
            //Invalidate();
            Refresh();
        }

        public Image PickMainPicture()
        {
            DateTime timeNow = DateTime.Now;
            return (timeNow.Hour >= 7 && timeNow.Hour < 18)
            ? Properties.Resources.DayDerevnya
            : Properties.Resources.NightDerevnya;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(mainPic, ClientRectangle);
            foreach (var flake in snowFlakesList)
            {
                g.DrawImage(snowFlakeImage,
             new Rectangle((int)flake.X, (int)flake.Y, 30, 20));
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e) => Close();

        private void CreateSnowFlakes(int count)
        {
            random = new Random();
            for (int i = 0; i < count; i++)
            {
                snowFlakesList.Add(new SnowFlake
                {
                    X = random.Next(0, Width + 1000),
                    Y = random.Next(-100, 0),
                });
            }
        }
    }
}
