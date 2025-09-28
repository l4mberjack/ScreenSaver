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
        private const int MaxSnowFlakes = 100;
        private int currentSnowFlakesCount = 0;
        Image? scene;

        /// <summary>
        /// Главная форма
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            snowFlakeImage = Properties.Resources.SnowFlake;
            InitTimer();
            InitSpawnerTimer();
            mainPic = PickMainPicture();
            random = new Random();
            scene = new Bitmap(1, 1);
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
                spawnSnowFlakeTimer?.Stop();
            }
        }

        private void AddNewSnowFlake()
        {
            snowFlakesList.Add(new SnowFlake
            {
                X = random.Next(50, Width - 100),
                Y = random.Next(-100, 0),
                SizeType = random.Next(0, 2) == 0 ? SnowFlakeSize.Small : SnowFlakeSize.Large
            });
        }
        private void InitTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 80;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            foreach (var flake in snowFlakesList)
            {
                float speed = flake.SizeType == SnowFlakeSize.Large ? 4.0f : 2.0f;
                flake.Y += speed;
                flake.X += (float)(Math.Sin(flake.Y * 0.02) * 0.3);
                if (flake.Y > Height)
                {
                    flake.Y = random.Next(-50, -10);
                    flake.X = random.Next(Width);
                }
            }

            Form1_Paint(this, new PaintEventArgs(CreateGraphics(), ClientRectangle));
        }

        /// <summary>
        /// Метод выбора фоновой картинки по времени
        /// </summary>
        public Image PickMainPicture()
        {
            DateTime timeNow = DateTime.Now;
            return (timeNow.Hour >= 7 && timeNow.Hour < 18)
            ? Properties.Resources.DayDerevnya
            : Properties.Resources.NightDerevnya;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (scene == null || scene.Width != Width || scene.Height != Height)
            {
                scene.Dispose();
                scene = new Bitmap(Width, Height);
            }

            var bg = Graphics.FromImage(scene);
            bg.DrawImage(mainPic, 0, 0, Width, Height);

            foreach (var flake in snowFlakesList)
            {
                int width = flake.SizeType == SnowFlakeSize.Small ? 30 : 100;
                int height = flake.SizeType == SnowFlakeSize.Small ? 20 : 70;

                bg.DrawImage(snowFlakeImage, new Rectangle((int)flake.X, (int)flake.Y, width, height));
            }

            e.Graphics.DrawImage(scene, Point.Empty);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            timer?.Stop();
            Close();
        }
    }
}
