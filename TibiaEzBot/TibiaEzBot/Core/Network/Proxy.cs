using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using TibiaEzBot.Core.Entities;
using TibiaEzBot.Core.Util;

namespace TibiaEzBot.Core.Network
{
    public class Proxy : ProxyBase
    {
        #region Vars

        private Client client;

        private static byte[] localHostBytes = new byte[] { 127, 0, 0, 1 };
        private static Random random = new Random();

        private NetworkMessage clientRecvMsg, serverRecvMsg;
        private NetworkMessage clientSendMsg, serverSendMsg;

        private bool isOtServer;

        private LoginServer[] loginServers;
        private CharacterLoginInfo[] charList;
        private ushort loginServerPort, worldServerPort;

        private uint[] xteaKey;

        private int selectedLoginServer = 0;

        private TcpListener loginClientTcp, worldClientTcp;
        private Socket clientSocket;
        private NetworkStream clientStream;
        private Queue<byte[]> clientSendQueue;
        private object clientSendQueueLock;
        private bool clientWriting;
        private Thread clientSendThread;
        private object clientSendThreadLock;

        private bool accepting = false;
        private object restartLock;

        private TcpClient serverTcp;
        private NetworkStream serverStream;
        private Queue<byte[]> serverSendQueue;
        private object serverSendQueueLock;
        private bool serverWriting;
        private Thread serverSendThread;
        private object serverSendThreadLock;

        private DateTime lastInteraction;

        #endregion

        #region Properties

        public Protocol Protocol { get { return protocol; } }

        public uint[] XteaKey
        {
            get { return xteaKey; }
        }

        public bool AllowIncomingModification { get; set; }
        public bool AllowOutgoingModification { get; set; }

        #endregion

        #region Public Functions

        public void Shutdown()
        {
            if (loginServers != null)
                client.Login.Servers = loginServers;
            if (charList != null)
                client.Login.SetCharListServer(charList);

            client.Login.RSA = Constants.RSAKey.RealTibia;
            Stop();
        }

        public void CheckState()
        {
            if ((DateTime.Now - lastInteraction).TotalSeconds >= 30)
            {
                Restart();
            }
        }

        public void SendToServer(byte[] data)
        {
            lock (serverSendQueueLock)
            {
                serverSendQueue.Enqueue(data);
            }

            lock (serverSendThreadLock)
            {
                if (!serverWriting)
                {
                    serverWriting = true;
                    serverSendThread = new Thread(new ThreadStart(ServerSend));
                    serverSendThread.Start();
                }
            }
        }

        public void SendToClient(byte[] data)
        {
            lock (clientSendQueueLock)
            {
                clientSendQueue.Enqueue(data);
            }

            lock (clientSendThreadLock)
            {
                if (!clientWriting)
                {
                    clientWriting = true;
                    clientSendThread = new Thread(new ThreadStart(ClientSend));
                    clientSendThread.Start();
                }
            }
        }

        #endregion

        #region Constructor

        public Proxy(Client client)
        {
            try
            {
                this.client = client;
                Initialize();
            }
            catch (Exception e)
            {
                Logger.Log("Falha ao inciar o proxy. Erro: " + e.ToString(), LogType.FATAL);
            }
        }

        private void Initialize()
        {
            Logger.Log("Iniciando o proxy.");

            serverRecvMsg = new NetworkMessage();
            clientRecvMsg = new NetworkMessage();
            clientSendMsg = new NetworkMessage();
            serverSendMsg = new NetworkMessage();

            clientSendQueue = new Queue<byte[]>();
            serverSendQueue = new Queue<byte[]>();

            clientSendQueueLock = new object();
            serverSendQueueLock = new object();
            restartLock = new object();

            clientSendThreadLock = new object();
            serverSendThreadLock = new object();

            xteaKey = new uint[4];

            loginServers = client.Login.Servers;

            if (loginServers[0].Server == "localhost" && !isOtServer)
                loginServers = client.Login.DefaultServers;

            if (loginServerPort == 0)
                loginServerPort = GetFreePort();

            worldServerPort = (ushort)(loginServerPort + 1);

            client.Login.SetServer("localhost", (short)loginServerPort);


            client.Login.RSA = Constants.RSAKey.OpenTibia;


            if (client.Login.CharListCount != 0)
            {
                charList = client.Login.CharacterList;
                client.Login.SetCharListServer(localHostBytes, loginServerPort);
            }

            StartListenFromClient();
        }

        #endregion

