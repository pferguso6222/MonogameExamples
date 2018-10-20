using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tweening;


namespace Source.PatUtils
{

    class SlicedSpriteAnimated : SlicedSprite
    {

        Tween tween;
        Tweener _tweener;

        public Rectangle _tweenRect = new Rectangle();
        public Vector2 myPoint = new Vector2(0, 0);

        public Action <Tween> notifyAnimationComplete;

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

        
        public void animate(Rectangle startRect, Rectangle endRect, float duration, float delay, Action <Tween> OnCompleteFunc)
        {
            notifyAnimationComplete = OnCompleteFunc;
            _tweenRect = startRect;
            myPoint.X = _tweenRect.Width;
            myPoint.Y = _tweenRect.Height;


            tween = _tweener.TweenTo(this, a => a.myPoint, new Vector2(endRect.Width, endRect.Height), duration: duration, delay: delay)
                .RepeatForever(repeatDelay: delay) //optional
                .AutoReverse() //optional
                .Easing(EasingFunctions.SineOut) //optional
                .OnEnd(notifyAnimationComplete); //optional
        }

        public void Update(GameTime gameTime)
        {
            //if (!tween.IsComplete)
            //{
                var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _tweenRect.Width = (int)myPoint.X;
                _tweenRect.Height = (int)myPoint.Y;
                _tweener.Update(elapsedSeconds);
                SetRectangle(_tweenRect);
                Console.WriteLine("tweenRect.Width:" + elapsedSeconds + "\n");
           // }
            
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
        }
    }
}
