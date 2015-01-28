using JangadaWinClient.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network.ClientPackets
{
    public class LoginPacket
    {
        public string Login;
        public string Password;

        public void Add(NetworkMessage message)
        {
            message.AddByte((byte)ClientMessageType.Login);
            message.AddString(this.Login);
            message.AddString(this.Password);
        }

        public LoginPacket(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
