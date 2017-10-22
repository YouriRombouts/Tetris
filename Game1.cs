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
        SpriteFont Font;
        Block m_ActiveBlock;
        Texture2D LegoBlue, LegoBaby, LegoPurple, ActiveColor, SideMenu;
        Vector2 scale;
        KeyboardState previousKeyboardState, currentKeyboardState;
        string[,] Grid;
        bool IsBlockActive, IsLocked;
        Point screen;
        int /*TargetX,*/ Score, TimeInterval = 1000, BlocksSet, Level;
        float TwoTenthSecond;
        string DrawScore, DrawLevel;

        enum Gamestate
        {
            MainMenu,
            //Options,
            Playing,
            GameOver,
        }

        Gamestate CurrentGameState = Gamestate.MainMenu;

        public void SetFullScreen(bool fullscreen = true)
        {
            screen = new Point(560, 500);
            float scalex = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)screen.X;
            float scaley = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / (float)screen.Y;
            float finalscale = 1;
            if (!fullscreen)
            {
                if (scalex < 1f || scaley < 1f)
                    finalscale = Math.Min(scalex, scaley);
                graphics.IsFullScreen = false;
            }
            else
            {
                finalscale = scalex;
                if (Math.Abs(1 - scaley) < Math.Abs(1 - scalex))
                    finalscale = scaley;
                graphics.IsFullScreen = fullscreen;
            }
            graphics.PreferredBackBufferWidth = (int)(finalscale * screen.X);
            graphics.PreferredBackBufferHeight = (int)(finalscale * screen.Y);
            graphics.ApplyChanges();
        }

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
            graphics.PreferredBackBufferWidth = 560;
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
            //Amount of blocks had
            BlocksSet = 0;
            //initialize score
            Score = 0;
            //start timer
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(Everysecond);
            aTimer.Interval = TimeInterval;
            aTimer.Enabled = true;
            //0.2 second timer
            TwoTenthSecond = 0.2f;
            //Initialize and fill grid
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
            //Load content
            Font = Content.Load<SpriteFont>("Score");
            LegoBlue = Content.Load<Texture2D>("legoblue");
            LegoBaby = Content.Load<Texture2D>("legobaby");
            LegoPurple = Content.Load<Texture2D>("legopurple");
            SideMenu = Content.Load<Texture2D>("SideMenu");
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
            //Score
            DrawScore = "Score: " + Score.ToString();
            //Level
            Level = BlocksSet / 10 + 1;
            DrawLevel = "Level: " + Level.ToString();
            //Speed up the game
            TimeInterval = 1000 - ((Level - 1) * 10);
            // TODO: Add your update logic here
            TwoTenthSecond -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Handle the keyboard input
            HandleInput();
            if (currentKeyboardState.IsKeyDown(Keys.F5))
            {
                SetFullScreen(!graphics.IsFullScreen);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            //Spawn m_ActiveBlock
            if (IsBlockActive == false)
            {
                m_ActiveBlock = new OShape(new Vector2(180, 0));
                int TargetX = m_ActiveBlock.GetWidth();
                scale = new Vector2(TargetX / (float)LegoBlue.Width, TargetX / (float)LegoBlue.Width);
                ActiveColor = Content.Load<Texture2D>(m_ActiveBlock.GetColor());
                IsBlockActive = true;
                IsLocked = false;
            }
            //Rotate block
            if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
            {
                try
                {
                    int i;
                    int OpenBlocks = 0;
                    for (i = 0; i < 4; i++)
                    {
                        if (Grid[(int)m_ActiveBlock.GetRRotatedGridPos(i).X, (int)m_ActiveBlock.GetRRotatedGridPos(i).Y] == string.Empty)
                        {
                            OpenBlocks++;
                        }
                    }
                    if (OpenBlocks == 4)
                    {
                        m_ActiveBlock.AddRotation();
                    }
                }
                catch (IndexOutOfRangeException) { };
            }
            if (currentKeyboardState.IsKeyDown(Keys.D) && previousKeyboardState.IsKeyUp(Keys.D))
            {
                try
                {
                    int i;
                    int OpenBlocks = 0;
                    for (i = 0; i < 4; i++)
                    {
                        if (Grid[(int)m_ActiveBlock.GetLRotatedGridPos(i).X, (int)m_ActiveBlock.GetLRotatedGridPos(i).Y] == string.Empty)
                        {
                            OpenBlocks++;
                        }
                    }
                    if (OpenBlocks == 4)
                    {
                        m_ActiveBlock.SubRotation();
                    }
                }
                catch (IndexOutOfRangeException) { };
            }
            //Move block horizontally
            //left
            if ((currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left) || (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyDown(Keys.Left) && TwoTenthSecond < 0)) && m_ActiveBlock.GetMinPosX() > 0)
            {
                TwoTenthSecond = 0.2f;
                if (Grid[m_ActiveBlock.GetGridPosX() - 1, m_ActiveBlock.GetGridPosY()] == string.Empty)
                {
                    m_ActiveBlock.MoveHorizontal(-m_ActiveBlock.GetWidth());
                }
            }
            //right
            if ((currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right) || (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyDown(Keys.Right) && TwoTenthSecond < 0)) && m_ActiveBlock.GetMaxPosX() < 360)
            {
                TwoTenthSecond = 0.2f;
                if (Grid[m_ActiveBlock.GetGridPosX() + 1, m_ActiveBlock.GetGridPosY()] == string.Empty)
                {
                    m_ActiveBlock.MoveHorizontal(m_ActiveBlock.GetWidth());
                }
            }
            //Move block down
            if ((currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down) || (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyDown(Keys.Down) && TwoTenthSecond < 0)) && m_ActiveBlock.GetPosY() != 475 && IsLocked == false)
            {
                TwoTenthSecond = 0.2f;
                if (Grid[m_ActiveBlock.GetGridPosX(), (m_ActiveBlock.GetGridPosY() + 1)] == String.Empty)
                {
                    m_ActiveBlock.Fall();
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                m_ActiveBlock.Fall();
            }
            //Make sure the block stays in screen horizontally
            if (m_ActiveBlock.GetMaxPosX() > graphics.GraphicsDevice.Viewport.Width)
            {
                m_ActiveBlock.GBISX();
            }
            else if (m_ActiveBlock.GetMinPosX() < 0)
            {
                m_ActiveBlock.SetPosX((int)-m_ActiveBlock.GetMinPosX());
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
            if (m_ActiveBlock.GetMaxPosY() < 500)
            {
                try
                {
                    if (Grid[m_ActiveBlock.GetGridPosX(), (m_ActiveBlock.GetGridPosY() + 1)] != String.Empty || Grid[m_ActiveBlock.GetNextGridPosX(1, m_ActiveBlock.GetRotation()), (m_ActiveBlock.GetNextGridPosY(1, m_ActiveBlock.GetRotation()) + 1)] != String.Empty || Grid[m_ActiveBlock.GetNextGridPosX(2, m_ActiveBlock.GetRotation()), (m_ActiveBlock.GetNextGridPosY(2, m_ActiveBlock.GetRotation()) + 1)] != String.Empty || Grid[m_ActiveBlock.GetNextGridPosX(3, m_ActiveBlock.GetRotation()), (m_ActiveBlock.GetNextGridPosY(3, m_ActiveBlock.GetRotation()) + 1)] != String.Empty)
                    {
                        IsLocked = true;
                        BlocksSet++;
                    }
                }
                catch (IndexOutOfRangeException) { };
            }
            else if (m_ActiveBlock.GetMaxPosY() == 500)
            {
                IsLocked = true;
                BlocksSet++;
            }

            if (IsLocked == true)
            {
                //Set grid value to the color of activeblock           
                try
                {
                    int i = 0;
                    for (i = 0; i < 4; i++)
                    {
                        Grid[m_ActiveBlock.GetNextGridPosX(i, m_ActiveBlock.GetRotation()), m_ActiveBlock.GetNextGridPosY(i, m_ActiveBlock.GetRotation())] = m_ActiveBlock.GetColor();
                    }
                    IsBlockActive = false;
                    int InARow = 0;
                    int FullRow = 12;
                    for (y = 0; y < 20; y++)
                    {
                        int x = 0;
                        InARow = 0;
                        for (x = 0; x < 12; x++)
                        {
                            if (Grid[x, y] != string.Empty)
                            {
                                InARow++;
                                if (InARow == 12)
                                {
                                    FullRow = y;
                                    Score += 100;
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


                    //Empty full rows and move rows down
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
                catch (IndexOutOfRangeException) { }
            }

            base.Update(gameTime);
        }

        private void Everysecond(object source, ElapsedEventArgs e)
        {
            try
            {
                if (m_ActiveBlock.GetMaxPosY() != graphics.GraphicsDevice.Viewport.Height && m_ActiveBlock.GetMaxPosY() < graphics.GraphicsDevice.Viewport.Height && currentKeyboardState.IsKeyUp(Keys.Down))
                {
                    m_ActiveBlock.Fall();
                }
            }
            catch (IndexOutOfRangeException) { };
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(/*SpriteSortMode.Deferred, null, null, null, null, null, spriteScale*/);
            spriteBatch.Draw(SideMenu, new Vector2(360, 0));
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
            m_ActiveBlock.Draw(spriteBatch, scale, ActiveColor);
            spriteBatch.DrawString(Font, DrawScore, new Vector2(410, 100), Color.White);
            spriteBatch.DrawString(Font, DrawLevel, new Vector2(410, 120), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}