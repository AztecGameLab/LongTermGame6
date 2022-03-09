using System.Collections.Generic;
using ConsoleUtility;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Commands
{
    [AutoRegisterConsoleCommand]
    public class SaveCommand : IConsoleCommand
    {
        public string name => "save";
        public string summary => "Interface with the save system.";
        public string help => @"
    * save create [saveName] : Create a new save named [saveName].
    * save load [saveName] : Load a save named [saveName]";

        public IEnumerable<Console.Alias> aliases { get; }
        
        public void Execute(string[] args)
        {
            if (args.Length > 1 && args[0] == "create")
                HandleCreate(args[1]);

            if (args.Length > 1 && args[0] == "load")
                HandleLoad(args[1]);
            
            if (args.Length > 0 && args[0] == "location")
                HandleLocation();
        }

        private void HandleLocation()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(Application.persistentDataPath + "/Saves/");
            #endif
        }
        
        private static void HandleCreate(string saveName)
        {
            var targetSave = new Save(saveName);
            targetSave.UpdateData();
            targetSave.Write();
        }

        private static void HandleLoad(string saveName)
        {
            var targetSave = Save.Read(saveName);
            targetSave.ApplyData();
        }
    }
}