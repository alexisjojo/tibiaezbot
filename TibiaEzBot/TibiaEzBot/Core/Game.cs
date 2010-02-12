
using System;
using TibiaEzBot.Core.Entities;
using TibiaEzBot.Core.Constants;

namespace TibiaEzBot.Core
{
	public class Game
	{
		#region Singleton
		private static Game instance;
		
		public static Game GetInstance()
		{
			if(instance == null)
				instance = new Game();
			
			return instance;
		}
		#endregion
		
		private Kernel kernel;
		
		private Game()
		{
			kernel = Kernel.GetInstance();
		}
		
		public bool Say(String words, SpeechType type) 
		{
			if(kernel.WorldProtocol != null)
			{
				kernel.WorldProtocol.SendSay(type, words);
				return true;
			}
			
			return false;
		}
		
		public bool Say(String words)
		{
			return Say(words, SpeechType.Say);
		}
		
		public bool Walk(Position pos)
        {
			if(GlobalVariables.IsConnected())
			{
	         	var Memory = kernel.Client.Memory;
	            Memory.WriteUInt32(Addresses.Player.GoToX, pos.X);
	            Memory.WriteUInt32(Addresses.Player.GoToY, pos.Y);
	            Memory.WriteUInt32(Addresses.Player.GoToZ, pos.Z);
	            Memory.WriteByte(GlobalVariables.GetPlayerMemoryAddress() + Addresses.Creature.DistanceIsWalking, Convert.ToByte(true));
				return true;
			}
			
			return false;
        }
		
		public bool UseItemOn(ushort itemId, Position position)
		{
			Tile tile = Map.GetInstance().GetTile(position);
			
			if(tile == null)
			{
				Logger.Log("Falha ao usar o item. Tile igual a null.", LogType.ERROR);
				return false;
			}
			
			int stackPos = tile.GetExtendedUseStackPosition();
			Thing thing = tile.GetThingByStackPosition(stackPos);
			
			if(thing is Item && stackPos != -1)
			{
				if(kernel.WorldProtocol != null)
						kernel.WorldProtocol.SendUseItemWith(new Position(0xFFFF, 0, 0), itemId,
					    	0, position, (ushort)thing.GetId(), (byte)stackPos);
			}
			else
			{
				Logger.Log("Falha ao usar o item. Item não encontrado.", LogType.ERROR);
				return false;
			}
			
			return true;
		}
		
		public bool UseItemOnSelf(ushort itemId)
		{
			if(kernel.WorldProtocol != null)
			{
				kernel.WorldProtocol.SendUseBattleWindow(new Position(0xFFFF, 0, 0),
                            itemId, 0, GlobalVariables.GetPlayerId());
				return true;
			}
			
			return false;
		}
		
		public bool UseItem(Position pos)
		{
			Tile tile = Map.GetInstance().GetTile(pos);
			
			if(tile == null)
			{
				Logger.Log("Falha ao usar o item. Tile igual a null.", LogType.ERROR);
				return false;
			}
			
			int stackPos = tile.GetUseStackPosition();
			Thing thing = tile.GetThingByStackPosition(stackPos);
			
			if(thing is Item && stackPos != -1)
			{
				Item item = (Item)thing;
				
				if(!item.IsExtendedUseable())
				{
					if(kernel.WorldProtocol != null)
						kernel.WorldProtocol.SendUseItem(pos, (ushort)item.GetId(), (byte)stackPos);
					else
					{
						Logger.Log("Falha ao usar item. Protocol não está iniciado.", LogType.ERROR);
						return false;
					}
				}
				else
				{
					Logger.Log("TODO: Send extended.");
				}	
			}
			else
			{
				Logger.Log("Falha ao usar o item. Item não encontrado.", LogType.ERROR);
				return false;
			}
			
			return true;
		}

        public void OnReceivePlayerMove(Position newPosition)
        {
            //Logger.Log("Nova Posição: " + newPosition);
        }
		
		public void OnReceiveCreatureSquare(Creature creature)
		{
			
		}
	}
}
