using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTConttroller : MonoBehaviour
{
    [SerializeField] private Transform treasure;
    [SerializeField] private Transform safeZone;
    [SerializeField] private Transform frontDoor;
    [SerializeField] private Transform backDoor;

    public enum ActionState { Idle, Working };
    ActionState state = ActionState.Idle;

    BTNode.Status treeStatus = BTNode.Status.Running;

    BTRoot tree;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BTRoot();
        BTNode theft = new Sequence("Recupera il tesoro");
        BTNode goToFrontDoor = new Leaf("Raggiungi la porta principale", GoToFrontDoor);
        BTNode goToBackDoor = new Leaf("Raggiungi la porta sul retro", GoToBackDoor);
        BTNode goToItem = new Leaf("Raggiungi l'Item", GoToItem);
        BTNode getToSafety = new Leaf("Scappa dall'edificio", GetToSafety);

        theft.AddChild(goToItem);
        theft.AddChild(getToSafety);
        tree.AddChild(theft);

        tree.PrintTree();

    }

    private void Update()
    {
        if (treeStatus == BTNode.Status.Running)
            treeStatus = tree.Process();
    }

    public BTNode.Status GoToFrontDoor()
    {
        return GoToLocation(frontDoor.position);
    }

    public BTNode.Status GoToBackDoor()
    {
        return GoToLocation(backDoor.position);
    }

    public BTNode.Status GoToItem()
    {
        return GoToLocation(treasure.position);
    }

    public BTNode.Status GetToSafety()
    {
        return GoToLocation(safeZone.position);
    }

    BTNode.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);

        if (state == ActionState.Idle)
        {
            agent.SetDestination(destination);
            state = ActionState.Working;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.Idle;
            return BTNode.Status.Failure;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.Idle;
            return BTNode.Status.Success;
        }

        return BTNode.Status.Running;
    }
}
