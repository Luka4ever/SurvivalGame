using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class PineTree : Entity
    {
        private static new int image;


        public PineTree(int x, int y)
            : base(x, y)
        {

        }

        public static void Init(int image)
        {
            PineTree.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, PineTree.image, 0, -32);
        }
    }
}
