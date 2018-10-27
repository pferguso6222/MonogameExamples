using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.PatUtils;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Content;


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

            GameConfigUtility gameConfigUtil = new GameConfigUtility("PatsGame");
            gameConfigUtil.LoadVars();
            GameConfig = gameConfigUtil.data;

            //Parse Game Config Screen Resolution
            graphics.PreferredBackBufferWidth = GameConfig.screenWidth;
            graphics.PreferredBackBufferHeight = GameConfig.screenHeight;

            //Parse Game Config Sampler State Index
            SamplerStateIndex = GameConfig.SamplerStateIndex;
            UpdateSamplerState();

            //Parse Game Config Fullscreen
            if (GameConfig.isFullScreen)
            {
                if (!graphics.IsFullScreen)
                {
                    graphics.ToggleFullScreen();
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //do this FIRST
            base.LoadContent();

            titleScreen = new TitleScreen_Base("Graphics/Bkg_Title", ".\\YosterIsland_12px_2", ".\\YosterIsland_12px_1", ".\\YosterIsland_12px", ".\\Makaimura", 2.0f);
            optionsScreen = new Options_Base("Graphics/Bkg_Title", ".\\YosterIsland_12px_2", ".\\YosterIsland_12px_1", ".\\YosterIsland_12px", 4.0f);

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
