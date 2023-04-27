using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}

public enum Event
{
    Enter,
    Update,
    Exit
}


public class AIState
{
    public State name;
    protected Event stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected AIState nextState;
    protected NavMeshAgent agent;
    protected Transform[] checkpoints;

    //Senses
    float visDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 7.0f;

    public AIState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
    {
        stage = Event.Enter;
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        checkpoints = _checkpoints;
    }

    public virtual void Enter() { stage = Event.Update; }
    public virtual void Update() { /*stage = Event.Update;*/ }
    public virtual void Exit() { stage = Event.Exit; }

    public AIState Process()
    {
        if (stage == Event.Enter) Enter();
        else if (stage == Event.Update) Update();
        else if (stage == Event.Exit)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    #region Helpers

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }

        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
        {
            return true;
        }

        return false;
    }

    #endregion

}
