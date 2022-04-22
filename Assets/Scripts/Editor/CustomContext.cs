using Game.Interaction;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using Utility;

namespace Game.Editor
{
    public class CustomContext
    {
        [MenuItem("GameObject/Add Hint")]
        private static void AddHint(MenuCommand menuCommand)
        {
            if (menuCommand.context is GameObject target)
            {
                Undo.IncrementCurrentGroup();
                Undo.SetCurrentGroupName("Add Hint to GameObject");
                
                var lookTrigger = Undo.AddComponent<LookTrigger>(target);
                
                var hintObject = Resources.Load<Hint>("Hint");
                Object hintInstance = PrefabUtility.InstantiatePrefab(hintObject, target.transform);
                
                if (hintInstance is Hint hint)
                {
                    var canvas = hint.GetComponentInChildren<Canvas>();
                    canvas.worldCamera = Camera.main;
                    
                    Undo.RegisterCreatedObjectUndo(hint.gameObject, "");
                    UnityEventTools.AddVoidPersistentListener(lookTrigger.onLookStart, hint.ShowHint);
                    UnityEventTools.AddVoidPersistentListener(lookTrigger.onLookEnd, hint.HideHint);
                    
                    Selection.activeObject = hint.gameObject;
                }
                
                Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            }
        }
    }
}