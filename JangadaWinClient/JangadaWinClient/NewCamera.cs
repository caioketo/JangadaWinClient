using JangadaWinClient.Creatures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    public class NewCamera
    {
        float aspectRatio;
        public Player player
        {
            get
            {
                return Jangada.getInstance().world.player;
            }
        }
        public Matrix viewMatrix;
        public Matrix projectionMatrix;
        public Quaternion rotation = Quaternion.Identity;
        Vector3 position = new Vector3(0, 10.0f, -30.0f);

        public NewCamera(float aspectRatio)
        {
            this.aspectRatio = aspectRatio;
        }

        public void Pitch(float amount)
        {
            this.rotation *= Quaternion.CreateFromYawPitchRoll(0, MathHelper.ToRadians(amount), 0);
        }

        public void Yaw(float amount)
        {
            this.rotation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(amount), 0, 0);
        }

        public void Zoom(float amount)
        {
            if (amount > 0)
            {
                if (this.position.Y < 60f && this.position.Z > -60f)
                {
                    this.position += new Vector3(0, 2f, -6f);
                }
            }
            else
            {
                if (this.position.Y > 10f && this.position.Z < -30f)
                {
                    this.position += new Vector3(0, -2f, 6f);
                }
            }
        }

        public void Update()
        {
            Vector3 campos = this.position;
            campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(player.rotation * rotation));
            campos += player.position;
 
            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(player.rotation * rotation));
 
            viewMatrix = Matrix.CreateLookAt(campos, player.position, camup);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.2f, 500.0f);
        }
    }
}