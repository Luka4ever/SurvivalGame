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

        public virtual void Use(World world, Unit source)
        {

        }

        public static void Init()
        {

        }
    }
}
