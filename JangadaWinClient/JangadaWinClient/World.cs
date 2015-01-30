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
        public Creature selectedCreature = null;


        public World(Player player)
        {
            this.player = player;
        }

        public Player FindPlayerByGuid(string guid)
        {
            foreach (Creature creature in creatures)
            {
                if (creature.Guid.Equals(guid))
                {
                    return (Player)creature;
                }
            }
            return null;
        }

        public void AddPlayer(Player _player)
        {
            creatures.Add(_player);
        }

        public void SetTerrain(Terrain terrain)
        {
            this.terrain = terrain;
        }

        public void Draw(NewCamera camera)
        {
            terrain.Draw(camera);
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
            creatures.Remove(nPlayer);
        }
    }
}
