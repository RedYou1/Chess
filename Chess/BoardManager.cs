using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chess
{
    class BoardManager
    {
        /// <summary>
        /// put back the pieces
        /// </summary>
        public void Reset()
        {
            board = new char[,]
            {
                {'R','P',' ',' ',' ',' ','p','r'},
                {'C','P',' ',' ',' ',' ','p','c'},
                {'B','P',' ',' ',' ',' ','p','b'},
                {'Q','P',' ',' ',' ',' ','p','q'},
                {'K','P',' ',' ',' ',' ','p','k'},
                {'B','P',' ',' ',' ',' ','p','b'},
                {'C','P',' ',' ',' ',' ','p','c'},
                {'R','P',' ',' ',' ',' ','p','r'}
            };
            whiteturn = true;
            wrleft = true;
            wrrigth = true;
            brleft = true;
            brrigth = true;
            whiteInDanger = false;
            blackInDanger = false;
            blacktime.Stop();
            whitetime.Restart();
            blacktime.Restart();
            blacktime.Stop();
            whitetime.Start();
        }

        public BoardManager()
        {
            whitetime.Start();
        }

        public bool whiteInDanger = false;
        public bool blackInDanger = false;

        public Stopwatch whitetime = new Stopwatch();
        public Stopwatch blacktime = new Stopwatch();

        /// <summary>
        /// return a board clone<br/>
        /// uppercase = Black<br/>
        /// lowercase = white<br/>
        /// P=pion<br/>
        /// R=rock<br/>
        /// C=knight<br/>
        /// B=bishops<br/>
        /// K=King<br/>
        /// Q=Queen
        /// </summary>
        public char[,] Board { get => (char[,])board.Clone(); }

        public bool whiteturn = true;

        private bool wrleft = true;
        private bool wrrigth = true;
        private bool brleft = true;
        private bool brrigth = true;

        /// <summary>
        /// dont check if its legal and no check for roque(just do it)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void ForceToMove(int x1, int y1, int x2, int y2)
        {
            if (x1 < 8 && x1 >= 0 && x2 < 8 && x2 >= 0 && y1 < 8 && y1 >= 0 && y2 < 8 && y2 >= 0)
            {
                if (board[x1, y1] == 'P' || board[x1, y1] == 'p')
                {
                    if (Math.Abs(x1 - x2) == 1 && Math.Abs(y1 - y2) == 1 && board[x2, y2] == ' ')
                    {
                        board[x2, y1] = ' ';
                    }
                }

                board[x2, y2] = board[x1, y1];
                board[x1, y1] = ' ';

                if (board[x2, y2] == 'K')
                {
                    if (brleft && x2 == 6 && board[7, 0] == 'R')
                    {
                        board[5, 0] = 'R';
                        board[7, 0] = ' ';
                    }
                    if (brrigth && x2 == 2 && board[3, 0] == ' ' && board[0, 0] == 'R')
                    {
                        board[3, 0] = 'R';
                        board[0, 0] = ' ';
                    }
                    brleft = false;
                    brrigth = false;
                }
                if (board[x2, y2] == 'k')
                {
                    if (wrleft && x2 == 6 && board[7, 7] == 'r')
                    {
                        board[5, 7] = 'r';
                        board[7, 7] = ' ';
                    }
                    if (wrrigth && x2 == 2 && board[3, 7] == ' ' && board[0, 7] == 'r')
                    {
                        board[3, 7] = 'r';
                        board[0, 7] = ' ';
                    }
                    wrleft = false;
                    wrrigth = false;
                }

                if (board[x2, y2] == 'P' && y2 == 7)
                {
                    board[x2, y2] = 'Q';
                }
                if (board[x2, y2] == 'p' && y2 == 0)
                {
                    board[x2, y2] = 'q';
                }
            }
        }

        /// <summary>
        /// move a piece
        /// </summary>
        /// <param name="x1">from x</param>
        /// <param name="y1">from y</param>
        /// <param name="x2">to x</param>
        /// <param name="y2">to y</param>
        public void Move(int x1, int y1, int x2, int y2, bool withsound)
        {
            if (x1 < 8 && x1 >= 0 && x2 < 8 && x2 >= 0 && y1 < 8 && y1 >= 0 && y2 < 8 && y2 >= 0)
            {
                if (LegalMove(x1, y1, x2, y2))
                {
                    if (Program.movesound != null && withsound)
                    {
                        Program.movesound.Stop();
                        Program.movesound.Play();
                    }

                    if (Char.IsLower(board[x1, y1]))
                    {
                        whitetime.Stop();
                        blacktime.Start();
                    }
                    if (Char.IsUpper(board[x1, y1]))
                    {
                        whitetime.Start();
                        blacktime.Stop();
                    }

                    if (board[x2, y2] == 'K' || board[x2, y2] == 'k')
                    {
                        Program.EndGame();
                    }
                    else
                    {
                        ForceToMove(x1, y1, x2, y2);
                    }
                }
            }
        }

        public void CheckIfKingsInDanger()
        {
            whiteInDanger = false;
            blackInDanger = false;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] == 'K')
                    {
                        if (CanBeKilled(x, y))
                        {
                            blackInDanger = true;
                        }
                    }
                    if (board[x, y] == 'k')
                    {
                        if (CanBeKilled(x, y))
                        {
                            whiteInDanger = true;
                        }
                    }
                }
            }
        }

        public bool CanBeKilled(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return false;
            }

            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8; b++)
                {
                    if (a != x && b != y)
                    {
                        if (LegalMove(a, b, x, y))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private BoardManager test(int x1, int y1, int x2, int y2)
        {
            char[,] tm = (char[,])board.Clone();
            tm[x2, y2] = tm[x1, y1];
            tm[x1, y1] = ' ';
            BoardManager nbm = new BoardManager();
            nbm.Load(BoardManager.ToString(tm));
            nbm.CheckIfKingsInDanger();
            return nbm;
        }

        public char Get(int x, int y)
        {
            return board[x, y];
        }

        public bool LegalMove(int x1, int y1, int x2, int y2)
        {
            //out of boundary
            if (x1 < 0 || x2 < 0 || x1 >= 8 || x2 >= 8 || y1 < 0 || y2 < 0 || y1 >= 8 || y2 >= 8)
            {
                return false;
            }

            if (board[x1, y1] == ' ')
            {
                return false;
            }

            //dont kill your own pieces
            if (Char.IsUpper(board[x1, y1]) == Char.IsUpper(board[x2, y2]) && Char.IsLower(board[x1, y1]) == Char.IsLower(board[x2, y2]))
            {
                return false;
            }

            //cant go on himself
            if (x1 == x2 && y1 == y2)
            {
                return false;
            }

            //if his king can be kill and cant protect him
            if (blackInDanger == true && Char.IsUpper(board[x1, y1]))
            {
                if (board[x1, y1] == 'K' && Math.Abs(x1 - x2) > 1)
                {
                    return false;
                }
                if (test(x1, y1, x2, y2).blackInDanger == true)
                {
                    return false;
                }
            }
            if (whiteInDanger == true && Char.IsLower(board[x1, y1]))
            {
                if (board[x1, y1] == 'k' && Math.Abs(x1 - x2) > 1)
                {
                    return false;
                }
                if (test(x1, y1, x2, y2).whiteInDanger == true)
                {
                    return false;
                }
            }


            switch (board[x1, y1])
            {
                case 'P':
                    {
                        bool basic = (x1 == x2
                            && (y2 - y1 == 1 || (y2 - y1 == 2
                                                && y1 == 1
                                                && TrajCheck(x1, y1, x2, y2))));
                        bool attack = (((board[x2, y2] != ' ' && Char.IsLower(board[x2, y2])) || (board[x2, y1] != ' ' && Char.IsLower(board[x2, y1]) && y1 == 4))
                                        && ((x1 == x2 + 1 || x1 == x2 - 1) && y2 - y1 == 1));
                        return (basic && board[x2, y2] == ' ') || attack;
                    }
                case 'p':
                    {
                        bool basic = (x1 == x2
                            && (y2 - y1 == -1 || (y2 - y1 == -2
                                                && y1 == 6
                                                && TrajCheck(x1, y1, x2, y2))));
                        bool attack = (((board[x2, y2] != ' ' && Char.IsUpper(board[x2, y2])) || (board[x2, y1] != ' ' && Char.IsUpper(board[x2, y1]) && y1 == 3))
                                        && ((x1 == x2 + 1 || x1 == x2 - 1) && y2 - y1 == -1));
                        return (basic && board[x2, y2] == ' ') || attack;
                    }

                case 'R':
                case 'r':
                    return (x1 == x2 || y2 == y1) && TrajCheck(x1, y1, x2, y2);

                case 'C':
                case 'c':
                    {
                        int ax = Math.Abs(x2 - x1);
                        int ay = Math.Abs(y2 - y1);
                        return (ax == 1 && ay == 2) || (ax == 2 && ay == 1);
                    }

                case 'B':
                case 'b':
                    return Math.Abs(x2 - x1) == Math.Abs(y2 - y1) && TrajCheck(x1, y1, x2, y2);

                case 'K':
                case 'k':
                    {
                        bool move = Math.Abs(x2 - x1) <= 1 && Math.Abs(y2 - y1) <= 1;
                        bool rock = false;
                        if (x2 == 2 && y2 == 0 && brrigth && board[x1, y1] == 'K' && board[0,0] == 'R' && TrajCheck(x1, y1, 1, y2))
                        {
                            rock = true;
                        }
                        if (x2 == 2 && y2 == 7 && wrrigth && board[x1, y1] == 'k' && board[0,7] == 'r' && TrajCheck(x1, y1, 1, y2))
                        {
                            rock = true;
                        }
                        if (x2 == 6 && y2 == 0 && brleft && board[x1, y1] == 'K' && board[7,0] == 'R' && TrajCheck(x1, y1, x2, y2))
                        {
                            rock = true;
                        }
                        if (x2 == 6 && y2 == 7 && wrleft && board[x1, y1] == 'k' && board[7,7] == 'r' && TrajCheck(x1, y1, x2, y2))
                        {
                            rock = true;
                        }
                        return move || rock;
                    }

                case 'Q':
                case 'q':
                    return (Math.Abs(x2 - x1) == Math.Abs(y2 - y1) || (x1 == x2 || y2 == y1)) && TrajCheck(x1, y1, x2, y2);
            }
            return false;
        }

        private bool TrajCheck(int x1, int y1, int x2, int y2)
        {
            //out of boundary
            if (x1 < 0 || x2 < 0 || x1 >= 8 || x2 >= 8 || y1 < 0 || y2 < 0 || y1 >= 8 || y2 >= 8)
            {
                return false;
            }

            int ux = (x1 - x2 > 0 ? 1 : -1);
            int uy = (y1 - y2 > 0 ? 1 : -1);
            if (x1 == x2)
            {
                ux = 0;
            }
            if (y1 == y2)
            {
                uy = 0;
            }
            int nx2 = x2 + ux;
            int ny2 = y2 + uy;
            int a = 0;
            if (x1 != nx2 || y1 != ny2)
            {
                while ((x1 != nx2 || y1 != ny2) && a < 2)
                {
                    if (nx2 >= 0 && nx2 < 8 && ny2 >= 0 && ny2 < 8)
                    {
                        a = 0;
                        if (board[nx2, ny2] != ' ')
                        {
                            return false;
                        }
                    }
                    else
                    {
                        a++;
                    }
                    nx2 += ux;
                    ny2 += uy;
                }
            }
            return true;
        }

        /// <summary>
        /// load a board from a ToString of another BoardManager
        /// </summary>
        /// <param name="b">a board in String</param>
        public void Load(string b)
        {
            char[,] board = new char[8, 8];
            int pointerx = 0;
            int pointery = 0;
            for (int i = 0; i < b.Length;)
            {
                String nums = "";
                for (; Char.IsDigit(b[i]); i++)
                {
                    nums += b[i];
                }
                char c = b[i++];
                for (int num = Int32.Parse(nums); num > 0; num--)
                {
                    board[pointerx, pointery] = c;
                    pointery++;
                    if (pointery == 8)
                    {
                        pointery = 0;
                        pointerx++;
                        if (pointery == 8 && pointerx == 8)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            this.board = board;
        }

        /// <summary>
        /// transform a string to board (from a tostring)
        /// </summary>
        /// <param name="b"></param>
        /// <returns>the board</returns>
        public static char[,] ExLoad(string b)
        {
            char[,] board = new char[8, 8];
            int pointerx = 0;
            int pointery = 0;
            for (int i = 0; i < b.Length;)
            {
                String nums = "";
                for (; Char.IsDigit(b[i]); i++)
                {
                    nums += b[i];
                }
                char c = b[i++];
                for (int num = Int32.Parse(nums); num > 0; num--)
                {
                    board[pointerx, pointery] = c;
                    pointery++;
                    if (pointery == 8)
                    {
                        pointery = 0;
                        pointerx++;
                        if (pointery == 8 && pointerx == 8)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            return board;
        }

        /// <summary>
        /// usefull to save the board in text or to export it with load in another BoardManager
        /// </summary>
        /// <returns>the board but in String</returns>
        public override string ToString()
        {
            string s = "";
            char c = board[0, 0];
            int n = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] == c)
                    {
                        n++;
                    }
                    else
                    {
                        s += n + "" + c;
                        n = 1;
                        c = board[x, y];
                    }
                }
            }
            s += n + "" + c;
            return s;
        }

        /// <summary>
        /// uppercase = Black<br/>
        /// lowercase = white<br/>
        /// P=pion<br/>
        /// R=rock<br/>
        /// C=knight<br/>
        /// B=bishop<br/>
        /// K=King<br/>
        /// Q=Queen
        /// </summary>
        private char[,] board =
        {
            {'R','P',' ',' ',' ',' ','p','r'},
            {'C','P',' ',' ',' ',' ','p','c'},
            {'B','P',' ',' ',' ',' ','p','b'},
            {'Q','P',' ',' ',' ',' ','p','q'},
            {'K','P',' ',' ',' ',' ','p','k'},
            {'B','P',' ',' ',' ',' ','p','b'},
            {'C','P',' ',' ',' ',' ','p','c'},
            {'R','P',' ',' ',' ',' ','p','r'}
        };

        /// <summary>
        /// usefull to save the board in text or to export it with load in another BoardManager
        /// </summary>
        /// <returns>the board but in String</returns>
        public static string ToString(char[,] board)
        {
            string s = "";
            char c = board[0, 0];
            int n = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] == c)
                    {
                        n++;
                    }
                    else
                    {
                        s += n + "" + c;
                        n = 1;
                        c = board[x, y];
                    }
                }
            }
            s += n + "" + c;
            return s;
        }

        public BoardManager Clone()
        {
            BoardManager bm = new BoardManager();
            bm.whiteInDanger = whiteInDanger;
            bm.blackInDanger = blackInDanger;
            bm.whitetime = whitetime;
            bm.blacktime = blacktime;
            bm.brleft = brleft;
            bm.brrigth = brrigth;
            bm.wrleft = wrleft;
            bm.wrrigth = wrrigth;
            bm.board = (char[,])board.Clone();
            return bm;
        }
    }
}
