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
        private int[] craftingSlots;
        private bool crafting;

        public Inventory(int slots, int equipment, bool crafting)
        {
            this.slots = new int[slots];
            Random rnd = new Random();
            for (int i = 0; i < slots; i++)
            {
                this.slots[i] = rnd.Next(24) - 1;
            }
            this.equipment = new int[equipment];
            for (int i = 0; i < equipment; i++)
            {
                this.equipment[i] = -1;
            }
            this.crafting = crafting;
            if (this.crafting)
            {
                this.craftingSlots = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    this.craftingSlots[i] = -1;
                }
            }
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

        public void SetEquipmentAt(int slot, int item)
        {
            this.equipment[slot] = item;
        }

        public int GetCraftingItemAt(int slot)
        {
            return this.craftingSlots[slot];
        }

        public void SetCraftingItemAt(int slot, int item)
        {
            this.craftingSlots[slot] = item;
        }
    }
}
