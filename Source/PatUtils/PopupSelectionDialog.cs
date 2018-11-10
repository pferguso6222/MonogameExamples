using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Source.PatUtils;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Tweening;

namespace Source.PatUtils
{
    class PopupSelectionDialog : DrawableGameComponent, IDisposable
    {
        public Action notifyOpenComplete;
        public Action notifyPressedButtonA;
        public Action notifyPressedButtonB;
        public Action notifyCloseComplete;
        new Game Game;

        private SlicedSprite _slicedSprite;
        private SlicedSpriteAnimator slicedSpriteAnimator;

        protected BitmapFont font_normal;
        protected BitmapFont font_highlighted;
        protected BitmapFont font_pressed;

        protected BitmapFontButton buttonA;
        protected BitmapFontButton buttonB;

        protected Rectangle _startRect;
        protected Rectangle _endRect;

        private string _dialogText;
        private string _buttonAText;
        private string _buttonBText;

        private Vector2 _dialogTextPosition;

        public float pixelScaleFactor;

        bool contentShowing = false;

        ButtonMenu menu;
        KeyboardState previousState;
        GamePadState previousGamepadState;

        public PopupSelectionDialog
        (
            Game game,
            SlicedSprite slicedSprite,
            Point location,
            Point size,
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
            _endRect = new Rectangle(location.X, location.Y, size.X, size.Y);
            _startRect = new Rectangle(location.X, location.Y, 48, 48);
            this.pixelScaleFactor = pixelScaleFactor;
            _dialogText = dialogText;
            _buttonAText = buttonAText;
            _buttonBText = buttonBText;
            font_normal = fontNormal;
            font_highlighted = fontHighlighted;
            font_pressed = fontPressed;
            Rectangle contentRect = slicedSprite.GetContentAreaRectFromRect(_endRect);
            RectangleF dialogRect = fontNormal.GetStringRectangle(dialogText);
            dialogRect.Width *= pixelScaleFactor;
            dialogRect.Height *= pixelScaleFactor;
            RectangleF buttonARect = fontNormal.GetStringRectangle(buttonAText);
            buttonARect.Width *= pixelScaleFactor;
            buttonARect.Height *= pixelScaleFactor;
            RectangleF buttonBRect = fontNormal.GetStringRectangle(buttonBText);
            buttonBRect.Width *= pixelScaleFactor;
            buttonBRect.Height *= pixelScaleFactor;


            int contentPaddingY = contentRect.Height / 3;
            int contentPaddingX = contentRect.Width / 3;
            int dialogY = contentRect.Y + (int)(slicedSprite.BorderHeight * pixelScaleFactor) + contentPaddingY;
            float buttonsY = (dialogY + contentPaddingY);

            int menuPaddingX = contentRect.Width / 3;

            Console.WriteLine("contentRect:X:" + contentRect.X + ", Y:" + contentRect.Y + ", Width:" + contentRect.Width +", Height:" + contentRect.Height);

            int dtX = (int)(_endRect.Width - fontNormal.GetStringRectangle(dialogText).Width) / 2;
            Console.WriteLine("dtX:" + dtX);

            _dialogTextPosition = new Vector2((contentRect.X + ((contentRect.Width - dialogRect.Width) / 2)), (float)contentRect.Y + (float)contentPaddingY - (dialogRect.Height / 2));

            Console.WriteLine("_dialogTextPosition: X:" + _dialogTextPosition.X +" Y:" + _dialogTextPosition.Y);

            int menuPositionX = location.X;
            int menuPositionY = contentRect.Y + (contentRect.Height - (contentRect.Height / 3));

            Console.WriteLine("menuPositionX:" + menuPositionX +", Y:" + menuPositionY);


            menu = new ButtonMenu(new Point(location.X, menuPositionY), 2, 1, menuPaddingX, 0, null, null, Button.ButtonAlignment.CENTER);

            buttonA = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, _buttonAText, new Point(0, 0), Button.ButtonAlignment.CENTER);
            buttonB = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, _buttonBText, new Point(0, 0), Button.ButtonAlignment.CENTER);

            buttonA.OnPress = CloseA;
            buttonB.OnPress = CloseB;

