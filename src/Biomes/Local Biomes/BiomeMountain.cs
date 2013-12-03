using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    public class BiomeMountain : Biome
    {
        public BiomeMountain(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {

        }


        public override int Generate(float localNoise)
        {
            if (localNoise > 0.3) { return 6; }
            if (localNoise > 0.2) { return 3; }
            return 4;
        }
    }
}
