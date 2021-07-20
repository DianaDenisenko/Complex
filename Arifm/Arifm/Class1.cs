using System;

namespace Arifm
{
   
    public class De
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        
        public De(double A, double B, double C)
        {
            a = A;
            b = B;
            c = C;
        }
        public double D()
        {
            return Func(a + c * b) + Func(b - a) + Func(0.2);
        }
        public double Func(double x)
        {
            return x - (x * x * x) / 6D + (x * x * x * x * x) / 120D;
        }
    }
    public class Uravnenie
    {
        public double d { get; set; }
        public double S { get; set; }
        public Uravnenie(double d, double s)
        {
            this.d = d;
            this.S = S;
        }
        public double Func()
        {
            if (S < 0)
            {
                return d * S;
            }
            else
            if(S>=0 && S < 10){
                return 5.5 + d;
            }
            else
            {
                return d * S * S;
            }
        }
    }

    public class Dengi
    {
        public int count;
        public Dengi(int c)
        {
            count = c;
        }
        public int Cost()
        {
            return count * (((1000 - count) / 100) + 1);
        }
    }

    public class Treg
    {
        public double a;
        public double b;
        public double c;
        public Treg(double a, double b, double c)
        {
            this.a = a; this.b = b; this.c = c;
        }

        public string Check()
        {
            if (a == b && b == c)
            {
                return "Равносторонний";
            }
            else if (a == b || a == c || b == c)
            {
                return "Равнобедренный";
            }
            else return "Никакой";

        }
        public string CheckAngle()
        {
            double ab = (Math.Acos((a * a + b * b - c * c) / (2 * a * b))) * (180 / Math.PI);
            double bc = (Math.Acos((b * b + c * c - a * a) / (2 * b * c))) * (180 / Math.PI);
            double ac = 180 - ab - bc;
            if (ab == 90 || bc == 90 || ac == 90)
            {
                return "Прямоугольный";
            }
            else if (ab > 90 || bc > 90 || ac > 90)
            {
                return "Тупоугольный";
            }
            else
            {
                return "Остроугольный";
            }

        }
    }
    public class Arifmetica
    {
        public double a { get; set; }
        public double b { get; set; }

            public Arifmetica(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public double Summa()
        {
            return a + b;
        }
        public double Vych()
        {
            return a - b;
        }

        public double Del()
        {
            return a / b;
        }
        public double Proizv()
        {
            return a * b;
        }
    }
}
