﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Source.PatUtils
{
    public class BitmapFontButton : Button
    {

        BitmapFont _textNormal;
        BitmapFont _textHighlighted;
        string _buttonText;
        protected SpriteBatch _spriteBatch;
        float _pixelScale;
        Vector2 _origin;

        public BitmapFontButton(SpriteBatch spriteBatch, 
                                BitmapFont textNormal, 
                                BitmapFont textHighlighted, 
                                string buttonText, 
                                Vector2 position, 
                                Vector2 origin, 
                                float pixelScale)
        {
            _pixelScale = pixelScale;
            _spriteBatch = spriteBatch;
            _textNormal = textNormal;
            _textHighlighted = textHighlighted;
            _buttonText = buttonText;
            _origin = origin;
            _position = position;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime) 
        { 
            switch(State){
                case BUTTON_STATE.NORMAL:
                    _spriteBatch.DrawString(_textNormal, _buttonText, _position, Color.White, 0.0f, _origin, _pixelScale, SpriteEffects.None, 0.0f);
                    break;
                case BUTTON_STATE.HIGHLIGHTED:
                    _spriteBatch.DrawString(_textHighlighted, _buttonText, _position, Color.White, 0.0f, _origin, _pixelScale, SpriteEffects.None, 0.0f);
                    break;
                case BUTTON_STATE.PRESSED:
                    _spriteBatch.DrawString(_textHighlighted, _buttonText, _position, Color.White, 0.0f, _origin, _pixelScale, SpriteEffects.None, 0.0f);
                    break;
            }
        }

    }
}
