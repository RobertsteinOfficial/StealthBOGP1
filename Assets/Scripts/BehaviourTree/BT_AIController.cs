using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BT_AIController : MonoBehaviour
{
    BT_Node.Status treeStatus = BT_Node.Status.Running;

    public enum ActionState { Idle, Working };
    ActionState state = ActionState.Idle;

    [SerializeField] Transform target;
    [SerializeField] Transform escapeRoute;
    NavMeshAgent agent;
    BT_Root treeRoot;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        treeRoot = new BT_Root();
        BT_Sequence lootItem = new BT_Sequence("Acquire Item");
        BT_Leaf getToItem = new BT_Leaf("Get To Item", GetToItem);
        BT_Leaf escape = new BT_Leaf("Escape", GetToSafety);

        //Primo layer
        treeRoot.AddChild(lootItem);

        //Secondo layer
        lootItem.AddChild(getToItem);
        lootItem.AddChild(escape);

        treeRoot.PrintTree();

    }

    private void Update()
    {
        if (treeStatus == BT_Node.Status.Running)
            treeStatus = treeRoot.Process();
    }

    BT_Node.Status GoToLocation(Vector3 destination)
    {
        if (state == ActionState.Idle)
        {
            agent.SetDestination(destination);
            state = ActionState.Working;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 3)
        {
            state = ActionState.Idle;
            return BT_Node.Status.Failure;
        }
        else if (Vector3.Distance(destination, transform.position) < 3)
        {
            state = ActionState.Idle;
            return BT_Node.Status.Success;
        }

        return BT_Node.Status.Running;
    }

    public BT_Node.Status GetToItem()
    {
        return GoToLocation(target.position);
    }

    public BT_Node.Status GetToSafety()
    {
        return GoToLocation(escapeRoute.position);
    }
}
