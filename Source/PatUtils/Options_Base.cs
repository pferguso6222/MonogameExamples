using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;

namespace Source.PatUtils
{

    public class Options_Base : Screen
    {
        private Texture2D _background;
        private string _backgroundImage;
        private string _menuFontNormal;
        private string _menuFontHighlighted;
        private string _menuFontPressed;

        private BitmapFont tfTitle;
        private BitmapFont font_normal;
        private BitmapFont font_highlighted;
        private BitmapFont font_pressed;

        private float _pixelScale = 1.0f;

        KeyboardState previousState;
        GamePadState previousGamepadState;

        ButtonMenu menu;
        ButtonMenu resolutionSwitchMenu;
        ButtonMenu activeMenu = null;
        private int currentResolutionIndex = 0;
        private int lastKnownWidth = 0;
        private int lastKnownHeight = 0;
        private int desiredWidth = 0;
        private int desiredHeight = 0;
        private string resolutionString;

        public Options_Base(string backgroundImage, 
                                string menuFontNormal,
                               string menuFontHighlighted,
                               string menuFontPressed,
                               float pixelScale)
        {
            _backgroundImage = backgroundImage;
            _menuFontNormal = menuFontNormal;
            _menuFontPressed = menuFontPressed;
            _menuFontHighlighted = menuFontHighlighted;
            _pixelScale = pixelScale;
        }

        private void notifyButtonPressed(){
            Console.Write("Options_Base: Button Pressed!");
        }

        public override void LoadContent()
        {
            base.LoadContent();
            previousState = Keyboard.GetState();
            previousGamepadState = GamePad.GetState(PlayerIndex.One);
            _background = GameBase.Instance.Content.Load<Texture2D>(_backgroundImage);
            font_normal = GameBase.Instance.Content.Load<BitmapFont>(_menuFontNormal);
            tfTitle = GameBase.Instance.Content.Load<BitmapFont>(_menuFontNormal);
            font_highlighted = GameBase.Instance.Content.Load<BitmapFont>(_menuFontHighlighted);
            font_pressed = GameBase.Instance.Content.Load<BitmapFont>(_menuFontPressed);

            menu = new ButtonMenu(0, 100, 1, 4, new Vector2(GameBase.Instance.ScreenWidth() * .1f, GameBase.Instance.ScreenHeight() * .3f), GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonClick_1"), GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonSelected_1"), ButtonMenu.ButtonAlignment.LEFT);

            //Return to Main Menu
            BitmapFontButton bStartGame = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "RETURN", new Vector2(0, 0), new Vector2(0, 0), _pixelScale);
            bStartGame.OnPress = returnToMain;
            menu.addButtonAt(bStartGame, 0, 0);

            //Windowed / Fullscreen
            BitmapFontButton bOptions = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "DISPLAY MODE", new Vector2(0, 0), new Vector2(0, 0), _pixelScale);
            bOptions.OnPress = toggleFullscreen;
            menu.addButtonAt(bOptions, 0, 1);

            //Change Resolution
            BitmapFontButton bResolution = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "RESOLUTION", new Vector2(0, 0), new Vector2(0, 0), _pixelScale);
            bResolution.OnPress = changeResolution;
            menu.addButtonAt(bResolution, 0, 2);

            //Pixel Filtering
            BitmapFontButton bFiltering = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "PIXEL FILTERING", new Vector2(0, 0), new Vector2(0, 0), _pixelScale);
            bFiltering.OnPress = toggleFiltering;
            menu.addButtonAt(bFiltering, 0, 3);



            menu.setActiveButton(0, 0);

        }

        private void changeResolution(){
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Console.WriteLine(mode.Width + " x " + mode.Height +", Aspect Ratio:" + mode.AspectRatio);
            }
        }

        private void toggleFullscreen(){
            GameBase.Instance.graphics.ToggleFullScreen();
        }

        private void onDisplayPressed(){
            toggleFullscreen();
            Console.WriteLine("Resolutions Available:\n");
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Console.WriteLine(mode.Width + "X" + mode.Height +"\n");
            }
        }

        private void toggleFiltering(){

            if (GameBase.Instance.SamplerStateIndex == 0){
                GameBase.Instance.SamplerStateIndex = 1;
            }
            else if (GameBase.Instance.SamplerStateIndex == 1){
                GameBase.Instance.SamplerStateIndex = 2;
            }
            else{
                GameBase.Instance.SamplerStateIndex = 0;
            }
            GameBase.Instance.UpdateSamplerState();
        }

        private void returnToMain(){
            GameBase.Instance.ChangeGameState(GameBase.GameState.TITLE_MAIN);
        }

        private void LoadOptions(){
            GameBase.Instance.ChangeGameState(GameBase.GameState.OPTIONS_MAIN);
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
            //GraphicsDevice.Clear(Color.Red);

            GameBase.Instance.spriteBatch.Begin(SpriteSortMode.Deferred,
                                                BlendState.AlphaBlend,
                                                GameBase.Instance.SamplerState,
                                                DepthStencilState.Default,
                                                RasterizerState.CullNone,
                                                null,
                                                Matrix.CreateScale(1.0f));

            GameBase.Instance.spriteBatch.Draw(_background, new Rectangle(new Point(0, 0), new Point(GameBase.Instance.GraphicsDevice.Viewport.Width, GameBase.Instance.GraphicsDevice.Viewport.Height)), Color.White);
            //_spriteBatch.DrawString(_textHighlighted, _buttonText, _position, Color.White, 0.0f, _origin, _pixelScale, SpriteEffects.None, 0.0f);
            GameBase.Instance.spriteBatch.DrawString(tfTitle, "GAME OPTIONS", new Vector2(GameBase.Instance.ScreenWidth() * .5f, GameBase.Instance.ScreenHeight() * .1f), Color.White, 0.0f, new Vector2(tfTitle.GetStringRectangle("GAME OPTIONS").Width / 2,.5f), _pixelScale, SpriteEffects.None, 0.0f);
            GameBase.Instance.spriteBatch.DrawString(tfTitle, GameBase.Instance.graphics.IsFullScreen? "FULLSCREEN" : "WINDOWED", new Vector2(GameBase.Instance.ScreenWidth() * .5f, menu.getButtonAt(0, 1)._position.Y), Color.White, 0.0f, new Vector2(tfTitle.GetStringRectangle(GameBase.Instance.graphics.IsFullScreen ? "FULLSCREEN" : "WINDOWED").Width / 2, .5f), _pixelScale, SpriteEffects.None, 0.0f);
            GameBase.Instance.spriteBatch.DrawString(tfTitle, "1024 x 768", new Vector2(GameBase.Instance.ScreenWidth() * .5f, menu.getButtonAt(0, 2)._position.Y), Color.White, 0.0f, new Vector2(tfTitle.GetStringRectangle("1024 x 768").Width / 2, .5f), _pixelScale, SpriteEffects.None, 0.0f);
            GameBase.Instance.spriteBatch.DrawString(tfTitle, GameBase.Instance.SamplerStateString(), new Vector2(GameBase.Instance.ScreenWidth() * .5f, menu.getButtonAt(0, 3)._position.Y), Color.White, 0.0f, new Vector2(tfTitle.GetStringRectangle(GameBase.Instance.SamplerStateString()).Width / 2, .5f), _pixelScale, SpriteEffects.None, 0.0f);

            menu.Draw(gameTime);

            GameBase.Instance.spriteBatch.End();
        }
    }
}
