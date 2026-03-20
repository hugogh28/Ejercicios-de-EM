using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _11_de_febrero
{
    public class Ejercicio1
    {
        static Random random = new Random();
        static volatile bool producido = false;
        static volatile int producto = 0;
        
        static void Productor()
        {
            producto = random.Next(10);
            //Genera un núumero aleatorio
            Console.WriteLine("Productor: " + producto);
            producido = true;
        }
        static void Consumidor()
        {
            //Muestra el numero aleatorio
            while (!producido)
            {
                Console.WriteLine("Esperando");
            }
            ;
            Console.WriteLine("*******Consumidor*******: " + producto);
        }
        public static void Main(String[] args)//El main cuenta como hilo, por lo tanto, en este ejercicio hay tres
        {
            Thread pro1 = new Thread(Productor); //En principio no nos hace falta guardar los hilos en una variable (al menos por el momento)
            pro1.Start();
            new Thread(Consumidor).Start();
            //Consumidor(); //Puesto que no declaramos un nuevo hilo, simplemente llamamos al método, esto no cuenta como un hilo
        }
    }
}
