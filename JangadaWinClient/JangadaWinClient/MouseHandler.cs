using Jangada;
using Microsoft.Xna.Framework;
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

        public MouseHandler()
        {
            previousMouseState = Mouse.GetState();
        }

        public void Update(NewCamera camera)
        {
            MouseState currentMouseState = Mouse.GetState();
            bool clicked = currentMouseState.LeftButton == ButtonState.Pressed && 
                previousMouseState.LeftButton == ButtonState.Released;
            bool holding = currentMouseState.LeftButton == ButtonState.Pressed && 
                previousMouseState.LeftButton == ButtonState.Pressed;

            if (clicked)
            {
                mouseClickedPos = new Vector2(currentMouseState.X, currentMouseState.Y);
            }

            if (holding)
            {
                Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
                camera.Pitch((mousePos.Y - mouseClickedPos.Y) / 10);
                float yaw = -1 * ((mousePos.X - mouseClickedPos.X) / 10);
                Util.getPlayer().Yaw(yaw);
                MessageHelper.SendRequestMovement(RequestMovementPacket.Types.MovementType.YAW, yaw);
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
    }
}
