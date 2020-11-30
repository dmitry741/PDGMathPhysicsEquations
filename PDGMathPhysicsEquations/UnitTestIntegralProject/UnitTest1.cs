using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestIntegralProject
{
    [TestClass]
    public class UnitTest1
    {
        double Epsilon => 0.001;

        [TestMethod]
        [Description("Тестируем интеграл от y=x^2 отрезке от 0 до 2.")]
        public void TestMethod1()
        {
            double S = PDGMathPhysicsEquations.Integral.Get(x => x * x, 0, 2);
            Assert.IsTrue(Math.Abs(S - 8.0 / 3) < Epsilon);
        }
    }
}
