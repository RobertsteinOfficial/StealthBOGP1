using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationGroup : MonoBehaviour
{
    List<NavMeshAgent> agents = new List<NavMeshAgent>();

    private void Start()
    {
        NavMeshAgent[] tempAgents = FindObjectsOfType<NavMeshAgent>();
        agents.AddRange(tempAgents);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                foreach (NavMeshAgent agent in agents)
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
