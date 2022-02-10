using UnityEngine;

/// <summary>
/// Controls the jumping system with input.
/// </summary>

public class InputJumpingController : InputController<JumpingSystem>
{
    private Buffer _jumpBuffer = new Buffer();

    private void OnEnable()
    {
        system.onJump.AddListener(ClearBuffer);
    }

    private void OnDisable()
    {
        system.onJump.RemoveListener(ClearBuffer);
        system.UpdateGravity(false);
        _jumpBuffer.Clear();
    }

    private void ClearBuffer()
    {
        _jumpBuffer.Clear();
    }
    
    private void Update()
    {
        bool wantsToJump = system.JumpSettings.HoldAndJump
            ? Input.GetKey(controls.jump)
            : Input.GetKeyDown(controls.jump);
 
        // Instead of jumping whenever we press a button, we queue a jump instead.
        // This will still be responsive when grounded, and won't eat your inputs when falling.
        
        if (wantsToJump)
            _jumpBuffer.Queue();
    }

    private void FixedUpdate()
    {
        if (_jumpBuffer.IsQueued(system.JumpSettings.JumpBufferTime))
            system.TryToJump();
        
        system.UpdateGravity(Input.GetKey(controls.jump));
    }
}