using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtPos : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = transform.parent;

        transform.SetParent(null);
    }

    private void Update()
    {
        transform.position = player.position + Vector3.up * 1.5f;
    }
}
