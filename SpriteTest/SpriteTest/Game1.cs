using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;


namespace SpriteTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D wizTexture;
        AnimatedSprite spr;
       // Sprite spr1;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            wizTexture = Content.Load<Texture2D>("Wizard_SpriteSheet");
            TextureAtlas wizTextureAtlas = Content.Load<TextureAtlas>("Wiz_Map");
            SpriteSheetAnimationFactory factory = new SpriteSheetAnimationFactory(wizTextureAtlas);

            factory.Add("run", new SpriteSheetAnimationData(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}, frameDuration:0.05f, isLooping:true));

            spr = new AnimatedSprite(factory, "run");

            spr.Play("run");
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

            base.Update(gameTime);

            spr.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            spriteBatch.Begin(samplerState:SamplerState.PointClamp, blendState:null);
            spr.Position = new Vector2(200, 200);
            spriteBatch.Draw((Sprite)spr);

            spriteBatch.End();
        }

        /*
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

            foreach (var entity in ActiveEntities)
            {
                var sprite = _animatedSpriteMapper.Has(entity)
                    ? _animatedSpriteMapper.Get(entity)
                    : _spriteMapper.Get(entity);
                var transform = _transforMapper.Get(entity);

                if (sprite is AnimatedSprite animatedSprite)
                    animatedSprite.Update(gameTime.GetElapsedSeconds());

                _spriteBatch.Draw(sprite, transform);

            }

            _spriteBatch.End();
        }
        */

    }
}
