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
    public Int64 Name;
    public int LevelsComplete;
    public List<string> activeItems;
}

public class SaveGameData
{
    public List<SaveGameEntry> SaveEntries;
}
