using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ServerPackets
{
    public class PlayerMovementPacket
    {
        public Vector3 newPosition;
        public Quaternion newRotation;

        public static PlayerMovementPacket Parse(NetworkMessage message)
        {
            PlayerMovementPacket packet = new PlayerMovementPacket();
            packet.newPosition = message.GetPosition();
            packet.newRotation = message.GetQuaternion();
            return packet;
        }
    }
}
