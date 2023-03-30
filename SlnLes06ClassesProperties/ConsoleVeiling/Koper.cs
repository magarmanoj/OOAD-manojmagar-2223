using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Koper
    {
        public string Name { get; set; }
        public List<Item> Aangeschafte { get; set; }

        public Koper(string name)
        {
            Name = name;
            Aangeschafte = new List<Item>();
        }

        public void ItemList(Item item)
        {
            Aangeschafte.Add(item);
        }
    }
}
