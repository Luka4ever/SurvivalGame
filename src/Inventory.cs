using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurvivalGame.src
{
    class Inventory
    {
        private int[] slots;
        private int[] equipment;
        private bool crafting;

        public Inventory(int slots, int equipment, bool crafting)
        {
            this.slots = new int[slots];
            this.equipment = new int[equipment];
            this.crafting = crafting;
        }
    }
}
