using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Source.PatUtils
{
    public class ButtonMenu : IDisposable
    {
        public float xSpacing;
        public float ySpacing;
        int _rows;
        int _cols;
        Vector2 _position;//Position X and Y should be a number from 0.0f to 1.0f, representing their percentage of screen width or height

        int xIndex;
        int yIndex;

        SoundEffect sound_move_to_next_button;
        SoundEffect sound_press_button;

        protected SpriteBatch _spriteBatch;

        Button[,] buttons;

        private Button.ButtonAlignment alignment;

        public ButtonMenu(float xSpacing, float ySpacing, int cols, int rows, Vector2 position, SoundEffect nextButtonSound = null, SoundEffect pressButtonSound = null, Button.ButtonAlignment buttonAlignment = Button.ButtonAlignment.CENTER)
        {
            sound_move_to_next_button = nextButtonSound;
            sound_press_button = pressButtonSound;
            alignment = buttonAlignment;
            this.xSpacing = xSpacing;
            this.ySpacing = ySpacing;
            _cols = cols;
            _rows = rows;
            _position = position;

            //int[,] array2Da = new int[4, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };

            buttons = new Button[cols, rows];

            //for (int i = 0; i < _cols; i++){
              //  for (int j = 0; j < _rows; j++){
                    //Console.Write("buttons[" + i + "][" + j + "]: " + buttons[i, j]);
                //}
            //}
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

        public Button getButtonAt(int col, int row){
            return buttons[col, row];
        }

        public void addButtonAt(Button button, int col, int row){
            buttons[col, row] = button;
            updateButtonPositions();
        }

        private void updateButtonPositions(){
            float startX = GameBase.Instance.ScreenWidth * _position.X;
            float startY = GameBase.Instance.ScreenWidth * _position.Y;

            float currentX = startX;
            float currentY = startY;

            float xSpc = GameBase.Instance.ScreenWidth * xSpacing;
            float ySpc = GameBase.Instance.ScreenHeight * ySpacing;

            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Button b = buttons[i, j];
                    if (b != null)
                    {
                        b._position = new Vector2(currentX / GameBase.Instance.ScreenWidth, currentY / GameBase.Instance.ScreenHeight);
                    }
                    currentY += ySpc;
                }
                currentY = startY;
                currentX += xSpc;

            }
        }

        /// <summary>
        /// This sets the active button according to a direction offset in X or Y.
        /// </summary>
        public void setActiveOffset(int x, int y){

            //if there is only one column, do not allow wrapping
            if (x != 0 && _cols <= 1) return;
 
            int newX = xIndex + x;
            int newY = yIndex + y;

            //Wrap around in X
            if (newX >= _cols){ newX = 0;} else if (newX < 0){newX = _cols - 1;}

            //Wrap Around in Y
            if (newY >= _rows){newY = 0; } else if (newY < 0){newY = _rows - 1;}

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
                    //Console.Write("buttons[" + i + "][" + j + "]: " + buttons[i, j]);

                    if (b != null){
                        b.Draw(gameTime);
                    }
                }
            }
        }

        public void Dispose()
        {
            buttons = null;
        }
    }
}
