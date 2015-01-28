using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Enums
{
    public enum ServerMessageType : byte
    {
        Characters = 1,
        AreaDescription = 2,
        PlayerLogin = 3,
        PlayerMovement = 4,
        CharacterMovement = 5,
        PlayerLogout = 6
    }
}
