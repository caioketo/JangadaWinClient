using Jangada;
using JangadaWinClient.Creatures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    public class Util
    {

        public static Vector3 fromPosition(Position pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
        }

        public static Quaternion fromQuaternionMessage(QuaternionMessage quaMessage)
        {
            return new Quaternion(quaMessage.X, quaMessage.Y, quaMessage.Z, quaMessage.W);
        }

        public static NewPlayer getPlayer()
        {
            return Jangada.getInstance().GetCamera().player;
        }

        public static World getWorld()
        {
            return Jangada.getInstance().world;
        }

        public static NewCamera getCamera()
        {
            return Jangada.getInstance().GetCamera();
        }
    }
}
