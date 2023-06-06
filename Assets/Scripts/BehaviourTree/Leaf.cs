using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : BTNode
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public Leaf() { }
    public Leaf(string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }

    public override Status Process()
    {
        if (ProcessMethod != null)
            return ProcessMethod();
        Debug.Log("No method attached to delegate");
        return Status.Failure;
    }
}
