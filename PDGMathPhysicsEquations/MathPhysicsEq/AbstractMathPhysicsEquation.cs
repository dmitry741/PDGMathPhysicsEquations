using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDGMathPhysicsEquations;

namespace MathPhysicsEq
{
    public abstract class AbstractMathPhysicsEquation
    {
        public abstract double U(double x, double t);
        public double A { get; set; }
        public Function f { get; set; }
    }
}
