using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Creatures
{
    public class NewPlayer
    {
        Model model;
        public string Guid;
        public Vector3 position = new Vector3(8, 1, -3);
        public Quaternion rotation = Quaternion.Identity;
        public Matrix worldMatrix = Matrix.Identity;
        int health;
        int maxHealth;
        public int GetHPPct()
        {
            return (health * 100) / maxHealth;
        }

        public NewPlayer(Model _model)
        {
            model = _model;
            this.health = 100;
            this.maxHealth = 100;
        }


        public void RecreateWorld()
        {
            worldMatrix = Matrix.CreateRotationY(MathHelper.Pi)
                * Matrix.CreateFromQuaternion(rotation)
                * Matrix.CreateTranslation(position);
        }

        public void Draw(NewCamera camera)
        {
            RecreateWorld();
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index]
                        * worldMatrix;
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();
            }
        }

        public void MoveForward(float speed)
        {   
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, 1), this.rotation);
            this.position += addVector * speed;
        }

        public void MoveBackward(float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), this.rotation);
            this.position += addVector * speed;
        }

        public void Yaw(float amount)
        {
            this.rotation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(amount), 0, 0);
        }

        public void ChangeBoneTransform(int boneIndex, Matrix t)
        {
            model.Bones[boneIndex].Transform = t * model.Bones[boneIndex].Transform;
        }
    }
}
