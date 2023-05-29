using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Lista di nodi da valutare -> OPEN
// Lista di nodi esaminati -> CLOSED

//starting node aggiunto a OPEN

//loop
//nodo corrente: il nodo in OPEN con l'f-cost minore
//rimuoviamo il nodo corrente da OPEN e lo aggiungiamo a CLOSED


//if nodo corrente é = a destination node 
//return path

//foreach neighbour del nodo corrente
//se neighbour non è percorribile oppure è in CLOSED
//skip
//if il percorso verso neighbour è PIU CORTO o neighbour non è in OPEN
//calcoliamo il suo f-cost
//settiamo il nodo corrente come parent di neighbour 
//se niegbour non è in OPEN ce lo aggiungiamo



public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;

    public int gCost, hCost;
    public int fCost { get { return gCost + hCost; } }

    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
