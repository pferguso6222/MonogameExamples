using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.PatUtils;

namespace Source.PatUtils
{
    class SlicedSprite : IDisposable
    {

        private Texture2D sourceTex;
        private Texture2D[] sourceTextures = new Texture2D[9];
        private Rectangle[] sourceRectangles = new Rectangle[9];
        private Rectangle[] targetRectangles = new Rectangle[9];
        private Rectangle rectangle;
        private Rectangle originalRect;
        public float pixelScaleFactor;
        public alignment anchorPoint = alignment.ALIGNMENT_TOP_LEFT;
        private CenterType _centerType;

        public enum alignment
        {
            ALIGNMENT_TOP_LEFT,
            ALIGNMENT_TOP_CENTER,
            ALIGNMENT_TOP_RIGHT,
            ALIGNMENT_MID_LEFT,
            ALIGNMENT_MID_CENTER,
            ALIGNMENT_MID_RIGHT,
            ALIGNMENT_BOTTOM_LEFT,
            ALIGNMENT_BOTTOM_CENTER,
            ALIGNMENT_BOTTOM_RIGHT,
        }

        public enum CenterType{
            STRETCHED,
            TILED
        }


        //SLICED SPRITE 1.0 by Pat Ferguson

        public SlicedSprite(
            Texture2D _sourceTexture,           //the sprite to be 9-Sliced
            Rectangle _sliceRect,               //the rectangle that defines the vertical and horizontal slices within the original texture bounds
            GraphicsDevice _graphicsDevice,     //the GraphicsDevice from which the other Textures must be made
            float _pixelScaleFactor,            //the pixel scaling of the final render
            CenterType centerType,              //STRETCHED or TILED center slice
            alignment _alignment)               //The anchor point of the sliced sprite from which positioning and resizing will take place
        {
            if (_sourceTexture.Bounds.Width <= _sliceRect.Width || _sourceTexture.Bounds.Height <= _sliceRect.Height)
            {
                Console.WriteLine("ERROR! Slice must be within the source texture height and width!");
                return;
            }

            sourceTex = _sourceTexture;
            rectangle = _sourceTexture.Bounds;
            originalRect = rectangle;
            pixelScaleFactor = _pixelScaleFactor;
            anchorPoint = _alignment;
            _centerType = centerType;
 
            //spriteBatch = new SpriteBatch(_graphicsDevice);

            sourceRectangles[0] = new Rectangle(new Point(0, 0), new Point(_sliceRect.X, _sliceRect.Y));//Top Left
            sourceRectangles[1] = new Rectangle(new Point(_sliceRect.X, 0), new Point(_sliceRect.Width, _sliceRect.Y));//Top Center
            sourceRectangles[2] = new Rectangle(new Point(_sliceRect.X + _sliceRect.Width, 0), new Point(_sourceTexture.Width - (sourceRectangles[0].Width + sourceRectangles[1].Width), _sliceRect.Y));//Top Right
            sourceRectangles[3] = new Rectangle(new Point(0, _sliceRect.Y), new Point(_sliceRect.X, _sliceRect.Height));//Mid Left
            sourceRectangles[4] = new Rectangle(new Point(_sliceRect.X + _sliceRect.Width, _sliceRect.Y), new Point(sourceRectangles[2].Width, _sliceRect.Height));//Mid Right
            sourceRectangles[5] = new Rectangle(new Point(0, _sliceRect.Height + _sliceRect.Y), new Point(_sliceRect.X, _sourceTexture.Height - (_sliceRect.Y + _sliceRect.Height)));//Bottom Left
            sourceRectangles[6] = new Rectangle(new Point(_sliceRect.X, _sliceRect.Height + _sliceRect.Y), new Point(sourceRectangles[1].Width, sourceRectangles[5].Height));//Bottom Center
            sourceRectangles[7] = new Rectangle(new Point(sourceRectangles[2].X, sourceRectangles[6].Y), new Point(sourceRectangles[2].Width, sourceRectangles[6].Height));//BottomRight
            sourceRectangles[8] = new Rectangle(new Point(_sliceRect.X, _sliceRect.Y), new Point(_sliceRect.Width, _sliceRect.Height));//Mid Center

            //targetRectangles = sourceRectangles;
            Color[] data;
            Texture2D tex;

            for (int i = 0; i < sourceRectangles.Length; i++)
            {
                targetRectangles[i].X = sourceRectangles[i].X;
                targetRectangles[i].Y = sourceRectangles[i].Y;
                targetRectangles[i].Width = sourceRectangles[i].Width;
                targetRectangles[i].Height = sourceRectangles[i].Height;

                Rectangle rect = sourceRectangles[i];
                //Console.WriteLine("sourceRectangle[" + i + "].Origin: " + rect.X + ", " + rect.Y + ", Width: " + rect.Width + ", Height: " + rect.Height);


                tex = new Texture2D(_graphicsDevice, sourceRectangles[i].Width, sourceRectangles[i].Height);
                data = new Color[tex.Width * tex.Height];
                _sourceTexture.GetData(0, sourceRectangles[i], data, 0, data.Length);
                tex.SetData(data);
                sourceTextures[i] = tex;
                sourceRectangles[i].X = 0;//our origins will be reset for the new textures
                sourceRectangles[i].Y = 0;//our origins will be reset for the new textures
            }
        }

