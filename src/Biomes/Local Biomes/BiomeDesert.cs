using SurvivalGame.src.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    class BiomeDesert : Biome
    {
        public BiomeDesert(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {

        }


        public override int Generate(float localNoise)
        {
            if (localNoise > 0.92) { return 1; }
            return 4;
        }

        public override Entity GenerateEntity(float localNoise, float biomeNoise, float continentNoise, int x, int y, double seed)
        {
            if (localNoise < 0.92)
            {
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.005)
                {
                    return new Ore(x, y);
                }

                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.01)
                {
                    return new Cactus(x, y);
                }
                
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.04)
                {
                    return new Pebble1(x, y);
                }

                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.06)
                {
                    return new Pebble2(x, y);
                }

                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.07)
                {
                    return new Orc(x, y);
                }

            }
            return null;
        }
    }
}
