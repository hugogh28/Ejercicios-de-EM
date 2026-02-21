using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _11_de_febrero
{
    public class Ejercicio7
    {
        static Random random = new Random();
        static Mutex museo = new Mutex();
        static volatile int numPersonas = 0;

        static void Persona(Object nombre)
        {
            Thread.CurrentThread.Name = "Persona" + nombre.ToString(); //Cada nuevo Thread tomará de nombre PersonaN, siendo N el número de la iteración en la que se encuentre
            WriteLine("He nacido");

            for (int i = 0; i < 10; i++)//while (true)
            {
                //Entrar al museo

                //Hola
                museo.WaitOne();    //Indicamos que solo el primero que haga esta acción puede sumar 1 al número de personas y saludar a la totalidad de ese número
                    numPersonas++;
                    WriteLine("hola, somos " + numPersonas + " personas");
                    if (numPersonas == 1)
                    {
                        WriteLine("Tengo un regalo :D");
                    }
                    else
                    {
                        WriteLine("No tengo regalo :(");
                    }
                museo.ReleaseMutex();


                //Que bonito
                WriteLine("qué bonito!");

                /*
                 * 
                 * Es muy importante que usemos solo un Mutex para este ejercicio, puesto que se nos pide usar una única variable, por lo que nos interesa
                 * asegurarla lo más posible, o dicho de otro modo, queremos evitar a toda costa que otro hilo acceda a ella mientras que otro esté saludando
                 * o despidiéndose para evitar que recibamos, por ejemplo: "hola, somos 2 personas" y "adiós a los 2"
                 * 
                 */

                //Adiós
                museo.WaitOne();    ///Lo mismo que con el saludo, solo que para despedirse, en este caso es muy importante que sea único, es decir SOLO UN MUTEX, de lo contrario se podrá intercalar con otro hilo en el primer Mutex
                numPersonas--;
                    WriteLine("adiós a los " + numPersonas);
                museo.ReleaseMutex();


                //Pasear
                WriteLine("paseo");
            }
        }

        public static void WriteLine(string texto)
        {
            Thread.Sleep(random.Next(10));
            Console.WriteLine(Thread.CurrentThread.Name /*Esto usa el nombre del Thread en el que se encuentre*/ + ": " + texto);
            Thread.Sleep(random.Next(10));
        }


        public static void Main(String[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                Thread nuevoHilo = new Thread(Persona);
                nuevoHilo.Start(i);
            }
        }
    }
}
