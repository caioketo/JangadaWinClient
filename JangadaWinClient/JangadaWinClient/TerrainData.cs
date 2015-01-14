using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    public class TerrainData
    {
        public Texture2D HeightMap;
        public Texture2D Texture;
        public int Id;

        public TerrainData(Texture2D HM, Texture2D TEX)
        {
            HeightMap = HM;
            Texture = TEX;
        }

    }
}
