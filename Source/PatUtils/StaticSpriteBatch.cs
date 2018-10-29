using System;
using Microsoft.Xna.Framework.Graphics;

namespace Source.PatUtils
{
    public class StaticSpriteBatch : SpriteBatch
    {
        public static StaticSpriteBatch Instance;

        public StaticSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            Instance = this;
        }
    }
}
