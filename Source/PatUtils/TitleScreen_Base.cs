using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using Source.PatUtils;

namespace Source.PatUtils
{
    public class TitleScreen_Base : Screen
    {
        private Texture2D _background;
        private BitmapFont font_normal;
        private BitmapFont font_highlighted;
        private BitmapFont font_pressed;
        private BitmapFont font_copyright;
        private SlicedSprite slicedSprite;

        KeyboardState previousState;
        GamePadState previousGamepadState;
        ButtonMenu menu;

        public TitleScreen_Base(Texture2D backgroundImage,
                                SlicedSprite slicedSprite,
                                BitmapFont menuFontNormal,
                               BitmapFont menuFontHighlighted,
                               BitmapFont menuFontPressed,
                               BitmapFont fontCopyright)
        {
            _background = backgroundImage;
            this.slicedSprite = slicedSprite;
            font_normal = menuFontNormal;
            font_highlighted = menuFontHighlighted;
            font_pressed = menuFontPressed;
            font_copyright = fontCopyright;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            previousState = Keyboard.GetState();
            previousGamepadState = GamePad.GetState(PlayerIndex.One);

            /*
            //THE FOLLOWING IS THE LAYOUT FOR NAME ENTRY BUTTONS. SAVE THIS FOR NAME ENTRY SCREEN
            int rows = 5;
            int cols = 6;

            menu = new ButtonMenu(120, 120, 6, 5, new Vector2(400, 200), GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonClick_1"), GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonSelected_1"));

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,.!?";

            int row = 0;
            int col = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                BitmapFontButton charButton = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, c.ToString(), new Vector2(0, 0), new Vector2(0, 0), 4.0f);
                charButton.OnPress = notifyButtonPressed;
                menu.addButtonAt(charButton, col, row);
                col++;
                if (col >= cols)
                {
                    col = 0;
                    row++;
                    if (row >= rows)
                    {
                        row = rows - 1;
                    }
                }
            }

            menu.setActiveButton(2, 2);

            */
            Point position = GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.5f, 0.5f));
            Point spacing = GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.0f, .1f));

            menu = new ButtonMenu(position:position, 
                                  cols:1, 
                                  rows:3, 
                                  xSpacing:spacing.X, 
                                  ySpacing:spacing.Y, 
                                  nextButtonSound:GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonClick_1"), 
                                  pressButtonSound:GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonSelected_1"), 
                                  buttonAlignment:Button.ButtonAlignment.CENTER);

            //START GAME BUTTON
            BitmapFontButton bStartGame = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, "START GAME", new Point(0, 0), Button.ButtonAlignment.CENTER);
            //bStartGame.OnPress = notifyButtonPressed;
            menu.addButtonAt(bStartGame, 0, 0);

            //OPTIONS BUTTON
            BitmapFontButton bOptions = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, "OPTIONS", new Point(0, 0), Button.ButtonAlignment.CENTER);
            bOptions.OnPress = LoadOptions;
            menu.addButtonAt(bOptions, 0, 1);

            //QUIT GAME
            BitmapFontButton bQuit = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, "EXIT GAME", new Point(0, 0), Button.ButtonAlignment.CENTER);
            bQuit.OnPress = QuitVerify;
            menu.addButtonAt(bQuit, 0, 2);

            menu.setActiveButton(0, 0);

        }

        private void LoadOptions(){
            GameBase.Instance.ChangeGameState(GameBase.GameState.OPTIONS_MAIN);
        }

        private void QuitVerify(){
            menu.Enabled = false;
            PopupSelectionDialog popup = new PopupSelectionDialog(GameBase.Instance,
                                             slicedSprite,
                                                                  GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.5f, 0.25f)),
                                                                  GameBase.Instance.ScreenPointFromScreenVector(new Vector2(.65f, .3f)),
                                             GameBase.Instance.GetCurrentPixelScale(),
                                             SlicedSprite.alignment.ALIGNMENT_MID_CENTER,
                                             "ARE YOU SURE YOU WANT TO QUIT?",
                                             "YES",
                                             "NO",
                                             font_normal,
                                             font_pressed,
                                             font_pressed)
            {
                notifyPressedButtonA = QuitGame,
                notifyPressedButtonB = CancelQuit
            };
        }

        private void CancelQuit(){
            menu.Enabled = true;
        }

        private void QuitGame(){
            GameBase.Instance.Exit();
        }

        public override void Update(GameTime gameTime)
        {
           // base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
                GameBase.Instance.Exit();

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
            if (state.IsKeyDown(Keys.Space) & !previousState.IsKeyDown(
                Keys.Space))
                menu.PressCurrentButton();
            if (state.IsKeyDown(Keys.Enter) & !previousState.IsKeyDown(
                Keys.Enter))
                menu.PressCurrentButton();

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

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            StaticSpriteBatch.Instance.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, GameBase.Instance.SamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1.0f));
            StaticSpriteBatch.Instance.Draw(_background, new Rectangle(new Point(0, 0), new Point(GameBase.Instance.ScreenWidth, GameBase.Instance.ScreenHeight)), Color.White);
            menu.Draw(gameTime);
            StaticSpriteBatch.Instance.DrawString(font_copyright, "Copyright 2018", new Vector2((GameBase.Instance.ScreenWidth * .5f) - (font_copyright.GetStringRectangle("Copyright 2018").Width * .38f), (float)(GameBase.Instance.ScreenHeight * .95)), Color.White, 0.0f, new Vector2(50, 1), 1.0f, SpriteEffects.None, 0.0f);
            StaticSpriteBatch.Instance.End();
        }
    }
}
