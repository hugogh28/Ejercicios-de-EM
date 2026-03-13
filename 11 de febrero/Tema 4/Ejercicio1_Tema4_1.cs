using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _11_de_febrero
{
    public class Ejercicio1_Tema4_1
    {
        SemaphoreSlim sinc = new SemaphoreSlim(0); //Al no ser static depende de cada objeto al que este asociado, por lo que si añadimos dos objetos de tipo Ejercicio1_Tema4_1 que ejecuten cada uno Exec(), quiere decir que el semaforo no sera igual para los dos, cosa que para static, si que lo es por donde se almacena en memoria
        Random random = new Random();
         volatile int producto = 0;

         void Productor()
        {
            producto = random.Next(10);
            //Genera un núumero aleatorio
            Console.WriteLine("Productor: " + producto);
            sinc.Release();
        }
         void Consumidor()
        {
            sinc.Wait();
            Console.WriteLine("Producto: {0}", producto);
        }

        public void Exec()
        {
            /*//Declaración de un hilo con Action y con Invoke

            Action met_productor = Productor;
            //ThreadStart hilo = Productor;
            ThreadStart hilo = met_productor.Invoke;
            Thread prod = new Thread(hilo);
            
            //Thread prod = new Thread(() => Productor()); //Podemos usar una expresion lambda para indicar la funcion a la que queremos llamar, todo porque ThreadStart es un Delegate 
            prod.Start();

            Thread cons = new Thread(() => Consumidor());
            cons.Start();*/

            new Thread(Productor).Start();
            new Thread(Consumidor).Start();
        }

        public static void Main(String[] args)
        {
            new Ejercicio1_Tema4_1().Exec();
        }
    }
}
