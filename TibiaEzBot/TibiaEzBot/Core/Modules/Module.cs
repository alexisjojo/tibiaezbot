using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Modules
{
	public enum ModulePriority : int
    {
        LOW = 0,
        NORMAL = 1,
        HIGH = 2
    }

    public abstract class Module : IModule
    {
        #region IModule Members

        public virtual ModulePriority GetPriority()
        {
            return ModulePriority.NORMAL;
        }

        public virtual bool RunOnlyOnce()
        {
            return false;
        }

        public virtual bool RunOnlyConnected()
        {
            return false;
        }

        public virtual bool RunPaused() { return false; }

        public virtual void Initialize() { }
        public virtual void Run() { }

        public virtual String GetName()
        {
            return "Module";
        }

        #endregion
    }
}
