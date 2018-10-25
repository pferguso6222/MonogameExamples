using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FLCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlcPlayerTest.Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Color[] data;

        Rectangle displayRect;
        Texture2D tex;

        static FLCFile file;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void OnPlaybackStarted(FLCFile file)
        {
            Console.WriteLine("Playback started");
        }

        private void OnPlaybackFinished(FLCFile file, bool didFinishNormally)
        {
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            file = new FLCFile(File.OpenRead("MrTesty_Sample.flc"));
            file.OnFrameUpdated += OnFrameUpdated;
            file.OnPlaybackFinished += OnPlaybackFinished;
            file.OnPlaybackStarted += OnPlaybackStarted;
            file.ShouldLoop = true;

            data = new Color[128 * 128];

            tex = new Texture2D(GraphicsDevice, 128, 128);

            displayRect = new Rectangle();
            displayRect.Width = 500;
            displayRect.Height = 500;

            file.Open();
            file.Play();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        private void OnFrameUpdated(FLCFile _file)
        {
            FLCColor[] colors = _file.GetFramebufferCopy();

            for (int i = 0; i < colors.Length; i++){
                data[i].R = colors[i].R;
                data[i].G = colors[i].G;
                data[i].B = colors[i].B;
                data[i].A = colors[i].A;
            }

            tex.SetData(data);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(tex, displayRect, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
