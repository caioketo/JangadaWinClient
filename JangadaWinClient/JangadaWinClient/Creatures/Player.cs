using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Creatures
{
    public class Player
    {
        Model model;
        float aspectRatio;
        public float AspectRatio
        {
            get
            {
                return aspectRatio;
            }
        }

        Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        float rotation = 0.0f;
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        public Player(Model _model, float _aspectRatio)
        {
            model = _model;
            aspectRatio = _aspectRatio;
            foreach (ModelBone bone in model.Bones)
            {
                bone.Transform *= Matrix.CreateScale(0.15f);
            }
        }

        public void Draw(Vector3 cameraPosition)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateRotationY(rotation)
                        * Matrix.CreateTranslation(position);
                    effect.View = Matrix.CreateLookAt(cameraPosition,
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio,
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}
