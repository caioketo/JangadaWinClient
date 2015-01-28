using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Utils
{
    public class CustomRectangle
    {
        Texture2D texture;
        Rectangle rectangle;
        Color Colori;

        public CustomRectangle(Rectangle rect, Color colori)
        {
            rectangle = rect;
            Colori = colori;
        }

        public int Width
        {
            set
            {
                rectangle.Width = value;
            }
        }

        public void LoadContent(GraphicsDevice GraphicsDevice)
        {
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Colori);
        }
    }
}
