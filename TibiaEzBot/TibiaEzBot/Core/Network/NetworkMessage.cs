using System;
using System.Text;
using TibiaEzBot.Core.Entities;

namespace TibiaEzBot.Core.Network
{
    public class NetworkMessage
    {
        #region Class Variables
        public static uint[] XteaKey;
        #endregion

        #region Instance Variables
        private byte[] buffer;
        private int position, length, bufferSize = 16394;
        #endregion

        #region Contructors
        
        public NetworkMessage()
        {
            buffer = new byte[bufferSize];
            position = GetPacketHeaderSize() + 2;
        }

        public NetworkMessage(int size)
        {
            bufferSize = size;
            buffer = new byte[bufferSize];
            position = GetPacketHeaderSize() + 2;
        }

        public NetworkMessage(byte[] data)
        {
            buffer = new byte[bufferSize];
            Array.Copy(data, buffer, data.Length);
            length = data.Length;
            position = 0;
        }

        public NetworkMessage(byte[] data, int length)
        {
            buffer = new byte[bufferSize];
            Array.Copy(data, buffer, length);
            this.length = length;
            position = 0;
        }

        public static NetworkMessage CreateUnencrypted()
        {
            NetworkMessage nm = new NetworkMessage();
            nm.Position = 0;
            return nm;
        }

        public static NetworkMessage CreateUnencrypted(int size)
        {
            NetworkMessage nm = new NetworkMessage(size);
            nm.Position = 0;
            return nm;
        }

        #endregion

        #region Properties

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public byte[] GetBuffer()
        {
            return buffer;
        }

        public byte[] Data
        {
            get
            {
                byte[] t = new byte[length];
                Array.Copy(buffer, t, length);
                return t;
            }
        }

        #endregion

        #region Packer Header

        public void InsertPacketHeader()
        {
            AddPacketHeader((ushort)(length - 2));
        }

        public int GetPacketHeaderSize()
        {
        	return 6;
        }

        #endregion

        #region Get

        public byte GetByte()
        {
            if (position + 1 > length)
                throw new Exception("NetworkMessage try to get more bytes from a smaller buffer");

            return buffer[position++];
        }

        public byte[] GetBytes(int count)
        {
            if (position + count > length)
                throw new Exception("NetworkMessage try to get more bytes from a smaller buffer");

            byte[] t = new byte[count];
            Array.Copy(buffer, position, t, 0, count);
            position += count;
            return t;
        }

        public string GetString()
        {
            int len = (int)GetUInt16();
            string t = System.Text.ASCIIEncoding.Default.GetString(buffer, position, len);
            position += len;
            return t;
        }

        public ushort GetUInt16()
        {
            return BitConverter.ToUInt16(GetBytes(2), 0);
        }

        public uint GetUInt32()
        {
            return BitConverter.ToUInt32(GetBytes(4), 0);
        }

        public Position GetPosition()
        {
            return new Position(GetUInt16(), GetUInt16(), GetByte());
        }

        private ushort GetPacketHeader()
        {
            return BitConverter.ToUInt16(buffer, 0);
        }

        public Outfit GetOutfit()
        {
            byte head, body, legs, feet, addons;
            ushort looktype = GetUInt16();

            if (looktype != 0)
            {
                head = GetByte();
                body = GetByte();
                legs = GetByte();
                feet = GetByte();
                addons = GetByte();

                return new Outfit(looktype, head, body, legs, feet, addons);
            }
            else
                return new Outfit(looktype, GetUInt16());
        }

        #endregion

        #region Add

        public void AddByte(byte value)
        {
            if (1 + length > bufferSize)
                throw new Exception("NetworkMessage buffer is full.");

            AddBytes(new byte[] { value });
        }

        public void AddBytes(byte[] value)
        {
            if (value.Length + length > bufferSize)
                throw new Exception("NetworkMessage buffer is full.");

            Array.Copy(value, 0, buffer, position, value.Length);
            position += value.Length;

            if (position > length)
                length = position;
        }

        public void AddString(string value)
        {
            AddUInt16((ushort)value.Length);
            AddBytes(System.Text.ASCIIEncoding.Default.GetBytes(value));
        }

        public void AddUInt16(ushort value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }

        public void AddUInt32(uint value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }

        public void AddPosition(Entities.Position pos)
        {
            AddUInt16((ushort)pos.X);
            AddUInt16((ushort)pos.Y);
            AddByte((byte)pos.Z);
        }

