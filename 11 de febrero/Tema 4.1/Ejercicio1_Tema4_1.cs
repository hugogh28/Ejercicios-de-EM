using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _11_de_febrero
{
    public class Ejercicio1_Tema4_1
    {
        static Random random = new Random();
        static volatile bool producido = false;
        static volatile int producto = 0;

         void Productor()
        {
            producto = random.Next(10);
            //Genera un núumero aleatorio
            Console.WriteLine("Productor: " + producto);
            producido = true;
        }
         void Consumidor()
        {
            //Muestra el numero aleatorio
            while (!producido)
            {
                Console.WriteLine("Esperando");
            }
            ;
            Console.WriteLine("*******Consumidor*******: " + producto);
        }

        public void Exec()
        {
            //Declaración de un hilo con Action y con Invoke

            Action met_productor = Productor;
            //ThreadStart hilo = Productor;
            ThreadStart hilo = met_productor.Invoke;
            Thread prod = new Thread(hilo);
            
            //Thread prod = new Thread(() => Productor()); //Podemos usar una expresion lambda para indicar la funcion a la que queremos llamar, todo porque ThreadStart es un Delegate 
            prod.Start();

            Thread cons = new Thread(() => Consumidor());
            cons.Start();
        }

        public static void Main(String[] args)
        {
            new Ejercicio1_Tema4_1().Exec();
        }
    }
}
