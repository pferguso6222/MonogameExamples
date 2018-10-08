﻿using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using ButtonTest.Desktop;

namespace ButtonTest
{
    public class TitleScreen : Screen
    {
        public Game1 Game { get; }
        public ContentManager Content => Game.Content;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameServiceContainer Services => Game.Services;
        private SpriteBatch spriteBatch;
        public ScreenManager screenManager;
        private Texture2D background;

        public TitleScreen(Game1 game)
        {
            Game = game;
            spriteBatch = game.spriteBatch;
            Content.RootDirectory = "Content";
        }

        public override void LoadContent()
        {
            base.LoadContent();
            background = Content.Load<Texture2D>(".\\Bkg_Title");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend, 
                              SamplerState.PointClamp, 
                              DepthStencilState.Default, 
                              RasterizerState.CullNone, 
                              null, 
                              Matrix.CreateScale(1.0f));

            spriteBatch.Draw(background, new Rectangle(new Point(0,0), new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
