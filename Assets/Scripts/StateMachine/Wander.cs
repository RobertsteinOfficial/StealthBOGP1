using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : AIState
{
    float wanderRadius = 20;
    float wanderDistance = 15;
    float wanderJitter = 3;

    Vector3 wanderTarget = Vector3.zero;

    public Wander(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Wander;
    }

    public override void Enter()
    {
        agent.speed = 2;
        agent.isStopped = false;
        anim.SetBool("IsMoving", true);
        base.Enter();
    }

    public override void Update()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                     Random.Range(-1.0f, 1.0f) * wanderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, npc.transform.position.y, wanderDistance);
        Vector3 targetWorld = npc.transform.InverseTransformVector(targetLocal);

        Debug.DrawLine(npc.transform.position, targetWorld, Color.red);

        agent.SetDestination(targetWorld);

        if (CanSeePlayer())
        {
            nextState = new Chase(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
        }
        else if (IsPlayerBehind())
        {
            nextState = new Evade(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
        }
    }

    public override void Exit()
    {
        anim.SetBool("IsMoving", false);
        base.Exit();
    }
}
