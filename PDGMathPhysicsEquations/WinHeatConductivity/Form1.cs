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

namespace WinHeatConductivity
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
        double _curTime;
        List<Color> _colors;
        int[] _indexes;

        HeatConductivityEquation _heatConductivity;

        const double _cL = 2.0;
        const double _cMaxTemp = 500;
        const double _cTimeStep = 0.005;

        #endregion

        #region === private methods ===

        double Plateau(double x)
        {
            const double cPlateau = _cL / 5;
            double r = 0;

            if (-cPlateau < x && x < cPlateau)
                r = _cMaxTemp;

            return r;
        }

        void FillFunctionPropertiesCombo()
        {
            FunctionProperties functionProperties;

            comboBox1.BeginUpdate();

            functionProperties = new FunctionProperties(Plateau, -_cL / 2, _cL / 2, "Плато");
            comboBox1.Items.Add(functionProperties);

            comboBox1.SelectedIndex = 0;
            comboBox1.EndUpdate();
        }

        int KernelLength => pictureBox1.Width - 80;

        List<Color> CreateHotPalette()
        {
            Palette palette = new Palette();

            palette.AddBaseColor(Color.Black);
            palette.AddBaseColor(Color.DarkRed);
            palette.AddBaseColor(Color.Red);
            palette.AddBaseColor(Color.Yellow);

            palette.CreatePalette();

            return palette.Colors;
        }

        void Next()
        {
            _curTime += _cTimeStep;
            ComputeColorIndexes();
        }

        void ComputeColorIndexes()
        {
            for (int i = 0; i < _indexes.Length; i++)
            {
                double x = _cL / (_indexes.Length - 1) * i - 1;
                double u = _heatConductivity.HeatConductivity(x, _curTime);

                _indexes[i] = Convert.ToInt32((_colors.Count - 1) / _cMaxTemp * u);
            }
        }

        void RenderMarker(Graphics g, float x, float y, string text)
        {            
            const int cHeight = 16;
            Font font = new Font("Arial", 8.0f, FontStyle.Regular);
            SizeF szf = g.MeasureString(text, font);

            g.DrawLine(Pens.Black, x, y + cHeight + 4, x, y + cHeight + 12);
            g.DrawString(text, font, Brushes.Black, x - szf.Width / 2, y + cHeight + 14);
        }

        void RenderLegend(Graphics g, int X, int Y)
        {
            const int cHeight = 16;
            int xCur = X;

            foreach (Color color in _colors)
            {
                g.DrawLine(new Pen(color), xCur, Y, xCur, Y + cHeight);
                xCur++;
            }

            int markerCount = 4;            

            for (int i = 0; i < markerCount; i++)
            {
                float xf = Convert.ToSingle(_colors.Count * i) / (markerCount - 1) + X;
                int t = Convert.ToInt32(Convert.ToSingle(_cMaxTemp) / _colors.Count * (xf - X));

                RenderMarker(g, xf, Y, t.ToString());
            }
        }

        void RenderKernel(Graphics g, List<Color> colors, int[] indexes)
        {
            const int cYShift = 20;
            int xCur = (pictureBox1.Width - KernelLength) / 2;

            foreach (int index in indexes)
            {
                g.DrawLine(new Pen(colors[index]), xCur, pictureBox1.Height / 2 + cYShift, xCur, pictureBox1.Height / 2 + cYShift + 24);
                xCur++;
            }
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

            // отрисвока легенды
            RenderLegend(g, (pictureBox1.Width - _colors.Count) / 2, 20);

            // отрисовка стержня
            RenderKernel(g, _colors, _indexes);

            // отрисовка.
            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            FillFunctionPropertiesCombo();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _curTime = 0.001;

            // палитра цветов
            _colors = CreateHotPalette();

            FunctionProperties fp = (FunctionProperties)comboBox1.SelectedItem;
            _heatConductivity = new HeatConductivityEquation(fp.f, fp.Low, fp.High, 0.25);
            _indexes = new int[KernelLength];
            ComputeColorIndexes();

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_timer.Enabled)
                _timer.Stop();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
            Render();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
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
