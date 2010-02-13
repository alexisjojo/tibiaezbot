using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TibiaEzBot.Core.Constants;
using TibiaEzBot.Core.Network;
using TibiaEzBot.Core.Entities;

namespace TibiaEzBot.Core.Modules
{
    public class AutoHeal : Module
    {
        public String MagicWords { get; set; }
        public int MagicMinimumHealth { get; set; }
        public int MagicMinimumMana { get; set; }
        public bool MagicEnable { get; set; }

        public int PotionItemNumber { get; set; }
        public int PotionMinimumHealth { get; set; }
        public bool PotionEnable { get; set; }

        public override ModulePriority GetPriority()
        {
            return ModulePriority.HIGH;
        }

        public override String GetName()
        {
            return "AutoHeal";
        }

        public override bool RunOnlyConnected()
        {
            return true;
        }

        public override void Run()
        {
            ActionControl actionControl = Kernel.GetInstance().ActionControl;

            GlobalVariables.GetUpdateLock().EnterReadLock();

            if (PotionEnable && GlobalVariables.GetPlayerStatus(PlayerStatus.Health) <= PotionMinimumHealth)
            {
                if (actionControl.CanPerformAction(ActionControlType.USE_HEAL_POTION) &&
				    Game.GetInstance().UseItemOnSelf((ushort)PotionItemNumber))
                {
                	actionControl.ActionPerformed(ActionControlType.USE_HEAL_POTION);
                }
            }
            else if (MagicEnable && GlobalVariables.GetPlayerStatus(PlayerStatus.Health) <= MagicMinimumHealth &&
                GlobalVariables.GetPlayerStatus(PlayerStatus.Mana) >= MagicMinimumMana)
            {
                if (actionControl.CanPerformAction(ActionControlType.USE_HEAL_SPELL) &&
				    Game.GetInstance().Say(MagicWords))
                {
                	actionControl.ActionPerformed(ActionControlType.USE_HEAL_SPELL);
                }
            }

            GlobalVariables.GetUpdateLock().ExitReadLock();
        }

    }
}
