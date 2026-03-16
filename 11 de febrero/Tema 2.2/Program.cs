using System;
using System.Threading;

namespace _11_de_febrero
{
    internal class Program
    {
        const int N_PERSONAS = 3;
        const int N_ITERACIONES = 10;
        static volatile int numPersonas;
        static Mutex museo = new Mutex();
        static Mutex saludo = new Mutex();

        static void Persona()
        {
            for(int i = 0; i < N_PERSONAS; i++) {
                museo.WaitOne();
                
                
                WriteLine("Hola a los "+numPersonas);

                if (numPersonas == 0)
                {
                    WriteLine("Tengo un regalo :D");
                }
                else
                {
                    WriteLine("No tengo regalo :(");
                }

                ++numPersonas;
                museo.ReleaseMutex();

                WriteLine("Qué bonito!");
                WriteLine("Alucinante!");
                museo.WaitOne();
                --numPersonas;
                WriteLine("Adiós a los "+numPersonas);
                museo.ReleaseMutex();

                for(int j = 0;  j < N_ITERACIONES; j++)
                {
                    WriteLine("Paseando...");
                }
            }
        }

        static void Main(string[] args)
        {
            for(int i =0; i < N_PERSONAS; i++)
            {
                Thread persona = new Thread(Persona);
                persona.Name = new String('\t',i) + "persona " + i;
                persona.Start();
            }
            Console.ReadLine();
        }

        static void WriteLine(string s)
        {
            Thread.Sleep(10);
            Console.WriteLine(Thread.CurrentThread.Name + ": " + s);
            Thread.Sleep(10);
        }
    }
}
