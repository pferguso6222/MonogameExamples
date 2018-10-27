using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Source.PatUtils
{
    public class BitmapFontButton : Button
    {
        BitmapFont _textNormal;
        BitmapFont _textHighlighted;
        BitmapFont _textPressed;

        Vector2 _origin = new Vector2();
        string _buttonText;
        protected SpriteBatch _spriteBatch;
        float _pixelScale;
        private Vector2 _current_position = new Vector2();

        public BitmapFontButton(SpriteBatch spriteBatch, 
                                BitmapFont textNormal, 
                                BitmapFont textHighlighted,
                                BitmapFont textPressed,
                                string buttonText, 
                                Vector2 position, 
                                Button.ButtonAlignment alignment, 
                                float pixelScale)
        {
            this.IsVisible = true;
            _pixelScale = pixelScale;
            _spriteBatch = spriteBatch;
            _textNormal = textNormal;
            _textHighlighted = textHighlighted;
            _textPressed = textPressed;
            _buttonText = buttonText;
            _alignment = alignment;
            _position = position;
            _origin = GetOrigin(_buttonText);
        }

        public BitmapFontButton(SpriteBatch spriteBatch,
                                BitmapFont textNormal,
                                BitmapFont textHighlighted,
                                BitmapFont textPressed,
                                string buttonText,
                                Point position,
                                Button.ButtonAlignment alignment,
                                float pixelScale)
        {
            this.IsVisible = true;
            _pixelScale = pixelScale;
            _spriteBatch = spriteBatch;
            _textNormal = textNormal;
            _textHighlighted = textHighlighted;
            _textPressed = textPressed;
            _buttonText = buttonText;
            _alignment = alignment;
            _position = new Vector2(GameBase.Instance.ScreenWidth() / position.X, GameBase.Instance.ScreenHeight() / position.Y);
            _origin = GetOrigin(_buttonText);
        }

        private Vector2 GetOrigin(string text){
            float _totalWidth = _textNormal.GetStringRectangle(text).Width;
            _origin.Y = 0;

            switch (_alignment){
                case ButtonAlignment.CENTER:
                    _origin.X = _totalWidth / 2;
                    break;
                case ButtonAlignment.LEFT:
                    _origin.X = 0;
                    break;
                case ButtonAlignment.RIGHT:
                    _origin.X = _totalWidth;
                    break;
            }
            return _origin;
        }

        public override float GetWidth()
        {
            return (float)_textNormal.GetStringRectangle(_buttonText).Width * _pixelScale;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public void SetButtonText(string text){
            _buttonText = text;
        }

        public override void Press()
        {
            base.Press();
        }

        private void updatePosition(){
            _current_position.X = GameBase.Instance.ScreenWidth() * _position.X;
            _current_position.Y = GameBase.Instance.ScreenHeight() * _position.Y;
        }

        public override void Draw(GameTime gameTime) 
        {
            if (!IsVisible) return;
            updatePosition();
            switch(State){
                case BUTTON_STATE.NORMAL:
                    _spriteBatch.DrawString(_textNormal, _buttonText, _current_position, Color.White, 0.0f, _origin, _pixelScale, SpriteEffects.None, 0.0f);
                    break;
                case BUTTON_STATE.HIGHLIGHTED:
                    _spriteBatch.DrawString(_textHighlighted, _buttonText, _current_position, Color.White, 0.0f, _origin, _pixelScale, SpriteEffects.None, 0.0f);
                    break;
                case BUTTON_STATE.PRESSED:
                    _spriteBatch.DrawString(_textPressed, _buttonText, _current_position, Color.White, 0.0f, _origin, _pixelScale, SpriteEffects.None, 0.0f);
                    break;
            }
        }

    }
}
