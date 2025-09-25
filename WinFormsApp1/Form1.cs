namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Image? mainPic;
        private List<SnowFlake> snowFlakesList = new();
        private System.Windows.Forms.Timer? timer;
        private System.Windows.Forms.Timer? spawnSnowFlakeTimer;
        private Image snowFlakeImage;
        private Random random;
        private const int MaxSnowFlakes = 150;
        private int currentSnowFlakesCount = 0;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            snowFlakeImage = Properties.Resources.SnowFlake;
            InitTimer();
            InitSpawnerTimer();
            DoubleBuffered = true;
            mainPic = PickMainPicture();
            random = new Random();
        }

        private void InitSpawnerTimer()
        {
            spawnSnowFlakeTimer = new System.Windows.Forms.Timer();
            spawnSnowFlakeTimer.Interval = 500;
            spawnSnowFlakeTimer.Tick += SpawnTimer_Tick;
            spawnSnowFlakeTimer.Start();
        }

        private void SpawnTimer_Tick(object? sender, EventArgs e)
        {
            if (currentSnowFlakesCount < MaxSnowFlakes)
            {
                AddNewSnowFlake();
                currentSnowFlakesCount++;
            }
            else
            {
                spawnSnowFlakeTimer.Stop();
            }
        }

        private void AddNewSnowFlake()
        {
            snowFlakesList.Add(new SnowFlake
            {
                X = random.Next(0, Width),
                Y = random.Next(-100, 0),
                SizeType = random.Next(0, 2) == 0 ? SnowFlakeSize.Small : SnowFlakeSize.Large
            });
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
                float speed = flake.SizeType == SnowFlakeSize.Large ? 0.8f : 1.2f;
                flake.Y += speed;
                flake.X += (float)(Math.Sin(flake.Y * 0.01) * 0.3);
                if (flake.Y > Height)
                {
                    flake.Y = random.Next(-50, -10);
                    flake.X = random.Next(Width);
                }
            }
            Invalidate();
        }

        /// <summary>
        /// Метод выбора фоновой картинки по времени
        /// </summary>
        /// <returns>Image</returns>
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
                int width, height;
                if (flake.SizeType == SnowFlakeSize.Small)
                {
                    width = 30;
                    height = 20;
                }
                else
                {
                    width = 100;
                    height = 70;
                }

                g.DrawImage(snowFlakeImage, new Rectangle((int)flake.X, (int)flake.Y, width, height));
            }

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e) => Close();

    }
}
