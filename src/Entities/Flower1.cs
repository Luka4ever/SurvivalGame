using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class Flower1 : Entity
    {
        private static new int image;


        public Flower1(int x, int y)
            : base(x, y)
        {

        }

        public static void Init(int image)
        {
            Flower1.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Flower1.image, 16, 16);
        }
    }
}
