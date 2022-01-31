using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds dependencies for pausing panel
/// </summary>

public class PauseSystem : MyNamespace.System
{
    [Header("Dependencies")]

    [SerializeField]
    [Tooltip("the pause panel")]
    public GameObject pauseMenuUI;

}
