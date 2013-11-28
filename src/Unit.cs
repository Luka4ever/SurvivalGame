using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SurvivalGame.src
{
    class Unit : Entity
    {
        private int health;
        protected Actions action;
        protected Actions nextAction;
        private Inventory inventory;
        public enum Actions { None = 0, Move = 1 }

        public Unit(int x, int y, int health) : base(x, y)
        {
            this.action = Actions.None;
            this.nextAction = Actions.None;
            this.speed = 3;
        }

        public void SetAction(Actions action)
        {
            this.nextAction = action;
        }
        
        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        public int Damage(int damage)
        {
            return this.health -= damage;
        }

        public override void Tick(World world, float delta)
        {
            if (this.action == Actions.None)
            {
                this.progress = 0;
                if (this.nextAction != Actions.None)
                {
                    this.action = this.nextAction;
                    this.nextAction = Actions.None;
                }
            }
            switch (this.action)
            {
                case (Actions.Move):
                    if (this.progress == 0)
                    {
                        bool upSet = (this.control >> 3 & 1) == 1;
                        bool up = (this.control >> 2 & 1) == 1;
                        bool leftSet = (this.control >> 1 & 1) == 1;
                        bool left = (this.control & 1) == 1;
                        this.control = (byte)(this.control & 0xf0);
                        if (upSet)
                        {
                            if (leftSet)
                            {
                                if (up)
                                {
                                    if (left)
                                    {
                                        this.direction = (byte)Direction.NorthWest;
                                    }
                                    else
                                    {
                                        this.direction = (byte)Direction.NorthEast;
                                    }
                                }
                                else
                                {
                                    if (left)
                                    {
                                        this.direction = (byte)Direction.SouthWest;
                                    }
                                    else
                                    {
                                        this.direction = (byte)Direction.SouthEast;
                                    }
                                }
                            }
                            else
                            {
                                if (up)
                                {
                                    this.direction = (byte)Direction.North;
                                }
                                else
                                {
                                    this.direction = (byte)Direction.South;
                                }
                            }
                        }
                        else if (leftSet)
                        {
                            if (left)
                            {
                                this.direction = (byte)Direction.West;
                            }
                            else
                            {
                                this.direction = (byte)Direction.East;
                            }
                        }
                    }
                    Tile a = Tile.GetTile(world.GetTile(this.x, this.y));
                    int dx = this.x;
                    int dy = this.y;
                    if (this.direction <= 1 || this.direction >= 7)
                    {
                        dy--;
                    }
                    if (this.direction >= 3 && this.direction <= 5)
                    {
                        dy++;
                    }
                    if (this.direction >= 1 && this.direction <= 3)
                    {
                        dx++;
                    }
                    if (this.direction >= 5 && this.direction <= 7)
                    {
                        dx--;
                    }
                    Tile b = Tile.GetTile(world.GetTile(dx, dy));
                    float friction;
                    if (this.progress >= 0.5)
                    {
                        friction = a.GetFriction();
                    }
                    else
                    {
                        friction = b.GetFriction();
                    }
                    double distance = 0.5;
                    if (dx != this.x && dy != this.y)
                    {
                        distance = Math.Sqrt(2) / 2;
                    }
                    this.progress += (float) ((double) this.speed / distance * Math.Sin(friction) * (double) delta);
                    if (this.progress >= 1)
                    {
                        this.progress = 0;
                        this.x = dx;
                        this.y = dy;
                        this.action = Actions.None;
                    }
                    break;
            }
            base.Tick(world, delta);
        }

        protected override void Draw(Graphics g, View view, World world, float delta, int image)
        {
            g.DrawImage(ImageManager.GetImage(image), this.GetVisualX() - view.GetX(), this.GetVisualY() - Tile.size / 4 - view.GetY());
        }

        public override int GetVisualX()
        {
            float x = 0;
            if (this.action == Actions.Move)
            {
                if (this.direction >= 1 && this.direction <= 3)
                {
                    x = this.progress;
                }
                if (this.direction >= 5 && this.direction <= 7)
                {
                    x = this.progress * -1;
                }
            }
            return (int)((this.x + x) * Tile.size);
        }

        public override int GetVisualY()
        {
            float y = 0;
            if (this.action == Actions.Move)
            {
                if (this.direction <= 1 || this.direction >= 7)
                {
                    y = this.progress * -1;
                }
                if (this.direction >= 3 && this.direction <= 5)
                {
                    y = this.progress;
                }
            }
            return (int)((this.y + y) * Tile.size);
        }
    }
}
