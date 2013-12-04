using SurvivalGame.src.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    class BiomeSea : Biome
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

        public override Entity GenerateEntity(float localNoise, float biomeNoise, float continentNoise, int x, int y, double seed)
        {
            if (localNoise > 0.8)
            {
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.02)
                {
                    return new Pebble1(x, y);
                }
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.04)
                {
                    return new Pebble2(x, y);
                }
            }
            return null;
        }
    }
}
