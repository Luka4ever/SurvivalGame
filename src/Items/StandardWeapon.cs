﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.src.Items
{
    class StandardWeapon : Equipment
    {
        public StandardWeapon(string name, int icon, int damage, int defense, int equippedImage)
            : base(name, icon, damage, defense, equippedImage)
        {

        }

    }
}
