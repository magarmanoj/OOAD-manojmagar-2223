﻿using System.Net.NetworkInformation;

namespace WpfEscapeGame
{
    public class Item : Actor
    {

        public bool IsLocked { get; set; } = false;
        public bool IsPortable { get; set; } = true;
        public Item Key { get; set; }
        public Item HiddenItem { get; set; }
        public Item(string name, string desc) : base(name, desc) { }

        public Item(string name, string desc, bool isPortable) : base(name,desc) 
        {
            IsPortable = isPortable;
        }
    }
}
