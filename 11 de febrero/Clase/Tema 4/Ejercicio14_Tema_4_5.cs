using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ejercicios_de_EM.Clase.Tema_4
{
    public class Ejercicio14_Tema_4_5
    {
        const int N_Tareas = 10;
        Random _random = new Random(); //Ojo a la notación a la hora de programar
        List<Task<string>> listatareas = new List<Task<string>>(N_Tareas);

        public static void Main(string[] arg) 
        {
            new Ejercicio14_Tema_4_5().Exec();
        }

        void Exec()
        {
            for(int i = 0; i < N_Tareas; i++) //Hacemos n tareas
            {
                listatareas.Add(Task.Run(Method)); //Guardamos las tareas en la lista de tareas
            }

            while (listatareas.Count > 0)
            {
                try
                {
                    int taskIndex = Task.WaitAny(listatareas.ToArray());
                    var tareafi = listatareas[taskIndex];
                    listatareas.RemoveAt(taskIndex);
                    if (tareafi.IsFaulted)
                    {
                        Console.WriteLine("Esta nos va a fallar");
                    }
                    else
                    {
                        Console.WriteLine("No es bloqueante");
                    }
                        Console.WriteLine("Una tarea ha dicho" + tareafi.Result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Excepción en tarea: " + ex.InnerException.Message.ToString()); //Captamos la excepción y la ponemos por pantalla
                }
            }
        }

        string Method()
        {
            Thread.Sleep(_random.Next(500));

            if (_random.Next(10) < 2) //Si la tarea es errónea se lanzará una excepción
            {
                throw new MyException("Estamos probando");
            }
            else //Si la tarea es correcta se indicará por medio de un string
            {
                return "Tarea correcta";
            }
        }

        [Serializable]
        private class MyException : Exception
        {
            public MyException()
            {
            }

            public MyException(string message) : base(message)
            {
            }

            public MyException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected MyException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
