using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Tiles
{
    class WaterTile : Tile
    {
        private int cliffSide;

        public WaterTile(int image, string name, float friction, int id) : base(image, name, friction, id)
        {
            this.cliffSide = ImageManager.RegisterImage(@"./res/Tiles/cliff1.png");
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, int x, int y, int up, int down, int left, int right)
        {
            base.Draw(g, view, world, x, y, up, down, left, right);
            if (up != this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffSide), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }
        }
    }
}
