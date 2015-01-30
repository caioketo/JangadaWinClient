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
        public static int HP_POSITION_X = 52;
        public static int HP_POSITION_Y = 3;
        public static int HP_WIDTH_MAX = 93;
        public static int HP_HEIGHT = 12;
        public static int MP_POSITION_X = 52;
        public static int MP_POSITION_Y = 19;
        public static int MP_WIDTH_MAX = 93;
        public static int MP_HEIGHT = 12;


        Texture2D background;
        Texture2D bgBars;
        Creature creature;

        public SmallPortrait(Texture2D background, Texture2D bgHP, Creature creature)
        {
            this.background = background;
            this.bgBars = bgHP;
            this.creature = creature;
        }

        public void Draw(SpriteBatch spriteBatch, int offset = 0)
        {
            spriteBatch.Draw(this.background, new Rectangle(offset, 0, 150, 50), Color.White);
            //HP
            spriteBatch.Draw(this.bgBars, new Rectangle(offset + HP_POSITION_X, HP_POSITION_Y, this.creature.GetHPPct(HP_WIDTH_MAX),
                HP_HEIGHT), new Rectangle(HP_POSITION_X, HP_POSITION_Y, this.creature.GetHPPct(), HP_HEIGHT), Color.Red);
            //MANA
            spriteBatch.Draw(this.bgBars, new Rectangle(offset + MP_POSITION_X, MP_POSITION_Y, this.creature.GetMPPct(MP_WIDTH_MAX),
                MP_HEIGHT), new Rectangle(MP_POSITION_X, MP_POSITION_Y, this.creature.GetMPPct(), MP_HEIGHT), Color.Blue);       
        }
    }
}
