using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Map
    {
        #region Singleton
        private static Map instance;

        public static Map GetInstance()
        {
            if (instance == null) { instance = new Map(); }
            return instance;
        }
        #endregion

        private IDictionary<ulong, ushort> coordinates;
        private IList<uint> freeTiles;
        private Tile[] tiles;

        private Map()
        {
            tiles = new Tile[4096];
            coordinates = new Dictionary<ulong, ushort>();
            freeTiles = new List<uint>();

            for (uint i = 0; i < 4096; ++i)
            {
                freeTiles.Add(i);
            }
        }

        public Tile SetTile(uint x, uint y, uint z)
        {
            Tile tile = GetTile(x, y, z);

            if (tile == null)
            {
                if (freeTiles.Count == 0)
                {
                    uint freedTiles = 0;
                    IList<ulong> eraseKeys = new List<ulong>();

                    foreach (KeyValuePair<ulong, ushort> keyPair in coordinates)
                    {
                        Position pos = IndexToPosition(keyPair.Key);

                        if (!PlayerCanSee(pos.X, pos.Y, pos.Z))
                        {
                            freeTiles.Add(keyPair.Value);
                            eraseKeys.Add(keyPair.Key);

                            if (++freedTiles > 384)
                                break;
                        }

                    }

                    foreach (ulong key in eraseKeys)
                    {
                        coordinates.Remove(key);
                    }

                    if (freedTiles == 0)
                    {
                        Logger.Log("Nenhum tile disponivel.", LogType.ERROR);
                        return null;
                    }
                }

                uint freeTile = freeTiles[0];
                freeTiles.RemoveAt(0);

                if (tiles[freeTile] == null)
                    tiles[freeTile] = new Tile();


                coordinates.Add(PositionToIndex(x, y, z), (ushort)freeTile);
                tiles[freeTile].SetPosition(new Position(x, y, z));
                return tiles[freeTile];
            }
            else
            {
                return tile;
            }
        }

        public Tile SetTile(Position position) { return SetTile(position.X, position.Y, position.Z); }

        public Tile GetTile(uint x, uint y, uint z)
        {
            ulong posIndex = PositionToIndex(x, y, z);

            if (coordinates.ContainsKey(posIndex))
            {
                return tiles[coordinates[posIndex]];
            }

            return null;
        }

        public Tile GetTile(Position position) { return GetTile(position.X, position.Y, position.Z); }

        public bool PlayerCanSee(uint x, uint y, uint z)
        {
            Position playerPos = GlobalVariables.GetPlayerPosition();
            if (playerPos.Z <= 7)
            {
                //we are on ground level or above (7 -> 0)
                //view is from 7 -> 0
                if (z > 7)
                {
                    return false;
                }
            }
            else if (playerPos.Z >= 8)
            {
                //we are underground (8 -> 15)
                //view is +/- 2 from the floor we stand on
                if (Math.Abs((int)playerPos.Z - z) > 2)
                {
                    return false;
                }
            }

            //negative offset means that the action taken place is on a lower floor than ourself
            int offsetz = (int)(playerPos.Z - z);

            if ((x >= (int)playerPos.X - 9 + offsetz) && (x <= (int)playerPos.X + 10 + offsetz) &&
                (y >= (int)playerPos.Y - 7 + offsetz) && (y <= (int)playerPos.Y + 8 + offsetz))
            {
                return true;
            }

            return false;

        }

        private ulong PositionToIndex(uint x, uint y, uint z)
        {
            return ((ulong)(x & 0xFFFF) << 24 | (y & 0xFFFF) << 8 | (z & 0xFF));
        }

        public ulong PositionToIndex(Position pos)
        {
            return PositionToIndex(pos.X, pos.Y, pos.Z);
        }

        private Position IndexToPosition(ulong index)
        {
            Position pos = new Position();
            pos.X = (uint)(index >> 24) & 0xFFFF;
            pos.Y = (uint)(index >> 8) & 0xFFFF;
            pos.Z = (uint)(index & 0xFF);
            return pos;
        }

        public void Clear()
        {
            Logger.Log("Limpando mapa.");

            for (uint i = 0; i < 4096; i++)
            {
                if (tiles[i] != null)
                    tiles[i].Clear();
            }

            coordinates.Clear();
            freeTiles.Clear();

            for (uint i = 0; i < 4096; ++i)
            {
                freeTiles.Add(i);
            }
        }
    }
}
