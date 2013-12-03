using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    public class BiomeForest : Biome
    {
        public BiomeForest(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {

        }


        public override int Generate(float localNoise)
        {
            if (localNoise > 0.65) { return 5; }
            return 2;
        }
    }
}
