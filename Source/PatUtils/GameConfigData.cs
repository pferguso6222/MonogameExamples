using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]

public class GameConfigData
{
    public int screenWidth;
    public int screenHeight;
    public bool isFullScreen;
    public int SamplerStateIndex;
}