using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.PatUtils;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;

namespace GameTemplate.Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : GameBase
    {
        protected TitleScreen_Base TitleScreen;
        protected Options_Base OptionsScreen;
        public GameConfigUtility GameConfig;
        public SaveGameUtility SaveConfig;
        public SaveGameSelect_Base SaveGameSelectScreen;
        public NameEntry_Base NameEntryScreen;
        public StaticSpriteBatch spriteBatch;
        public ScreenManager screenManager;
        public SlicedSprite slicedSprite;

        RenderTarget2D RenderTarget;
        Effect scanlines;

        public Game1(int VirtualWidth, int VirtualHeight) : base(VirtualWidth, VirtualHeight){
            this.VirtualWidth = VirtualWidth;
            this.VirtualHeight = VirtualHeight;
        }

        protected override void Initialize()
        {
            RenderTarget = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight);

            GameName = "PatsGame";

            spriteBatch = new StaticSpriteBatch(GraphicsDevice);
            screenManager = new ScreenManager();

            GameConfig = new GameConfigUtility(GameName);
            GameConfig.LoadVars();

            SaveConfig = new SaveGameUtility(GameName);
            SaveConfig.LoadVars();

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

            base.Initialize();

        }

        public override float GetCurrentPixelScale()
        {
            return 1.0f;
            //return (float)Math.Ceiling(ScreenWidth / 480.0f);//We want 4X scaling on a 1920 x 1080 display
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            slicedSprite = new SlicedSprite(Content.Load<Texture2D>("Graphics/TiledDialogBkg_01"), new Rectangle(new Point(8, 8), new Point(48, 48)), GraphicsDevice, GetCurrentPixelScale(), SlicedSprite.CenterType.TILED, SlicedSprite.alignment.ALIGNMENT_TOP_CENTER);
            Texture2D menu_background = Content.Load<Texture2D>("Graphics/Bkg_Title");
            BitmapFont menu_font_normal = Content.Load<BitmapFont>(".\\YosterIsland_12px_2");
            BitmapFont menu_font_highlighted = Content.Load<BitmapFont>(".\\YosterIsland_12px_1");
            BitmapFont menu_font_pressed = Content.Load<BitmapFont>(".\\YosterIsland_12px");
            BitmapFont menu_font_copyright = menu_font_normal;

            TitleScreen = new TitleScreen_Base(menu_background, slicedSprite, menu_font_normal, menu_font_highlighted, menu_font_pressed, menu_font_copyright);
            OptionsScreen = new Options_Base(menu_background, menu_font_normal, menu_font_highlighted, menu_font_pressed);
            SaveGameSelectScreen = new SaveGameSelect_Base(menu_background, slicedSprite, menu_font_normal, menu_font_highlighted, menu_font_pressed, menu_font_copyright);
            NameEntryScreen = new NameEntry_Base(menu_background, slicedSprite, menu_font_normal, menu_font_highlighted, menu_font_pressed);
            ChangeGameState(GameState.TITLE_MAIN);//Make this the program start

            scanlines = Content.Load<Effect>("Shaders/Scanlines");
            scanlines.CurrentTechnique = scanlines.Techniques["BasicColorDrawing"];
            Console.WriteLine("SHADER:" + scanlines.ToString());

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
                    screenManager.LoadScreen(TitleScreen, new FadeTransition(GraphicsDevice, Color.Black, 1.0f));
                    break;
                case GameState.PLAYER_SELECTION_MAIN:
                    screenManager.LoadScreen(SaveGameSelectScreen, new FadeTransition(GraphicsDevice, Color.Black, 1.0f));
                    break;
                case GameState.PLAYER_ENTRY_MAIN:
                    screenManager.LoadScreen(NameEntryScreen, new FadeTransition(GraphicsDevice, Color.Black, 1.0f));
                    break;
                case GameState.OPTIONS_MAIN:
                    screenManager.LoadScreen(OptionsScreen, new FadeTransition(GraphicsDevice, Color.Black, 1.0f));
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
              //  Exit();

            screenManager.Update(gameTime);
            base.Update(gameTime);

            //GraphicsDevice.GetRenderTargets()[0].GetData(0, ScreenRect, ScreenPixelData, 0, ScreenPixelData.Length);
            //RenderTarget.SetData(ScreenPixelData);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            screenManager.Draw(gameTime);
            base.Draw(gameTime);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, GameBase.Instance.SamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1.0f));
            //scanlines.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(RenderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), ScreenRect, Color.White);
            spriteBatch.End();
        }
    }
}
