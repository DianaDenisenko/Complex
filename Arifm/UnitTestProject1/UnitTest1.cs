using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Arifm;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SummaCheck()
        {
            double a = 10; double b = 4;
            Arifmetica ar = new Arifmetica(a, b);
            Assert.AreEqual(14, ar.Summa());
            Assert.AreEqual(a - b, ar.Vych());
            Assert.AreEqual(a * b, ar.Proizv());
            Assert.AreEqual(a / b, ar.Del());
        }

        [TestMethod]
        public void UravnenieCheck()
        {
            double S = 0;
            double d = 5;
            Uravnenie ur = new Uravnenie(d, S);
            Assert.AreEqual(5.5 + d, ur.Func());

            S =-4;
            ur.S = S;
            Assert.AreEqual(d * S, ur.Func());

            S = 10;
            ur.S = S;
            Assert.AreEqual(d * S * S, ur.Func());

        }
        [TestMethod]
        public void TestMethod1()
        {
            double a = 23.4;
            double b = 22.1;
            double c = 10;
            double D = Func(a + c * b) + Func(b - a) + Func(0.2);
            De ez = new De(a, b, c);
            Assert.AreEqual(D, ez.D());
        }
        public double Func(double x)
        {
            return x - (x * x * x) / 6D + (x * x * x * x * x) / 120D;
        }
        [TestMethod]
        public void TestMethod4()
        {
            int a =301;
            Dengi CostP = new Dengi(a);
            Assert.AreEqual(201*8, CostP.Cost());
        }
        [TestMethod]
        public void TestMethod5()
        {
            double a = 3;
            double b = 3;
            double c = 3;
            Treg triangle = new Treg(a, b, c);
            Assert.AreEqual("Равнобедренный", triangle.Check());
            Assert.AreEqual("Остроугольный", triangle.CheckAngle());
        }
    }
}
