using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Constants;

namespace TibiaEzBot.Core.Entities
{
    public class Creature : Thing
    {
        private uint id;
        private String name;
        private Position position;
        private bool impassable;

        private ushort speed;
        private byte skull;
        private byte shield;
        private byte emblem;
        private Direction direction;
        private byte health;

        private Outfit outfit;

        private byte lightLevel;
        private byte lightColor;

        private SquareColor square;

        public Creature(uint id)
        {
            this.id = id;
        }

        public override uint GetId() { return id; }
        public override uint GetOrder() { return 4; }

        public bool IsPlayer() { return !IsMonster() && !IsNpc(); }
        public bool IsMonster() { return CreatureLists.AllCreatures.ContainsKey(name); }
        public bool IsNpc() { return  id > 0x40000000; }

        public String GetName() { return name; }
        public void SetName(String name) { this.name = name; }

        public Position GetPosition() { return position; }
        public void SetPosition(Position position) { this.position = position; }

        public bool IsImpassable() { return impassable; }
        public void SetImpassable(bool value) { this.impassable = value; }
		
		public bool IsReachable()
		{
			return PathFinder.GetInstance().FindPath(GlobalVariables.GetPlayerPosition(), GetPosition(), true);
		}

        public override String ToString()
        {
            return "Name: " + name;
        }


        public void SetTurnDirection(Direction direction)
        {
            this.direction = direction;
        }

        public void SetHealth(byte health)
        {
            this.health = health;
        }

        public byte GetHealth()
        {
            return health;
        }

        public void SetLightLevel(byte lightLevel)
        {
            this.lightLevel = lightLevel;
        }

        public byte GetLightLevel()
        {
            return lightLevel;
        }

        public void SetLightColor(byte lightColor)
        {
            this.lightColor = lightColor;
        }

        public byte GetLightColor()
        {
            return lightColor;
        }

        public void SetSpeed(ushort speed) { this.speed = speed; }

        public ushort GetSpeed()
        {
            return speed;
        }

        public void SetSkull(byte skull)
        {
            this.skull = skull;
        }

        public byte GetSkull()
        {
            return skull;
        }

        public void SetShield(byte shield)
        {
            this.shield = shield;
        }

        public byte GetShield()
        {
            return shield;
        }

        public void SetEmblem(byte emblem) { this.emblem = emblem; }
        public byte GetEmblem() { return emblem; }

        public Outfit GetOutfit() { return outfit; }
        public void SetOutfit(Outfit outfit) { this.outfit = outfit; }

        public void SetSquare(SquareColor color) { this.square = color; }
		public SquareColor GetSquare() { return this.square; }

		public override Creature GetCreature ()
		{
			return this;
		}

        public override bool Equals(object obj)
        {
            if (obj is Creature)
                return Equals((Creature)obj);

            return false;
        }

        public bool Equals(Creature creature)
        {
            return creature.GetId().Equals(GetId());
        }

        public override int GetHashCode()
        {
            return (int)GetId();
        }
    }
}
