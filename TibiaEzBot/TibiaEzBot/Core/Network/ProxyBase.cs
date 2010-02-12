using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Windows.Forms;

namespace TibiaEzBot.Core.Network
{
    public abstract class ProxyBase
    {
        protected Protocol protocol;

        protected bool ParsePacketFromServer(NetworkMessage msg, NetworkMessage outMsg)
        {
            if (protocol != null)
                return protocol.ParseMessageFromServer(msg, outMsg);

            return false;
        }

        #region ParsePacketFromClient
        protected bool ParsePacketFromClient(NetworkMessage msg, NetworkMessage outMsg)
        {
            if (protocol != null)
                return protocol.ParseMessageFromClient(msg, outMsg);

            return false;
        }
        #endregion

        #region Port Checking
        /// <summary>
        /// Check if a port is open on localhost
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckPort(ushort port)
        {
            try
            {
                TcpListener tcpScan = new TcpListener(IPAddress.Any, port);
                tcpScan.Start();
                tcpScan.Stop();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get the first free port on localhost starting at the default 7171
        /// </summary>
        /// <returns></returns>
        public static ushort GetFreePort()
        {
            return GetFreePort(7171);
        }

        /// <summary>
        /// Get the first free port on localhost beginning at start
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static ushort GetFreePort(ushort start)
        {
            while (!CheckPort(start))
            {
                start++;
            }

            return start;
        }
        #endregion

        #region Misc

        public static string GetDefaultLocalIp()
        {
            string localIp = null;
            IPHostEntry hostEntry = Dns.GetHostEntry((Dns.GetHostName()));
            foreach (IPAddress ipa in hostEntry.AddressList)
            {
                // Find the first IPv4 address
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ipa.ToString();
                    break;
                }
            }
            return localIp;
        }

        private object debugLock = new object();
        protected void WriteDebug(string msg)
        {
            //try
            //{
            //    lock (debugLock)
            //    {
            //        System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(Application.StartupPath, "proxy_log.txt"), true);
            //        sw.WriteLine(System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString() + " " + msg + "\nLast received packet types: " + GetLastReceivedPacketTypesString());
            //        sw.Close();
            //    }
            //}
            //catch
            //{
            //}
        }

        /*protected FixedCollector<byte> lastReceivedPacketTypes = new FixedCollector<byte>(10);

        public string GetLastReceivedPacketTypesString()
        {
            return String.Join(", ", lastReceivedPacketTypes.Select(delegate(byte b)
            {
                if (Enum.IsDefined(typeof(IncomingPacketType), b))
                {
                    return ((IncomingPacketType)b).ToString();
                }
                else
                {
                    return b.ToString("X2");
                }
            }).ToArray());
        }*/

        #endregion

    }
}
