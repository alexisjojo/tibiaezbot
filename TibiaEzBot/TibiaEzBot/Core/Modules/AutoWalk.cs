using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Entities;
using System.Collections.ObjectModel;
using TibiaEzBot.Core.Util;
using TibiaEzBot.Core.Constants;

namespace TibiaEzBot.Core.Modules
{
    public class AutoWalk : Module
    {
        private bool enable;
        private DateTime delayStart;
        private bool delayStartSet;

        public event EventHandler CurrentWaypointChanged;

        public int CurrentWaypoint { get; set; }
        public bool Enable
        {
            get { return enable; }
            set
            {
                if (Waypoints.Count > 0)
                    enable = value;
                else
                    enable = false;
            }
        }
        public ObservableCollection<Waypoint> Waypoints { get; set; }
        public bool UseElvenhairRope { get; set; }
        public bool UseLightShovel { get; set; }
        public String WaypointsFile { get; set; }

        public override void Initialize()
        {
            Waypoints = new ObservableCollection<Waypoint>();
        }

        private Waypoint GetCurrentWaypoint()
        {
            return Waypoints[CurrentWaypoint];
        }

        private void IncrementWaypoint()
        {
            CurrentWaypoint++;

            if (CurrentWaypoint >= Waypoints.Count)
                CurrentWaypoint = 0;

            if (CurrentWaypointChanged != null)
                CurrentWaypointChanged.BeginInvoke(this, new EventArgs(), null, null);
        }

        public override void Run()
        {
            if (Enable && !GlobalVariables.IsWalking() && !Kernel.GetInstance().AutoLoot.IsLooting)
            {
                Waypoint waypoint = GetCurrentWaypoint();

                switch (waypoint.WaypointType)
                {
                    case WaypointType.WAYPOINT_GROUND:
                        if (GlobalVariables.GetPlayerPosition().IsAdjacentTo(waypoint.GetWalkWaypoint().Position))
                            IncrementWaypoint();
                        else
                        {
                            Game.GetInstance().Walk(waypoint.GetWalkWaypoint().Position);
                        }
                        break;
                    case WaypointType.WAYPOINT_RAMP:
                    case WaypointType.WAYPOINT_HOLE:
                    case WaypointType.WAYPOINT_STAIR_UP:
                    case WaypointType.WAYPOINT_START_DOWN:
                        if (GlobalVariables.GetPlayerPosition().Z != waypoint.GetWalkWaypoint().Position.Z)
                            IncrementWaypoint();
                        else
                        {
                            Game.GetInstance().Walk(waypoint.GetWalkWaypoint().Position);
                        }
                        break;
                    case WaypointType.WAYPOINT_ROPE:
                        if (GlobalVariables.GetPlayerPosition().Z != waypoint.GetWalkWaypoint().Position.Z)
                            IncrementWaypoint();
                        else if (GlobalVariables.GetPlayerPosition().IsAdjacentTo(waypoint.GetWalkWaypoint().Position))
                        {
                            if (UseElvenhairRope)
                                Game.GetInstance().UseItemOn(646, waypoint.GetWalkWaypoint().Position);
                            else
                                Game.GetInstance().UseItemOn(3003, waypoint.GetWalkWaypoint().Position);
                        }
                        else
                        {
                            Game.GetInstance().Walk(waypoint.GetWalkWaypoint().Position);
                        }
                        break;
                    case WaypointType.WAYPOINT_LADDER:
                        if (GlobalVariables.GetPlayerPosition().Z != waypoint.GetWalkWaypoint().Position.Z)
                            IncrementWaypoint();
                        else if (GlobalVariables.GetPlayerPosition().IsAdjacentTo(waypoint.GetWalkWaypoint().Position))
                        {
                            Game.GetInstance().UseItem(waypoint.GetWalkWaypoint().Position);
                            IncrementWaypoint();
                        }
                        else
                        {
                            Game.GetInstance().Walk(waypoint.GetWalkWaypoint().Position);
                        }
                        break;
                    case WaypointType.WAYPOINT_DELAY:
                        if (delayStartSet && (DateTime.UtcNow - delayStart).TotalMilliseconds >= waypoint.GetWaitWaypoint().Delay)
                        {
                            delayStartSet = false;
                            IncrementWaypoint();
                        }
                        else if (!delayStartSet)
                        {
                            delayStart = DateTime.UtcNow;
                            delayStartSet = true;
                        }

                        break;
                    case WaypointType.WAYPOINT_SAY:
                        if (Kernel.GetInstance().WorldProtocol != null)
                        {
                            Kernel.GetInstance().WorldProtocol.SendSay((TibiaEzBot.Core.Constants.SpeechType)
                                waypoint.GetSayWaypoint().SayType, waypoint.GetSayWaypoint().Words);
                            IncrementWaypoint();
                        }
                        break;
                }


            }
        }

