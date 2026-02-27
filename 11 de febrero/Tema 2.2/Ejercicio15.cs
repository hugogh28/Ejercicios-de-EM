using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ejercicios_de_EM.Tema_2._2
{

    /////////////////////// SUJETO A REVISIÓN, MIRA EL VÍDEO DE JULIO /////////////////////// 

    internal class Ejercicio15
    {
        const int N_Hilos = 4;

        static volatile int contador = 0; //Si lo cambias a 10 los hilos se intercalan en un patrón ABAB

        static SemaphoreSlim barrera = new SemaphoreSlim(0);
        static SemaphoreSlim excMutua = new SemaphoreSlim(1); //Simplemente dejamos pasar a uno
        static SemaphoreSlim hiloFinal = new SemaphoreSlim(0);

        static Random random = new Random();

        static void Proceso(object o)
        {
            char c = (char)o;

            for (int i = 0; i<10; i++) 
            {
                Console.Write(c);

                excMutua.Wait();
                contador++;

                if (contador < N_Hilos) //Aseguramos que este if esté adaptado al bucle
                {
                    excMutua.Release();
                    //Sincronizar
                    barrera.Wait();
                    //hiloFinal.Release();
                }
                else
                {
                    contador = 0; //Hay que ponerlo a 0

                    //Thread.Sleep(100);
                    Console.Write('-');
                    barrera.Release(N_Hilos - 1);
                    /*for(int j = 0; j < N_Hilos - 1; j++)
                    {
                        hiloFinal.Wait();
                    }*/
                    //hiloFinal.Wait(N_Hilos - 2); //Aquí no se puede hacer con el Wait, por algún motivo que desconozco falla 
                    excMutua.Release();
                }
            }
        }

        public static void Main(String[] args)
        {
            char letra = 'A';
            for (int i = 0; i < N_Hilos; i++)
            {
                new Thread(Proceso).Start(letra);
                letra++;
            }
        }
    }
}
