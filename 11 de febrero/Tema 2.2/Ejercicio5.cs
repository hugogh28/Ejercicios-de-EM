using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _11_de_febrero
{
    public class Ejercicio5
    {
        static Random random = new Random();
        static Mutex museo = new Mutex();

        static void Persona(Object nombre)
        {
            Thread.CurrentThread.Name = "Persona"+nombre.ToString(); //Cada nuevo Thread tomará de nombre PersonaN, siendo N el número de la iteración en la que se encuentre
            WriteLine("He nacido");

            for(int i=0; i<10; i++)//while (true)
            {
                //Entrar al museo
                museo.WaitOne();
                //(Solo puede entrar uno)
                    //Hola
                    WriteLine("hola!");
                    //Que bonito
                    //Thread.Sleep(random.Next(100)); //Mera comprobación para ver que el código funciona bien
                    WriteLine("qué bonito!");
                    //Adiós
                    WriteLine("adiós");
                museo.ReleaseMutex();
                //Pasear
                WriteLine("paseo");
            }
        }

        public static void WriteLine(string texto)
        {
            Thread.Sleep(random.Next(10));
            Console.WriteLine(Thread.CurrentThread.Name /*Esto usa el nombre del Thread en el que se encuentre*/ + ": " + texto);
            Thread.Sleep(random.Next(10));
        }


        public static void Main(String[] args)
        {
            for(int i=0; i < 3; i++)
            {
                new Thread(Persona).Start(i);
            }
        }
    }
}
