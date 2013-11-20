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
    }
}
