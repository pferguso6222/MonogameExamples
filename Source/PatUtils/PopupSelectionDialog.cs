using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.PatUtils;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Tweening;

namespace Source.PatUtils
{
    class PopupSelectionDialog : DrawableGameComponent
    {
        public static Action notifyOpenComplete;
        public static Action notifyPressedButtonA;
        public static Action notifyPressedButtonB;
        public static Action notifyCloseComplete;
        new Game Game;

        private SpriteBatch spriteBatch;

        private SlicedSprite _slicedSprite;
        private SlicedSpriteAnimator slicedSpriteAnimator;

        protected BitmapFont font_normal;
        protected BitmapFont font_highlighted;
        protected BitmapFont font_pressed;

        protected Rectangle _startRect;
        protected Rectangle _endRect;

        private string _dialogText;

        bool contentShowing = false;

        public PopupSelectionDialog
        (
            Game game,SlicedSprite slicedSprite,Rectangle startRect,Rectangle endRect,string dialogText,string buttonAText, string buttonBText, BitmapFont fontNormal,BitmapFont fontHighlighted,BitmapFont fontPressed
        ) : base(game)
        {
            Game = game;
            spriteBatch = new SpriteBatch(game.GraphicsDevice)
            _slicedSprite = slicedSprite;
            _startRect = startRect;
            _endRect = endRect;
            font_normal = fontNormal;
            font_highlighted = fontHighlighted;
            font_pressed = fontPressed;
            slicedSpriteAnimator = new SlicedSpriteAnimator(Game);
        }

        public void Open(){
            slicedSpriteAnimator.AnimateSlicedSprite(_slicedSprite, _startRect, _endRect, .25f, 1.0f, openComplete);
        }

        private void openComplete(Tween tween){
            contentShowing = true;
            notifyOpenComplete?.Invoke();
        }

        private void closeCompleteA(Tween tween){
            notifyPressedButtonA?.Invoke();
            notifyCloseComplete?.Invoke();
        }

        private void closeCompleteB(Tween tween)
        {
            notifyPressedButtonB?.Invoke();
            notifyCloseComplete?.Invoke();
        }

        private void buttonAPressed(){
            slicedSpriteAnimator.AnimateSlicedSprite(_slicedSprite, _endRect, _startRect, .25f, 1.0f, closeCompleteA);
        }

        private void buttonBPressed()
        {
            slicedSpriteAnimator.AnimateSlicedSprite(_slicedSprite, _endRect, _startRect, .25f, 1.0f, closeCompleteB);
        }

        public void Dismiss(){

        }

        public new void Dispose()
        {
            _slicedSprite = null;
            //_slicedSpriteAnimated = null;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public void DismissSprite()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (contentShowing){
                spriteBatch.Begin();
                GameBase.Instance.spriteBatch.DrawString(font_normal, _dialogText, new Vector2((GameBase.Instance.ScreenWidth() * .5f) - (font_copyright.GetStringRectangle("Copyright 2018").Width * .38f), (float)(GameBase.Instance.GraphicsDevice.Viewport.Height * .95)), Color.White, 0.0f, new Vector2(50, 1), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.End();
            }
        }
    }
}
