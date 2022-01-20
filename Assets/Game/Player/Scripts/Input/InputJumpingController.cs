using UnityEngine;

public class InputJumpingController : InputController<JumpingSystem>
{
    private Buffer _jumpBuffer = new Buffer();

    private void OnEnable()  { system.onJump.AddListener(ClearBuffer);    }
    private void OnDisable() { system.onJump.RemoveListener(ClearBuffer); }

    private void ClearBuffer()
    {
        _jumpBuffer.Clear();
    }
    
    private void Update()
    {
        if (IsRunning == false)
            return;
        
        bool wantsToJump = system.JumpSettings.HoldAndJump
            ? Input.GetKey(controls.jump)
            : Input.GetKeyDown(controls.jump);
        
        if (wantsToJump)
            _jumpBuffer.Queue();
    }

    private void FixedUpdate()
    {
        if (IsRunning == false)
            return;
        
        if (_jumpBuffer.IsQueued(system.JumpSettings.JumpBufferTime))
            system.TryToJump();
        
        system.UpdateGravity(Input.GetKey(controls.jump));
    }
}