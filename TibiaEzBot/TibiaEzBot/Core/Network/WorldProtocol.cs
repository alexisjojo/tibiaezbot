using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Entities;
using TibiaEzBot.Core.Constants;
using System.Threading;

namespace TibiaEzBot.Core.Network
{
    public class WorldProtocol : Protocol
    {
        private ushort skipTiles;
        private NetworkMessage serverSendMsg = new NetworkMessage(4096);

        public WorldProtocol()
        {
            protocolType = ProtocolType.World;
        }

        #region Recv Client

        public override bool ParseMessageFromClient(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            byte command = incomingMsg.GetByte();
            outgoingMsg.AddByte(command);

            switch (command)
            {
                //Logout = 0x14,
                //ItemMove = 0x78,
                //ShopBuy = 0x7A,
                //ShopSell = 0x7B,
                //ShopClose = 0x7C,
                //ItemUse = 0x82,
                //ItemUseOn = 0x83,
                //ItemRotate = 0x85,
                //LookAt = 0x8C,
                //PlayerSpeech = 0x96,
                //ChannelList = 0x97,
                //ChannelOpen = 0x98,
                //ChannelClose = 0x99,
                //Attack = 0xA1,
                //Follow = 0xA2,
                //CancelMove = 0xBE,
                //ItemUseBattlelist = 0x84,
                //ContainerClose = 0x87,
                //ContainerOpenParent = 0x88,
                //TurnUp = 0x6F,
                //TurnRight = 0x70,
                //TurnDown = 0x71,
                //TurnLeft = 0x72,
                //AutoWalk = 0x64,
                //AutoWalkCancel = 0x69,
                case 0x65: //MoveUp = 0x65,
                    return parseMoveUp(incomingMsg, outgoingMsg);
                //MoveRight = 0x66,
                //MoveDown = 0x67,
                //MoveLeft = 0x68,
                //MoveUpRight = 0x6A,
                //MoveDownRight = 0x6B,
                //MoveDownLeft = 0x6C,
                //MoveUpLeft = 0x6D,
                //VipAdd = 0xDC,
                //VipRemove = 0xDD,
                //SetOutfit = 0xD3,
                //Ping = 0x1E,
                //FightModes = 0xA0,
                //ContainerUpdate = 0xCA,
                //TileUpdate = 0xC9,
                //PrivateChannelOpen = 0x9A,
                //NpcChannelClose = 0x9E,

            }

            return false;
        }

        private bool parseMoveUp(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            return true;
        }

        #endregion

        #region Recv Server
        public override bool ParseMessageFromServer(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            byte command = incomingMsg.GetByte();
            outgoingMsg.AddByte(command);

            switch (command)
            {
                case 0x0A:
                    return parseSelfApper(incomingMsg, outgoingMsg);
                case 0x0B: //GMAction = 0x0B,
                    return parseGMAction(incomingMsg, outgoingMsg);
                case 0x14: //ErrorMessage = 0x14,
                    return parseErrorMessage(incomingMsg, outgoingMsg);
                case 0x15: //FyiMessage = 0x15,
                    return parseFyiMessage(incomingMsg, outgoingMsg);
                case 0x16: //WaitingList = 0x16,
                    return parseWaitingList(incomingMsg, outgoingMsg);
                case 0x1E: //Ping = 0x1E,
                    return parsePing(incomingMsg, outgoingMsg);
                case 0x28: //Death = 0x28,
                    return parseDeath(incomingMsg, outgoingMsg);
                case 0x32: //CanReportBugs = 0x32,
                    return parseCanReportBugs(incomingMsg, outgoingMsg);
                case 0x64: //MapDescription = 0x64,
                    return parseMapDescription(incomingMsg, outgoingMsg);
                case 0x65: //MoveNorth = 0x65,
                    return parseMoveNorth(incomingMsg, outgoingMsg);
                case 0x66: //MoveEast = 0x66,
                    return parseMoveEast(incomingMsg, outgoingMsg);
                case 0x67: //MoveSouth = 0x67,
                    return parseMoveSouth(incomingMsg, outgoingMsg);
                case 0x68: //MoveWest = 0x68,
                    return parseMoveWest(incomingMsg, outgoingMsg);
                case 0x69: //TileUpdate = 0x69,
                    return parseUpdateTile(incomingMsg, outgoingMsg);
                case 0x6A: //TileAddThing = 0x6A,
                    return parseTileAddThing(incomingMsg, outgoingMsg);
                case 0x6B: //TileTransformThing = 0x6B,
                    return parseTileTransformThing(incomingMsg, outgoingMsg);
                case 0x6C: //TileRemoveThing = 0x6C,
                    return parseTileRemoveThing(incomingMsg, outgoingMsg);
                case 0x6D: //CreatureMove = 0x6D,
                    return parseCreatureMove(incomingMsg, outgoingMsg);
                case 0x6E: //ContainerOpen = 0x6E,
                    return parseOpenContainer(incomingMsg, outgoingMsg);
                case 0x6F: //ContainerClose = 0x6F,
                    return parseCloseContainer(incomingMsg, outgoingMsg);
                case 0x70: //ContainerAddItem = 0x70,
                    return parseContainerAddItem(incomingMsg, outgoingMsg);
                case 0x71: //ContainerUpdateItem = 0x71,
                    return parseContainerUpdateItem(incomingMsg, outgoingMsg);
                case 0x72: //ContainerRemoveItem = 0x72,
                    return parseContainerRemoveItem(incomingMsg, outgoingMsg);
                case 0x78: //InventorySetSlot = 0x78,
                    return parseInventorySetSlot(incomingMsg, outgoingMsg);
                case 0x79: //InventoryResetSlot = 0x79,
                    return parseInventoryResetSlot(incomingMsg, outgoingMsg);
                case 0x7A: //ShopWindowOpen = 0x7A,
                    return parseOpenShopWindow(incomingMsg, outgoingMsg);
                case 0x7B: //ShopSaleGoldCount = 0x7B,
                    return parsePlayerCash(incomingMsg, outgoingMsg);
                case 0x7C: //ShopWindowClose = 0x7C,
                    return parseCloseShopWindow(incomingMsg, outgoingMsg);
                case 0x7D: //SafeTradeRequestAck = 0x7D,
                    return parseSafeTradeRequest(incomingMsg, outgoingMsg, true);
                case 0x7E: //SafeTradeRequestNoAck = 0x7E,
                    return parseSafeTradeRequest(incomingMsg, outgoingMsg, false);
                case 0x7F: //SafeTradeClose = 0x7F,
                    return parseSafeTradeClose(incomingMsg, outgoingMsg);
                case 0x82: //WorldLight = 0x82,
                    return parseWorldLight(incomingMsg, outgoingMsg);
                case 0x83: //MagicEffect = 0x83,
                    return parseMagicEffect(incomingMsg, outgoingMsg);
                case 0x84: //AnimatedText = 0x84,
                    return parseAnimatedText(incomingMsg, outgoingMsg);
                case 0x85: //Projectile = 0x85,
                    return parseDistanceShot(incomingMsg, outgoingMsg);
                case 0x86: //CreatureSquare = 0x86,
                    return parseCreatureSquare(incomingMsg, outgoingMsg);
                case 0x8C: //CreatureHealth = 0x8C,
                    return parseCreatureHealth(incomingMsg, outgoingMsg);
                case 0x8D: //CreatureLight = 0x8D,
                    return parseCreatureLight(incomingMsg, outgoingMsg);
                case 0x8E: //CreatureOutfit = 0x8E,
                    return parseCreatureOutfit(incomingMsg, outgoingMsg);
                case 0x8F: //CreatureSpeed = 0x8F,
                    return parseCreatureSpeed(incomingMsg, outgoingMsg);
                case 0x90: //CreatureSkull = 0x90,
                    return parseCreatureSkulls(incomingMsg, outgoingMsg);
                case 0x91: //CreatureShield = 0x91,
                    return parseCreatureShields(incomingMsg, outgoingMsg);
                case 0x96: //ItemTextWindow = 0x96,
                    return parseItemTextWindow(incomingMsg, outgoingMsg);
                case 0x97: //HouseTextWindow = 0x97,
                    return parseHouseTextWindow(incomingMsg, outgoingMsg);
                case 0xA0: //PlayerStatus = 0xA0,
                    return parsePlayerStats(incomingMsg, outgoingMsg);
                case 0xA1: //PlayerSkillsUpdate = 0xA1,
                    return parsePlayerSkills(incomingMsg, outgoingMsg);
                case 0xA2: //PlayerFlags = 0xA2,
                    return parsePlayerIcons(incomingMsg, outgoingMsg);
                case 0xA3: //CancelTarget = 0xA3,
                    return parsePlayerCancelAttack(incomingMsg, outgoingMsg);
                case 0xAA: //CreatureSpeech = 0xAA,
                    return parseCreatureSpeak(incomingMsg, outgoingMsg);
                case 0xAB: //ChannelList = 0xAB,
                    return parseChannelList(incomingMsg, outgoingMsg);
                case 0xAC: //ChannelOpen = 0xAC,
                    return parseOpenChannel(incomingMsg, outgoingMsg);
                case 0xAD: //ChannelOpenPrivate = 0xAD,
                    return parseOpenPrivatePlayerChat(incomingMsg, outgoingMsg);
                case 0xAE: //RuleViolationOpen = 0xAE,
                    return parseOpenRuleViolation(incomingMsg, outgoingMsg);
                case 0xAF: //RuleViolationRemove = 0xAF,
                    return parseRuleViolationAF(incomingMsg, outgoingMsg);
                case 0xB0: //RuleViolationCancel = 0xB0,
                    return parseRuleViolationB0(incomingMsg, outgoingMsg);
                case 0xB1: //RuleViolationLock = 0xB1,
                    return parseRuleViolationB1(incomingMsg, outgoingMsg);
                case 0xB2: //PrivateChannelCreate = 0xB2,
                    return parseCreatePrivateChannel(incomingMsg, outgoingMsg);
                case 0xB3: //ChannelClosePrivate = 0xB3,
                    return parseClosePrivateChannel(incomingMsg, outgoingMsg);
                case 0xB4: //TextMessage = 0xB4,
                    return parseTextMessage(incomingMsg, outgoingMsg);
                case 0xB5: //PlayerWalkCancel = 0xB5,
                    return parsePlayerCancelWalk(incomingMsg, outgoingMsg);
                case 0xBE: //FloorChangeUp = 0xBE,
                    return parseFloorChangeUp(incomingMsg, outgoingMsg);
                case 0xBF: //FloorChangeDown = 0xBF,
                    return parseFloorChangeDown(incomingMsg, outgoingMsg);
                case 0xC8: //OutfitWindow = 0xC8,
                    return parseOutfitWindow(incomingMsg, outgoingMsg);
                case 0xD2: //VipState = 0xD2,
                    return parseVipState(incomingMsg, outgoingMsg);
                case 0xD3: //VipLogin = 0xD3,
                    return parseVipLogin(incomingMsg, outgoingMsg);
                case 0xD4: //VipLogout = 0xD4,
                    return parseVipLogout(incomingMsg, outgoingMsg);
                case 0xF0: //QuestList = 0xF0,
                    return parseQuestList(incomingMsg, outgoingMsg);
                case 0xF1: //QuestPartList = 0xF1,
                    return parseQuestPartList(incomingMsg, outgoingMsg);
                case 0xDC: //ShowTutorial = 0xDC,
                    return parseShowTutorial(incomingMsg, outgoingMsg);
                case 0xDD: //AddMapMarker = 0xDD,
                    return parseAddMapMarker(incomingMsg, outgoingMsg);
                default:
                    Logger.Log("Tipo de pacote desconhecido recebido do servidor. Tipo: " + command.ToString("X2"), LogType.ERROR);
                    return false;
            }
        }

