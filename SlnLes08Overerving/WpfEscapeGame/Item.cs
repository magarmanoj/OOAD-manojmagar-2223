namespace WpfEscapeGame
{
    public class Item : Actor
    {
        public bool IsPortable { get; set; } = true;
        public Key Key { get; set; }
        public Item HiddenItem { get; set; }

        public Item(string name, string desc) : base(name, desc) { }

        public Item(string name, string desc, bool isPortable) : base(name,desc) 
        {
            IsPortable = isPortable;
        }
    }
}
