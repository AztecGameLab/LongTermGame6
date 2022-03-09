using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Game
{
    [Serializable]
    public class Save
    {
        public string SaveName { get; private set; }
        public string CurrentScene { get; set; }

        private Dictionary<string, Dictionary<string, object>> _sceneData = 
            new Dictionary<string, Dictionary<string, object>>();

        public Save(string saveSaveName)
        {
            SaveName = saveSaveName;
            
            if (IsValid())
                Debug.LogWarning($"There already exists a save \"{SaveName}\"! Be careful writing.");
            
            Debug.Log($"Created new save \"{saveSaveName}\".");
        }

        public void UpdateData()
        {
            CurrentScene = SceneManager.GetActiveScene().name;

            if (_sceneData.ContainsKey(CurrentScene) == false)
                _sceneData.Add(CurrentScene, new Dictionary<string, object>());
            
            foreach (var saveData in SaveData.Instances)
            {
                string saveID = saveData.GetID();
                
                if (_sceneData[CurrentScene].ContainsKey(saveID) == false)
                    _sceneData[CurrentScene].Add(saveID, null);

                _sceneData[CurrentScene][saveID] = saveData.WriteData();
            }

            Debug.Log($"Updated save {SaveName}.");
        }

        public void ApplyData()
        {
            // todo: better system needed than callbacks for loading
            SceneTransitionSystem.Instance.TransitionToScene(CurrentScene, () =>
            {
                foreach (SaveData instance in SaveData.Instances)
                {
                    object saveData = _sceneData[CurrentScene][instance.GetID()];
                    instance.ReadData(saveData);
                }
            });
        }
        
        #region Instance Methods

        public void Write()
        {
            UpdateData();
            
            // Creates a new file for the save.
            string savePath = GetPath();
            using FileStream saveFile = File.Create(savePath);
            
            // Writes our data to the file.
            var surrogateSelector = new SurrogateSelector();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSerializationSurrogate());

            var binaryFormatter = new BinaryFormatter { SurrogateSelector = surrogateSelector };
            binaryFormatter.Serialize(saveFile, this);
            
            Debug.Log("Wrote save to disk.");
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
            var surrogateSelector = new SurrogateSelector();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSerializationSurrogate());

            var binaryFormatter = new BinaryFormatter { SurrogateSelector = surrogateSelector };
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