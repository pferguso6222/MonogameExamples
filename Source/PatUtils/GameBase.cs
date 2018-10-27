using System;
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

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public ScreenManager screenManager;
        public GameConfigUtility GameConfig;
        protected GameState gameState;
        public int SamplerStateIndex = 0;
        public SamplerState SamplerState = SamplerState.PointClamp;

        public GameBase()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            //Window.Title = "My new title";
            //Window.AllowUserResizing = true;
            //Window.ClientSizeChanged += OnResize;
            Instance = this;
        }

        public virtual float GetCurrentPixelScale(){
            return 1.0f;
        }

        public virtual void ChangeGameState(GameState state){
            gameState = state;
        }

        public virtual void NotifyStateComplete(GameState state){

        }

        public int ScreenWidth()
        {
            if (graphics.IsFullScreen){
                return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            }else{
                return GraphicsDevice.Viewport.Bounds.Width;
            }
        }

        public int ScreenHeight()
        {
            if (graphics.IsFullScreen)
            {
                return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }else{
                return GraphicsDevice.Viewport.Bounds.Height;
            }
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

        public void UpdateSamplerState(){

            if (SamplerStateIndex == 0)
            {
                SamplerState = SamplerState.PointClamp;
            }
            else if (SamplerStateIndex == 1)
            {
                SamplerState = SamplerState.LinearClamp;
            }
            else
            {
                SamplerState = SamplerState.AnisotropicClamp;
            }
        }

        public string SamplerStateString()
        {
            string str = "";

            switch (SamplerStateIndex)
            {
                case (0):
                    str = "NO FILTERING";
                    break;
                case (1):
                    str = "LINEAR FILTERING";
                    break;
                case (2):
                    str = "ANSIOTROPIC FILTERING";
                    break;
            }
            return str;
        }

        public string ResolutionString(){
            return string.Concat(ScreenWidth().ToString(), "x", ScreenHeight().ToString());
        }

        public void OnResize(Object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth = ScreenWidth();
            graphics.PreferredBackBufferHeight = ScreenHeight();
            graphics.ApplyChanges();
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
