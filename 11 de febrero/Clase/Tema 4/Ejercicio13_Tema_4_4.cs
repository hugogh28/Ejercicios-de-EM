using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ejercicios_de_EM.Clase.Tema_4
{
    class Maquina
    {
        Random random = new Random();
        const int MAX_PIEZAS = 5;
        volatile List<double> almacen = new List<double>();

        public void FabricarPieza()
        {

        }

        public void AlmacenarPieza()
        {

        }
    }

    class Robot
    {
        public void RecogerPieza()
        {

        }

        public void MontarPieza()
        {

        }
    }

    class Ejercicio13_Tema_4_4
    {
        const int NUM_ROBOTS = 4;
        const int NUM_TIPOS_PIEZAS = 6;

        List<Task<string>> tareas = new List<Task<string>>();

        void Exec()
        {
            while(true)
            {
            }
        }

        public static void Main(string[] args)
        {
            new Ejercicio13_Tema_4_4().Exec();
        }
    }
}
