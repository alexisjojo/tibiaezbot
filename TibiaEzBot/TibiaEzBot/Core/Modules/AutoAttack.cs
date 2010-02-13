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
            if (Enable && !GlobalVariables.IsAttacking() && creature.GetSquare() == SquareColor.Black)
            {
                if (creature.IsMonster())
                {
                    if (!Kernel.GetInstance().AutoLoot.IsLooting ||
                       GlobalVariables.GetPlayerPosition().IsAdjacentTo(creature.GetPosition()))
                    {
                        Game.GetInstance().Attack(creature);
                    }
                }
            }
        }

        public override void Run()
        {
            if (TargetAll && !GlobalVariables.IsAttacking())
            {
                Creature creature = null;

                GlobalVariables.GetUpdateLock().EnterReadLock();

                Position playerPosition = GlobalVariables.GetPlayerPosition();
                //Se estivermos coletando o loot nao podemos atacar qualquer creatura.
                if (Kernel.GetInstance().AutoLoot.IsLooting)
                {
                    creature = (from c in Creatures.GetInstance().GetScreenMonsters()
                                where c.GetPosition().IsAdjacentTo(playerPosition)
                                select c).FirstOrDefault();
                }
                else
                {
                    creature = (from c in Creatures.GetInstance().GetScreenMonsters()
                                where c.IsReachable() && Creatures.GetInstance().
                                    GetFloorPlayers().FirstOrDefault(delegate(Creature cr)
                                    {
                                        return cr.GetPosition().IsAdjacentTo(c.GetPosition());
                                    }) == null
                                orderby c.GetPosition().DistanceTo(playerPosition)
                                select c).FirstOrDefault();
                }

                GlobalVariables.GetUpdateLock().ExitReadLock();

                if (creature != null)
                {
                    Game.GetInstance().Attack(creature);
                }
            }
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
