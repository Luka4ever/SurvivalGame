using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Tiles
{
    class CliffTile : Tile
    {
        private int[] cliffTexture = new int[8];

        public CliffTile(int image, string name, float friction, int id)
            : base(image, name, friction, id)
        {
            this.cliffTexture[0] = ImageManager.RegisterImage(@"./res/Tiles/cliffNorth.png");
            this.cliffTexture[1] = ImageManager.RegisterImage(@"./res/Tiles/cliffNorthEast.png");
            this.cliffTexture[2] = ImageManager.RegisterImage(@"./res/Tiles/cliffEast.png");
            this.cliffTexture[3] = ImageManager.RegisterImage(@"./res/Tiles/cliffSouthEast.png");
            this.cliffTexture[4] = ImageManager.RegisterImage(@"./res/Tiles/cliffSouth.png");
            this.cliffTexture[5] = ImageManager.RegisterImage(@"./res/Tiles/cliffSouthWest.png");
            this.cliffTexture[6] = ImageManager.RegisterImage(@"./res/Tiles/cliffWest.png");
            this.cliffTexture[7] = ImageManager.RegisterImage(@"./res/Tiles/cliffNorthWest.png");
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, int x, int y, int up, int down, int left, int right)
        {
            base.Draw(g, view, world, x, y, up, down, left, right);

            //north
            if (up != this.id && left == this.id && right == this.id && down == this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[0]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }

            if (up != this.id && left == this.id && right != this.id && down == this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[1]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }

            if (up == this.id && left == this.id && right != this.id && down == this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[2]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }

            if (up == this.id && left == this.id && right != this.id && down != this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[3]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }

            if (up == this.id && left == this.id && right == this.id && down != this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[4]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }

            if (up == this.id && left != this.id && right == this.id && down != this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[5]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }

            if (up == this.id && left != this.id && right == this.id && down == this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[6]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }

            if (up != this.id && left != this.id && right == this.id && down == this.id)
            {
                g.DrawImage(ImageManager.GetImage(this.cliffTexture[7]), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
            }
        }
    }
}