//Cody Ngo, Johhny Le  11/7/15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Network_Controller
{
    // Preserve State Object
    public class PreservedState
    {
        /// <summary>
        /// Client socket
        /// </summary>
        public Socket clientSocket = null;

        /// <summary>
        /// Size of receive buffer
        /// </summary>
        public const int BufferSize = 1024;

        /// <summary>
        /// Receive buffer
        /// </summary>
        public byte[] buffer = new byte[BufferSize];

        /// <summary>
        /// This is our callback function/delegate from View.
        /// </summary>
        public Delegate callback;                       //Could be action<state>?

        /// <summary>
        /// String incoming to be decoded 
        /// </summary>
        public StringBuilder incoming = new StringBuilder();

        /// <summary>
        /// Hold cutoff information
        /// </summary>
        public StringBuilder oldIncoming = new StringBuilder();

        /// <summary>
        /// 
        /// </summary>
        public string[] cubes;

        /// <summary>
        /// Encoding used for incoming/outgoing data
        /// </summary>
        public UTF8Encoding encoding = new UTF8Encoding();

        public Exception exceptions;
    }



    public static class Network_Controller
    {
        // The port number for the remote device
        private const int port = 11000;

        private static readonly object sendSync = new object();

        private static readonly object receiveSync = new object();

        public static Socket Connect_to_Server(Delegate callback, string hostname)
        {
            PreservedState ps = new PreservedState();
            try
            {

                ps.callback = callback;

                IPHostEntry ipHostInfo = Dns.GetHostEntry(hostname);
                IPAddress ipAddress = ipHostInfo.AddressList[1];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                ps.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                ps.clientSocket.BeginConnect(remoteEP,
                    new AsyncCallback(Connected_to_Server), ps);


            }
            catch (Exception e)
            {
                ps.exceptions = e;
                ps.callback.DynamicInvoke(ps);
            }
            return ps.clientSocket;
        }

        private static void Connected_to_Server(IAsyncResult ar)
        {

            // Retrieve the socket from the state object
            PreservedState state = (PreservedState)ar.AsyncState;
            try
            {
                state.clientSocket.EndConnect(ar);

                state = (PreservedState)state.callback.DynamicInvoke(state);

                state.buffer = new byte[state.buffer.Length];

                state.incoming.Clear();

                state.clientSocket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                state.exceptions = e;
                state.callback.DynamicInvoke(state);
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            PreservedState client = (PreservedState)ar.AsyncState;
            try
            {


                // Get the buffer to which the data was written.
                byte[] buffer = client.buffer;

                Socket socket = client.clientSocket;

                // Figure out how many bytes have come in
                int bytes = socket.EndReceive(ar);

                if (bytes > 0)
                {
                    ////Connect cutoff data from previous recieve attempt to current incoming
                    //client.incoming.Append(client.oldIncoming);

                    ////Clear our old data for new possible cutoff data
                    //client.oldIncoming.Clear();

                    // Convert the bytes into Json string
                    client.incoming.Append(client.encoding.GetString(client.buffer, 0, bytes));

                    ////If the converted string is not complete
                    //if (client.incoming.ToString().StartsWith("{") && !client.incoming.ToString().EndsWith("}"))
                    //{
                    //    // Store the end of the string
                    //    client.oldIncoming.Append(client.incoming.ToString());

                    //    client.incoming.Clear();
                    //}

                    client.callback.DynamicInvoke(client);

                }
            }
            catch (Exception e)
            {
                client.exceptions = e;
                client.callback.DynamicInvoke(client);
            }


        }

        /// <summary>
        /// Called when more data is requested by the client
        /// </summary>
        /// <param name="state"></param>
        public static void i_want_more_data(PreservedState state)
        {
            try
            {
                state.clientSocket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                state.exceptions = e;
            }
        }

        /// <summary>
        /// Sends data to the server with the given socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send(Socket socket, string data)
        {
            PreservedState ps = new PreservedState();
            try
            {


                ps.clientSocket = socket;

                byte[] outgoingBuffer = ps.encoding.GetBytes(data);

                ps.buffer = outgoingBuffer;


                lock (sendSync)
                {
                    socket.BeginSend(outgoingBuffer, 0, outgoingBuffer.Length,
                        SocketFlags.None, SendCallBack, ps);
                }
            }
            catch (Exception e)
            {
                ps.exceptions = e;
                ps.callback.DynamicInvoke(ps);
            }

        }

        private static void SendCallBack(IAsyncResult ar)
        {
            PreservedState ps = (PreservedState)ar.AsyncState;

            int bytes = ps.clientSocket.EndSend(ar);

            lock (sendSync)
            {

                // The socket has been closed
                if (bytes == 0)
                {
                    //ps.clientSocket.Shutdown(SocketShutdown.Both);
                    //ps.clientSocket.Close();
                }

                //Prepend the unsent bytes and try sending again.
                else
                {
                    string data = ps.encoding.GetString(ps.buffer, bytes,
                                                        ps.buffer.Length - bytes);
                    Send(ps.clientSocket, data);
                }
            }
        }

        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void Server_Awaiting_Client_Loop(Delegate callback)
        {
            //Saves the callback method 
            PreservedState state = new PreservedState();
            state.callback = callback;

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);


            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetworkV6,
                SocketType.Stream, ProtocolType.Tcp);

            listener.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    state.clientSocket = listener;

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    Console.WriteLine("Please take any bathroom breaks and snack runs in the meantime...");
                    listener.BeginAccept(
                        new AsyncCallback(Accept_a_New_Client),
                        state);

                    Console.WriteLine("Waiting for a connection...");
                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Accept_a_New_Client(IAsyncResult ar)
        {
            allDone.Set();

            Console.WriteLine("we are in there");
            //Takes out the preserved state from the previous function.
            PreservedState state = (PreservedState)ar.AsyncState;

            Socket listener = state.clientSocket;

            //Accepts the connection attempt and returns a new socket that is used to handle communication.
            Socket handler = listener.EndAccept(ar);

            //Changes the socket of the state to the socket used for handling communcation.
            state.clientSocket = handler;

            //Remember to input a paremeter for the dynamic invoke if necessary.
            state.callback.DynamicInvoke(state);
        }
    }
}