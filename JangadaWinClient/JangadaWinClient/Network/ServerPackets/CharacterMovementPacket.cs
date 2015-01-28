using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ServerPackets
{
    public class CharacterMovementPacket
    {
        public PlayerDescription Player;

        public static CharacterMovementPacket Parse(NetworkMessage message)
        {
            CharacterMovementPacket packet = new CharacterMovementPacket();
            packet.Player = PlayerDescription.Parse(message);
            return packet;
        }
    }
}
