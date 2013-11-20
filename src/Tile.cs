using SurvivalGame.src.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    abstract class Tile
    {
        private int image;
        private string name;
        private float friction;
        private static List<Tile> tiles = new List<Tile>();
        public static int size = 32;

        public Tile(int image, string name, float friction)
        {
            this.image = image;
            this.name = name;
            this.friction = friction;
        }

        public virtual void Tick(World world, int x, int y)
        {

        }

        public string Name
        {
            get { return this.name; }
        }

        public virtual void Draw(Graphics g, View view, World world, int x, int y)
        {
            //TODO: fix
            g.DrawImage(ImageManager.GetImage(this.image), x * Tile.size - view.GetX(), y * Tile.size - view.GetY());
        }

        public virtual void Create(World world, int x, int y)
        {
            
        }

        public static void Init()
        {
            tiles.Add(new WaterTile(ImageManager.RegisterImage(@"res/water/1.png"), "Water", 0.5f));
        }

        public static Tile GetTile(int index)
        {
            return tiles[index];
        }
    }
}
