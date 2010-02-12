﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Outfit
    {
        private ushort lookType;
        private ushort lookItem;
        private byte head;
        private byte body;
        private byte legs;
        private byte feet;
        private byte addons;

        public ushort LookType
        {
            get { return lookType; }
            set { lookType = value; }
        }
        public ushort LookItem
        {
            get { return lookItem; }
            set { lookItem = value; }
        }
        public byte Head
        {
            get { return head; }
            set { head = value; }
        }
        public byte Body
        {
            get { return body; }
            set { body = value; }
        }
        public byte Legs
        {
            get { return legs; }
            set { legs = value; }
        }
        public byte Feet
        {
            get { return feet; }
            set { feet = value; }
        }
        public byte Addons
        {
            get { return addons; }
            set { addons = value; }
        }

        public Outfit(ushort looktype, ushort lookitem)
        {
            this.lookItem = lookitem;
            this.lookType = looktype;
        }

        public Outfit(ushort looktype, byte head, byte body, byte legs, byte feet, byte addons)
        {
            this.lookType = looktype;
            this.head = head;
            this.body = body;
            this.legs = legs;
            this.feet = feet;
            this.addons = addons;
        }

        public byte[] ToByteArray()
        {
            byte[] temp;

            if (LookType != 0)
            {
                temp = new byte[7];
                Array.Copy(BitConverter.GetBytes(LookType), temp, 2);
                temp[2] = Head;
                temp[3] = Body;
                temp[4] = Legs;
                temp[5] = Feet;
                temp[6] = Addons;
            }
            else
            {
                temp = new byte[4];
                Array.Copy(BitConverter.GetBytes(LookType), temp, 2);
                Array.Copy(BitConverter.GetBytes(LookItem), 0, temp, 2, 2);
            }

            return temp;
        }

        public override string ToString()
        {
            return "LookType: " + LookType.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Outfit)
                return Equals((Outfit)obj);

            return false;
        }

        public bool Equals(Outfit outfit)
        {
            return LookType == outfit.LookType && Head == outfit.Head && Body == outfit.Body
                && Legs == outfit.Legs && Feet == outfit.Feet && Addons == outfit.Addons ||
                LookType == outfit.LookType && LookItem == outfit.LookItem;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
