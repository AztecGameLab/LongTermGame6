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


    [SerializeField]
    [Tooltip("Duration for slow movement")]
    private float slowDuration = 10f;
    private float duration;

    [Space(10)]
    [SerializeField] private Animator animator;
    private WaitForSeconds animationWait;

    public void Awake()
    {
        animationWait = new WaitForSeconds(1f);
    }
    public void DoDamage(float attackDamage, Trigger damageTrigger)
    {
        duration  = slowDuration + Time.deltaTime;
        foreach (Rigidbody occupant in damageTrigger.Rigidbodies)
        {
            //animator.SetTrigger("Attack");
            if (occupant.TryGetComponent(out Health health))
            {
                animator.SetTrigger("Attack");
                StartCoroutine(DelayForAnimation(health, occupant, attackDamage));
                //health.Damage(attackDamage);
                //damageClip.Play();
            }
            
            //var targetDirection = occupant.transform.position - this.transform.position;
            //occupant.AddForce(targetDirection * pushMultiplier, ForceMode.Impulse);
             
            if(occupant.GetComponent<MovementSystem>()){
                occupant.GetComponent<MovementSystem>().SpeedMultiplier = 0.5f;     
                StartCoroutine(RestoreMovement(occupant.GetComponent<MovementSystem>()));
            }
        }

        IEnumerator RestoreMovement (MovementSystem system)
        {
            yield return new WaitForSeconds(slowDuration);
            system.SpeedMultiplier = 2.0f;
        }

    }
    public IEnumerator DelayForAnimation(Health health, Rigidbody occupant, float attackDamage)
    {
        yield return animationWait;
        health.Damage(attackDamage);
        damageClip.Play();
        var targetDirection = occupant.transform.position - this.transform.position;
        occupant.AddForce(targetDirection * pushMultiplier, ForceMode.Impulse);


    }

}
