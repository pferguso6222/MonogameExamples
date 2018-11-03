﻿using System;
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
            _startRect = new Rectangle((int)(GameBase.Instance.ScreenWidth * location.X), (int)(GameBase.Instance.ScreenHeight * location.Y), 0, 0);
            _endRect = new Rectangle((int)(GameBase.Instance.ScreenWidth * location.X), (int)(GameBase.Instance.ScreenHeight * location.Y), (int)(GameBase.Instance.ScreenWidth * size.X), (int)(GameBase.Instance.ScreenHeight * size.Y));
            this.pixelScaleFactor = pixelScaleFactor;

            RectangleF dialogRect = fontNormal.GetStringRectangle(dialogText);
            RectangleF buttonARect = fontNormal.GetStringRectangle(buttonAText);
            RectangleF buttonBRect = fontNormal.GetStringRectangle(buttonBText);

            float contentWidth;
            float contentHeight = dialogRect.Height + buttonARect.Height;
            float buttonsBothWidth = buttonARect.Width + buttonBRect.Width;
            float contentAreaHeight = _endRect.Height - (slicedSprite.BorderHeight * 2);
            float contentAreaWidth = _endRect.Width - (slicedSprite.BorderWidth * 2);

            if (buttonsBothWidth >= dialogRect.Width){
                contentWidth = buttonsBothWidth;
            }else{
                contentWidth = dialogRect.Width;
            }
            float contentPaddingY = contentAreaHeight / 3;
            float contentPaddingX = contentAreaWidth / 3;
            float dialogY = location.Y + slicedSprite.BorderHeight + contentPaddingY;
            float buttonsY = (dialogY + dialogRect.Height + contentPaddingY + buttonARect.Height) / GameBase.Instance.ScreenHeight;
            float menuPositionX = location.X - size.X / 2.0f + (contentPaddingX / GameBase.Instance.ScreenWidth);
            float menuPaddingX = (buttonARect.Width + contentPaddingX) / GameBase.Instance.ScreenWidth;

            _dialogText = dialogText;
            _buttonAText = buttonAText;
            _buttonBText = buttonBText;
            font_normal = fontNormal;
            font_highlighted = fontHighlighted;
            font_pressed = fontPressed;

            _dialogTextPosition = new Vector2(location.X - ((dialogRect.Width / 2.0f) / GameBase.Instance.ScreenWidth), location.Y + (slicedSprite.BorderHeight + contentHeight) / GameBase.Instance.ScreenHeight);

            menu = new ButtonMenu(menuPaddingX, 0.0f, 2, 1, new Vector2(menuPositionX, _dialogTextPosition.Y));

            buttonA = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, _buttonAText, new Vector2(.0f, .0f), Button.ButtonAlignment.CENTER);
            buttonB = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, _buttonBText, new Vector2(.0f, .0f), Button.ButtonAlignment.CENTER);

            menu.addButtonAt(buttonA, 0, 0);
            menu.addButtonAt(buttonB, 1, 0);

            menu.setActiveButton(0, 0);

            //buttonA._position = new Vector2(locatio);

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
                Keys.Space))
                menu.PressCurrentButton();
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
                StaticSpriteBatch.Instance.DrawString(font_normal, _dialogText, new Vector2(GameBase.Instance.ScreenWidth * _dialogTextPosition.X, GameBase.Instance.ScreenHeight * _dialogTextPosition.Y), Color.White, 0.0f, new Vector2(50, 1), pixelScaleFactor, SpriteEffects.None, 0.0f);
                menu.Draw(gameTime);
                StaticSpriteBatch.Instance.End();
            }
        }
    }
}
