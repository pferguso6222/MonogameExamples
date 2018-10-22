using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]

public class SaveGameEntry
{
    public int ID;
    public string Name;
    public int LevelsComplete;
    public List<string> activeItems;
}

[Serializable]

public class SaveGameData
{
    public List<SaveGameEntry> SaveEntries;
}
