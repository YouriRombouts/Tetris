﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Block m_Block;
        Texture2D LegoBlue, LegoBaby;
        Vector2 scale;

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
            graphics.PreferredBackBufferHeight = 600;
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

            bool[,] Grid;
            Grid = new bool[20,12];
            int p = 0;
            for(p = 0; p < Grid.GetLength(0); p++)
            {
                int o = 0;
                for (o = 0; o < Grid.GetLength(1); o++)
                {
                    Grid[p, o] = false;
                }
            }

            Vector2 [,] GridPos;
            GridPos = new Vector2[20, 12];
            int s = 0;
            for (s = 0; s < GridPos.GetLength(0); s++)
            {
                int a = 0;
                for (a = 0; a < GridPos.GetLength(1); a++)
                {
                    GridPos[s, a].X = (30 * a);
                    GridPos[s, a].Y = (30 * s);
                }
            }
            m_Block = new Block(new Vector2(0, 0));
            LegoBlue = Content.Load<Texture2D>("legoblue");
            LegoBaby = Content.Load<Texture2D>("legobaby");
            int TargetX = m_Block.GetWidth();
            scale = new Vector2(TargetX / (float)LegoBlue.Width, TargetX / (float)LegoBlue.Width);
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
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (m_Block.GetPosY() >= graphics.GraphicsDevice.Viewport.Height)
            {
                //notin
            }
            else
            {
                m_Block.Fall((int)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
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
            spriteBatch.Draw(LegoBaby, m_Block.GetPos() , scale: scale);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
