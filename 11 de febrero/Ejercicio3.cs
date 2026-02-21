using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _11_de_febrero
{
    public class Ejercicio3
    {
        static Random random = new Random();
        static volatile bool pedido = false;
        static volatile bool respondido = false;
        static volatile int mensaje = 0;

        static void Cliente()
        {
            //Genera un núumero aleatorio
            mensaje = random.Next(10);
            Console.WriteLine("Cliente: Manda una petición: " + mensaje);
            pedido = true;
            while (!respondido) ;
            Console.WriteLine("*******Respuesta*******: " + mensaje);
        }
        static void Servidor()
        {
            //Muestra el numero aleatorio
            while (!pedido) ;
            Console.WriteLine("Servidor: Recibe la petición " + mensaje);
            mensaje++;
            Console.WriteLine("Servidor: Manda la respuesta " + mensaje);
            respondido = true;
            
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
