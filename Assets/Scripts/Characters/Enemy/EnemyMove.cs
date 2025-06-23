using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public void ChasePlayer(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction * 3 * Time.deltaTime;
    }
}
