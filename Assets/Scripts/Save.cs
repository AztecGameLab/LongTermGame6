using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [Serializable]
    public class Save
    {
        public string CurrentScene { get; set; }
        public string SaveName { get; private set; }
        
        public Save(string saveSaveName)
        {
            CurrentScene = SceneManager.GetActiveScene().name;
            SaveName = saveSaveName;
            
            if (IsValid())
                Debug.LogWarning($"There already exists a save \"{SaveName}\"! Be careful writing.");
            
            Debug.Log($"Created new save \"{saveSaveName}\".");
        }

        #region Instance Methods

        public void Write()
        {
            Debug.Log($"Updated save {SaveName}.");
            
            // Creates a new file for the save.
            string savePath = GetPath();
            using FileStream saveFile = File.Create(savePath);
            
            // Writes our data to the file.
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(saveFile, this);
        }
        
        public void Delete()
        {
            if (IsValid())
            {
                Debug.Log($"Deleted save {SaveName}.");
                
                string savePath = GetPath();
                File.Delete(savePath);
            }
        }
        
        public bool IsValid()
        {
            return IsValid(SaveName);
        }

        public string GetPath()
        {
            return GetPath(SaveName);
        }

        #endregion
        
        #region Static Methods

        public static Save Read(string saveName)
        {
            if (IsValid(saveName) == false)
                throw new FileNotFoundException($"Tried to open the save \"{saveName}\" that doesn't exist!");
        
            Debug.Log($"Accessed the save \"{saveName}\".");
            
            // Opens the save file.
            string savePath = GetPath(saveName);
            using FileStream saveFile = File.OpenRead(savePath);
            
            // Parses the data from our file.
            var binaryFormatter = new BinaryFormatter();
            var saveData = (Save) binaryFormatter.Deserialize(saveFile);
            return saveData;
        }

        public static bool IsValid(string saveName)
        {
            string savePath = GetPath(saveName);
            return File.Exists(savePath);
        }

        public static string GetPath(string fileName)
        {
            return Application.persistentDataPath + $"/Saves/{fileName}.dat";
        }

        #endregion
    }    
}