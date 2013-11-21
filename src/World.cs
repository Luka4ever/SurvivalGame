using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SurvivalGame.src
{
    class World
    {
        private int seed;
        private List<Chunk> chunks;

        public World(int seed)
        {
            this.seed = seed;
            this.chunks = new List<Chunk>();
            this.chunks.Add(new Chunk(0, 0));
        }

        public int Seed
        {
            get {return this.seed; }
        }

        public void Draw(Graphics g, View view)
        {
            for (int i = 0, l = chunks.Count; i < l; i++)
            {
                this.chunks[i].Draw(g, view, this);
            }
        }

        public int GetTile(int x, int y)
        {
            int X = (int)Math.Floor((decimal)x / (decimal)Chunk.size);
            int Y = (int)Math.Floor((decimal)y / (decimal)Chunk.size);
            for (int i = 0, l = this.chunks.Count; i < l; i++)
            {
                Chunk chunk = this.chunks[i];
                if (chunk.X == X && chunk.Y == Y)
                {
                    return chunk.GetTile(x, y);
                }
            }
            return 0;
        }

        public bool SetTile(int x, int y, int type)
        {
            int X = (int)Math.Floor((decimal)x / (decimal)Chunk.size);
            int Y = (int)Math.Floor((decimal)y / (decimal)Chunk.size);
            for (int i = 0, l = this.chunks.Count; i < l; i++)
            {
                Chunk chunk = this.chunks[i];
                if (chunk.X == X && chunk.Y == Y)
                {
                    chunk.SetTile(x, y, type);
                    return true;
                }
            }
            return false;
        }
    }
}
