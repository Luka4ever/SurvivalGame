using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    class Chunk
    {
        private int[] tiles;
        public static readonly int size = 64;
        private int x;
        private int y;
        private List<Entity> entities;
        private MetaCollection meta;
        private int[,] pathing;

        public Chunk(int x, int y)
        {
            this.tiles = new int[size * size];
            this.x = x;
            this.y = y;
            this.entities = new List<Entity>();
            this.meta = new MetaCollection();
            this.pathing = new int[size * size, size * size];
        }

        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }

        public int GetTile(int x, int y)
        {
            this.Localize(ref x, ref y);
            return this.tiles[this.ToIndex(x, y)];
        }

        public void SetTile(int x, int y, int type)
        {
            this.Localize(ref x, ref y);
            this.tiles[this.ToIndex(x, y)] = type;
        }

        public void Tick(World world)
        {

        }

        public void Draw(Graphics g, View view, World world)
        {
            int X = this.x * size;
            int Y = this.y * size;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Tile.GetTile(this.tiles[this.ToIndex(x, y)]).Draw(g, view, world, X + x, Y + y);
                }
            }
        }

        public void Localize(ref int x, ref int y)
        {
            x = x % size;
            y = y % size;
        }

        public string GetMeta(int x, int y)
        {
            this.Localize(ref x, ref y);
            return this.meta.Get(x, y);
        }

        public void CalcPathing()
        {

        }

        public int GetDistance(int x1, int y1, int x2, int y2)
        {
            this.Localize(ref x1, ref y1);
            this.Localize(ref x2, ref y2);
            return this.pathing[this.ToIndex(x1, x2), this.ToIndex(x2, y2)];
        }

        public int ToIndex(int x, int y)
        {
            return y * size + x;
        }
    }
}
