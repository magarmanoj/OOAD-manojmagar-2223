using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    public class Key : Item
    {
        public Key(string name, string desc) : base(name, desc) { }

        public Key(string name, string desc, bool isPortable) : base(name, desc, isPortable) { }
    }
}
