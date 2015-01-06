using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TomShane.Neoforce.Controls;

namespace JangadaWinClient
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Jangada : Microsoft.Xna.Framework.Game
    {
        private static Jangada instance;

        public static Jangada getInstance()
        {
            return instance;
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Manager manager;
        MainMenu mainMenu;
        public LoginWindow loginWindow;
        TomShane.Neoforce.Controls.Console consoleWindow;
        Texture2D background;
        GraphicsDevice device;

        int screenWidth;
        int screenHeight;

        public void AddLog(string message)
        {
            consoleWindow.MessageBuffer.Add(new ConsoleMessage("", message, 0));
        }

        public Jangada()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            manager = new Manager(this, graphics);
            manager.Initialize();
            manager.AutoCreateRenderTarget = false;
            new TCPClient(7777);
            base.Initialize();
        }


        void loginBtn_click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            loginWindow.Show();
        }

        public void CreateControls()
        {
            consoleWindow = new TomShane.Neoforce.Controls.Console(manager);
            consoleWindow.Top = 320;
            consoleWindow.Width = 400;
            consoleWindow.Init();
            consoleWindow.TextBoxVisible = false;
            consoleWindow.Name = "ConsoleWindow";
            manager.Add(consoleWindow);
            consoleWindow.Channels.Add(new ConsoleChannel(0, "DebugMsg", Color.Yellow));
            mainMenu = new MainMenu(manager);
            mainMenu.Init();
            Button loginBtn = new Button(manager);
            loginBtn.Init();
            loginBtn.Width = 100;
            loginBtn.Text = "Login";
            loginBtn.Click += new TomShane.Neoforce.Controls.EventHandler(this.loginBtn_click);
            mainMenu.Add(loginBtn);
            mainMenu.Width = 100;
            manager.Add(mainMenu);
            loginWindow = new LoginWindow(manager);
            loginWindow.Init();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            background = Content.Load<Texture2D>("background");
            CreateControls();
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            // Allows the game to exit
            if (state.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            manager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            manager.BeginDraw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            DrawScenery();
            spriteBatch.End();
            
            manager.EndDraw();

            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(background, screenRectangle, Color.White);
        }
    }
}
