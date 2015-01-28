using Jangada;
using JangadaWinClient.Enums;
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
            if (Jangada.getInstance().useProto)
            {
                Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
                newMessage.LoginPacket = LoginPacket.CreateBuilder()
                     .SetLogin(login)
                     .SetPassword(password)
                     .Build();
                newMessage.Type = Networkmessage.Types.Type.LOGIN;
                Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
                TCPClient.getInstance().Send(messagesToSend);
            }
            else
            {
                Network.NetworkMessage messageToSend = new Network.NetworkMessage();
                (new Network.ClientPackets.LoginPacket(login, password)).Add(messageToSend);
                TCPClient.getInstance().Send(messageToSend);
            }
        }


        public static void SendSelectCharacter(int charId)
        {
            Jangada.getInstance().AddLog("Selecting character id = " + charId.ToString());
            if (Jangada.getInstance().useProto)
            {
                Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
                newMessage.SelectCharacterPacket = SelectCharacterPacket.CreateBuilder()
                    .SetId(charId)
                    .Build();
                newMessage.Type = Networkmessage.Types.Type.SELECTEDCHAR;
                Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
                TCPClient.getInstance().Send(messagesToSend);
            }
            else
            {
                Network.NetworkMessage messageToSend = new Network.NetworkMessage();
                (new Network.ClientPackets.SelectedCharPacket(charId)).Add(messageToSend);
                TCPClient.getInstance().Send(messageToSend);
            }
        }

        public static void SendRequestMovement(RequestMovementPacket.Types.MovementType type, float ammount)
        {
            if (Jangada.getInstance().useProto)
            {
                Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
                newMessage.RequestMovementPacket = RequestMovementPacket.CreateBuilder()
                    .SetAmmount(ammount)
                    .SetMovementType(type)
                    .Build();
                newMessage.SetType(Networkmessage.Types.Type.REQUEST_MOVEMENT);
                TCPClient.getInstance().Send(Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build());
            }
            else
            {
                MovementType mType;
                switch (type)
                {
                    case RequestMovementPacket.Types.MovementType.FORWARD:
                        mType = MovementType.FORWARD;
                        break;
                    case RequestMovementPacket.Types.MovementType.BACKWARD:
                        mType = MovementType.BACKWARD;
                        break;
                    case RequestMovementPacket.Types.MovementType.YAW:
                        mType = MovementType.YAW;
                        break;
                    default:
                        mType = MovementType.FORWARD;
                        break;
                }

                Network.NetworkMessage messageToSend = new Network.NetworkMessage();
                (new Network.ClientPackets.RequestMovementPacket(mType, ammount)).Add(messageToSend);
                TCPClient.getInstance().Send(messageToSend);
            }
        }

    }
}
