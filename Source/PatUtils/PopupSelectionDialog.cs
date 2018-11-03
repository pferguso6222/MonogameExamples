using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.PatUtils;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Tweening;

namespace Source.PatUtils
{
    class PopupSelectionDialog : DrawableGameComponent, IDisposable
    {
        public static Action notifyOpenComplete = null;
        public static Action notifyPressedButtonA = null;
        public static Action notifyPressedButtonB = null;
        public static Action notifyCloseComplete = null;
        new Game Game;

        private SlicedSprite _slicedSprite;
        private SlicedSpriteAnimator slicedSpriteAnimator;

        protected BitmapFont font_normal;
        protected BitmapFont font_highlighted;
        protected BitmapFont font_pressed;

        protected Rectangle _startRect;
        protected Rectangle _endRect;

        private string _dialogText;
        private string _buttonAText;
        private string _buttonBText;

        public float pixelScaleFactor;

        bool contentShowing = false;

        //ButtonMenu menu = new ButtonMenu();

        public PopupSelectionDialog
        (
            Game game,
            SlicedSprite slicedSprite,
            Vector2 location,
            Vector2 size,
            float pixelScaleFactor,
            SlicedSprite.alignment alignment,
            string dialogText,
            string buttonAText, 
            string buttonBText, 
            BitmapFont fontNormal,
            BitmapFont fontHighlighted,
            BitmapFont fontPressed
        ) : base(game)
        {
            Game = game;
            _slicedSprite = slicedSprite;
            _startRect = new Rectangle((int)(GameBase.Instance.ScreenWidth() * location.X), (int)(GameBase.Instance.ScreenHeight() * location.Y), 0, 0);
            _endRect = new Rectangle((int)(GameBase.Instance.ScreenWidth() * location.X), (int)(GameBase.Instance.ScreenHeight() * location.Y), (int)(GameBase.Instance.ScreenWidth() * size.X), (int)(GameBase.Instance.ScreenHeight() * size.Y));
            this.pixelScaleFactor = pixelScaleFactor;
            _dialogText = dialogText;
            _buttonAText = buttonAText;
            _buttonBText = buttonBText;
            font_normal = fontNormal;
            font_highlighted = fontHighlighted;
            font_pressed = fontPressed;
            slicedSpriteAnimator = new SlicedSpriteAnimator(Game);
            GameBase.Instance.Components.Add(this);
            this.DrawOrder = 1;
            Open();
        }

        public void Open(){
            slicedSpriteAnimator.AnimateSlicedSprite(_slicedSprite, _startRect, _endRect, .25f, 0f, openComplete);
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
            notifyOpenComplete = null;
            notifyPressedButtonA = null;
            notifyPressedButtonB = null;
            notifyCloseComplete = null;
            Game = null;
            slicedSpriteAnimator = null;
            font_normal = null;
            font_highlighted = null;
            font_pressed = null;
            _dialogText = null;
            _buttonAText = null;
            _buttonBText = null;
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
                StaticSpriteBatch.Instance.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1.0f));
                StaticSpriteBatch.Instance.DrawString(font_normal, _dialogText, new Vector2(_endRect.X, _endRect.Y), Color.White, 0.0f, new Vector2(50, 1), pixelScaleFactor, SpriteEffects.None, 0.0f);
                StaticSpriteBatch.Instance.End();
            }
        }
    }
}
