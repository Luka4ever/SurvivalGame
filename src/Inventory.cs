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
            for (int i = 0; i < slots; i++)
            {
                this.slots[i] = -1;
            }
            this.equipment = new int[equipment];
            this.crafting = crafting;
        }

        public int GetItemAt(int slot)
        {
            return this.slots[slot];
        }

        public void SetItemAt(int slot, int item)
        {
            this.slots[slot] = item;
        }

        public int GetEquipmentAt(int slot)
        {
            return this.equipment[slot];
        }
    }
}
