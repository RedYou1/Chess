using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Loop
    {
        private BaseForm firstpanel;

        /// <summary>
        /// Status of GameLoop
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// Load Game into GameLoop
        /// </summary>
        public void Load(BaseForm gameObj)
        {
            firstpanel = gameObj;
        }

        /// <summary>
        /// Start GameLoop
        /// </summary>
        public async void Start()
        {
            if (firstpanel == null)
                throw new ArgumentException("Game not loaded!");


            // Set gameloop state
            Running = true;

            // Set previous game time
            DateTime _previousGameTime = DateTime.Now;

            while (Running)
            {
                // Calculate the time elapsed since the last game loop cycle
                TimeSpan GameTime = DateTime.Now - _previousGameTime;
                // Update the current previous game time
                _previousGameTime = _previousGameTime + GameTime;
                // Update the game
                firstpanel.Update(GameTime);
                // Update Game at 60fps
                await Task.Delay(10);
            }
        }

        /// <summary>
        /// Stop GameLoop
        /// </summary>
        public void Stop()
        {
            Program.closed = true;
            if (Program.wait != null)
            {
                Program.wait = null;
            }
            if (Program.receiveThread != null)
            {
                Program.receiveThread = null;
            }
            Running = false;
            firstpanel?.Unload();
        }

        /// <summary>
        /// Draw Game Graphics
        /// </summary>
        public void Draw(Graphics gfx)
        {
            gfx.DrawImage(firstpanel.Draw(), new Rectangle(0, 0, firstpanel.Resolution.Width, firstpanel.Resolution.Height));
        }
    }
}
