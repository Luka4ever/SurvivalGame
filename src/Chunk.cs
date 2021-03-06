﻿using SurvivalGame.src.Biomes;
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
            for (int i = 0; i < this.entities.Count; i++)
            {
                this.entities[i].Tick(world, delta);
            }
        }

        public void Draw(Graphics g, View view, World world, float delta)
        {
            Chunk cUp = world.GetChunk(this.x, this.y - 1);
            Chunk cDown = world.GetChunk(this.x, this.y + 1);
            Chunk cLeft = world.GetChunk(this.x - 1, this.y);
            Chunk cRight = world.GetChunk(this.x + 1, this.y);
            int X = this.x * Chunk.size;
            int Y = this.y * Chunk.size;
            for (int y = 0; y < Chunk.size; y++)
            {
                for (int x = 0; x < Chunk.size; x++)
                {
                    int up = (y > 0 ? this.tiles[this.ToIndex(x, y - 1)] : (cUp == null ? 0 : cUp.GetTile(x, y - 1)));
                    int down = (y < Chunk.size - 1 ? this.tiles[this.ToIndex(x, y + 1)] : (cDown == null ? 0 : cDown.GetTile(x, y + 1)));
                    int left = (x > 0 ? this.tiles[this.ToIndex(x - 1, y)] : (cLeft == null ? 0 : cLeft.GetTile(x - 1, y)));
                    int right = (x < Chunk.size - 1 ? this.tiles[this.ToIndex(x + 1, y)] : (cRight == null ? 0 : cRight.GetTile(x + 1, y)));
                    Tile.GetTile(this.tiles[this.ToIndex(x, y)]).Draw(g, view, world, X + x, Y + y, up, down, left, right);
                }
            }
        }

        public void DrawEntities(Graphics g, View view, World world, float delta)
        {
            List<Entity>[] entities = new List<Entity>[size];
            for (int y = 0; y < size; y++)
            {
                entities[y] = new List<Entity>(5);
            }
            foreach (Entity entity in this.entities)
            {
                entities[(entity.Y % Chunk.size + Chunk.size) % Chunk.size].Add(entity);
            }
            for (int y = 0; y < size; y++)
            {
                foreach (Entity entity in entities[y])
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
            return (y + Chunk.size) % Chunk.size * size + (x + Chunk.size) % Chunk.size;
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

        public static float GetContinentSeed(double seed)
        {
            return (float)Math.Floor((double)(GetLocalSeed(seed) * (long)73 / (long)15 % (long)Int32.MaxValue));
        }

        public static float GetContinentNoise(int x, int y, float continentSeed)
        {
            return Noise.GetNoise((double)x / 256, (double)y / 256, continentSeed);
        }

        public static float GetBiomeSeed(double seed)
        {
            return (float)Math.Floor((double)(GetLocalSeed(seed) * (long)13 / (long)6 % (long)Int32.MaxValue));
        }

        public static float GetBiomeNoise(int x, int y, float biomeSeed)
        {
            return Noise.GetNoise((double)x / 128, (double)y / 128, biomeSeed);
        }

        public static float GetLocalSeed(double seed)
        {
            return (float)(seed / 1000d);
        }

        public static float GetLocalNoise(int x, int y, float localSeed)
        {
            return Noise.GetNoise((double)x / 32, (double)y / 32, localSeed);
        }

        public Chunk Generate(double seed)
        {
            float localSeed = GetLocalSeed(seed);
            float biomeSeed = GetBiomeSeed(seed);
            float continentSeed = GetContinentSeed(seed);

            for (int y = 0; y < Chunk.size; y++)
            {
                for (int x = 0; x < Chunk.size; x++)
                {
                    float localNoise = GetLocalNoise(x + this.x * Chunk.size, y + this.y * Chunk.size, localSeed);
                    float biomeNoise = GetBiomeNoise(x + this.x * Chunk.size, y + this.y * Chunk.size, biomeSeed);
                    float worldNoise = GetContinentNoise(x + this.x * Chunk.size, y + this.y * Chunk.size, continentSeed);
                    this.SetTile(x, y, Biome.GetContinentFromNoise(worldNoise).GetBiomeFromNoise(biomeNoise).Generate(localNoise));

                    Entity tempEntity = Biome.GetContinentFromNoise(worldNoise).GetBiomeFromNoise(biomeNoise).GenerateEntity(localNoise, biomeNoise, worldNoise, x + this.x * Chunk.size, y + this.y * Chunk.size, seed);
                    if(tempEntity != null)
                    {
                        this.AddEntity(tempEntity);
                    }
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

        public Entity GetEntityAt(int x, int y)
        {
            foreach (Entity entity in this.entities)
            {
                if (entity.X == x && entity.Y == y)
                {
                    return entity;
                }
            }
            return null;
        }

        public List<Entity> GetEntitiesAt(int x, int y)
        {
            List<Entity> entities = new List<Entity>(2);
            for (int i = 0; i < this.entities.Count; i++)
            {
                if (this.entities[i].X == x && this.entities[i].Y == y)
                {
                    entities.Add(this.entities[i]);
                }
            }
            return entities;
        }
    }
}