        private bool parseGMAction(NetworkMessage incomingMsg, NetworkMessage outgoingMsg)
        {
            return false;
        }

        private bool parseSelfApper(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Creatures.GetInstance().Clear();
            Inventory.GetInstance().Clear();
            Containers.GetInstance().Clear();
            Map.GetInstance().Clear();
            GlobalVariables.Clear();

            GlobalVariables.SetPlayerId(incMsg.GetUInt32());
            outMsg.AddUInt32(GlobalVariables.GetPlayerId());

            outMsg.AddUInt16(incMsg.GetUInt16());

            GlobalVariables.SetCanReportBugs(Convert.ToBoolean(incMsg.GetByte()));
            outMsg.AddByte(Convert.ToByte(GlobalVariables.GetCanReportBugs()));

            GlobalVariables.SetConnected(true);

            new Action(getPlayerMemoryAddress).BeginInvoke(null, null);

            Logger.Log("Entrou no jogo.");
            return true;
        }

        private bool parseErrorMessage(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            String msg = incMsg.GetString();
            outMsg.AddString(msg);
            Logger.Log("Mensagem de erro recebida do servidor. Mensagem: " + msg, LogType.ERROR);
            return true;
        }

        private bool parseFyiMessage(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            String msg = incMsg.GetString();
            outMsg.AddString(msg);
            Logger.Log("Mensagem de informação recebida do servidor. Mensagem: " + msg);
            return true;
        }

        private bool parseWaitingList(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            outMsg.AddString(incMsg.GetString());
            byte waitTime = incMsg.GetByte();
            outMsg.AddByte(waitTime);
            Logger.Log("Entrando na fila de espera. Tempo: " + waitTime);
            return true;
        }

        private bool parsePing(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //no data
            return true;
        }

        private bool parseDeath(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //no data
            return true;
        }

        private bool parseCanReportBugs(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            GlobalVariables.SetCanReportBugs(Convert.ToBoolean(incMsg.GetByte()));
            outMsg.AddByte(Convert.ToByte(GlobalVariables.GetCanReportBugs()));
            return true;
        }

        private bool parseMapDescription(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position currentPos = incMsg.GetPosition();
            outMsg.AddPosition(currentPos);
            GlobalVariables.SetPlayerPosition(currentPos);

            if (!setMapDescription(incMsg, outMsg, (int)currentPos.X - 8, (int)currentPos.Y - 6, (int)currentPos.Z, 18, 14))
            {
                Logger.Log("Erro ao efetuar o parse da descrição do mapa. Código: 0x64", LogType.ERROR);
                return false;
            }

            Game.GetInstance().OnReceivePlayerMove(currentPos);
            return true;
        }

        private bool parseMoveNorth(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position myPos = GlobalVariables.GetPlayerPosition();
            myPos.Y--;

            if (!setMapDescription(incMsg, outMsg, (int)myPos.X - 8, (int)myPos.Y - 6, (int)myPos.Z, 18, 1))
            {
                Logger.Log("Erro ao efetuar o parse da descrição do mapa. Código: 0x65", LogType.ERROR);
                return false;
            }

            Game.GetInstance().OnReceivePlayerMove(myPos);
            return true;
        }

