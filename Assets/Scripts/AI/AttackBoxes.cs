using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoxes : MonoBehaviour
{

    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AttackBox(Trigger damagedTrigger)
    {
        foreach (Rigidbody occupant in damagedTrigger.Rigidbodies)
        {
            if(occupant.TryGetComponent(out Health health))
            {
                Debug.Log("Attack BOX");
                animator.SetTrigger("Attack");
                break;
            }
        }
    }
}
