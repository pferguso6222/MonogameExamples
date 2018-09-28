using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using PatUtils;
using MonoGame.Extended.BitmapFonts;

namespace ScreenWarpTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BitmapFont _bitmapFont;

        RenderTarget2D renderTarget;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
        }


        protected override void Initialize()
        {

            base.Initialize();

            
        }

        public static Texture2D sand;

        Vector2 x_finder;
        List<Line> lines;

        protected override void LoadContent()
        {
            sand = Content.Load<Texture2D>(".\\Sand");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            lines = new List<Line>();

            //this vector is rotated around vector.zero, so we get an x value that waves up and down...
            // CHANGE this value to determine how far and fast the waves move
            x_finder = new Vector2(0, 25);

            //here we add a rectangle for each line of image we want wobbled.
            //the rectangle will be modified dynamically by the x_finder above.

            for (int i = 0; i < 64; i++)
            {
                lines.Add(new Line(GraphicsDevice, Content, new Rectangle(0, i, 690, 1)));
                x_finder = (RotateVector2(x_finder, 0.02f, Vector2.Zero));
                lines[i].my_destination = new Rectangle((int)x_finder.X, lines[i].my_destination.Y, lines[i].my_destination.Width, lines[i].my_destination.Height);
                lines[i].x_finder += x_finder;
                lines[i].my_from = (new Rectangle(0, i, 690, 1));
            }

            renderTarget = new RenderTarget2D(
                            GraphicsDevice,
                            GraphicsDevice.PresentationParameters.BackBufferWidth,
                            GraphicsDevice.PresentationParameters.BackBufferHeight,
                            false,
                            GraphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

        }

        //THIS method just orbits a vector around another vector, here we are using it to generate wave patterns.
        public static Vector2 RotateVector2(Vector2 point, float radians, Vector2 pivot)
        {
            float cosRadians = (float)Math.Cos(radians);
            float sinRadians = (float)Math.Sin(radians);

            Vector2 translatedPoint = new Vector2();
            translatedPoint.X = point.X - pivot.X;
            translatedPoint.Y = point.Y - pivot.Y;

            Vector2 rotatedPoint = new Vector2();
            rotatedPoint.X = translatedPoint.X * cosRadians - translatedPoint.Y * sinRadians + pivot.X;
            rotatedPoint.Y = translatedPoint.X * sinRadians + translatedPoint.Y * cosRadians + pivot.Y;

            return rotatedPoint;
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (Line line in lines)
            {
                line.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            

            //GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);


            //Render scrolling text
            Line line0 = lines[0];
            line0.DrawSceneToTexture(renderTarget, spriteBatch);

            //draw each line
            foreach (Line line in lines)
            {
                line.Draw(renderTarget, spriteBatch);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
