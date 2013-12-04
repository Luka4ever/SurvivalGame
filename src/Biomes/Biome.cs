using SurvivalGame.src.Biomes.Continents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes
{
    abstract class Biome
    {
        private float minRange;
        private float maxRange;
        private string name;
        protected List<Biome> biomes = new List<Biome>();
        private static List<Biome> continents = new List<Biome>();

        public Biome(float minRange, float maxRange, string name)
        {
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.name = name;
        }

        public static void Init()
        {
            continents.Add(new ContinentSea(0, 0.3f, "Sea"));
            continents.Add(new ContinentLush(0.3f, 0.65f, "Lush"));
            continents.Add(new ContinentDry(0.65f, 1, "Dry"));

        }

        public Biome GetBiomeFromNoise(float biomeNoise)
        {
            foreach (Biome biome in biomes)
            {
                if (biome.IsInRange(biomeNoise))
                {
                    return biome;
                }
            }
            return null;
        }

        public static Biome GetContinentFromNoise(float biomeNoise)
        {
            foreach (Biome biome in continents)
            {
                if (biome.IsInRange(biomeNoise))
                {
                    return biome;
                }
            }
            return null;
        }

        public virtual bool IsInRange(float biomeNoise)
        {
            if (biomeNoise >= this.maxRange) { return false; }
            if (biomeNoise <= this.minRange) { return false; }
            return true;
        }

        public virtual int Generate(float localNoise)
        {
            return 1;
        }

        public virtual Entity GenerateEntity(float localNoise, float biomeNoise, float continentNoise, int x, int y, double seed)
        {
            return null;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
