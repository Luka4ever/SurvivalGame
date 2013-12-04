using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class GrassTuft : Entity
    {
        private static new int image;


        public GrassTuft(int x, int y)
            : base(x, y)
        {

        }

        public static void Init(int image)
        {
            GrassTuft.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, GrassTuft.image, 8 ,8);
        }
    }
}
