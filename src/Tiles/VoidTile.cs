using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Tiles
{
    class VoidTile : Tile
    {
        public VoidTile(int image, string name, float friction, int id) : base(image, name, friction, id)
        {

        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, int x, int y, int up, int down, int left, int right)
        {
            g.FillRectangle(new SolidBrush(Color.Black), x * Tile.size - view.GetX(), y * Tile.size - view.GetY(), 32, 32);
        }
    }
}
