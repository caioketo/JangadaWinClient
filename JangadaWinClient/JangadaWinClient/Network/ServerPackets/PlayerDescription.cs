using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ServerPackets
{
    public class PlayerDescription
    {
        public string Guid;
        public Vector3 Position;
        public Quaternion Rotation;

        public static PlayerDescription Parse(NetworkMessage message)
        {
            PlayerDescription packet = new PlayerDescription();
            packet.Guid = message.GetString();
            packet.Position = message.GetPosition();
            packet.Rotation = message.GetQuaternion();
            return packet;
        }
    }
}
