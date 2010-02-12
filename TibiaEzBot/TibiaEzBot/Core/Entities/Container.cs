using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Container
    {
    	private uint id;
    	private List<Item> items = new List<Item>();
    	private ushort itemId;
    	private uint capacity;
    	private String name;
    	private bool hasParent;
    
    	public Container() 
    	{
    		capacity = 20;
    	}
		
		public Container(uint id)
		{
			this.id = id;
		}
		
		public Item GetItem(uint slot)
		{
			if(slot >= capacity){
				Logger.Log("Falha ao tentar pegar um item em um slot maior que a capacidade do container. Slot: " + slot, LogType.ERROR);
				return null;
			}

			if(slot >= items.Count)
				return null;
		
			return items[(int)slot];
		}
		
		public bool AddItem(Item item)
		{
			if(items.Count == capacity){
				Logger.Log("Falha ao tentar adicionar um item em um container cheio.", LogType.ERROR);
				return false;
			}
			
			items.Insert(0, item);
			return true;
		}
		
		public bool AddItemInitial(Item item)
		{
			if(items.Count == capacity){
				Logger.Log("Falha ao tentar adicionar um item em um container cheio.", LogType.ERROR);
				return false;
			}
			
			items.Add(item);
			return true;
		}
		
		public bool RemoveItem(uint slot)
		{
			if(slot >= capacity){
				Logger.Log("Falha ao tentar remover um item em um slot maior que a capacidade do container. Slot: " + slot, LogType.ERROR);
				return false;
			}

			if(slot >= items.Count)
			{
				Logger.Log("Tentando remover um item em um slot nulo.");
				return false;
			}
			
			items.RemoveAt((int)slot);
			return true;
		}
		
		public bool UpdateItem(uint slot, Item newItem)
		{
			if(slot >= capacity){
				Logger.Log("Falha ao tentar atualizar um item em um slot maior que a capacidade do container. Slot: " + slot, LogType.ERROR);
				return false;
			}
			
			if(slot >= items.Count)
			{
				Logger.Log("Tentando atualizar um item em um slot nulo.", LogType.ERROR);
				return false;
			}

			items[(int)slot] = newItem;
			return true;
		}

		public void SetItemId(ushort itemId) { this.itemId = itemId; }
		public ushort GetItemId() { return itemId; }

		public void SetName(String name){ this.name = name; }
		public String GetName() { return name; }

		public void SetCapacity(uint cap){ this.capacity = cap; }
		public uint GetCapacity() { return capacity; }

		public void SetHasParent(bool hasParent){ this.hasParent = hasParent; }
		public bool GetHasParent() { return hasParent; }

		public int GetSize() { return items.Count; }

		public uint GetId() {return id; }
    }
}
