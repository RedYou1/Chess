using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        Timer graphicsTimer;

        Loop gameLoop;

        BaseForm game;

        public Form1()
        {
            this.SetStyle(
    ControlStyles.AllPaintingInWmPaint |
    ControlStyles.UserPaint |
    ControlStyles.DoubleBuffer,
    true);
            InitializeComponent();
            // Initialize Paint Event
            Paint += Form1_Paint;
            MouseClick += Form1_MouseClick;
            KeyPress += Form1_KeyPressed;
            FormClosed += Form1_Cloed;
            // Initialize graphicsTimer
            graphicsTimer = new Timer();
            graphicsTimer.Interval = 1000 / 120;
            graphicsTimer.Tick += GraphicsTimer_Tick;
        }

        private void Form1_Cloed(object sender, EventArgs e)
        {
            gameLoop.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize Game
            game = new Game();
            game.Resolution = this.ClientSize;

            // Initialize & Start GameLoop
            gameLoop = new Loop();
            gameLoop.Load(game);
            gameLoop.Start();

            // Start Graphics Timer
            graphicsTimer.Start();
        }

        private void Form1_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (Program.page.name == "connect" && Program.onadresse == true)
            {
                char c = e.KeyChar;
                if (c == '\b')
                {
                    if (Program.adresse.Length > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Program.adresse);
                        sb.Remove(Program.adresse.Length - 1, 1);
                        Program.adresse = sb.ToString();
                    }
                }
                else if (c != '\r')
                {
                    Program.adresse += c;
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Size res = this.ClientSize;

                if (Program.page.name == "connect")
                {
                    Program.onadresse = false;
                }

                for (int i = 0; i < Program.page.buttons.Length; i++)
                {
                    Button button = Program.page.buttons[i];
                    if (button.rectangle(this.ClientSize).IntersectsWith(new Rectangle(e.X, e.Y, 1, 1)))
                    {
                        button.MousePressed(e, this.ClientSize, button);
                        break;
                    }
                }
                if (Program.page.name == "game")
                {
                    res = Program.GetGameSpace(res);

                    if (Program.board.whiteturn && Program.whitemode != 0)
                    {
                        return;
                    }
                    if (!Program.board.whiteturn && Program.blackmode != 0)
                    {
                        return;
                    }

                    int x = (int)((float)e.X / res.Width * 8);
                    int y = (int)((float)e.Y / res.Height * 8);

                    if (x < 0 || x > 7 || y < 0 || y > 7)
                    {
                        return;
                    }

                    if (Program.board.LegalMove(Program.selected.X, Program.selected.Y, x, y))
                    {
                        bool a = false;
                        if (Program.board.Get(x, y) == 'K' || Program.board.Get(x, y) == 'k')
                        {
                            
                            a = true;
                        }

                        Program.board.Move(Program.selected.X, Program.selected.Y, x, y,true);

                        if (Program.blackmode == 255 || Program.whitemode == 255)
                        {
                            Program.connection.Send("MOVE " + Program.selected.X + " " + Program.selected.Y + " " + x + " " + y);
                        }

                        if (a)
                        {
                            Program.ChangePage("finished");
                        }
                        else
                        { 
                            Program.board.whiteturn = !Program.board.whiteturn;
                        }
                        Program.selected = new Point(-1, -1);
                    }
                    else
                    {
                        if ((Program.board.whiteturn && Char.IsLower(Program.board.Board[x, y])) || (!Program.board.whiteturn && Char.IsUpper(Program.board.Board[x, y])))
                        {
                            Program.selected = new Point(x, y);
                        }
                        else
                        {
                            Program.selected = new Point(-1, -1);
                        }
                    }

                    Program.board.CheckIfKingsInDanger();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            game.Resolution = this.ClientSize;
            // Draw game graphics on Form1
            gameLoop.Draw(e.Graphics);
        }

        private void GraphicsTimer_Tick(object sender, EventArgs e)
        {
            // Refresh Form1 graphics
            Invalidate();
        }
    }
}
