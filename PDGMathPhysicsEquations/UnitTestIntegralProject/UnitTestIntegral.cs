using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDGMathPhysicsEquations;

namespace UnitTestIntegralProject
{
    [TestClass]
    public class UnitTestIntegral
    {
        double Epsilon => 0.001;

        [TestMethod]
        [Description("Тестируем интеграл от y=x^2 отрезке от 0 до 2.")]
        public void TestMethod1()
        {
            double S = Integral.Get(x => x * x, 0, 2);
            Assert.IsTrue(Math.Abs(S - 8.0 / 3) < Epsilon);
        }

        [TestMethod]
        [Description("Тестируем интеграл от y=sin(x) отрезке от -pi/2 до pi/2.")]
        public void TestMethod2()
        {
            double S = Integral.Get(x => Math.Sin(x), 0, Math.PI);
            Assert.IsTrue(Math.Abs(S - 2) < Epsilon);
        }
    }
}
