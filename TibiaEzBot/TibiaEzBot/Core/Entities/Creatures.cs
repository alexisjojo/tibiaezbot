using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TibiaEzBot.Core.Entities
{
    public class Creatures
    {
        private static Creatures instance;

        public static Creatures GetInstance()
        {
            if (instance == null)
                instance = new Creatures();

            return instance;
        }

        private Creatures()
        {
        }

        private IDictionary<uint, Creature> creatures = new Dictionary<uint, Creature>();
        private ReaderWriterLockSlim creaturesLock = new ReaderWriterLockSlim();

        public ReaderWriterLockSlim CreaturesLock { get { return creaturesLock; } }

        public Creature AddCreature(uint id)
        {
            Creature cr = new Creature(id);
            creaturesLock.EnterWriteLock();
            creatures.Add(id, cr);
            creaturesLock.ExitWriteLock();
            return cr;
        }

        public Creature GetCreature(uint id)
        {
            if (creatures.ContainsKey(id))
                return creatures[id];

            return null;
        }

        public Creature GetPlayer()
        {
            return GetCreature(GlobalVariables.GetPlayerId());
        }

        public void RemoveCreature(uint id)
        {
            creaturesLock.EnterWriteLock();

            if (creatures.ContainsKey(id))
                creatures.Remove(id);

            creaturesLock.ExitWriteLock();
        }

        public void Clear()
        {
            Logger.Log("Limpando todas as criaturas.");
            creaturesLock.EnterWriteLock();

            creatures.Clear();

            creaturesLock.ExitWriteLock();
        }
    }
}
