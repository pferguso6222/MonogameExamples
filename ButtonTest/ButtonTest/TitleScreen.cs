using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.BitmapFonts;
using Source.PatUtils;
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

        KeyboardState previousState;

        private BitmapFont textField1;

        //BitmapFontButton buttonStartGame;

        ButtonMenu menu;

        public TitleScreen(Game1 game)
        {
            Game = game;
            spriteBatch = game.spriteBatch;
            Content.RootDirectory = "Content";
        }

        public override void LoadContent()
        {
            base.LoadContent();
            previousState = Keyboard.GetState();
            background = Content.Load<Texture2D>(".\\Bkg_Title");
            textField1 = Content.Load<BitmapFont>(".\\YosterIsland_12px_2");
            //buttonStartGame = new BitmapFontButton(spriteBatch, Content.Load<BitmapFont>(".\\YosterIsland_12px_1"), Content.Load<BitmapFont>(".\\YosterIsland_12px_2"), "START GAME", new Vector2(300, 400), new Vector2(0, 0), 2.0f);
            //buttonStartGame.State = Button.BUTTON_STATE.HIGHLIGHTED;

            int rows = 5;
            int cols = 6;

            menu = new ButtonMenu(120, 120, 6, 5, new Vector2(400, 200));

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,.!?";

            int row = 0;
            int col = 0;

            for (int i = 0; i < chars.Length; i++){
                char c = chars[i];
                BitmapFontButton charButton = new BitmapFontButton(spriteBatch, Content.Load<BitmapFont>(".\\YosterIsland_12px_1"), Content.Load<BitmapFont>(".\\YosterIsland_12px_2"), c.ToString(), new Vector2(0, 0), new Vector2(0, 0), 4.0f);
                menu.addButtonAt(charButton, col, row);
                col++;
                if (col >= cols){
                    col = 0;
                    row++;
                    if (row >= rows){
                        row = rows - 1;
                    }
                }
            }

            //menu.addButtonAt(buttonStartGame, 2, 2);
            menu.setActiveButton(2, 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
           // if (state.IsKeyDown(Keys.Escape))
             //   Exit();

            // Move our sprite based on arrow keys being pressed:
            if (state.IsKeyDown(Keys.Right) & !previousState.IsKeyDown(
                Keys.Right))
                menu.setActiveOffset(1, 0);
            if (state.IsKeyDown(Keys.Left) & !previousState.IsKeyDown(
                Keys.Left))
                menu.setActiveOffset(-1, 0);
            if (state.IsKeyDown(Keys.Up) & !previousState.IsKeyDown(
                Keys.Up))
                menu.setActiveOffset(0, -1);
            if (state.IsKeyDown(Keys.Down) & !previousState.IsKeyDown(
                Keys.Down))
                menu.setActiveOffset(0, 1);

            previousState = state;
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

            //buttonStartGame.Draw(gameTime);

            menu.Draw(gameTime);

            spriteBatch.DrawString(textField1, "Copyright 2018", new Vector2(GraphicsDevice.Viewport.Width /2, (float)(GraphicsDevice.Viewport.Height * .95)), Color.White, 0.0f, new Vector2(50, 1), 2.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
