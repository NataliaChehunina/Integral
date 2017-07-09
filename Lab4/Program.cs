using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Integral intg = new Integral();
            intg.FTable();
            Console.WriteLine();
            intg.STable();
            Console.WriteLine();
        }
    }
}
