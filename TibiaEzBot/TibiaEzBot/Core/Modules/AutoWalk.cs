using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TibiaEzBot.Core.Entities;
using System.Collections.ObjectModel;
using TibiaEzBot.Core.Util;

namespace TibiaEzBot.Core.Modules
{
    public class AutoWalk : Module
    {
        private bool enable;
        private DateTime delayStart;

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
                        if (delayStart != DateTime.MinValue && (DateTime.UtcNow - delayStart).Milliseconds >= waypoint.GetWaitWaypoint().Delay)
                        {
                            delayStart = DateTime.MinValue;
                            IncrementWaypoint();
                        }
                        else if (delayStart == DateTime.MinValue)
                            delayStart = DateTime.UtcNow;

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

        public void Clear()
        {
            if (!Enable)
                Waypoints.Clear();
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
