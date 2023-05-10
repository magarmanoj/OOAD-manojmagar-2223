using System.Collections.Generic;
using System.Security.RightsManagement;

namespace WpfEscapeGame
{
    public class Room : Actor
    {
        
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Door> Doors { get; set; } = new List<Door>();
        public string ImagePath { get; set; }

        public Room(string name, string desc) : base(name, desc) { }
    }
}
