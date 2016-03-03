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

            try
            {
                UI_STREAM.Write(Encoding.ASCII.GetBytes("START"), 0, 5);
                LOGFILE.WriteLine(">> Sent: \"START\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
            }
            catch (Exception e)
            {
                LOGFILE.WriteLine(e.InnerException);
            }
        }

        public void SendStop()
        {
            try
            {
                UI_STREAM.Write(Encoding.ASCII.GetBytes("STOP"), 0, 4);
                LOGFILE.WriteLine(">> Sent: \"STOP\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
            }
            catch (Exception e)
            {
                LOGFILE.WriteLine(e.InnerException);
            }
        }

        public void SendKill()
        {
            try
            {
                UI_STREAM.Write(Encoding.ASCII.GetBytes("KILL"), 0, 4);
                LOGFILE.WriteLine(">> Sent: \"KILL\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
            }
            catch (Exception e)
            {
                LOGFILE.WriteLine(e.InnerException);
            }
        }

        public void SendImage(string fn)
        {
            try
            {
                string message = "IMAGE " + fn;

                UI_STREAM.Write(Encoding.ASCII.GetBytes(message), 0, Encoding.ASCII.GetByteCount(message));
                LOGFILE.WriteLine(">> Sent: \"IMAGE\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
            }
            catch (Exception e) {
                LOGFILE.WriteLine(e.InnerException);
            }
        }

        Thread networkCom = new Thread(delegate ()
        {
            while (true)
            {
                NetworkConnected = false;

                LOGFILE.WriteLine(">> LISTENING: port " + UI_PORT + " " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));

                UI_STREAM = ui.Connect();

                LOGFILE.WriteLine(">> CLIENT CONNECTED: UI " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));

                ImageCounter = 0;

                while (UI_CLIENT.Connected)
                {
                    NetworkConnected = true;
                    try {
                        byte[] buffer = new byte[UI_CLIENT.ReceiveBufferSize];
                        int bytesRead = UI_STREAM.Read(buffer, 0, UI_CLIENT.ReceiveBufferSize);
                        string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        LOGFILE.WriteLine(">> Received: \"" + data + "\" FROM SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));

                        string[] split = data.Split(' ');

                        switch (split[0])
                        {
                            case "IMAGE":
                                if (split.Length > 1)
                                {
                                    FTPImageTransfer ftp = new FTPImageTransfer("ftp://192.168.168.1", "Drone", "NEVERAGAIN");
                                    CurrentTimeStamp = DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff");
                                    ftp.Download(split[1], "PERSON.jpg");
                                }
                                else
                                {
                                    LOGFILE.WriteLine(">> ERROR: NULL PATH " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
                                }
                                break;

                            case "UPLOAD":
                                if (split.Length > 1)
                                {
                                    if(split[1] == "SUCCESS")
                                    {
                                        UploadStatus = true;
                                    }
                                    else if(split[1] == "FAIL")
                                    {
                                        UploadStatus = false;
                                    }
                                }
                                    break;

                            case "VOLTAGE": //MAX VOLTAGE 5.5 MIN VOLTAGE 3.3
                                if(split.Length > 1)
                                {
                                    float f = float.Parse(split[1]);
                                    CurrentBatteryVoltage = f;
                                }
                                else
                                {
                                    LOGFILE.WriteLine(">> ERROR: NULL VOLTAGE " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
                                }
                                break;

                            case "MOVING":
                                RobotMoving = true;
                                break;

                            case "NOTMOVING":
                                RobotMoving = false;
                                break;

                            case "SAFE":
                                SafeToDrive = true;
                                break;

                            case "UNSAFE":
                                SafeToDrive = false;
                                break;

                            case "MATCH":
                                Matched = true;
                                break;

                            case "CONFIDENCE":
                                if(split.Length > 1)
                                {
                                    float con = float.Parse(split[1]);
                                    Confidence = con;
                                }
                                break;
                            default:
                                // Invalid Signal
                                UI_STREAM.Write(Encoding.ASCII.GetBytes("NO"), 0, 2);
                                LOGFILE.WriteLine(">> Sent: \"NO\" TO SERVER " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
                                break;
                        }
                    }catch(Exception e)
                    {
                        LOGFILE.WriteLine(e.InnerException);
                        LOGFILE.WriteLine(">> CLIENT DISCONNECTED: UI " + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss.fff"));
                        Console.WriteLine(">> CLIENT DISCONNECTED: UI");
                        break;
                    }
                }
            }
        });

        private static String CurrentTimeStamp { get; set; }
        private static int ImageCounter { get; set; }
        private static int UI_PORT { get; set; }
        private static string LOCAL_IP { get; set; }

        private static float CurrentBatteryVoltage { get; set; }
        private static bool RobotMoving { get; set; }
        private static bool SafeToDrive { get; set; }
        private static bool UploadStatus { get; set; }
        private static bool Matched { get; set; }
        private static float Confidence { get; set; }

        private static bool NetworkConnected { get; set; }

        private static StreamWriter LOGFILE { get; set; }
        private static NetworkStream UI_STREAM { get; set; }
        private static TcpClient UI_CLIENT { get; set; }
        private static SocketConnection ui { get; set; }
    }
}
