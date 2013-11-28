using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class Human : Unit
    {
        private static new int image;

        public Human(int x, int y) : base(x, y, 20)
        {

        }

        public static void Init(int image)
        {
            Human.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Human.image);
        }
    }
}
