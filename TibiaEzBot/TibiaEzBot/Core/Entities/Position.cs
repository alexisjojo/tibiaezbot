using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public class Position
    {
    	public uint X, Y, Z;
    	
    	public Position() { }
    	public Position(uint x, uint y, uint z)
    	{
    		X = x;
    		Y = y;
    		Z = z;
    	}
		
		public bool IsAdjacentTo(Position pos)
        {

            return pos.Z == Z && Math.Max(Math.Abs(X - pos.X), Math.Abs(Y - pos.Y)) <= 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position)
                return Equals((Position)obj);

            return false;
        }

        public bool Equals(Position pos)
        {
            return pos != null && pos.X == X && pos.Y == Y && pos.Z == Z;
        }

        public Position Clone()
        {
            return new Position(X, Y, Z);
        }

        public override int GetHashCode()
        {
            return (int)(X + Y + Z);
        }
    	
    	public override String ToString()
    	{
    		return String.Format("X: {0}, Y: {1}, Z: {2}", X, Y, Z);
    	}
    }
}
