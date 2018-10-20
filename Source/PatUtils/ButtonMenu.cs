using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Source.PatUtils
{
    public class ButtonMenu
    {
        int _xSpacing;
        int _ySpacing;
        int _rows;
        int _cols;
        Vector2 _position;

        int xIndex;
        int yIndex;

        SoundEffect sound_move_to_next_button;
        SoundEffect sound_press_button;

        protected SpriteBatch _spriteBatch;

        Button[,] buttons;

        public ButtonMenu(int xSpacing, int ySpacing, int cols, int rows, Vector2 position, SoundEffect nextButtonSound = null, SoundEffect pressButtonSound = null)
        {
            sound_move_to_next_button = nextButtonSound;
            sound_press_button = pressButtonSound;

            _xSpacing = xSpacing;
            _ySpacing = ySpacing;
            _cols = cols;
            _rows = rows;
            _position = position;
           // xIndex = 0;
            //yIndex = 0;

            //int[,] array2Da = new int[4, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };

            buttons = new Button[cols, rows];

            for (int i = 0; i < _cols; i++){
                for (int j = 0; j < _rows; j++){
                    Console.Write("buttons[" + i + "][" + j + "]: " + buttons[i, j]);
                }
            }
        }

        public void PressCurrentButton(){
            Button b = buttons[xIndex, yIndex];
            if (b.Enabled){
                b.Press();
                b.State = Button.BUTTON_STATE.PRESSED;
                if (sound_press_button != null)
                {
                    sound_press_button.Play();
                }
            }
        }

        private void notifyButtonPressed(){

        }

        public int xSpacing{
            get{
                return _xSpacing;
            }
        }

        public int ySpacing
        {
            get
            {
                return _ySpacing;
            }
        }

        public void addButtonAt(Button button, int col, int row){
            buttons[col, row] = button;
            button._position = new Vector2(_position.X + (_xSpacing * col), _position.Y + (_ySpacing * row));
        }

        public void setActiveOffset(int x, int y){
            int newX = xIndex + x;
            int newY = yIndex + y;

            if (newX >= _cols){
                newX = 0;
            }else if (newX < 0){
                newX = _cols - 1;
            }

            if (newY >= _rows){
                newY = 0;
            }else if (newY < 0){
                newY = _rows - 1;
            }

            xIndex = newX;
            yIndex = newY;

            setAllNormal();
            Button b = buttons[xIndex, yIndex];
            if (b != null)
            {
                b.State = Button.BUTTON_STATE.HIGHLIGHTED;
                b.Enabled = true;
                if (sound_move_to_next_button != null)
                {
                    sound_move_to_next_button.Play();
                }
            }
        }

        public void setActiveButton(int col, int row){
            if (row < 0 || row >= _rows) return;
            if (col < 0 || col >= _cols) return;
            xIndex = col;
            yIndex = row;
            setAllNormal();
            Button b = buttons[col, row];
            if (b != null)
            {
                b.State = Button.BUTTON_STATE.HIGHLIGHTED;
            }
        }

        private void setAllNormal(){
            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Button b = buttons[i, j];
                    if (b != null)
                    {
                        b.State = Button.BUTTON_STATE.NORMAL;
                        b.Enabled = false;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Button b = buttons[i, j];
                    Console.Write("buttons[" + i + "][" + j + "]: " + buttons[i, j]);

                    if (b != null){
                        b.Draw(gameTime);
                    }
                }
            }
        }
    }
}
