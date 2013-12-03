using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    public class BiomeField : Biome
    {
        public BiomeField(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {

        }

        public override int Generate(float localNoise)
        {
            if (localNoise > 0.8) { return 1; }
            if (localNoise > 0.25) { return 2; }
            return 3;
        }
    }
}
