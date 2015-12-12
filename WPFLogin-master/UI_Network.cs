using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace WpfApp1
{
    class SocketConnection
    {

        int iPort;
        String strHost;
        Socket connection;

        public SocketConnection(int port, string host)
        {
                iPort = port;
                strHost = host;
                connection = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
                connection.Connect(strHost, iPort);
        }

        public NetworkStream Connect() //should be in a thread
        {
            return new NetworkStream(connection);
        }

    }

    class UI_Network
    {
        public UI_Network()
        {
            UI_PORT = 18000;

            LOCAL_IP = "192.168.168.1";
            LOGFILE = new StreamWriter("UI_Network_log.txt");
            LOGFILE.AutoFlush = true;

            ui = new SocketConnection(UI_PORT, LOCAL_IP);

            //if (UI_STREAM.CanWrite)
                UI_STREAM = ui.Connect();
            //else
                //Console.Write("");
        }

        public void SendStart()
        {
            UI_STREAM.Write(Encoding.ASCII.GetBytes("START"), 0, 5);
            LOGFILE.WriteLine(">> Sent: \"START\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
        }

        public void SendStop()
        {
            UI_STREAM.Write(Encoding.ASCII.GetBytes("STOP"), 0, 4);
            LOGFILE.WriteLine(">> Sent: \"STOP\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
        }

        public void SendKill()
        {
            UI_STREAM.Write(Encoding.ASCII.GetBytes("KILL"), 0, 4);
            LOGFILE.WriteLine(">> Sent: \"KILL\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
        }

        public void SendImage(string fn)
        {
            UI_STREAM.Write(Encoding.ASCII.GetBytes("IMAGE"), 0, 5);
            LOGFILE.WriteLine(">> Sent: \"IMAGE\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
            UI_STREAM.Write(Encoding.ASCII.GetBytes(fn), 0, Encoding.ASCII.GetByteCount(fn));
            LOGFILE.WriteLine(">> Sent: \"" + fn + "\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
        }



        Thread networkCom = new Thread(delegate ()
        {
            LOGFILE.WriteLine(">> LISTENING: port " + UI_PORT + " " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));

            UI_STREAM = ui.Connect();

            LOGFILE.WriteLine(">> CLIENT CONNECTED: UI " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));


            while (true)
            {
                if (UI_CLIENT == null)
                {
                    LOGFILE.WriteLine(">> CLIENT DISCONNECTED: UI " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
                    Console.WriteLine(">> CLIENT DISCONNECTED: UI");
                    break;
                }
                byte[] buffer = new byte[UI_CLIENT.ReceiveBufferSize];
                int bytesRead = UI_STREAM.Read(buffer, 0, UI_CLIENT.ReceiveBufferSize);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                LOGFILE.WriteLine(">> Received: \"" + data + "\" FROM SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));

                switch (data)
                {
                    case "IMAGE":
                        // server is going to tell UI to fetch image from server
                        buffer = new byte[UI_CLIENT.ReceiveBufferSize];
                        bytesRead = UI_STREAM.Read(buffer, 0, UI_CLIENT.ReceiveBufferSize);
                        string filename = Encoding.ASCII.GetString(buffer, 0, bytesRead); // Get Filename
                        LOGFILE.WriteLine(">> Received: \"" + filename + "\" FROM SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
                        // TODO: DO FTP STUFF TO GET <filename> OFF OF SERVER

                        break;

                    default:
                        // Invalid Signal
                        UI_STREAM.Write(Encoding.ASCII.GetBytes("NO"), 0, 2);
                        LOGFILE.WriteLine(">> Sent: \"NO\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
                        break;
                }
            }
        });

        private static int UI_PORT { get; set; }
        private static string LOCAL_IP { get; set; }
        private static StreamWriter LOGFILE { get; set; }
        private static NetworkStream UI_STREAM { get; set; }
        private static TcpClient UI_CLIENT { get; set; }
        private static SocketConnection ui { get; set; }
    }
}
