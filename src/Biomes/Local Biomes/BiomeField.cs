using SurvivalGame.src.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    class BiomeField : Biome
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

        public override Entity GenerateEntity(float localNoise, float biomeNoise, float continentNoise, int x, int y, double seed)
        {
            if (localNoise > 0.25 && localNoise < 0.8)
            {
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.02)
                {
                    return new OakTree(x, y);
                }

                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.04)
                {
                    return new Flower2(x, y);
                }

                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.06)
                {
                    return new Flower1(x, y);
                } 
            }

            if (localNoise < 0.25)
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

            if (localNoise > 0.8)
            {
                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.02)
                {
                    return new Lilypad1(x, y);
                }

                if (localNoise * localNoise * seed / biomeNoise * continentNoise % 1 < 0.04)
                {
                    return new Lilypad2(x, y);
                }
            }

            return null;
        }
    }
}
