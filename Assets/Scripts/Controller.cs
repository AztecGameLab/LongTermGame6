using UnityEngine;

/// <summary>
/// Generalizes behaviours that control an object.
/// </summary>
/// <typeparam name="T">The object we want to control.</typeparam>

public abstract class Controller<T> : MonoBehaviour where T : Object
{
    [SerializeField] 
    [Tooltip("The system this object controls.")]
    protected T system;
}