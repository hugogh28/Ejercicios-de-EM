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

namespace Ejercicios_de_EM.Tema_2._3
{
    public class Ejercicio20
    {
        static void Client()
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("10.8.237.153"), 9050); //Si se escribe aquí la misma IP y puerto que la del servidor abierto, se podrá mandar mensajes al mismo (por ejemplo en clase la IP que usamos hacía que saliese en la pantalla del ordenador del profe porque estabamos en la misma red)
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);

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

            while (true)
            {
                EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
                int recv = serverSocket.ReceiveFrom(data, ref senderEP);

                string msg = System.Text.Encoding.UTF8.GetString(data, 0, recv);

                Console.WriteLine(msg);
            }

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
