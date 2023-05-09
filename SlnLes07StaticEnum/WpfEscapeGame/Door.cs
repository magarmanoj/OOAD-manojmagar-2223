using System.Windows.Documents;

namespace WpfEscapeGame
{
    public class Door
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLocked { get; set; } = false;
        public Item Key { get; set; }
        public Room ToRoom { get; set; }

        public Door(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public Door(string name, string desc, bool islocked)
        {
            Name = name;
            Description = desc;
            IsLocked = islocked;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
