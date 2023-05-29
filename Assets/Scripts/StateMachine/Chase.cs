using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : AIState
{
    Player playerComponent;

    public Chase(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Chase;
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
        Pursue();

        if (agent.hasPath)
        {
            if(CanSeeMe())
            {
                nextState = new Hide(npc, agent, anim, player, checkpoints);
                stage = Event.Exit;
                return;
            }

            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player, checkpoints);
                stage = Event.Exit;
            }
            else if (!CanSeePlayer())
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

    void Pursue()
    {
        Vector3 targetDir = player.position - npc.transform.position;

        float relativeHeading =
            Vector3.Angle(npc.transform.forward, npc.transform.TransformVector(playerComponent.CurrentVelocity.normalized));
        float toTarget = Vector3.Angle(npc.transform.forward, npc.transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20))
        {
            agent.SetDestination(player.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + playerComponent.CurrentVelocity.magnitude);
        Vector3 dest = player.position + playerComponent.CurrentVelocity.normalized * lookAhead * 5;

        Debug.DrawRay(player.position, playerComponent.CurrentVelocity.normalized, Color.green);
        Debug.DrawRay(npc.transform.position, playerComponent.CurrentVelocity.normalized * lookAhead * 100, Color.magenta);

        agent.SetDestination(dest);
    }
}
