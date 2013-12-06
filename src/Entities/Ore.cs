using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class Ore : Entity
    {
        private static new int image;


        public Ore(int x, int y)
            : base(x, y)
        {

        }

        public static void Init(int image)
        {
            Ore.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Ore.image);
        }
    }
}
