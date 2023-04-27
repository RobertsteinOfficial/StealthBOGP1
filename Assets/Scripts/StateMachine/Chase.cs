using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : AIState
{
    public Chase(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Chase;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        anim.SetBool("IsMoving", true);
        anim.speed = 2f;
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);

        if(agent.hasPath)
        {
            if(CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player, checkpoints);
                stage = Event.Exit;
            }
            else
            {
                nextState = new Patrol(npc, agent, anim, player, checkpoints);
                stage = Event.Exit;
            }
        }

        base.Update();
    }

    public override void Exit()
    {
        anim.SetBool("IsMoving", false);
        anim.speed = 1;
        base.Exit();
    }
}