        public void AddWaypoint(WaypointType waypointType)
        {
            if (!Enable && GlobalVariables.IsConnected())
            {
                Position pos = GlobalVariables.GetPlayerPosition().Clone();
                switch (waypointType)
                {
                    case WaypointType.WAYPOINT_GROUND:
                    case WaypointType.WAYPOINT_ROPE:
                    case WaypointType.WAYPOINT_LADDER:
                        Waypoints.Add(new WalkWaypoint(pos, waypointType));
                        break;
                    case WaypointType.WAYPOINT_HOLE:
                        pos.Y++;
                        pos.Z--;
                        Waypoints.Add(new WalkWaypoint(pos, waypointType));
                        break;
                    case WaypointType.WAYPOINT_RAMP:
                        break;
                    case WaypointType.WAYPOINT_STAIR_UP:
                        pos.Y--;
                        Waypoints.Add(new WalkWaypoint(pos, waypointType));
                        break;
                    case WaypointType.WAYPOINT_START_DOWN:
                        pos.Y++;
                        Waypoints.Add(new WalkWaypoint(pos, waypointType));
                        break;
                }
            }
        }

        public void AddWaypoint(Direction direction)
        {
            Position playerPos = GlobalVariables.GetPlayerPosition().Clone();
            switch (direction)
            {
                case Direction.Down:
                    playerPos.Y++;
                    break;
                case Direction.Left:
                    playerPos.X--;
                    break;
                case Direction.Up:
                    playerPos.Y--;
                    break;
                case Direction.Right:
                    playerPos.X++;
                    break;
            }

            Waypoints.Add(new WalkWaypoint(playerPos, WaypointType.WAYPOINT_RAMP));
        }

        public void AddWaypoint(Waypoint waypoint)
        {
            if (!Enable)
                Waypoints.Add(waypoint);
        }

        public void AddWaypoint(String words, SayType type)
        {
            if (!Enable)
                Waypoints.Add(new SayWaypoint(words, type));
        }

        public void AddWaypoint(int delay)
        {
            if (!Enable)
                Waypoints.Add(new WaitWaypoint(delay));
        }

        public void MoveUp(int index)
        {
            if (!Enable)
            {
                if (index > 0 && index < Waypoints.Count)
                    Waypoints.Move(index, index - 1);
            }
        }

        public void MoveDown(int index)
        {
            if (!Enable)
            {
                if (index >= 0 && index < Waypoints.Count - 1)
                    Waypoints.Move(index, index + 1);
            }
        }

        public void Remove(int index)
        {
            if (!Enable)
                Waypoints.RemoveAt(index);
        }

        public void Clear()
        {
            if (!Enable)
            {
                CurrentWaypoint = 0;
                Waypoints.Clear();
            }
        }

        public override string GetName()
        {
            return "AutoWalk";
        }

        public override bool RunOnlyConnected()
        {
            return true;
        }

        public override ModulePriority GetPriority()
        {
            return ModulePriority.LOW;
        }
    }
}
