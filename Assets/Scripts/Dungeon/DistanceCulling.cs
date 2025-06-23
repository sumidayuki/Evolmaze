using UnityEngine;

public class DistanceCulling : MonoBehaviour
{
    private Transform player;
    private float cullingDistance = 50f; // 50m�ȏ㗣�ꂽ���\��

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

    }

    void Update()
    {
        if (player == null)
        {
            if(GameObject.FindGameObjectWithTag("Player") == null)
            {
                return;
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        float distance = Vector3.Distance(transform.position, player.position);
        rend.enabled = (distance < cullingDistance);
    }
}