using Jangada;
using JangadaWinClient.Creatures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient
{
    public class MouseHandler
    {
        MouseState previousMouseState;
        Vector2 mouseClickedPos;
        int previousScrollValue;
        GraphicsDevice GraphicsDevice;

        public MouseHandler(GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
            previousMouseState = Mouse.GetState();
        }

        public void Update(NewCamera camera)
        {
            MouseState currentMouseState = Mouse.GetState();
            bool clicked = currentMouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released;

            bool clickedR = currentMouseState.RightButton == ButtonState.Pressed &&
                previousMouseState.RightButton == ButtonState.Released;

            bool holdingL = currentMouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Pressed;
            bool holdingR = currentMouseState.RightButton == ButtonState.Pressed &&
                previousMouseState.RightButton == ButtonState.Pressed;

            if (clickedR || clicked)
            {
                mouseClickedPos = new Vector2(currentMouseState.X, currentMouseState.Y);
            }
            if (clicked)
            {
                Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
                Ray cursorRay = CalculateCursorRay(Util.getCamera().projectionMatrix, Util.getCamera().viewMatrix, mousePos);
                float? lowValue = null;
                Creature selectedCreature = null;
                foreach (Creature creature in Util.getWorld().creatures)
                {
                    float? curValue = creature.Intersects(cursorRay);
                    if (curValue.HasValue && (!lowValue.HasValue || lowValue.Value > curValue.Value))
                    {
                        lowValue = curValue;
                        selectedCreature = creature;
                    }
                }
                if (selectedCreature != null)
                {
                    Jangada.getInstance().AddLog("Clicou no player de GUID: " + selectedCreature.Guid);
                }
                Util.getWorld().selectedCreature = selectedCreature;
            }

            if (holdingR)
            {
                Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
                camera.Pitch((mousePos.Y - mouseClickedPos.Y) / 10);
                float yaw = -1 * ((mousePos.X - mouseClickedPos.X) / 10);
                Util.getPlayer().Yaw(yaw);
                MessageHelper.SendRequestMovement(RequestMovementPacket.Types.MovementType.YAW, yaw);
                camera.Update();
                mouseClickedPos = mousePos;
            }

            if (holdingL)
            {
                Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
                camera.Pitch((mousePos.Y - mouseClickedPos.Y) / 10);
                float yaw = -1 * ((mousePos.X - mouseClickedPos.X) / 10);
                Util.getCamera().Yaw(yaw);
                //MessageHelper.SendRequestMovement(RequestMovementPacket.Types.MovementType.YAW, yaw);
                camera.Update();
                mouseClickedPos = mousePos;
            }

            if (currentMouseState.ScrollWheelValue < previousScrollValue)
            {
                camera.Zoom(1);
            }
            else if (currentMouseState.ScrollWheelValue > previousScrollValue)
            {
                camera.Zoom(-1);
            }
            previousScrollValue = currentMouseState.ScrollWheelValue;

            previousMouseState = currentMouseState;
        }

        public Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix, Vector2 mousePosition)
        {
            //Position is your mouse position
            Vector3 nearSource = new Vector3(mousePosition, 0f);
            Vector3 farSource = new Vector3(mousePosition, 1f);

            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }
    }
}
