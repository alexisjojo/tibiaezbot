using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Constants;
using TibiaEzBot.Core.Entities;

namespace TibiaEzBot.Core.Modules
{
    public class AutoManaRestore : Module
    {
        public int MinimumMana { get; set; }
        public bool Enable { get; set; }
        public int ItemId { get; set; }

        public override void Run()
        {
            ActionControl actionControl = Kernel.GetInstance().ActionControl;

            if (Enable && GlobalVariables.GetPlayerStatus(PlayerStatus.Mana) <= MinimumMana)
            {
                if (actionControl.CanPerformAction(ActionControlType.USE_MANA_POTION))
                {
                    if (Kernel.GetInstance().WorldProtocol != null)
                    {
                        Kernel.GetInstance().WorldProtocol.SendUseBattleWindow(new Position(0xFFFF, 0, 0),
                            (ushort)ItemId, 0, GlobalVariables.GetPlayerId());
                        actionControl.ActionPerformed(ActionControlType.USE_MANA_POTION);
                    }
                }
            }
        }

        public override bool RunOnlyConnected()
        {
            return true;
        }

        public override ModulePriority GetPriority()
        {
            return ModulePriority.HIGH;
        }

        public override string GetName()
        {
            return "AutoManaRestore";
        }
    }
}
