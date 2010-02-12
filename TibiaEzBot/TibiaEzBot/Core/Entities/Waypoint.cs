using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TibiaEzBot.Core.Entities
{
    public enum WaypointType : int
    {
        WAYPOINT_GROUND,
        WAYPOINT_HOLE,
        WAYPOINT_ROPE,
        WAYPOINT_LADDER,
        WAYPOINT_RAMP,
        WAYPOINT_STAIR_UP,
        WAYPOINT_START_DOWN,
        WAYPOINT_DELAY,
        WAYPOINT_SAY
    }

    public enum SayType : byte
    {
        SAY_NORMAL = 0x01,
        SAY_WHISPER = 0x02,
        SAY_YELL = 0x03,
        SAY_NPC = 0x04,
    }

    public class Waypoint
    {
        public WaypointType WaypointType { get; set; }
        public String Description { get { return ToString(); } }

        public Waypoint(WaypointType waypointType)
        {
            WaypointType = waypointType;
        }

        public override string ToString()
        {
            return GetWaypointTypeDescription(WaypointType);
        }

        public virtual WalkWaypoint GetWalkWaypoint() { return null; }
        public virtual WaitWaypoint GetWaitWaypoint() { return null; }
        public virtual SayWaypoint GetSayWaypoint() { return null; }

        protected string GetWaypointTypeDescription(WaypointType type)
        {
            switch (type)
            {
                case WaypointType.WAYPOINT_DELAY:
                    return "Delay";
                case WaypointType.WAYPOINT_GROUND:
                    return "Ground";
                case WaypointType.WAYPOINT_HOLE:
                    return "Hole";
                case WaypointType.WAYPOINT_RAMP:
                    return "Ramp";
                case WaypointType.WAYPOINT_LADDER:
                    return "Ladder";
                case WaypointType.WAYPOINT_ROPE:
                    return "Rope";
                case WaypointType.WAYPOINT_STAIR_UP:
                    return "Stair Up";
                case WaypointType.WAYPOINT_START_DOWN:
                    return "Stair Down";
                case WaypointType.WAYPOINT_SAY:
                    return "Say";
                default:
                    return "Unk";
            }
        }
    }

    public class WaitWaypoint : Waypoint
    {
        public int Delay { get; set; }

        public WaitWaypoint(int delay)
            : base(WaypointType.WAYPOINT_DELAY)
        {
            Delay = delay;
        }

        public override WaitWaypoint GetWaitWaypoint()
        {
            return this;
        }

        public override string ToString()
        {
            return base.ToString() + " " + Delay;
        }
    }

    public class WalkWaypoint : Waypoint
    {
        public Position Position { get; set; }

        public WalkWaypoint(Position pos, WaypointType type)
            : base(type)
        {
            Position = pos;
        }

        public override WalkWaypoint GetWalkWaypoint()
        {
            return this;
        }

        public override string ToString()
        {
            return base.ToString() + " " + Position;
        }
    }

    public class SayWaypoint : Waypoint
    {
        public String Words { get; set; }
        public SayType SayType { get; set; }

        public SayWaypoint(String words, SayType type)
            : base(WaypointType.WAYPOINT_SAY)
        {
            Words = words;
            SayType = type;
        }

        public override SayWaypoint GetSayWaypoint()
        {
            return this;
        }

        public String GetSayTypeDescription(SayType type)
        {
            switch (type)
            {
                case SayType.SAY_NORMAL:
                    return "Normal";
                case SayType.SAY_NPC:
                    return "NPC";
                case SayType.SAY_WHISPER:
                    return "Whisper";
                case SayType.SAY_YELL:
                    return "Yell";
                default:
                    return "Unk";
            }
        }

        public override string ToString()
        {
            return base.ToString() + "[" + GetSayTypeDescription(SayType) + "] " + Words;
        }

    }
}
