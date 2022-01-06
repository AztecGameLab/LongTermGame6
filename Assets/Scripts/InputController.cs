using UnityEngine;

public abstract class InputController<T> : Controller<T> where T : MyNamespace.System
{
    [SerializeField] public Controls controls;
}