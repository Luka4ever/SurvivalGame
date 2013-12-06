﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src
{
    abstract class Equipment : Item
    {
        private string name;
        private int icon;
        private int equippedImage;
        private int damage;
        private int defense;

        public Equipment(string name, int icon, int damage, int defense, int equippedImage)
            : base(name, icon)
        {
            this.damage = damage;
            this.defense = defense;
            this.equippedImage = equippedImage;
        }

        public virtual void Use(World world, Unit source)
        {

        }

        public static void Init()
        {

        }
    }
}
