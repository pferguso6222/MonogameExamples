using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Source.PatUtils
{
    public abstract class Button : IDisposable
    {

        public Action OnPress;

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

        public bool Enabled
        {
            get;
            set;
        }

        public Vector2 _position
        {
            get;
            set;
        }

        public virtual float GetWidth(){
            return 0.0f;
        }

        public virtual void Press(){
            OnPress();
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
