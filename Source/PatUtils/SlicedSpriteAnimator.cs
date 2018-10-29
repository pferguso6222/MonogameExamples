using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tweening;
using MonoGame.Extended;


namespace Source.PatUtils
{

    class SlicedSpriteAnimator : DrawableGameComponent
    {
        private Tween tween;
        private Tweener _tweener = new Tweener();
        public SlicedSprite _sprite;

        public Rectangle _tweenRect = new Rectangle();
        public Vector2 myPoint = new Vector2(0, 0);

        public static Action <Tween> notifyAnimationComplete;

        public SlicedSpriteAnimator(Game game) : base(game)
        {
        }

        public void AnimateSlicedSprite(SlicedSprite sprite, Rectangle startRect, Rectangle endRect, float duration, float delay, Action <Tween> OnCompleteFunc)
        {
            _sprite = sprite;
            notifyAnimationComplete = OnCompleteFunc;
            _tweenRect = startRect;
            myPoint.X = _tweenRect.Width;
            myPoint.Y = _tweenRect.Height;

            tween = _tweener.TweenTo(this, a => a.myPoint, new Vector2(endRect.Width, endRect.Height), duration: duration, delay: delay)
                .RepeatForever(repeatDelay: delay) //optional
                .AutoReverse() //optional
                .Easing(EasingFunctions.SineOut) //optional
                .OnEnd(notifyAnimationComplete); //optional

            Game.Components.Add(this);
            this.Visible = true;
        }

        public void DismissSprite()
        {
            Game.Components.Remove(this);
        }

        public override void Update(GameTime gameTime)
        {
            _tweener.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _tweenRect.Width = (int)myPoint.X;
            _tweenRect.Height = (int)myPoint.Y;
            _sprite.SetRectangle(_tweenRect);
        }

        public override void Draw(GameTime gameTime)
        {
            _sprite.Draw();
            base.Draw(gameTime);
        }
    }
}
