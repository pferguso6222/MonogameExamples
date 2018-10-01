using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended;
using System.Diagnostics;

namespace TestApp1.Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TiledMap map;

        Camera2D camera;

        Vector2 parallaxFactor = new Vector2(0.5f, 0.5f);
        Vector2 fixedFactor = new Vector2(0.0f, 0.0f);

        TiledMapRenderer mapRenderer;

        float scrollXSpeed = 200.0f;
        float scrollYSpeed = 400.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferWidth = 1080;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<TiledMap>("Tilemaps/Tilemap_01");

            camera = new Camera2D(GraphicsDevice);

            mapRenderer = new TiledMapRenderer(GraphicsDevice);

            camera.Position = new Vector2(1, (map.TileHeight * map.Height) - GraphicsDevice.Viewport.Height);
            camera.Zoom = 2.0f;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);

            //Random camera movement
            camera.Move(new Vector2(scrollXSpeed * gameTime.GetElapsedSeconds(), scrollYSpeed * gameTime.GetElapsedSeconds()));

            //Limit camera position to map extents, reverse X or Y speed when edge of map reached
            if ((camera.Position.X >= (map.TileWidth * map.Width) - GraphicsDevice.Viewport.Bounds.Width) || (camera.Position.X <= 0)){
                scrollXSpeed *= -1;
            }
            if ((camera.Position.Y >= (map.TileHeight * map.Height) - GraphicsDevice.Viewport.Bounds.Height) || (camera.Position.Y <= 0))
            {
                scrollYSpeed *= -1;
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);//not needed if drawing a map, silly!

            spriteBatch.Begin(samplerState: SamplerState.LinearClamp);

            mapRenderer.Draw(map.GetLayer("Base"), camera.GetViewMatrix(fixedFactor));//draws fixed layer
            mapRenderer.Draw(map.GetLayer("Background"), camera.GetViewMatrix(parallaxFactor));//draws parallax layer
            mapRenderer.Draw(map.GetLayer("Foreground"), camera.GetViewMatrix());//draws foreground layer
            //mapRenderer.Draw(map, camera.GetViewMatrix());//OLD, draws all map layers

            //DRAW SPRITES HERE

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
