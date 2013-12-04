using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class Lilypad2 : Entity
    {
        private static new int image;


        public Lilypad2(int x, int y)
            : base(x, y)
        {

        }

        public static void Init(int image)
        {
            Lilypad2.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Lilypad2.image, 16, 16);
        }
    }
}
