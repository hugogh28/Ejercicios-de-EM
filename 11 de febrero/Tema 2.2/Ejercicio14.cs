using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ejercicios_de_EM.Tema_2._2
{
    internal class Ejercicio14
    {
        const int N_Hilos = 10;
        
        static volatile int contador = 0; //Si lo cambias a 10 los hilos se intercalan en un patrón ABAB

        static SemaphoreSlim barrera = new SemaphoreSlim(0);
        static SemaphoreSlim excMutua = new SemaphoreSlim(1); //Simplemente dejamos pasar a uno

        static Random random = new Random();

        static void Proceso()
        {

            /*
             * 
             * MUY ATENTO A LAS PROPIEDADES DE CORRECCIÓN
             * Como por ejemplo, asegurar que las variables globales están seguras
             * 
             */
            
            Console.Write("A");

            excMutua.Wait();
            contador++; //Azúcar sintáctica :D

            //int d = contador; //El número 10 no llega porque alguno no ha actualizado
            //d = d + 1; //El número 10 no llega porque alguno no ha actualizado

            //Thread.Sleep(100); //Esto haría que se beneficie erróneamente la condición de carrera ya que ponemos a dormir un hilo tras hacer lo que queríamos que hiciese
            //contador = d; //El número 10 no llega porque alguno no ha actualizado
            if (contador < N_Hilos) //Siempre que haya una variable compartida hay que protegerla, por eso usábamos un < antes
            {
                excMutua.Release();
                //Sincronizar 
                barrera.Wait();

            }
            else
            {
                excMutua.Release();

                //Liberamos los 9 hilos que no hayan entrado en el primera ejecución

                /*for (int i = 0; i < N_Hilos; i++)
                {
                    barrera.Release();
                }*/

                barrera.Release(N_Hilos-1); //Esto es la equivalencia a liberar el semáforo N_Hilos-1 veces
            }
            

            Console.Write("B");
        }
        public static void Main(String[] args)
        {
            for (int i = 0; i < N_Hilos; i++)
            {
                new Thread(Proceso).Start();
            }
        }
    }
}
