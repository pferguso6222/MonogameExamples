﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;

namespace Source.PatUtils
{
    public class NameEntry_Base : Screen
    {

        private Texture2D _background;
        private BitmapFont font_normal;
        private BitmapFont font_highlighted;
        private BitmapFont font_pressed;
        private SlicedSprite slicedSprite;

        KeyboardState previousState;
        GamePadState previousGamepadState;
        ButtonMenu menu;

        Rectangle bkgRect;
        Rectangle player1Rect;
        Rectangle player2Rect;
        Rectangle player3Rect;

        string chars;
        string chars_caps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,.!?";
        string chars_lowercase = "abcdefghijklmnopqrstuvwxyz,.!?";
        string char_spc = "SPC";
        string char_del = "DEL";
        string char_left = "<";
        string char_right = ">";

        public NameEntry_Base(
                                Texture2D backgroundImage,
                                SlicedSprite slicedSprite,
                                BitmapFont menuFontNormal,
                                BitmapFont menuFontHighlighted,
                                BitmapFont menuFontPressed)
        {
            _background = backgroundImage;
            this.slicedSprite = slicedSprite;
            font_normal = menuFontNormal;
            font_highlighted = menuFontHighlighted;
            font_pressed = menuFontPressed;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            previousState = Keyboard.GetState();
            previousGamepadState = GamePad.GetState(PlayerIndex.One);


            //THE FOLLOWING IS THE LAYOUT FOR NAME ENTRY BUTTONS. SAVE THIS FOR NAME ENTRY SCREEN
            int rows = 5;
            int cols = 6;

            Point menuSpacing = GameBase.Instance.ScreenPointFromScreenVector(new Vector2(.1f, .1f));

            menu = new ButtonMenu(GameBase.Instance.ScreenPointFromScreenVector(new Vector2(.25f, .25f)), 6, 5, menuSpacing.X, menuSpacing.Y, GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonClick_1"), GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonSelected_1"));

            chars = chars_caps;

            int row = 0;
            int col = 0;

            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                BitmapFontButton charButton = new BitmapFontButton(StaticSpriteBatch.Instance, font_normal, font_highlighted, font_pressed, c.ToString(), new Point(0, 0), Button.ButtonAlignment.CENTER);
                //charButton.OnPress = notifyButtonPressed;
                menu.addButtonAt(charButton, col, row);
                col++;
                if (col >= cols)
                {
                    col = 0;
                    row++;
                    if (row >= rows)
                    {
                        row = rows - 1;
                    }
                }
            }

            menu.setActiveButton(2, 2);

            bkgRect = new Rectangle(new Point(0, 0), new Point(GameBase.Instance.VirtualWidth, GameBase.Instance.VirtualHeight));

            player1Rect = new Rectangle(GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.2f, 0.2f)), GameBase.Instance.ScreenPointFromScreenVector(new Vector2(.75f, 0.2f)));
            player2Rect = new Rectangle(GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.2f, 0.45f)), GameBase.Instance.ScreenPointFromScreenVector(new Vector2(.75f, 0.2f)));
            player3Rect = new Rectangle(GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.2f, 0.7f)), GameBase.Instance.ScreenPointFromScreenVector(new Vector2(.75f, 0.2f)));

            Point position = GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.15f, 0.28f));
            Point spacing = GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.0f, .25f));

        }

        private void returnToMain()
        {
            GameBase.Instance.ChangeGameState(GameBase.GameState.TITLE_MAIN);
        }


        private void QuitVerify()
        {
            menu.Enabled = false;
            PopupSelectionDialog popup = new PopupSelectionDialog(GameBase.Instance,
                                             slicedSprite,
                                                                  GameBase.Instance.ScreenPointFromScreenVector(new Vector2(0.5f, 0.5f)),
                                                                  GameBase.Instance.ScreenPointFromScreenVector(new Vector2(.5f, .25f)),
                                             GameBase.Instance.GetCurrentPixelScale(),
                                             SlicedSprite.alignment.ALIGNMENT_MID_CENTER,
                                             "ARE YOU SURE YOU WANT TO QUIT?",
                                             "YES",
                                             "NO",
                                             font_normal,
                                             font_pressed,
                                             font_pressed)
            {
                notifyPressedButtonA = QuitGame,
                notifyPressedButtonB = CancelQuit
            };
        }

        private void CancelQuit()
        {
            menu.Enabled = true;
        }

        private void QuitGame()
        {
            GameBase.Instance.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            // base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
            {
                menu.Enabled = false;
                returnToMain();
            }

            // Move our sprite based on arrow keys being pressed:
            if (state.IsKeyDown(Keys.Up) & !previousState.IsKeyDown(
                Keys.Up))
                menu.setActiveOffset(0, -1);
            if (state.IsKeyDown(Keys.Down) & !previousState.IsKeyDown(
                Keys.Down))
                menu.setActiveOffset(0, 1);
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
                if (_state.IsButtonDown(Buttons.DPadUp) && !previousGamepadState.IsButtonDown(Buttons.DPadUp))
                    menu.setActiveOffset(0, -1);
                if (_state.IsButtonDown(Buttons.DPadDown) && !previousGamepadState.IsButtonDown(Buttons.DPadDown))
                    menu.setActiveOffset(0, 1);

                previousGamepadState = _state;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            StaticSpriteBatch.Instance.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1.0f));
            StaticSpriteBatch.Instance.Draw(_background, new Rectangle(new Point(0, 0), new Point(GameBase.Instance.VirtualWidth, GameBase.Instance.VirtualHeight)), Color.White);


            StaticSpriteBatch.Instance.End();

            slicedSprite.SetRectangle(bkgRect);
            slicedSprite.anchorPoint = SlicedSprite.alignment.ALIGNMENT_TOP_LEFT;
            slicedSprite.Draw();

            StaticSpriteBatch.Instance.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1.0f));
            menu.Draw(gameTime);

            StaticSpriteBatch.Instance.DrawString(font_normal, "PLAYER NAME", new Vector2(GameBase.Instance.VirtualWidth * .5f, GameBase.Instance.VirtualHeight * .1f), Color.White, 0.0f, new Vector2(font_normal.GetStringRectangle("PLAYER NAME").Width / 2, .5f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);

            StaticSpriteBatch.Instance.End();
        }
    }
}

