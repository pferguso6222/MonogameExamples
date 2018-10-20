using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using PatUtils;
using MonoGame.Extended.Tweening;
using System;


namespace TiledSprite
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //ContentManager contentManager;

        SlicedSpriteAnimated myDialogBkg;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //contentManager = new ContentManager(
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myDialogBkg = new SlicedSpriteAnimated(Content.Load<Texture2D>(".\\TiledDialogBkg_01"), new Rectangle(new Point(8, 8), new Point(48, 48)), GraphicsDevice, 2.0f, SlicedSprite.CenterType.TILED, SlicedSprite.alignment.ALIGNMENT_TOP_CENTER);

            //Point center = new Point(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2);
            Point center = new Point(GraphicsDevice.Viewport.Bounds.Width / 2, 0);

            //myDialogBkg.SetRectangle(new Rectangle(new Point(0, 0), new Point(300, 150)));
            //myDialogBkg.SetRectangle(new Rectangle(center, new Point(300, 150)));
            // TODO: use this.Content to load your game content here

            myDialogBkg.animate(new Rectangle(center, new Point(16, 16)), 
                                new Rectangle(center, new Point((int)(GraphicsDevice.Viewport.Bounds.Width * .5f), 150)),
                                .25f, 
                                1.0f, 
                                TweenComplete);
        }

        public void TweenComplete(Tween tween)
        {
            Console.Write("Tween Complete\n");
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

            // TODO: Add your update logic here
            myDialogBkg.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);

            myDialogBkg.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