        private bool parseMoveEast(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position myPos = GlobalVariables.GetPlayerPosition();
            myPos.X++;

            if (!setMapDescription(incMsg, outMsg, (int)myPos.X + 9, (int)myPos.Y - 6, (int)myPos.Z, 1, 14))
            {
                Logger.Log("Erro ao efetuar o parse da descrição do mapa. Código: 0x66", LogType.ERROR);
                return false;
            }

            Game.GetInstance().OnReceivePlayerMove(myPos);
            return true;
        }

        private bool parseMoveSouth(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position myPos = GlobalVariables.GetPlayerPosition();
            myPos.Y++;

            if (!setMapDescription(incMsg, outMsg, (int)myPos.X - 8, (int)myPos.Y + 7, (int)myPos.Z, 18, 1))
            {
                Logger.Log("Erro ao efetuar o parse da descrição do mapa. Código: 0x67", LogType.ERROR);
                return false;
            }

            Game.GetInstance().OnReceivePlayerMove(myPos);
            return true;
        }

        private bool parseMoveWest(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position myPos = GlobalVariables.GetPlayerPosition();
            myPos.X--;

            if (!setMapDescription(incMsg, outMsg, (int)myPos.X - 8, (int)myPos.Y - 6, (int)myPos.Z, 1, 14))
            {
                Logger.Log("Erro ao efetuar o parse da descrição do mapa. Código: 0x68", LogType.ERROR);
                return false;
            }

            Game.GetInstance().OnReceivePlayerMove(myPos);
            return true;
        }

        private bool parseUpdateTile(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position tilePos = incMsg.GetPosition();
            outMsg.AddPosition(tilePos);

            ushort thingId = incMsg.PeekUInt16();

            if (thingId == 0xFF01)
            {
                outMsg.AddUInt16(incMsg.GetUInt16());

                Tile tile = Map.GetInstance().GetTile(tilePos);

                if (tile == null)
                    Logger.Log("Erro ao atualizar o tile. Posição: " + tilePos.ToString(), LogType.ERROR);

                tile.Clear();
            }
            else
            {
                if (!setTileDescription(incMsg, outMsg, tilePos))
                    Logger.Log("Erro ao atualizar o tile. Posição: " + tilePos.ToString(), LogType.ERROR);

                outMsg.AddUInt16(incMsg.GetUInt16());
            }

            return true;
        }

