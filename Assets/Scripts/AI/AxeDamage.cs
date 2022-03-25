using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The multiplier that pushes the player backwards")]
    private float pushMultiplier = 5;

    [SerializeField]
    [Tooltip("The sound to be played when the player takes damage")]
    private AudioSource damageClip;

    [SerializeField]
    [Tooltip("Duration for slow movement")]
    private float slowDuration = 10f;
    private float duration;   
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("What percent of the player's health should we take away when hitting them.")]
    private float attackDamage = 0.25f;
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody == null) return;
        Rigidbody occupant = collision.rigidbody;
        if (occupant.TryGetComponent(out Health health))
            {
                health.Damage(attackDamage);
                damageClip.Play();
            }
        var targetDirection = occupant.transform.position - this.transform.position;
        occupant.AddForce(targetDirection * pushMultiplier, ForceMode.Impulse);
             
        if(occupant.GetComponent<MovementSystem>())
            {
                occupant.GetComponent<MovementSystem>().SpeedMultiplier = 0.5f;     
                StartCoroutine(RestoreMovement(occupant.GetComponent<MovementSystem>()));
            }
        IEnumerator RestoreMovement (MovementSystem system)
        {
            yield return new WaitForSeconds(slowDuration);
            system.SpeedMultiplier = 2.0f;
        }
    }
}