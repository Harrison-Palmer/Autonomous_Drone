using System;
using System.Net;
using System.Net.Sockets;

namespace Server_Socket_Router
{
    class SocketConnection
    {

        int iPort;
        String strHost;
        TcpListener listener;
        TcpClient client;

        public SocketConnection(int port, string host)
        {
            iPort = port;
            strHost = host;
            listener = new TcpListener(IPAddress.Parse(strHost), iPort);
        }

        public NetworkStream Connect() //should be in a thread
        {
            listener.Start();
            client = listener.AcceptTcpClient();
            return client.GetStream();
        }

        public TcpClient getClient()
        {
            return client;
        }

        /*
        void write(string str)
        {
            if (netStream != null)
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(str);
                netStream.Write(b, 0, b.Length);
            }
        }*/

    }
}