        #region Main Flow

        private void StartListenFromClient()
        {
            try
            {
                accepting = true;
                protocol = null;
                clientSendQueue.Clear();
                serverSendQueue.Clear();

                loginClientTcp = new TcpListener(IPAddress.Any, loginServerPort);
                loginClientTcp.Start();
                loginClientTcp.BeginAcceptSocket(new AsyncCallback(ListenClientCallBack), 0);

                worldClientTcp = new TcpListener(IPAddress.Any, worldServerPort);
                worldClientTcp.Start();
                worldClientTcp.BeginAcceptSocket(new AsyncCallback(ListenClientCallBack), 1);
            }
            catch (Exception ex)
            {
                Logger.Log("Falha ao iniciar a escuta pela cliente. Error: " + ex.ToString(), LogType.FATAL);
            }
        }

        private void ListenClientCallBack(IAsyncResult ar)
        {
            try
            {
                if (!accepting)
                    return;

                accepting = false;
                clientSocket = loginClientTcp.EndAcceptSocket(ar);

                loginClientTcp.Stop();
                worldClientTcp.Stop();


                int type = (int)ar.AsyncState;

                if (type == 1)
                {
                    serverTcp = new TcpClient(BitConverter.GetBytes(charList[client.Login.SelectedChar].WorldIP).ToIPString(), charList[client.Login.SelectedChar].WorldPort);
                    serverStream = serverTcp.GetStream();
                    serverStream.BeginRead(serverRecvMsg.GetBuffer(), 0, 2, new AsyncCallback(ServerReadCallBack), null);
                }


                clientStream = new NetworkStream(clientSocket);
                clientStream.BeginRead(clientRecvMsg.GetBuffer(), 0, 2, new AsyncCallback(ClientReadCallBack), null);
            }
            catch (ObjectDisposedException) { /*We don't have to log this exception. */ }
            catch (Exception ex)
            {
                Logger.Log("Falha ao aceitar a conexão do cliente. Erro: " + ex.ToString(), LogType.ERROR);
                Restart();
            }
        }

        private void ClientReadCallBack(IAsyncResult ar)
        {
            byte[] clientData = null;

            try
            {
                int read = clientStream.EndRead(ar);

                if (read < 2)
                {
                    Restart();
                    return;
                }

                int pSize = (int)BitConverter.ToUInt16(clientRecvMsg.GetBuffer(), 0) + 2;

                while (read < pSize)
                {
                    if (clientStream.CanRead)
                        read += clientStream.Read(clientRecvMsg.GetBuffer(), read, pSize - read);
                    else
                    {
                        throw new Exception("Connection broken.");
                    }
                }

                clientRecvMsg.Length = pSize;

                if (protocol == null)
                {
                    ParseFirstClientMsg();
                }
                else if (protocol.ProtocolType == ProtocolType.World)
                {
                    clientData = clientRecvMsg.Data;

                    if (clientRecvMsg.CheckAdler32() && clientRecvMsg.XteaDecrypt(xteaKey))
                    {
                        clientRecvMsg.Position = clientRecvMsg.GetPacketHeaderSize();
                        int msgLength = (int)clientRecvMsg.GetUInt16() + 8;
                        serverSendMsg.Reset();

                        byte lastPacket = clientRecvMsg.PeekByte();
                        bool sendModified = true;

                        if (!ParsePacketFromClient(clientRecvMsg, serverSendMsg))
                        {
                            //unknown packet
                            //Logger.Log("Falha ao efetuar o parse. Type: " + lastPacket,  LogType.ERROR);
                            sendModified = false;
                        }

                        if (sendModified && serverSendMsg.Length > serverSendMsg.GetPacketHeaderSize() + 2)
                        {
                            serverSendMsg.InsetLogicalPacketHeader();
                            serverSendMsg.PrepareToSend(xteaKey);
                            SendToServer(serverSendMsg.Data);
                        }
                        else
                        {
                            SendToServer(clientData);
                        }
                    }

                    clientStream.BeginRead(clientRecvMsg.GetBuffer(), 0, 2, new AsyncCallback(ClientReadCallBack), null);
                }
            }
            catch (ObjectDisposedException) { /*We don't have to log this exception. */ }
            catch (System.IO.IOException ex)
            {
                Logger.Log("Falha ao receber dados cliente. Erro: " + ex.ToString(), LogType.ERROR);
            }
            catch (Exception ex)
            {
                Logger.Log("Falha ao receber dados do cliente. Erro: " + ex.ToString(), LogType.ERROR);
            }
        }

