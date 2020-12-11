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
using PDGMathPhysicsEquations;

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

        WaveEquation _waveEquation;
        IEnumerable<PointF> _wave = null;

        const double _cL = 2.0;
        const double _cTimeStep = 0.025;

        #endregion

        #region === private methods ===

        double Trapeze(double x)
        {
            double pie = _cL / 20.0;
            double r = 0;

            if (8 * pie < x && x <= 9 * pie)
            {
                r = x - 8 * pie;
            }
            else if (9 * pie < x && x <= 11 * pie)
            {
                r = pie;
            }
            else if (11 * pie < x && x <= 12 * pie)
            {
                r = 12 * pie - x;
            }

            return r;
        }

        double Zigzag(double x)
        {
            double pie = _cL / 20.0;
            double r = 0;

            if (7 * pie < x && x <= 8 * pie)
            {
                r = x - 7 * pie;
            }
            else if (8 * pie < x && x <= 9 * pie)
            {
                r = pie;
            }
            else if (9 * pie < x && x <= 11 * pie)
            {
                r = 10 * pie - x;
            }
            else if (11 * pie < x && x <= 12 * pie)
            {
                r = -pie;
            }
            else if (12 * pie < x && x <= 13 * pie)
            {
                r = x - 13 * pie;
            }

            return r;
        }

        double TwoWaves(double x)
        {
            double pie = _cL / 20.0;
            double r = 0;

            if (4 * pie < x && x <= 5 * pie)
            {
                r = x - 4 * pie;
            }
            else if (5 * pie < x && x <= 6 * pie)
            {
                r = 6 * pie - x;
            }
            else if (14 * pie < x && x <= 15 * pie)
            {
                r = 2 * (x - 14 * pie);
            }
            else if (15 * pie < x && x <= 16 * pie)
            {
                r = 2 * (16 * pie - x);
            }

            return r;
        }

        void Next()
        {
            _curTime += _cTimeStep;
            FunctionProperties fp = (FunctionProperties)comboBox1.SelectedItem;
            _wave = GetWave(_curTime, fp);
        }

        void FillFunctionPropertiesCombo()
        {
            FunctionProperties functionProperties;

            comboBox1.BeginUpdate();

            functionProperties = new FunctionProperties(Trapeze, 0, _cL, "Трапеция");
            comboBox1.Items.Add(functionProperties);

            functionProperties = new FunctionProperties(Zigzag, 0, _cL, "Зигзаг");
            comboBox1.Items.Add(functionProperties);

            functionProperties = new FunctionProperties(TwoWaves, 0, _cL, "Две волны");
            comboBox1.Items.Add(functionProperties);

            comboBox1.SelectedIndex = 0;
            comboBox1.EndUpdate();
        }

        PointF ConvertToWin(PointF point, float ymin, float ymax)
        {
            float x = Convert.ToSingle(_chartBoundRect.Width / _cL * point.X + _chartBoundRect.X);
            float y = Convert.ToSingle(_chartBoundRect.Height / (ymin - ymax) * (point.Y - ymax) + _chartBoundRect.Y);

            return new PointF(x, y);
        }

        IEnumerable<PointF> GetWave(double time, FunctionProperties functionProperties)
        {            
            int N = Convert.ToInt32(_chartBoundRect.Width / 2);
            PointF[] temps = new PointF[N + 1];

            for (int i = 0; i <= N; i++)
            {
                float x = Convert.ToSingle(_cL / N * i);
                float y = Convert.ToSingle(_waveEquation.Wave(x, time));

                temps[i] = new PointF(x, y);
            }

            float max = Convert.ToSingle(functionProperties.Ymax);

            return temps.Select(point => ConvertToWin(point, -max, max));
        }

        void RenderWave(Graphics g, IEnumerable<PointF> points)
        {
            if (_wave == null)
                return;

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

            // рисуем волну
            RenderWave(g, _wave);

            // отрисовка.
            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            FillFunctionPropertiesCombo();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int cShiftX = 20;
            int cShiftY = 80;
            _chartBoundRect = new RectangleF(cShiftX, cShiftY, pictureBox1.Width - 2 * cShiftX, pictureBox1.Height - 2 * cShiftY);

            _curTime = 0;

            FunctionProperties fp = (FunctionProperties)comboBox1.SelectedItem;
            _waveEquation = new WaveEquation(fp.f, 1, _cL, fp.Low, fp.High);

            // получаем волну
            _wave = GetWave(_curTime, fp);

            // создаем таймер
            _timer = new Timer
            {
                Interval = 500,
                Enabled = false
            };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Next();
            Render();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_timer.Enabled) 
                _timer.Stop();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void OnNextStep(object sender, EventArgs e)
        {
            Next();
            Render();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_waveEquation == null)
                return;

            _curTime = 0;
            FunctionProperties fp = (FunctionProperties)comboBox1.SelectedItem;

            _waveEquation = new WaveEquation(fp.f, 1, _cL, fp.Low, fp.High);
            _wave = GetWave(_curTime, fp);

            Render();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                btnPlay.Text = "Старт";
            }
            else
            {
                _timer.Start();
                btnPlay.Text = "Стоп";
            }
        }
    }
}
