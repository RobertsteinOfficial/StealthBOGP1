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
    Dead,
    Flee,
    Evade,
    Wander,
    Hide
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
    float visDist = 40.0f;
    float visAngle = 60.0f;
    float shootDist = 3.0f;
    float perceptionDist = 2.5f;

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

    public bool CanSeeMe()
    {
        Vector3 direction = npc.transform.position - Camera.main.transform.position;
        float angle = Vector3.Angle(direction, Camera.main.transform.forward);

        if (angle < visAngle)
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

    public bool IsPlayerBehind()
    {
        Vector3 dir = npc.transform.position - player.position;

        float angle = Vector3.Angle(dir, npc.transform.forward);
        if (dir.magnitude < perceptionDist && angle < visAngle)
        {
            return true;
        }

        return false;
    }

    #endregion

}
