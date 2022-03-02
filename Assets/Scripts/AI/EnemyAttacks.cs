using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The multiplier that pushes the player backwards")]
    private float pushMultiplier = 5;

    [SerializeField]
    [Tooltip("The sound to be played when the player takes damage")]
    private AudioSource damageClip;
    public void DoDamage(float attackDamage, Trigger damageTrigger)
    {
        foreach (Rigidbody occupant in damageTrigger.Occupants)
        {
            if (occupant.TryGetComponent(out Health health))
            {
                health.Damage(attackDamage);
                damageClip.Play();
            }
            var targetDirection = occupant.transform.position - this.transform.position;
            occupant.AddForce(targetDirection * pushMultiplier, ForceMode.Impulse);
            //occupant.drag += 5;
            //occupant.drag = 1;
        }
    }
}
