using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Modules
{
    public interface IModule
    {
        ModulePriority GetPriority();
        bool RunOnlyOnce();
        bool RunOnlyConnected();
        bool RunPaused();
        void Initialize();
        void Run();
        String GetName();
    }
}
