using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core
{
    public enum ActionControlType : int
    {
        USE_HEAL_POTION,
        USE_MANA_POTION,
        USE_HEAL_SPELL,
        USE_ATTACK_SPELL,
        USE_SUPORT_SPELL,
        LOGIN,
    }

    public class ActionControl
    {
        private IDictionary<int, DateTime> actions;

        public ActionControl()
        {
            actions = new Dictionary<int, DateTime>();
        }

        public bool CanPerformAction(ActionControlType actionType)
        {
            int timeInterval = GetInterval(actionType);

            if (actions.ContainsKey((int)actionType))
            {
                if ((DateTime.Now - actions[(int)actionType]).TotalMilliseconds < timeInterval)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetNextActionTime(ActionControlType actionType)
        {
            if (actions.ContainsKey((int)actionType))
            {
                return (int)(DateTime.Now - actions[(int)actionType]).TotalMilliseconds;
            }

            return 0;
        }

        public void ActionPerformed(ActionControlType actionType)
        {
            actions[(int)actionType] = DateTime.Now;
        }
        
        public static int GetInterval(ActionControlType actionType)
        {
            switch (actionType)
            {
                //TODO: Verificar os valores exatos.
                case ActionControlType.USE_ATTACK_SPELL: return 2000;
                case ActionControlType.USE_HEAL_POTION: return 1000;
                case ActionControlType.USE_HEAL_SPELL: return 1000;
                case ActionControlType.USE_MANA_POTION: return 1000;
                case ActionControlType.USE_SUPORT_SPELL: return 1000;
                case ActionControlType.LOGIN: return 10000;
                default: return 0;
            }
        }
        
    }
}
