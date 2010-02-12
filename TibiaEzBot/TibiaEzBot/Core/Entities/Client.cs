using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using System.IO;

namespace TibiaEzBot.Core.Entities
{
    /// <summary>
    /// Represents a single Tibia Client. Contains wrapper methods 
    /// for memory, packet sending, battlelist, and slots. Also contains
    /// any "helper methods" that automate tasks, such as making a rune.
    /// </summary>
    public partial class Client
    {
        #region Variables
        private Process process;
        private IntPtr processHandle;

        // References to commonly used objects
        private MemoryHelper memory;
        private WindowHelper window;
        private LoginHelper login;
        private InputHelper input;


        private String cachedVersion;

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the Tibia client is exited.
        /// </summary>
        public event EventHandler Exited;

        private void process_Exited(object sender, EventArgs e)
        {
            if (Exited != null)
                Exited.Invoke(this, e);
        }

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// "Support" constructor
        /// </summary>
        /// <param name="p">used when necessary to use classes such as packet builder when working clientless</param>
        public Client() { }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="p">the client's process object</param>
        public Client(Process p)
        {
            process = p;
            process.Exited += new EventHandler(process_Exited);
            process.EnableRaisingEvents = true;
            
            // Wait until we can really access the process
            process.WaitForInputIdle();

            while (process.MainWindowHandle == IntPtr.Zero)
            {
                process.Refresh();
                System.Threading.Thread.Sleep(5);
            }

            // Save a copy of the handle so the process doesn't have to be opened
            // every read/write operation
            processHandle = Util.WinApi.OpenProcess(Util.WinApi.PROCESS_ALL_ACCESS, 0, (uint)process.Id);

            memory = new MemoryHelper(this);
            window = new WindowHelper(this);
            login = new LoginHelper(this);
            input = new InputHelper(this);
        }

        /// <summary>
        /// Finalize this client, closing the handle.
        /// Called before the object is garbage collected.
        /// </summary>
        ~Client()
        {
            // Close the process handle
            Util.WinApi.CloseHandle(ProcessHandle);
        }
        #endregion

        #region Properties

        public string PlayerName
        {
            get 
            {
                int playerId = Memory.ReadInt32(Addresses.Player.Id);

                for (uint i = Addresses.BattleList.Start; i < Addresses.BattleList.End; i += Addresses.BattleList.StepCreatures)
                {
                    if (memory.ReadInt32(i + Addresses.Creature.DistanceId) == playerId)
                    {
                        return memory.ReadString(i + Addresses.Creature.DistanceName);
                    }
                }

                return "";
            }
        }

        public string Version
        {
            get
            {
                if (cachedVersion == null)
                {
                    cachedVersion = process.MainModule.FileVersionInfo.FileVersion;
                }
                return cachedVersion;
            }
        }

        public bool HasExited
        {
            get { return process.HasExited; }
        }

        public ushort LastSeenId
        {
            get { return BitConverter.ToUInt16(Memory.ReadBytes(Addresses.Client.SeeId, 2), 0); }
        }


        public Constants.LoginStatus Status
        {
            get { return (Constants.LoginStatus)Memory.ReadByte(Addresses.Client.Status); }
        }

        public bool LoggedIn
        {
            get { return Status == Constants.LoginStatus.LoggedIn; }
        }

        /// <summary>
        /// Gets the dialog pointer
        /// </summary>
        public uint DialogPointer
        {
            get { return Memory.ReadUInt32(Addresses.Client.DialogPointer); }
        }

        /// <summary>
        /// Gets a value indicating if a dialog is opened.
        /// </summary>
        public bool IsDialogOpen
        {
            get { return DialogPointer != 0; }
        }

        /// <summary>
        /// Gets the position of the current opened dialog. Returns null if dialog is not opened.
        /// </summary>
        //public Rectangle DialogPosition
        //{
        //    get
        //    {
        //        return null;
        //        //if (!IsDialogOpen)
        //        //    return new Rectangle();

        //        //return new Rectangle(Memory.ReadInt32(DialogPointer + Addresses.Client.DialogLeft),
        //        //    Memory.ReadInt32(DialogPointer + Addresses.Client.DialogTop),
        //        //    Memory.ReadInt32(DialogPointer + Addresses.Client.DialogWidth),
        //        //    Memory.ReadInt32(DialogPointer + Addresses.Client.DialogHeight));
        //    }
        //}

        /// <summary>
        /// Gets the caption text of the current opened dialog. Returns null if dialog is not opened.
        /// </summary>
        public string DialogCaption
        {
            get
            {
                if (!IsDialogOpen)
                    return "";

                return Memory.ReadString(DialogPointer + Addresses.Client.DialogCaption);
            }
        }

        #endregion

