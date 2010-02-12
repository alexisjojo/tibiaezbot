using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Item : Thing
    {
        private ushort id;
        private byte count;
        private ObjectType itemType;

        public Item(ushort id, byte count, ObjectType itemType)
        {
            this.id = id;
            this.count = count;
            this.itemType = itemType == null ? Objects.GetInstance().GetItemType(id) : itemType;
        }

		public Item(ushort id, byte count) : this(id, count, null) { }
        public Item(ushort id) : this(id, 0) { }

        public override uint GetId() { return id; }
        public override uint GetOrder() { return GetAlwaysOnTopLevel(); }

        public byte GetCount() { return count; }
        public void SetCount(byte count) { this.count = count; }

        public bool IsAlwaysOnTop() { return itemType.IsAlwaysOnTop; }
        public uint GetAlwaysOnTopLevel() { return itemType.AlwaysOnTopOrder; }
        public bool IsGroundTile() { return itemType.IsGround; }

        public bool IsRune() { return itemType.IsRune; }
        public bool IsBlocking() { return itemType.IsBlockSolid; }
        public bool IsStackable() { return itemType.IsStackable; }
        public bool IsSplash() { return itemType.IsSplash; }
        public bool IsFluidContainer() { return itemType.IsFluidContainer; }
        public bool IsExtendedUseable() { return itemType.IsUseable; }
        public bool IsAlwaysUsed() { return itemType.IsAlwaysUsed; }
		
		public override byte GetMapColor ()
		{
			return (byte)itemType.MapColor;
		}

        public ObjectType GetObjectType() { return itemType; }

        public bool HasHeight() { return itemType.HasHeight; }

		public override Item GetItem ()
		{
			return this;
		}
		
        public override String ToString()
        {
            return String.Format("Id: {0}, Count: {1}", id, count);
        }
    }
}
