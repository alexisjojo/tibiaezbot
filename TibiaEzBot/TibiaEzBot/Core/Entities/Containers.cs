using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Containers
    {
    	public const uint MAX_ALLOWED_CONTAINERS = 15;	
    	
    	#region Singleton
    	private static Containers instance;
    	
    	public static Containers GetInstance()
    	{
    		if(instance == null)
    			instance = new Containers();
    			
    		return instance;
    	}
    	
    	private Containers() { }
    	#endregion
    	
    	private Container[] containers = new Container[MAX_ALLOWED_CONTAINERS];
    
		public int GetFreeContainerSlot()
		{
			for(int i = 0; i < MAX_ALLOWED_CONTAINERS; ++i)
			{
				if(containers[i] == null)
					return i;
			}
			
			return  (int)MAX_ALLOWED_CONTAINERS - 1;
		}

		public Container GetContainer(uint id)
		{
			if(id >= MAX_ALLOWED_CONTAINERS)
			{
				Logger.Log("Falha ao tentar pegar um container com um id maior que o número maximo de containers. Id: " + id, LogType.ERROR);		
				return null;
			}

			return containers[id];
		}
		
		public Container CreateContainer(uint id)
		{
			if(id >= MAX_ALLOWED_CONTAINERS)
			{
				Logger.Log("Falha ao tentar criar um container com um id maior que o número maximo de containers. Id: " + id, LogType.ERROR);		
				return null;
			}
			
			containers[id] = new Container(id);
			return containers[id];
		}
		
		public bool DeleteContainer(uint id)
		{
			if(id >= MAX_ALLOWED_CONTAINERS)
			{
				Logger.Log("Falha ao tentar deletar um container com um id maior que o número maximo de containers. Id: " + id, LogType.ERROR);		
				return false;
			}
			
			containers[id] = null;
            return true;
		}
    
    	public void Clear()
    	{
            Logger.Log("Limpando containers.");

    		for(uint i = 0; i < MAX_ALLOWED_CONTAINERS; ++i){
				containers[i] = null;
			}
    	}
    }
}
