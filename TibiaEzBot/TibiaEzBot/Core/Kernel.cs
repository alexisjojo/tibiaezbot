using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Modules;
using System.Threading;
using TibiaEzBot.Core.Entities;
using TibiaEzBot.Core.Network;
using System.Windows.Forms;
using System.IO;

namespace TibiaEzBot.Core
{
    public class Kernel
    {
        private static Kernel instance;
        private Thread mainThread;

        public Client Client { get; set; }
        public Proxy Proxy { get; set; }

        public IList<IModule> Modules { get; set; }
        
        public ActionControl ActionControl { get; set; }
        public Hotkeys Hotkeys { get; set; }

        public int RunInterval { get; set; }

        public AutoHeal AutoHeal { get; set; }
        public AutoManaRestore AutoManaRestore { get; set; }
        public AutoWalk AutoWalk { get; set; }
        public AutoAttack AutoAttack { get; set; }
        public AutoLoot AutoLoot { get; set; }
        public AutoLogin AutoLogin { get; set; }

        public bool Paused { get; set; }

        public WorldProtocol WorldProtocol
        {
            get
            {
                if (Proxy.Protocol is WorldProtocol)
                    return (WorldProtocol)Proxy.Protocol;

                return null;
            }
        }

        public bool Closing { get; set; }

        public static Kernel GetInstance()
        {
            if (instance == null)
            {
                instance = new Kernel();
            }

            return instance;
        }

        public void Initialize()
        {
            if (this.Client != null)
            {
                Logger.LogLevel = 3;
                Logger.Log("Iniciando o kernel.");

                Objects.GetInstance().LoadDat(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Tibia\tibia.dat"));

                Proxy = new Proxy(Client);

                //Iniciando o controlodar de ações
                ActionControl = new ActionControl();
                Hotkeys = new Hotkeys();

                Logger.Log("Iniciando modulos.");
                this.Modules = new List<IModule>();

                AutoHeal = new AutoHeal();
                Modules.Add(AutoHeal);

                AutoManaRestore = new AutoManaRestore();
                Modules.Add(AutoManaRestore);

                AutoWalk = new AutoWalk();
                Modules.Add(AutoWalk);

                AutoAttack = new AutoAttack();
                Modules.Add(AutoAttack);

                AutoLoot = new AutoLoot();
                Modules.Add(AutoLoot);

                AutoLogin = new AutoLogin();
                Modules.Add(AutoLogin);

                Modules = (from module in Modules orderby module.GetPriority() descending select module).ToList();

                foreach (IModule module in Modules)
                {
                    try
                    {
                        module.Initialize();
                    }
                    catch (Exception e) 
                    {
                        Logger.Log(String.Format("Falha ao carregar o modulo {0}. Erro: {1}", module.GetName(), e.ToString()), LogType.ERROR);
                    }
                }

                RunInterval = 500;

                Logger.Log("Ligando o kernel.");
                mainThread = new Thread(new ThreadStart(Run));
                mainThread.Start();
            }
        }

        public void Shutdown()
        {
            Logger.Log("Finalizando o kernel.");
            Closing = true;
            Proxy.Shutdown();
        }

        private void Run()
        {
            while (!Closing)
            {
                foreach (IModule module in Modules)
                {
                    if (!module.RunOnlyOnce() && (GlobalVariables.IsConnected()|| !module.RunOnlyConnected())
                        && (!Paused || module.RunPaused()))
                    {
                        try
                        {
                            module.Run();
                        }
                        catch (Exception e)
                        {
                            //Garante que não vamos travar o lock
                            if (GlobalVariables.GetUpdateLock().IsReadLockHeld)
                                GlobalVariables.GetUpdateLock().ExitReadLock();

                            Logger.Log(String.Format("Falha ao executar o modulo {0}. Erro: {1}", module.GetName(), e.ToString()), LogType.ERROR);
                        }
                    }
                }

                Thread.Sleep(RunInterval);
            }
        }

        private Kernel()
        {
        }
    }
}
