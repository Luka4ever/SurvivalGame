using SurvivalGame.src.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    class BiomeForest : Biome
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

        public override Entity GenerateEntity(float localNoise, float biomeNoise, float continentNoise, int x, int y, double seed)
        {
            if (localNoise > 0.65) 
            {
                if (localNoise * localNoise * Math.Abs(x) * Math.Abs(y) / biomeNoise * continentNoise % 1 < 0.7)
                {
                    return new PineTree(x, y);
                }
            }

            if (localNoise < 0.65)
            {
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.045)
                {
                    return new PineTree(x, y);
                }
                
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.1)
                {
                    return new GrassTuft(x, y);
                } 
            }
            return null;
        }
    }
}