        public void SetRectangle(Rectangle _rect)
        {
            rectangle.Width = _rect.Width;
            rectangle.Height = _rect.Height;
            UpdateRectangles(new Point((int)(_rect.X / pixelScaleFactor), (int)(_rect.Y / pixelScaleFactor)));
        }

        private void UpdateRectangles(Point Origin)
        {
            targetRectangles[0].X = targetRectangles[3].X = targetRectangles[5].X = Origin.X;
            targetRectangles[1].X = targetRectangles[6].X = targetRectangles[8].X = targetRectangles[0].X + targetRectangles[0].Width;
            targetRectangles[0].Y = targetRectangles[1].Y = targetRectangles[2].Y = Origin.Y;
            targetRectangles[1].Width = targetRectangles[6].Width = targetRectangles[8].Width = rectangle.Width - (targetRectangles[0].Width + targetRectangles[2].Width);
            targetRectangles[2].X = targetRectangles[4].X = targetRectangles[7].X = targetRectangles[1].X + targetRectangles[1].Width;
            targetRectangles[3].Y = targetRectangles[4].Y = targetRectangles[8].Y = Origin.Y + targetRectangles[0].Height;
            targetRectangles[3].Height = targetRectangles[4].Height = targetRectangles[8].Height = rectangle.Height - (targetRectangles[0].Height + targetRectangles[5].Height);
            targetRectangles[5].Y = targetRectangles[6].Y = targetRectangles[7].Y = targetRectangles[3].Y + targetRectangles[3].Height;

            //for (int i = 0; i < targetRectangles.Length; i++){
                //Rectangle rect = targetRectangles[i];
                //Console.WriteLine("targetRectangle[" + i + "].Origin: " + rect.X + ", " + rect.Y + ", Width: " + rect.Width + ", Height: " + rect.Height);
            //}

            UpdateAlignment();
        }

        private void UpdateAlignment()
        {
            int xOffset = 0;
            int yOffset = 0;

            int totalWidth = rectangle.Width;
            int totalHeight = rectangle.Height;

            switch (anchorPoint)
            {
                case alignment.ALIGNMENT_TOP_CENTER:
                    xOffset = -(totalWidth / 2);
                    break;
                case alignment.ALIGNMENT_TOP_RIGHT:
                    xOffset = -totalWidth;
                    break;
                case alignment.ALIGNMENT_MID_LEFT:
                    yOffset = -(totalHeight / 2);
                    break;
                case alignment.ALIGNMENT_MID_CENTER:
                    xOffset = -(totalWidth / 2);
                    yOffset = -(totalHeight / 2);
                    break;
                case alignment.ALIGNMENT_MID_RIGHT:
                    xOffset = -totalWidth;
                    yOffset = -(totalHeight / 2);
                    break;
                case alignment.ALIGNMENT_BOTTOM_LEFT:
                    yOffset = -totalHeight;
                    break;
                case alignment.ALIGNMENT_BOTTOM_CENTER:
                    xOffset = -totalWidth;
                    yOffset = -totalHeight;
                    break;
                case alignment.ALIGNMENT_BOTTOM_RIGHT:
                    xOffset = -totalWidth;
                    yOffset = -totalHeight;
                    break;
            }

            for (int i = 0; i < targetRectangles.Length; i++)
            {
                targetRectangles[i].X += xOffset;
                targetRectangles[i].Y += yOffset;
            }
        }

        public virtual void Draw()
        {
            StaticSpriteBatch.Instance.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone,null,Matrix.CreateScale(pixelScaleFactor));
            for (int i = 0; i < 8; i++){StaticSpriteBatch.Instance.Draw(sourceTextures[i], targetRectangles[i], sourceRectangles[i], Color.White);}
            if (_centerType == CenterType.TILED){
                StaticSpriteBatch.Instance.End();
                StaticSpriteBatch.Instance.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointWrap,DepthStencilState.Default,RasterizerState.CullNone,null,Matrix.CreateScale(pixelScaleFactor));
                StaticSpriteBatch.Instance.Draw(sourceTextures[8], targetRectangles[8], new Rectangle(new Point(sourceRectangles[8].X, sourceRectangles[8].Y), new Point(targetRectangles[8].Width, targetRectangles[8].Height)), Color.White);
            }
            else{
                StaticSpriteBatch.Instance.Draw(sourceTextures[8], targetRectangles[8], sourceRectangles[8], Color.White);
            }
            StaticSpriteBatch.Instance.End();
        }

        public virtual void Dispose()
        {
            sourceTex = null;
            sourceTextures = null;
            //spriteBatch = null;
            targetRectangles = null;
            sourceRectangles = null;

        }
    }
}
