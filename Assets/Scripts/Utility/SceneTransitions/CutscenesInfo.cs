using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CutscenesInfo
{
    [SerializeField] private List<string> dialogs;
    [SerializeField] private RectTransform imageRectTransform;
    [SerializeField] private bool isFirstScene;
    [SerializeField] private float duration;

    public float Duration => duration;
    
    public List<string> DialogData
    {
        get => dialogs;
        private set => dialogs = value;
    }

    public RectTransform ImageRectTransform
    {
        get => imageRectTransform;
        private set => imageRectTransform = value;
    }

    public bool IsFirstScene
    {
        get => isFirstScene;
        private set => isFirstScene = value;
    }

}
