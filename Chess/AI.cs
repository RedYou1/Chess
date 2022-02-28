using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Chess
{
    /// <summary>
    /// a struct for the predict AI
    /// </summary>
    struct PG
    {
        public int score;
        public List<KeyValuePair<Point, Point>> poss;
    }

    class AI
    {
        public static void NextMove(bool whiteturn, uint strength)
        {

            Program.canfinish = false;

            PG a = Predict(Program.board, whiteturn, strength);
            Random r = new Random();

            Program.canfinish = true;
            if (a.poss.Count == 0)
            {
                Program.board.whiteturn = !Program.board.whiteturn;
                Program.EndGame();
                return;
            }
            KeyValuePair<Point, Point> poss = a.poss[r.Next(0, a.poss.Count)];

            if (Program.page.name == "game")
            {
                Program.board.Move(poss.Key.X, poss.Key.Y, poss.Value.X, poss.Value.Y, false);
            }
        }

        /// <summary>
        /// make the tree of possibilities
        /// </summary>
        /// <param name="boardS"></param>
        /// <param name="whiteturn"></param>
        /// <param name="strength"></param>
        /// <returns>the best moves</returns>
        private static PG Predict(BoardManager boardO, bool whiteturn, uint strength)
        {
            if (strength <= 0)
            {
                PG h = new PG();
                h.score = GetScoreFor(whiteturn, boardO.Board);
                return h;
            }
            PG score = new PG();
            score.score = GetScoreFor(!whiteturn, boardO.Board);
            score.poss = new List<KeyValuePair<Point, Point>>();
            for (byte x1 = 0; x1 < 8; x1++)
            {
                for (byte y1 = 0; y1 < 8; y1++)
                {
                    BoardManager board = boardO.Clone();
                    char c = board.Get(x1, y1);
                    if (c != ' ' && ((Char.IsUpper(c) && !whiteturn) || (Char.IsLower(c) && whiteturn)))
                    {
                        for (byte x2 = 0; x2 < 8; x2++)
                        {
                            for (byte y2 = 0; y2 < 8; y2++)
                            {
                                if (!(x1 == x2 && y1 == y2))
                                {
                                    if (board.LegalMove(x1, y1, x2, y2))
                                    {
                                        board.ForceToMove(x1, y1, x2, y2);
                                        PG tscore = Predict(board, !whiteturn, strength - 1);
                                        tscore.score *= -1;
                                        if (tscore.score > score.score)
                                        {
                                            score.score = tscore.score;
                                            score.poss = new List<KeyValuePair<Point, Point>>();
                                            score.poss.Add(new KeyValuePair<Point, Point>(new Point(x1, y1), new Point(x2, y2)));
                                        }
                                        if (tscore.score == score.score)
                                        {
                                            score.poss.Add(new KeyValuePair<Point, Point>(new Point(x1, y1), new Point(x2, y2)));
                                        }
                                        board = boardO.Clone();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return score;
        }

        private static int GetScoreFor(bool iswhite, char[,] board)
        {
            int score = 0;
            for (byte x = 0; x < 8; x++)
            {
                for (byte y = 0; y < 8; y++)
                {
                    char c = board[x, y];
                    if (c != ' ')
                    {
                        score += (Char.IsUpper(c) ? -1 : 1) * PieceScore(c);
                    }
                }
            }
            if (iswhite)
            {
                score *= -1;
            }
            return score;
        }

        private static int PieceScore(char c)
        {
            switch (Char.ToUpper(c))
            {
                case 'P':
                    return 1;
                case 'R':
                    return 5;
                case 'C':
                    return 3;
                case 'B':
                    return 3;
                case 'Q':
                    return 9;
                case 'K':
                    return 90;
            }
            return 0;
        }
    }
}
