using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Root : BT_Node
{
    public BT_Root()
    {
        name = "Root";
    }

    public BT_Root(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        return children[currentChild].Process();
    }

    public void PrintTree()
    {
        string treePrintout = "";
        Stack<BT_NodeLevel> nodeStack = new Stack<BT_NodeLevel>();
        BT_Node currentNode = this;
        nodeStack.Push(new BT_NodeLevel { node = currentNode, level = 0 });

        while (nodeStack.Count != 0)
        {
            BT_NodeLevel nextNode = nodeStack.Pop();
            treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";

            for (int i = nextNode.node.children.Count - 1; i >= 0; i--)
            {
                nodeStack.Push(new BT_NodeLevel { node = nextNode.node.children[i], level = nextNode.level + 1 });
            }
        }

        Debug.Log(treePrintout);
    }

    struct BT_NodeLevel
    {
        public BT_Node node;
        public int level;
    }
}
