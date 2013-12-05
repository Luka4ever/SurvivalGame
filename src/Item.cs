﻿using SurvivalGame.src.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    abstract class Item
    {
        private string name;
        private int icon;
        private static List<Item> items = new List<Item>();

        public Item(string name, int icon)
        {
            this.name = name;
            this.icon = icon;
        }

        public virtual void Use(World world, Unit source)
        {

        }

        public static void Init()
        {
            items.Add(new Lumber("Lumber", ImageManager.RegisterImage(@"res/Entities/Human.png")));
        }
    }
}
