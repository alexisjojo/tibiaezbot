using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Tile
    {
        private Position position;
        private IList<Thing> objects;
        private Item ground;

        public Tile()
        {
            objects = new List<Thing>();
        }

        public Position GetPosition() { return position; }
        public void SetPosition(Position position) { this.position = position; }

        public Item GetGround() { return ground; }
        public void SetGround(Item ground) { this.ground = ground; }

        public int GetThingCount() { return (ground != null ? 1 : 0) + objects.Count; }

        public void Clear()
        {
            ground = null;

            foreach (Thing thing in objects)
            {
                if (thing is Creature)
                {
                    ((Creature)thing).SetPosition(new Position(0, 0, 0));
                }
            }

            objects.Clear();
        }

        public bool InsertThing(Thing thing, int stackPosition)
        {
            if (thing == null) { return false; }

            if (GetThingCount() == 10)
            {
                Thing pushThing = GetThingByStackPosition(9);
                if (!RemoveThing(pushThing)) { return false; }
            }

            int pos = stackPosition;

            if (thing is Item)
            {
                Item item = (Item)thing;
                if (item.IsGroundTile() && pos == 0)
                {
                    ground = item;
                    return true;
                }
                else if (item.IsGroundTile() && pos != 0) 
                { 
                    Logger.Log("Tentando inserir um ground, porem o stack fornecido não é igual a zero.", LogType.ERROR);
                    return false;
                }
            }

            if (pos != 255)
            {
                if (ground != null) 
                    --pos;

                int it = 0;
                while (pos > 0 && it < objects.Count)
                {
                    pos--;
                    ++it;
                }

                if (pos > 0) 
                    return false;

                objects.Insert(it, thing);

                return true;
            }
            else
            {
                return AddThing(thing, true);
            }
        }

        public bool RemoveThing(Thing thing)
        {
            if (thing == null) { return false; }

            if (thing.Equals(ground))
            {
                ground = null;
                return true;
            }

            return objects.Remove(thing);
        }

        public bool AddThing(Thing thing, bool pushThing)
        {
            if (thing == null) { return false; }

            if (GetThingCount() == 10)
            {
                Thing push = GetThingByStackPosition(9);
                if (!RemoveThing(push)) 
                {
                 	Logger.Log("O tile está cheio, e não foi possivel remover o objeto.");
                	return false; 
                }
            }

            if (thing is Item)
            {
                Item item = (Item)thing;

                if (item.IsGroundTile())
                {
                    if (ground == null)
                    {
                        ground = item;
                        return true;
                    }
                    else
                    {
                        Logger.Log("Tentando adicionar um ground a um tile, porem o tile já possui um ground.", LogType.ERROR);
                        return false;
                    }
                }
            }

            uint thingOrder = thing.GetOrder();

            int it = 0;

            for (; it < objects.Count; ++it)
            {
                uint itThingOrder = objects[it].GetOrder();

                if (pushThing)
                {
                    if (itThingOrder >= thingOrder)
                        break;
                }
                else
                {
                    if (itThingOrder > thingOrder)
                        break;
                }

            }

            objects.Insert(it, thing);

            if (thing is Creature)
            {
                Creature cr = (Creature)thing;
                cr.SetPosition(this.GetPosition());

                Logger.Log("Creatura: " + cr.GetName() + " é reachable: " + cr.IsReachable());
            }

            return true;
        }

        public bool AddThing(Thing thing) { return AddThing(thing, false); }

        public Thing GetThingByStackPosition(int position)
        {
            if (ground != null)
            {
                if(position == 0)
                    return ground;

                position--;
            }

            if (position < objects.Count)
            {
                return objects[position];
            }

            return null;
        }

        public ushort GetSpeedIndex()
        {
            return (ushort)(ground != null ? Objects.GetInstance().GetItemType((ushort)ground.GetId()).Speed : 500);
        }

        public bool IsTileBlocking()
        {
            for (int pos = 0; pos != GetThingCount(); ++pos)
            {
                Thing thing = GetThingByStackPosition(pos);
                if (thing == null) { return false; }

                if (thing is Creature)
                {
                    Creature creature = (Creature)thing;

                    if (creature.IsImpassable())
                        return true;
                }
                else if (thing is Item)
                {
                    Item item = (Item)thing;

                    if (item.IsBlocking())
                        return true;
                }
            }

            return false;
        }
		
		public int GetUseStackPosition()
		{
			int lastPos = 0;
			
			for(int pos = 0; pos != GetThingCount(); ++pos) 
			{
				Thing thing = GetThingByStackPosition(pos);
				
				if(thing == null)
					return 0;
		
				if((thing is Item))
				{
					Item item = (Item)thing;
					Thing lastThing = GetThingByStackPosition(lastPos);
		
					if(item.IsAlwaysUsed()) 
						return pos;
					
					if(lastThing == null || thing.GetOrder() > lastThing.GetOrder()) 
						lastPos = pos;
				}
			}
		
			return lastPos;
		}
		
		public int GetExtendedUseStackPosition()
		{
			int lastPos = 0;
			int lastCreaturePos = 0;
			
			for(int pos = 0; pos != GetThingCount(); ++pos) 
			{
				Thing thing = GetThingByStackPosition(pos);
				
				if(thing == null)
					return 0;
		
				Item item = thing.GetItem();
		
				if(item != null || thing.GetCreature() != null)
				{
					Thing lastThing = GetThingByStackPosition(lastPos);
		
					if(item != null && item.IsAlwaysUsed())
						return pos;
					else if(lastThing == null || thing.GetOrder() > lastThing.GetOrder()) 
					{
						lastPos = pos;
						if(thing.GetCreature() != null)
							lastCreaturePos = pos;
					}
				}
			}
			
			if(lastCreaturePos == 0)
				return lastPos;
			
			return lastPos;
		}
		
		public byte GetMinimapColor()
		{
		    byte color = 0;	
		    
			if(GetGround() != null)
				color = GetGround().GetMapColor();
			
			foreach(Thing thing in objects)
			{
				if(thing.GetMapColor() > 0)
					color = thing.GetMapColor();
			}
			
		    return color;
		}
    }
}
