using System;
using System.Threading;

namespace Ejercicios_de_EM.Tema_2._2
{
    class Ejercicio17
    {
        const int N_FILOSOFOS = 5;
        static volatile Random random = new Random();

        private static void Filosofo(object numFilosofo)
        {
            while (true)
            {
                WriteLineI("Pensar");
                //Obtener tenedores
                WriteLineI("Comer");
                //Liberar tenedores
            }
        }
        private static void Main()
        {
            for (int i = 0; i < N_FILOSOFOS; i++)
            {
                Thread t = new Thread(Filosofo);
                t.Name = new string('\t', i);
                t.Start(i);
            }
        }
        private static void WriteLineI(string s)
        {
            Thread.Sleep(random.Next(10));
            Console.WriteLine(Thread.CurrentThread.Name + s);
            Thread.Sleep(random.Next(10));
        }
    }
}