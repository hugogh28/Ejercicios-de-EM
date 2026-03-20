using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicios_de_EM.Tema_2._3
{
     class Class1
    {
        public delegate void Print(int value);
        public static void PrintNumber(int num)
        {
            Console.WriteLine("Number: {0}", num);
        }
        public static void PrintMoney(int money)
        {
            Console.WriteLine("Money: {0}", money);
        }
        public static void Main()
        {
            Print printDelegate = PrintNumber;
            printDelegate(1000);
            printDelegate = PrintMoney;
            printDelegate(1000);
        }
    }
}
