using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


///// Para la próxima clase
namespace Ejercicios_de_EM.Tema_2._3
{
    public class Ejercicio21
    {
        static void Client()
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string msg = "Hello world!";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            clientSocket.SendTo(data, serverEP); //Este SendTo no es bloqueante

            //El finally es porque si el try no funciona, se debe recurrir en última instancia a lo indicado dentro de las llaves
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                Console.WriteLine("Error");
                clientSocket.Close();
            }
        }

        static void Server()
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 9050);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(serverEP);

            byte[] data = new byte[1024];
            EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
            int recv = serverSocket.ReceiveFrom(data, ref senderEP);

            string msg = System.Text.Encoding.UTF8.GetString(data, 0, recv);

            Console.WriteLine(msg);

            //El finally es porque si el try no funciona, se debe recurrir en última instancia a lo indicado dentro de las llaves
            try
            {
                serverSocket.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                serverSocket.Close();
            }
        }

        public static void Main(String[] args)
        {
            new Thread(Server).Start();
            new Thread(Client).Start();

        }
    }
}
