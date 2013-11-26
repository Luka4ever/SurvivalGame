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
        private ChunkManager chunkManager;

        public World(int seed, string saveFilePath)
        {
            this.seed = seed;
            this.chunkManager = new ChunkManager(saveFilePath);
        }

        public int Seed
        {
            get {return this.seed; }
        }

        public void Draw(Graphics g, View view, float delta)
        {
            int y = view.GetY() / (Chunk.size * Tile.size) - 1;
            int my = y + (int) Math.Ceiling(g.VisibleClipBounds.Height / (Chunk.size * Tile.size)) + 1;
            for (; y <= my; y++)
            {
                int x = view.GetX() / (Chunk.size * Tile.size) - 1;
                int mx = x + (int)Math.Ceiling(g.VisibleClipBounds.Width / (Chunk.size * Tile.size)) + 1;
                for (; x <= mx; x++)
                {
                    Chunk chunk = this.chunkManager.GetChunk(x, y);
                    if (chunk == null)
                    {
                        this.chunkManager.Load(x, y, this.seed);
                    }
                    else
                    {
                        chunk.Draw(g, view, this, delta);
                    }
                }
            }
        }

        public void Tick(float delta)
        {
            this.chunkManager.Tick(this, delta);
        }

        public int GetTile(int x, int y)
        {
            return this.chunkManager.GetTile(x, y);
        }

        public bool SetTile(int x, int y, int type)
        {
            int X = (int)Math.Floor((decimal)x / (decimal)Chunk.size);
            int Y = (int)Math.Floor((decimal)y / (decimal)Chunk.size);
            Chunk chunk = this.chunkManager.GetChunk(X, Y);
            if (chunk != null)
            {
                chunk.SetTile(x, y, type);
                return true;
            }
            return false;
        }
    }
}
