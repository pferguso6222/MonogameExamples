using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Source.PatUtils
{
    public class SaveGameUtility
    {

        public static int maxSaveEntries = 3;
        public List<SaveGameEntry> entries = new List<SaveGameEntry>();
        public SaveGameData data;
        public int currentSaveDataIndex = 0;
        public string saveFile = ".\\Save.dat";
        private IFormatter formatter;
        private Stream stream;
        public SaveGameUtility()
        {

        }

        public bool LoadVars()
        {
            Console.WriteLine(File.Exists(saveFile) ? "File exists." : "File does not exist.");

            if (!File.Exists(saveFile))
            {
                //Save File Gone. Make new one and populate with empty entries.
                for (int i = 0; i < maxSaveEntries; i++)
                {
                    SaveGameEntry saveGameEntry = new SaveGameEntry();
                    saveGameEntry.ID = i;
                    saveGameEntry.Name = "Empty";//TODO: let's figure out how to convert name string into numbers to hide them in the save file.
                    saveGameEntry.LevelsComplete = 0;
                    saveGameEntry.activeItems = new List<string> { "Pat", "John", "Shawn", "Brian" };

                    entries.Add(saveGameEntry);
                }

                data = new SaveGameData();
                data.SaveEntries = entries;
               
                //Serialize the created empty entries and save them to disk.
                formatter = new BinaryFormatter();
                stream = new FileStream(".\\Save.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, data);
                stream.Close();
            }

            if (File.Exists(saveFile))
            {
                //Load Entries
                formatter = new BinaryFormatter();
                stream = new FileStream(".\\Save.dat", FileMode.Open, FileAccess.Read);
                data = (SaveGameData)formatter.Deserialize(stream);
                entries = data.SaveEntries;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

