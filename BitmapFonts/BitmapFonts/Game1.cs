using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BitmapFonts
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BitmapFontDialogBox dialogBox;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dialogBox = new BitmapFontDialogBox(GraphicsDevice, Content, spriteBatch, "YosterIsland_12px_2", new Rectangle(new Point(200, 200), new Point(250, 100)), "ONCE UPON A TIME THERE WAS A CHARACTER DIALOG BOX.\b THE DIALOG BOX WAS DESIGNED TO AUTO WRAP TEXT WITHIN A DEFINED RECTANGLE.", 2.0f);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                dialogBox.paused = false;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                    dialogBox.paused = false;
            }

            dialogBox.Update(gameTime);

            base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            dialogBox.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
