using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Node
{
    public enum Status { Success, Running, Failure };
    public Status status;

    public List<BT_Node> children = new List<BT_Node>();
    public int currentChild = 0;

    public string name;

    public BT_Node() { }

    public BT_Node(string n)
    {
        name = n;
    }

    public void AddChild(BT_Node n)
    {
        children.Add(n);
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }
}
