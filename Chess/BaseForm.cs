using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    interface BaseForm
    {
        Size Resolution { get; set; }
        Bitmap Draw();
        void Load();
        void Update(TimeSpan gameTime);
        void Unload();
    }
}
