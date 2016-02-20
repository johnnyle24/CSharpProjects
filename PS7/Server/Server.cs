using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        private TcpListener server;
       

        static void Main(string[] args)
        {
        }

        /// <summary>
        /// Creates a server with the given port.
        /// </summary>
        /// <param name="port"></param>
        public Server (int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            server.BeginAcceptSocket(ConnectionReceived, null);
        }

        /// <summary>
        /// Called when connection happens.
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectionReceived(IAsyncResult ar)
        {
            Socket socket = server.EndAcceptSocket(ar);
            server.BeginAcceptSocket(ConnectionReceived, null);
        }

        private void playerNameReceive(string name)
        {

        }
    }
}
