using System.Collections.Generic;

namespace ConsoleVeiling
{
    internal class Koper
    {
        private string _name;

        public string Name 
        { 
            get { return _name; }
        }
        public List<Item> Aangeschafte { get; set; }

        public Koper(string name)
        {
            _name = name;
            Aangeschafte = new List<Item>();
        }

        public void ItemList(Item item)
        {
            Aangeschafte.Add(item);
        }
    }
}
