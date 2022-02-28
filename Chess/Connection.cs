using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chess
{
    //https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=net-5.0
    class Connection
    {
        Socket socket;


        public Connection(String adress)
        {
            if (!CorrectIp(adress))
            {
                throw new ArgumentException();
            }
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(adress.Split(':')[0]).Address, Int32.Parse(adress.Split(':')[1]));
            socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
        }
        public Connection(Socket soc)
        {
            socket = soc.Accept();
        }

        public void Send(String message)
        {
            byte[] m = Encoding.ASCII.GetBytes(message);
            socket.Send(m, m.Length, 0);
        }

        public String Receive()
        {
            String page = "";
            while (page == "")
            {
                // Receive the server home page content.
                Byte[] bytesReceived = new Byte[256];
                page = Encoding.ASCII.GetString(bytesReceived, 0, socket.Receive(bytesReceived, bytesReceived.Length, 0));
            }
            return page;
        }

        public static bool CorrectIp(String adress)
        {
            IPAddress a;
            int b;
            if (!adress.Contains(':'))
            {
                return false;
            }
            return IPAddress.TryParse(adress.Split(':')[0], out a) && Int32.TryParse(adress.Split(':')[1], out b);
        }

        public static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        public static void Thread()
        {
            bool continu = true;
            while (!Program.closed && continu)
            {
                String[] m = Program.connection.Receive().Split(' ');
                switch (m[0])
                {
                    case "MOVE":
                        int x1 = Int32.Parse(m[1]);
                        int y1 = Int32.Parse(m[2]);
                        int x2 = Int32.Parse(m[3]);
                        int y2 = Int32.Parse(m[4]);
                        char a = Program.board.Get(x2, y2);
                        if (a == 'K' || a == 'k')
                        {
                            continu = false;
                        }
                        Program.tomove.Add(new System.Collections.Generic.KeyValuePair<System.Drawing.Point, System.Drawing.Point>(
                            new System.Drawing.Point(x1, y1), new System.Drawing.Point(x2, y2)));
                        Program.board.whiteturn = !Program.board.whiteturn;
                        Program.selected = new System.Drawing.Point(-1, -1);
                        break;
                }
            }
        }

        public static void WaitConnection()
        {
            Socket listener = new Socket(Program.tempadress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(Program.tempadress);
            listener.Listen(1);


            Program.connection = new Connection(listener);
            Program.whitemode = 0;
            Program.blackmode = 255;
            Program.receiveThread = new System.Threading.Thread(Connection.Thread);
            Program.receiveThread.Start();
            Program.board.Reset();
            Program.ChangePage("game");
        }
    }
}
