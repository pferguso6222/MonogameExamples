using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Source.PatUtils
{
    public class GameConfigUtility
    {
        public static GameConfigUtility Instance;

        public string GameName = "";
        protected string SaveFolderPath{
            get; private set;
        }
        public GameConfigData data;
        public string saveFile = "";
        private IFormatter formatter;
        private Stream stream;

        public GameConfigUtility(string gameName)
        {
            //find out where our directory for write permissions are. This is where our options and save game files will go.
            GameName = gameName;
            saveFile = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/", GameName, "/GameConfig.dat");
            //Console.WriteLine("SaveFolderFath:" + saveFile);
            formatter = new BinaryFormatter();
            Instance = this;
        }

        public bool LoadVars()
        {
            //Console.WriteLine(File.Exists(saveFile) ? "File exists: " + saveFile : "File does not exist: " + saveFile);

            if (!File.Exists(saveFile))
            {
                GameConfigData gameConfigData = new GameConfigData();
                gameConfigData.screenWidth = 1920;
                gameConfigData.screenHeight = 1080;
                gameConfigData.SamplerStateIndex = 0;
                gameConfigData.isFullScreen = false;

                //Serialize the created empty entries and save them to disk.
                System.IO.Directory.CreateDirectory(String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/", GameName));

                stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, gameConfigData);
                stream.Close();
            }

            if (File.Exists(saveFile))
            {
                //Load Entries
                formatter = new BinaryFormatter();
                stream = new FileStream(saveFile, FileMode.Open, FileAccess.Read);
                data = (GameConfigData)formatter.Deserialize(stream);
                stream.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Save(){
            stream = new FileStream(saveFile, FileMode.Truncate, FileAccess.Write);
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }
}

