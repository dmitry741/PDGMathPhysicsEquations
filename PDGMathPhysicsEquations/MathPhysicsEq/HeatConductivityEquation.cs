using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace MathPhysicsEq
{
    class HeatConductivityEquation
    {
        readonly double _low, _high, _A;
        readonly Function _f;
        double _x, _t;

        public HeatConductivityEquation(Function f, double low, double high, double A)
        {
            _f = f;
            _low = low;
            _high = high;
            _A = A;
        }

        private double UnderIntegralExpression(double s)
        {
            double arge = -(_x - s) * (_x - s) / (4 * _A * _A * _t);
            return _f(s) * Math.Exp(arge);
        }

        public double HeatConductivity(double x, double t)
        {
            double m = 1.0 / (2 * _A * Math.Sqrt(Math.PI * t));
            _x = x;
            _t = t;

            return m * Integral.Get(UnderIntegralExpression, _low, _high);
        }
    }
}
