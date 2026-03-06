using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ejercicios_de_EM.Tema_2._3
{
    internal class Ejercicio22
    {
        static int ServerPort = 9050;
        static void Client()
        {
            Socket client = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint clientEP = new IPEndPoint(IPAddress.Loopback/*IPAddress.Parse("10.125.4.10")*/ /*Parse para conectarse a un servidor específico, en este caso el del profesor*/, ServerPort);



            while (true)
            {
                string msg = "Handshake initialized!";
                msg = /*"Hello"*/Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(msg);
                client.SendTo(data, clientEP);

                if (msg == "EXIT") //De este modo cerramos el servidor abierto
                {
                    break;
                }
            }

        }

        static void ConnectionThread()
        {
            int newServerPort = (int)ServerPort;
            Socket connThread = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            EndPoint conEP = new IPEndPoint(IPAddress.Any, ServerPort);
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

        }

        static void Server()
        {
            Socket server = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Any, ServerPort);
            server.Bind(serverEP);
            Console.WriteLine("Server started at " + DateTime.UtcNow.ToString());

            byte[] msg = new byte[1024];
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

            for (int i = 0; i < 10; i++)
            {
                //int recv = server.ReceiveFrom(msg, ref remote);
                //string message = Encoding.UTF8.GetString(msg, 0, recv);
                //Console.WriteLine(remote.ToString() + ":" + message);
                int recv = server.ReceiveFrom(msg, ref remote);
                string message = Encoding.UTF8.GetString(msg, 0, recv);
                Console.WriteLine(remote.ToString() + ":" + message);
                int nuevoPuerto = ServerPort + 1 + i;
                
                //AVISAR AL CLIENTE NUEVO SERVER

                new Thread(ConnectionThread).Start(nuevoPuerto);

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
            new Thread(Client).Start();
            new Thread(Server).Start();
        }
    }
}
