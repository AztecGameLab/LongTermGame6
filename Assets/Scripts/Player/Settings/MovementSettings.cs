using UnityEngine;

[CreateAssetMenu]
public class MovementSettings : ScriptableObject
{
    [Header("Standard Movement Settings")]
    [SerializeField] public float baseMovementSpeed;
    [SerializeField] public float groundedAcceleration = 0.125f;
    [SerializeField] public float groundedDeceleration = 0.2f;
    [SerializeField] public float airborneAcceleration = 0.6f;
    [SerializeField] public float airborneDeceleration = 0.6f;
}