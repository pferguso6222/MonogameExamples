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
        GamePadState previousGamepadState;

        private BitmapFont textField1;

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
            previousGamepadState = GamePad.GetState(PlayerIndex.One);
            background = Content.Load<Texture2D>(".\\Bkg_Title");
            textField1 = Content.Load<BitmapFont>(".\\YosterIsland_12px_2");

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

            menu.setActiveButton(2, 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
                Game.Exit();

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

            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            GamePadCapabilities capabilities2 = GamePad.GetCapabilities(PlayerIndex.Two);
            GamePadCapabilities capabilities3 = GamePad.GetCapabilities(PlayerIndex.Three);
            GamePadCapabilities capabilities4 = GamePad.GetCapabilities(PlayerIndex.Four);

            if (capabilities.IsConnected)
            {
                // Get the current state of Controller1
                GamePadState _state = GamePad.GetState(PlayerIndex.One);
                GamePadState _state2 = GamePad.GetState(PlayerIndex.Two);
                GamePadState _state3 = GamePad.GetState(PlayerIndex.Three);
                GamePadState _state4 = GamePad.GetState(PlayerIndex.Four);

                if (_state.IsButtonDown(Buttons.DPadRight) && !previousGamepadState.IsButtonDown(Buttons.DPadRight))
                    menu.setActiveOffset(1, 0);
                if (_state.IsButtonDown(Buttons.DPadLeft) && !previousGamepadState.IsButtonDown(Buttons.DPadLeft))
                    menu.setActiveOffset(-1, 0);
                if (_state.IsButtonDown(Buttons.DPadUp) && !previousGamepadState.IsButtonDown(Buttons.DPadUp))
                    menu.setActiveOffset(0, -1);
                if (_state.IsButtonDown(Buttons.DPadDown) && !previousGamepadState.IsButtonDown(Buttons.DPadDown))
                    menu.setActiveOffset(0, 1);

                previousGamepadState = _state;
            }
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

            menu.Draw(gameTime);

            spriteBatch.DrawString(textField1, "Copyright 2018", new Vector2(GraphicsDevice.Viewport.Width /2, (float)(GraphicsDevice.Viewport.Height * .95)), Color.White, 0.0f, new Vector2(50, 1), 2.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
