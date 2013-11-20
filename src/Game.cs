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
        private float fpsSmoothing = 1f;
        private int frameCountSkipped;
        private void UpdateFPS()
        {
            int time = DateTime.Now.Millisecond;
            int difference = this.lastFrameTime - time;
            if (difference > 0)
            {
                this.fps = (this.fpsSmoothing < 1 ? this.fps / (1f - this.fpsSmoothing) : 0) + 1000 / ((float)difference / (float) (this.frameCountSkipped + 1) * this.fpsSmoothing);
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

        public void Run(BufferedGraphics buffer, Queue<KeyEventArgs> inputQueue)
        {
            this.running = true;
            this.lastGameTick = DateTime.Now.Millisecond;
            this.lastFrameTime = DateTime.Now.Millisecond;
            this.fps = 0;
            this.frameCountSkipped = 0;
            this.targetTickRate = 20;

            while (this.running)
            {
                int timeDraw = this.lastFrameTime - DateTime.Now.Millisecond;
                int minimumDraw = 0;
                int timeTick = this.lastGameTick - DateTime.Now.Millisecond;
                int minimumTick = (int)Math.Floor(1000f / (float)targetTickRate);
                System.Windows.Forms.Application.DoEvents();
                //Input
                Input(inputQueue);
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
                System.Threading.Thread.Sleep(1);
            }
            Application.Exit();
        }

        private void Draw(BufferedGraphics buffer, float delta)
        {
            UpdateFPS();
            Console.WriteLine(fps);
            this.window.Refresh();
            this.world.Draw(buffer.Graphics, this.view);
            buffer.Render();
        }

        private void Tick(float delta)
        {
            this.lastGameTick = DateTime.Now.Millisecond;
        }

        private void Input(Queue<KeyEventArgs> inputQueue)
        {
            if (inputQueue.Count > 0) this.running = false;
            while (inputQueue.Count > 0)
            {
                KeyEventArgs key = inputQueue.Dequeue();
                if (key.KeyCode == Keys.Escape)
                {
                    this.running = false;
                    Application.Exit();
                }
            }
        }
    }
}
