using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Game : BaseForm
    {
        public Size Resolution { get => resolution; set { resolution = value; } }
        private Size resolution = new Size(800, 800);

        public Bitmap Draw()
        {
            Bitmap img = new Bitmap(resolution.Width, resolution.Height);
            Graphics gfx = Graphics.FromImage(img);
            Program.page.Draw(gfx, resolution);
            for (int i = 0; i < Program.page.buttons.Length; i++)
            {
                Button button = Program.page.buttons[i];
                button.Draw(gfx, resolution, button);
            }
            return img;
        }

        public void Load()
        {

        }

        public void Unload()
        {

        }

        public void Update(TimeSpan gameTime)
        {
            if (Program.tomove.Count > 0)
            {
                for (int a = 0; a < Program.tomove.Count; a++)
                {
                    KeyValuePair<Point, Point> l = Program.tomove[a];
                    Program.board.Move(l.Key.X, l.Key.Y, l.Value.X, l.Value.Y,true);
                }
                Program.tomove = new List<KeyValuePair<Point, Point>>();
            }
        }
    }
}
