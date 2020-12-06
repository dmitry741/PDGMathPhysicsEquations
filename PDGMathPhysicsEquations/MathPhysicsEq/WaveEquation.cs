using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace MathPhysicsEq
{
    public class WaveEquation
    {
        double _k, _L, _low, _high, _A;
        Function _f;

        public WaveEquation(Function f, double A, double l, double low, double high)
        {
            _L = l;
            _low = low;
            _high = high;
            _f = f;
            _A = A;
        }

        private double AkUnderIntegralExpression(double x)
        {
            return _f(x) * Math.Sin(_k * Math.PI * x / _L);
        }

        private double Ak(double k)
        {
            _k = k;
            return 2 / _L * Integral.Get(AkUnderIntegralExpression, _low, _high);
        }

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
