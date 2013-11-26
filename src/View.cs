using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurvivalGame.src
{
    class View
    {
        private Rectangle view;
        private Entity target;
        private Strategy strategy;
        public enum Strategy { Fixed, Smooth, Snap }

        public View(Form window)
        {
            this.view = window.ClientRectangle;
        }

        public void Tick(float delta)
        {
            //test code to move the view around
            /*
            double dir = new Random().NextDouble();
            double force = new Random().NextDouble() * 1;
            this.view.X += (int)Math.Round(Math.Cos(dir) * force * delta);
            this.view.Y += (int)Math.Round(Math.Sin(dir) * force * delta);
            */
            switch (this.strategy)
            {
                case Strategy.Fixed:
                    break;
                case Strategy.Smooth:
                    break;
                case Strategy.Snap:
                    break;
            }
        }

        public Strategy GetStrategy()
        {
            return this.strategy;
        }

        public void SetStrategy(Strategy value)
        {
            this.strategy = value;
        }

        public int GetX()
        {
            return this.view.X;
        }

        public int GetY()
        {
            return this.view.Y;
        }
    }
}
