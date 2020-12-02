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

        AbstractMathPhysicsEquation _waveEquation = new WaveEquation(1);

        #endregion

        #region === private methods ===

        void Render()
        {
            // проверка картинки на валидность.
            if (_bitmap == null)
                return;

            // создаем новый графический контекст.
            Graphics g = Graphics.FromImage(_bitmap);

            // очищаем контекст.
            g.Clear(Color.White);

            // TODO:

            // отрисовка.
            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO:

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
