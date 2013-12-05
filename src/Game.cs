using SurvivalGame.src.Interface;
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
        private UI ui;

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
            this.world = new World(seed, @"saves/save.csf");
            this.view = new View(window, this.world.GetPlayer());
            this.ui = new UI(this.world.GetPlayer());
            this.ui.Reflow(this.view);
            this.running = false;
            this.options = options;
        }

        public void Run(BufferedGraphics buffer, Queue<KeyEvent> inputQueue, Form window)
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
                Input(inputQueue, window);
                //Tick
                if (timeTick > minimumTick)
                {
                    if (timeTick > this.lastGameTick + minimumTick * 2)
                    {
                        Console.WriteLine("Skipping Ticks!");
                    }
                    Tick((float) timeTick / 1000f);
                }
                //Draw
                if (this.options.LimitFPS)
                {
                    minimumDraw = (int) Math.Floor(1000f / (float) this.options.MaxFPS);
                }
                if (timeDraw > this.lastFrameTime + minimumDraw)
                {
                    Draw(buffer, (float) timeTick / 1000f);
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        private void Draw(BufferedGraphics buffer, float delta)
        {
            UpdateFPS();
            this.ui.Reflow(this.view);
            this.world.Draw(buffer.Graphics, this.view, delta);
            this.ui.Draw(buffer.Graphics, this.world, this);
            buffer.Render();
        }

        private void Tick(float delta)
        {
            TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1);
            this.lastGameTick = span.TotalMilliseconds;
            this.world.Tick(delta);
            this.view.Tick(delta);
        }

        private void Input(Queue<KeyEvent> inputQueue, Form window)
        {
            this.ui.Input(window);
            while (inputQueue.Count > 0)
            {
                KeyEvent key = inputQueue.Dequeue();
                if (key.KeyCode == Keys.Escape)
                {
                    this.running = false;
                }
                else if (key.KeyCode == Keys.W && key.KeyDown)
                {
                    key.Handled = true;
                    Unit player = (Unit) this.world.GetPlayer();
                    if (player.GetProgress() == 0)
                    {
                        player.MoveUp();
                        player.SetAction(Unit.Actions.Move);
                    }
                }
                else if (key.KeyCode == Keys.S && key.KeyDown)
                {
                    key.Handled = true;
                    Unit player = (Unit)this.world.GetPlayer();
                    if (player.GetProgress() == 0)
                    {
                        player.MoveDown();
                        player.SetAction(Unit.Actions.Move);
                    }
                }
                else if (key.KeyCode == Keys.A && key.KeyDown)
                {
                    key.Handled = true;
                    Unit player = (Unit)this.world.GetPlayer();
                    if (player.GetProgress() == 0)
                    {
                        player.MoveLeft();
                        player.SetAction(Unit.Actions.Move);
                    }
                }
                else if (key.KeyCode == Keys.D && key.KeyDown)
                {
                    key.Handled = true;
                    Unit player = (Unit)this.world.GetPlayer();
                    if (player.GetProgress() == 0)
                    {
                        player.MoveRight();
                        player.SetAction(Unit.Actions.Move);
                    }
                }
                else if (key.KeyCode == Keys.F3 && key.KeyDown)
                {
                    key.Handled = true;
                    this.ui.Debug = !this.ui.Debug;
                }
                else if (key.KeyCode == Keys.B && key.KeyDown)
                {
                    key.Handled = true;
                    Node inventory = this.ui.GetNodeByID("inventory");
                    inventory.Visible = !inventory.Visible;
                }
            }
        }

        public void Exit()
        {
            this.running = false;
        }

        public float FPS
        {
            get { return this.fps; }
        }

        public UI UI
        {
            get { return this.ui; }
        }
    }
}
