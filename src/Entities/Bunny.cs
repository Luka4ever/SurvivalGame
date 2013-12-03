using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class Bunny : Unit
    {
        private static new int image;

        public Bunny(int x, int y) : base(x, y, 5)
        {

        }

        public static void Init(int image)
        {
            Bunny.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Bunny.image);
        }
    }
}
