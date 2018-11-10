using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Source.PatUtils
{
    public class ButtonMenu : IDisposable
    {
        public int xSpacing;
        public int ySpacing;
        int _rows;
        int _cols;
        Point _position;

        public int xIndex;
        public int yIndex;

        SoundEffect sound_move_to_next_button;
        SoundEffect sound_press_button;

        protected SpriteBatch _spriteBatch;

        Button[,] buttons;

        private Button.ButtonAlignment alignment;

        public bool Enabled = true;

        public ButtonMenu(Point position, int cols, int rows, int xSpacing, int ySpacing, SoundEffect nextButtonSound = null, SoundEffect pressButtonSound = null, Button.ButtonAlignment buttonAlignment = Button.ButtonAlignment.CENTER)
        {
            _position = position;
            _cols = cols;
            _rows = rows;
            this.xSpacing = xSpacing;
            this.ySpacing = ySpacing;
            sound_move_to_next_button = nextButtonSound;
            sound_press_button = pressButtonSound;
            alignment = buttonAlignment;

            //int[,] array2Da = new int[4, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };

            buttons = new Button[cols, rows];

            //for (int i = 0; i < _cols; i++){
              //  for (int j = 0; j < _rows; j++){
                    //Console.Write("buttons[" + i + "][" + j + "]: " + buttons[i, j]);
                //}
            //}
        }

        public void PressCurrentButton(){
            if (Enabled)
            {
                Button b = buttons[xIndex, yIndex];
                if (b.Enabled)
                {
                    b.Press();
                    b.State = Button.BUTTON_STATE.PRESSED;
                    if (sound_press_button != null)
                    {
                        sound_press_button.Play();
                    }
                }
            }
        }

        public void SetAllButtonsEnabled(bool enable){
            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Button b = buttons[i, j];
                    if (b != null)
                    {
                        b.Enabled = enable;
                    }
                   
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
            int currentX = _position.X;
            int currentY = _position.Y;

            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Button b = buttons[i, j];
                    if (b != null)
                    {
                        b._position = new Point(currentX, currentY);
                    }
                    currentY += ySpacing;
                }
                currentY = _position.Y;
                currentX += xSpacing;
            }

            int xOffset = 0;

            switch (alignment)
            {
                case Button.ButtonAlignment.CENTER:
                    xOffset = -((xSpacing * (_cols -1)) / 2);
                    break;
                case Button.ButtonAlignment.RIGHT:
                    xOffset = -(xSpacing * (_cols -1));
                    break;
                default:
                    break;
            }

            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Button b = buttons[i, j];
                    if (b != null)
                    {
                        Point newPos = b._position;
                        newPos.X += xOffset;
                        b._position = newPos;
                        b.UpdatePosition();
                    }

                }

            }
        }

        /// <summary>
        /// This sets the active button according to a direction offset in X or Y.
        /// </summary>
        public void setActiveOffset(int x, int y){
            if  (Enabled){
                //if there is only one column, do not allow wrapping
                if (x != 0 && _cols <= 1) return;

                int newX = xIndex + x;
                int newY = yIndex + y;

                //Wrap around in X
                if (newX >= _cols) { newX = 0; } else if (newX < 0) { newX = _cols - 1; }

                //Wrap Around in Y
                if (newY >= _rows) { newY = 0; } else if (newY < 0) { newY = _rows - 1; }

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
        }

        public void setActiveButton(int col, int row){
            if (Enabled){
                if (row < 0 || row >= _rows) return;
                if (col < 0 || col >= _cols) return;
                xIndex = col;
                yIndex = row;
                setAllNormal();
                Button b = buttons[col, row];
                if (b != null)
                {
                    b.State = Button.BUTTON_STATE.HIGHLIGHTED;
                    b.Enabled = true;
                }
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
