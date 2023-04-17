using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle,
    Patrolling,
    Chasing,
    Dead
}

public class SimpleStatemachine : MonoBehaviour
{
    public State actualState = State.Idle;
    NavMeshAgent agent;
    public Transform[] targets;

    int actualTarget = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (actualState == State.Dead) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            actualState = State.Patrolling;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            actualState = State.Chasing;
        }


        if (actualState == State.Patrolling)
        {
            Debug.Log("Patrolling");
            agent.SetDestination(targets[actualTarget].position);

            if(Vector3.Distance(agent.destination, transform.position) < 2)
            {
                actualTarget++;
                if(actualTarget >= targets.Length)
                {
                    actualTarget = 0;
                }
            }

        }
        else if (actualState == State.Chasing)
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        }
    }
}
