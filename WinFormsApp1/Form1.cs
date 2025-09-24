namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Image? mainPic;
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        public Image PickMainPicture()
        {
            DateTime timeNow = DateTime.Now;
            return (timeNow.Hour >= 6 && timeNow.Hour < 18)
            ? Properties.Resources.DayDerevnya
            : Properties.Resources.NightDerevnya;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            mainPic = PickMainPicture();
            g.DrawImage(mainPic, ClientRectangle);
        }
    }
}
