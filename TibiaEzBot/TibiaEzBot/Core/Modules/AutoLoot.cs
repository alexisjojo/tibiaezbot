using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Modules
{
    public class AutoLoot : Module
    {
        private bool looting;

        public bool IsLooting { get { return looting; } }
    }
}
