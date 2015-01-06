using Jangada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JangadaWinClient
{
    public class Parser
    {
        public static bool Parse(Networkmessage.Types.Type type, Networkmessage message)
        {
            switch (type)
            {
                case Networkmessage.Types.Type.CHARACTERS:
                    Jangada.getInstance().AddLog(message.CharactersPacket.CharacterListCount.ToString());
                    Jangada.getInstance().loginWindow.ShowCharList(message.CharactersPacket.CharacterListList.ToList());
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
