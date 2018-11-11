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
        public static SaveGameUtility Instance;

        private string GameName = "";
        public string SaveFolderPath;
        public static int maxSaveEntries = 3;
        public List<SaveGameEntry> SaveGameEntries = new List<SaveGameEntry>();
        public SaveGameData data;
        string saveFile = "";
        public int currentSaveDataIndex = 0;
        private IFormatter formatter;
        private Stream stream;


        public SaveGameUtility(string gameName)
        {
            //find out where our directory for write permissions are. This is where our options and save game files will go.
            // SaveFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            GameName = gameName;
            saveFile = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/", GameName, "/Save.dat");
            Console.WriteLine("SaveFolderFath:" + SaveFolderPath);
            formatter = new BinaryFormatter();
            Instance = this;
        }

        public bool LoadVars()
        {
            Console.WriteLine(File.Exists(saveFile) ? "File exists." : "File does not exist.");

            List<string> sampleNames = new List<string> { "EMPTY", "EMPTY", "EMPTY" };

            if (!File.Exists(saveFile))
            {
                //Save File Gone. Make new one and populate with empty entries.
                for (int i = 0; i < maxSaveEntries; i++)
                {
                    SaveGameEntry saveGameEntry = new SaveGameEntry();
                    saveGameEntry.ID = i;
                    saveGameEntry.Name = sampleNames[i];//TODO: let's figure out how to convert name string into numbers to hide them in the save file.
                    saveGameEntry.LevelsComplete = 0;

                    SaveGameEntries.Add(saveGameEntry);
                }

                data = new SaveGameData();
                data.SaveEntries = SaveGameEntries;

                //Serialize the created empty entries and save them to disk.
                System.IO.Directory.CreateDirectory(String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/", GameName));

                stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, data);
                stream.Close();
            }

            if (File.Exists(saveFile))
            {
                //Load Entries
                stream = new FileStream(saveFile, FileMode.Open, FileAccess.Read);
                data = (SaveGameData)formatter.Deserialize(stream);
                SaveGameEntries = data.SaveEntries;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Save()
        {
            stream = new FileStream(saveFile, FileMode.Truncate, FileAccess.Write);
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }
}

