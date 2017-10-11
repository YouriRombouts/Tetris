using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Block m_ActiveBlock;
        Texture2D LegoBlue, LegoBaby;
        Vector2 scale;
        KeyboardState previousKeyboardState, currentKeyboardState;
        string[,] Grid;
        bool IsBlockActive;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 360;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IsBlockActive = false;

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(Everysecond);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

            Grid = new string[12, 20];
            int p = 0;
            for (p = 0; p < Grid.GetLength(1); p++)
            {
                int o = 0;
                for (o = 0; o < Grid.GetLength(0); o++)
                {
                    Grid[o, p] = "Empty";
                }
            }
            LegoBlue = Content.Load<Texture2D>("legoblue");
            LegoBaby = Content.Load<Texture2D>("legobaby");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public void HandleInput()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandleInput();
            //Rotate block
            if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                m_ActiveBlock.AddRotation();
            }
            //Spawn m_ActiveBlock
            if (IsBlockActive == false)
            {
                m_ActiveBlock = new Shape1x4(new Vector2(0, 0));
                int TargetX = m_ActiveBlock.GetWidth();
                scale = new Vector2(TargetX / (float)LegoBlue.Width, TargetX / (float)LegoBlue.Width);
                IsBlockActive = true;
            }


            //Move block horizontally
            if (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
            {
                m_ActiveBlock.MoveHorizontal(-m_ActiveBlock.GetWidth());
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
            {
                m_ActiveBlock.MoveHorizontal(m_ActiveBlock.GetWidth());
            }
            //Move block down
            if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                m_ActiveBlock.Fall();
            }
            //Make sure the block stays in screen vertically
            if (m_ActiveBlock.GetMaxPosY() > graphics.GraphicsDevice.Viewport.Height)
            {
                m_ActiveBlock.GBISY();
            }
            //Make sure the block stays in screen horizontally
            if (m_ActiveBlock.GetMaxPosX() > graphics.GraphicsDevice.Viewport.Width)
            {
                m_ActiveBlock.GBISX();
            }
            else if (m_ActiveBlock.GetPosX() < 0)
            {
                m_ActiveBlock.SetPosX(0);
            }


            base.Update(gameTime);
        }

        private void Everysecond(object source, ElapsedEventArgs e)
        {
            if (m_ActiveBlock.GetMaxPosY() != graphics.GraphicsDevice.Viewport.Height && m_ActiveBlock.GetMaxPosY() < graphics.GraphicsDevice.Viewport.Height)
            {
                m_ActiveBlock.Fall();
            }
            //Lock block
            else if (m_ActiveBlock.GetMaxPosY() == graphics.GraphicsDevice.Viewport.Height)
            {
                //Set grid value to the color of activeblock
                m_ActiveBlock.GridPos = new Vector2((m_ActiveBlock.GetPosX() / m_ActiveBlock.GetWidth()), (m_ActiveBlock.GetPosY() / m_ActiveBlock.GetHeight()));
                Grid[m_ActiveBlock.GetGridPosX(), m_ActiveBlock.GetGridPosY()] = m_ActiveBlock.GetColor();
                Grid[m_ActiveBlock.GetGridPosX(), m_ActiveBlock.GetGridPosY()] = m_ActiveBlock.GetColor();
                Grid[m_ActiveBlock.GetGridPosX(), m_ActiveBlock.GetGridPosY()] = m_ActiveBlock.GetColor();
                Grid[m_ActiveBlock.GetGridPosX(), m_ActiveBlock.GetGridPosY()] = m_ActiveBlock.GetColor();
                IsBlockActive = false;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            int p = 0;
            for (p = Grid.GetLength(1) - 1; p >= 0; p--)
            {
                int o = 0;
                for (o = Grid.GetLength(0) - 1; o >= 0; o--)
                {
                    if (Grid[o, p] != "Empty")
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>(Grid[o, p]), new Vector2(o * 30, p * 25), scale: scale);
                    }
                }
            }
            m_ActiveBlock.Draw(spriteBatch, scale, LegoBaby);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
