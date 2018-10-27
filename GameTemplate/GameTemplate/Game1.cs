using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.PatUtils;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.BitmapFonts;


namespace GameTemplate.Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : GameBase
    {

        protected TitleScreen_Base titleScreen;
        protected Options_Base optionsScreen;

        protected override void Initialize()
        {
            base.Initialize();

            GameConfig = new GameConfigUtility("PatsGame");
            GameConfig.LoadVars();

            //Parse Game Config Screen Resolution
            graphics.PreferredBackBufferWidth = GameConfig.data.screenWidth;
            graphics.PreferredBackBufferHeight = GameConfig.data.screenHeight;
            graphics.ApplyChanges();

            //Parse Game Config Sampler State Index
            SamplerStateIndex = GameConfig.data.SamplerStateIndex;
            UpdateSamplerState();

            //Parse Game Config Fullscreen
            if (GameConfig.data.isFullScreen)
            {
                if (!graphics.IsFullScreen)
                {
                    graphics.ToggleFullScreen();
                }
            }
        }

        public override float GetCurrentPixelScale()
        {
            return (float)Math.Ceiling(ScreenWidth() / 480.0f);//We want 4X scaling on a 1920 x 1080 display
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Texture2D menu_background = Content.Load<Texture2D>("Graphics/Bkg_Title");
            BitmapFont menu_font_normal = Content.Load<BitmapFont>(".\\YosterIsland_12px_2");
            BitmapFont menu_font_highlighted = Content.Load<BitmapFont>(".\\YosterIsland_12px_1");
            BitmapFont menu_font_pressed = Content.Load<BitmapFont>(".\\YosterIsland_12px");
            BitmapFont menu_font_copyright = Content.Load<BitmapFont>(".\\Makaimura");

            titleScreen = new TitleScreen_Base(menu_background, menu_font_normal, menu_font_highlighted, menu_font_pressed, menu_font_copyright);
            optionsScreen = new Options_Base(menu_background, menu_font_normal, menu_font_highlighted, menu_font_pressed);
            ChangeGameState(GameState.TITLE_MAIN);//Make this the program start
        }

        protected override void UnloadContent()
        {

        }

        public override void ChangeGameState(GameState state)
        {
            base.ChangeGameState(state);

            switch (state){
                case GameState.WAIT:
                    return;
                case GameState.PROGRAM_INTRO:

                    break;
                case GameState.TITLE_MAIN:
                    //screenManager.LoadScreen(titleScreen);
                    screenManager.LoadScreen(titleScreen, new FadeTransition(GraphicsDevice, Color.Black, 1.0f));//not working when switch to different screen
                    break;
                case GameState.PLAYER_SELECTION_MAIN:

                    break;
                case GameState.PLAYER_ENTRY_MAIN:

                    break;
                case GameState.OPTIONS_MAIN:
                    //screenManager.LoadScreen(optionsScreen);
                    screenManager.LoadScreen(optionsScreen, new FadeTransition(GraphicsDevice, Color.Black, 1.0f));//not working when switch to different screen
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
              //  Exit();

            screenManager.Update(gameTime);

            //base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime);
        }
    }
}
