using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TibiaEzBot.Core.Entities
{
    public class Objects
    {
        #region Singleton
        private static Objects instance;

        public static Objects GetInstance()
        {
            if (instance == null) { instance = new Objects(); }
            return instance;
        }
        #endregion

        private static ushort minItemId;
        private static ushort maxItemId;
        private static ushort minOutfitId;
        private static ushort maxOutfitId;
        private static ushort minEffectId;
        private static ushort maxEffectId;
        private static ushort minDistanceId;
        private static ushort maxDistanceId;

        public static ushort MinItemId
        {
            get { return Objects.minItemId; }
            set { Objects.minItemId = value; }
        }

        public static ushort MaxItemId
        {
            get { return Objects.maxItemId; }
            set { Objects.maxItemId = value; }
        }

        public static ushort MinOutfitId
        {
            get { return Objects.minOutfitId; }
            set { Objects.minOutfitId = value; }
        }

        public static ushort MaxOutfitId
        {
            get { return Objects.maxOutfitId; }
            set { Objects.maxOutfitId = value; }
        }

        public static ushort MinEffectId
        {
            get { return Objects.minEffectId; }
            set { Objects.minEffectId = value; }
        }

        public static ushort MaxEffectId
        {
            get { return Objects.maxEffectId; }
            set { Objects.maxEffectId = value; }
        }

        public static ushort MinDistanceId
        {
            get { return Objects.minDistanceId; }
            set { Objects.minDistanceId = value; }
        }

        public static ushort maxdistanceid
        {
            get { return Objects.maxDistanceId; }
            set { Objects.maxDistanceId = value; }
        }

        private IDictionary<ushort, ObjectType> items;
        private IDictionary<ushort, ObjectType> outfits;
        private IDictionary<ushort, ObjectType> effects;
        private IDictionary<ushort, ObjectType> distances;

        private bool datLoaded;

        private Objects()
        {
            items = new Dictionary<ushort, ObjectType>();
            outfits = new Dictionary<ushort, ObjectType>();
            effects = new Dictionary<ushort, ObjectType>();
            distances = new Dictionary<ushort, ObjectType>();
        }

        public ObjectType GetItemType(ushort id)
        {
            return items.ContainsKey(id) ? items[id] : null;
        }

        public ObjectType GetOutfitType(ushort id)
        {
            return outfits.ContainsKey(id) ? outfits[id] : null;
        }

        public ObjectType GetEffectType(ushort id)
        {
            return effects.ContainsKey(id) ? effects[id] : null;
        }

        public ObjectType GetDistanceType(ushort id)
        {
            return distances.ContainsKey(id) ? distances[id] : null;
        }

        public bool LoadDat(String fileName)
        {
            if (datLoaded || String.IsNullOrEmpty(fileName)) { return false; }

            Logger.Log("Carregando Tibia.dat.");

            FileStream fileStream = File.OpenRead(fileName);
            long size = fileStream.Length;
            BinaryReader binaryReader = new BinaryReader(fileStream);
            binaryReader.ReadInt32();

            uint maxObjects = 0;

            ushort id = 100;
            Objects.minItemId = 100;
            Objects.maxItemId = binaryReader.ReadUInt16();
            maxObjects += Objects.maxItemId;

            Objects.minOutfitId = 0;
            Objects.maxOutfitId = binaryReader.ReadUInt16();
            maxObjects += Objects.maxOutfitId;

            Objects.minEffectId = 0;
            Objects.maxEffectId = binaryReader.ReadUInt16();
            maxObjects += Objects.maxEffectId;

            Objects.minDistanceId = 0;
            Objects.maxDistanceId = binaryReader.ReadUInt16();
            maxObjects += Objects.maxDistanceId;

            while (fileStream.Position < size && id <= maxObjects)
            {
                ObjectType oType = new ObjectType(id);

                int optbyte;
                while (((optbyte = binaryReader.ReadByte()) >= 0) && (optbyte != 0xFF))
                {
                    switch (optbyte)
                    {
                        case 0x00: //Ground tile
                            oType.Speed = binaryReader.ReadUInt16();
                            oType.IsGround = true;
                            oType.AlwaysOnTopOrder = 0;
                            break;
                        case 0x01: //ontop
                            oType.IsAlwaysOnTop = true;
                            oType.AlwaysOnTopOrder = 1;
                            break;
                        case 0x02: //Walk through (doors etc)
                            oType.IsAlwaysOnTop = true;
                            oType.AlwaysOnTopOrder = 2;
                            break;
                        case 0x03: //Can walk trough (arces)
                            oType.IsAlwaysOnTop = true;
                            oType.AlwaysOnTopOrder = 3;
                            break;
                        case 0x04: //Container
                            oType.IsContainer = true;
                            break;
                        case 0x05: //Stackable
                            oType.IsStackable = true;
                            break;
                        case 0x06: //Ladders?
                            oType.IsAlwaysUsed = true;
                            break;
                        case 0x07: //Useable
                            oType.IsUseable = true;
                            break;
                        case 0x08: //Runes
                            oType.IsRune = true;
                            break;
                        case 0x09: //Writtable/Readable Objectss
                            oType.IsReadable = true;
                            binaryReader.ReadUInt16(); //maximum size of text entry TODO (ivucica#3#) store this data
                            break;
                        case 0x0A: //Writtable Objectss that can't be edited
                            oType.IsReadable = true;
                            binaryReader.ReadUInt16(); //maximum size of text entry TODO (ivucica#3#) store this data
                            break;
                        case 0x0B: //Fluid containers
                            oType.IsFluidContainer = true;
                            break;
                        case 0x0C: //Splashes?
                            oType.IsSplash = true;
                            break;
                        case 0x0D: //Is blocking
                            oType.IsBlockSolid = true;
                            break;
                        case 0x0E: //Is not moveable
                            oType.IsMoveable = false;
                            break;
                        case 0x0F: //Blocks missiles (walls, magic wall etc)
                            oType.CanBlockProjectTile = true;
                            break;
                        case 0x10: //Blocks monster movement (flowers, parcels etc)
                            oType.CanBlockPathFind = true;
                            break;
                        case 0x11: //Can be equipped
                            oType.IsPickupable = true;
                            break;
                        case 0x12: //Wall items
                            oType.IsHangable = true;
                            break;
                        case 0x13:
                            oType.IsHorizontal = true;
                            break;
                        case 0x14:
                            oType.IsVertical = true;
                            break;
                        case 0x15: //Rotatable items
                            oType.IsRotatable = true;
                            break;
                        case 0x16: //Light info
                            oType.LightLevel = binaryReader.ReadUInt16();
                            oType.LightColor = binaryReader.ReadUInt16();
                            break;
                        case 0x17:  //Floor change?
                            break;
                        case 0x18: //??
                            //optbyte = optbyte;
                            break;
                        case 0x19: //Offset?
                            oType.XOffset = binaryReader.ReadUInt16();
                            oType.YOffset = binaryReader.ReadUInt16();
                            break;
                        case 0x1A:
                            oType.HasHeight = true;
                            // (should be) the height change in px; Tibia always uses 8
                            binaryReader.ReadUInt16();
                            break;
                        case 0x1B://draw with height offset for all parts (2x2) of the sprite
                            break;
                        case 0x1C://some monsters
                            break;
                        case 0x1D:
                            oType.MapColor = binaryReader.ReadUInt16();
                            break;
                        case 0x1E:  //line spot
                            int tmp = binaryReader.ReadByte(); // 86 -> openable holes, 77-> can be used to go down, 76 can be used to go up, 82 -> stairs up, 79 switch,
                            if (tmp == 0x58)
                                oType.IsReadable = true;

                            binaryReader.ReadByte(); // always 4
                            break;
                        case 0x1F: //?
                            break;
                        case 0x20: // New with Tibia 8.5 - "look through"
                            // NOTE (nfries88): Not sure if client or server does this...
                            // NOTE (ivucica): It's almost certainly the client
                            oType.CanLookThrough = true;
                            break;
                        default:
                            return false;
                    }
                }

                oType.Width = binaryReader.ReadByte();
                oType.Height = binaryReader.ReadByte();

                if ((oType.Width > 1) || (oType.Height > 1))
                {
                    binaryReader.ReadByte();
                }

                oType.BlendFrames = binaryReader.ReadByte();
                oType.Xdiv = binaryReader.ReadByte();
                oType.Ydiv = binaryReader.ReadByte();
                oType.Unk1 = binaryReader.ReadByte();
                oType.AnimationCount = binaryReader.ReadByte();

                oType.SpriteCount = (ushort)(oType.Width * oType.Height * oType.BlendFrames *
                    oType.Xdiv * oType.Ydiv * oType.AnimationCount * oType.Unk1);

                //oType->imageData = new uint16_t[oType->numsprites];
                //ASSERT(oType->imageData);

                for (uint i = 0; i < oType.SpriteCount; i++)
                {
                    binaryReader.ReadUInt16();
                }

                if (id <= Objects.maxItemId)
                {
                    items.Add(id, oType);
                }
                else if (id <= (Objects.maxItemId + Objects.maxOutfitId))
                {
                    outfits.Add((ushort)(id - Objects.maxItemId), oType);
                }
                else if (id <= (Objects.maxItemId + Objects.maxOutfitId + Objects.maxEffectId))
                {
                    effects.Add((ushort)(id - Objects.maxItemId - Objects.maxOutfitId), oType);
                }
                else if (id <= (Objects.maxItemId + Objects.maxOutfitId + Objects.maxEffectId + Objects.maxDistanceId))
                {
                    distances.Add((ushort)(id - Objects.maxItemId - Objects.maxOutfitId - Objects.maxEffectId), oType);
                }

                id++;
            }

            binaryReader.Close();
            datLoaded = true;

            return true;
        }
    }
}
