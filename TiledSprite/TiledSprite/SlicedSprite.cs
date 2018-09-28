using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TiledSprite
{
    class SlicedSprite
    {

        private Texture2D sourceTex;
        private RenderTarget2D[] sourceTextures = new RenderTarget2D[9];
        private Rectangle[] sourceRectangles = new Rectangle[9];
        private SpriteBatch spriteBatch;
        private Rectangle[] targetRectangles = new Rectangle[9];
        private Rectangle rectangle;
        private float pixelScaleFactor;
        private alignment anchorPoint = alignment.ALIGNMENT_TOP_LEFT;
        //private int OriginX;
        //private int OriginY;

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

        //SLICED SPRITE 1.0 by Pat Ferguson

        public SlicedSprite(
            Texture2D _sourceTexture,           //the sprite to be 9-Sliced
            Rectangle _sliceRect,               //the rectangle that defines the vertical and horizontal slices within the original texture bounds
            GraphicsDevice _graphicsDevice,     //the GraphicsDevice from which a spriteBatch can be made to construct the sliced sprite
            float _pixelScaleFactor,            //the pixel scaling of the final render
            alignment _alignment)               //The anchor point of the sliced sprite from which positioning and resizing will take place
        {
            if (_sourceTexture.Bounds.Width <= _sliceRect.Width || _sourceTexture.Bounds.Height <= _sliceRect.Height)
            {
                Console.WriteLine("ERROR! Slice must be within the source texture height and width!");
                return;
            }
            sourceTex = _sourceTexture;
            rectangle = sourceTex.Bounds;
            pixelScaleFactor = _pixelScaleFactor;
            anchorPoint = _alignment;
 
            spriteBatch = new SpriteBatch(_graphicsDevice);

            sourceRectangles[0] = new Rectangle(new Point(0, 0), new Point(_sliceRect.X, _sliceRect.Y));//Top Left
            sourceRectangles[1] = new Rectangle(new Point(_sliceRect.X, 0), new Point(_sliceRect.Width, _sliceRect.Y));//Top Center
            sourceRectangles[2] = new Rectangle(new Point(_sliceRect.X + _sliceRect.Width, 0), new Point(sourceTex.Width - (sourceRectangles[0].Width + sourceRectangles[1].Width), _sliceRect.Y));//Top Right
            sourceRectangles[3] = new Rectangle(new Point(0, _sliceRect.Y), new Point(_sliceRect.X, _sliceRect.Height));//Mid Left
            sourceRectangles[4] = new Rectangle(new Point(_sliceRect.X, _sliceRect.Y), new Point(_sliceRect.Width, _sliceRect.Height));//Mid Center
            sourceRectangles[5] = new Rectangle(new Point(_sliceRect.X + _sliceRect.Width, _sliceRect.Y), new Point(sourceTex.Width - (sourceRectangles[0].Width + sourceRectangles[1].Width), _sliceRect.Height));//Mid Right
            sourceRectangles[6] = new Rectangle(new Point(0, _sliceRect.Height + _sliceRect.Y), new Point(_sliceRect.X, sourceTex.Height - (_sliceRect.Y + _sliceRect.Height)));//Bottom Left
            sourceRectangles[7] = new Rectangle(new Point(_sliceRect.X, _sliceRect.Height + _sliceRect.Y), new Point(_sliceRect.Width, sourceTex.Height - (_sliceRect.Y + _sliceRect.Height)));//Bottom Center
            sourceRectangles[8] = new Rectangle(new Point(_sliceRect.X + _sliceRect.Width, _sliceRect.Height + _sliceRect.Y), new Point(sourceTex.Width - (sourceRectangles[0].Width + sourceRectangles[1].Width), sourceTex.Height - (_sliceRect.Y + _sliceRect.Height)));//BottomRight

            //targetRectangles = sourceRectangles;

            for (int i = 0; i < sourceRectangles.Length; i++)
            {
                targetRectangles[i].X = sourceRectangles[i].X;
                targetRectangles[i].Y = sourceRectangles[i].Y;
                targetRectangles[i].Width = sourceRectangles[i].Width;
                targetRectangles[i].Height = sourceRectangles[i].Height;

                Rectangle rect = targetRectangles[i];
                Console.WriteLine("sourceRectangle[" + i + "].Origin: " + rect.X + ", " + rect.Y + ", Width: " + rect.Width + ", Height: " + rect.Height);

            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullNone);

            _graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            for (int i = 0; i < 9; i++)
            {
                sourceTextures[i] = new RenderTarget2D(
                _graphicsDevice,
                sourceRectangles[i].Width,
                sourceRectangles[i].Height,
                false,
                _graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

                Rectangle rect = sourceRectangles[i];
                Console.WriteLine("sourceRectangle["+ i + "].Origin: " + rect.X + ", " + rect.Y + ", Width: "  + rect.Width + ", Height: " + rect.Height);

                _graphicsDevice.SetRenderTarget(sourceTextures[i]);

                spriteBatch.Draw(_sourceTexture, rect, Color.White);

                _graphicsDevice.SetRenderTarget(null);
            }
            spriteBatch.End();
        }

        public void SetRectangle(Rectangle _rect)
        {

            if (_rect.Width >= sourceTex.Bounds.Width)
            {
                rectangle.Width = _rect.Width;
            }

            if (_rect.Height >= sourceTex.Bounds.Height)
            {
                rectangle.Height = _rect.Height;
            }

            UpdateRectangles(new Point((int)(_rect.X / pixelScaleFactor), (int)(_rect.Y / pixelScaleFactor)));
        }

        private void UpdateRectangles(Point Origin)
        {
            
            targetRectangles[0].X = targetRectangles[3].X = targetRectangles[6].X = Origin.X;
            targetRectangles[1].X = targetRectangles[4].X = targetRectangles[7].X = targetRectangles[0].X + targetRectangles[0].Width;
            targetRectangles[0].Y = targetRectangles[1].Y = targetRectangles[2].Y = Origin.Y;
            targetRectangles[1].Width = targetRectangles[4].Width = targetRectangles[7].Width = rectangle.Width - (targetRectangles[0].Width + targetRectangles[2].Width);
            targetRectangles[2].X = targetRectangles[5].X = targetRectangles[8].X = targetRectangles[1].X + targetRectangles[1].Width;
            targetRectangles[3].Y = targetRectangles[4].Y = targetRectangles[5].Y = Origin.Y + targetRectangles[0].Height;
            targetRectangles[3].Height = targetRectangles[4].Height = targetRectangles[5].Height = rectangle.Height - (targetRectangles[0].Height + targetRectangles[6].Height);
            targetRectangles[4].X = targetRectangles[3].X + targetRectangles[3].Width;
            targetRectangles[6].Y = targetRectangles[7].Y = targetRectangles[8].Y = targetRectangles[3].Y + targetRectangles[3].Height;

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
                case alignment.ALIGNMENT_TOP_LEFT:
                default:
                    //Do nothing
                    break;

            }

            for (int i = 0; i < targetRectangles.Length; i++)
            {
                targetRectangles[i].X += xOffset;
                targetRectangles[i].Y += yOffset;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullNone, null, Matrix.CreateScale(pixelScaleFactor));

            for (int i = 0; i < 9; i++)
            {
                _spriteBatch.Draw(sourceTex, targetRectangles[i], sourceRectangles[i], Color.White);
            }

            _spriteBatch.End();
            
        }

    }
}
