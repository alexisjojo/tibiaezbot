using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TibiaEzBot.Core.Entities;
using TibiaEzBot.Core.Constants;

namespace TibiaEzBot.Core
{
    public static class GlobalVariables
    {
		private static Position playerPosition;
		private static uint playerId;
        private static bool canReportBugs;
        private static uint playerCash;
        private static uint playerAddress;

        private static bool connected;
		private static bool attacking;

        private static byte worldLightLevel;
        private static byte worldLightColor;
        
        private static uint[] playerStatus = new uint[(int)PlayerStatus.Last];
		private static uint[,] playerSkills = new uint[(int)Skills.Last, (int)SkillAttribute.Last];

        private static ushort playerIcons;

		public static Position GetPlayerPosition() { return playerPosition; }
		public static void SetPlayerPosition(Position position) { playerPosition = position; }
		
		public static uint GetPlayerId() { return playerId; }
		public static void SetPlayerId(uint id) { playerId = id; }
		
		public static bool GetCanReportBugs() { return canReportBugs; }
		public static void SetCanReportBugs(bool value) { canReportBugs = value; }

        public static byte GetWorldLightLevel() { return worldLightLevel; }
        public static void SetWorldLightLevel(byte value) { worldLightLevel = value; }

        public static byte GetWorldLightColor() { return worldLightColor; }
        public static void SetWorldLightColor(byte value) { worldLightColor = value; }
        
        public static uint GetPlayerCash() { return playerCash; }
        public static void SetPlayerCash(uint value) { playerCash = value; }

        public static ushort GetPlayerIcons() { return playerIcons; }
        public static void SetPlayerIcons(ushort value) { playerIcons = value; }

        public static bool IsConnected() { return connected; }
        public static void SetConnected(bool value) { connected = value; }

        public static bool IsWalking() { return playerAddress != 0 && Convert.ToBoolean(Kernel.GetInstance().
            Client.Memory.ReadByte(playerAddress + Addresses.Creature.DistanceIsWalking)); }

        public static uint GetPlayerMemoryAddress() { return playerAddress; }
        public static void SetPlayerMemoryAddress(uint value) { playerAddress = value; }
		
		public static bool IsAttacking() { return attacking; }
        public static void SetAttacking(bool value) { attacking  = value; }
		
        public static uint GetPlayerStatus(PlayerStatus status)
        {
        	if(status < PlayerStatus.Last)
        		return playerStatus[(int)status];
        		
        	return 0;
        }
        
		public static void SetPlayerStatus(PlayerStatus status, uint value)
		{
			if(status < PlayerStatus.Last)
			{
				playerStatus[(int)status] = value;
			}
		}

		public static uint GetPlayerSkill(Skills skill, SkillAttribute attr)
		{
			if(skill < Skills.Last && attr < SkillAttribute.Last)
			{
				return playerSkills[(int)skill, (int)attr];
			}

            return 0;
		}
		
		public static void SetPlayerSkill(Skills skill, SkillAttribute attr, uint value)
		{
			if(skill < Skills.Last && attr < SkillAttribute.Last)
			{
				playerSkills[(int)skill, (int)attr] = value;
			}
		}
		
		public static void Clear()
		{
            Logger.Log("Limpando variaves globais.");
            playerCash = 0;
            playerIcons = 0;
            playerId = 0;
            playerPosition = null;

            for (int i = 0; i < (int)Skills.Last; i++)
            {
                for (int j = 0; j < (int)SkillAttribute.Last; j++)
                {
                    playerSkills[i,j] = 0;
                }
            }

            for (int i = 0; i < (int)PlayerStatus.Last; i++)
            {
                playerStatus[i] = 0;
            }

            connected = false;
            canReportBugs = false;
            playerAddress = 0;

		}
    }
}
