using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurvivalGame.src
{
    class Entity
    {
        private int x;
        private int y;
        private float velocity;
        private float acceleration;
        private byte direction;
        private float progress;

        public virtual void Tick(World world)
        {

        }

        public virtual void Draw()
        {

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
    }
}
