using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    private Vector3 previousPosition;
    public float curSpeed;
    NavMeshAgent agent;
    private Animator animator;
    public bool die;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", currentSpeed());
        Die();
    }


    private float currentSpeed()
    {
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
        return curSpeed;
    }


    private void Die()
    {
        if (die)
        {
            agent.SetDestination(transform.position);
            animator.Play("Dead");
            gameObject.layer = LayerMask.NameToLayer("DeadBody");
        }
    }
}
