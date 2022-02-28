using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    class Button
    {
        public Func<MouseEventArgs,Size,Button,bool> MousePressed;
        public Func<Size,Rectangle> rectangle;
        public Func<Graphics,Size,Button,bool> Draw;
        public Button(Func<MouseEventArgs,Size,Button,bool> MousePressed,Func<Size,Rectangle> rectangle,Func<Graphics,Size,Button,bool> Draw)
        {
            this.MousePressed = MousePressed;
            this.rectangle = rectangle;
            this.Draw = Draw;
        }
    }
}
