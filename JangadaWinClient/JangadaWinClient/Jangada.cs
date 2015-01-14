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
using JangadaWinClient.Creatures;

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
        Camera camera;
        Dictionary<int, Terrain> terrains = new Dictionary<int,Terrain>();
        List<TerrainData> terrainDatas = new List<TerrainData>();
        public int mapIndex = 0;
        Player player;
        int previousScrollValue;

        public void setIsInMenu(bool inMenu)
        {
            if (inMenu)
            {
                mainMenu.Show();
            }
            else
            {
                mainMenu.Hide();
            }
            isInMenu = inMenu;
        }

        bool isInMenu = true;
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
            terrains.Add(1, new Terrain(GraphicsDevice));
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
            terrainDatas.Add(new TerrainData(Content.Load<Texture2D>("HM1"), Content.Load<Texture2D>("TEX1"))
                {
                    Id = 1
                });

            terrains[1].SetHeightMapData(terrainDatas[0]);
            CreateControls();
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            player = new Player(Content.Load<Model>("human"), graphics.GraphicsDevice.Viewport.AspectRatio);
            // initialize camera start position
            camera = new Camera(new Vector3(0, -100, 256), player);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState key = Keyboard.GetState();
            // Allows the game to exit
            if (key.IsKeyDown(Keys.Escape))
                this.Exit();


            // move camera position with keyboard            
            if (key.IsKeyDown(Keys.A))
            {
                camera.Update(1);
            }
            if (key.IsKeyDown(Keys.D))
            {
                camera.Update(2);
            }
            if (key.IsKeyDown(Keys.W))
            {
                camera.Update(3);
            }
            if (key.IsKeyDown(Keys.S))
            {
                camera.Update(4);
            }
            if (key.IsKeyDown(Keys.F))
            {
                camera.Update(5);
            }
            if (key.IsKeyDown(Keys.R))
            {
                camera.Update(6);
            }
            if (key.IsKeyDown(Keys.Q))
            {
                camera.Update(7);
            }
            if (key.IsKeyDown(Keys.E))
            {
                camera.Update(8);
            }
            if (key.IsKeyDown(Keys.G))
            {
                camera.Update(9);
            }
            if (key.IsKeyDown(Keys.T))
            {
                camera.Update(10);
            }

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.ScrollWheelValue < previousScrollValue)
            {
                camera.SetZoom(0);
            }
            else if (currentMouseState.ScrollWheelValue > previousScrollValue)
            {
                camera.SetZoom(1);
            }
            previousScrollValue = currentMouseState.ScrollWheelValue;

            // TODO: Add your update logic here
            manager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            manager.BeginDraw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (isInMenu)
            {
                DrawScenery();
            }
            else
            {
                camera.Draw(terrains[mapIndex]);
                player.Draw(camera.Position);
            }
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
