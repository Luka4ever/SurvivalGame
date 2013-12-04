using SurvivalGame.src.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SurvivalGame.src
{
    class Entity
    {
        protected int x;
        protected int y;
        protected int image;
        protected float speed;
        protected byte direction;
        protected float progress;
        public enum Direction { North = 0, NorthEast = 1, East = 2, SouthEast = 3, South = 4, SouthWest = 5, West = 6, NorthWest = 7 }
        protected byte control;

        public Entity(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.speed = 0;
            this.direction = 0;
            this.progress = 0;
            this.control = 0;
        }

        public float GetProgress()
        {
            return this.progress;
        }

        public Direction GetDirection()
        {
            return (Direction)this.direction;
        }

        public void MoveUp()
        {
            byte data = (byte)(this.control >> 2 & 0x03);
            bool set = data >> 1 == 1;
            bool up = (data & 0x01) == 1;
            if (!up && set)
            {
                set = false;
            }
            else
            {
                up = true;
                set = true;
            }
            data = 0;
            if (set)
            {
                data += 2;
            }
            if (up)
            {
                data += 1;
            }
            this.control = (byte)(this.control & 0xf3 | (data << 2));
        }

        public void MoveDown()
        {
            byte data = (byte)(this.control >> 2 & 0x03);
            bool set = data >> 1 == 1;
            bool up = (data & 0x01) == 1;
            if (up && set)
            {
                set = false;
            }
            else
            {
                up = false;
                set = true;
            }
            data = 0;
            if (set)
            {
                data += 2;
            }
            if (up)
            {
                data += 1;
            }
            this.control = (byte)(this.control & 0xf3 | (data << 2));
        }

        public void MoveLeft()
        {
            byte data = (byte)(this.control & 0x03);
            bool set = data >> 1 == 1;
            bool left = (data & 0x01) == 1;
            if (!left && set)
            {
                set = false;
            }
            else
            {
                left = true;
                set = true;
            }
            data = 0;
            if (set)
            {
                data += 2;
            }
            if (left)
            {
                data += 1;
            }
            this.control = (byte)(this.control & 0xfc | data);
        }

        public void MoveRight()
        {
            byte data = (byte)(this.control & 0x03);
            bool set = data >> 1 == 1;
            bool left = (data & 0x01) == 1;
            if (left && set)
            {
                set = false;
            }
            else
            {
                left = false;
                set = true;
            }
            data = 0;
            if (set)
            {
                data += 2;
            }
            if (left)
            {
                data += 1;
            }
            this.control = (byte)(this.control & 0xfc | data);
        }

        public virtual void Tick(World world, float delta)
        {

        }

        public virtual void Draw(Graphics g, View view, World world, float delta)
        {
            this.Draw(g, view, world, delta, image);
        }

        public virtual void Draw(Graphics g, View view, World world, float delta, int offX, int offY)
        {
            this.Draw(g, view, world, delta, image, offX, offY);
        }

        protected virtual void Draw(Graphics g, View view, World world, float delta, int image)
        {
            g.DrawImage(ImageManager.GetImage(image), this.x * Tile.size - view.GetX(), this.y * Tile.size - Tile.size / 4 - view.GetY());
        }

        protected virtual void Draw(Graphics g, View view, World world, float delta, int image, int offsetX, int offsetY)
        {
            g.DrawImage(ImageManager.GetImage(image), this.x * Tile.size - view.GetX() + offsetX, this.y * Tile.size - Tile.size / 4 - view.GetY() + offsetY);
        }

        public static void Init()
        {
            Human.Init(ImageManager.RegisterImage(@"res/Entities/Human.png"));
            Bunny.Init(ImageManager.RegisterImage(@"res/Entities/Bunny.png"));
            OakTree.Init(ImageManager.RegisterImage(@"res/Entities/Tree2.png"));
            PineTree.Init(ImageManager.RegisterImage(@"res/Entities/Tree1.png"));
            Flower1.Init(ImageManager.RegisterImage(@"res/Entities/Flower1.png"));
            Flower2.Init(ImageManager.RegisterImage(@"res/Entities/Flower2.png"));
            GrassTuft.Init(ImageManager.RegisterImage(@"res/Entities/GrassTuft.png"));
            Pebble1.Init(ImageManager.RegisterImage(@"res/Entities/pebble1.png"));
            Pebble2.Init(ImageManager.RegisterImage(@"res/Entities/pebble2.png"));
            Lilypad1.Init(ImageManager.RegisterImage(@"res/Entities/lilypad1.png"));
            Lilypad2.Init(ImageManager.RegisterImage(@"res/Entities/lilypad2.png"));
            Cactus.Init(ImageManager.RegisterImage(@"res/Entities/cactus.png"));
        }

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int AddX(int x)
        {
            return this.x += x;
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public int AddY(int y)
        {
            return this.y += y;
        }

        public virtual int GetVisualX()
        {
            return this.x * Tile.size;
        }

        public virtual int GetVisualY()
        {
            return this.y * Tile.size;
        }
    }
}
