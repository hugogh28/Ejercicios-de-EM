using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _11_de_febrero
{
    public class Ejercicio4
    {
        static Random random = new Random();
        static volatile bool pedido = false;
        static volatile bool respondido = false;
        static volatile bool proceso = true;
        static volatile int mensaje = 0;

        static void Cliente()
        {
            for (int i = 0; i < 10; i++)
            {
                //Genera un núumero aleatorio
                mensaje = random.Next(10);
                Console.WriteLine("Cliente: " + mensaje);
                pedido = true;
                while (!respondido) ;
                respondido = false; //Cerramos la puerta
                Console.WriteLine("*******Respuesta*******: " + mensaje);
            }
        }
        static void Servidor()
        {
            for (int i = 0; i < 10; i++)
            {
                //Muestra el numero aleatorio
                while (!pedido) ;
                pedido = false; //CIERRA SIEMPRE LAS PUERTAS, ES MUY IMPORTANTE
                mensaje++;
                Console.WriteLine("Servidor: " + mensaje);
                respondido = true;
                //Thread.Sleep(100);
                //pedido = false; //Aquí el cliente adelanta al servidor, lo que provoca un bucle infinito del que no podemos salir

            }
        }

        public static void Main(String[] args)
        {
            Thread pro1 = new Thread(Cliente); //En principio no nos hace falta guardar los hilos en una variable
            pro1.Start();
            Thread pro2 = new Thread(Servidor);
            pro2.Start();
        }
    }
}
