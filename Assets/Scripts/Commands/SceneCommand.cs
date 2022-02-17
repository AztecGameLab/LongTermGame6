using System.Collections.Generic;
using System.IO;
using System.Text;
using ConsoleUtility;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Commands
{
    [AutoRegisterConsoleCommand, UsedImplicitly]
    public class SceneCommand : IConsoleCommand
    {
        public string name => "scene";
        public string summary => "Interface with the Unity scene system.";
        public string help => @"
    * scene: List available scenes.
    * scene load [sceneName]: Load a scene by name.";

        public IEnumerable<Console.Alias> aliases
        {
            get
            {
                yield return Console.Alias.Get("load", "scene load");
                yield return Console.Alias.Get("reload", "scene reload");
            }
        }
        
        public void Execute(string[] args)
        {
            if (args.Length > 1 && args[0] == "load")
                HandleLoad(args[1]);

            if (args.Length > 0 && args[0] == "reload")
                HandleLoad(SceneManager.GetActiveScene().name);

            else HandleList();
        }

        private void HandleList()
        {
            int availableScenes = SceneManager.sceneCountInBuildSettings;
            string header = availableScenes > 0 ? "Available Scenes:" : "No scenes available";
            var message = new StringBuilder(header);
            
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string sceneName = BuildIndexToSceneName(i);
                message.Append($"\n- {sceneName}");
            }

            Console.Log(name, message.ToString());
        }

        private void HandleLoad(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scene = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

                if (scene == sceneName)
                {
                    SceneTransitionSystem.Instance.TransitionToScene(sceneName);
                    Console.Log(name, $"Loaded {sceneName}.");
                    return;
                }
            }
            
            Console.Log(name, $"There is no scene named {sceneName}.", LogType.Error);
        }

        private static string BuildIndexToSceneName(int buildIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            
            return sceneName;
        }
    }
}