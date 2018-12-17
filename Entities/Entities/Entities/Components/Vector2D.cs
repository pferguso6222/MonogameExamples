using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;

namespace EntityComponents
{
    public class Vector2D
    {

        float X;
        float Y;
        float Length;

        public Vector2D()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Console.WriteLine("Vector2D update");
        }

    }
}
  
