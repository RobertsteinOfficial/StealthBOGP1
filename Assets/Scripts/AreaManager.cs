using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] GameObject[] hidingSpots;
    [SerializeField] Transform safeZone;
    public GameObject[] HidingSpots { get { return hidingSpots; } }
    public Transform SafeZone { get { return safeZone; } }

    public static AreaManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
