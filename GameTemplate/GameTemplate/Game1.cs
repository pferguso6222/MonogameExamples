using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.PatUtils;
using MonoGame.Extended.Content;


namespace GameTemplate.Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : GameBase
    {

        protected TitleScreen_Base titleScreen;


        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //do this FIRST
            base.LoadContent();

            //ScreenGameComponent screenGameComponent = new ScreenGameComponent(this);
            //Components.Add(screenGameComponent);

            titleScreen = new TitleScreen_Base(this, ".\\bkg", ".\\menuFontNormal", ".\\menuFontHighlighted", ".\\menuFontPressed", ".\\fontCopyright");
            ChangeGameState(GameState.TITLE_MAIN);//Make this the program start
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        protected override void ChangeGameState(GameState state)
        {
            base.ChangeGameState(state);

            switch (state){
                case GameState.WAIT:
                    return;
                case GameState.PROGRAM_INTRO:

                    break;
                case GameState.TITLE_MAIN:
                    //screenManager.LoadScreen(titleScreen);
                    screenManager.LoadScreen(titleScreen, fadeOut2Sec);
                    break;
                case GameState.PLAYER_SELECTION_MAIN:

                    break;
                case GameState.PLAYER_ENTRY_MAIN:

                    break;
                case GameState.OPTIONS_MAIN:

                    break;
            }
        }

        public override void NotifyStateComplete(GameBase.GameState state)
        {


        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
              //  Exit();

            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime);
        }
    }
}
