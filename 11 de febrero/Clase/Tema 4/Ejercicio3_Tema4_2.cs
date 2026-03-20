using Ejercicios_de_EM.Tema_4._1;
using System;
using System.Diagnostics;
using System.Threading;

namespace Ejercicios_de_EM.Tema_4
{
    public class SincCond
    {
        //Con espera activa, por ello no se usa SemaphoreSlim (aunque es similar a su concepto)
        volatile bool condicion = false;

        //El funcionamiento del mutex es, en esencia, extremadamente similar a la clase que hemos diseñado con espera activa
        Mutex mimutex = new Mutex();

        public void Await()
        {
            while (!condicion) { }
            //mimutex.WaitOne();
        }

        public void Signal()
        {
            condicion = true;
            //mimutex.ReleaseMutex();
        }
    }

    public class Ejercicio3_Tema4_2
    {
        volatile bool producido = false;
        volatile float producto;

        SincCond mimutex = new SincCond();
        SincCond mimutex2 = new SincCond();

        public void Productor()
        {
            Random random = new Random();
            producto = random.Next(1, 10);
            //Indicamos al Consumidor que ya se ha mandado el nuevo valor de producto
            mimutex.Signal();
            //Restringimos a Productor para que no avance hasta no recibir una confirmacion de que Consumidor ha consumido el producto
            mimutex.Await();
        }
        public void Consumidor()
        {
            //Hacemos esperar a Consumidor para evitar que tome un valor erroneo de producto
            mimutex.Await();
            Console.WriteLine("Producto: {0}", producto);
            //Productor que ya se ha consumido 

            //Permitimos a Productor que continue avanzando
            mimutex2.Signal();
        }
        public void Exec()
        {
            new Thread(() => Productor()).Start();
            new Thread(() => Consumidor()).Start();
        }
        static void Main(string[] args)
        {
            new Ejercicio3_Tema4_2().Exec();
        }
    }
}
