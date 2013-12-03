using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes.Continents
{
    public class ContinentSea : Biome
    {
        public ContinentSea(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {
            biomes.Add(new BiomeSea(0, 0.75f, "Sea"));
            biomes.Add(new BiomeDesert(0.75f, 0.8f, "Desert"));
            biomes.Add(new BiomeField(0.8f, 1, "Field"));
        }
    }
}
