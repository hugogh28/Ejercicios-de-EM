using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace Ejercicios_de_EM.Tema_2._3
{
    public class Ejercicio22
    {
        static int ServerPort = 9050;

        static void Client(object o)
        {
            int minombre = (int)o;
            Socket client = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            EndPoint clientEP = new IPEndPoint(IPAddress.Loopback/*IPAddress.Parse("10.125.4.10")*//*Parse para conectarse a un servidor específico, en este caso el del profesor*/, ServerPort);
            client.ReceiveTimeout = -1;

            byte[] msg = new byte[1024];
            Thread.Sleep(100);
            string msgWelcome = "Handshake initialized!";

            //PASO 1: Cliente manda un saludo
            msg = /*"Hello"*/Encoding.UTF8.GetBytes(msgWelcome);
            
            client.SendTo(msg, clientEP);

            //PASO 4: Recibido el nuevo puerto, creo el nuevo socket
            int recv = client.ReceiveFrom(msg, ref clientEP);
            int nuevoPuerto = /*se puede poner solo como int*/ Int32.Parse(Encoding.UTF8.GetString(msg, 0, recv));

            EndPoint nuevoConnectionServer = new IPEndPoint(IPAddress.Loopback, nuevoPuerto);

            int i = 0;

            //PASO 5: Mandar mensajes por el nuevo puerto
            while (true)/*for (int i = 0; i < 10; i++)*/
            {
                i++;
                msgWelcome = "Mensaje" + i;
                //msgWelcome = Console.ReadLine();

                Console.WriteLine("Cliente:" + minombre + msgWelcome);
                msg = Encoding.UTF8.GetBytes(msgWelcome);
                client.SendTo(msg, clientEP);

                if (msgWelcome == "EXIT") //De este modo cerramos el servidor abierto
                {
                    break;
                }
            }

            msgWelcome = "EXIT";
            msg = Encoding.UTF8.GetBytes(msgWelcome);
            client.SendTo(msg, clientEP);

            try
            {
                client.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                client.Close();
            }

        }

        static void ConnectionThread(object o)
        {
            //PASO 5: Escuchar al cliente por este hilo
            int newServerPort = (int)o;
            Socket connThread = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            connThread.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            EndPoint conEP = new IPEndPoint(IPAddress.Any, newServerPort);
            connThread.Bind(conEP);

            byte[] msg = new byte[1024];
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                int recv = connThread.ReceiveFrom(msg, ref remote);
                string message = Encoding.UTF8.GetString(msg, 0, recv);
                Console.WriteLine(remote.ToString() + ":" + message);

                if (message == "EXIT") //De este modo se cierra el servidor abierto
                {
                    Console.WriteLine("Me han mandado cerrar");
                    break;
                }
            }

            try
            {
                connThread.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                connThread.Close();
                connThread.Dispose();
            }
        }

        static void Server()
        {
            Socket server = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Any, ServerPort);
            server.Bind(serverEP);
            Console.WriteLine("Server started at " + DateTime.UtcNow.ToString());

            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

            int i = 0;
            while(true)//for (int i = 0; i < 10; i++)
            {
                //int recv = server.ReceiveFrom(msg, ref remote);
                //string message = Encoding.UTF8.GetString(msg, 0, recv);
                //Console.WriteLine(remote.ToString() + ":" + message);

                byte[] msg = new byte[1024];

                //PASO 2: Recibo el primer handshake
                int recv = server.ReceiveFrom(msg, ref remote);
                string message = Encoding.UTF8.GetString(msg, 0, recv);
                Console.WriteLine(remote.ToString() + ":" + message);

                //PASO 3: AVISAR AL CLIENTE del nuevo puerto
                i++;
                if (i > 20)
                {
                    i = 0;
                }
                int nuevoPuerto = ServerPort + 1 + i;
                msg = Encoding.UTF8.GetBytes(nuevoPuerto.ToString());
                server.SendTo(msg, remote);

                //PASO 4: Creo un nuevo hilo para comunicarme con el nuevo cliente
                new Thread(ConnectionThread).Start(nuevoPuerto); //Como tengamos una peticion de memoria aunque no sea infinito, preocúpate

                //AVISAR AL CLIENTE NUEVO SERVER



                //if (message == "EXIT") //De este modo se cierra el servidor abierto
                //{
                //    Console.WriteLine("Me han mandado cerrar");
                //    break;
                //}
            }

            try     //Si ocurre algo, cerrar la conexión
            {
                server.Shutdown(SocketShutdown.Both);
            }
            finally //Si ocurre un error fatal, cerrar el cliente
            {
                server.Close();
            }
        }

        public static void Main(String[] args)
        {
            new Thread(Server).Start();
            for (int i = 0; i < 10; i++)
            {
                new Thread(Client).Start(i);
            }
            
        }
    }
}
