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
            int y = 0;
            for (y = 0; y < 20; y++)
            {
                int x;
                for (x = 0; x < Grid.GetLength(0); x++)
                {
                    Grid[x, y] = String.Empty;
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
                m_ActiveBlock = new Shape1x4(new Vector2(180, 0));
                int TargetX = m_ActiveBlock.GetWidth();
                scale = new Vector2(TargetX / (float)LegoBlue.Width, TargetX / (float)LegoBlue.Width);
                IsBlockActive = true;
            }

            //Move block horizontally
            if (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left) && Grid[m_ActiveBlock.GetGridPosX() - 1, m_ActiveBlock.GetGridPosY()] == string.Empty)
            {
                m_ActiveBlock.MoveHorizontal(-m_ActiveBlock.GetWidth());
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right) && Grid[m_ActiveBlock.GetGridPosX() + 1, m_ActiveBlock.GetGridPosY()] == string.Empty)
            {
                m_ActiveBlock.MoveHorizontal(m_ActiveBlock.GetWidth());
            }
            //Move block down
            if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down) && m_ActiveBlock.GetPosY() != 475)
            {
                if(Grid[m_ActiveBlock.GetGridPosX(), (m_ActiveBlock.GetGridPosY() + 1)] == String.Empty)
                {
                    m_ActiveBlock.Fall();
                }                
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

            //Remove empty rows
            int y;
            for (y = 1; y < 20; y++)
            {
                int x;
                int EmptyInRow = 0;
                for (x = 0; x < 12; x++)
                {
                    if (Grid[x, y] == string.Empty)
                    {
                        EmptyInRow++;
                        if (EmptyInRow == 12)
                        {
                            for (x = 0; x < 12; x++)
                            {
                                Grid[x, y] = Grid[x, y - 1];
                                Grid[x, y - 1] = string.Empty;
                            }
                        }
                    }
                    else if (Grid[x, y] != string.Empty)
                    {
                        EmptyInRow = 0;
                    }
                }
            }

            base.Update(gameTime);
        }

        private void Everysecond(object source, ElapsedEventArgs e)
        {
            if (m_ActiveBlock.GetMaxPosY() != graphics.GraphicsDevice.Viewport.Height && m_ActiveBlock.GetMaxPosY() < graphics.GraphicsDevice.Viewport.Height && Grid[m_ActiveBlock.GetGridPosX(), (m_ActiveBlock.GetGridPosY() + 1)] == String.Empty)
            {
                m_ActiveBlock.Fall();
            }
            //Lock block
            else if (m_ActiveBlock.GetMaxPosY() == graphics.GraphicsDevice.Viewport.Height || Grid[m_ActiveBlock.GetGridPosX(), (m_ActiveBlock.GetGridPosY() +1)] != String.Empty)
            {
                //Set grid value to the color of activeblock           
                int i = 0;
                for (i = 0; i < 4; i++)
                {
                    Grid[m_ActiveBlock.GetNextGridPosX(i), m_ActiveBlock.GetNextGridPosY(i)] = m_ActiveBlock.GetColor();
                }
                IsBlockActive = false;
                int InARow = 0;
                int FullRow = 12;                
                int y;
                for (y = 0; y < 20; y++)
                {
                    int x = 0;
                    InARow = 0;
                    for (x = 0; x < 12; x++)
                    {
                        if(Grid[x,y] != string.Empty)
                        {
                            InARow++;                            
                            if (InARow == 12)
                            {
                                FullRow = y;                            
                            }
                        }
                        else
                        {
                            InARow = 0;                            
                        }
                    }
                    if (InARow == 12)
                    {
                        for (x = 0; x < 12; x++)
                        {
                            Grid[x, y] = string.Empty;
                        }

                    }
                }
                if (InARow == 12)
                {
                    for (y = FullRow - 1; y >= 0; y--)
                    {
                        int x;
                        for (x = 0; x < 12; x++)
                        {
                            if (Grid[x, y + 1] == string.Empty)
                            {
                                Grid[x, y + 1] = Grid[x, y];
                                Grid[x, y] = string.Empty;
                            }                            
                        }                        
                    }
                }
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
                    if (Grid[o, p] != String.Empty)
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
