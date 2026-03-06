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
    internal class Ejercicio21
    {
        static int ServerPort = 9050;
        static void Client()
        {
            Socket client = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint clientEP = new IPEndPoint(IPAddress.Loopback/*IPAddress.Parse("10.125.4.10")*/ /*Parse para conectarse a un servidor específico, en este caso el del profesor*/, ServerPort);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,
            SocketType.Dgram, ProtocolType.Udp);


            while (true)
            {
                string msg = "Handshake initialized!";
                msg = /*"Hello"*/Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(msg);
                client.SendTo(data, clientEP);

                //clientSocket.SendTo(data, clientEP);


                if (msg == "EXIT") //De este modo cerramos el servidor abierto
                {
                    break;
                }
            }

            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                clientSocket.Close();
            }

        }

        

        static void Server()
        {
            Socket server = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            EndPoint serverEP = new IPEndPoint(IPAddress.Any, ServerPort);
            server.Bind(serverEP);
            Console.WriteLine("Server started at " + DateTime.UtcNow.ToString() + " on port " + ServerPort);

            byte[] msg = new byte[1024];
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                //int recv = server.ReceiveFrom(msg, ref remote);
                //string message = Encoding.UTF8.GetString(msg, 0, recv);
                //Console.WriteLine(remote.ToString() + ":" + message);
                int recv = server.ReceiveFrom(msg, ref remote);
                string message = Encoding.UTF8.GetString(msg, 0, recv);

                if(message == "EXIT")  //De este modo se cierra el servidor abierto
                {
                    Console.WriteLine("Me han mandado cerrar");
                    break;
                }

                Console.WriteLine(remote.ToString() + ":" + message);
            }

            try     //Si ocurre algo, cerrar la conexión
            {
                server.Shutdown(SocketShutdown.Both);
            }
            finally //Si ocurre un error fatal, cerrar el servidor
            {
                server.Close();
            }
        }

        public static void Main(String[] args)
        {
            new Thread(Client).Start();
            Thread.Sleep(200);
            new Thread(Server).Start();
        }
    }
}
