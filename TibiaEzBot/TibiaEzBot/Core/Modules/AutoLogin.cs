using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Modules
{
    public class AutoLogin : Module
    {
        public bool Enable { get; set; }
        public String Account { get; set; }
        public String Password { get; set; }
        public String CharacterName { get; set; }

        private bool connecting;

        public override void Run()
        {
            if (Enable && !connecting && !GlobalVariables.IsConnected() &&
                Kernel.GetInstance().ActionControl.CanPerformAction(ActionControlType.LOGIN))
            {
                var func = new Func<String, String, String, bool>(Kernel.GetInstance().Client.Login.Login);
                func.BeginInvoke(Account, Password, CharacterName, new AsyncCallback(LoginCallback), func);
                connecting = true;
            }
        }

        private void LoginCallback(IAsyncResult ar)
        {
            var func = (Func<String, String, String, bool>)ar.AsyncState;
            if (func.EndInvoke(ar))
            {
                Kernel.GetInstance().ActionControl.ActionPerformed(ActionControlType.LOGIN);
            }

            connecting = false;
        }

        public override bool RunOnlyConnected()
        {
            return false;
        }

        public override string GetName()
        {
            return "AutoLogin";
        }

        public override void Initialize()
        {
        }

        public override ModulePriority GetPriority()
        {
            return ModulePriority.HIGH;
        }

        public override bool RunOnlyOnce()
        {
            return false;
        }
    }
}
