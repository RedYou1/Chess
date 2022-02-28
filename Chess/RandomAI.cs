using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    static class RandomAI
    {
        /// <summary>
        /// generate and do the generated move
        /// </summary>
        /// <param name="isblack"></param>
        public static void NextMove(bool isblack)
        {
            List<KeyValuePair<Point, List<Point>>> piece = GetPieces(isblack);
            if (piece.Count == 0)
            {
                return;
            }
            Random r = new Random();
            KeyValuePair<Point, List<Point>> pi = piece[r.Next(0, piece.Count)];

            List<Point> ran = pi.Value;
            if (ran.Count == 0)
            {
                return;
            }
            Point next = ran[r.Next(0, ran.Count)];
            Program.board.Move(pi.Key.X, pi.Key.Y, next.X, next.Y,true);
        }

        private static List<Point> Moves(Point start)
        {
            List<Point> ran = new List<Point>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (Program.board.LegalMove(start.X, start.Y, x, y))
                    {
                        ran.Add(new Point(x, y));
                    }
                }
            }
            return ran;
        }
        private static List<KeyValuePair<Point, List<Point>>> GetPieces(bool isblack)
        {
            List<KeyValuePair<Point, List<Point>>> piece = new List<KeyValuePair<Point, List<Point>>>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (Program.board.Get(x, y) != ' '
                        && ((Char.IsUpper(Program.board.Get(x, y)) && isblack)
                        || (Char.IsLower(Program.board.Get(x, y)) && !isblack)))
                    {
                        List<Point> p = Moves(new Point(x, y));
                        if (p.Count > 0)
                        {
                            piece.Add(new KeyValuePair<Point, List<Point>>(new Point(x, y), p));
                        }
                    }
                }
            }
            return piece;
        }
    }
}
