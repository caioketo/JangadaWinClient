using Jangada;
using JangadaWinClient.Content;
using JangadaWinClient.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Creatures
{
    public class Player : Creature
    {
        public Player(Model _model, PlayerDescription playerDesc) : base(_model, playerDesc)
        {
        }
        public Player(Model _model) : base(_model)
        {            
        }
        
    }
}
