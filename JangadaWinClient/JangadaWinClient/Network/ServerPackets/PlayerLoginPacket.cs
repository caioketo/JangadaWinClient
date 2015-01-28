using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ServerPackets
{
    public class PlayerLoginPacket
    {
        public PlayerDescription Player;

        public static PlayerLoginPacket Parse(NetworkMessage message)
        {
            PlayerLoginPacket packet = new PlayerLoginPacket();
            packet.Player = PlayerDescription.Parse(message);
            return packet;
        }
    }
}
