using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.BitmapFonts;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Utilities;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Graphics.Geometry;
//using MonoGame.Extended.Entities.Systems;

namespace BitmapFonts
{
    class BitmapFontDialogBox : EntitySystem
    {
        GraphicsDevice _graphicsDevice;
        BitmapFont _bitmapFont;
        Rectangle _rect;
        string _text;
        string _currentText;
        public SpriteBatch _spriteBatch;
        float delayBetweenCharacters = 0.1f;
        int _currentCharIndex;
        float _startTime = 0.0f;
        float _currentTime = 0.0f;
        bool animatingText;

        public BitmapFontDialogBox(GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch, string bitmapFontName, Rectangle rect, string text)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _rect = rect;
            _bitmapFont = content.Load<BitmapFont>(bitmapFontName);
            setString(text, true);
        }

        public void setString(string theString, bool animate)
        {
            _text = theString;
            _currentText = "";
            _currentCharIndex = 0;
            _startTime = _currentTime;
            animatingText = true;
        }

        public void Refresh()
        {
            if (_currentCharIndex < _text.Length)
            {
                char newChar = _text[_currentCharIndex];
                _currentText += newChar;
                _currentCharIndex++;
                
                //if our character is a space character, then let's add it and skip delaying
                if (newChar == ' ')
                {
                    newChar = _text[_currentCharIndex];
                    _currentText += newChar;
                    _currentCharIndex++;
                }

                Console.Write("Text Length: " + _bitmapFont.GetStringRectangle(_currentText) + "\n"); //use this to add automatic newline chars.

            }
            else
            {
                animatingText = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            ///Id we are animating text, check to see if we exceeded the delay between characters, and if so, refresh the field.
            if (animatingText)
            {
                _currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
                float elapsed = _currentTime - _startTime;
                if (elapsed >= delayBetweenCharacters)
                {
                    _startTime = _currentTime;
                    Refresh();
                }
            }
            
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null); //ensures we use PointClamp rendering to eliminatepixel smoothing

            //Draw our rectangle
            Texture2D rect = new Texture2D(_graphicsDevice, _rect.Width, _rect.Height);
            Color[] data = new Color[rect.Width * _rect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
            rect.SetData(data);
            Vector2 coor = new Vector2(_rect.X, _rect.Y);
            _spriteBatch.Draw(rect, coor, Color.Red);

            //draw our text
            _spriteBatch.DrawString(_bitmapFont, _currentText, new Vector2(_rect.X, _rect.Y), Color.White, 0.0f, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0.0f);

            _spriteBatch.End();
        }


    }
}
