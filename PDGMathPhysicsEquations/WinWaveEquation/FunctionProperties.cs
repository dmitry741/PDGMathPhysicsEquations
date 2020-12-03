using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace WinWaveEquation
{
    class FunctionProperties
    {
        Function _f;
        double _a, _b;

        public FunctionProperties(Function f, double L)
        {
            _f = f;

            const int N = 1000;
            double max = 0;

            // определяем максимальное по модулю значение функции f.
            for (int i = 0; i <= N; i++)
            {
                double x = L / N * i;
                double y = Math.Abs(_f(x));

                if (y > max)
                {
                    max = y;
                }
            }

            Ymax = max;

            // определяем нижний предел интегрирования
            for (int i = 0; i <= N; i++)
            {
                double x = L / N * i;

                if (_f(x) != 0)
                {
                    _a = x;
                    break;
                }
            }

            // определяем верхний предел интегрирования
            for (int i = N; i >= 0; i--)
            {
                double x = L / N * i;

                if (_f(x) != 0)
                {
                    _b = x;
                    break;
                }
            }            
        }

        public Function f => _f;

        public double A => _a;
        public double B => _b;

        public double Ymax { get; private set; }
    }
}
