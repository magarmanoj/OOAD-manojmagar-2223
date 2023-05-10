using System.Collections.Generic;
using System.Security.RightsManagement;

namespace WpfEscapeGame
{
    public class Room
    {
        public string Name { get; } // read-only: kan maar één keer ingesteld worden
        public string Description { get; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Door> Doors { get; set; } = new List<Door>();
        public string ImagePath { get; set; }

        public Room(string name, string desc)
        {
            Name = name;
            Description = desc;
        }
    }
}
