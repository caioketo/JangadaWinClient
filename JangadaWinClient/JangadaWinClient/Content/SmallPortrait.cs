using JangadaWinClient.Creatures;
using JangadaWinClient.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Content
{
    public class SmallPortrait
    {
        Texture2D background;
        NewPlayer player;
        CustomRectangle HPRect;

        public SmallPortrait(Texture2D background, NewPlayer player)
        {
            this.background = background;
            this.player = player;
            this.HPRect = new Utils.CustomRectangle(new Rectangle(47, 2, 95, 12), Color.Green); ;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            HPRect.Width = (player.GetHPPct() * 100) / 95;
            spriteBatch.Draw(this.background, new Rectangle(0, 0, 150, 50), Color.White);
            HPRect.Draw(spriteBatch);
        }

        public void Load(GraphicsDevice GraphicsDevice)
        {
            HPRect.LoadContent(GraphicsDevice);
        }
    }
}
