using System.Windows.Documents;

namespace WpfEscapeGame
{
    public class Door : Actor
    {
        public bool IsLocked { get; set; } = false;
        public Item Key { get; set; }
        public Room ToRoom { get; set; }

        public Door(string name, string desc) : base(name, desc) { }

        public Door(string name, string desc, bool islocked) : base(name, desc)
        {
            IsLocked = islocked;
        }
    }
}
