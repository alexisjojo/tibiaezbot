using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Network
{
    public class LoginProtocol : Protocol
    {
        public LoginProtocol()
        {
            protocolType = ProtocolType.Login;
        }

        public override bool ParseMessageFromClient(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            return base.ParseMessageFromClient(incomingMsg, outgoingMsg);
        }

        public override bool ParseMessageFromServer(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            return base.ParseMessageFromServer(incomingMsg, outgoingMsg);
        }
    }
}
