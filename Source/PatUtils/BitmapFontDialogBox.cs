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

namespace PatUtils
{
    class BitmapFontDialogBox : EntitySystem
    {
        GraphicsDevice _graphicsDevice;
        BitmapFont _bitmapFont;
        Rectangle _rect;
        string _text;
        List<List<string>> _pageArray;
        int _textArrayIdx;
        int _currentPage;
        string _currentText;
        public SpriteBatch _spriteBatch;
        float delayBetweenCharacters = 0.1f;
        int _currentCharIndex;
        float _startTime = 0.0f;
        float _currentTime = 0.0f;
        public bool paused = true;
        float _pixelScale = 1.0f;

        public static char pageBreak = '\b';


        public BitmapFontDialogBox(GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch, string bitmapFontName, Rectangle rect, string text, float pixelScale)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _rect = rect;
            _bitmapFont = content.Load<BitmapFont>(bitmapFontName);
            _pixelScale = pixelScale;
            setString(text, true);
        }

        public void setString(string theString, bool animate)
        {
            _text = theString;
            //Console.Write("Set String: '" + "', stringLength: " + _text.Length);
            _pageArray = pageArrayFromRect(_text, _rect, _bitmapFont);
            _textArrayIdx = 0;
            _currentText = "";
            _currentCharIndex = 0;
            _startTime = _currentTime;
            paused = false;
        }

        private bool rangeValid(Point range, int maxLength){
            return (range.X + range.Y < maxLength - 1);
        }

        //Returns the string divided into a string array, where each string will fit in the rectangle, including with newline chars added
        private List<List<string>> pageArrayFromRect(string str, Rectangle rect, BitmapFont font)
        {
            List<List<string>> pageArray = new List<List<string>>();
            List<string> textArray;
            _currentText = "";
            Point currentTextRange = new Point(0, 0);//char index, length
            char newChar = '.';
            int currentSpaceCharIdx = 0;
            string _newLine = "";
            //string secondArg = myString.Substring(begin, end - begin + 1);//get range inside string

            while(rangeValid(currentTextRange, str.Length))
            {
                textArray = new List<string>();

                //build columns
                while (font.GetStringRectangle(_currentText + "\n").Height < (double)(rect.Height / _pixelScale))//add extra line when we measure
                {
                    //build rows
                    _newLine = "";

                    while (font.GetStringRectangle(_newLine + 'Z').Width < (double)(rect.Width / _pixelScale))//add extra character when we measure
                    {

                        if ((currentTextRange.X + currentTextRange.Y) < str.Length)
                        {
                            currentTextRange.Y++;
                        }
                        newChar = str[currentTextRange.X + currentTextRange.Y];

                        if (newChar == ' ')
                        {
                            if (currentTextRange.Y >= 2)
                            {
                                currentSpaceCharIdx = currentTextRange.Y;
                            }
                        }
                        _newLine += newChar;

                        if ((currentTextRange.X + currentTextRange.Y) >= str.Length - 1)
                        {
                            //End of entire string.
                            currentTextRange.Y++;
                            break;
                        }
                    }

                    if ((currentTextRange.X + currentTextRange.Y) < str.Length - 1)
                    {
                        currentTextRange.Y = currentSpaceCharIdx;//we hit a new word that was too big to fit on the line, so go back to the last space and end the line
                    }
                    
                    _newLine = str.Substring(currentTextRange.X, currentTextRange.Y);

                    //reached end of line, so add new line char
                    if ((currentTextRange.X + currentTextRange.Y) < str.Length - 1)
                    {
                        _newLine += '\n';
                    }
                    

                    //add current line onto _currentText
                    _currentText += _newLine;
                    if ((currentTextRange.X + currentTextRange.Y) >= str.Length - 1){
                        //End of entire string.
                        textArray.Add(_currentText);
                        break;
                    }
                    else
                    {
                        currentTextRange.X += currentTextRange.Y + 1;//increment ange start index, adding 1 to remove a first character space
                        currentTextRange.Y = 0;//reset range length
                    }

                    textArray.Add(_currentText);
                }

                Console.Write("_pageArray.Add(" + _currentText + ") + width:" + font.GetStringRectangle(_newLine).Width + "\n");
                pageArray.Add(textArray);
                _currentText = "";

                if ((currentTextRange.X + currentTextRange.Y) >= str.Length - 1)
                {
                    //End of entire string.
                    break;
                }

                currentTextRange.X += currentTextRange.Y;
                currentTextRange.Y = 0;
            }

            Console.Write("PageArray Created. PageArray.Count: " + pageArray.Count +"\n");
            for (int i = 0; i < pageArray.Count; i++)
            {
                List<string> _txt = pageArray[i];

                Console.Write("pageArray[" + i + "]");
                for (int j = 0; j < _txt.Count; j++)
                {
                    string _str = _txt[j];
                    Console.Write(_str +"\n");
                }
            }
            return pageArray;
        }

        public void Refresh()
        {
            if (_currentPage >= _pageArray.Count){
                paused = true;
                return;
            }
            if (_currentCharIndex <= _pageArray[_currentPage][_textArrayIdx].Length -1)
            {
                if (_currentCharIndex == 0) _currentText = "";
                //char newChar = _textArray[_textArrayIdx][_currentCharIndex];
                char newChar = _pageArray[_currentPage][_textArrayIdx][_currentCharIndex];
                _currentText += newChar;
                _currentCharIndex++;
                
                //if our character is a space character, then let's add it and skip delaying
                if (newChar == ' ')
                {
                    newChar = _pageArray[_currentPage][_textArrayIdx][_currentCharIndex];
                    _currentText += newChar;
                    _currentCharIndex++;
                }

                //Console.Write("Text: " + _currentText + " length: " + _bitmapFont.GetStringRectangle(_currentText) + "\n"); //use this to add automatic newline chars.

            }
            else
            {
                _textArrayIdx++;
                if (_textArrayIdx >= _pageArray[_currentPage].Count && _currentPage < _pageArray.Count)
                {
                    _currentCharIndex = 0;
                    _currentPage++;
                    _textArrayIdx = 0;
                    paused = true;
                }
                else
                {
                    //finished line
                    //paused = true;
                }
                
            }
        }

        public void Update(GameTime gameTime)
        {
            ///Id we are animating text, check to see if we exceeded the delay between characters, and if so, refresh the field.
            if (!paused)
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
            _spriteBatch.Draw(rect, coor, Color.Black);

            //draw our text
            _spriteBatch.DrawString(_bitmapFont, _currentText, new Vector2(_rect.X, _rect.Y), Color.White, 0.0f, new Vector2(0, 0), _pixelScale, SpriteEffects.None, 0.0f);

            _spriteBatch.End();
        }


    }
}
