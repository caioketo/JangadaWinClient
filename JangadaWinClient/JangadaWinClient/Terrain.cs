using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    public class Terrain
    {
        GraphicsDevice graphicsDevice;

        // heightMap
        Texture2D heightMap;
        Texture2D heightMapTexture;
        VertexPositionTexture[] vertices;
        int width;
        int height;

        public BasicEffect basicEffect;
        int[] indices;

        // array to read heightMap data
        float[,] heightMapData;



        public Terrain(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void SetHeightMapData(TerrainData data)
        {
            this.heightMap = data.HeightMap;
            this.heightMapTexture = data.Texture;
            width = heightMap.Width;
            height = heightMap.Height;
            SetHeights();
            SetVertices();
            SetIndices();
            SetEffects();
        }

        public void SetHeights()
        {
            Color[] greyValues = new Color[width * height];
            heightMap.GetData(greyValues);
            heightMapData = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightMapData[x, y] = greyValues[x + y * width].G / 3.1f;
                }
            }
        }

        public void SetIndices()
        {
            // amount of triangles
            indices = new int[6 * (width - 1) * (height - 1)];
            int number = 0;
            // collect data for corners
            for (int y = 0; y < height - 1; y++)
                for (int x = 0; x < width - 1; x++)
                {
                    // create double triangles
                    indices[number] = x + (y + 1) * width;      // up left
                    indices[number + 1] = x + y * width + 1;        // down right
                    indices[number + 2] = x + y * width;            // down left
                    indices[number + 3] = x + (y + 1) * width;      // up left
                    indices[number + 4] = x + (y + 1) * width + 1;  // up right
                    indices[number + 5] = x + y * width + 1;        // down right
                    number += 6;
                }
        }

        public void SetVertices()
        {
            vertices = new VertexPositionTexture[width * height];
            Vector2 texturePosition;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texturePosition = new Vector2((float)x / 25.5f, (float)y / 25.5f);
                    vertices[x + y * width] = new VertexPositionTexture(new Vector3(x, heightMapData[x, y], -y), texturePosition);
                }
                //graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionTexture.VertexElements);
            }
            graphicsDevice.SetVertexBuffer(new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None));
        }



        public void SetEffects()
        {
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.Texture = heightMapTexture;
            basicEffect.TextureEnabled = true;
        }

        public void Draw()
        {
            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);
        }

    }
}
