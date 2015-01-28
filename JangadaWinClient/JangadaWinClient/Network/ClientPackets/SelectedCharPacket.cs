using JangadaWinClient.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ClientPackets
{
    public class SelectedCharPacket
    {
        public int Id;

        public void Add(NetworkMessage message)
        {
            message.AddByte((byte)ClientMessageType.SelectedChar);
            message.AddUInt16((ushort)this.Id);
        }

        public SelectedCharPacket(int id)
        {
            this.Id = id;
        }
    }
}