        private void ParseFirstClientMsg()
        {
            try
            {
                clientRecvMsg.Position = clientRecvMsg.GetPacketHeaderSize();

                byte protocolId = clientRecvMsg.GetByte();
                int position;

                switch (protocolId)
                {
                    case 0x01:

                        protocol = new LoginProtocol();
                        clientRecvMsg.GetUInt16();
                        ushort clientVersion = clientRecvMsg.GetUInt16();

                        clientRecvMsg.GetUInt32();
                        clientRecvMsg.GetUInt32();
                        clientRecvMsg.GetUInt32();

                        position = clientRecvMsg.Position;

                        clientRecvMsg.RsaOTDecrypt();

                        if (clientRecvMsg.GetByte() != 0)
                        {
                            Restart();
                            return;
                        }

                        xteaKey[0] = clientRecvMsg.GetUInt32();
                        xteaKey[1] = clientRecvMsg.GetUInt32();
                        xteaKey[2] = clientRecvMsg.GetUInt32();
                        xteaKey[3] = clientRecvMsg.GetUInt32();

                        NetworkMessage.XteaKey = xteaKey;


                        clientRecvMsg.GetString(); // account name
                        clientRecvMsg.GetString(); // password

                        if (isOtServer)
                            clientRecvMsg.RsaOTEncrypt(position);
                        else
                            clientRecvMsg.RsaCipEncrypt(position);


                        clientRecvMsg.AddAdler32();
                        clientRecvMsg.InsertPacketHeader();

                        serverTcp = new TcpClient(loginServers[selectedLoginServer].Server, loginServers[selectedLoginServer].Port);
                        serverStream = serverTcp.GetStream();
                        serverStream.Write(clientRecvMsg.GetBuffer(), 0, clientRecvMsg.Length);
                        serverStream.BeginRead(serverRecvMsg.GetBuffer(), 0, 2, new AsyncCallback(ServerReadCallBack), null);
                        break;

                    case 0x0A:

                        protocol = new WorldProtocol();

                        clientRecvMsg.GetUInt16();
                        clientRecvMsg.GetUInt16();

                        position = clientRecvMsg.Position;

                        clientRecvMsg.RsaOTDecrypt();

                        if (clientRecvMsg.GetByte() != 0)
                        {
                            Restart();
                            return;
                        }

                        xteaKey[0] = clientRecvMsg.GetUInt32();
                        xteaKey[1] = clientRecvMsg.GetUInt32();
                        xteaKey[2] = clientRecvMsg.GetUInt32();
                        xteaKey[3] = clientRecvMsg.GetUInt32();

                        NetworkMessage.XteaKey = xteaKey;

                        clientRecvMsg.GetByte(); // GM mode

                        clientRecvMsg.GetString(); // account name
                        string characterName = clientRecvMsg.GetString();
                        clientRecvMsg.GetString(); // password

                        if (isOtServer)
                            clientRecvMsg.RsaOTEncrypt(position);
                        else
                            clientRecvMsg.RsaCipEncrypt(position);

                        clientRecvMsg.AddAdler32();
                        clientRecvMsg.InsertPacketHeader();

                        int index = GetSelectedIndex(characterName);

                        serverStream.Write(clientRecvMsg.GetBuffer(), 0, clientRecvMsg.Length);
                        serverStream.BeginRead(serverRecvMsg.GetBuffer(), 0, 2, new AsyncCallback(ServerReadCallBack), null);
                        clientStream.BeginRead(clientRecvMsg.GetBuffer(), 0, 2, new AsyncCallback(ClientReadCallBack), null);

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Falha ao efetuar o parse da primeira mensagem do cliente. Erro: " + ex.ToString(), LogType.ERROR);
                Restart();
            }
        }

        private void ServerReadCallBack(IAsyncResult ar)
        {
            byte[] serverData = null;

            try
            {
                int read = serverStream.EndRead(ar);

                if (read < 2)
                {
                    Restart();
                    return;
                }

                lastInteraction = DateTime.Now;
                int pSize = (int)BitConverter.ToUInt16(serverRecvMsg.GetBuffer(), 0) + 2;

                while (read < pSize)
                {
                    if (serverStream.CanRead)
                        read += serverStream.Read(serverRecvMsg.GetBuffer(), read, pSize - read);
                    else
                    {
                        throw new Exception("Connection broken.");
                    }
                }

                serverRecvMsg.Length = pSize;

                if (protocol == null)
                {
                    SendToClient(serverRecvMsg.Data);
                }
                else if (protocol.ProtocolType == ProtocolType.World)
                {
                    serverData = serverRecvMsg.Data;
                    if (serverRecvMsg.CheckAdler32() && serverRecvMsg.XteaDecrypt(xteaKey))
                    {
                        serverRecvMsg.Position = serverRecvMsg.GetPacketHeaderSize();
                        int msgSize = (int)serverRecvMsg.GetUInt16() + serverRecvMsg.GetPacketHeaderSize() + 2;

                        bool sendModified = true;
                        clientSendMsg.Reset();


                        //DateTime t = DateTime.UtcNow;

                        GlobalVariables.GetUpdateLock().EnterWriteLock();

                        try
                        {
                            while (serverRecvMsg.Position < msgSize)
                            {
                                byte type = serverRecvMsg.PeekByte();
                                lastReceivedPacketTypes.Push(type);

                                if (!ParsePacketFromServer(serverRecvMsg, clientSendMsg))
                                {
                                    Logger.Log("Falha ao efetuar o parse. Ultimos pacotes recebidos: " + GetLastReceivedPacketTypesString(), LogType.ERROR);

                                    //Verifica se já tivamos algum erro de parse, caso sim
                                    //vamos parar por aqui.
                                    if (!sendModified)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Logger.Log("Tentando efetuar o parse novamente.");
                                        sendModified = false;
                                    }
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Log("Erro ao efetuar o parse das mensagens do servidor. Erro: " + e.ToString());
                            sendModified = false;
                        }

                        GlobalVariables.GetUpdateLock().ExitWriteLock();

                        //Console.WriteLine("Tempo para processar a msg: " + (DateTime.UtcNow - t).TotalMilliseconds);

                        if (sendModified && clientSendMsg.Length > clientSendMsg.GetPacketHeaderSize() + 2)
                        {
                            clientSendMsg.InsetLogicalPacketHeader();
                            clientSendMsg.PrepareToSend(xteaKey);

                            SendToClient(clientSendMsg.Data);
                        }
                        else
                        {
                            SendToClient(serverData);
                        }
                    }

                    serverStream.BeginRead(serverRecvMsg.GetBuffer(), 0, 2, new AsyncCallback(ServerReadCallBack), null);
                }
                else if (protocol.ProtocolType == ProtocolType.Login)
                {
                    ParseCharacterList();
                }
            }
            catch (System.IO.IOException) { }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Logger.Log("Falha ao receber dados do servidor. Erro: " + ex.ToString(), LogType.ERROR);
                if (serverData != null)
                    SendToClient(serverData);
            }
        }

        private void ParseCharacterList()
        {
            try
            {
                if (serverRecvMsg.CheckAdler32() && serverRecvMsg.PrepareToRead())
                {
                    int msgSize = serverRecvMsg.GetUInt16() + 6;

                    while (serverRecvMsg.Position < msgSize)
                    {
                        byte cmd = serverRecvMsg.GetByte();

                        switch (cmd)
                        {
                            case 0x0A: //Error message
                                serverRecvMsg.GetString();
                                break;
                            case 0x0B: //For your information
                                serverRecvMsg.GetString();
                                break;
                            case 0x14: //MOTD
                                serverRecvMsg.GetString();
                                break;
                            case 0x1E: //Patching exe/dat/spr messages
                            case 0x1F:
                            case 0x20:
                                //DisconnectClient(0x0A, "A new client is avalible, please download it first!");
                                break;
                            case 0x28: //Select other login server
                                selectedLoginServer = random.Next(0, loginServers.Length - 1);
                                break;
                            case 0x64: //character list
                                int nChar = (int)serverRecvMsg.GetByte();
                                charList = new CharacterLoginInfo[nChar];

                                for (int i = 0; i < nChar; i++)
                                {
                                    charList[i].CharName = serverRecvMsg.GetString();
                                    charList[i].WorldName = serverRecvMsg.GetString();
                                    charList[i].WorldIP = serverRecvMsg.PeekUInt32();
                                    serverRecvMsg.AddBytes(localHostBytes);
                                    charList[i].WorldPort = serverRecvMsg.PeekUInt16();
                                    serverRecvMsg.AddUInt16(worldServerPort);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    serverRecvMsg.PrepareToSend();
                    clientStream.Write(serverRecvMsg.GetBuffer(), 0, serverRecvMsg.Length);

                    // Give the client time to process the message
                    System.Threading.Thread.Sleep(500);

                    Restart();
                    return;
                }
                else
                    Restart();
            }
            catch (Exception ex)
            {
                Logger.Log("Falha ao efetuar o parse da lista de personagens. Erro: " + ex.ToString(), LogType.ERROR);
                Restart();
            }
        }

        #endregion

        #region Control

        private void Restart()
        {
            lock (restartLock)
            {
                if (accepting)
                    return;

                if (GlobalVariables.IsConnected())
                    GlobalVariables.SetConnected(false);

                Logger.Log("Reiniciando o proxy.");
                Stop();
                StartListenFromClient();
            }
        }

        private void Stop()
        {
            try
            {
                if (loginClientTcp != null)
                    loginClientTcp.Stop();

                if (clientSocket != null)
                    clientSocket.Close();

                if (clientStream != null)
                    clientStream.Close();

                if (serverTcp != null)
                    serverTcp.Close();
            }
            catch (Exception ex)
            {
                Logger.Log("Falha ao para o proxy. Erro: " + ex.ToString(), LogType.ERROR);
            }
        }

        #endregion

        #region Send

        private void ServerSend()
        {
            try
            {
                byte[] packet = null;

                lock (serverSendQueueLock)
                {
                    if (serverSendQueue.Count > 0)
                        packet = serverSendQueue.Dequeue();
                    else
                    {
                        serverWriting = false;
                        return;
                    }
                }

                if (packet == null)
                {
                    serverWriting = false;
                    throw new Exception("Null Packet.");
                }

                serverStream.BeginWrite(packet, 0, packet.Length, new AsyncCallback(ServerSendCallBack), null);
            }
            catch (Exception ex)
            {
                Logger.Log("Falha ao enviar dados para o servidor. Erro: " + ex.ToString(), LogType.ERROR);
            }
        }

        private void ServerSendCallBack(IAsyncResult ar)
        {
            try
            {
                serverStream.EndWrite(ar);

                bool runAgain = false;

                lock (serverSendQueueLock)
                {
                    if (serverSendQueue.Count > 0)
                        runAgain = true;
                }

                if (runAgain)
                    ServerSend();
                else
                    serverWriting = false;
            }
            catch (ObjectDisposedException) { Restart(); }
            catch (System.IO.IOException) { Restart(); }
            catch (Exception ex)
            {
                Logger.Log("Falha ao enviar dados para o servidor. Erro: " + ex.ToString(), LogType.ERROR);
                Restart();
            }
        }

        private void ClientSend()
        {
            try
            {
                byte[] packet = null;

                lock (clientSendQueueLock)
                {
                    if (clientSendQueue.Count > 0)
                        packet = clientSendQueue.Dequeue();
                    else
                    {
                        clientWriting = false;
                        return;
                    }
                }

                if (packet == null)
                {
                    clientWriting = false;
                    throw new Exception("Null Packet.");
                }

                clientStream.BeginWrite(packet, 0, packet.Length, new AsyncCallback(ClientSendCallBack), null);
            }
            catch (System.IO.IOException) { }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Logger.Log("Falha ao enviar dados para o client. Erro: " + ex.ToString(), LogType.ERROR);
            }
        }

        private void ClientSendCallBack(IAsyncResult ar)
        {
            try
            {
                clientStream.EndWrite(ar);

                bool runAgain = false;

                lock (clientSendQueueLock)
                {
                    if (clientSendQueue.Count > 0)
                        runAgain = true;
                }

                if (runAgain)
                    ClientSend();
                else
                    clientWriting = false;
            }
            catch (ObjectDisposedException) { Restart(); }
            catch (System.IO.IOException) { Restart(); }
            catch (Exception ex)
            {
                Logger.Log("Falha ao enviar dados para o cliente. Erro: " + ex.ToString(), LogType.ERROR);
                Restart();
            }
        }

        #endregion

        #region Other Functions

        private int GetSelectedIndex(string characterName)
        {
            for (int i = 0; i < charList.Length; i++)
            {
                if (charList[i].CharName == characterName)
                    return i;
            }

            return -1;
        }

        #endregion

        #region Debug

        protected FixedCollector<byte> lastReceivedPacketTypes = new FixedCollector<byte>(10);

        public string GetLastReceivedPacketTypesString()
        {
            return String.Join(", ", lastReceivedPacketTypes.Select(delegate(byte b)
            {
                return b.ToString("X2");
            }).ToArray());
        }

        #endregion
    }
}
