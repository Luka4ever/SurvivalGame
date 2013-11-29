using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    class UI
    {
        private Entity player;
        private int line;
        private Font font;
        private Brush brush;

        public UI(Entity player)
        {
            this.player = player;
            this.font = new Font("Lucida Console", 14, FontStyle.Bold);
            this.brush = new SolidBrush(Color.White);
        }

        private void Reset()
        {
            this.line = 0;
        }

        private void DrawLine(Graphics g, String line)
        {
            g.DrawString(line, font, brush, 10, 10 + (this.line++) * 20);
        }

        public void Draw(Graphics g, World world)
        {
            Reset();
            DrawLine(g, "X: " + this.player.X);
            DrawLine(g, "Y: " + this.player.Y);
            DrawLine(g, "Seed: " + world.Seed);
        }
    }
}
