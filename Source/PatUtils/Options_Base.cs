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

    public class Options_Base : Screen
    {
        private Texture2D _background;
        private BitmapFont font_normal;
        private BitmapFont font_highlighted;
        private BitmapFont font_pressed;

        KeyboardState previousState;
        GamePadState previousGamepadState;

        ButtonMenu menu;

        private int currentResolutionIndex = 0;
        private bool editingResolution = false;
        private List<string> resolutionStrings = new List<string>();
        private List<Point> supportedResolutions = new List<Point>();
        private string currentResolutionString;

        public Options_Base(Texture2D backgroundImage,
                                BitmapFont menuFontNormal,
                               BitmapFont menuFontHighlighted,
                               BitmapFont menuFontPressed)
        {
            _background = backgroundImage;
            font_normal = menuFontNormal;
            font_highlighted = menuFontHighlighted;
            font_pressed = menuFontPressed;

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                string leftArrow = (mode.Width <= 999) ? "<  " : "< ";
                string rightArrow = (mode.Height <= 999) ? "  >" : " >";
                string resolutionString = string.Concat(leftArrow, mode.Width, " x ", mode.Height, rightArrow);
                resolutionStrings.Add(resolutionString);
                if (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width == mode.Width && GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height == mode.Height){
                    currentResolutionString = resolutionString;
                }
                Point resolution = new Point(mode.Width, mode.Height);
                supportedResolutions.Add(resolution);
                Console.WriteLine(resolutionString);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();
            previousState = Keyboard.GetState();
            previousGamepadState = GamePad.GetState(PlayerIndex.One);

            menu = new ButtonMenu(0, 100, 1, 4, new Vector2(GameBase.Instance.ScreenWidth() * .1f, GameBase.Instance.ScreenHeight() * .3f), GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonClick_1"), GameBase.Instance.Content.Load<SoundEffect>(".\\ButtonSelected_1"), Button.ButtonAlignment.LEFT);

            //Return to Main Menu
            BitmapFontButton bReturn = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "RETURN", new Vector2(.1f, .25f), Button.ButtonAlignment.LEFT);
            bReturn.OnPress = returnToMain;
            menu.addButtonAt(bReturn, 0, 0);

            //Windowed / Fullscreen
            BitmapFontButton bFullscreen = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "DISPLAY MODE", new Vector2(.1f, .3f), Button.ButtonAlignment.LEFT);
            bFullscreen.OnPress = toggleFullscreen;
            menu.addButtonAt(bFullscreen, 0, 1);

            //Change Resolution
            BitmapFontButton bResolution = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "RESOLUTION", new Vector2(.1f, .35f), Button.ButtonAlignment.LEFT);
            bResolution.OnPress = changeResolution;
            menu.addButtonAt(bResolution, 0, 2);

            //Pixel Filtering
            BitmapFontButton bFiltering = new BitmapFontButton(GameBase.Instance.spriteBatch, font_normal, font_highlighted, font_pressed, "PIXEL FILTERING", new Vector2(.1f, .4f), Button.ButtonAlignment.LEFT);
            bFiltering.OnPress = toggleFiltering;
            menu.addButtonAt(bFiltering, 0, 3);

            menu.setActiveButton(0, 0);

        }

        private void changeResolution(){
            if (!editingResolution){
                editingResolution = true;
            }else{
                editingResolution = false;
            }
        }

        private void toggleFullscreen(){
            GameBase.Instance.graphics.ToggleFullScreen();
            GameBase.Instance.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            GameBase.Instance.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            GameBase.Instance.graphics.ApplyChanges();

            //Save Selection to disk.
            GameBase.Instance.GameConfig.data.isFullScreen = GameBase.Instance.graphics.IsFullScreen;
            GameBase.Instance.GameConfig.Save();
        }

        private void onDisplayPressed(){
            toggleFullscreen();
            /*Console.WriteLine("Resolutions Available:\n");
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Console.WriteLine(mode.Width + "X" + mode.Height +"\n");
            }*/
        }

        private void toggleFiltering(){

            if (GameBase.Instance.SamplerStateIndex == 0){
                GameBase.Instance.SamplerStateIndex = 1;
            }
            else if (GameBase.Instance.SamplerStateIndex == 1){
                GameBase.Instance.SamplerStateIndex = 2;
            }
            else{
                GameBase.Instance.SamplerStateIndex = 0;
            }
            GameBase.Instance.UpdateSamplerState();

            //Save Selection to Disk
            GameBase.Instance.GameConfig.data.SamplerStateIndex = GameBase.Instance.SamplerStateIndex;
            GameBase.Instance.GameConfig.Save();
        }

        private void returnToMain(){
            GameBase.Instance.ChangeGameState(GameBase.GameState.TITLE_MAIN);
        }

        private void LoadOptions(){
            GameBase.Instance.ChangeGameState(GameBase.GameState.OPTIONS_MAIN);
        }

        private void QuitGame(){
            GameBase.Instance.Exit();
        }

        private void editResolution(){
            KeyboardState state = Keyboard.GetState();

            // Move our sprite based on arrow keys being pressed:
            if (state.IsKeyDown(Keys.Right) & !previousState.IsKeyDown(
                Keys.Right)){
                SetResolutionSelectionOffset(1);
            }
            if (state.IsKeyDown(Keys.Left) & !previousState.IsKeyDown(
                Keys.Left)){
                SetResolutionSelectionOffset(-1);
            }
                

            if (state.IsKeyDown(Keys.Space) & !previousState.IsKeyDown(
                Keys.Space)){
                SetSelectedResolution();
            }
                
            if (state.IsKeyDown(Keys.Enter) & !previousState.IsKeyDown(
                Keys.Enter)){
                SetSelectedResolution();
            }
            previousState = state;

            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

            if (capabilities.IsConnected)
            {
                // Get the current state of Controller1
                GamePadState _state = GamePad.GetState(PlayerIndex.One);

                if (_state.IsButtonDown(Buttons.DPadRight) && !previousGamepadState.IsButtonDown(Buttons.DPadRight))
                    menu.setActiveOffset(1, 0);
                if (_state.IsButtonDown(Buttons.DPadLeft) && !previousGamepadState.IsButtonDown(Buttons.DPadLeft))
                    menu.setActiveOffset(-1, 0);

                previousGamepadState = _state;
            }
        }

        private void SetResolutionSelectionOffset(int offset){
            currentResolutionIndex += offset;
            if (currentResolutionIndex <= 0){
                currentResolutionIndex = resolutionStrings.Count - 1;
            }else if (currentResolutionIndex >= resolutionStrings.Count){
                currentResolutionIndex = 0;
            }
            currentResolutionString = resolutionStrings[currentResolutionIndex];
        }

        private void SetSelectedResolution(){
            GameBase.Instance.graphics.PreferredBackBufferWidth = supportedResolutions[currentResolutionIndex].X;
            GameBase.Instance.graphics.PreferredBackBufferHeight = supportedResolutions[currentResolutionIndex].Y;
            GameBase.Instance.graphics.ApplyChanges();

            GameBase.Instance.GameConfig.data.screenWidth = supportedResolutions[currentResolutionIndex].X;
            GameBase.Instance.GameConfig.data.screenHeight = supportedResolutions[currentResolutionIndex].Y;
            GameBase.Instance.GameConfig.Save();
            changeResolution();
        }

        public override void Update(GameTime gameTime)
        {
            if (editingResolution){
                editResolution();
                return;
            }

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

            if (capabilities.IsConnected)
            {
                // Get the current state of Controller1
                GamePadState _state = GamePad.GetState(PlayerIndex.One);

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
            GameBase.Instance.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, GameBase.Instance.SamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1.0f));
            GameBase.Instance.spriteBatch.Draw(_background, new Rectangle(new Point(0, 0), new Point(GameBase.Instance.ScreenWidth(), GameBase.Instance.ScreenHeight())), Color.White);
            GameBase.Instance.spriteBatch.DrawString(font_normal, "GAME OPTIONS", new Vector2(GameBase.Instance.ScreenWidth() * .5f, GameBase.Instance.ScreenHeight() * .1f), Color.White, 0.0f, new Vector2(font_normal.GetStringRectangle("GAME OPTIONS").Width / 2,.5f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);
            GameBase.Instance.spriteBatch.DrawString(font_normal, GameBase.Instance.graphics.IsFullScreen? "FULLSCREEN" : "WINDOWED", new Vector2(GameBase.Instance.ScreenWidth() * .65f, GameBase.Instance.ScreenHeight() * menu.getButtonAt(0, 1)._position.Y), Color.White, 0.0f, new Vector2(font_normal.GetStringRectangle(GameBase.Instance.graphics.IsFullScreen ? "FULLSCREEN" : "WINDOWED").Width / 2, .5f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);
            if (editingResolution){
                editResolution();
                GameBase.Instance.spriteBatch.DrawString(font_pressed, currentResolutionString, new Vector2(GameBase.Instance.ScreenWidth() * .65f, GameBase.Instance.ScreenHeight() * menu.getButtonAt(0, 2)._position.Y), Color.White, 0.0f, new Vector2(font_pressed.GetStringRectangle(currentResolutionString).Width / 2, .5f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);
            }
            else{
                GameBase.Instance.spriteBatch.DrawString(font_normal, GameBase.Instance.ResolutionString(), new Vector2(GameBase.Instance.ScreenWidth() * .65f, GameBase.Instance.ScreenHeight() * menu.getButtonAt(0, 2)._position.Y), Color.White, 0.0f, new Vector2(font_normal.GetStringRectangle(GameBase.Instance.ResolutionString()).Width / 2, .5f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);
            }
            GameBase.Instance.spriteBatch.DrawString(font_normal, GameBase.Instance.SamplerStateString(), new Vector2(GameBase.Instance.ScreenWidth() * .65f, GameBase.Instance.ScreenHeight() * menu.getButtonAt(0, 3)._position.Y), Color.White, 0.0f, new Vector2(font_normal.GetStringRectangle(GameBase.Instance.SamplerStateString()).Width / 2, .5f), GameBase.Instance.GetCurrentPixelScale(), SpriteEffects.None, 0.0f);

            menu.Draw(gameTime);
            GameBase.Instance.spriteBatch.End();
        }
    }
}