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
				if(creature.IsMonster())
				{
					if(!Kernel.GetInstance().AutoLoot.IsLooting ||
					   GlobalVariables.GetPlayerPosition().IsAdjacentTo(creature.GetPosition()))
					{
						Game.GetInstance().Attack(creature);
					}
				}
			}
		}

        //public override void Run()
        //{
        //    if (TargetAll && !GlobalVariables.IsAttacking())
        //    {
        //        Creature creature = null;

        //        //Se estivermos coletando o loot nao podemos atacar qualquer creatura.
        //        if (Kernel.getInstance().AutoLoot.IsLooting)
        //        {
        //            Position playerPosition = GlobalVariables.GetPlayerPosition();
					
        //            Creatures.GetInstance().CreaturesLock.EnterReadLock();
					
        //            creature = Kernel.getInstance().BattleList.GetScreenMonsters().FirstOrDefault(
        //                delegate(TibiaEzBot.Core.Entities.Creature cr) { return cr.Location.DistanceTo(playerLocation) <= 1; });
					
        //            Creatures.GetInstance().CreaturesLock.ExitReadLock();
        //        }
        //        else
        //        {
        //            creature = Kernel.getInstance().BattleList.GetScreenMonsters().FirstOrDefault();
        //        }

        //        if (creature != null)
        //        {
        //            Kernel.getInstance().Player.Attack((uint)creature.Id);
        //        }
        //    }
        //}

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
