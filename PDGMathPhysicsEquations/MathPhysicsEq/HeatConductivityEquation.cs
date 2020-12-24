using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace MathPhysicsEq
{
    /// <summary>
    /// Класс-решение для уравнения теплопроводности.
    /// </summary>
    public class HeatConductivityEquation
    {
        readonly double _low, _high, _A;
        readonly Function _f;
        double _x, _t;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="f">Функция вида y=f(x), первоначальное распределение тепла в стержне.</param>
        /// <param name="low">Нижний предел интегрирования.</param>
        /// <param name="high">Верхний предел интегрирования.</param>
        /// <param name="A">Параметр уравнения теплопроводности.</param>
        public HeatConductivityEquation(Function f, double low, double high, double A)
        {
            _f = f;
            _low = low;
            _high = high;
            _A = A;
        }

        /// <summary>
        /// Подинтегральная функция для уравнения теплопроводности.
        /// </summary>
        /// <param name="s">Переменная для интегрирования.</param>
        /// <returns>Значение подинтегральной функции в точке s.</returns>
        private double UnderIntegralExpression(double s)
        {
            double arge = -(_x - s) * (_x - s) / (4 * _A * _A * _t);
            return _f(s) * Math.Exp(arge);
        }

        /// <summary>
        /// Вычисление значения функции решения уравнения теплопроводности при заданных x и t.
        /// </summary>
        /// <param name="x">Независимая переменная.</param>
        /// <param name="t">Время.</param>
        /// <returns>Значения функции решения уравнения теплопроводности при заданных x и t.</returns>
        public double HeatConductivity(double x, double t)
        {
            double m = 1.0 / (2 * _A * Math.Sqrt(Math.PI * t));
            _x = x;
            _t = t;

            return m * Integral.Get(UnderIntegralExpression, _low, _high, 800);
        }
    }
}
