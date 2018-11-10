using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public string GameName = "Game_Base";
        public GraphicsDeviceManager graphics;
        protected GameState gameState;
        public int SamplerStateIndex = 0;
        public SamplerState SamplerState = SamplerState.PointClamp;

        public int VirtualWidth;
        public int VirtualHeight;

        public Rectangle ScreenRect;
        public Color[] ScreenPixelData; 

        public GameBase(int VirtualWidth, int VirtualHeight) : base()
        {
            graphics = new GraphicsDeviceManager(this);
            this.VirtualWidth = VirtualWidth;
            this.VirtualHeight = VirtualHeight;
            ScreenRect = new Rectangle(0, 0, VirtualWidth, VirtualHeight);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        public int ScreenWidth
        {
            get
            {
                if (graphics.IsFullScreen)
                {
                    return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                }
                else
                {
                    return GraphicsDevice.Viewport.Bounds.Width;
                }
            }
            private set
            {
                ScreenWidth = value;
            }
        }

        public int ScreenHeight
        {
            get
            {
                if (graphics.IsFullScreen)
                {
                    return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                }
                else
                {
                    return GraphicsDevice.Viewport.Bounds.Height;
                }
            }
            private set
            {
                ScreenHeight = value;
            }
        }

        public Point ScreenPointFromScreenVector(Vector2 pos){
            return new Point((int)(VirtualWidth * pos.X), (int)(VirtualHeight * pos.Y));
        }

        public Vector2 ScreenVectorFromScreenPoint(Point pos)
        {
            return new Vector2((float)(pos.X / VirtualWidth), (float)(pos.Y / VirtualHeight));
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
            base.Draw(gameTime);

        }
    }
}
