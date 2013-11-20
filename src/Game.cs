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
        private int lastFrameTime;
        private float fpsSmoothing = 0.1f;
        private int frameCountSkipped;
        private void UpdateFPS()
        {
            int time = DateTime.Now.Millisecond;
            int difference = this.lastFrameTime - time;
            if (difference > 0)
            {
                this.fps = this.fps / (1f - this.fpsSmoothing) + 1000 / ((float)difference / (float) (this.frameCountSkipped + 1) * this.fpsSmoothing);
                this.lastFrameTime = time;
                this.frameCountSkipped = 0;
            }
            else
            {
                frameCountSkipped++;
            }
        }

        private int lastGameTick;
        private int targetTickRate;

        public Game(int seed, Form window, Options options)
        {
            this.window = window;
            this.world = new World(seed);
            this.view = new View(window);
            this.running = false;
            this.options = options;
        }

        public void Run(BufferedGraphics buffer)
        {
            running = true;
            this.lastGameTick = DateTime.Now.Millisecond;
            this.lastFrameTime = DateTime.Now.Millisecond;
            this.fps = 0;
            this.frameCountSkipped = 0;
            this.targetTickRate = 20;

            while (running)
            {
                int timeDraw = this.lastFrameTime - DateTime.Now.Millisecond;
                int minimumDraw = 0;
                int timeTick = this.lastGameTick - DateTime.Now.Millisecond;
                int minimumTick = (int) Math.Floor(1000f / (float) targetTickRate);
                //Input

                //Tick
                if (timeTick > minimumTick)
                {
                    if (timeTick > minimumTick * 2)
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
                if (timeDraw > minimumDraw)
                {
                    Draw(buffer, 1000f / (float) timeDraw);
                }
            }
        }

        private void Draw(BufferedGraphics buffer, float delta)
        {
            UpdateFPS();
            Console.WriteLine(fps);
            this.world.Draw(buffer.Graphics, this.view);
            buffer.Render();
        }

        private void Tick(float delta)
        {
            this.lastGameTick = DateTime.Now.Millisecond;
        }
    }
}
