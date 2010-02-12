using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Network
{
    public enum ProtocolType { None, Login, World }

    public class Protocol
    {
        protected ProtocolType protocolType;

        public virtual bool ParseMessageFromServer(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            return false;
        }

        public virtual bool ParseMessageFromClient(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            return false;
        }

        public ProtocolType ProtocolType { get { return protocolType; } }
    }
}
