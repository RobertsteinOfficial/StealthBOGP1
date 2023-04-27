using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleStatemachine : MonoBehaviour
{
    public State actualState = State.Idle;
    NavMeshAgent agent;
    public Transform[] targets;

    int actualTarget = 0;
    Transform myTarget;
    Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (actualState == State.Dead) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            actualState = State.Patrol;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            actualState = State.Chase;
        }


        if (actualState == State.Patrol)
        {
            Debug.Log("Patrolling");
            agent.SetDestination(targets[actualTarget].position);
            myTarget = targets[actualTarget];

            if (Vector3.Distance(agent.destination, transform.position) < 2)
            {
                actualTarget++;
                if (actualTarget >= targets.Length)
                {
                    actualTarget = 0;
                }
            }

        }
        else if (actualState == State.Chase)
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        }


        if (agent.remainingDistance < agent.stoppingDistance)
        {
            anim.SetBool("IsMoving", false);
        }
        else
        {
            anim.SetBool("IsMoving", true);
        }
    }
}
