using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _11_de_febrero
{
    public class Ejercicio2
    {
        static Random random = new Random();
        static volatile bool producido = false;
        static volatile bool consumido = true;
        static volatile int producto = 0;

        static void Productor()
        {
            for (int i = 0; i < 10; i++)
            {
                while (!consumido)
                {
                    //Console.WriteLine("Esperando");
                }
                producto = random.Next(10);
                //Genera un núumero aleatorio
                Console.WriteLine("Productor: " + producto);

                /*consumido = false;  //El orden y lugar donde se pongan estos booleanos es MUY importante se puede permitir que un thread continúe su ejecución en momentos no deseados 
                Thread.Sleep(1000); //Esta instrucción permite que un hilo no continúe durante el número de milisegundos indicado
                producido = true;*/   //El orden y lugar donde se pongan estos booleanos es MUY importante se puede permitir que un thread continúe su ejecución en momentos no deseados 
                producido = true;   //El orden y lugar donde se pongan estos booleanos es MUY importante se puede permitir que un thread continúe su ejecución en momentos no deseados 
                consumido = false;  //El orden y lugar donde se pongan estos booleanos es MUY importante se puede permitir que un thread continúe su ejecución en momentos no deseados 
            }
        }
        static void Consumidor()
        {
            for (int i = 0; i < 10; i++)
            {
                //Muestra el numero aleatorio
                while (!producido)
                {
                    //Console.WriteLine("Esperando");
                }
            ;
                Console.WriteLine("*******Consumidor*******: " + producto);
                consumido = true;   //El orden y lugar donde se pongan estos booleanos es MUY importante se puede permitir que un thread continúe su ejecución en momentos no deseados 
                producido = false;  //El orden y lugar donde se pongan estos booleanos es MUY importante se puede permitir que un thread continúe su ejecución en momentos no deseados 
            }
        }
        public static void Main(String[] args)
        {
            Thread pro1 = new Thread(Productor); //En principio no nos hace falta guardar los hilos en una variable
            pro1.Start();
            Thread pro2 = new Thread(Consumidor);
                pro2.Start();
        }
    }
}
