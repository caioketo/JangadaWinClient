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
using Jangada;
using JangadaWinClient.Content;
using JangadaWinClient.Utils;

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

        MouseHandler mouseHandler;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Manager manager;
        MainMenu mainMenu;
        public LoginWindow loginWindow;
        TomShane.Neoforce.Controls.Console consoleWindow;
        Texture2D background;
        public GraphicsDevice device;
        NewCamera newCamera;
        public World world;
        Dictionary<int, Terrain> terrains = new Dictionary<int, Terrain>();
        public Dictionary<int, Model> models = new Dictionary<int, Model>();
        public Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();
        List<TerrainData> terrainDatas = new List<TerrainData>();
        int mapIndex;
        public Model humanModel;
        public bool useProto = true;
        SkillBars skills;

        public int MapIndex
        {
            get
            {
                return mapIndex;
            }

            set
            {
                mapIndex = value;
                world.SetTerrain(terrains[mapIndex]);
            }
        }

        public NewCamera GetCamera()
        {
            return newCamera;
        }

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
            terrains.Add(1, new Terrain(GraphicsDevice, new Vector3(0, 0, 0)));
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
            TexturesHolder.BG_BARS = Content.Load<Texture2D>("bghp");
            TexturesHolder.BG_SMALL_PORTRAIT = Content.Load<Texture2D>("bgsp");
            TexturesHolder.LINE_TEXT = new Texture2D(GraphicsDevice, 1, 1);
            TexturesHolder.LINE_TEXT.SetData(new[] { Color.White });
            terrainDatas.Add(new TerrainData(Content.Load<Texture2D>("HM2"), Content.Load<Texture2D>("TEX1"))
                {
                    Id = 1
                });

            terrains[1].SetHeightMapData(terrainDatas[0]);
            CreateControls();
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            humanModel = Content.Load<Model>("human");
            foreach (ModelBone bone in humanModel.Bones)
            {
                bone.Transform *= Matrix.CreateScale(0.08f);
            }
            models.Add(1, humanModel);
            textures.Add(1, Content.Load<Texture2D>("1"));
            Player player = new Player(humanModel);
            mouseHandler = new MouseHandler(device);
            world = new World(player);
            newCamera = new NewCamera(graphics.GraphicsDevice.Viewport.AspectRatio);
            skills = new SkillBars(Content.Load<Texture2D>("skillsbar"), player);
            
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

            if (!isInMenu)
            {
                if (key.IsKeyDown(Keys.W))
                {
                    newCamera.player.MoveForward(0.5f);
                    MessageHelper.SendRequestMovement(RequestMovementPacket.Types.MovementType.FORWARD, 0.5f);
                }
                if (key.IsKeyDown(Keys.S))
                {
                    newCamera.player.MoveBackward(0.5f);
                    MessageHelper.SendRequestMovement(RequestMovementPacket.Types.MovementType.BACKWARD, 0.5f);
                }
                if (key.IsKeyDown(Keys.A))
                {
                    newCamera.player.Yaw(1f);
                    MessageHelper.SendRequestMovement(RequestMovementPacket.Types.MovementType.YAW, 1f);
                }
                if (key.IsKeyDown(Keys.D))
                {
                    newCamera.player.Yaw(-1f);
                    MessageHelper.SendRequestMovement(RequestMovementPacket.Types.MovementType.YAW, -1f);
                }
                if (key.IsKeyDown(Keys.Q))
                {
                    newCamera.player.ChangeBoneTransform(1, Matrix.CreateRotationZ(0.1f));
                }
                mouseHandler.Update(newCamera);
            }

            // TODO: Add your update logic here
            manager.Update(gameTime);
            newCamera.Update();
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            manager.BeginDraw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch.Begin();
            if (isInMenu)
            {
                DrawScenery();
            }
            spriteBatch.End();

            
            GraphicsDevice.BlendState = BlendState.Opaque;   
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            if (!isInMenu)
            {
                world.Draw(newCamera);
            }
            //spriteBatch.End();
            
            manager.EndDraw();

            spriteBatch.Begin();
            if (!isInMenu)
            {
                Util.getWorld().player.smallPortrait.Draw(spriteBatch);
                if (Util.getWorld().selectedCreature != null)
                {
                    Util.getWorld().selectedCreature.smallPortrait.Draw(spriteBatch, 300);
                }
                skills.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(background, screenRectangle, Color.White);
        }
    }
}
