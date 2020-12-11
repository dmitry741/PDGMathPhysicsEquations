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

        HeatConductivityEquation _heatConductivity;

        const double _cL = 2.0;
        const double _cMaxTemp = 500;
        const double _cTimeStep = 0.025;

        #endregion

        #region === private methods ===

        double Plateau(double x)
        {
            const double cPlateau = _cL / 10;
            double r = 0;

            if (-cPlateau < x && x < cPlateau)
                r = _cMaxTemp;

            return r;
        }

        void FillFunctionPropertiesCombo()
        {
            FunctionProperties functionProperties;

            comboBox1.BeginUpdate();

            functionProperties = new FunctionProperties(Plateau, _cL, "Плато");
            comboBox1.Items.Add(functionProperties);

            comboBox1.SelectedIndex = 0;
            comboBox1.EndUpdate();
        }

        List<Color> CreateHotPalette()
        {
            Palette palette = new Palette();

            palette.AddBaseColor(Color.Black);
            palette.AddBaseColor(Color.DarkBlue);
            palette.AddBaseColor(Color.Red);
            palette.AddBaseColor(Color.Yellow);

            palette.CreatePalette();

            return palette.Colors;
        }

        void Next()
        {
            //_curTime += _cTimeStep;
            //FunctionProperties fp = (FunctionProperties)comboBox1.SelectedItem;

            // TODO
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
            Font font = new Font("Arial", 8.0f, FontStyle.Regular);            

            for (int i = 0; i < markerCount; i++)
            {
                float xf = Convert.ToSingle(_colors.Count * i) / (markerCount - 1) + X;
                g.DrawLine(Pens.Black, xf, Y + cHeight + 4, xf, Y + cHeight + 12);

                int  t = Convert.ToInt32(Convert.ToSingle(_cMaxTemp) / _colors.Count * (xf - X));
                SizeF szf = g.MeasureString(t.ToString(), font);

                g.DrawString(t.ToString(), font, Brushes.Black, xf - szf.Width / 2, Y + cHeight + 14);
            }
        }

        void RenderKernel(Graphics g)
        {
            // TODO:
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
            RenderKernel(g);

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
            _heatConductivity = new HeatConductivityEquation(fp.f, fp.Low, fp.High, 1);

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
    }
}
