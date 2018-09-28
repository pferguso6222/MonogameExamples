using System;

public class AnimatedDialogBox
{
	public AnimatedDialogBox()
	{

	}

    private Rectangle[] CreatePatches(Rectangle rectangle)
    {
        var x = rectangle.X;
        var y = rectangle.Y;
        var w = rectangle.Width;
        var h = rectangle.Height;
        var middleWidth = w - LeftPadding - RightPadding;
        var middleHeight = h - TopPadding - BottomPadding;
        var bottomY = y + h - BottomPadding;
        var rightX = x + w - RightPadding;
        var leftX = x + LeftPadding;
        var topY = y + TopPadding;
        var patches = new[]
        {
        new Rectangle(x,      y,        LeftPadding,  TopPadding),      // top left
        new Rectangle(leftX,  y,        middleWidth,  TopPadding),      // top middle
        new Rectangle(rightX, y,        RightPadding, TopPadding),      // top right
        new Rectangle(x,      topY,     LeftPadding,  middleHeight),    // left middle
        new Rectangle(leftX,  topY,     middleWidth,  middleHeight),    // middle
        new Rectangle(rightX, topY,     RightPadding, middleHeight),    // right middle
        new Rectangle(x,      bottomY,  LeftPadding,  BottomPadding),   // bottom left
        new Rectangle(leftX,  bottomY,  middleWidth,  BottomPadding),   // bottom middle
        new Rectangle(rightX, bottomY,  RightPadding, BottomPadding)    // bottom right
    };
        return patches;
    }
}
