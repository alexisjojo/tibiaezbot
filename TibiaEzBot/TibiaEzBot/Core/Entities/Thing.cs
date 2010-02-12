using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Thing
    {
    	public virtual uint GetId() { return 0; }
    	public virtual uint GetOrder() { return 0; }
		
		public virtual Item GetItem() { return null; }
		public virtual Creature GetCreature() { return null; }
		public virtual byte GetMapColor() { return 0; }
    }
}