        public void AddOutfit(Outfit outfit)
        {
            AddBytes(outfit.ToByteArray());
        }

        public void AddPaddingBytes(int count)
        {
            position += count;

            if (position > length)
                length = position;
        }

        private void AddPacketHeader(ushort value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 2);
        }

        //public void AddItem(Item item)
        //{
        //    AddUInt16((ushort)item.Id);

        //    if (item.HasExtraByte)
        //        AddByte(item.Count);
        //}

        #endregion

        #region Peek

        public byte PeekByte()
        {
            return buffer[position];
        }

        public byte[] PeekBytes(int count)
        {
            byte[] t = new byte[count];
            Array.Copy(buffer, position, t, 0, count);
            return t;
        }

        public ushort PeekUInt16()
        {
            return BitConverter.ToUInt16(PeekBytes(2), 0);
        }

        public uint PeekUInt32()
        {
            return BitConverter.ToUInt32(PeekBytes(4), 0);
        }

        public string PeekString()
        {
            int len = (int)PeekUInt16();
            return System.Text.ASCIIEncoding.ASCII.GetString(PeekBytes(len + 2), 2, len);
        }

        #endregion

        #region Replace

        public void ReplaceBytes(int index, byte[] value)
        {
            if (length - index >= value.Length)
                Array.Copy(value, 0, buffer, index, value.Length);
        }

        #endregion

        #region Other Functions

        public bool CanRead(int value)
        {
            return position + value <= length;
        }

        public void Reset()
        {
            position = (GetPacketHeaderSize() + 2);
            length = (GetPacketHeaderSize() + 2);
        }

        public bool PrepareToSend()
        {
            return PrepareToSend(NetworkMessage.XteaKey);
        }

        public bool PrepareToSend(uint[] xteaKey)
        {
            if (!XteaEncrypt(xteaKey))
                return false;


            AddAdler32();
            InsertPacketHeader();

            return true;
        }

        public bool PrepareToRead()
        {
            return PrepareToRead(NetworkMessage.XteaKey);
        }

        public bool PrepareToRead(uint[] xteaKey)
        {
            if (!XteaDecrypt(xteaKey))
                return false;

            position = GetPacketHeaderSize();
            return true;
        }

        public void InsetLogicalPacketHeader()
        {
            Array.Copy(BitConverter.GetBytes((ushort)length - (GetPacketHeaderSize() + 2)), 0, buffer, GetPacketHeaderSize(), 2);
        }

        public void UpdateLogicalPacketHeader()
        {
            Array.Copy(BitConverter.GetBytes((ushort)(length - (GetPacketHeaderSize() + 2))), 0, buffer, GetPacketHeaderSize(), 2);
        }

        #endregion

        #region Rsa Wrappers

        public bool RsaOTEncrypt(int pos)
        {
            return Rsa.RsaOTEncrypt(ref buffer, pos);
        }

        public bool RsaCipEncrypt(int pos)
        {
            return Rsa.RsaCipEncrypt(ref buffer, pos);
        }

        public bool RsaOTDecrypt()
        {
            return Rsa.RsaOTDecrypt(ref buffer, position, length);
        }

        #endregion

        #region Adler32 Wrappers

        public bool CheckAdler32()
        {
            if (AdlerChecksum.Generate(ref buffer, 6, length) != GetAdler32())
                return false;

            return true;
        }

        public void AddAdler32()
        {
            Array.Copy(BitConverter.GetBytes(AdlerChecksum.Generate(ref buffer, 6, length)), 0, buffer, 2, 4);
        }

        private uint GetAdler32()
        {
            return BitConverter.ToUInt32(buffer, 2);
        }

        #endregion

        #region Xtea Wrappers

        public bool XteaDecrypt()
        {
            return XteaDecrypt(NetworkMessage.XteaKey);
        }

        public bool XteaDecrypt(uint[] key)
        {
            return Xtea.XteaDecrypt(ref buffer, ref length, GetPacketHeaderSize(), key);
        }

        public bool XteaEncrypt()
        {
            return XteaEncrypt(NetworkMessage.XteaKey);
        }

        public bool XteaEncrypt(uint[] key)
        {
            return Xtea.XteaEncrypt(ref buffer, ref length, GetPacketHeaderSize(), key);
        }

        #endregion
    }
}
