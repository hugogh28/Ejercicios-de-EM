using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ejercicios_de_EM.Tema_4._1
{
    class Ejercicio2_Tema4_2
    {
        void Mensajes()
        {
            string[] mensajes =
            {
                "La vida es bella",
                "O no...",
                "Los pajaritos cantan",
                "Y molestan..."
            };

            foreach (var mensaje in mensajes)
            {
                Console.WriteLine("Mensajes: " + mensaje);
                try
                {
                    Thread.Sleep(1000); //Necesitamos escribir cada 2 segundos
                }
                catch(ThreadInterruptedException ex)//Si escribes catch vacío, al recibir la excepción el código finaliza sin una excecpción nombrada como tal
                {
                    //Thread.Sleep(5000);
                    Console.WriteLine(ex.ToString());//Para imprimir la excepción en la consola y permitir así al programador saber dónde podría haber un error
                    Console.WriteLine("Mensajes: Se acabó!");
                    return;
                }
            }
        }

        void Exec()
        {
            Thread t = new Thread(() => Mensajes());
            t.Start();

            int contador = 0;
            while (true)
            {
                //Thread.Sleep(1000);

                bool a = t.Join(1000); //Otra forma de "pausar" un proceso
                if (a /*o también*//*!t.IsAlive*/) break;    
                
                contador++;
                Console.WriteLine("Main: Todavía esperando...");

                if (contador == 4)
                {
                    Console.WriteLine("Cansado de esperar!");
                    t.Interrupt();
                    //Console.WriteLine("222Por fin!");
                    //Thread.Sleep(3000);
                    //Console.WriteLine("Main:me levanto!");
                    break;
                }
            }
            t.Join();
            Console.WriteLine("Por fin!");
        }

        static void Main(string[] args)
        {
            new Ejercicio2_Tema4_2().Exec();
        }
        }
    }

