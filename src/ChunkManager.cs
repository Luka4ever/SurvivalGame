using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    class ChunkManager
    {
        private string saveFilePath;
        private List<Chunk> chunks;
        private List<Chunk> active;
        private Thread thread;
        private Queue<WorkTask> tasks;
        private float unload;

        public ChunkManager(string saveFilePath)
        {
            this.unload = 0;
            this.saveFilePath = saveFilePath;
            this.chunks = new List<Chunk>();
            this.active = new List<Chunk>();
            this.tasks = new Queue<WorkTask>();
            this.thread = new Thread(this.Manager);
            this.thread.Start();
        }

        public void Tick(World world, float delta)
        {
            if ((this.unload += delta) > 1)
            {
                this.unload = 0;
                foreach (Chunk chunk in this.chunks)
                {
                    if (this.GetChunk(chunk.X - 1, chunk.Y - 1) != null &&
                        this.GetChunk(chunk.X, chunk.Y - 1) != null &&
                        this.GetChunk(chunk.X + 1, chunk.Y - 1) != null &&
                        this.GetChunk(chunk.X - 1, chunk.Y) != null &&
                        this.GetChunk(chunk.X + 1, chunk.Y) != null &&
                        this.GetChunk(chunk.X - 1, chunk.Y + 1) != null &&
                        this.GetChunk(chunk.X, chunk.Y + 1) != null &&
                        this.GetChunk(chunk.X + 1, chunk.Y + 1) != null)
                    {
                        if (!this.active.Contains(chunk))
                        {
                            this.active.Add(chunk);
                        }
                    }
                    else if (this.active.Contains(chunk))
                    {
                        this.active.Remove(chunk);
                    }
                }
            }
            foreach (Chunk chunk in this.active)
            {
                chunk.Tick(world, delta);
            }
            foreach (Chunk chunk in this.active)
            {
                chunk.CheckEntityPosition(world);
            }
        }

        private async void Manager()
        {
            //DO NOT SET DEBUG POINTS IN THIS METHOD
            //it will crash
            FileStream saveFile = null;
            byte[] buffer;
            if (!File.Exists(this.saveFilePath))
            {
                saveFile = File.Create(this.saveFilePath, 1024 * 64);
                buffer = new byte[1 + 256 * 16 + 4];
                await saveFile.WriteAsync(buffer, 0, buffer.Length);
            }
            else
            {
                bool loaded = false;
                while (!loaded)
                {
                    try
                    {
                        saveFile = File.Open(this.saveFilePath, FileMode.Open);
                        loaded = true;
                    }
                    catch
                    {

                    }
                }
            }
            while (tasks.Count > 0)
            {
                WorkTask task = tasks.Dequeue();
                if (task.load)
                {
                    Console.WriteLine("Loading chunk [x: {0}, y: {1}]", task.x, task.y);
                    await this.LoadChunk(saveFile, task.x, task.y, task.seed);
                }
                else
                {
                    await this.UnloadChunk(saveFile, this.GetChunk(task.x, task.y));
                }
            }
            saveFile.Close();
        }

        public void Load(int x, int y, int seed)
        {
            foreach (WorkTask task in this.tasks)
            {
                if (task.x == x && task.y == y)
                {
                    if (!task.load)
                    {
                        Queue<WorkTask> queue = new Queue<WorkTask>(this.tasks.Count);
                        if (!task.Equals(this.tasks.Peek())) queue.Enqueue(this.tasks.Dequeue());
                        this.tasks = queue;
                    }
                    return;
                }
            }
            this.tasks.Enqueue(new WorkTask(x, y, true, seed));
            if (!this.thread.IsAlive)
            {
                this.thread = new Thread(this.Manager);
                this.thread.Start();
            }
        }

        public void Unload(int x, int y)
        {
            foreach (WorkTask task in this.tasks)
            {
                if (task.x == x && task.y == y)
                {
                    if (task.load)
                    {
                        Queue<WorkTask> queue = new Queue<WorkTask>(this.tasks.Count);
                        if (!task.Equals(this.tasks.Peek())) queue.Enqueue(this.tasks.Dequeue());
                        this.tasks = queue;
                    }
                    return;
                }
            }
            this.tasks.Enqueue(new WorkTask(x, y));
            if (!this.thread.IsAlive)
            {
                this.thread = new Thread(this.Manager);
                this.thread.Start();
            }
        }

        private async Task UnloadChunk(FileStream saveFile, Chunk chunk)
        {
            byte[] buffer = new byte[Chunk.size * Chunk.size * 4];
            byte[] row = await this.ReferenceTableLookup(saveFile, 0, chunk.X, chunk.Y);
            for (int j = 0; j < Chunk.size; j++)
            {
                for (int k = 0; k < Chunk.size; k++)
                {
                    BitConverter.GetBytes(chunk.GetTile(k, j)).CopyTo(buffer, (j * Chunk.size + k) * 4);
                }
            }
            
            if (row == null)
            {
                byte[] temp = new byte[4];
                int offset = 0;
                do
                {
                    await saveFile.ReadAsync(temp, offset, 1);
                    if (temp[0] == 255)
                    {
                        await saveFile.ReadAsync(temp, offset + 1 + 256 * 16, 4);
                        offset = BitConverter.ToInt32(temp, 0);
                    }
                }
                while (temp[0] == 255);
                byte[] refRow = new byte[16];
                BitConverter.GetBytes(chunk.X).CopyTo(refRow, 0);
                BitConverter.GetBytes(chunk.Y).CopyTo(refRow, 4);
                BitConverter.GetBytes(saveFile.Length).CopyTo(refRow, 8);
                await saveFile.WriteAsync(buffer, (int)saveFile.Length, buffer.Length);
                //TODO: Fix reference point
                BitConverter.GetBytes(0).CopyTo(refRow, 12);
                await saveFile.WriteAsync(refRow, offset + temp[0] * 16, 16);
            }
            else
            {
                await saveFile.WriteAsync(buffer, BitConverter.ToInt32(row, 0), buffer.Length);
            }
            saveFile.Close();
        }

        private async Task LoadChunk(FileStream saveFile, int x, int y, int seed)
        {
            byte[] row = await this.ReferenceTableLookup(saveFile, 0, x, y);
            if (row == null)
            {
                this.chunks.Add(new Chunk(x, y).Generate(seed));
            }
            else
            {
                Chunk chunk = new Chunk(x, y);
                byte[] buffer = new byte[4 * Chunk.size * Chunk.size];
                await saveFile.ReadAsync(buffer, BitConverter.ToInt32(row, 0), buffer.Length);
                for (int j = 0; j < Chunk.size; j++)
                {
                    for (int k = 0; k < Chunk.size; k++)
                    {
                        chunk.SetTile(k, j, BitConverter.ToInt32(buffer, (j * Chunk.size + k) * 4));
                    }
                }
                this.chunks.Add(chunk);
            }
        }

        /// <summary>
        /// Goes through the savefile reference table to find the location of the tile data for a specific chunk
        /// and the entity table for the chunk, if it doesn't exist in the save file it will return null.
        /// </summary>
        /// <param name="saveFile">FileStream for the save file</param>
        /// <param name="offset">The offset for the reference table (this should be 0 for all non internal calls)</param>
        /// <param name="x">The x coordinate of the chunk</param>
        /// <param name="y">The y coordinate of the chunk</param>
        /// <returns>Byte[] or null</returns>
        private async Task<byte[]> ReferenceTableLookup(FileStream saveFile, int offset, int x, int y)
        {
            byte[] buffer = new byte[1 + 256 * 16 + 4];
            await saveFile.ReadAsync(buffer, offset, buffer.Length);
            for (int i = 0, l = buffer[0]; i < l; i++)
            {
                int row = 1 + i * 16;
                if (BitConverter.ToInt32(buffer, row) == x && BitConverter.ToInt32(buffer, row + 4) == y)
                {
                    return buffer.Skip(row + 8).Take(8).ToArray();
                }
            }
            int next = BitConverter.ToInt32(buffer, 1 + 256 * 16);
            if (next != 0) return await this.ReferenceTableLookup(saveFile, next, x, y);
            return null;
        }

        public int GetTile(int x, int y)
        {
            Chunk chunk = this.GetChunk((int)Math.Floor((double)x / (double)Chunk.size), (int)Math.Floor((double)y / (double)Chunk.size));
            if (chunk == null)
            {
                return 0;
            }
            return chunk.GetTile((x + Chunk.size) % Chunk.size, (y + Chunk.size) % Chunk.size);
        }

        public Chunk GetChunk(int x, int y)
        {
            for (int i = 0; i < this.chunks.Count; i++)
            {
                if (this.chunks[i].X == x && this.chunks[i].Y == y)
                {
                    return this.chunks[i];
                }
            }
            return null;
        }

        public struct WorkTask
        {
            public int x;
            public int y;
            public bool load;
            public int seed;

            public WorkTask(int x, int y, bool load, int seed)
            {
                this.x = x;
                this.y = y;
                this.load = load;
                this.seed = seed;
            }

            public WorkTask(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.load = false;
                this.seed = 0;
            }
        }

        public Entity GetEntityAt(int x, int y)
        {
            Chunk chunk = this.GetChunk((int)Math.Floor((double)x / (double)Chunk.size), (int)Math.Floor((double)y / (double)Chunk.size));
            if (chunk == null)
            {
                return null;
            }
            return chunk.GetEntityAt((x + Chunk.size) % Chunk.size, (y + Chunk.size) % Chunk.size);
        }

        public List<Entity> GetEntitiesAt(int x, int y)
        {
            Chunk chunk = this.GetChunk((int)Math.Floor((double)x / (double)Chunk.size), (int)Math.Floor((double)y / (double)Chunk.size));
            if (chunk == null)
            {
                return new List<Entity>();
            }
            return chunk.GetEntitiesAt(x, y);
        }
    }
}
