using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Leaf : BT_Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public BT_Leaf() { }

    public BT_Leaf(string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }

    public override Status Process()
    {
        if (ProcessMethod != null)
            return ProcessMethod();
        return Status.Failure;
    }
}
