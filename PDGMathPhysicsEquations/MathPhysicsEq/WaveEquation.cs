using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace MathPhysicsEq
{
    /// <summary>
    /// Класс-решение для волнового уравнения.
    /// </summary>
    public class WaveEquation
    {
        readonly double _L, _low, _high, _A;
        readonly Function _f;
        double _k;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="f">Функция, первоначальное положение волны y=f(x)</param>
        /// <param name="A">Параметр волновго уравнения.</param>
        /// <param name="l">Длина струны.</param>
        /// <param name="low">Нижний предел интегрирования.</param>
        /// <param name="high">Верхний предел интегрирования.</param>
        public WaveEquation(Function f, double A, double l, double low, double high)
        {
            _L = l;
            _low = low;
            _high = high;
            _f = f;
            _A = A;
        }

        /// <summary>
        /// Подинтегральное выражение для вычисления Ак.
        /// </summary>
        /// <param name="x">Независимая переменная.</param>
        /// <returns>Значение подинтегрального выражения в точке x.</returns>
        private double AkUnderIntegralExpression(double x)
        {
            return _f(x) * Math.Sin(_k * Math.PI * x / _L);
        }

        /// <summary>
        /// Вычисление коэффициента Ak.
        /// </summary>
        /// <param name="k">k</param>
        /// <returns>Значение коэффициента Ак.</returns>
        private double Ak(double k)
        {
            _k = k;
            return 2 / _L * Integral.Get(AkUnderIntegralExpression, _low, _high);
        }

        /// <summary>
        /// Значение функции решения волнового уравнения в при x и t.
        /// </summary>
        /// <param name="x">Независимая переменная.</param>
        /// <param name="t">Время.</param>
        /// <param name="N">Количество членов в разложении.</param>
        /// <returns>Значение функции решения волнового уравнения.</returns>
        public double Wave(double x, double t, int N = 100)
        {
            double u = 0;
            double pi = Math.PI;

            for (int k = 1; k <= N; k++)
            {
                u += Ak(k) * Math.Cos(k * pi * _A * t / _L) * Math.Sin(k * pi * x / _L);
            }

            return u;
        }
    }
}
