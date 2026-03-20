using Ejercicios_de_EM.Tema_4;
using System;
using System.Collections;
using System.Threading;

namespace Ejercicios_de_EM.Clase.Tema_4
{
    public class Ejercicio8_Tema_4_3
    {
        //Indicamos el número de hilos que queremos
        public const int NUM_HILOS = 6;

        //Indicamos el número de participantes de la barrera y la acción que debe ejecutarse inmediatamente al pasar esta (es importante revisar la documentación de C#)
        public Barrier barrera = new Barrier(NUM_HILOS, guion => Console.Write("-"));//Dependiendo en qué situaciones, "guion" debería ir entre paréntesis o no, en este caso no es necesario
        
        //El Proceso que repetiremos N veces
        void Proceso()
        {
            while (true)
            {
                //Por la segunda abstracción, no se calcula en condiciones el número de llegada de cada participante, por lo tanto, ignora las dos siguientes líneas
                Thread.Sleep(10);
                Console.Write("He llegado el :"+(barrera.ParticipantsRemaining));

                Console.Write("A");
                //barrera.RemoveParticipant();//Esto termina creando una excepción
                barrera.SignalAndWait(); //Restrigimos los hilos a la barrera
                Console.Write("B");
                barrera.SignalAndWait(); //Si usamos un bucle infinito será necesario indicar otra barrera para la escritura de "B", puesto que de otro modo se entremezclan las "A" y "B"
            }
        }

        void FinalBarrera(Barrier barrier)
        {
            if (barrier.CurrentPhaseNumber % 2 == 0)
                Console.Write("-");
            else
                Console.WriteLine(".");
        }

        //Indicamos en el método la creación de los hilos
        void Exec() 
        {
            Action <Barrier> accion = FinalBarrera;
            barrera = new Barrier(NUM_HILOS, accion); //De este modo señalizamos que barrera debe usar la nueva acción definido (además de crear una nueva en este espacio de memoria)
            for(int i = 0; i < NUM_HILOS; i++)
                new Thread(() => Proceso()).Start();
        }

        static void Main(string[] args)
        {
            new Ejercicio8_Tema_4_3().Exec();
        }
    }
}
