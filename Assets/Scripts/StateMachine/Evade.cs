using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Evade : AIState
{
    Player playerComponent;

    public Evade(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Evade;
    }

    public override void Enter()
    {
        agent.speed = 6;
        agent.isStopped = false;
        anim.SetBool("IsMoving", true);
        anim.speed = 2f;
        playerComponent = player.GetComponent<Player>();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 targetDir = player.position - npc.transform.position;

        if (Vector3.SqrMagnitude(targetDir) > 20 * 20)
        {
            nextState = new Idle(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + playerComponent.CurrentVelocity.magnitude);
        Vector3 dest = player.position + playerComponent.CurrentVelocity.normalized * lookAhead * 5;

        Vector3 fleeVector = dest - npc.transform.position;
        agent.SetDestination(npc.transform.position - fleeVector);

        base.Update();
    }

    public override void Exit()
    {
        anim.SetBool("IsMoving", false);
        anim.speed = 1;
        base.Exit();
    }
}