        private bool parseTileAddThing(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position tilePos = incMsg.GetPosition();
            outMsg.AddPosition(tilePos);

            byte stack = incMsg.GetByte();
            outMsg.AddByte(stack);

            Thing thing = internalGetThing(incMsg, outMsg);

            if (thing == null)
            {
                Logger.Log("Falha ao obter o objeto.", LogType.ERROR);
                return false;
            }

            Tile tile = Map.GetInstance().GetTile(tilePos);

            if (tile == null)
            {
                Logger.Log("Falha ao obter o tile.", LogType.ERROR);
                return false;
            }

            if (!tile.InsertThing(thing, stack))
            {
                Logger.Log("Falha ao adicionar o objeto no tile. Posição: " + tile.GetPosition(), LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseTileTransformThing(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position tilePos = incMsg.GetPosition();
            outMsg.AddPosition(tilePos);

            byte stackPos = incMsg.GetByte();
            outMsg.AddByte(stackPos);

            ushort thingId = incMsg.GetUInt16();
            outMsg.AddUInt16(thingId);

            if (thingId == 0x0061 || thingId == 0x0062 || thingId == 0x0063)
            {
                //creature turn
                uint creatureID = incMsg.GetUInt32();
                outMsg.AddUInt32(creatureID);

                byte direction = incMsg.GetByte();
                outMsg.AddByte(direction);

                Creature creature = Creatures.GetInstance().GetCreature(creatureID);

                if (creature == null)
                {
                    Logger.Log("Falha ao transformar o objeto. Creatura retornou nula.", LogType.ERROR);
                    return false;
                }

                if (direction > 3)
                {
                    Logger.Log("Falha ao transformar o objeto. Direção maior que 3.", LogType.ERROR);
                    return false;
                }

                creature.SetTurnDirection((Direction)direction);
            }
            else
            {
                //get tile
                Tile tile = Map.GetInstance().GetTile(tilePos);

                if (tile == null)
                {
                    Logger.Log("Falha ao transformar o objeto. Tile retornou nulo.", LogType.ERROR);
                    return false;
                }

                //removes the old item
                Thing thing = tile.GetThingByStackPosition(stackPos);

                if (thing == null)
                {
                    Logger.Log("Falha ao transformar o objeto. Objeto retornou nulo.", LogType.ERROR);
                    return false;
                }

                if (!tile.RemoveThing(thing))
                {
                    Logger.Log("Falha ao transformar o objeto. Falha ao remover o item.", LogType.ERROR);
                    return false;
                }

                //adds a new item
                Item item = internalGetItem(incMsg, outMsg, thingId);

                if (!tile.InsertThing(item, (int)stackPos))
                {
                    Logger.Log("Falha ao transformar o objeto. Não foi possivel inserir o item.", LogType.ERROR);
                    return false;
                }
            }

            return true;
        }

        private bool parseTileRemoveThing(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position tilePos = incMsg.GetPosition();
            outMsg.AddPosition(tilePos);

            byte stackPos = incMsg.GetByte();
            outMsg.AddByte(stackPos);

            //get tile
            Tile tile = Map.GetInstance().GetTile(tilePos);
            if (tile == null)
            {
                Logger.Log("Falha ao remover o objeto. O tile retornou nulo.", LogType.ERROR);
                return false;
            }

            //remove thing
            Thing thing = tile.GetThingByStackPosition(stackPos);
            if (thing == null)
            {
                //RAISE_PROTOCOL_WARNING("Tile Remove - !thing");
                //TODO. send update tile
                return true;
            }

            // NOTE (nfries88): Maybe this will fix http://otfans.net/project.php?issueid=490
            if (thing is Creature)
            {
                Creature cr = (Creature)thing;
                cr.SetPosition(new Position(0, 0, 0));
            }

            if (!tile.RemoveThing(thing))
            {
                Logger.Log("Falha ao remover o objeto. Não foi possivel remover o objeto.", LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseCreatureMove(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position oldPos = incMsg.GetPosition();
            outMsg.AddPosition(oldPos);

            byte oldStackPos = incMsg.GetByte();
            outMsg.AddByte(oldStackPos);

            Position newPos = incMsg.GetPosition();
            outMsg.AddPosition(newPos);

            if (oldStackPos > 9)
            {
                Logger.Log("Creature move - oldStackpos", LogType.ERROR);
                return false;
            }

            Tile tile = Map.GetInstance().GetTile(oldPos);

            if (tile == null)
            {
                Logger.Log("Creature move - !tile old", LogType.ERROR);
                return false;
            }

            Thing thing = tile.GetThingByStackPosition(oldStackPos);
            if (thing == null)
            {
                //RAISE_PROTOCOL_WARNING("Creature move - !thing");
                //TODO. Notify GUI
                //TODO. send update tile
                return true;
            }

            if (!(thing is Creature))
            {
                Logger.Log("Creature move - !creature", LogType.ERROR);
                return false;
            }

            Creature creature = (Creature)thing;

            if (!tile.RemoveThing(creature))
            {
                Logger.Log("Creature move - removeThing", LogType.ERROR);
                return false;
            }

            tile = Map.GetInstance().GetTile(newPos);

            if (tile == null)
            {
                Logger.Log("Creature move - !tile new", LogType.ERROR);
                return false;
            }

            if (!tile.AddThing(creature))
            {
                Logger.Log("Creature move - addThing", LogType.ERROR);
                return false;
            }

            //creature->setMoving(oldPos, newPos);

            //update creature direction
            if (oldPos.X > newPos.X)
            {
                creature.SetTurnDirection(Direction.Left);
            }
            else if (oldPos.X < newPos.X)
            {
                creature.SetTurnDirection(Direction.Right);
            }
            else if (oldPos.Y > newPos.Y)
            {
                creature.SetTurnDirection(Direction.Up);
            }
            else if (oldPos.Y < newPos.Y)
            {
                creature.SetTurnDirection(Direction.Down);
            }

            return true;
        }

        private bool parseOpenContainer(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte cid = incMsg.GetByte();
            outMsg.AddByte(cid);
            ushort itemId = incMsg.GetUInt16();
            outMsg.AddUInt16(itemId);
            String name = incMsg.GetString();
            outMsg.AddString(name);
            byte capacity = incMsg.GetByte();
            outMsg.AddByte(capacity);
            byte hasParent = incMsg.GetByte();
            outMsg.AddByte(hasParent);
            byte size = incMsg.GetByte();
            outMsg.AddByte(size);

            if (size > capacity)
            {
                Logger.Log("Open container - size > cap", LogType.ERROR);
                return false;
            }

            // NOTE (nfries88)
            // The server sends a message to open a container when it is moved client-side
            // In the event of it already being opened, don't remake it
            // but allow updates.
            Container container = Containers.GetInstance().CreateContainer(cid);
            if (container == null)
            {
                Logger.Log("Open container - !container", LogType.ERROR);
                return true;
            }

            container.SetName(name);
            container.SetItemId(itemId);
            container.SetCapacity(capacity);
            container.SetHasParent(hasParent == 1);

            for (uint i = 0; i < size; ++i)
            {
                Item item = internalGetItem(incMsg, outMsg, 0xFFFFFFFF);
                if (item == null)
                {
                    Logger.Log("Container Open - !item", LogType.ERROR);
                    return false;
                }
                // When the server sends a message to open a container that's already opened
                if (container.GetItem(i) != null)
                {
                    if (!container.UpdateItem(i, item))
                    {
                        Logger.Log("Container Open - updateItem", LogType.ERROR);
                        return false;
                    }
                }
                else if (!container.AddItemInitial(item))
                {
                    Logger.Log("Container Open - addItem", LogType.ERROR);
                    return false;
                }
            }

            return true;
        }

        private bool parseCloseContainer(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte cid = incMsg.GetByte();
            outMsg.AddByte(cid);

            Containers.GetInstance().DeleteContainer(cid);
            return true;
        }

        private bool parseContainerAddItem(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte cid = incMsg.GetByte();
            outMsg.AddByte(cid);

            Item item = internalGetItem(incMsg, outMsg, 0xFFFFFFFF);

            if (item == null)
            {
                Logger.Log("Container add - !item", LogType.ERROR);
                return false;
            }

            Container container = Containers.GetInstance().GetContainer(cid);

            if (container == null)
            {
                Logger.Log("Container add - !container");
                return true;
            }

            if (!container.AddItem(item))
            {
                Logger.Log("Container add - addItem", LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseContainerUpdateItem(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte cid = incMsg.GetByte();
            outMsg.AddByte(cid);

            byte slot = incMsg.GetByte();
            outMsg.AddByte(slot);

            Item item = internalGetItem(incMsg, outMsg, 0xFFFFFFFF);

            if (item == null)
            {
                Logger.Log("Container update - !item", LogType.ERROR);
                return false;
            }

            Container container = Containers.GetInstance().GetContainer(cid);

            if (container == null)
            {
                Logger.Log("Container update - !container");
                return true;
            }

            if (!container.UpdateItem(slot, item))
            {
                Logger.Log("Container update - updateItem", LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseContainerRemoveItem(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte cid = incMsg.GetByte();
            outMsg.AddByte(cid);

            byte slot = incMsg.GetByte();
            outMsg.AddByte(slot);

            Container container = Containers.GetInstance().GetContainer(cid);

            if (container == null)
            {
                Logger.Log("Container remove - !container");
                return true;
            }

            if (!container.RemoveItem(slot))
            {
                Logger.Log("Container remove - removeItem", LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseInventorySetSlot(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte slot = incMsg.GetByte();
            outMsg.AddByte(slot);

            Item item = internalGetItem(incMsg, outMsg, 0xFFFFFFFF);

            if (item == null)
            {
                Logger.Log("Inventory set - !item", LogType.ERROR);
                return false;
            }

            if (!Inventory.GetInstance().AddItem(slot, item))
            {
                Logger.Log("Inventory set - addItem", LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseInventoryResetSlot(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte slot = incMsg.GetByte();
            outMsg.AddByte(slot);

            if (!Inventory.GetInstance().RemoveItem(slot))
            {
                Logger.Log("Inventory reset - removeItem", LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseSafeTradeRequest(NetworkMessage incMsg, NetworkMessage outMsg, bool ack)
        {
            String name = incMsg.GetString();
            outMsg.AddString(name);
            byte count = incMsg.GetByte();
            outMsg.AddByte(count);

            /*Container* container = NULL;
            if(ack){
                container = Containers::getInstance().newTradeContainerAck();
            }
            else{
                container = Containers::getInstance().newTradeContainer();
            }

            if(!container){
                RAISE_PROTOCOL_ERROR("Trade open - !container");
            }
            container->setName(name);
            container->setCapacity(count);*/

            for (uint i = 0; i < count; ++i)
            {

                Item item = internalGetItem(incMsg, outMsg, 0xFFFFFFFF);

                if (item == null)
                {
                    Logger.Log("Trade open - !item", LogType.ERROR);
                    return false;
                }

                /*if(!container->addItemInitial(item)){
                    RAISE_PROTOCOL_ERROR("Container add - addItem");
                }*/
            }

            return true;
        }

        private bool parseSafeTradeClose(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //no data
            //Containers::getInstance().closeTradeContainer();
            //Notifications::closeTradeWindow();
            return true;
        }

        private bool parseWorldLight(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte level = incMsg.GetByte();
            outMsg.AddByte(level);
            byte color = incMsg.GetByte();
            outMsg.AddByte(color);

            GlobalVariables.SetWorldLightLevel(level);
            GlobalVariables.SetWorldLightColor(color);

            return true;
        }

        private bool parseMagicEffect(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position effectPos = incMsg.GetPosition();
            outMsg.AddPosition(effectPos);
            byte effect = incMsg.GetByte();
            outMsg.AddByte(effect);

            Tile tile = Map.GetInstance().GetTile(effectPos);
            if (tile == null)
            {
                Logger.Log("Magic effect - !tile", LogType.ERROR);
                return false;
            }

            //tile->addEffect(effect);
            return true;
        }

        private bool parseAnimatedText(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position textPos = incMsg.GetPosition();
            outMsg.AddPosition(textPos);
            byte color = incMsg.GetByte();
            outMsg.AddByte(color);
            String text = incMsg.GetString();
            outMsg.AddString(text);

            //Map::getInstance().addAnimatedText(textPos, color, text);
            return true;
        }

        private bool parseDistanceShot(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position fromPos = incMsg.GetPosition();
            outMsg.AddPosition(fromPos);
            Position toPos = incMsg.GetPosition();
            outMsg.AddPosition(toPos);
            byte effect = incMsg.GetByte();
            outMsg.AddByte(effect);

            //Map::getInstance().addDistanceEffect(fromPos, toPos, effect);
            return true;
        }

        private bool parseCreatureSquare(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);
            byte color = incMsg.GetByte();
            outMsg.AddByte(color);

            Creature creature = Creatures.GetInstance().GetCreature(creatureId);
            if (creature != null)
            {
                creature.SetSquare((SquareColor)color);
                Game.GetInstance().OnReceiveCreatureSquare(creature);
            }

            return true;
        }

        private bool parseCreatureHealth(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);
            byte percent = incMsg.GetByte();
            outMsg.AddByte(percent);

            Creature creature = Creatures.GetInstance().GetCreature(creatureId);

            if (creature != null)
            {
                if (percent > 100)
                {
                    Logger.Log("Creature health - percent > 100", LogType.ERROR);
                    return false;
                }

                creature.SetHealth(percent);
                //Notifications::onCreatureChangeHealth(creatureID, percent);*/
            }

            return true;
        }

        private bool parseCreatureLight(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);

            byte level = incMsg.GetByte();
            outMsg.AddByte(level);

            byte color = incMsg.GetByte();
            outMsg.AddByte(color);

            Creature creature = Creatures.GetInstance().GetCreature(creatureId);

            if (creature != null)
            {
                creature.SetLightLevel(level);
                creature.SetLightColor(color);
            }

            return true;
        }

        private bool parseCreatureOutfit(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);

            Creature creature = Creatures.GetInstance().GetCreature(creatureId);

            if (creature != null)
            {
                creature.SetOutfit(incMsg.GetOutfit());
                outMsg.AddOutfit(creature.GetOutfit());
            }
            else
            {
                outMsg.AddOutfit(incMsg.GetOutfit());
            }
            return true;
        }

        private bool parseCreatureSpeed(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);

            ushort speed = incMsg.GetUInt16();
            outMsg.AddUInt16(speed);

            Creature creature = Creatures.GetInstance().GetCreature(creatureId);
            if (creature != null)
            {
                creature.SetSpeed(speed);
            }
            return true;
        }

        private bool parseCreatureSkulls(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);

            byte skull = incMsg.GetByte();
            outMsg.AddByte(skull);

            Creature creature = Creatures.GetInstance().GetCreature(creatureId);
            if (creature != null)
            {
                creature.SetSkull(skull);
            }
            return true;
        }

        private bool parseCreatureShields(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);

            byte shield = incMsg.GetByte();
            outMsg.AddByte(shield);

            Creature creature = Creatures.GetInstance().GetCreature(creatureId);
            if (creature != null)
            {
                creature.SetShield(shield);
            }
            return true;
        }

        private bool parseItemTextWindow(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            /*MSG_READ_U32(windowID);
            MSG_READ_U16(itemID);
            MSG_READ_U16(maxlen);
            MSG_READ_STRING(text);
            MSG_READ_STRING(writter);
            MSG_READ_STRING(date);*/

            outMsg.AddUInt32(incMsg.GetUInt32());
            outMsg.AddUInt16(incMsg.GetUInt16());
            outMsg.AddUInt16(incMsg.GetUInt16());
            outMsg.AddString(incMsg.GetString());
            outMsg.AddString(incMsg.GetString());
            outMsg.AddString(incMsg.GetString());

            return true;
        }

        private bool parseHouseTextWindow(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            /*MSG_READ_U8(unk);
            MSG_READ_U32(windowID);
            MSG_READ_STRING(text);*/

            outMsg.AddByte(incMsg.GetByte());
            outMsg.AddUInt32(incMsg.GetUInt32());
            outMsg.AddString(incMsg.GetString());

            return true;
        }
        private bool parsePlayerStats(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            ushort health = incMsg.GetUInt16();
            outMsg.AddUInt16(health);

            ushort healthMax = incMsg.GetUInt16();
            outMsg.AddUInt16(healthMax);

            uint capacity = incMsg.GetUInt32();
            outMsg.AddUInt32(capacity);

            uint experience = incMsg.GetUInt32();
            outMsg.AddUInt32(experience);

            ushort level = incMsg.GetUInt16();
            outMsg.AddUInt16(level);

            byte levelPercent = incMsg.GetByte();
            outMsg.AddByte(levelPercent);

            ushort mana = incMsg.GetUInt16();
            outMsg.AddUInt16(mana);

            ushort maxMana = incMsg.GetUInt16();
            outMsg.AddUInt16(maxMana);

            byte magicLevel = incMsg.GetByte();
            outMsg.AddByte(magicLevel);

            byte magicLevelPercent = incMsg.GetByte();
            outMsg.AddByte(magicLevelPercent);

            byte soul = incMsg.GetByte();
            outMsg.AddByte(soul);

            ushort stamina = incMsg.GetUInt16();
            outMsg.AddUInt16(stamina);

            //some validations
            // NOTE (nfries88): sometimes, upon death, your mana will be greater than your maximum mana (due to level loss)
            if (health > healthMax || levelPercent > 100 || magicLevelPercent > 100)
            {
                Logger.Log("Player stats - values", LogType.ERROR);
                return false;
            }

            GlobalVariables.SetPlayerStatus(PlayerStatus.Health, health);
            GlobalVariables.SetPlayerStatus(PlayerStatus.HealthMax, healthMax);
            GlobalVariables.SetPlayerStatus(PlayerStatus.Capacity, capacity);
            GlobalVariables.SetPlayerStatus(PlayerStatus.Experience, experience);
            GlobalVariables.SetPlayerSkill(Skills.Level, SkillAttribute.Level, level);
            GlobalVariables.SetPlayerSkill(Skills.Level, SkillAttribute.Percent, levelPercent);
            GlobalVariables.SetPlayerStatus(PlayerStatus.Mana, mana);
            GlobalVariables.SetPlayerStatus(PlayerStatus.ManaMax, maxMana);
            GlobalVariables.SetPlayerSkill(Skills.Magic, SkillAttribute.Level, magicLevel);
            GlobalVariables.SetPlayerSkill(Skills.Magic, SkillAttribute.Percent, magicLevelPercent);
            GlobalVariables.SetPlayerStatus(PlayerStatus.Soul, soul);
            GlobalVariables.SetPlayerStatus(PlayerStatus.Stamina, stamina);

            return true;
        }

        private bool parsePlayerSkills(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte fist = incMsg.GetByte();
            outMsg.AddByte(fist);

            byte fistPercent = incMsg.GetByte();
            outMsg.AddByte(fistPercent);

            byte club = incMsg.GetByte();
            outMsg.AddByte(club);

            byte clubPercent = incMsg.GetByte();
            outMsg.AddByte(clubPercent);

            byte sword = incMsg.GetByte();
            outMsg.AddByte(sword);

            byte swordPercent = incMsg.GetByte();
            outMsg.AddByte(swordPercent);

            byte axe = incMsg.GetByte();
            outMsg.AddByte(axe);

            byte axePercent = incMsg.GetByte();
            outMsg.AddByte(axePercent);

            byte distance = incMsg.GetByte();
            outMsg.AddByte(distance);

            byte distancePercent = incMsg.GetByte();
            outMsg.AddByte(distancePercent);

            byte shield = incMsg.GetByte();
            outMsg.AddByte(shield);

            byte shieldPercent = incMsg.GetByte();
            outMsg.AddByte(shieldPercent);

            byte fish = incMsg.GetByte();
            outMsg.AddByte(fish);

            byte fishPercent = incMsg.GetByte();
            outMsg.AddByte(fishPercent);

            //some validations
            if (fistPercent > 100 || clubPercent > 100 || swordPercent > 100 ||
               axePercent > 100 || distancePercent > 100 || shieldPercent > 100 ||
               fishPercent > 100)
            {
                Logger.Log("Player skills - values", LogType.ERROR);
            }

            GlobalVariables.SetPlayerSkill(Skills.Fist, SkillAttribute.Level, fist);
            GlobalVariables.SetPlayerSkill(Skills.Fist, SkillAttribute.Percent, fistPercent);
            GlobalVariables.SetPlayerSkill(Skills.Club, SkillAttribute.Level, club);
            GlobalVariables.SetPlayerSkill(Skills.Club, SkillAttribute.Percent, clubPercent);
            GlobalVariables.SetPlayerSkill(Skills.Sword, SkillAttribute.Level, sword);
            GlobalVariables.SetPlayerSkill(Skills.Sword, SkillAttribute.Percent, swordPercent);
            GlobalVariables.SetPlayerSkill(Skills.Axe, SkillAttribute.Level, axe);
            GlobalVariables.SetPlayerSkill(Skills.Axe, SkillAttribute.Percent, axePercent);
            GlobalVariables.SetPlayerSkill(Skills.Distance, SkillAttribute.Level, distance);
            GlobalVariables.SetPlayerSkill(Skills.Distance, SkillAttribute.Percent, distancePercent);
            GlobalVariables.SetPlayerSkill(Skills.Shield, SkillAttribute.Level, shield);
            GlobalVariables.SetPlayerSkill(Skills.Shield, SkillAttribute.Percent, shieldPercent);
            GlobalVariables.SetPlayerSkill(Skills.Fish, SkillAttribute.Level, fish);
            GlobalVariables.SetPlayerSkill(Skills.Fish, SkillAttribute.Percent, fishPercent);

            return true;
        }

        private bool parsePlayerIcons(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            ushort icons = incMsg.GetUInt16();
            outMsg.AddUInt16(icons);
            GlobalVariables.SetPlayerIcons(icons);
            return true;
        }

        private bool parsePlayerCancelAttack(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //no data
			GlobalVariables.SetAttackId(0);
            return true;
        }

        private bool parseCreatureSpeak(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint unknown = incMsg.GetUInt32();
            outMsg.AddUInt32(unknown);
            String senderName = incMsg.GetString();
            outMsg.AddString(senderName);
            ushort senderLevel = incMsg.GetUInt16();
            outMsg.AddUInt16(senderLevel);
            SpeechType type = (SpeechType)incMsg.GetByte();
            outMsg.AddByte((byte)type);

            switch (type)
            {
                case SpeechType.Say:
                case SpeechType.Whisper:
                case SpeechType.Yell:
                case SpeechType.MonsterSay:
                case SpeechType.MonsterYell:
                case SpeechType.PrivateNPCToPlayer:
                    Position position = incMsg.GetPosition();
                    outMsg.AddPosition(position);
                    break;
                case SpeechType.ChannelRed:
                case SpeechType.ChannelRedAnonymous:
                case SpeechType.ChannelOrange:
                case SpeechType.ChannelYellow:
                case SpeechType.ChannelWhite:
                    ChatChannel channelId = (ChatChannel)incMsg.GetUInt16();
                    outMsg.AddUInt16((ushort)channelId);
                    break;
                case SpeechType.RuleViolationReport:
                    uint time = incMsg.GetUInt32();
                    outMsg.AddUInt32(time);
                    break;
                default:
                    Logger.Log("Tipo de mensagem desconhecido.", LogType.ERROR);
                    break;
            }

            String message = incMsg.GetString();
            outMsg.AddString(message);

            return true;
        }

        private bool parseChannelList(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte count = incMsg.GetByte();
            outMsg.AddByte(count);

            for (uint i = 0; i < count; ++i)
            {
                ushort channelId = incMsg.GetUInt16();
                outMsg.AddUInt16(channelId);
                String name = incMsg.GetString();
                outMsg.AddString(name);
            }

            return true;
        }

        private bool parseOpenChannel(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            ushort channelId = incMsg.GetUInt16();
            outMsg.AddUInt16(channelId);
            String name = incMsg.GetString();
            outMsg.AddString(name);
            //Notifications::openChannel(channelID, name);
            return true;
        }

        private bool parseOpenPrivatePlayerChat(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            String name = incMsg.GetString();
            outMsg.AddString(name);
            return true;
        }

        private bool parseOpenRuleViolation(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //TODO
            outMsg.AddUInt16(incMsg.GetUInt16());
            return true;
        }
        private bool parseRuleViolationAF(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //TODO
            outMsg.AddUInt16(incMsg.GetUInt16());
            return true;
        }
        private bool parseRuleViolationB0(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //TODO
            outMsg.AddUInt16(incMsg.GetUInt16());
            return true;
        }
        private bool parseRuleViolationB1(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //TODO
            outMsg.AddUInt16(incMsg.GetUInt16());
            return true;
        }

        private bool parseCreatePrivateChannel(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            ushort channelId = incMsg.GetUInt16();
            outMsg.AddUInt16(channelId);
            String name = incMsg.GetString();
            outMsg.AddString(name);
            return true;
        }

        private bool parseClosePrivateChannel(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            ushort channelId = incMsg.GetUInt16();
            outMsg.AddUInt16(channelId);
            return true;
        }

        private bool parseTextMessage(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte messageType = incMsg.GetByte();
            outMsg.AddByte(messageType);

            String text = incMsg.GetString();
            outMsg.AddString(text);

            if (messageType < 0x11 || messageType > 0x26)
            {
                Logger.Log("text message - type: " + messageType, LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parsePlayerCancelWalk(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte direction = incMsg.GetByte();
            outMsg.AddByte(direction);
            if (direction > 3)
            {
                Logger.Log("cancel walk - direction > 3", LogType.ERROR);
                return false;
            }

            return true;
        }

        private bool parseFloorChangeUp(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position myPos = GlobalVariables.GetPlayerPosition();
            myPos.Z--;

            //going to surface
            if (myPos.Z == 7)
            {
                //floor 7 and 6 already set
                for (int i = 5; i >= 0; i--)
                {
                    if (!setFloorDescription(incMsg, outMsg, (int)myPos.X - 8, (int)myPos.Y - 6, i, 18, 14, 8 - i))
                    {
                        Logger.Log("Set Floor Desc z = 7 0xBE", LogType.ERROR);
                        return false;
                    }
                }
            }
            //underground, going one floor up (still underground)
            else if (myPos.Z > 7)
            {
                if (!setFloorDescription(incMsg, outMsg, (int)myPos.X - 8, (int)myPos.Y - 6, (int)myPos.Z - 2, 18, 14, 3))
                {
                    Logger.Log("Set Floor Desc  z > 7 0xBE", LogType.ERROR);
                    return false;
                }
            }

            myPos.X++;
            myPos.Y++;

            return true;
        }

        private bool parseFloorChangeDown(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position myPos = GlobalVariables.GetPlayerPosition();
            myPos.Z++;
            //going from surface to underground
            if (myPos.Z == 8)
            {
                int j, i;
                for (i = (int)myPos.Z, j = -1; i < (int)myPos.Z + 3; ++i, --j)
                {
                    if (!setFloorDescription(incMsg, outMsg, (int)myPos.X - 8, (int)myPos.Y - 6, i, 18, 14, j))
                    {
                        Logger.Log("Set Floor Desc  z = 8 0xBF", LogType.ERROR);
                        return false;
                    }
                }
            }
            //going further down
            else if (myPos.Z > 8 && myPos.Z < 14)
            {
                if (!setFloorDescription(incMsg, outMsg, (int)myPos.X - 8, (int)myPos.Y - 6, (int)myPos.Z + 2, 18, 14, -3))
                {
                    Logger.Log("Set Floor Desc  z > 8 && z < 14 0xBF", LogType.ERROR);
                    return false;
                }
            }

            myPos.X--;
            myPos.Y--;

            return true;
        }

        private bool parseOutfitWindow(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Outfit outfit = incMsg.GetOutfit();
            outMsg.AddOutfit(outfit);
            byte nOutfits = incMsg.GetByte();

            if (nOutfits == 0 || nOutfits > 25)
            {
                Logger.Log("Outfit window - n = 0 || n > 25", LogType.ERROR);
                return false;
            }

            for (uint i = 0; i < nOutfits; ++i)
            {

                ushort outfitID = incMsg.GetUInt16();
                outMsg.AddUInt16(outfitID);

                if (Objects.GetInstance().GetOutfitType(outfitID) == null)
                {
                    Logger.Log("Outfit window  - outfit list error", LogType.ERROR);
                    return false;
                }

                String name = incMsg.GetString();
                outMsg.AddString(name);

                byte addons = incMsg.GetByte();
                outMsg.AddByte(addons);

            }

            return true;
        }

        private bool parseVipState(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);

            String name = incMsg.GetString();
            outMsg.AddString(name);

            byte online = incMsg.GetByte();
            outMsg.AddByte(online);

            //VipList::getInstance().setEntry(creatureID, name, online);
            return true;
        }

        private bool parseVipLogin(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);
            // VipList::getInstance().setEntry(creatureID, true);
            return true;
        }

        private bool parseVipLogout(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint creatureId = incMsg.GetUInt32();
            outMsg.AddUInt32(creatureId);

            //VipList::getInstance().setEntry(creatureID, false);
            return true;
        }

        private bool parseQuestList(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            ushort nQuests = incMsg.GetUInt16();
            outMsg.AddUInt16(nQuests);

            for (uint i = 0; i < nQuests; ++i)
            {
                ushort questId = incMsg.GetUInt16();
                outMsg.AddUInt16(questId);

                String questsName = incMsg.GetString();
                outMsg.AddString(questsName);

                byte questState = incMsg.GetByte();
                outMsg.AddByte(questState);
            }
            return true;
        }

        private bool parseQuestPartList(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            ushort questId = incMsg.GetUInt16();
            outMsg.AddUInt16(questId);

            byte nMission = incMsg.GetByte();
            outMsg.AddByte(nMission);

            for (uint i = 0; i < nMission; ++i)
            {
                String questsName = incMsg.GetString();
                outMsg.AddString(questsName);

                String questsDesc = incMsg.GetString();
                outMsg.AddString(questsDesc);

            }
            return true;
        }

        // 8.2+
        private bool parseOpenShopWindow(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte size = incMsg.GetByte();
            outMsg.AddByte(size);

            for (uint i = 0; i < size; ++i)
            {
                ushort itemId = incMsg.GetUInt16();
                outMsg.AddUInt16(itemId);

                byte runeCharges = incMsg.GetByte();
                outMsg.AddByte(runeCharges);

                String name = incMsg.GetString();
                outMsg.AddString(name);

                uint weight = incMsg.GetUInt32();
                outMsg.AddUInt32(weight);

                uint buyPrice = incMsg.GetUInt32();
                outMsg.AddUInt32(buyPrice);

                uint sellPrice = incMsg.GetUInt32();
                outMsg.AddUInt32(sellPrice);
            }

            return true;
        }

        private bool parsePlayerCash(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            uint cash = incMsg.GetUInt32();
            outMsg.AddUInt32(cash);

            byte size = incMsg.GetByte();
            outMsg.AddByte(size);

            for (uint i = 0; i < size; ++i)
            {
                ushort itemId = incMsg.GetUInt16();
                outMsg.AddUInt16(itemId);

                byte runeCharges = incMsg.GetByte();
                outMsg.AddByte(runeCharges);
            }

            GlobalVariables.SetPlayerCash(cash);
            return true;
        }

        private bool parseCloseShopWindow(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            return true;
        }

        private bool parseShowTutorial(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            byte tutorialId = incMsg.GetByte();
            outMsg.AddByte(tutorialId);

            return true;
        }

        private bool parseAddMapMarker(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            Position myPos = incMsg.GetPosition();
            outMsg.AddPosition(myPos);

            byte icon = incMsg.GetByte();
            outMsg.AddByte(icon);

            String desc = incMsg.GetString();
            outMsg.AddString(desc);

            return true;
        }
        #endregion

        #region Send Server

        private void send()
        {
            if (serverSendMsg.Length > serverSendMsg.GetPacketHeaderSize() + 2)
            {
                serverSendMsg.InsetLogicalPacketHeader();
                serverSendMsg.PrepareToSend(Kernel.GetInstance().Proxy.XteaKey);

                Kernel.GetInstance().Proxy.SendToServer(serverSendMsg.Data);
            }
        }

		public void SendAttackCreature(uint creatureId)
		{
			lock(serverSendMsg)
			{
				serverSendMsg.Reset();
				serverSendMsg.AddByte(0xA1);
				serverSendMsg.AddUInt32(creatureId);
				send();
				GlobalVariables.SetAttackId(creatureId);
			}
		}
		
        public void SendLookItem(Position pos, ushort itemId, byte stackPos)
        {
            lock (serverSendMsg)
            {
                serverSendMsg.Reset();
                serverSendMsg.AddByte(0x8C);
                serverSendMsg.AddPosition(pos);
                serverSendMsg.AddUInt16(itemId);
                serverSendMsg.AddByte(stackPos);
                send();
            }
        }

        public void SendSay(SpeechType type, String text)
        {
            lock (serverSendMsg)
            {
                serverSendMsg.Reset();
                serverSendMsg.AddByte(0x96);
                serverSendMsg.AddByte((byte)type);
                serverSendMsg.AddString(text);
                send();
            }
        }

        public void SendUseItem(Position pos, ushort itemId, byte stackPos)
        {
            lock (serverSendMsg)
            {
                serverSendMsg.Reset();
                serverSendMsg.AddByte(0x82);
                serverSendMsg.AddPosition(pos);
                serverSendMsg.AddUInt16(itemId);
                serverSendMsg.AddByte(stackPos);
                serverSendMsg.AddByte((byte)Containers.GetInstance().GetFreeContainerSlot());
                send();
            }
        }

        public void SendUseItemWith(Position fromPos, ushort fromItemId, byte fromStackpos,
            Position toPos, ushort toItemid, byte toStackpos)
        {
            lock (serverSendMsg)
            {
                serverSendMsg.Reset();
                serverSendMsg.AddByte(0x83);
                serverSendMsg.AddPosition(fromPos);
                serverSendMsg.AddUInt16(fromItemId);
                serverSendMsg.AddByte(fromStackpos);
                serverSendMsg.AddPosition(toPos);
                serverSendMsg.AddUInt16(toItemid);
                serverSendMsg.AddByte(toStackpos);
                send();
            }
        }

        public void SendUseBattleWindow(Position pos, ushort itemId, byte stackPos, uint creatureId)
        {
            lock (serverSendMsg)
            {
                serverSendMsg.Reset();
                serverSendMsg.AddByte(0x84);
                serverSendMsg.AddPosition(pos);
                serverSendMsg.AddUInt16(itemId);
                serverSendMsg.AddByte(stackPos);
                serverSendMsg.AddUInt32(creatureId);
                send();
            }
        }

        //public void 

        #endregion

        #region Generic Functions
        private bool setMapDescription(NetworkMessage incMsg, NetworkMessage outMsg, int x, int y, int z, int width, int height)
        {
            int startz, endz, zstep;

            //calculate map limits
            if (z > 7)
            {
                startz = z - 2;
                endz = Math.Min(16 - 1, z + 2);
                zstep = 1;
            }
            else
            {
                startz = 7;
                endz = 0;
                zstep = -1;
            }

            for (int nz = startz; nz != endz + zstep; nz += zstep)
            {
                //pare each floor
                if (!setFloorDescription(incMsg, outMsg, x, y, nz, width, height, z - nz))
                {
                    Logger.Log(String.Format("Falha ao setar a descrição do mapa. Coordenadas({0}, {1}, {2})", x, y, nz), LogType.ERROR);
                    return false;
                }
            }

            return true;
        }

        private bool setFloorDescription(NetworkMessage incMsg, NetworkMessage outMsg, int x, int y, int z, int width, int height, int offset)
        {
            for (int nx = 0; nx < width; nx++)
            {
                for (int ny = 0; ny < height; ny++)
                {
                    if (skipTiles == 0)
                    {
                        ushort tileOpt = incMsg.PeekUInt16();
                        //Decide if we have to skip tiles
                        // or if it is a real tile
                        if (tileOpt >= 0xFF00)
                        {
                            ushort skip = incMsg.GetUInt16();
                            outMsg.AddUInt16(skip);
                            skipTiles = (ushort)(skip & 0xFF);
                        }
                        else
                        {
                            //real tile so read tile
                            Position pos = new Position((uint)(x + nx + offset), (uint)(y + ny + offset), (uint)z);
                            if (!setTileDescription(incMsg, outMsg, pos))
                            {
                                Logger.Log("Falha ao setar a descrição do tile. Posição: " + pos.ToString(), LogType.ERROR);
                                return false;
                            }

                            //read skip tiles info
                            ushort skip = incMsg.GetUInt16();
                            outMsg.AddUInt16(skip);
                            skipTiles = (ushort)(skip & 0xFF);
                        }
                    }
                    //skipping tiles...
                    else
                    {
                        skipTiles--;
                    }
                }
            }
            return true;
        }

        private bool setTileDescription(NetworkMessage incMsg, NetworkMessage outMsg, Position pos)
        {
            //set the tile in the map
            Tile tile = Map.GetInstance().SetTile(pos);

            if (tile == null)
                return false;

            //and clear it
            tile.Clear();

            int n = 0;
            while (true)
            {
                //avoid infinite loop
                n++;

                ushort inspectTileId = incMsg.PeekUInt16();

                if (inspectTileId >= 0xFF00)
                {
                    //end of the tile
                    //Notifications::onTileUpdate(pos);
                    return true;
                }
                else
                {
                    if (n > 10)
                    {
                        Logger.Log("Muitos objetos no tile. Posição: " + pos.ToString(), LogType.ERROR);
                        return false;
                    }

                    //read tile things: items and creatures
                    Thing thing = internalGetThing(incMsg, outMsg);

                    if (thing == null)
                    {
                        Logger.Log("Falha ao obter o objeto. Posição: " + pos.ToString(), LogType.ERROR);
                        return false;
                    }

                    //and add to the tile
                    if (!tile.AddThing(thing))
                    {
                        Logger.Log("Falha ao adicionar um objeto. Posição: " + pos.ToString(), LogType.ERROR);
                        return false;
                    }
                }
            }
        }

        private Thing internalGetThing(NetworkMessage incMsg, NetworkMessage outMsg)
        {
            //get thing type
            ushort thingId = incMsg.GetUInt16();
            outMsg.AddUInt16(thingId);

            if (thingId == 0x0061 || thingId == 0x0062)
            {
                //creatures
                Creature creature = null;

                if (thingId == 0x0062)
                { //creature is known
                    uint creatureID = incMsg.GetUInt32();
                    outMsg.AddUInt32(creatureID);
                    creature = Creatures.GetInstance().GetCreature(creatureID);
                }
                else if (thingId == 0x0061)
                {
                    //creature is not known
                    //perhaps we have to remove a known creature
                    uint removeID = incMsg.GetUInt32();
                    outMsg.AddUInt32(removeID);
                    Creatures.GetInstance().RemoveCreature(removeID);

                    //add a new creature
                    uint creatureID = incMsg.GetUInt32();
                    outMsg.AddUInt32(creatureID);
                    creature = Creatures.GetInstance().AddCreature(creatureID);

                    if (creature == null)
                        return null;

                    creature.SetName(incMsg.GetString());
                    outMsg.AddString(creature.GetName());
                }

                if (creature == null)
                    return null;

                //read creature properties
                creature.SetHealth(incMsg.GetByte());
                outMsg.AddByte(creature.GetHealth());
                if (creature.GetHealth() > 100)
                    return null;

                byte direction = incMsg.GetByte();
                outMsg.AddByte(direction);
                if (direction > 3)
                    return null;

                creature.SetTurnDirection((Direction)direction);

                creature.SetOutfit(incMsg.GetOutfit());
                outMsg.AddOutfit(creature.GetOutfit());

                //check if we can read 6 bytes
                if (!incMsg.CanRead(6))
                    return null;

                creature.SetLightLevel(incMsg.GetByte());
                outMsg.AddByte(creature.GetLightLevel());

                creature.SetLightColor(incMsg.GetByte());
                outMsg.AddByte(creature.GetLightColor());

                creature.SetSpeed(incMsg.GetUInt16());
                outMsg.AddUInt16(creature.GetSpeed());

                creature.SetSkull(incMsg.GetByte());
                outMsg.AddByte(creature.GetSkull());

                creature.SetShield(incMsg.GetByte());
                outMsg.AddByte(creature.GetShield());

                if (thingId == 0x0061)
                {
                    // emblem is sent only in packet type 0x61
                    creature.SetEmblem(incMsg.GetByte());
                    outMsg.AddByte(creature.GetEmblem());
                }

                byte impassable = incMsg.GetByte();
                creature.SetImpassable(impassable == 0x01);
                outMsg.AddByte(impassable);

                return creature;
            }
            else if (thingId == 0x0063)
            {
                //creature turn
                uint creatureID = incMsg.GetUInt32();
                outMsg.AddUInt32(creatureID);

                Creature creature = Creatures.GetInstance().GetCreature(creatureID);

                if (creature == null)
                    return null;

                //check if we can read 1 byte
                byte direction = incMsg.GetByte();
                outMsg.AddByte(direction);

                if (direction > 3)
                    return null;

                creature.SetTurnDirection((Direction)direction);

                return creature;
            }
            else
            {
                //item
                return internalGetItem(incMsg, outMsg, thingId);
            }
        }

        private Item internalGetItem(NetworkMessage incMsg, NetworkMessage outMsg, uint itemId)
        {
            if (itemId == 0xFFFFFFFF)
            {
                if (incMsg.CanRead(2))
                {
                    itemId = incMsg.GetUInt16();
                    outMsg.AddUInt16((ushort)itemId);
                }
                else
                    return null;
            }

            TibiaEzBot.Core.Entities.ObjectType it = Objects.GetInstance().GetItemType((ushort)itemId);

            if (it == null)
                return null;

            byte count = 0;
            if (it.IsStackable || it.IsSplash || it.IsFluidContainer || it.IsRune)
            {
                count = incMsg.GetByte();
                outMsg.AddByte(count);
            }

            return new Item((ushort)itemId, count, it);
        }

        private void getPlayerMemoryAddress()
        {
            while (GlobalVariables.GetPlayerMemoryAddress() == 0)
            {
                Thread.Sleep(100);
                for (uint i = Addresses.BattleList.Start; i < Addresses.BattleList.End; i += Addresses.BattleList.StepCreatures)
                {
                    if (Kernel.GetInstance().Client.Memory.ReadByte(i + Addresses.Creature.DistanceIsVisible) == 1)
                    {
                        if (Kernel.GetInstance().Client.Memory.ReadUInt32(i + Addresses.Creature.DistanceId) == GlobalVariables.GetPlayerId())
                        {
                            GlobalVariables.SetPlayerMemoryAddress(i);
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
