using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDGMathPhysicsEquations
{
    public class FunctionProperties
    {
        readonly Function _f;
        readonly double _low, _high;
        readonly string _functionDescription;

        public FunctionProperties(Function f, double a, double b, string functionDescription)
        {
            _f = f;
            _functionDescription = functionDescription;

            const int N = 1000;
            double max = 0;

            // определяем максимальное по модулю значение функции f.
            for (int i = 0; i <= N; i++)
            {
                double x = (b - a) / N * i + a;
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
                double x = (b - a) / N * i + a;

                if (_f(x) != 0)
                {
                    _low = x;
                    break;
                }
            }

            // определяем верхний предел интегрирования
            for (int i = N; i >= 0; i--)
            {
                double x = (b - a) / N * i + a;

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
