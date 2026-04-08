using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace _11_de_febrero
{
    internal class Mostoles
    {
        const int N_COCHES = 10;
        const int M = 3;
        const int vueltas = 12;
        static volatile List<string> clasificacion = new List<string>();
        static Random random = new Random();
        static Mutex mutex = new Mutex();
        static SemaphoreSlim barrera = new SemaphoreSlim(0);
        static SemaphoreSlim correr = new SemaphoreSlim(0);
        static SemaphoreSlim boxes = new SemaphoreSlim(M);
        static SemaphoreSlim finalizar = new SemaphoreSlim(0);
        static volatile int contadorCoches = 0;

        public static void Coche(object id)
        {
            //Creación de variables locales necesarias para el desarrollo del apartado B
            int intentos = 0;
            bool intentando = false;

            //Indicamos una zona de exclusión mutua para que solo cuando todos los coches estén, el semáforo se ponga verde
            mutex.WaitOne();
            ++contadorCoches;

            if (contadorCoches == N_COCHES)
            {
                barrera.Release();
            }
            mutex.ReleaseMutex();

            correr.Wait();

            //Hacemos que el coche de 12 vueltas
            for (int i = 0; i < vueltas; i++)
            {
                //Tiempo que tarda en dar una vuelta
                Thread.Sleep(random.Next(700, 1000));

                if (i % 4 == 0 || intentando) //Debido a la forma de diseñar el bucle for es innecesario indicar que no se tenga en cuenta la vuelta 12
                {
                    //Para comprobar si puede acceder a los boxes, el coche comprobará antes el número de permisos disponibles en el semáforo boxes
                    if (boxes.CurrentCount > 0)
                    {
                        intentos = 0;
                        boxes.Wait();
                        Thread.Sleep(200);
                        boxes.Release();
                    }
                    else //Si no hay sitio el coche se marcará a sí mismo una variable booleana a true para que vuelva a intentar entrar en la siguiente vuelta
                    {
                        intentando = true;
                        WriteLine("Boxes ocupados", id);
                        ++intentos;
                        if (intentos == 2) //Si el coche ha intentado entrar dos veces sin exito a boxes, abandonará la carrera
                        {
                            intentando = false;
                            WriteLine("Abandona", id);
                            finalizar.Release();
                            return;
                        }
                    }
                }
            }
            mutex.WaitOne(); //Indicamos una nueva zona de exclusión mutua para indicar que 
            clasificacion.Add("Coche " + id);
            mutex.ReleaseMutex();

            finalizar.Release();
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < N_COCHES; i++)
            {
                new Thread(Coche).Start(i);
            }

            barrera.Wait();

            if (contadorCoches == N_COCHES)
            {
                Console.WriteLine("¡SEMÁFORO EN VERDE!");
            }

            correr.Release(N_COCHES);

            for (int i = 0; i < N_COCHES; i++) //ESTO ESTÁ MAL
            {
                finalizar.Wait();
            }

            Console.WriteLine("Clasificacion final: ");
            foreach (string i in clasificacion) { Console.WriteLine(i); }
            Console.ReadLine();
        }

        static void WriteLine(string s, object id)
        {
            Console.WriteLine("Coche " + id + ": " + s);
        }
    }
}