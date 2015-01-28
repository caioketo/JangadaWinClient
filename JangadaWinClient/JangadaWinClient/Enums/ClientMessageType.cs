using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Enums
{
    public enum ClientMessageType : byte
    {
        Login = 1,
        SelectedChar = 2,
        RequestMovement = 3
    }
}
