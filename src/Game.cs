using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurvivalGame.src
{
    class Game
    {
        private World world;
        private Form window;
        private View view;
        private bool running;
        private Options options;

        private float fps;
        private double lastFrameTime;
        private float fpsSmoothing = 0.05f;
        private int frameCountSkipped;
        private void UpdateFPS()
        {
            TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1);
            double time = span.TotalMilliseconds;
            double difference = time - this.lastFrameTime;
            if (difference > 0)
            {
                this.fps = this.fps * (1f - this.fpsSmoothing) + 1000 / ((float)difference / (float)(this.frameCountSkipped + 1)) * this.fpsSmoothing;
                this.lastFrameTime = time;
                this.frameCountSkipped = 0;
            }
            else
            {
                frameCountSkipped++;
            }
        }

        private double lastGameTick;
        private int targetTickRate;

        public Game(int seed, Form window, Options options)
        {
            this.window = window;
            this.world = new World(seed);
            this.view = new View(window);
            this.running = false;
            this.options = options;
        }

        public void Run(BufferedGraphics buffer, Queue<KeyEventArgs> inputQueue)
        {
            TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1);
            this.running = true;
            this.lastGameTick = span.TotalMilliseconds;
            this.lastFrameTime = span.TotalMilliseconds;
            this.fps = 0;
            this.frameCountSkipped = 0;
            this.targetTickRate = 20;

            while (this.running)
            {
                span = DateTime.UtcNow - new DateTime(1970, 1, 1);
                double timeDraw = span.TotalMilliseconds;
                int minimumDraw = 0;
                double timeTick = span.TotalMilliseconds - this.lastGameTick;
                int minimumTick = (int)Math.Floor(1000f / (float)targetTickRate);
                Application.DoEvents();
                //Input
                Input(inputQueue);
                //Tick
                if (timeTick > minimumTick)
                {
                    if (timeTick > this.lastGameTick + minimumTick * 2)
                    {
                        Console.WriteLine("Skipping Ticks!");
                    }
                    Tick(1000f / (float) timeTick);
                }
                //Draw
                if (this.options.LimitFPS)
                {
                    minimumDraw = (int) Math.Floor(1000f / (float) this.options.MaxFPS);
                }
                if (timeDraw > this.lastFrameTime + minimumDraw)
                {
                    Draw(buffer, 1000f / (float) timeDraw);
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        private void Draw(BufferedGraphics buffer, float delta)
        {
            for (int y = 0; y < Chunk.size; y++)
            {
                for (int x = 0; x < Chunk.size; x++)
                {
                    this.world.SetTile(x, y, (this.world.GetTile(x, y) + 1) % 4);
                }
            }
            UpdateFPS();
            this.world.Draw(buffer.Graphics, this.view);
            buffer.Render();
        }

        private void Tick(float delta)
        {
            TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1);
            this.lastGameTick = span.TotalMilliseconds;
        }

        private void Input(Queue<KeyEventArgs> inputQueue)
        {
            while (inputQueue.Count > 0)
            {
                Console.WriteLine("test");
                KeyEventArgs key = inputQueue.Dequeue();
                if (key.KeyCode == Keys.Escape)
                {
                    this.running = false;
                }
                else if (key.KeyCode == Keys.A)
                {
                    for (int y = 0; y < Chunk.size; y++)
                    {
                        for (int x = 0; x < Chunk.size; x++)
                        {
                            this.world.SetTile(x, y, (this.world.GetTile(x, y) + 1) % 4);
                        }
                    }
                }
            }
        }

        public void Exit()
        {
            this.running = false;
        }
    }
}
