using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using Source.PatUtils;
using MonoGame.Extended.Tweening;
using System;


namespace TiledSprite
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : GameBase
    {
        StaticSpriteBatch spriteBatch;
        //ContentManager contentManager;

        SlicedSprite slicedSprite;

        PopupSelectionDialog popup;

        BitmapFont font_normal;
        BitmapFont font_highlighted;
        BitmapFont font_pressed;

        public Game1()
        {
            Content.RootDirectory = "Content";
            //contentManager = new ContentManager(
        }

        public override float GetCurrentPixelScale()
        {
            return 4.0f;
            //return (float)Math.Ceiling(ScreenWidth / 480.0f);//We want 4X scaling on a 1920 x 1080 display
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new StaticSpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            font_normal = Content.Load<BitmapFont>("YosterIsland_12px_2");
            font_highlighted = Content.Load<BitmapFont>("YosterIsland_12px_1");
            font_pressed = Content.Load<BitmapFont>("YosterIsland_12px");

            slicedSprite = new SlicedSprite(Content.Load<Texture2D>(".\\TiledDialogBkg_01"), new Rectangle(new Point(8, 8), new Point(48, 48)), GraphicsDevice, GetCurrentPixelScale(), SlicedSprite.CenterType.TILED, SlicedSprite.alignment.ALIGNMENT_MID_CENTER);
            popup = new PopupSelectionDialog(this,
                                             slicedSprite,
                                             new Vector2(0.5f, 0.5f),
                                             new Vector2(.5f, .2f),
                                             GetCurrentPixelScale(),
                                             SlicedSprite.alignment.ALIGNMENT_TOP_CENTER,
                                             "THIS IS THE LABEL THAT PAYS ME",
                                             "BUTTON A",
                                             "BUTTON B",
                                             font_normal,
                                             font_highlighted,
                                             font_pressed)
            {
                notifyPressedButtonA = PressedA,
                notifyPressedButtonB = PressedB
            };
        }

        private void PressedA(){
            Console.WriteLine("PRESSED BUTTON A");
        }
        private void PressedB()
        {
            Console.WriteLine("PRESSED BUTTON B");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

           //animator.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
