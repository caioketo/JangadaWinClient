using Jangada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    public class MessageHelper
    {
        public static void SendLoginMessage(string login, string password)
        {
            Jangada.getInstance().AddLog("Attempting to connect...");
            TCPClient.getInstance().StartConnect();
            Jangada.getInstance().AddLog("Connected.");
            Jangada.getInstance().AddLog("Sending LoginPacket (User: " + login + " Pass: " + password + ")");
            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.LoginPacket = LoginPacket.CreateBuilder()
                 .SetLogin(login)
                 .SetPassword(password)
                 .Build();
            newMessage.Type = Networkmessage.Types.Type.LOGIN;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            TCPClient.getInstance().Send(messagesToSend);
        }


        public static void SendSelectCharacter(int charId)
        {
            Jangada.getInstance().AddLog("Selecting character id = " + charId.ToString());
            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.SelectCharacterPacket = SelectCharacterPacket.CreateBuilder()
                .SetId(charId)
                .Build();
            newMessage.Type = Networkmessage.Types.Type.SELECTEDCHAR;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            TCPClient.getInstance().Send(messagesToSend);
        }

    }
}
