using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee : AIState
{
    Transform safeZone;

    public Flee(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Flee;
        safeZone = AreaManager.Instance.SafeZone;
    }

    public override void Enter()
    {
        anim.SetBool("IsMoving", true);
        agent.isStopped = false;
        agent.speed = 6;
        agent.SetDestination(safeZone.position);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 2)
        {
            nextState = new Idle(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
        }

        base.Update();
    }

    public override void Exit()
    {
        anim.SetBool("IsMoving", false);
        base.Exit();
    }
}
