using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLine : MonoBehaviour
{
    public bool IsEnd { get; private set; }

    private void Awake()
    {
        IsEnd = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            IsEnd = true;
        }
    }
}
