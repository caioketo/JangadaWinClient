using JangadaWinClient.Creatures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Content
{
    public class SkillBars
    {
        public static int BAR_1_POSITION_X = 0;
        public static int BAR_1_POSITION_Y = 450;

        private Player player;
        private Texture2D background;

        public SkillBars(Texture2D background, Player player)
        {
            this.background = background;
            this.player = player;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.background, new Rectangle(BAR_1_POSITION_X, BAR_1_POSITION_Y, 500, 50), Color.White);
            foreach (Skill skill in player.Skills)
            {
                spriteBatch.Draw(Jangada.getInstance().textures[skill.TextureId], new Rectangle(BAR_1_POSITION_X + 2,
                    BAR_1_POSITION_Y, 50, 50), Color.White);
            }
        }
    }
}
