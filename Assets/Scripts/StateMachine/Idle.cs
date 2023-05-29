using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : AIState
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Idle;
    }

    public override void Enter()
    {
        anim.SetBool("IsMoving", false);
        base.Enter();
    }

    public override void Update()
    {
        //if (CanSeePlayer())
        //{
        //    nextState = new Hide(npc, agent, anim, player, checkpoints);
        //    stage = Event.Exit;
        //}

        if (CanSeePlayer())
        {
            if (CanSeeMe())
            {
                nextState = new Hide(npc, agent, anim, player, checkpoints);
                stage = Event.Exit;
            }
            else
            {
                nextState = new Chase(npc, agent, anim, player, checkpoints);
                stage = Event.Exit;
            }
        }
        else if (Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
        }



        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
