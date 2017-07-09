using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    class Integral
    {
        const double epsilon = 1e-6;
        static int q = 7;
        double x1 = 0;
        double x2 = 83;
        Dictionary<string,double> Variables = new Dictionary<string,double>(q);

        delegate double Funct(double x);

        public Integral()
        {
        }

        double F(double x)
        {
            return Math.Sin(x) * Math.Cos(x);
        }

        double Antiderivative(double x)
        {
            return Math.Sin(x) * Math.Sin(x) / 2;
        }

        double FourthDerivative(double x)
        {
            return 8 * Math.Sin(x) * Math.Cos(x);
        }

        void Swap(ref double a, ref double b)
        {
            double buf = a;
            a = b;
            b = buf;
        }

        void NewtonLeibniz(double a, double b, Funct antder)
        {
            if (a>b){Swap(ref a,ref b);}
            Variables.Add("Iexact",(antder(b) - antder(a)));
        }

        void FindN(double eps, double a, double b, double M)
        {
            if (a > b) { Swap(ref a, ref b); }
            double h = Math.Sqrt(Math.Sqrt(180*eps/((b-a)*M)));
            int m = (int)((b-a)/(h*2))+1;
            Variables.Add("n", 2 * m);
        }

        double SimpsonCalc(double a, double b, Funct function, int n)
        {
            double PairSumm = 0, OddSumm = 0, h = (double)(b-a)/n;
            for (int i = 1; i < n; i++)
            {
                if (i % 2 == 0) { PairSumm += function(a + i * h); }
                else { OddSumm += function(a + i * h); }
            }
            return (h / 3 * (function(a) + function(b) + 4 * OddSumm + 2 * PairSumm));
        }

        void Simpson(double a, double b,Funct function)
        {
            int n = (int)Variables["n"];
            double result = SimpsonCalc(a, b,function,n);
            Variables.Add("In",result); 
            double Error = Math.Abs(Variables["Iexact"] - result);
            Variables.Add("Error",Error); 
        }

        void RefinedCalc(double error, double a, double b, Funct function)
        {
            int n = 2 * (int)Variables["n"];
            double In = Variables["In"], err = Variables["Error"];
            double I2n = SimpsonCalc(a,b,function,n), del = Math.Abs(In - I2n);
            while (del /15 > err)
            {
                In = I2n;
                n *= 2;
                I2n = SimpsonCalc(a, b, function,n);
                del = Math.Abs(In - I2n);
            }
            Variables.Add("n2", n);
            Variables.Add("I2n", I2n);
            double Error = Math.Abs(Variables["Iexact"]- I2n);
            Variables.Add("Error2n", Error);
        }


        public void FTable()
        {
            Console.WriteLine("Composite Simpson`s rule");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("| Eps  |          h          |      F(b)-F(a)     |        delta         |");
            Console.WriteLine("----------------------------------------------------------------------------");
            int M = 4;
            NewtonLeibniz(x1, x2, Antiderivative);
            FindN(epsilon,x1, x2,M);
            Simpson(x1, x2, F);
            double h = (double)(x2-x1)/Variables["n"];
            Console.WriteLine("|{0,6}|{1,21}|{2,20}|{3,22}|",epsilon,h,
                Variables["Iexact"],Variables["Error"]);
            Console.WriteLine("----------------------------------------------------------------------------");
        }

        public void STable()
        {
            Console.WriteLine("Refined calculation");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("|         delta           |           h          |        Error         |");
            Console.WriteLine("----------------------------------------------------------------------------");
            double errorn = Variables["Error"];
            RefinedCalc(errorn, x1, x2, F);
            double h = (int)(x2-x1)/Variables["n2"], error2n = Variables["Error2n"];
            Console.WriteLine("|{0,25}|{1,22}|{2,22}|",errorn,h,error2n);
        }








    }
}
