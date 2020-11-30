using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDGMathPhysicsEquations
{
    /// <summary>
    /// Функция одной переменной.
    /// </summary>
    /// <param name="x">Независимый параметр.</param>
    /// <returns>Значение функции.</returns>
    public delegate double Function(double x);

    /// <summary>
    /// Класс для вычисления определенного интеграла.
    /// </summary>
    public class Integral
    {
        /// <summary>
        /// Вычисление определенного интеграла.
        /// </summary>
        /// <param name="f">Функция одной переменной.</param>
        /// <param name="a">Нижний предел интегрирования.</param>
        /// <param name="b">Верхний предел интегрирования.</param>
        /// <returns>Значение интеграла.</returns>
        public static double Get(Function f, double a, double b)
        {
            double N = (b - a) * 100;
            double S = 0;
            double dx = (b - a) / N;

            for (int i = 1; i < N; i++)
            {
                S += f(dx * i + a);
            }

            S += (f(a) + f(b)) / 2;
            S *= dx;

            return S;
        }
    }
}
