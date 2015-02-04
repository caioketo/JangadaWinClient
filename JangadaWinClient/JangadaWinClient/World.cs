using JangadaWinClient.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    public class World
    {
        public Terrain terrain;
        public Player player;
        public List<Creature> creatures = new List<Creature>();
        public List<Player> players = new List<Player>();
        public Creature selectedCreature = null;


        public World(Player player)
        {
            this.player = player;
        }

        public Player FindPlayerByGuid(string guid)
        {
            foreach (Player player in players)
            {
                if (player.Guid.Equals(guid))
                {
                    return player;
                }
            }
            return null;
        }

        public Creature FindCreatureByGuid(string guid)
        {
            foreach (Creature creature in creatures)
            {
                if (creature.Guid.Equals(guid))
                {
                    return creature;
                }
            }
            return null;
        }

        public void AddCreature(Creature creature)
        {
            creatures.Add(creature);
        }

        public void AddPlayer(Player _player)
        {
            players.Add(_player);
        }

        public void SetTerrain(Terrain terrain)
        {
            this.terrain = terrain;
        }

        public void Draw(NewCamera camera)
        {
            terrain.Draw(camera);
            foreach (Player nPlayer in players)
            {
                nPlayer.Draw(camera);
            }
            foreach (Creature creature in creatures)
            {
                creature.Draw(camera);
            }
            player.Draw(camera);
        }

        public bool IsSelected(Creature creature)
        {
            return creature.Equals(selectedCreature);
        }

        internal void RemovePlayer(Player nPlayer)
        {
            players.Remove(nPlayer);
        }

        internal void RemoveCreature(Creature creature)
        {
            creatures.Remove(creature);
        }
    }
}
