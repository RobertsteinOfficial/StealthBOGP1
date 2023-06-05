using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
OPEN
CLOSED

aggiungo il nodo iniziale in OPEN
 
loop
imposto come nodo corrente il nodo in OPEN con l'f-cost minore
rimuovo nodo corrente da OPEN
e lo aggiungo in CLOSED

if il nodo corrente == target node allora sono arrivato

foreach neighbour del nodo corrente
if neigbour non è percorribile o è già in close
skip

if neighbour è più vicino a target node rispetto al nodo corrente || neighbour non è in OPEN
calcoliamo l'f-cost del neighbour
imparentiamo il neighbour al nodo corrente

if neighbour non è in OPEN
aggiungiamo neighbour a OPEN

 */



public class Node
{
    public Vector3 worldPosition;
    public bool walkable;

    public int gCost;
    public int hCost;
    public int FCost { get { return gCost + hCost; } }

    public int gridX, gridY;

    public Node parent;

    public Node(Vector3 _worldPos, bool _walkable, int _gridX, int _gridY)
    {
        worldPosition = _worldPos;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }
}