        #region Open Client
        /// <summary>
        /// Open a client at the default path
        /// </summary>
        /// <returns></returns>
        public static Client Open()
        {
            return Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Tibia\Tibia.exe"));
        }

        /// <summary>
        /// Open a client from the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Client Open(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path);
            psi.UseShellExecute = true; // to avoid opening currently running Tibia's
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
            return Open(psi);
        }

        /// <summary>
        /// Open a client from the specified path with arguments
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Client Open(string path, string arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path, arguments);
            psi.UseShellExecute = true; // to avoid opening currently running Tibia's
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
            return Open(psi);
        }

        /// <summary>
        /// Opens a client given a process start info object.
        /// </summary>
        public static Client Open(ProcessStartInfo psi)
        {
            Process p = Process.Start(psi);
            return new Client(p);
        }


        public static Client OpenMC()
        {
            return OpenMC(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Tibia\Tibia.exe"), "");
        }

        /// <summary>
        /// Opens a client with dynamic multi-clienting support
        /// </summary>
        public static Client OpenMC(string path, string arguments)
        {
            Util.WinApi.PROCESS_INFORMATION pi = new Util.WinApi.PROCESS_INFORMATION();
            Util.WinApi.STARTUPINFO si = new Util.WinApi.STARTUPINFO();
            
            if (arguments == null)
                arguments = "";

            Util.WinApi.CreateProcess(path, " " + arguments, IntPtr.Zero, IntPtr.Zero,
                false, Util.WinApi.CREATE_SUSPENDED, IntPtr.Zero,
                System.IO.Path.GetDirectoryName(path), ref si, out pi);

            IntPtr handle = Util.WinApi.OpenProcess(Util.WinApi.PROCESS_ALL_ACCESS, 0, pi.dwProcessId);
            Process p = Process.GetProcessById(Convert.ToInt32(pi.dwProcessId));
            Util.Memory.WriteByte(handle, (long)Addresses.Client.MultiClient, Addresses.Client.MultiClientJMP);
            Util.WinApi.ResumeThread(pi.hThread);
            p.WaitForInputIdle();
            Util.Memory.WriteByte(handle, (long)Addresses.Client.MultiClient, Addresses.Client.MultiClientJNZ);
            Util.WinApi.CloseHandle(handle);
            Util.WinApi.CloseHandle(pi.hProcess);
            Util.WinApi.CloseHandle(pi.hThread);

            return new Client(p);
        }           

        #endregion

        #region Override Functions
        /// <summary>
        /// String identifier for this client.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "[" + Version + "] ";
            if (!LoggedIn)
                s += "Not logged in.";
            else
                s += PlayerName;

            return s;
        }
        #endregion

        #region Client Processes
        /// <summary>
        /// Get a list of all the open clients. Class method.
        /// </summary>
        /// <returns></returns>
        public static List<Client> GetClients()
        {

            return GetClients(null);
        }

        /// <summary>
        /// Get a list of all the open clients of certain version. Class method.
        /// </summary>
        /// <returns></returns>
        public static List<Client> GetClients(string version) {
            return GetClients(version, false);
        }

        /// <summary>
        /// Get a list of all the open clients of certain version. Class method.
        /// </summary>
        /// <returns></returns>
        public static List<Client> GetClients(string version, bool offline) {
            List<Client> clients = new List<Client>();
            Client client = null;

            foreach (Process process in Process.GetProcesses()) {
                StringBuilder classname = new StringBuilder();
                Util.WinApi.GetClassName(process.MainWindowHandle, classname, 12);

                if (classname.ToString().Equals("TibiaClient", StringComparison.CurrentCultureIgnoreCase)) {
                    if (version == null) {
                        client = new Client(process);
                        if (!offline || !client.LoggedIn) 
                            clients.Add(client);
                    }
                    else if (process.MainModule.FileVersionInfo.FileVersion == version) {
                        clients.Add(new Client(process));
                        if (!offline || !client.LoggedIn)
                            clients.Add(client);
                    }
                }
            }
            return clients;
        }

        public void Close()
        {
            if (process != null && !process.HasExited)
                process.Kill();
        }

        #endregion

        #region Client's Objects

        public MemoryHelper Memory
        {
            get { return memory; }
        }

        public WindowHelper Window
        {
            get { return window; }
        }

        public LoginHelper Login
        {
            get { return login; }
        }

        public InputHelper Input
        {
            get { return input; }
        }

        /// <summary>
        /// Get the client's process.
        /// </summary>
        public Process Process
        {
            get { return process; }
        }

        /// <summary>
        /// Get the client's process handle
        /// </summary>
        public IntPtr ProcessHandle
        {
            get { return processHandle; }
        }

        #endregion
        
    }
}
