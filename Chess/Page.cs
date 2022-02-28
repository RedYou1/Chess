using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Page
    {
        public Button[] buttons;
        public Func<Graphics, Size, bool> Draw;
        public String name;

        public Page(String name, Button[] buttons, Func<Graphics, Size, bool> Draw)
        {
            this.name = name;
            this.buttons = buttons;
            this.Draw = Draw;
        }
    }
}
