using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private Creatures() { }

        private IDictionary<uint, Creature> creatures = new Dictionary<uint, Creature>();

        public Creature AddCreature(uint id)
        {
            Creature cr = new Creature(id);
            creatures.Add(id, cr);
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
            if (creatures.ContainsKey(id))
                creatures.Remove(id);
        }

        public void Clear()
        {
            Logger.Log("Limpando todas as criaturas.");
            creatures.Clear();
        }
    }
}
