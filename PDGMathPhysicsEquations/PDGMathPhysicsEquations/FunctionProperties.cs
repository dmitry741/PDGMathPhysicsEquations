using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDGMathPhysicsEquations
{
    public class FunctionProperties
    {
        Function _f;
        double _low, _high;
        string _functionDescription;

        public FunctionProperties(Function f, double L, string functionDescription)
        {
            _f = f;
            _functionDescription = functionDescription;

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
                    _low = x;
                    break;
                }
            }

            // определяем верхний предел интегрирования
            for (int i = N; i >= 0; i--)
            {
                double x = L / N * i;

                if (_f(x) != 0)
                {
                    _high = x;
                    break;
                }
            }
        }

        public Function f => _f;

        public double Low => _low;
        public double High => _high;

        public double Ymax { get; private set; }

        public override string ToString()
        {
            return _functionDescription;
        }
    }
}
