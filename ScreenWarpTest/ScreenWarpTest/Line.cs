using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ScreenWarpTest;
using MonoGame.Extended.BitmapFonts;

namespace PatUtils
{
    public class Line
    {
        public Rectangle my_from;
        public Rectangle my_destination;
        public Vector2 x_finder;

        private BitmapFont _bitmapFont;

        public float xPos = 600.0f;
        public float xSpeed = 100.0f;

        

        GraphicsDevice graphicsDevice;

        public Line(GraphicsDevice _graphicsDevice, ContentManager content, Rectangle start)
        {
            my_destination = start;
            _bitmapFont = content.Load<BitmapFont>("Font1");
            graphicsDevice  = _graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            x_finder = Game1.RotateVector2(x_finder, 0.04f, Vector2.Zero);

            my_destination.X = (int)x_finder.X;

            xPos -= xSpeed * delta;

            if (xPos <= -600) xPos = 700;

        }

        public void DrawSceneToTexture(RenderTarget2D _renderTarget, SpriteBatch spriteBatch)
        {
            // Set the render target
            graphicsDevice.SetRenderTarget(_renderTarget);

            //graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            //graphicsDevice.Clear(Color.AliceBlue);
            spriteBatch.DrawString(_bitmapFont, "Pat was here! Hello and welcome to my demo.", new Vector2(xPos, 1), Color.White, 0);


            // Drop the render target
            graphicsDevice.SetRenderTarget(null);
        }

        public void Draw(RenderTarget2D _renderTarget, SpriteBatch _spriteBatch)
        {

            //graphicsDevice.SetRenderTarget(renderTarget);
            // graphicsDevice.Clear(Color.CornflowerBlue);
            //spriteBatch.DrawString(_bitmapFont, "Hello World", new Vector2(xPos, 200), Color.White);
            //graphicsDevice.SetRenderTarget(null);

            //spriteBatch.Begin();
            //DrawSceneToTexture(renderTarget, spriteBatch);
            _spriteBatch.Draw(_renderTarget, my_destination, my_from, Color.White * 1.0f, 0, Vector2.Zero, SpriteEffects.None, 0);
            //spriteBatch.End();

            //spriteBatch.DrawString(_bitmapFont, "HelloWorld", new Vector2(xPos, 200), Color.White, 0.0f,);
            //spriteBatch.Draw(Game1.sand, my_destination, my_from, Color.White * 1.0f, 0, Vector2.Zero, SpriteEffects.None, 0);
            //spriteBatch.DrawString(_bitmapFont, "Hello World", new Vector2(xPos, 200), Color.Red);
        }
    }
}
