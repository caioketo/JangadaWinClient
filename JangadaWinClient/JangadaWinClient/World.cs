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
        public NewPlayer player;
        public List<NewPlayer> players = new List<NewPlayer>();

        public World(NewPlayer player)
        {
            this.player = player;
        }

        public NewPlayer FindPlayerByGuid(string guid)
        {
            foreach (NewPlayer nPlayer in players)
            {
                if (nPlayer.Guid.Equals(guid))
                {
                    return nPlayer;
                }
            }
            return null;
        }

        public void AddPlayer(NewPlayer _player)
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
            foreach (NewPlayer _player in players)
            {
                _player.Draw(camera);
            }
            player.Draw(camera);
        }

        internal void RemovePlayer(NewPlayer nPlayer)
        {
            players.Remove(nPlayer);
        }
    }
}
