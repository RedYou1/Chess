using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Chess
{
    static class Program
    {

        public static BoardManager board = new BoardManager();
        public static Point selected = new Point(-1, -1);

        public static IPEndPoint tempadress;
        public static String adresse;
        public static bool onadresse = false;
        public static Connection connection;
        public static Thread wait;
        public static int port;
        public static bool closed = false;
        public static Thread receiveThread;

        public static System.Windows.Media.MediaPlayer movesound;
        public static float volume = 1f;

        public static Image blackpion;
        public static Image blackrock;
        public static Image blackknight;
        public static Image blackbishop;
        public static Image blackking;
        public static Image blackqueen;

        public static Image whitepion;
        public static Image whiterock;
        public static Image whiteknight;
        public static Image whitebishop;
        public static Image whiteking;
        public static Image whitequeen;

        public static Page[] pages;
        public static Page page;

        public static List<KeyValuePair<Point, Point>> tomove = new List<KeyValuePair<Point, Point>>();


        public static byte whitemode = 0;
        public static byte blackmode = 0;
        public static String[] modes = { "Player", "Random AI", "Basic AI (strange)" };

        public static void ChangePage(String name)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                if (pages[i].name == name)
                {
                    page = pages[i];
                }
            }
        }

        public static bool canfinish = true;

        public static void EndGame()
        {
            if (canfinish)
            {
                Program.board.whiteturn = !Program.board.whiteturn;
                Program.board.blacktime.Stop();
                Program.board.whitetime.Stop();
                Program.ChangePage("finished");
            }
        }

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String folder = @"C:\Users\jcdem\source\repos\Chess\Chess\chess pieces\";
            if (!File.Exists(folder + "black pion.png"))
            {
                folder = @".\chess pieces\";
            }
            if (File.Exists(folder + "black pion.png"))
            {
                blackpion = Bitmap.FromFile(folder + "black pion.png");
            }
            if (File.Exists(folder + "white pion.png"))
            {
                whitepion = Bitmap.FromFile(folder + "white pion.png");
            }
            if (File.Exists(folder + "black knight.png"))
            {
                blackknight = Bitmap.FromFile(folder + "black knight.png");
            }
            if (File.Exists(folder + "white knight.png"))
            {
                whiteknight = Bitmap.FromFile(folder + "white knight.png");
            }
            if (File.Exists(folder + "black rock.png"))
            {
                blackrock = Bitmap.FromFile(folder + "black rock.png");
            }
            if (File.Exists(folder + "white rock.png"))
            {
                whiterock = Bitmap.FromFile(folder + "white rock.png");
            }
            if (File.Exists(folder + "black bishop.png"))
            {
                blackbishop = Bitmap.FromFile(folder + "black bishop.png");
            }
            if (File.Exists(folder + "white bishop.png"))
            {
                whitebishop = Bitmap.FromFile(folder + "white bishop.png");
            }
            if (File.Exists(folder + "black king.png"))
            {
                blackking = Bitmap.FromFile(folder + "black king.png");
            }
            if (File.Exists(folder + "white king.png"))
            {
                whiteking = Bitmap.FromFile(folder + "white king.png");
            }
            if (File.Exists(folder + "black queen.png"))
            {
                blackqueen = Bitmap.FromFile(folder + "black queen.png");
            }
            if (File.Exists(folder + "white queen.png"))
            {
                whitequeen = Bitmap.FromFile(folder + "white queen.png");
            }

            if (File.Exists(folder + "move.wav"))
            {
                movesound = new System.Windows.Media.MediaPlayer();
                movesound.Open(new Uri(folder + "move.wav", UriKind.Absolute));
            }

            CreatePages.InitPages();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static Size GetGameSpace(Size res)
        {
            String turn = (Program.board.whiteturn ? "white" : "black") + " turn";
            int slength = TextRenderer.MeasureText(turn, SystemFonts.DefaultFont).Width;

            int xstart = (int)(res.Width - (slength * 1.05f));

            return CreatePages.SquareResolution(new Size(xstart, res.Height));
        }
    }
}
