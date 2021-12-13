using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu]
public class JumpSettings : ScriptableObject
{
    [Header("Standard Jump Settings")] 
    [SerializeField] private bool holdAndJump;
    [SerializeField] private float jumpDistance = 4.5f;
    [SerializeField] private float jumpHeight = 1.1f;
    [SerializeField] private float assumedInitialSpeed = 5f;
    
    [SerializeField, Range(0.1f, 1)] 
    private float standardSkew = 0.5f;
    
    [Header("Fast Fall Settings")] 
    [SerializeField] private bool enableFastFall;
    [SerializeField] private float minDistance = 2.5f;
    [SerializeField] private float minHeight = 0.55f;
    
    [SerializeField, Range(0.1f, 1)] 
    private float fastFallSkew = 0.5f;

    [Header("Other Settings")] 
    [SerializeField] private int airJumps;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.15f;
    
    public float JumpSpeed { get; private set; }

    public float StandardGravityRising  {get; private set;} 
    public float StandardGravityFalling {get; private set;}
    public float FastFallGravityRising  {get; private set;}
    public float FastFallGravityFalling {get; private set;}

    public bool EnableFastFall => enableFastFall;
    public bool HoldAndJump => holdAndJump;
    public float JumpBufferTime => jumpBufferTime;
    public float CoyoteTime => coyoteTime;
    public int AirJumps => airJumps;

    private void OnEnable()   { CalculateGravityAndSpeed(); }
    private void OnValidate() { CalculateGravityAndSpeed(); }

    [PublicAPI]
    public void CalculateGravityAndSpeed()
    {
        // Gravity math based on the GDC talk found here:
        // https://youtu.be/hG9SzQxaCm8?t=794
    
        JumpSpeed = 2 * jumpHeight * assumedInitialSpeed / (jumpDistance * standardSkew);
    
        StandardGravityRising = 2 * jumpHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                / Mathf.Pow(jumpDistance * standardSkew, 2);
    
        StandardGravityFalling = 2 * jumpHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                 / Mathf.Pow(jumpDistance * (1 - standardSkew), 2);
    
        FastFallGravityRising = 2 * minHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                / Mathf.Pow(minDistance * fastFallSkew, 2);
    
        FastFallGravityFalling = 2 * minHeight * Mathf.Pow(assumedInitialSpeed, 2) 
                                 / Mathf.Pow(minDistance * (1 - fastFallSkew), 2);
    }

    [PublicAPI]
    public float GetCurrentGravity(bool rising, bool holdingJump)
    {
        if (EnableFastFall && holdingJump == false)
            return rising ? FastFallGravityRising : FastFallGravityFalling;

        return rising ? StandardGravityRising : StandardGravityFalling;
    }
}