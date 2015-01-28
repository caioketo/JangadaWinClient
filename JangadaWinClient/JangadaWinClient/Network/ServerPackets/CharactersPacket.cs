using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ServerPackets
{
    public class CharactersPacket
    {

        public int Count;
        public List<Character> Characters = new List<Character>();

        public static CharactersPacket Parse(NetworkMessage message)
        {
            CharactersPacket packet = new CharactersPacket();

            packet.Count = message.GetUInt16();
            for (int i = 0; i < packet.Count; i++)
            {
                Character newChar = new Character();
                newChar.Id = message.GetUInt16();
                newChar.Name = message.GetString();
                newChar.Info = message.GetString();
                packet.Characters.Add(newChar);
            }

            return packet;
        }




        public class Character
        {
            public string Name;
            public string Info;
            public int Id;
        }
    }
}
