using JangadaWinClient.Creatures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    class Camera
    {
        Vector3 thirdPersonReference = new Vector3(0, 100, -100);
        Matrix viewMatrix;
        Matrix projectionMatrix;
        public Matrix terrainMatrix;
        Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        public void SetZoom(int direction)
        {
            if (direction == 0)
            {
                thirdPersonReference += new Vector3(0, -10, 10);
            }
            else
            {
                thirdPersonReference += new Vector3(0, 10, -10);
            }
            Vector3 transformedReference = Vector3.Transform(thirdPersonReference, rotationMatrix);
            this.position = transformedReference + this.player.Position;
            viewMatrix = Matrix.CreateLookAt(this.position, this.player.Position, new Vector3(0.0f, 1.0f, 0.0f));
        }

        Player player;
        Matrix rotationMatrix;
        public Camera(Vector3 landscapePosition, Player _player)
        {
            this.player = _player;
            rotationMatrix = Matrix.CreateRotationY(this.player.Rotation);
            Vector3 transformedReference = Vector3.Transform(thirdPersonReference, rotationMatrix);
            this.position = transformedReference + this.player.Position;
            viewMatrix = Matrix.CreateLookAt(this.position, this.player.Position, new Vector3(0.0f, 1.0f, 0.0f));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.player.AspectRatio,
                1.0f, 1000.0f);
            terrainMatrix = Matrix.CreateTranslation(landscapePosition);
        }
 
        public void Update(int number)
        {
            /*Vector3 tempMovement = Vector3.Zero;
            Vector3 tempRotation = Vector3.Zero;
            //left
            if (number == 1)
            {
                tempMovement.X = +movement.X;
            }
            //right
            if (number == 2)
            {
                tempMovement.X = -movement.X;
            }
            //up
            if (number == 3)
            {
                tempMovement.Y = -movement.Y;
            }
            //down
            if (number == 4)
            {
                tempMovement.Y = +movement.Y;
            }
            //backward (zoomOut)
            if (number == 5)
            {
                tempMovement.Z = -movement.Z;
            }
            //forward (zoomIn)
            if (number == 6)
            {
                tempMovement.Z = +movement.Z;
            }
            //left rotation
            if (number == 7)
            {
                tempRotation.Y = -rotation.Y;
            }
            //right rotation
            if (number == 8)
            {
                tempRotation.Y = +rotation.Y;
            }
            //forward rotation
            if (number == 9)
            {
                tempRotation.X = -rotation.X;
            }
            //backward rotation
            if (number == 10)
            {
                tempRotation.X = +rotation.X;
            }
 
            //move camera to new position
            viewMatrix = viewMatrix * Matrix.CreateRotationX(tempRotation.X) * Matrix.CreateRotationY(tempRotation.Y) * Matrix.CreateTranslation(tempMovement);
            //update position
            position += tempMovement;
            direction += tempRotation;*/
        }
 
        public void SetEffects(BasicEffect basicEffect)
        {
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
            basicEffect.World = terrainMatrix;
        }
 
        public void Draw(Terrain terrain)
        {
            SetEffects(terrain.basicEffect);
            foreach (EffectPass pass in terrain.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
            }
        }
    }
}
