using System.IO;
using UnityEngine;

namespace Game
{
    public static class DataPathSetup
    {
        [RuntimeInitializeOnLoadMethod]
        public static void SetupDataStorage()
        {
            string directoryPath = Application.persistentDataPath + "/Saves";

            if (Directory.Exists(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);
        }
    }
}