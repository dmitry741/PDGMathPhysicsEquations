using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace MathPhysicsEq
{
    public class WaveEquation : AbstractMathPhysicsEquation
    {
        double _k;

        public WaveEquation(double l)
        {
            L = l;
        }

        private double AkUnderIntegralExpression(double x)
        {
            return f(x) * Math.Sin(_k * Math.PI * x / L);
        }

        private double Ak(double k)
        {
            _k = k;
            return 2 / L * Integral.Get(AkUnderIntegralExpression, 0, L);
        }

        public override double U(double x, double t)
        {
            const int N = 100;
            double u = 0;
            double pi = Math.PI;

            for (int k = 1; k <= N; k++)
            {
                u += Ak(k) * Math.Cos(k * pi * A * t / L) * Math.Sin(k * pi * x / L);
            }

            return u;
        }

        public double L { get; set; }
    }
}
