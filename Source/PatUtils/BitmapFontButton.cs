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

        public Point _origin = new Point();
        string _buttonText;
        protected SpriteBatch _spriteBatch;
        private Point _current_position = new Point();

        public float pixelScaleFactor = 1.0f;

        //public Action DynamicPixelScaleFunction;

        public BitmapFontButton(SpriteBatch spriteBatch, 
                                BitmapFont textNormal, 
                                BitmapFont textHighlighted,
                                BitmapFont textPressed,
                                string buttonText, 
                                Point position, 
                                Button.ButtonAlignment alignment)
        {
            this.IsVisible = true;
            _spriteBatch = spriteBatch;
            _textNormal = textNormal;
            _textHighlighted = textHighlighted;
            _textPressed = textPressed;
            _buttonText = buttonText;
            _position = position;
            _alignment = alignment;
        }

        protected override void UpdateOrigin(){
            int _totalWidth = GetWidth();
            _origin.Y = 0;

            switch (_alignment){
                case ButtonAlignment.CENTER:
                    _origin.X = -(_totalWidth / 2);
                    break;
                case ButtonAlignment.LEFT:
                    _origin.X = 0;
                    break;
                case ButtonAlignment.RIGHT:
                    _origin.X = -_totalWidth;
                    break;
            }
        }

        public override int GetWidth()
        {
            return (int)(_textNormal.GetStringRectangle(_buttonText).Width * GameBase.Instance.GetCurrentPixelScale());
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

        public override void UpdatePosition(){
            UpdateOrigin();
            _current_position.X = (_position.X + _origin.X);
            _current_position.Y = _position.Y;
        }

        public override void Draw(GameTime gameTime) 
        {
            if (!IsVisible) return;
            switch(State){
                case BUTTON_STATE.NORMAL:
                        _spriteBatch.DrawString(_textNormal, _buttonText, new Vector2(_current_position.X, _current_position.Y), Color.White, 0.0f, new Vector2(0.0f, 0.0f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);
                    break;
                case BUTTON_STATE.HIGHLIGHTED:
                    _spriteBatch.DrawString(_textHighlighted, _buttonText, new Vector2(_current_position.X, _current_position.Y), Color.White, 0.0f, new Vector2(0.0f, 0.0f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);
                    break;
                case BUTTON_STATE.PRESSED:
                    _spriteBatch.DrawString(_textPressed, _buttonText, new Vector2(_current_position.X, _current_position.Y), Color.White, 0.0f, new Vector2(0.0f, 0.0f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);
                    break;
            }
        }

    }
}
