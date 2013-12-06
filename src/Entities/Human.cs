using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Entities
{
    class Human : Unit
    {
        private static new int image;
        private float hunger; //100 is best, 0 is dead
        private Random rnd = new Random();

        private int defense;
        private int damage;

        public Human(int x, int y)
            : base(x, y, 20)
        {
            this.hunger = 100;
        }

        public Human(int x, int y, float hunger)
            : base(x, y, 20)
        {
            this.hunger = hunger;
        }

        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public float Hunger
        {
            get
            {
                if (hunger < 0)
                {
                    return 0;
                }
                if (hunger > 100)
                {
                    return 100;
                }
                return hunger;
            }
            set
            {
                hunger = value;
                if (hunger > 100)
                {
                    hunger = 100;
                }
            }
        }

        public override void Tick(World world, float delta)
        {
            base.Tick(world, delta);
            UpdateStats();
            if (this.action == Actions.Move)
            {
                hunger -= (float)0.6 * delta;
            }
            else
            {
                hunger -= (float)0.2 * delta;
            }

            if (hunger <= 0)
            {
                if (this.Inventory != null)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (Item.GetItem("Meat") == this.Inventory.GetItemAt(i))
                        {
                            this.Inventory.SetItemAt(i, -1);
                            this.Hunger += 100;
                            this.Health += 10;
                            return;
                        }
                    }
                }
                if (rnd.NextDouble() > 0.9)
                {
                    this.Health = this.Damage(1);
                    if (this.Health <= 0)
                    {
                        this.Kill(world);
                    }
                }
            }

        }

        public void UpdateStats()
        {
            if (this.Inventory != null)
            {
                damage = 1;
                defense = 0;

                if (this.Inventory.GetEquipmentAt(0) != -1 && ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(0)) != null))
                {
                    damage += ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(0))).Damage;
                    defense += ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(0))).Defense;
                }

                if (this.Inventory.GetEquipmentAt(1) != -1 && ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(1)) != null))
                {
                    damage += ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(1))).Damage;
                    defense += ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(1))).Defense;
                }

                if (this.Inventory.GetEquipmentAt(2) != -1 && ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(2)) != null))
                {
                    damage += ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(2))).Damage;
                    defense += ((Equipment)Item.GetItem(this.Inventory.GetEquipmentAt(2))).Defense;
                }
            }
        }

        public static void Init(int image)
        {
            Human.image = image;
        }

        public override void Draw(System.Drawing.Graphics g, View view, World world, float delta)
        {
            base.Draw(g, view, world, delta, Human.image);
        }
    }
}
