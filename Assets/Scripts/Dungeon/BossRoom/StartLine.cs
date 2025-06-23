using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    public bool IsStart { get; private set; } = false;
    public GameObject Player { get; private set; }

    private void Awake()
    {
        IsStart = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            IsStart = true;
            Player = other.gameObject;
        }
    }
}
