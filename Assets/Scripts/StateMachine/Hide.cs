using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hide : AIState
{
    private float hideDist = 15;

    public Hide(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Transform[] _checkpoints)
         : base(_npc, _agent, _anim, _player, _checkpoints)
    {
        name = State.Hide;
    }

    public override void Enter()
    {
        agent.isStopped = false;
        anim.SetBool("IsMoving", true);
        base.Enter();
    }


    public override void Update()
    {
        SimpleHide();
        //AdvancedHide();
    }


    public override void Exit()
    {
        anim.SetBool("IsMoving", false);
        base.Exit();
    }

    void SimpleHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < AreaManager.Instance.HidingSpots.Length; i++)
        {
            Vector3 hideDir = AreaManager.Instance.HidingSpots[i].transform.position - player.transform.position;
            Vector3 hidePos = AreaManager.Instance.HidingSpots[i].transform.position + hideDir.normalized * hideDist;

            if (Vector3.SqrMagnitude(hidePos - npc.transform.position) < dist * dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(npc.transform.position, hidePos);
            }
        }

        Debug.Log(Vector3.SqrMagnitude(chosenSpot - npc.transform.position));

        if (Vector3.SqrMagnitude(chosenSpot - npc.transform.position) < 11)
        {
            nextState = new Idle(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
            return;
        }

        agent.SetDestination(chosenSpot);
    }

    void AdvancedHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        //GameObject chosenGO = AreaManager.Instance.HidingSpots[0];

        for (int i = 0; i < AreaManager.Instance.HidingSpots.Length; i++)
        {
            Vector3 hideDir = AreaManager.Instance.HidingSpots[i].transform.position - player.transform.position;
            Vector3 hidePos = AreaManager.Instance.HidingSpots[i].transform.position + hideDir.normalized * hideDist;

            if (Vector3.SqrMagnitude(hidePos - npc.transform.position) < dist * dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                //chosenGO = AreaManager.Instance.HidingSpots[i];
                dist = Vector3.Distance(npc.transform.position, hidePos);
            }
        }


        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit hit;
        float rayDistance = 100.0f;
        Physics.Raycast(backRay, out hit, rayDistance);

        Vector3 hideDestination = hit.point + chosenDir.normalized * 4;

        if (Vector3.SqrMagnitude(chosenSpot - npc.transform.position) < 9)
        {
            nextState = new Idle(npc, agent, anim, player, checkpoints);
            stage = Event.Exit;
            return;
        }

        agent.SetDestination(hideDestination);
    }
}
