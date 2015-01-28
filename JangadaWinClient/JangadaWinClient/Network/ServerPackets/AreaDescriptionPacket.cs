using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ServerPackets
{
    public class AreaDescriptionPacket
    {
        public int Id;
        public PlayerDescription Player;
        public List<PlayerDescription> Players = new List<PlayerDescription>();

        public static AreaDescriptionPacket Parse(NetworkMessage message)
        {
            AreaDescriptionPacket packet = new AreaDescriptionPacket();
            packet.Id = message.GetUInt16();
            packet.Player = PlayerDescription.Parse(message);
            int playersCount = message.GetUInt16();
            for (int i = 0; i < playersCount; i++)
            {
                packet.Players.Add(PlayerDescription.Parse(message));
            }
            return packet;
        }
    }
}
