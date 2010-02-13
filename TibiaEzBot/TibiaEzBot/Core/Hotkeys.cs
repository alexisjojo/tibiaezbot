using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TibiaEzBot.Core.Util;

namespace TibiaEzBot.Core
{
    public class Hotkeys
    {

        public Hotkeys()
        {
            KeyboardHook.Enable();
            KeyboardHook.Add(Keys.Pause, new KeyboardHook.KeyPressed(pauseKeyPressed));
        }

        private bool isTibiaWindow()
        {
            return WinApi.GetForegroundWindow().Equals(Kernel.GetInstance().Client.Window.Handle);
        }

        private bool pauseKeyPressed()
        {
            if (isTibiaWindow())
            {
                if (Kernel.GetInstance().Paused)
                {
                    Kernel.GetInstance().Paused = false;
                    Logger.Log("Kernel resumed.");
                }
                else
                {
                    Kernel.GetInstance().Paused = true;
                    Game.GetInstance().CancelMove();
                    Logger.Log("Kernel paused.");
                }

                return false;
            }

            return true;
        }


    }
}
