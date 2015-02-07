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
        public static bool Parse(Networkmessage.Types.Type type, Networkmessage message)
        {
            Player nPlayer = null;
            switch (type)
            {
                case Networkmessage.Types.Type.CHARACTERS:
                    Jangada.getInstance().AddLog(message.CharactersPacket.CharacterListCount.ToString());
                    Jangada.getInstance().loginWindow.ShowCharList(message.CharactersPacket.CharacterListList.ToList());
                    break;
                case Networkmessage.Types.Type.AREA_DESCRIPTION:
                    Jangada.getInstance().loginWindow.HideConnecting();
                    Jangada.getInstance().MapIndex = message.AreaDescriptionPacket.AreaId;
                    Util.getPlayer().SetDescription(message.AreaDescriptionPacket.Player);
                    if (message.AreaDescriptionPacket.PlayersCount > 0)
                    {
                        foreach (PlayerDescription playerDesc in message.AreaDescriptionPacket.PlayersList)
                        {
                            Util.getWorld().AddPlayer(new Player(Jangada.getInstance().humanModel, playerDesc));
                        }
                    }

                    if (message.AreaDescriptionPacket.CreaturesCount > 0)
                    {
                        foreach (CreatureDescription creatureDesc in message.AreaDescriptionPacket.CreaturesList)
                        {
                            Util.getWorld().AddCreature(new Creature(creatureDesc));
                        }
                    }
                    Jangada.getInstance().setIsInMenu(false);
                    break;
                case Networkmessage.Types.Type.CREATURE_RESPAWN:
                    Util.getWorld().AddCreature(new Creature(message.CreatureRespawnPacket.CreatureDescription));
                    break;
                case Networkmessage.Types.Type.PLAYER_LOGIN:
                    Util.getWorld().AddPlayer(new Player(Jangada.getInstance().humanModel, message.PlayerLoginPacket.Player));
                    break;
                case Networkmessage.Types.Type.PLAYER_MOVEMENT:
                    Util.getPlayer().position = Util.fromPosition(message.PlayerMovementPacket.NewPosition);
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
