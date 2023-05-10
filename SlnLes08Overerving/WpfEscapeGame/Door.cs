namespace WpfEscapeGame
{
    public class Door : LockableItem
    {
        public Room ToRoom { get; set; }

        public Door(string name, string desc) : base(name, desc) { }

    }
}
