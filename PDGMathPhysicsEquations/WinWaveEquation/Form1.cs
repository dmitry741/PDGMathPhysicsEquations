using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathPhysicsEq;

namespace WinWaveEquation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region === members ===

        Bitmap _bitmap;
        Timer _timer;
        RectangleF _chartBoundRect;
        double _curTime;

        WaveEquation _waveEquation = new WaveEquation(4);

        #endregion

        #region === private methods ===

        /*double Trapeze(double L)
        {

        }*/

        PointF ConvertToWin(PointF point, float ymin, float ymax)
        {
            float x = Convert.ToSingle(_chartBoundRect.Width / _waveEquation.L * point.X + _chartBoundRect.X);
            float y = Convert.ToSingle(_chartBoundRect.Height / (ymin - ymax) * (point.Y - ymax) + _chartBoundRect.Y);

            return new PointF(x, y);
        }

        IEnumerable<PointF> GetWave(double time)
        {
            List<PointF> temps = new List<PointF>();
            int N = Convert.ToInt32(_chartBoundRect.Width / 2);

            for (int i = 0; i <= N; i++)
            {
                float x = Convert.ToSingle(_waveEquation.L / N * i);
                float y = Convert.ToSingle(_waveEquation.U(x, time));

                temps.Add(new PointF(x, y));
            }

            float ymin = temps.Min(point => point.Y);
            float ymax = temps.Max(point => point.Y);

            return temps.Select(point => ConvertToWin(point, ymin, ymax));
        }

        void RenderWave(Graphics g, IEnumerable<PointF> points)
        {
            Pen pen = new Pen(Color.DarkBlue, 2.0f);
            g.DrawLines(pen, points.ToArray());
        }

        void Render()
        {
            // проверка картинки на валидность.
            if (_bitmap == null)
                return;

            // создаем новый графический контекст.
            Graphics g = Graphics.FromImage(_bitmap);

            // очищаем контекст.
            g.Clear(Color.White);

            // получаем волну
            var wave = GetWave(_curTime);

            // рисуем волну
            RenderWave(g, wave);

            // отрисовка.
            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int cShiftX = 20;
            int cShiftY = 40;
            _chartBoundRect = new RectangleF(cShiftX, cShiftY, pictureBox1.Width - 2 * cShiftX, pictureBox1.Height - 2 * cShiftY);

            _curTime = 0;

            // создаем таймер
            _timer = new Timer
            {
                Interval = 100,
                Enabled = false
            };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Render();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _timer.Stop();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }
    }
}
