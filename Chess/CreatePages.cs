using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    static class CreatePages
    {
        public static void InitPages()
        {
            List<Button> mainbuttons = new List<Button>();
            //play offline
            mainbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.whitemode = 0;
                    Program.blackmode = 0;
                    Program.ChangePage("play offline");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 8 + Size.Width / 4, Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("play offline", SystemFonts.DefaultFont);
                    gfx.DrawString("play offline", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));
            //play online
            mainbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.adresse = "";
                    Program.onadresse = false;
                    Program.ChangePage("connect");
                    //Program.ChangePage("play online");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 8 + Size.Width / 4, Size.Height / 4 + Size.Height / 64, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("play online", SystemFonts.DefaultFont);
                    gfx.DrawString("play online", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));
            mainbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.ChangePage("setting");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 8 + Size.Width / 4, 3 * Size.Height / 8 + Size.Height / 32, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("Setting", SystemFonts.DefaultFont);
                    gfx.DrawString("Setting", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));

            List<Page> pages = new List<Page>();
            Page main = new Page("main", mainbuttons.ToArray(),
                (gfx, resolution) =>
                {
                    gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, resolution.Width, resolution.Height));
                    return true;
                });
            pages.Add(main);
            Program.page = main;


            List<Button> offbuttons = new List<Button>();
            //is AI white
            offbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.whitemode++;
                    if (Program.whitemode >= Program.modes.Length)
                    {
                        Program.whitemode = 0;
                    }
                    return true;
                },
                Size =>
                {
                    Size a = CreatePages.SquareResolution(new Size(Size.Width / 5, Size.Height / 5));
                    return new Rectangle(Size.Width / 2 - Size.Width / 8 - a.Width, Size.Height / 6, a.Width, a.Height);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.FillRectangle(Brushes.White, rect);
                    gfx.DrawImage(Program.whitepion, rect);

                    String a = Program.modes[Program.whitemode];
                    Size b = TextRenderer.MeasureText(a, SystemFonts.DefaultFont);
                    gfx.DrawString(a, SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - b.Width / 2, rect.Y + rect.Height + 10));
                    return true;
                }));
            //is AI black
            offbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.blackmode++;
                    if (Program.blackmode >= Program.modes.Length)
                    {
                        Program.blackmode = 0;
                    }
                    return true;
                },
                Size =>
                {
                    Size a = CreatePages.SquareResolution(new Size(Size.Width / 5, Size.Height / 5));
                    return new Rectangle(Size.Width / 2 + Size.Width / 8, Size.Height / 6, a.Width, a.Height);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.FillRectangle(Brushes.White, rect);
                    gfx.DrawImage(Program.blackpion, rect);

                    String a = Program.modes[Program.blackmode];
                    Size b = TextRenderer.MeasureText(a, SystemFonts.DefaultFont);
                    gfx.DrawString(a, SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - b.Width / 2, rect.Y + rect.Height + 10));
                    return true;
                }));
            //Play
            offbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.board = new BoardManager();
                    Program.ChangePage("game");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 2 - Size.Width / 8, Size.Height - Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("Play", SystemFonts.DefaultFont);
                    gfx.DrawString("Play", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));
            //return mainmenu
            offbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.ChangePage("main");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width - Size.Width / 4, Size.Height - Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("Main menu", SystemFonts.DefaultFont);
                    gfx.DrawString("Main menu", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));

            Page off = new Page("play offline", offbuttons.ToArray(),
                (gfx, resolution) =>
                {
                    gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, resolution.Width, resolution.Height));
                    return true;
                });
            pages.Add(off);

            List<Button> connectbuttons = new List<Button>();
            //adresse
            connectbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.onadresse = true;
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 8, Size.Height / 8, Size.Width / 2, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Program.onadresse ? (Connection.CorrectIp(Program.adresse) ? Pens.Green : Pens.Red) : Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText(Program.adresse, SystemFonts.DefaultFont);
                    gfx.DrawString(Program.adresse, SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));
            //connect
            connectbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    if (Connection.CorrectIp(Program.adresse))
                    {
                        Program.connection = new Connection(Program.adresse);
                        Program.whitemode = 255;
                        Program.blackmode = 0;
                        Program.ChangePage("game");
                        Program.receiveThread = new System.Threading.Thread(Connection.Thread);
                        Program.receiveThread.Start();
                        Program.board.Reset();
                    }
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 2 + Size.Width / 7, Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("connect", SystemFonts.DefaultFont);
                    gfx.DrawString("connect", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));
            //wait connection
            connectbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.port = Connection.FreeTcpPort();
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length - 1];
                    Program.tempadress = new IPEndPoint(ipAddress, Program.port);
                    Program.wait = new System.Threading.Thread(Connection.WaitConnection);
                    Program.wait.Start();
                    Program.ChangePage("wait");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 7, Size.Height / 4 + Size.Height / 64, 3 * Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("wait someone to connect to you", SystemFonts.DefaultFont);
                    gfx.DrawString("wait someone to connect to you", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));
            //return mainmenu
            connectbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.ChangePage("main");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width - Size.Width / 4, Size.Height - Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("Main menu", SystemFonts.DefaultFont);
                    gfx.DrawString("Main menu", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));

            Page connect = new Page("connect", connectbuttons.ToArray(),
            (gfx, resolution) =>
            {
                gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, resolution.Width, resolution.Height));
                return true;
            });
            pages.Add(connect);

            List<Button> waitbuttons = new List<Button>();
            //return mainmenu
            waitbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.ChangePage("main");
                    Program.wait.Abort();
                    Program.wait = null;
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width - Size.Width / 4, Size.Height - Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("Main menu", SystemFonts.DefaultFont);
                    gfx.DrawString("Main menu", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));

            Page wait = new Page("wait", waitbuttons.ToArray(),
            (gfx, resolution) =>
            {
                String text = Program.tempadress.Address + ":" + Program.tempadress.Port;
                Size play = TextRenderer.MeasureText(text, SystemFonts.DefaultFont);
                gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, resolution.Width, resolution.Height));
                gfx.DrawString(text, SystemFonts.DefaultFont, Brushes.White, new Point(resolution.Width / 2 - play.Width / 2, resolution.Height / 2 - play.Height / 2));
                return true;
            });
            pages.Add(wait);

            List<Button> settingbuttons = new List<Button>();
            //volume
            settingbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    int x = MP.X - rect.X;
                    Program.movesound.Volume = (double)(x) / (double)(rect.Width);
                    Program.movesound.Stop();
                    Program.movesound.Play();
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width / 7, Size.Height / 6, Size.Width - (2 * Size.Width / 7), Size.Height / 32);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    String text = "volume";
                    Size play = TextRenderer.MeasureText(text, SystemFonts.DefaultFont);
                    gfx.DrawString(text, SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y - play.Height - play.Height / 2));

                    int ball = rect.Height * 2;
                    gfx.FillRectangle(Brushes.White, new Rectangle(rect.X, rect.Y + rect.Height / 8, rect.Width, rect.Height - rect.Height / 4));
                    gfx.FillEllipse(Brushes.White, new Rectangle((int)(rect.X + (rect.Width * Program.movesound.Volume) - (ball / 2)), rect.Y + rect.Height / 2 - (ball / 2), ball, ball));
                    return true;
                }));
            //return mainmenu
            settingbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.ChangePage("main");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width - Size.Width / 4, Size.Height - Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("Main menu", SystemFonts.DefaultFont);
                    gfx.DrawString("Main menu", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));

            Page setting = new Page("setting", settingbuttons.ToArray(),
            (gfx, resolution) =>
            {
                gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, resolution.Width, resolution.Height));
                return true;
            });
            pages.Add(setting);

            List<Button> finishedbuttons = new List<Button>();
            //return game
            finishedbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.board.Reset();
                    Program.ChangePage("game");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width - Size.Width / 4 - Size.Width / 16, Size.Height - Size.Height / 4 - Size.Height / 16, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("return game", SystemFonts.DefaultFont);
                    gfx.DrawString("return game", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));
            //return mainmenu
            finishedbuttons.Add(new Button(
                (MP, resolution, button) =>
                {
                    Program.ChangePage("main");
                    return true;
                },
                Size =>
                {
                    return new Rectangle(Size.Width - Size.Width / 4 - Size.Width / 16, Size.Height - Size.Height / 8, Size.Width / 4, Size.Height / 8);
                },
                (gfx, resolution, button) =>
                {
                    Rectangle rect = button.rectangle(resolution);
                    gfx.DrawRectangle(Pens.White, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
                    Size play = TextRenderer.MeasureText("Main menu", SystemFonts.DefaultFont);
                    gfx.DrawString("Main menu", SystemFonts.DefaultFont, Brushes.White, new Point(rect.X + rect.Width / 2 - play.Width / 2, rect.Y + rect.Height / 2 - play.Height / 2));
                    return true;
                }));

            int gameindex = pages.Count+1;
            Page finished = new Page("finished", finishedbuttons.ToArray(),
             (gfx, res) =>
             {
                 Program.pages[gameindex].Draw(gfx,res);
                 Size rect = Program.GetGameSpace(res);
                 gfx.FillRectangle(new SolidBrush(Color.FromArgb(100,255,0,0)), new Rectangle(0, 0, rect.Width, rect.Height));
                 gfx.DrawString((Program.board.whiteturn ? "white" : "black") + " won", SystemFonts.DefaultFont, Brushes.White, new Point(res.Width / 2, res.Height / 2));
                 return true;
             });
            pages.Add(finished);

            Page game = new Page("game", new Button[0],
                (gfx, resolution) =>
                {
                    gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, resolution.Width, resolution.Height));
                    String turn = (Program.board.whiteturn ? "white" : "black") + " turn";
                    Size slength = TextRenderer.MeasureText(turn, SystemFonts.DefaultFont);

                    int xstart = (int)(resolution.Width - (slength.Width * 1.05f));

                    Size gres = SquareResolution(new Size(xstart, resolution.Height));

                    int xmid = (int)(gres.Width + ((resolution.Width - gres.Width) / 2));

                    gfx.DrawString(turn, SystemFonts.DefaultFont, Brushes.White, new Point(xmid - slength.Width / 2, 10));

                    int wsec = (int)(Program.board.whitetime.ElapsedMilliseconds / 1000 % 60);
                    String wtstring = "white: " + (Program.board.whitetime.ElapsedMilliseconds / 60000) + ":" + (wsec < 10 ? "0" : "") + "" + wsec;
                    Size wtlength = TextRenderer.MeasureText(wtstring, SystemFonts.DefaultFont);
                    gfx.DrawString(wtstring, SystemFonts.DefaultFont, Brushes.White, new Point(xmid - wtlength.Width / 2, 10 + slength.Height * 2));

                    int bsec = (int)(Program.board.blacktime.ElapsedMilliseconds / 1000 % 60);
                    String btstring = "black: " + (Program.board.blacktime.ElapsedMilliseconds / 60000) + ":" + (bsec < 10 ? "0" : "") + "" + bsec;
                    Size btlength = TextRenderer.MeasureText(btstring, SystemFonts.DefaultFont);
                    gfx.DrawString(btstring, SystemFonts.DefaultFont, Brushes.White, new Point(xmid - btlength.Width / 2, 10 + slength.Height * 2 + wtlength.Height * 2));
                    
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            System.Drawing.Brush b = Brushes.Black;
                            if (x % 2 == y % 2)
                            {
                                b = Brushes.White;
                            }

                            if (Program.board.Board[x, y] == 'K')
                            {
                                if (Program.board.blackInDanger == true)
                                {
                                    b = Brushes.Red;
                                }
                            }
                            if (Program.board.Board[x, y] == 'k')
                            {
                                if (Program.board.whiteInDanger == true)
                                {
                                    b = Brushes.Red;
                                }
                            }

                            if (Program.selected.X >= 0 && Program.selected.X < 8 && Program.selected.Y >= 0 && Program.selected.Y < 8)
                            {
                                if (Program.board.LegalMove(Program.selected.X, Program.selected.Y, x, y))
                                {
                                    if (Program.board.Board[x, y] == ' ')
                                    {
                                        b = Brushes.Green;
                                        if (Math.Abs(Program.selected.X - x) == 1
                                            && Math.Abs(Program.selected.Y - y) == 1
                                            && ((Program.board.Get(Program.selected.X, Program.selected.Y) == 'P' && Program.selected.Y == 4)
                                                || (Program.board.Get(Program.selected.X, Program.selected.Y) == 'p' && Program.selected.Y == 3)))
                                        {
                                            b = Brushes.Red;
                                        }
                                    }
                                    else
                                    {
                                        b = Brushes.Red;
                                    }
                                }
                                if (Program.selected.X == x && Program.selected.Y == y)
                                {
                                    b = Brushes.Blue;
                                }
                            }

                            gfx.FillRectangle(b, new RectangleF(x * (gres.Width / 8), y * (gres.Height / 8), gres.Width / 8, gres.Height / 8));

                            Image piece = Program.blackpion;
                            switch (Program.board.Board[x, y])
                            {
                                case 'P':
                                    piece = Program.blackpion;
                                    break;
                                case 'p':
                                    piece = Program.whitepion;
                                    break;
                                case 'C':
                                    piece = Program.blackknight;
                                    break;
                                case 'c':
                                    piece = Program.whiteknight;
                                    break;
                                case 'R':
                                    piece = Program.blackrock;
                                    break;
                                case 'r':
                                    piece = Program.whiterock;
                                    break;
                                case 'K':
                                    piece = Program.blackking;
                                    break;
                                case 'k':
                                    piece = Program.whiteking;
                                    break;
                                case 'Q':
                                    piece = Program.blackqueen;
                                    break;
                                case 'q':
                                    piece = Program.whitequeen;
                                    break;
                                case 'B':
                                    piece = Program.blackbishop;
                                    break;
                                case 'b':
                                    piece = Program.whitebishop;
                                    break;
                            }
                            if (Program.board.Board[x, y] != ' ')
                            {
                                gfx.DrawImage(piece, new RectangleF(x * (gres.Width / 8), y * (gres.Height / 8), gres.Width / 8, gres.Height / 8));
                            }
                        }
                    }
                    if (Program.page.name == "game") {
                        if (Program.board.whiteturn == false && Program.blackmode == 1)
                        {
                            RandomAI.NextMove(!Program.board.whiteturn);
                            Program.board.whiteturn = !Program.board.whiteturn;
                        }
                        else if (Program.board.whiteturn == true && Program.whitemode == 1)
                        {
                            RandomAI.NextMove(!Program.board.whiteturn);
                            Program.board.whiteturn = !Program.board.whiteturn;
                        }
                        if (Program.board.whiteturn == false && Program.blackmode == 2)
                        {
                            AI.NextMove(Program.board.whiteturn, 3);
                            Program.board.whiteturn = !Program.board.whiteturn;
                        }
                        else if (Program.board.whiteturn == true && Program.whitemode == 2)
                        {
                            AI.NextMove(Program.board.whiteturn, 3);
                            Program.board.whiteturn = !Program.board.whiteturn;
                        }
                    }
                    return true;
                });
            pages.Add(game);

            Program.pages = pages.ToArray();
        }

        public static Size SquareResolution(Size res)
        {
            int a = res.Width < res.Height ? res.Width : res.Height;
            return new Size(a, a);
        }
    }
}
