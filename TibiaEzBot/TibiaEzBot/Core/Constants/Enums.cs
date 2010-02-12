using System;
using System.Collections.Generic;

// Enumerations

namespace TibiaEzBot.Core.Constants
{
    #region General
	
	public enum PlayerStatus 
	{
		Health,
		HealthMax,
		Capacity,
		Experience,
		Mana,
		ManaMax,
		Soul,
		Stamina,
		Last
	}

	public enum Skills
	{
		Level,
		Magic,
		Fist,
		Club,
		Sword,
		Axe,
		Distance,
		Shield,
		Fish,
		Last
	}
	
	public enum SkillAttribute
	{
		Level,
		Percent,
		Last
	}
	
    public enum ObjectType
    {
        Memory,
        Packet
    }

    /// <summary>
    /// The direction to walk in or turn to.
    /// </summary>
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
        UpRight = 5,
        DownRight = 6,
        DownLeft = 7,
        UpLeft = 8
    }

    /// <summary>
    /// The byte that is sent on RSA encrypted packets
    /// </summary>
    public enum OperatingSystem : byte
    {
        Linux = 1,
        Windows = 2
    }

    /// <summary>
    /// Different types of locations.
    /// </summary>
    public enum ItemLocationType
    {
        Ground,
        Slot,
        Container
    }

    /// <summary>
    /// The type of server.
    /// </summary>
    public enum ServerType
    {
        Real,
        OT
    }

    #endregion

    #region Client
    /// <summary>
    /// Formerly Cursor
    /// </summary>
    public enum ActionState : byte
    {
        None = 0,
        LeftClick = 1, // // left-click to walk or to use the client interface
        Left = LeftClick,    // walk etc
        RightClick = 2, // right-click to use an object such as a torch or an apple
        Right = RightClick,   // use
        InspectObject = 3, // left-click + right-click to see or inspect an object
        Inspect = InspectObject,
        MoveObject = 6, // dragging an object to move it to a new location
        Drag = MoveObject,
        UseObject = 7, // using an object such as a rope, a shovel, a fishing rod, or a rune
        Using = UseObject,   // in-use fishing shooting rune
        SelectHotkeyObject = 8, // selecting an object to bind to a hotkey from the "Hotkey Options" window
        TradeObject = 9, // using "Trade with..." on an object to select a player with whom to trade
        Trade = TradeObject,
        ClientHelp = 10, // // client mouse over tooltip help
        Help = ClientHelp,   // client help
        OpenDialogWindow = 11, // opening a dialog window such as the "Options" window, "Select Outfit" window, or "Move Objects" window
        PopupMenu = 12 // showing a popup menu with options such as "Invite to Party", "Set Outfit", "Copy Name", or "Set Mark"
    }

    public enum CurrentDialog
    {
        MoveObjects = 88
    }

    public enum LoginStatus : byte
    {
        LoggedOut = 0,
        NotLoggedIn = LoggedOut,
        LoggingIn = 6,
        LoggedIn = 8
    }

    public static class RSAKey
    {
        public static string OpenTibia = "109120132967399429278860960508995541528237502902798129123468757937266291492576446330739696001110603907230888610072655818825358503429057592827629436413108566029093628212635953836686562675849720620786279431090218017681061521755056710823876476444260558147179707119674283982419152118103759076030616683978566631413";
        public static string RealTibia = "124710459426827943004376449897985582167801707960697037164044904862948569380850421396904597686953877022394604239428185498284169068581802277612081027966724336319448537811441719076484340922854929273517308661370727105382899118999403808045846444647284499123164879035103627004668521005328367415259939915284902061793";
    }
    #endregion

    #region Player

    public enum Attack : byte
    {
        FullAttack = 1,
        Balance = 2,
        FullDefense = 3
    }

    public enum Follow : byte
    {
        DoNotFollow = 0,
        FollowClose = 1,
        FollowDistance = 2
    }

    public enum Flag
    {
        None = 0,
        Poisoned = 1,
        Burning = 2,
        Electrified = 4,
        Drunk = 8,
        ProtectedByMagicShield = 16,
        Paralysed = 32,
        Paralyzed = Paralysed,
        Hasted = 64,
        InBattle = 128,
        Drowning = 256,
        Freezing = 512,
        Dazzled = 1024,
        Cursed = 2048,
        Strengthened = 4096,
        CannotLogoutOrEnterProtectionZone = 8192,
        WithinProtectionZone = 16384
    }

    public enum SlotNumber
    {
        None = 0,
        Head = 1,
        Necklace = 2,
        Backpack = 3,
        Armor = 4,
        Right = 5,
        Left = 6,
        Legs = 7,
        Feet = 8,
        Ring = 9,
        Ammo = 10,
        First = Head,
        Last = Ammo
    }

    #endregion

    #region Outfits

    public enum OutfitAddon : byte
    {
        None = 0,
        Addon1 = 1,
        Addon2 = 2,
        Addon3 = 3,
        First = Addon1,
        Second = Addon2,
        Both = Addon3
    }

    // Not really an enum, because we want to allow any number for color.
    public static class OutfitColor
    {
        public static int Red = 94;
        public static int Orange = 77;
        public static int Yellow = 79;
        public static int Green = 82;
        public static int Blue = 88;
        public static int Purple = 90;
        public static int Brown = 116;
        public static int Black = 114;
        public static int White = 0;
        public static int Pink = 91;
        public static int Grey = 57;
        public static int Peach = 1;
    }

    public enum OutfitType
    {
        Invisible = 0,               // Stealth Ring Effect Also For Item As Outfit
        OrcWarlord = 2,
        WarWolf = 3,
        OrcRider = 4,
        Orc = 5,
        OrcShaman = 6,
        OrcWarrior = 7,
        OrcBerserker = 8,
        Necromancer = 9,
        ButterflyYellow = 10,
        WaterElemental = 11,
        DemonColor = 12,
        BlackSheep = 13,
        Sheep = 14,
        Troll = 15,
        Bear = 16,
        Beholder = 17,
        Ghoul = 18,
        Slime = 19,
        QuaraPredator = 20,
        Rat = 21,
        Cyclops = 22,
        MinotaurMage = 23,
        MinotaurArcher = 24,
        Minotaur = 25,
        Rotworm = 26,
        Wolf = 27,
        Snake = 28,
        MinotaurGuard = 29,
        Spider = 30,
        Deer = 31,
        Dog = 32,
        Skeleton = 33,
        Dragon = 34,
        Demon = 35,
        PoisonSpider = 36,
        DemonSkeleton = 37,
        GiantSpider = 38,
        DragonLord = 39,
        FireDevil = 40,
        Lion = 41,
        PolarBear = 42,
        Scorpion = 43,
        Wasp = 44,
        Bug = 45,
        QuaraConstrictor = 46,
        QuaraHydromancer = 47,
        Ghost = 48,
        FireElemental = 49,
        OrcSpearman = 50,
        GreenDjinn = 51,
        WinterWolf = 52,
        FrostTroll = 53,
        Witch = 54,
        Behemoth = 55,
        CaveRat = 56,
        Monk = 57,
        Priestess = 58,
        OrcLeader = 59,
        Pig = 60,
        Goblin = 61,
        Elf = 62,
        ElfArcanist = 63,
        ElfScout = 64,
        Mummy = 65,
        DwarfGeomancer = 66,
        StoneGolem = 67,
        Vampire = 68,
        Dwarf = 69,
        DwarfGuard = 70,
        DwarfSoldier = 71,
        QuaraMantassin = 72,
        Hero = 73,
        Rabbit = 74,
        GameMasterVoluntary = 75,
        SwampTroll = 76,
        QuaraPincher = 77,
        Banshee = 78,
        AncientScarab = 79,
        BlueDjinn = 80,
        Cobra = 81,
        Larva = 82,
        Scarab = 83,
        Pharaoh1 = 84,
        Pharaoh2 = 85,
        Pharaoh3 = 86,
        PharaohDressed1 = 87,
        PharaohDressed2 = 88,
        Pharaoh4 = 89,
        Pharaoh5 = 90,
        PharaohDressed3 = 91,
        Mimic = 92,
        PirateMarauder = 93,
        Hyaena = 94,
        Gargoyle = 95,
        PirateCutthroat = 96,
        PirateBuccaneer = 97,
        PirateCorsair = 98,
        Lich = 99,
        CryptShambler = 100,
        Bonebeast = 101,
        Deathslicer = 102,
        Efreet = 103,
        Marid = 104,
        Badger = 105,
        Skunk = 106,
        Demon2 = 107,
        ElderBeholder = 108,
        Gazer = 109,
        Yeti = 110,
        Chicken = 111,
        Crab = 112,
        LizardTemplar = 113,
        LizardSentinel = 114,
        LizardSnakecharmer = 115,
        Kongra = 116,
        Merlkin = 117,
        Sibang = 118,
        Crocodile = 119,
        Carniphila = 120,
        Hydra = 121,
        Bat = 122,
        Panda = 123,
        Centipede = 124,
        Tiger = 125,
        OldFemale = 126,
        OldMale = 127,
        CitizenMale = 128,
        HunterMale = 129,
        MageMale = 130,
        KnightMale = 131,
        NoblemanMale = 132,
        SummonerMale = 133,
        WarriorMale = 134,
        // Nothing = 135
        CitizenFemale = 136,
        HunterFemale = 137,
        SummonerFemale = 138,
        KnightFemale = 139,
        NoblemanFemale = 140,
        MageFemale = 141,
        WarriorFemale = 142,
        BarbarianMale = 143,
        DruidMale = 144,
        WizardMale = 145,
        OrientalMale = 146,
        BarbarianFemale = 147,
        DruidFemale = 148,
        WizardFemale = 149,
        OrientalFemale = 150,
        PirateMale = 151,
        AssassinMale = 152,
        BeggarMale = 153,
        ShamanMale = 154,
        PirateFemale = 155,
        AssassinFemale = 156,
        BeggarFemale = 157,
        ShamanFemale = 158,
        ElfColor = 159,
        DwarfColor = 160,
        // Nothing = 161-191
        CarrionWorm = 192,
        EnlightenedsOfTheCult = 193,
        AdeptsOfTheCult = 194,
        PirateSkeleton = 195,
        PirateGhost = 196,
        Tortoise = 197,
        ThornbackTortoise = 198,
        Mammoth = 199,
        BloodCrab = 200,
        Demon3 = 201,
        MinotaurGuard2 = 202,
        ElfArcanist2 = 203,
        DragonLord2 = 204,
        StoneGolem2 = 205,
        Monk2 = 206,
        MinotaurGuard3 = 207,
        GiantSpider2 = 208,
        Necromancer2 = 209,
        ElderBeholder2 = 210,
        Elephant = 211,
        Flamingo = 212,
        ButterflyPink = 213,
        DworcVoodoomaster = 214,
        DworcFleshhunter = 215,
        DworcVenomsniper = 216,
        Parrot = 217,
        TerrorBird = 218,
        Tarantula = 219,
        SerpentSpawn = 220,
        SpitNettle = 221,
        Toad = 222,
        Seagull = 223,
        AzureFrog = 224,
        DarkMonk = 225,
        FrogColor = 226,
        ButterflyBlue = 227,
        ButterflyRed = 228,
        Ferumbras = 229,
        HandOfCursedFate = 230,
        UndeadDragon = 231,
        LostSoul = 232,
        BetrayedWraith = 233,
        DarkTorturer = 234,
        Spectre = 235,
        Destroyer = 236,
        DiabloicImp = 237,
        Defiler = 238,
        Wyvern = 239,
        Hellhound = 240,
        Phantasm = 241,
        Hellfire = 242,
        HellfireFighter = 243,
        Juggernaut = 244,
        Nightmare = 245,
        Blightwalker = 246,
        Plaguesmith = 247,
        FrostDragon = 248,
        ChakoyaTribewarden = 249,
        Penguin = 250,
        NorsemanMale = 251,
        NorsemanFemale = 252,
        BarbarianHeadsplitter = 253,
        BarbarianSkullhunter = 254,
        BarbarianBloodwalker = 255,
        Braindeath = 256,
        FrostGiant = 257,
        Husky = 258,
        ChakoyaToolshaper = 259,
        ChakoyaWindcaller = 260,
        IceGolem = 261,
        SilverRabbit = 262,
        CrystalSpider = 263,
        BarbarianBrutetamer = 264,
        FrostGiantess = 265,
        GameMasterCustomerSupport = 266,
        Swimmer = 267,
        NightmareKnightMale = 268,
        NightmareKnightFemale = 269,
        JesterFemale = 270,
        DragonHatchling = 271,
        DragonLordHatchling = 272,
        JesterMale = 273,
        Squirrel = 274,
        SeaSerpent = 275,
        Cat = 276,
        CyclopsSmith = 277,
        BrotherhoodOfBonesMale = 278,
        BrotherhoodOfBonesFemale = 279,
        CyclopsDrone = 280,
        TrollChampion = 281,
        IslandTroll = 282,
        FrostDragonHatchling = 283,
        Cockroach = 284,
        EarthOverlord = 285,
        SlickWaterElemental = 286,
        TheCount = 287,
        DemonHunterFemale = 288,
        DemonHunterMale = 289,
        MassiveEnergyElemental = 290,
        Wyrm = 291,
        Pumpkin = 292,
        EnergyElemental = 293,
        Wisp = 294,
        RotwormQueen = 295,
        GoblinAssassin = 296,
        GoblinScavenger = 297,
        SkeletonWarrior = 298,
        BogRaider = 299,
        GrimReaper = 300,
        EarthElemental = 301,
        CommunityManager = 302,
        Unknown1 = 303,
        WorkerGolem = 304,
        MutatedRat = 305,
        UndeadGladiator = 306,
        MutatedBat = 307,
        Werewolf = 308,
        Azerus = 309,
        HauntedTreeling = 310,
        Zombie = 311,
        VampireBride = 312,
        Gozzler = 313,
        AcidBlob = 314,
        DeathBlob = 315,
        MercuryBlob = 316,
        YoungSeaSerpent = 317,
        MutatedTiger = 318,
        Unknown2 = 319,
        Nightstalker = 320,
        NightmareScion = 321,
        Hellspawn = 322,
        MutatedHuman = 323,
        YalaharianFemale = 324,
        YalaharianMale = 325,
        WarGolem = 326,
        WhiteFemale = 327,
        WeddingMale = 328,
        WeddingFemale = 329,
        Medusa = 330,
        Queen = 331,
        King = 332,
        SmallStoneGolem = 333
    }

    #endregion

    #region Creature

    public static class LightSize
    {
        public static int None = 0;
        public static int Torch = 7;
        public static int Full = 27;
    }

    public static class LightColor
    {
        public static int None = 0;
        public static int Default = 206; // default light color
        public static int Orange = Default;
        public static int White = 215;
    }

    public enum Skull : byte
    {
        None = 0,
        Yellow = 1,
        Green = 2,
        White = 3,
        Red = 4,
        Black = 5
    }

    public enum PartyShield
    {
        None = 0,
        Host = 1, // the host invites the guest to the party
        Inviter = Host,
        Guest = 2, // the guest joins the host at the party
        Invitee = Guest,
        Member = 3,
        Leader = 4,
        MemberSharedExp = 5,
        LeaderSharedExp = 6,
        MemberSharedExpInactive = 7,
        LeaderSharedExpInactive = 8

    }

    public enum CreatureType : byte
    {
        Player = 0x0,
        Target = 0x1,
        NPC = 0x40
    }

    public enum WarIcon
    {
        None = 0,
        Blue = 1,
        Green = 2,
        Red = 3
    }

    #endregion

    #region Spells

    public enum SpellCategory
    {
        Attack,
        Healing,
        Summon,
        Supply,
        Support
    }

    public enum SpellType
    {
        Instant,
        ItemTransformation,
        Creation
    }

    #endregion

    #region VipList

    public enum VipStatus
    {
        Offline = 0,
        Online = 1
    }
    
    public enum VipIcon
    {
        Blank = 0,
        Heart = 1,
        Skull = 2,
        Lightning = 3,
        Crosshair = 4,
        Star = 5,
        YinYang = 6,
        Triangle = 7,
        X = 8,
        Dollar = 9,
        Cross = 10
    }

    #endregion

    #region Hotkeys

    public enum HotkeyObjectUseType
    {
        WithCrosshairs = 0,
        UseOnTarget = 1,
        UseOnSelf = 2
    }

    #endregion

    #region Text Display
    public enum ClientFont : int
    {
        Normal = 1,
        NormalBorder = 2,
        Small = 3,
        Weird = 4
    }
    #endregion


    public enum SpeechType : byte
    {
        Say = 0x01,	//normal talk
        Whisper = 0x02,	//whispering - #w text
        Yell = 0x03,	//yelling - #y text
        PrivatePlayerToNPC = 0x04, //Player-to-NPC speaking(NPCs channel)
        PrivateNPCToPlayer = 0x05, //NPC-to-Player speaking
        Private = 0x06, //Players speaking privately to players
        ChannelYellow = 0x07,	//Yellow message in chat
        ChannelWhite = 0x08, //White message in chat
        RuleViolationReport = 0x09, //Reporting rule violation - Ctrl+R
        RuleViolationAnswer = 0x0A, //Answering report
        RuleViolationContinue = 0x0B, //Answering the answer of the report
        Broadcast = 0x0C,	//Broadcast a message - #b
        ChannelRed = 0x0D,	//Talk red on chat - #c
        PrivateRed = 0x0E,	//Red private - @name@ text
        ChannelOrange = 0x0F,	//Talk orange on text
        //SPEAK_                = 0x10, //?
        ChannelRedAnonymous = 0x11,	//Talk red anonymously on chat - #d
        //SPEAK_MONSTER_SAY12 = 0x12, //?????
        MonsterSay = 0x13,	//Talk orange
        MonsterYell = 0x14,	//Yell orange
    }

    public enum StatusMessage : byte
    {
        ConsoleRed = 0x12, //Red message in the console
        ConsoleOrange = 0x13, //Orange message in the console
        ConsoleOrange2 = 0x14, //Orange message in the console
        Warning = 0x15, //Red message in game window and in the console
        EventAdvance = 0x16, //White message in game window and in the console
        EventDefault = 0x17, //White message at the bottom of the game window and in the console
        StatusDefault = 0x18, //White message at the bottom of the game window and in the console
        DescriptionGreen = 0x19, //Green message in game window and in the console
        StatusSmall = 0x1A, //White message at the bottom of the game window"
        ConsoleBlue = 0x1B, //Blue message in the console
    }

    public enum SquareColor : byte
    {
        Black = 0,
        Blue = 5,
        Green = 30,
        LightBlue = 35,
        Crystal = 65,
        Purple = 83,
        Platinum = 89,
        LightGrey = 129,
        DarkRed = 144,
        Red = 180,
        Orange = 198,
        Gold = 210,
        White = 215,
        None = 255
    }

    public enum ChatChannel : ushort
    {
        Guild = 0x00,
        Party = 0x01,
        //?Gamemaster = 0x01,
        Tutor = 0x02,
        RuleReport = 0x03,
        Game = 0x05,
        Trade = 0x06,
        TradeRook = 0x07,
        RealLife = 0x08,
        Help = 0x09,
        OwnPrivate = 0x0E,
        Custom = 0xA0,
        Custom1 = 0xA1,
        Custom2 = 0xA2,
        Custom3 = 0xA3,
        Custom4 = 0xA4,
        Custom5 = 0xA5,
        Custom6 = 0xA6,
        Custom7 = 0xA7,
        Custom8 = 0xA8,
        Custom9 = 0xA9,
        Private = 0xFFFF,
        None = 0xAAAA
    }

    public enum TextColor : byte
    {
        Blue = 5,
        Green = 30,
        LightBlue = 35,
        Crystal = 65,
        Purple = 83,
        Platinum = 89,
        LightGrey = 129,
        DarkRed = 144,
        Red = 180,
        Orange = 198,
        Gold = 210,
        White = 215,
        None = 255
    }

    public enum ProjectileType : byte
    {
        Spear = 0x01,
        Bolt = 0x02,
        Arrow = 0x03,
        Fire = 0x04,
        Energy = 0x05,
        PoisonArrow = 0x06,
        BurstArrow = 0x07,
        ThrowingStar = 0x08,
        ThrowingKnife = 0x09,
        SmallStone = 0x0A,
        Skull = 0x0B,
        BigStone = 0x0C,
        SnowBall = 0x0D,
        PowerBolt = 0x0E,
        SmallPoison = 0x0F,
        InfernalBolt = 0x10,
        HuntingSpear = 0x11,
        EnchantedSpear = 0x12,
        AssassinStar = 0x13,
        ViperStar = 0x14,
        RoyalSpear = 0x15,
        SniperArrow = 0x16,
        OnyxArrow = 0x17,
        EarthArrow = 0x18,
        NormalSword = 0x19,
        NormalAxe = 0x1A,
        NormalClub = 0x1B,
        EtherealSpear = 0x1C,
        Ice = 0x1D,
        Earth = 0x1E,
        Holy = 0x1F,
        Death = 0x20,
        FlashArrow = 0x21,
        FlamingArrow = 0x22,
        ShiverArrow = 0x23,
        EnergySmall = 0x24,
        IceSmall = 0x25,
        HolySmall = 0x26,
        EarthSmall = 0x27,
        EarthArrow2 = 0x28,
        Explosion = 0x29,
        Cake = 0x2A
    }

    public enum TileAnimationType
    {
        DrawBlood = 0x00,
        LoseEnergy = 0x01,
        Puff = 0x02,
        BlockHit = 0x03,
        ExplosionArea = 0x04,
        ExplosionDamage = 0x05,
        FireArea = 0x06,
        YellowRings = 0x07,
        PoisonRings = 0x08,
        HitArea = 0x09,
        Teleport = 0x0a,
        EnergyDamage = 0x0b,
        MagicEnergy = 0x0c,
        MagicBlood = 0x0d,
        MagicPoison = 0x0e,
        HitByFire = 0x0f,
        Poison = 0x10,
        MortArea = 0x11,
        SoundGreen = 0x12,
        SoundRed = 0x13,
        PoisonArea = 0x14,
        SoundYellow = 0x15,
        SoundPurple = 0x16,
        SoundBlue = 0x17,
        SoundWhite = 0x18,
        Bubbles = 0x19,
        Craps = 0x1a,
        GiftWraps = 0x1b,
        FireworkYellow = 0x1c,
        FireworkRed = 0x1d,
        FireworkBlue = 0x1e,
        Stun = 0x1f,
        Sleep = 0x20,
        WaterCreature = 0x21,
        Groundshaker = 0x22,
        Hearts = 0x23,
        FireAttack = 0x24,
        EnergyArea = 0x25,
        SmallClouds = 0x26,
        HolyDamage = 0x27,
        BigClouds = 0x28,
        IceArea = 0x29,
        IceTornado = 0x2a,
        IceAttack = 0x2b,
        Stones = 0x2c,
        SmallPlants = 0x2d,
        Carniphila = 0x2e,
        PurpleEnergy = 0x2f,
        YellowEnergy = 0x30,
        HolyArea = 0x31,
        BigPlants = 0x32,
        Cake = 0x33,
        GiantIce = 0x34,
        WaterSplash = 0x35,
        PlantAttack = 0x36,
        BlueArrow = 0x38,
        FlashSquare = 0x39
    }

    // (http://www.tpforums.org/forum/showthread.php?t=2399)
    /// <summary>
    /// Credits to Vitor for the EventTypes
    /// </summary>
    public enum EventType : byte
    {
        RegularDialog = 0x01,
        RegularDialog2 = 0x02,
        CharacterListLoading = 0x03,
        ConnectionToGameWorld = 0x04,
        LoginQueue = 0x05,
        Logout = 0x06,
        Exit = 0x07,
        EnterGame = 0x08,
        CharacterListLoading2 = 0x09,
        CharacterList = 0x0A,
        YouAreDead = 0x0B,
        LinkcopyWarning = 0x0C,
        AccountDataWarning = 0x0D,
        Undefined1 = 0x0E,
        Undefined2 = 0x0F,
        EditList = 0x10,
        SetOutfit = 0x11,
        BugReport = 0x12,
        ChannelList = 0x13,
        InvitePlayerPrivate = 0x14,
        ExcludePlayerPrivate = 0x15,
        IgnoreList = 0x16,
        RuleViolationReport = 0x17,
        AddToVip = 0x18,
        EditVip = 0x19,
        Undefined3 = 0x1A,
        Undefined4 = 0x1B,
        QuestLog = 0x1C,
        QuestLine = 0x1D,
        Info = 0x1E,
        GMRuleViolationPanel = 0x1F,
        EditMinimapMark = 0x20,
        EditMinimapMark2 = 0x21,
        HelpMenu = 0x22,
        TutorialHintsMenu = 0x23,
        OptionsMenu = 0x24,
        GraphicsOptionMenu = 0x25,
        AdvancedGraphicsOptionMenu = 0x26,
        ConsoleOptions = 0x27,
        Hotkey = 0x28,
        GeneralOptionsMenu = 0x29,
        MessageOfTheDay = 0x2A,
        DownloadClientUpdate = 0x2B,
        Undefined5 = 0x2C,
        Undefined6 = 0x2D,
        LastUsedHotkeyCrosshair = 0x2E,
        LastTradedItem = 0x2F,
        ClientHelp = 0x30,
        OpenPrivateChannelWithPlayer = 0x31,
        OpenChatChannel = 0x32,
        OpenChatChannel2 = 0x33,
        Undefined7 = 0x34,
        RuleViolationReportChannel = 0x35,
        OpenNPCsChannel = 0x36,
        Undefined8 = 0x37,
        Undefined9 = 0x38,
        NPCTrade = 0x39,
        Undefined10 = 0x3A,
        Undefined11 = 0x3B,
        Undefined12 = 0x3C,
        Undefined13 = 0x3D,
        TutorialHint = 0x3E,
        LastLookedItemContextMenu = 0x3F,
        AttackCreatureContextMenu = 0x40,
        AddToVipContextMenu = 0x41,
        CurrentSelectedChannelMessagesContextMenu = 0x42,
        CurrentSelectedChannelContextMenu = 0x43,
        EmptyContextMenu = 0x44,
        PasteContextMenu = 0x45,
        MinimapMark = 0x46,
        SkillsContextMenu = 0x47,
        NPCTradingItemsContextMenu = 0x48,
        ConnectToCharacterList = 0x49,
        ConnectToGameWorldUsingLastChosenCharacter = 0x4A,
        Undefined14 = 0x4B,
        RestartClientAfterPatchExecution = 0x53,
        UpdateClient = 0x54
    }

    public enum IncomingPacketType : byte
    {
        // GameServer
        SelfAppear = 0x0A,
        GMAction = 0x0B,
        ErrorMessage = 0x14,
        FyiMessage = 0x15,
        WaitingList = 0x16,
        Ping = 0x1E,
        Death = 0x28,
        CanReportBugs = 0x32,
        MapDescription = 0x64,
        MoveNorth = 0x65,
        MoveEast = 0x66,
        MoveSouth = 0x67,
        MoveWest = 0x68,
        TileUpdate = 0x69,
        TileAddThing = 0x6A,
        TileTransformThing = 0x6B,
        TileRemoveThing = 0x6C,
        CreatureMove = 0x6D,
        ContainerOpen = 0x6E,
        ContainerClose = 0x6F,
        ContainerAddItem = 0x70,
        ContainerUpdateItem = 0x71,
        ContainerRemoveItem = 0x72,
        InventorySetSlot = 0x78,
        InventoryResetSlot = 0x79,
        ShopWindowOpen = 0x7A,
        ShopSaleGoldCount = 0x7B,
        ShopWindowClose = 0x7C,
        SafeTradeRequestAck = 0x7D,
        SafeTradeRequestNoAck = 0x7E,
        SafeTradeClose = 0x7F,
        WorldLight = 0x82,
        MagicEffect = 0x83,
        AnimatedText = 0x84,
        Projectile = 0x85,
        CreatureSquare = 0x86,
        CreatureHealth = 0x8C,
        CreatureLight = 0x8D,
        CreatureOutfit = 0x8E,
        CreatureSpeed = 0x8F,
        CreatureSkull = 0x90,
        CreatureShield = 0x91,
        ItemTextWindow = 0x96,
        HouseTextWindow = 0x97,
        PlayerStatus = 0xA0,
        PlayerSkillsUpdate = 0xA1,
        PlayerFlags = 0xA2,
        CancelTarget = 0xA3,
        CreatureSpeech = 0xAA,
        ChannelList = 0xAB,
        ChannelOpen = 0xAC,
        ChannelOpenPrivate = 0xAD,
        RuleViolationOpen = 0xAE,
        RuleViolationRemove = 0xAF,
        RuleViolationCancel = 0xB0,
        RuleViolationLock = 0xB1,
        PrivateChannelCreate = 0xB2,
        ChannelClosePrivate = 0xB3,
        TextMessage = 0xB4,
        PlayerWalkCancel = 0xB5,
        FloorChangeUp = 0xBE,
        FloorChangeDown = 0xBF,
        OutfitWindow = 0xC8,
        VipState = 0xD2,
        VipLogin = 0xD3,
        VipLogout = 0xD4,
        QuestList = 0xF0,
        QuestPartList = 0xF1,
        ShowTutorial = 0xDC,
        AddMapMarker = 0xDD,
    }

    public enum OutgoingPacketType : byte
    {
        LoginServerRequest = 0x01,
        GameServerRequest = 0x0A,
        Logout = 0x14,
        ItemMove = 0x78,
        ShopBuy = 0x7A,
        ShopSell = 0x7B,
        ShopClose = 0x7C,
        ItemUse = 0x82,
        ItemUseOn = 0x83,
        ItemRotate = 0x85,
        LookAt = 0x8C,
        PlayerSpeech = 0x96,
        ChannelList = 0x97,
        ChannelOpen = 0x98,
        ChannelClose = 0x99,
        Attack = 0xA1,
        Follow = 0xA2,
        CancelMove = 0xBE,
        ItemUseBattlelist = 0x84,
        ContainerClose = 0x87,
        ContainerOpenParent = 0x88,
        TurnUp = 0x6F,
        TurnRight = 0x70,
        TurnDown = 0x71,
        TurnLeft = 0x72,
        AutoWalk = 0x64,
        AutoWalkCancel = 0x69,
        MoveUp = 0x65,
        MoveRight = 0x66,
        MoveDown = 0x67,
        MoveLeft = 0x68,
        MoveUpRight = 0x6A,
        MoveDownRight = 0x6B,
        MoveDownLeft = 0x6C,
        MoveUpLeft = 0x6D,
        VipAdd = 0xDC,
        VipRemove = 0xDD,
        SetOutfit = 0xD3,
        Ping = 0x1E,
        FightModes = 0xA0,
        ContainerUpdate = 0xCA,
        TileUpdate = 0xC9,
        PrivateChannelOpen = 0x9A,
        NpcChannelClose = 0x9E,
    }
}
