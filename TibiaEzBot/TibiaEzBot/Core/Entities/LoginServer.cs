using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class LoginServer
    {
        public string Server { get; set; }
        public short Port { get; set; }

        public LoginServer()
            : this("") { }

        public LoginServer(string server)
            : this(server, 7171) { }

        public LoginServer(string server, short port)
        {
            Server = server;
            Port = port;
        }

        public override string ToString()
        {
            return Server + ":" + Port;
        }
    }
}
