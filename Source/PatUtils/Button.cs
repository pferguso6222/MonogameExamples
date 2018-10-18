﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Source.PatUtils
{
    public abstract class Button : IDisposable
    {

        public enum BUTTON_STATE{
            NORMAL,
            HIGHLIGHTED,
            PRESSED,
            NULL,
        }

        public BUTTON_STATE State = BUTTON_STATE.NORMAL;

        public bool IsInitialized
        {
            get;
            private set;
        }

        public bool IsVisible
        {
            get;
            set;
        }

        public Vector2 _position
        {
            get;
            set;
        }

        public virtual void Dispose() { }

        public void Show() { }

        public void Hide() { }

        public virtual void Initialize() { }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
    }
}