using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Player
    {
        public uint Id { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public uint Capacity { get; set; }
        public int Soul { get; set; }
        public uint Experience { get; set; }
        public int Level { get; set; }
        public bool IsConnected { get; set; }
        public bool IsAttacking { get; set; }

    }
}
