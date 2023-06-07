using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Sequence : BT_Node
{
    public BT_Sequence(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        Debug.Log(children[currentChild].name + " " + childStatus);
        if (childStatus == Status.Running) return Status.Running;
        if (childStatus == Status.Failure) return childStatus;

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.Success;
        }

        return Status.Running;
    }
}
