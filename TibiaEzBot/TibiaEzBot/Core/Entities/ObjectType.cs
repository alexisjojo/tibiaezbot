using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class ObjectType
    {
        public ushort Id { get; set; }

        public bool IsGround { get; set; }
        public ushort Speed { get; set; }
        public ushort AlwaysOnTopOrder { get; set; }
        public bool IsAlwaysOnTop { get; set; }
        public bool IsContainer { get; set; }
        public bool IsStackable { get; set; }
        public bool IsUseable { get; set; }
        public bool IsRune { get; set; }
        public bool IsReadable { get; set; }
        public bool IsFluidContainer { get; set; }
        public bool IsSplash { get; set; }
        public bool IsBlockSolid { get; set; }
        public bool IsMoveable { get; set; }
        public bool CanBlockProjectTile { get; set; }
        public bool CanBlockPathFind { get; set; }
        public bool IsPickupable { get; set; }
        public bool IsHangable { get; set; }
        public bool IsHorizontal { get; set; }
        public bool IsVertical { get; set; }
        public bool IsRotatable { get; set; }
        //items with 0x06 property
        public bool IsAlwaysUsed { get; set; }
        public ushort LightLevel { get; set; }
        public ushort LightColor { get; set; }
        public ushort XOffset { get; set; }
        public ushort YOffset { get; set; }
        public bool HasHeight { get; set; }
        public ushort MapColor { get; set; }
        public bool CanLookThrough { get; set; }

        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort BlendFrames { get; set; }
        public ushort Xdiv { get; set; }
        public ushort Ydiv { get; set; }
        public ushort Unk1 { get; set; }
        public ushort AnimationCount { get; set; }
        public ushort SpriteCount { get; set; }
        //uint16_t* imageData;

        public ObjectType(ushort id) 
        { 
            this.Id = id;
            AlwaysOnTopOrder = 5;
            Width = 1;
            Height = 1;
            Xdiv = 1;
            Ydiv = 1;
            AnimationCount = 1;
        }
    }
}
