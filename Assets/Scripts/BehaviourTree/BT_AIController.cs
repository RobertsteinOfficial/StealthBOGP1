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
    [SerializeField] Transform frontDoor;
    [SerializeField] Transform backDoor;

    public bool itemIsGuarded = true;

    NavMeshAgent agent;
    BT_Root treeRoot;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        treeRoot = new BT_Root();
        BT_Sequence lootItem = new BT_Sequence("Acquire Item");
        BT_Leaf getToItem = new BT_Leaf("Get To Item", GetToItem);
        BT_Leaf escape = new BT_Leaf("Escape", GetToSafety);
        BT_Leaf goToFrontDoor = new BT_Leaf("Go To FrontDoor", GoToFrontDoor);
        BT_Leaf goToBackDoor = new BT_Leaf("Go To BackDoor", GoToBackDoor);
        BT_Selector openDoor = new BT_Selector("Open Door");
        BT_Leaf canGetToItem = new BT_Leaf("Can Get To Item", CanGetToItem);

        //Primo layer
        treeRoot.AddChild(lootItem);

        //Secondo layer
        lootItem.AddChild(canGetToItem);
        lootItem.AddChild(openDoor);
        lootItem.AddChild(getToItem);
        lootItem.AddChild(escape);

        //terzo layer
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        treeRoot.PrintTree();

    }

    private void Update()
    {
        if (treeStatus != BT_Node.Status.Success)
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

    public BT_Node.Status GoToDoor(Transform door)
    {
        BT_Node.Status s = GoToLocation(door.position);

        if (s == BT_Node.Status.Success)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.gameObject.SetActive(false);
                return BT_Node.Status.Success;
            }
            else
            {
                return BT_Node.Status.Failure;
            }
        }
        else
        {
            return s;
        }


    }

    public BT_Node.Status GetToItem()
    {
        BT_Node.Status s = GoToLocation(target.position);

        if (s == BT_Node.Status.Success)
        {
            target.parent = transform;
        }

        return s;
    }

    public BT_Node.Status GetToSafety()
    {
        return GoToLocation(escapeRoute.position);
    }

    public BT_Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    public BT_Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public BT_Node.Status CanGetToItem()
    {
        if (itemIsGuarded)
            return BT_Node.Status.Failure;

        return BT_Node.Status.Success;
    }
}
