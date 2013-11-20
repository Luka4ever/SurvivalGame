using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurvivalGame.src
{
    class Unit : Entity
    {
        private int health;
        private Inventory inventory;
        
        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        public int Damage(int damage)
        {
            return this.health -= damage;
        }

        public override void Tick(World world)
        {
            base.Tick(world);
        }
    }
}
