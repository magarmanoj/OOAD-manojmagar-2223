namespace WpfEscapeGame
{
    internal class Door
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Room ToRoom { get; set; }

        public Door(string name, string desc)
        {
            Name = name;
            Description = desc;
        }
    }
}
