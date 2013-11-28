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
            //this.CalcPathing();
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

        public void Tick(World world, float delta)
        {
            foreach (Entity entity in this.entities)
            {
                entity.Tick(world, delta);
            }
        }

        public void Draw(Graphics g, View view, World world, float delta)
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
            List<Entity>[] stuff = new List<Entity>[size];
            for (int y = 0; y < size; y++)
            {
                stuff[y] = new List<Entity>(5);
            }
            foreach (Entity entity in this.entities)
            {
                stuff[(entity.Y + Chunk.size) % Chunk.size].Add(entity);
            }
            for (int y = 0; y < size; y++)
            {
                foreach (Entity entity in stuff[y])
                {
                    entity.Draw(g, view, world, delta);
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

        public void CalcPathing()
        {
            int l = size * size;
            this.pathing = new int[l, l];
            for (int k = 0; k < l; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    this.pathing[k, i] = int.MaxValue;
                }
                this.pathing[k, k] = 0;
            }
            //add the weight of the edges
            for (int v = 0, y = 0, u, x; y < size; y++)
            {
                for (x = 0; x < size; x++, v++)
                {
                    if (y > 0)
                    {
                        if (x > 0)
                        {
                            u = v - size - 1;
                            this.pathing[v, u] = this.pathing[u, v];
                        }
                        u = v - size;
                        this.pathing[v, u] = this.pathing[u, v];
                        if (x < size - 1)
                        {
                            u = v - size + 1;
                            this.pathing[v, u] = this.pathing[u, v];
                        }
                    }
                    if (x > 0)
                    {
                        u = v - 1;
                        this.pathing[v, u] = this.pathing[u, v];
                    }
                    if (x < size - 1)
                    {
                        u = v + 1;
                        this.pathing[v, u] = this.WeightEdgeLocal(x, y, x + 1, y);
                    }
                    if (y < size - 1)
                    {
                        if (x > 0)
                        {
                            u = v + size - 1;
                            this.pathing[v, u] = this.WeightEdgeLocal(x, y, x - 1, y + 1);
                        }
                        u = v + size;
                        this.pathing[v, u] = this.WeightEdgeLocal(x, y, x, y + 1);
                        if (x < size - 1)
                        {
                            u = v + size + 1;
                            this.pathing[v, u] = this.WeightEdgeLocal(x, y, x + 1, y + 1);
                        }
                    }
                }
            }
            for (int k = 0, i, j, m; k < l; k++)
            {
                for (i = 0; i < l; i++)
                {
                    for (j = 0; j < l; j++)
                    {
                        if ((m = this.pathing[i, k] + this.pathing[k, j]) < this.pathing[i, j])
                        {
                            this.pathing[i, j] = m;
                        }
                    }
                }
            }
            Console.WriteLine("Pathing done");
        }

        private int WeightEdgeLocal(int x1, int y1, int x2, int y2)
        {
            float friction1 = Tile.GetTile(this.tiles[this.ToIndex(x1, y1)]).GetFriction();
            float friction2 = Tile.GetTile(this.tiles[this.ToIndex(x2, y2)]).GetFriction();
            float dist = (float) (Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / 2d);
            float weight = (float) ((Math.Pow(Math.Sin(friction1 / 2d * Math.PI), 2) * dist + Math.Pow(Math.Sin(friction2 / 2d * Math.PI), 2) * dist) * 10d);
            return (int) Math.Min(Math.Max(Math.Round(weight), 0), 10);
        }

        public Chunk Generate(int seed)
        {
            for (int y = 0; y < Chunk.size; y++)
            {
                for (int x = 0; x < Chunk.size; x++)
                {
                    float biomeSeed = (float)Math.Floor((double)((long)seed * (long)13 / (long)6 % (long)Int32.MaxValue));
                    float biomeNoise = Noise.GetNoise((double)x / 32, (double)y / 32, biomeSeed);
                    float localNoise = Noise.GetNoise((double)x/ 32, (double)y / 32, seed);

                    this.tiles[this.ToIndex(x, y)] = 1; 
                    if (localNoise > 0.3) { this.tiles[this.ToIndex(x, y)] = 2; }
                    if (localNoise > 0.5) { this.tiles[this.ToIndex(x, y)] = 3; }
                    if (localNoise > 0.7) { this.tiles[this.ToIndex(x, y)] = 4; }
                }
            }
            return this;
        }

        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
        }

        public bool RemoveEntity(Entity entity)
        {
            return this.entities.Remove(entity);
        }

        public void CheckEntityPosition(World world)
        {
            List<Entity> remove = new List<Entity>();
            foreach (Entity entity in this.entities)
            {
                int x = (int)Math.Floor((double)entity.X / (double)Chunk.size);
                int y = (int)Math.Floor((double)entity.Y / (double)Chunk.size);
                if (this.x != x || this.y != y)
                {
                    Chunk chunk = world.GetChunk(x, y);
                    if (chunk != null)
                    {
                        remove.Add(entity);
                        chunk.AddEntity(entity);
                    }
                }
            }
            foreach (Entity entity in remove)
            {
                this.entities.Remove(entity);
            }
        }
    }
}
