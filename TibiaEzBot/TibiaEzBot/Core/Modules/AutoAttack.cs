using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Entities;
using TibiaEzBot.Core.Constants;

namespace TibiaEzBot.Core.Modules
{
    public class AutoAttack : Module
    {
        public bool Enable { get; set; }
        public bool TargetAll { get; set; }

        public override void Initialize()
        {
        }

		public void OnReceiveCreatureSquare(Creature creature)
		{
			if(Enable && !GlobalVariables.IsAttacking() && creature.GetSquare() == SquareColor.Black)
			{
				//TODO: Verificar se a creature é um monstro.
				
			}
		}
		
        //private bool Proxy_ReceivedCreatureSquareIncomingPacket(Packets.IncomingPacket packet)
        //{
        //    //if (Enable && !Kernel.getInstance().Player.IsAttacking)
        //    //{
        //    //    Packets.Incoming.CreatureSquarePacket p = (Packets.Incoming.CreatureSquarePacket)packet;

        //    //    if (p.Color == SquareColor.Black)
        //    //    {
        //    //        var creature = Kernel.getInstance().BattleList[p.CreatureId];

        //    //        if (creature != null && CreatureLists.AllCreatures.ContainsKey(creature.Name))
        //    //        {
        //    //            Kernel.getInstance().Player.Attack((uint)creature.Id);
        //    //        }
        //    //    }
        //    //}

        //    return true;
        //}

        public override void Run()
        {
            //if (TargetAll && !Kernel.getInstance().Player.IsAttacking)
            //{
            //    TibiaEzBot.Core.Entities.Creature creature = null;

            //    //Se estivermos coletando o loot nao podemos atacar qualquer creatura.
            //    if (Kernel.getInstance().AutoLoot.IsLooting)
            //    {
            //        Objects.Location playerLocation = Kernel.getInstance().Client.PlayerLocation;
            //        creature = Kernel.getInstance().BattleList.GetScreenMonsters().FirstOrDefault(
            //            delegate(TibiaEzBot.Core.Entities.Creature cr) { return cr.Location.DistanceTo(playerLocation) <= 1; });
            //    }
            //    else
            //    {
            //        creature = Kernel.getInstance().BattleList.GetScreenMonsters().FirstOrDefault();
            //    }

            //    if (creature != null)
            //    {
            //        Kernel.getInstance().Player.Attack((uint)creature.Id);
            //    }
            //}
        }

        public override bool RunOnlyConnected()
        {
            return true;
        }

        public override ModulePriority GetPriority()
        {
            return ModulePriority.LOW;
        }

        public override string GetName()
        {
            return "AutoAttack";
        }  

    }
}
