using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalGame;

namespace SurvivalGame.src.Entities
{
    class Bunny : Unit
    {
        private static new int image;
        private static Random random = new Random();

        public Bunny(int x, int y) : base(x, y, 5)
        {

        }

        public override void Tick(World world, float delta)
        {
            List<Entity> entities = world.GetEntitiesAt(this.x, this.y);
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].GetType() == typeof(Human))
                {
                    this.Kill(world);
                }
            }
            if (this.action == Actions.None)
            {
                for (int y = -2; y < 3; y++)
                {
                    for (int x = -2; x < 3; x++)
                    {
                        Entity e = world.GetEntityAt(this.x + x, this.y + y);
                        if (e != null && typeof(Bunny) != e.GetType())
                        {
                            if (typeof(Human) == e.GetType())
                            {
                                if (y < 0)
                                {
                                    this.MoveDown();
                                }
                                else if (y > 0)
                                {
                                    this.MoveUp();
                                }
                                if (x < 0)
                                {
                                    this.MoveRight();
                                }
                                else if (x > 0)
                                {
                                    this.MoveLeft();
                                }
                                this.nextAction = Actions.Move;
                            }
                        }
                    }
                }
                if (this.nextAction == Actions.None && random.NextDouble() > 0.97)
                {
                    this.nextAction = Actions.Move;
                    switch (random.Next(4))
                    {
                        case 0:
                            this.MoveUp();
                            break;
                        case 1:
                            this.MoveRight();
                            break;
                        case 2:
                            this.MoveDown();
                            break;
                        case 3:
                            this.MoveLeft();
                            break;
                    }
                }
            }
            base.Tick(world, delta);
        }

        public static void Init(int image)
        {
            Bunny.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Bunny.image);
        }

        public override void Kill(World world)
        {
            base.Kill(world);
            Chunk chunk = world.GetChunk((int)Math.Floor((double)this.x / (double)Chunk.size), (int)Math.Floor((double)this.y / (double)Chunk.size));
            chunk.AddEntity(new ItemEntity(this.x, this.y, Item.GetItem("Meat")));
        }
    }
}
