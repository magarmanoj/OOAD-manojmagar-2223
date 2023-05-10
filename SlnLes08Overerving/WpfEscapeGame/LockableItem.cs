using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    public class LockableItem : Item
    {
        public bool IsLocked { get; set; } = false;

        public LockableItem(string name, string desc) : base(name, desc) { }

        public LockableItem(string name, string desc, bool isPortable) : base(name, desc, isPortable) { }

    }
}
