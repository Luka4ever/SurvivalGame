using SurvivalGame.src.Biomes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    class UI
    {
        private Entity player;
        private int line;
        private Font font;
        private Brush brush;

        public UI(Entity player)
        {
            this.player = player;
            this.font = new Font("Lucida Console", 14, FontStyle.Bold);
            this.brush = new SolidBrush(Color.White);
        }

        private void Reset()
        {
            this.line = 0;
        }

        private void DrawLine(Graphics g, String line)
        {
            g.DrawString(line, font, brush, 10, 10 + (this.line++) * 20);
        }

        public void Draw(Graphics g, World world)
        {
            Reset();
            DrawLine(g, "X: " + this.player.X + ", Y: " + this.player.Y);
            DrawLine(g, "S: " + world.Seed + ", L: " + Chunk.GetLocalSeed(world.Seed) + ", B: " + Chunk.GetBiomeSeed(world.Seed) + ", C: " + Chunk.GetContinentSeed(world.Seed));
            Biome C = Biome.GetContinentFromNoise(Chunk.GetContinentNoise(this.player.X, this.player.Y, Chunk.GetContinentSeed(world.Seed)));
            Biome B = C.GetBiomeFromNoise(Chunk.GetBiomeNoise(this.player.X, this.player.Y, Chunk.GetBiomeSeed(world.Seed)));
            DrawLine(g, "C: " + C.Name + ", B: " + B.Name);
            float CN = Chunk.GetContinentNoise(this.player.X, this.player.Y, Chunk.GetContinentSeed(world.Seed));
            float BN = Chunk.GetBiomeNoise(this.player.X, this.player.Y, Chunk.GetBiomeSeed(world.Seed));
            float LN = Chunk.GetLocalNoise(this.player.X, this.player.Y, Chunk.GetLocalSeed(world.Seed));
            DrawLine(g, "CN: " + CN + ", BN: " + BN + ", LN: " + LN);
        }
    }
}
