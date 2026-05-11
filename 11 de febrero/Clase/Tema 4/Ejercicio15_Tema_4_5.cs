using _11_de_febrero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicios_de_EM.Clase.Tema_4
{
    public class Ejercicio15
    {
        public void Exec()
        {
            Task tareaCafe = Task.Run(() =>
            {
                Coffee coffee = new Coffee();
                coffee.PourCoffee();
                Console.WriteLine("Coffee is ready");
                return coffee;
            });

            Task<Egg> tareaHuevo = Task.Run(() =>
            {
                Egg egg = new Egg();
                egg.FryEggs(2);
                Console.WriteLine("Eggs are ready");
                return egg;
            });

            Task<Bacon> tareaBacon = Task.Run(() =>
            {
                Bacon bacon = new Bacon();
                bacon.FryBacon(3);
                Console.WriteLine("Bacon is ready");
                return bacon;
            });

            Task<Toast> tareaTostada = Task.Run(() =>
            {
                Toast toast = new Toast();
                toast.ToastBread(2);
                toast.ApplyButter(toast);
                toast.applyJam(toast);
                Console.WriteLine("Toasts are ready");
                return toast;
            });

            Task<Juice> tareaZumo = Task.Run(() =>
            {
                Juice juo = new Juice();
                juo.PourJuice();
                Console.WriteLine("Juice is ready");
                return juo;
            });

            tareaCafe.Wait();
            tareaHuevo.Wait();
            tareaBacon.Wait();
            tareaTostada.Wait();
            tareaZumo.Wait();

            Console.WriteLine("Acabé");
            Console.ReadLine(); //Esto lo he puesto solo porque mi consola se cierra automáticamente al acabar
        }

        static void Main(string[] args)
        {
            new Ejercicio15().Exec();
        }
    }

    public class Coffee
    {
        public Coffee PourCoffee()
        {
            Utils.SleepRandom(1000);

            return new Coffee();
        }
    }

    public class Egg
    {
        public Egg FryEggs(int eggs)
        {
            Utils.SleepRandom(eggs * 1000);

            return new Egg();
        }
    }

    public class Bacon
    {
        public Bacon FryBacon(int bacon)
        {
            Utils.SleepRandom(bacon * 1000);

            return new Bacon();
        }
    }

    public class Toast
    {
        public Toast ToastBread(int bread)
        {
            Toast breads = null;

            return breads;
        }
        public Toast ApplyButter(Toast toast)
        {
            return toast;
        }
        public Toast applyJam(Toast toast)
        {
            return toast;
        }
    }

    public class Juice
    {
        public Juice PourJuice()
        {
            Utils.SleepRandom(1000);

            return new Juice();
        }
    }
}
