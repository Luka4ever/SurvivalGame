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
        protected int id;
        private int image;
        private string name;
        private float friction;
        private static List<Tile> tiles = new List<Tile>();
        public static int size = 32;

        public Tile(int image, string name, float friction, int id)
        {
            this.image = image;
            this.name = name;
            this.friction = friction;
            this.id = id;
        }

        public virtual void Tick(World world, int x, int y)
        {

        }

        public string Name
        {
            get { return this.name; }
        }

        public virtual void Draw(Graphics g, View view, World world, int x, int y, int up, int down, int left, int right)
        {
            g.DrawImage(ImageManager.GetImage(this.image), x * Tile.size - view.GetX(), y * Tile.size - view.GetY(), 32, 32);
        }

        public virtual void Create(World world, int x, int y)
        {
            
        }

        public static void Init()
        {
            tiles.Add(new VoidTile(-1, "Void", 2f, 0));
            tiles.Add(new WaterTile(ImageManager.RegisterImage(@"res/Tiles/water1.png"), "Water", 0.5f, 1));
            tiles.Add(new GrassTile(ImageManager.RegisterImage(@"res/Tiles/grass1.png"), "Grass", 0.9f, 2));
            tiles.Add(new DirtTile(ImageManager.RegisterImage(@"res/Tiles/dirt1.png"), "Dirt", 0.9f, 3));
            tiles.Add(new SandTile(ImageManager.RegisterImage(@"res/Tiles/sand1.png"), "sand", 0.7f, 4));
        }

        public static Tile GetTile(int index)
        {
            return tiles[index];
        }

        public virtual float GetFriction()
        {
            return this.friction;
        }
    }
}
