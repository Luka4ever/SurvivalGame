using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes.Continents
{
    public class ContinentDry : Biome
    {
        public ContinentDry(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {
            biomes.Add(new BiomeField(0, 0.2f, "Field"));
            biomes.Add(new BiomeMountain(0.2f, 0.55f, "Mountain"));
            biomes.Add(new BiomeDesert(0.55f, 1, "Desert"));
        }
    }
}
