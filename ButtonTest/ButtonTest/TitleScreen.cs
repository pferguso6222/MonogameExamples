using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.BitmapFonts;
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

        private BitmapFont textField1;

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
            textField1 = Content.Load<BitmapFont>(".\\YosterIsland_12px_2");
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

            spriteBatch.DrawString(textField1, "Copyright 2018", new Vector2(GraphicsDevice.Viewport.Width /2, (float)(GraphicsDevice.Viewport.Height * .95)), Color.White, 0.0f, new Vector2(50, 1), 2.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
