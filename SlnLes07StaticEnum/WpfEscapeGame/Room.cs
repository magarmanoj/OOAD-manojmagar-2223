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

        public Room(string name, string desc, string imagePath)
        {
            Name = name;
            Description = desc;
            ImagePath = imagePath;
        }

        public List<Room> FindConnectedRooms()
        {
            List<Room> connectedRooms = new List<Room>();

            foreach (Door door in Doors)
            {
                if (door.ToRoom != null && !connectedRooms.Contains(door.ToRoom))
                {
                    connectedRooms.Add(door.ToRoom);
                }
            }

            return connectedRooms;
        }
    }
}
