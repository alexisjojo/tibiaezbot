using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Constants;

namespace TibiaEzBot.Core.Entities
{
    public class Inventory
    {
    	#region Singleton
    	private static Inventory instance;
    	
    	public static Inventory GetInstance()
    	{
    		if(instance == null)
    			instance = new Inventory();
    			
    		return instance;
    	}
    	
    	private Inventory() { }
    	#endregion
    
    	private Item[] inventory = new Item[(int)SlotNumber.Last];
    
    	public Item GetItem(uint slot)
    	{
    		if(slot < (int)SlotNumber.First || slot > (int)SlotNumber.Last)
				return null;

			return inventory[slot - 1];
    	}
    	
		public bool AddItem(uint slot, Item item)
		{
            if (slot < (int)SlotNumber.First || slot > (int)SlotNumber.Last)
				return false;

			inventory[slot - 1] = item;
			return true;
		}
		
		public bool RemoveItem(uint slot)
		{
            if (slot < (int)SlotNumber.First || slot > (int)SlotNumber.Last)
				return false;
				
			inventory[slot - 1] = null;
			return true;
		}
    
    	public void Clear()
    	{
            Logger.Log("Limpando o inventario.");
            for (uint i = (int)SlotNumber.First; i < (int)SlotNumber.Last; ++i)
    		{
				inventory[i] = null;
			}
    	}
    }
}
