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
            dialogBox = new BitmapFontDialogBox(GraphicsDevice, Content, spriteBatch, "ArcadeClassic", new Rectangle(new Point(200, 200), new Point(400, 200)), "ONCE UPON A TIME\nTHERE WAS AN OLD\nWIZARD WHO LIVED IN A\nTOWER.");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            dialogBox.Update(gameTime);

            base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.SteelBlue);
            dialogBox.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
