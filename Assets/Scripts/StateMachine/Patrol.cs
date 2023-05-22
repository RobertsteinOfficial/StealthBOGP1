using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : AIState
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Patrol;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float dist = Mathf.Infinity;

        for (int i = 0; i < checkpoints.Length; i++)
        {
            Transform currentCheckpoint = checkpoints[i];
            float tempDist = Vector3.Distance(npc.transform.position, currentCheckpoint.transform.position);

            if (tempDist < dist)
            {
                currentIndex = i - 1;
                dist = tempDist;
            }
        }


        anim.SetBool("IsMoving", true);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 2)
        {
            if (currentIndex >= checkpoints.Length - 1)
                currentIndex = 0;
            else
                currentIndex++;

            agent.SetDestination(checkpoints[currentIndex].position);
        }

        if (CanSeePlayer())
        {
            nextState = new Hide(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
        }
        else if (IsPlayerBehind())
        {
            nextState = new Hide(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
        }
    }

    public override void Exit()
    {
        anim.SetBool("IsMoving", false);
        base.Exit();
    }
}
