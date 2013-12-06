using SurvivalGame.src.Items;
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

        public static Item GetItem(int item)
        {
            return items[item];
        }

        public int Icon
        {
            get { return this.icon; }
        }

        public virtual void Use(World world, Unit source)
        {

        }

        public static void Init()
        {
            items.Add(new StandardItem("Lumber", ImageManager.RegisterImage(@"res/Interface/refinedMatIcon.png")));
            items.Add(new StandardItem("Iron Ore", ImageManager.RegisterImage(@"res/Interface/refinedMatIcon.png")));
            items.Add(new StandardItem("Stone", ImageManager.RegisterImage(@"res/Interface/refinedMatIcon.png")));
            items.Add(new StandardItem("Lilypad", ImageManager.RegisterImage(@"res/Interface/refinedMatIcon.png")));
            items.Add(new StandardItem("Red Flower", ImageManager.RegisterImage(@"res/Interface/refinedMatIcon.png")));
            items.Add(new StandardItem("Blue Flower", ImageManager.RegisterImage(@"res/Interface/refinedMatIcon.png")));
            items.Add(new StandardItem("Cactus", ImageManager.RegisterImage(@"res/Interface/refinedMatIcon.png")));

            items.Add(new StandardArmor("Farmer Clothes", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 1, ImageManager.RegisterImage(@"res/Armor/FarmerVest.png")));
            items.Add(new StandardArmor("Bark Armor", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 3, ImageManager.RegisterImage(@"res/Armor/BarkArmor.png")));
            items.Add(new StandardArmor("Miner Clothes", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 3, ImageManager.RegisterImage(@"res/Armor/MinerVest.png")));
            items.Add(new StandardArmor("Leather Armor", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 5, ImageManager.RegisterImage(@"res/Armor/LeatherArmor.png")));
            items.Add(new StandardArmor("Chain Mail", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 7, ImageManager.RegisterImage(@"res/Armor/ChainMail.png")));
            items.Add(new StandardArmor("Plate Mail", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 9, ImageManager.RegisterImage(@"res/Armor/PlateMail.png")));

            items.Add(new StandardHelmet("Straw Hat", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 0, ImageManager.RegisterImage(@"res/Helmet/StrawHat.png")));
            items.Add(new StandardHelmet("Miners Cap", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 1, ImageManager.RegisterImage(@"res/Helmet/MiningCap.png")));
            items.Add(new StandardHelmet("Leather Cap", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 2, ImageManager.RegisterImage(@"res/Helmet/ironHelm.png")));
            items.Add(new StandardHelmet("Metal Coif", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 4, ImageManager.RegisterImage(@"res/Helmet/SteelHelm.png")));
            items.Add(new StandardHelmet("Plate Helmet", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 0, 6, ImageManager.RegisterImage(@"res/Helmet/SteelHelm2.png")));

            items.Add(new StandardWeapon("Pitchfork", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 1, 0, ImageManager.RegisterImage(@"res/Tools/PitchFork.png")));
            items.Add(new StandardWeapon("Dagger", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 1, 0, ImageManager.RegisterImage(@"res/Tools/Dagger.png")));
            items.Add(new StandardWeapon("Short Sword", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 2, 0, ImageManager.RegisterImage(@"res/Tools/ShortSword.png")));
            items.Add(new StandardWeapon("Spear", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 2, 0, ImageManager.RegisterImage(@"res/Tools/Spear.png")));
            items.Add(new StandardWeapon("Pickaxe", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 1, 0, ImageManager.RegisterImage(@"res/Tools/PickAxe.png")));
            items.Add(new StandardWeapon("Hatchet", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 2, 0, ImageManager.RegisterImage(@"res/Tools/HandAxe.png")));
            items.Add(new StandardWeapon("Sabre", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 3, 0, ImageManager.RegisterImage(@"res/Tools/sabre.png")));
            items.Add(new StandardWeapon("Claidheamhmór", ImageManager.RegisterImage(@"res/Interface/EquipmentIcon.png"), 5, 0, ImageManager.RegisterImage(@"res/Tools/Claymore.png")));
        }
    }
}
