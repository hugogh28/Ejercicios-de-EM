using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _11_de_febrero
{
    class Utils
    {
        public static Random Rand = new Random();

        public static void SleepRandom(int millis)

        {

            Thread.Sleep(Rand.Next(0, millis));

        }

    }
    class Dune
    {
        public static int NUM_GRANJAS = 10;

        public static int NUM_MARTILLO = 5;

        public static int ESPECIA_MAX = 8;



        public static int NUM_EXPLOS = 3;

        public static int COSTE_EXPLOS = 5;

        //PARTE 1

        static SemaphoreSlim martillosDisponibles = new SemaphoreSlim(NUM_MARTILLO); //Definimos un semaforo para seguir la restricción del número de martillos disponibles
        static SemaphoreSlim esperarRecolectores = new SemaphoreSlim(0); //Definimos un semáforo para seguir la restricción que obliga al programa a esperar a todos los recolectores antes de iniciar un nuevo ciclo

        static Mutex almacenMutex = new Mutex(); //Definimos un mutex para evitar sobrescribir/leer incorrectamente la variable global ALMACEN

        static volatile int ALMACEN = 0;

        //PARTE 2
        static SemaphoreSlim huecosExploradores = new SemaphoreSlim(NUM_EXPLOS); //Definimos un semáforo para seguir la restricción que obliga al instructor a no producir más exploradores de los debidos

        //PARTE 1
        public static void Recolector(object id)
        {
            WriteLine(id, "Viajo a la granja de especia");

            //Aplicamos la restricción del semáforo martillosDisponibles
            martillosDisponibles.Wait();
            int especia = Utils.Rand.Next(1, ESPECIA_MAX);
            WriteLine("\t" + id, "He tomado " + especia + " especias");
            martillosDisponibles.Release();

            WriteLine(id, "Vuelvo a la base central");

            //Evitamos hacer malas lecturas/escrituras en la variable ALMACEN
            almacenMutex.WaitOne();
            ALMACEN += especia;
            almacenMutex.ReleaseMutex();

            //Indicamos que el recolector ha acabado a través del semáforo de fin de ciclo: esperarRecolectores
            esperarRecolectores.Release();
        }

        //PARTE 2
        public static void Explorador(object id)
        {
            //El explorador explorar en un tiempo aleatorio y da su informe
            Console.WriteLine("\t \tExplorador {0}: Explorando...", id);
            Utils.SleepRandom(200);
            Console.WriteLine("\t \tExplorador {0}: Mi informe", id);

            //Al terminar, deja espacio a nuevos exploradores
            huecosExploradores.Release();
        }

        public static void Instructor()
        {
            //Definimos un índice para identificar a nuevos exploradores
            int i = 0;
            //Mantenemos ejecutando a Instructor de forma infinita ya que requerimos que este trabaje continuamente
            while (true)
            {
                almacenMutex.WaitOne();
                if (ALMACEN >= COSTE_EXPLOS)
                {
                    //Restringimos la lectura/escritura de la variable global ALMACEN para evitar errores
                    ALMACEN -= COSTE_EXPLOS;
                    almacenMutex.ReleaseMutex();
                    huecosExploradores.Wait();

                    //Aumentamos el índice de exploradores
                    i++;

                    //Creamos un nuevo explorador cada vez que sea posible
                    Thread explorador = new Thread(Explorador);
                    explorador.Start(i);
                }
                else
                {
                    Console.WriteLine("\t\t\tInstructor: NO PUEDO CREAR EXPLORADORES, NO HAY SUFICIENTES ESPECIAS");
                    almacenMutex.ReleaseMutex();
                    //Thread.Sleep(100);

                }

            }
        }

        static void Main(string[] args)
        {
            //PARTE 2

            //Creamos un único hilo Instructor que será quien controle la creación de nuevos exploradores
            Thread instructor = new Thread(Instructor);
            instructor.Start();

            //PARTE 1
            while (true) //Ejecutamos indefinidamente un número infinito de ciclos
            {
                //Inicializamos y arrancamos los hilos del nuevo ciclo
                Console.WriteLine("NUEVO CICLO");
                for (int i = 0; i < NUM_GRANJAS; i++)
                {
                    Thread granja = new Thread(Recolector);
                    granja.Start(i);
                }

                //Detenemos la ejecución de Main hasta que todos los recolectores hayan acabado
                for (int i = 0; i < NUM_GRANJAS; i++)
                {
                    esperarRecolectores.Wait();
                }

                //Se procesan e indican las especias del almacen, y se espera un tiempo aleatorio entre un ciclo y el siguiente
                Console.WriteLine("EL ALMACEN CONTIENE:");

                Console.WriteLine(ALMACEN + " especias\n");

                Utils.SleepRandom(200);
            }
        }

        //Creación de un método auxiliar que permita identificar a cada hilo
        public static void WriteLine(object id, string s)
        {
            Console.WriteLine("Thread " + id + ": " + s);
        }
    }
}
