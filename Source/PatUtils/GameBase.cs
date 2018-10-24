using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Microsoft.Xna.Framework.Audio;

namespace Source.PatUtils
{
    public class GameBase : Game
    {

        public static GameBase Instance;

        public enum GameState
        {
            WAIT,
            PROGRAM_INTRO,
            TITLE_MAIN,
            PLAYER_SELECTION_MAIN,
            PLAYER_ENTRY_MAIN,
            OPTIONS_MAIN,
        }

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public ScreenManager screenManager;
        protected GameState gameState;

        public GameBase()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            Instance = this;
        }

        public virtual void ChangeGameState(GameState state){
            gameState = state;
        }

        public virtual void NotifyStateComplete(GameState state){

        }

        public int ScreenWidth(){
            return GraphicsDevice.Viewport.Bounds.Width;
        }

        public int ScreenHeight()
        {
            return GraphicsDevice.Viewport.Bounds.Height;
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenManager = new ScreenManager();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
