using Jangada;
using JangadaWinClient.Creatures;
using JangadaWinClient.Enums;
using JangadaWinClient.Network;
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
        public static bool Parse(byte type, NetworkMessage message)
        {
            switch ((ServerMessageType)type)
            {
                case ServerMessageType.Characters:
                    Network.ServerPackets.CharactersPacket charactersPacket = Network.ServerPackets.CharactersPacket.Parse(message);
                    break;
                case ServerMessageType.AreaDescription:
                    Network.ServerPackets.AreaDescriptionPacket areaDescPacket = Network.ServerPackets.AreaDescriptionPacket.Parse(message);
                    break;
                case ServerMessageType.PlayerLogin:
                    Network.ServerPackets.PlayerLoginPacket playerLoginPacket = Network.ServerPackets.PlayerLoginPacket.Parse(message);
                    break;
                case ServerMessageType.PlayerMovement:
                    Network.ServerPackets.PlayerMovementPacket playerMovementPacket = Network.ServerPackets.PlayerMovementPacket.Parse(message);
                    break;
                case ServerMessageType.CharacterMovement:
                    Network.ServerPackets.CharacterMovementPacket characterMovementPacket = Network.ServerPackets.CharacterMovementPacket.Parse(message);
                    break;
                case ServerMessageType.PlayerLogout:
                    Network.ServerPackets.PlayerLogoutPacket playerLogoutPacket = Network.ServerPackets.PlayerLogoutPacket.Parse(message);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool Parse(Networkmessage.Types.Type type, Networkmessage message)
        {
            NewPlayer nPlayer = null;
            switch (type)
            {
                case Networkmessage.Types.Type.CHARACTERS:
                    Jangada.getInstance().AddLog(message.CharactersPacket.CharacterListCount.ToString());
                    Jangada.getInstance().loginWindow.ShowCharList(message.CharactersPacket.CharacterListList.ToList());
                    break;
                case Networkmessage.Types.Type.AREA_DESCRIPTION:
                    Jangada.getInstance().loginWindow.HideConnecting();
                    Jangada.getInstance().MapIndex = message.AreaDescriptionPacket.AreaId;
                    Util.getPlayer().position = Util.fromPosition(message.AreaDescriptionPacket.Player.PlayerPosition);
                    Util.getPlayer().rotation = Util.fromQuaternionMessage(message.AreaDescriptionPacket.Player.PlayerRotation);
                    Util.getPlayer().Guid = message.AreaDescriptionPacket.Player.PlayerGuid;
                    if (message.AreaDescriptionPacket.PlayersCount > 0)
                    {
                        foreach (PlayerDescription playerDesc in message.AreaDescriptionPacket.PlayersList)
                        {
                            NewPlayer fplayer = new NewPlayer(Jangada.getInstance().humanModel);
                            fplayer.Guid = playerDesc.PlayerGuid;
                            fplayer.position = Util.fromPosition(playerDesc.PlayerPosition);
                            fplayer.rotation = Util.fromQuaternionMessage(playerDesc.PlayerRotation);
                            Util.getWorld().players.Add(fplayer);
                        }
                    }
                    Jangada.getInstance().setIsInMenu(false);
                    break;
                case Networkmessage.Types.Type.PLAYER_LOGIN:
                    NewPlayer player = new NewPlayer(Jangada.getInstance().humanModel);
                    player.Guid = message.PlayerLoginPacket.Player.PlayerGuid;
                    player.position = Util.fromPosition(message.PlayerLoginPacket.Player.PlayerPosition);
                    player.rotation = Util.fromQuaternionMessage(message.PlayerLoginPacket.Player.PlayerRotation);
                    Util.getWorld().players.Add(player);
                    break;
                case Networkmessage.Types.Type.PLAYER_MOVEMENT:
                    Jangada.getInstance().AddLog("Char pos: " + Util.getPlayer().position.ToString());
                    Util.getPlayer().position = Util.fromPosition(message.PlayerMovementPacket.NewPosition);
                    Jangada.getInstance().AddLog("Char newPos: " + Util.getPlayer().position.ToString());
                    Util.getPlayer().rotation = Util.fromQuaternionMessage(message.PlayerMovementPacket.NewRotation);
                    Util.getPlayer().RecreateWorld();
                    break;
                case Networkmessage.Types.Type.CHARACTER_MOVEMENT:
                    nPlayer = Util.getWorld().FindPlayerByGuid(message.CharacterMovementPacket.Player.PlayerGuid);
                    if (nPlayer != null)
                    {
                        nPlayer.position = Util.fromPosition(message.CharacterMovementPacket.Player.PlayerPosition);
                        nPlayer.rotation = Util.fromQuaternionMessage(message.CharacterMovementPacket.Player.PlayerRotation);
                        nPlayer.RecreateWorld();
                    }
                    break;
                case Networkmessage.Types.Type.PLAYER_LOGOUT:
                    nPlayer = Util.getWorld().FindPlayerByGuid(message.CharacterMovementPacket.Player.PlayerGuid);
                    if (nPlayer != null)
                    {
                        Util.getWorld().RemovePlayer(nPlayer);
                    }

                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
