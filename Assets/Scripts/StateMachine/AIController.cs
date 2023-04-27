using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform[] checkpoints;

    NavMeshAgent agent;
    Animator anim;
    AIState currentState;

    private void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player, checkpoints);
    }

    private void Update()
    {
        currentState = currentState.Process();
    }
}
