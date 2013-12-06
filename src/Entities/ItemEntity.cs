using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class ItemEntity : Entity
    {
        public int item;

        public ItemEntity(int x, int y, int item)
            : base(x, y)
        {
            this.item = item;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Item.GetItem(this.item).Icon);
        }

        public override void Tick(World world, float delta)
        {
            base.Tick(world, delta);
            Chunk chunk = world.GetChunk((int)Math.Floor((double)this.x / (double)Chunk.size), (int)Math.Floor((double)this.y / (double)Chunk.size));
            if (chunk != null)
            {
                List<Entity> entities = chunk.GetEntitiesAt(this.x, this.y);
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].GetType() == typeof(Human) && ((Human)entities[i]).Inventory != null)
                    {
                        if (((Human)entities[i]).Inventory.AddItem(this.item))
                        {
                            chunk.RemoveEntity(this);
                        }
                    }
                }
            }
        }
    }
}
