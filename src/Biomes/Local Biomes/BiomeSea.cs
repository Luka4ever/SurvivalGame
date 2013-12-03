using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    public class BiomeSea : Biome
    {
        public BiomeSea(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {

        }

        public override int Generate(float localNoise)
        {
            if (localNoise > 0.8) { return 2; }
            if (localNoise > 0.75) { return 3; }
            if (localNoise > 0.65) { return 4; }
            return 1;
        }
    }
}
