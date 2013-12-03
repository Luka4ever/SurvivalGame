using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Biomes.Continents
{
    public class ContinentLush : Biome
    {
        public ContinentLush(float minRange, float maxRange, string name)
            : base(minRange, maxRange, name)
        {
            this.biomes.Add(new BiomeField(0, 0.35f, "Field"));
            this.biomes.Add(new BiomeForest(0.35f, 0.65f, "Forest"));
            this.biomes.Add(new BiomeSea(0.65f, 0.75f, "Sea"));
            this.biomes.Add(new BiomeMountain(0.75f, 1, "Mountain"));
        }
    }
}
