using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tweening;


namespace PatUtils
{

    class SlicedSpriteAnimated : SlicedSprite
    {

        Tweener _tweener;

        public Vector2 Linear = new Vector2(200, 50);
        public Vector2 Quadratic = new Vector2(200, 100);
        public Vector2 Exponential = new Vector2(200, 150);
        public Vector2 Bounce = new Vector2(200, 200);
        public Vector2 Back = new Vector2(200, 250);
        public Vector2 Elastic = new Vector2(200, 300);
        public Vector2 Size = new Vector2(50, 50);

        public SlicedSpriteAnimated(
                Texture2D _sourceTexture,           //the sprite to be 9-Sliced
                Rectangle _sliceRect,               //the rectangle that defines the vertical and horizontal slices within the original texture bounds
                GraphicsDevice _graphicsDevice,     //the GraphicsDevice from which a spriteBatch can be made to construct the sliced sprite
                float _pixelScaleFactor,            //the pixel scaling of the final render
                CenterType centerType,              //STRETCHED or TILED center slice
                alignment _alignment
            ) : base(_sourceTexture, _sliceRect, _graphicsDevice, _pixelScaleFactor, centerType, _alignment)
        {
            _tweener = new Tweener();
        }

        
        public void animate(Rectangle startRect, Rectangle endRect, float duration, float delay)
        {
            _tweener.TweenTo(this, a => a.Linear, new Vector2(550, 50), duration: 2, delay: 1)
                .RepeatForever(repeatDelay: 0.2f)
                .AutoReverse()
                .Easing(EasingFunctions.Linear);
        }

        
        
    }
}
