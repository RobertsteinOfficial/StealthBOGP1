using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : AIState
{
    float rotationSpeed = 340000;
    AudioSource attackSound;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
        : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Attack;
        attackSound = npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        anim.SetTrigger("Attacking");
        agent.isStopped = true;
        //attackSound?.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
            Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        Debug.DrawRay(npc.transform.position, npc.transform.forward * 3, Color.red);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("Attacking");
        agent.Stop();
        base.Exit();
    }
}
