using UnityEngine;
using UnityEngine.Events;

// todo: this is non-functional right now. Hard part of implementation is finding speed right before collision.
// todo v2: ensure that screen-shake from damage depends on the amount of damage, so really small amounts arent jarring

// the reason this is a pain is because the onCollision event happens after speed has been zeroed out. So we need
// to cache our speed before it gets zeroed? seems like its very prone to glitching, needs more thought...

public class FallDamageApplier : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private AnimationCurve damageCurve;

    [Header("Dependencies")] 
    [SerializeField] private GroundedCheck groundCheck;
    [SerializeField] private Rigidbody targetRigidbody;

    [Header("Events")] 
    [SerializeField] public UnityEvent<float> onTakeFallDamage;
    
    private void Awake()
    {
        groundCheck.CollisionEvents.onEnterCollision.AddListener(ApplyFallDamage);
    }

    private void ApplyFallDamage(Collider other)
    {
        float currentSpeed = -targetRigidbody.velocity.y;
        float fallDamage = damageCurve.Evaluate(currentSpeed);

        Debug.Log($"cur speed: {currentSpeed} dmg: {fallDamage}");
        
        onTakeFallDamage.Invoke(fallDamage);
    }
}