            menu.addButtonAt(buttonA, 0, 0);
            menu.addButtonAt(buttonB, 1, 0);

            //buttonA._position = new Vector2(locatio);

            slicedSpriteAnimator = new SlicedSpriteAnimator(GameBase.Instance);
            GameBase.Instance.Components.Add(this);
            this.DrawOrder = 1;
            Open();
        }

        public void Open(){
            slicedSpriteAnimator.AnimateSlicedSprite(_slicedSprite, _startRect, _endRect, .25f, 0f, openComplete);
        }

        private void CloseA()
        {
            contentShowing = false;
            slicedSpriteAnimator.AnimateSlicedSprite(_slicedSprite, _endRect, _startRect, .25f, 0f, closeCompleteA);
        }
        private void CloseB(){
            contentShowing = false;
            slicedSpriteAnimator.AnimateSlicedSprite(_slicedSprite, _endRect, _startRect, .25f, 0f, closeCompleteB);
        }

        private void openComplete(Tween tween){
            contentShowing = true;
            menu.setActiveButton(0, 0);
            notifyOpenComplete?.Invoke();
        }

        private void closeCompleteA(Tween tween){
            GameBase.Instance.Components.Remove(this);
            slicedSpriteAnimator.DismissSprite();
            notifyPressedButtonA?.Invoke();
            notifyCloseComplete?.Invoke();
            Dispose();
        }

        private void closeCompleteB(Tween tween)
        {
            GameBase.Instance.Components.Remove(this);
            slicedSpriteAnimator.DismissSprite();
            notifyPressedButtonB?.Invoke();
            notifyCloseComplete?.Invoke();
            Dispose();
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
            // base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
                GameBase.Instance.Exit();

            // Move our sprite based on arrow keys being pressed:
            if (state.IsKeyDown(Keys.Right) & !previousState.IsKeyDown(
                Keys.Right))
                menu.setActiveOffset(1, 0);
            if (state.IsKeyDown(Keys.Left) & !previousState.IsKeyDown(
                Keys.Left))
                menu.setActiveOffset(-1, 0);
            if (state.IsKeyDown(Keys.Space) & !previousState.IsKeyDown(
                Keys.Space)){
                menu.PressCurrentButton();
                if (menu.yIndex == 0){
                    CloseA();
                }else{
                    CloseB();
                }
            }
                
            if (state.IsKeyDown(Keys.Enter) & !previousState.IsKeyDown(
                Keys.Enter))
                menu.PressCurrentButton();

            previousState = state;

            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            GamePadCapabilities capabilities2 = GamePad.GetCapabilities(PlayerIndex.Two);
            GamePadCapabilities capabilities3 = GamePad.GetCapabilities(PlayerIndex.Three);
            GamePadCapabilities capabilities4 = GamePad.GetCapabilities(PlayerIndex.Four);

            if (capabilities.IsConnected)
            {
                // Get the current state of Controller1
                GamePadState _state = GamePad.GetState(PlayerIndex.One);
                GamePadState _state2 = GamePad.GetState(PlayerIndex.Two);
                GamePadState _state3 = GamePad.GetState(PlayerIndex.Three);
                GamePadState _state4 = GamePad.GetState(PlayerIndex.Four);

                if (_state.IsButtonDown(Buttons.DPadRight) && !previousGamepadState.IsButtonDown(Buttons.DPadRight))
                    menu.setActiveOffset(1, 0);
                if (_state.IsButtonDown(Buttons.DPadLeft) && !previousGamepadState.IsButtonDown(Buttons.DPadLeft))
                    menu.setActiveOffset(-1, 0);

                previousGamepadState = _state;
            }
        }

        public void DismissSprite()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (contentShowing){
                StaticSpriteBatch.Instance.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1.0f));
                StaticSpriteBatch.Instance.DrawString(font_normal, _dialogText, new Vector2(_dialogTextPosition.X, _dialogTextPosition.Y), Color.White, 0.0f, new Vector2(0, 0), pixelScaleFactor, SpriteEffects.None, 0.0f);
                menu.Draw(gameTime);
                StaticSpriteBatch.Instance.End();
            }
        }
    }
}
