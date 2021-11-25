using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu]
public class JumpSettings : ScriptableObject
{
    [Header("Standard Jump Settings")] 
    [SerializeField] public bool holdAndJump;
    [SerializeField] public float jumpDistance = 4.5f;
    [SerializeField] public float jumpHeight = 1.1f;
    [SerializeField] public float assumedInitialSpeed = 5f;
    [SerializeField] [Range(0.1f, 1)] public float standardSkew = 0.5f;
    
    [Header("Fast Fall Settings")] 
    [SerializeField] public bool enableFastFall;
    [SerializeField] public float minDistance = 2.5f;
    [SerializeField] public float minHeight = 0.55f;
    [SerializeField] [Range(0.1f, 1)] public float fastFallSkew = 0.5f;

    [Header("Other Settings")] 
    [SerializeField] public int airJumps;
    [SerializeField] public float coyoteTime = 0.15f;
    [SerializeField] public float jumpBufferTime = 0.15f;
    
    public float JumpSpeed { get; private set; }

    public float StandardGravityRising  {get; private set;} 
    public float StandardGravityFalling {get; private set;}
    public float FastFallGravityRising  {get; private set;}
    public float FastFallGravityFalling {get; private set;}

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
}