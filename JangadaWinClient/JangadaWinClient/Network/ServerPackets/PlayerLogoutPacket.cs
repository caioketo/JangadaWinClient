using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ServerPackets
{
    public class PlayerLogoutPacket
    {
        public string Guid;

        public static PlayerLogoutPacket Parse(NetworkMessage message)
        {
            PlayerLogoutPacket packet = new PlayerLogoutPacket();
            packet.Guid = message.GetString();
            return packet;
        }
    }
}
