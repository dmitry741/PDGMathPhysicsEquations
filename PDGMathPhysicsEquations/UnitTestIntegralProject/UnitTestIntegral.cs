using System;
using System.Threading.Tasks;
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

        [TestMethod]
        [Description("Убеждаемся, что синхронный метод вычисления интеграла и асинхронный дают одинаковые результаты.")]
        public void TestIntegralSyncAsync()
        {
            const double edge = 10000;
            const int split = 1000;
            const double epsilon = 0.0001;

            double s1 = Integral.Get(x => 1.0 / (1.0 + x * x), -edge, edge, split);

            double s2 = Integral.GetAsync(x => 1.0 / (1.0 + x * x), -edge, edge, split);

            Assert.IsTrue(Math.Abs(s1 - s2) < epsilon);
        }

        [TestMethod]
        [Description("Убеждаемся, что асинхронный метод работает быстрее.")]
        public void TestIntegralPerformance()
        {
            const double edge = 20000;
            const int split = 1000;


            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();

            for (int i = 0; i < 20; i++)
            {
                sw1.Start();
                double s1 = Integral.Get(x => 1.0 / (1.0 + x * x), -edge, edge, split);
                sw1.Stop();

                sw2.Start();
                double s2 = Integral.GetAsync(x => 1.0 / (1.0 + x * x), -edge, edge, split);
                sw2.Stop();
            }

            long tSync = sw1.ElapsedMilliseconds;
            long tAsync = sw2.ElapsedMilliseconds;

            Assert.IsTrue(tAsync < tSync);
        }
    }
}
