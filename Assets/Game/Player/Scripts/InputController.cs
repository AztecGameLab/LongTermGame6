using UnityEngine;

/// <summary>
/// Generalizes classes that take user input, and apply it to an object.
/// </summary>
/// <typeparam name="T">What object are we applying our input to?</typeparam>

// todo: most subclasses are only tested with keyboard + mouse (but do we want to support controller?)

public abstract class InputController<T> : Controller<T> where T : Object
{
    [SerializeField]
    [Tooltip("The control scheme used to drive this controller.")]
    public Controls controls;
}