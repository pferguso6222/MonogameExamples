using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveGameTest.Desktop
{

    [Serializable]

    public class Tutorial
    {
        public int ID;
        public string Name;
        public int LevelsComplete;

    }
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DECLARE AN INSTANCE OF THE CLASS
            Tutorial obj = new Tutorial();
            obj.ID = 1;
            obj.Name = "Pat";
            obj.LevelsComplete = 10;

            //SEE IF FILE EXISTS
            string curFile = ".\\Save.dat";
            Console.WriteLine(File.Exists(curFile) ? "File exists." : "File does not exist.");

            //WRITE THE DATA TO NEW FILE
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(".\\Save.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, obj);
            stream.Close();

            //SEE IF FILE EXISTS AGAIN
            Console.WriteLine(File.Exists(curFile) ? "File exists.\n" : "File does not exist.\n");

            //OPEN THE FILE AND READ THE DATA
            stream = new FileStream(".\\Save.dat", FileMode.Open, FileAccess.Read);
            Tutorial objNew = (Tutorial)formatter.Deserialize(stream);
            Console.WriteLine("objNew.ID: " + objNew.ID);
            Console.WriteLine("objNew.Name: " + objNew.Name);
            Console.WriteLine("objNew.LevelsComplete: " + objNew.LevelsComplete);

            using (var game = new Game1())
                game.Run();
        }
    }
}
