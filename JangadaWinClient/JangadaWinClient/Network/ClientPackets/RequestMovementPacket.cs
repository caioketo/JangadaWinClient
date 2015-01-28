using JangadaWinClient.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ClientPackets
{
    public class RequestMovementPacket
    {
        public MovementType Type;
        public float Amount;

        public void Add(NetworkMessage message)
        {
            message.AddByte((byte)ClientMessageType.RequestMovement);
            message.AddUInt16((ushort)this.Type);
            message.AddDouble(this.Amount);
        }

        public RequestMovementPacket(MovementType type, float amount)
        {
            this.Type = type;
            this.Amount = amount;
        }
    }
}
