﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace MathPhysicsEq
{
    public class WaveEquation : AbstractMathPhysicsEquation
    {
        double _k, _L, _a, _b;

        public WaveEquation(double l, double a, double b)
        {
            _L = l;
            _a = a;
            _b = b;
        }

        private double AkUnderIntegralExpression(double x)
        {
            return f(x) * Math.Sin(_k * Math.PI * x / _L);
        }

        private double Ak(double k)
        {
            _k = k;
            return 2 / _L * Integral.Get(AkUnderIntegralExpression, _a, _b);
        }

        public override double U(double x, double t)
        {
            const int N = 100;
            double u = 0;
            double pi = Math.PI;

            for (int k = 1; k <= N; k++)
            {
                u += Ak(k) * Math.Cos(k * pi * A * t / _L) * Math.Sin(k * pi * x / _L);
            }

            return u;
        }

        public double L => _L;
    }
}