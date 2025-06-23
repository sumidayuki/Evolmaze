using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyCamera : MonoBehaviour
{
    private const float viewDistance = 20f;
    private const float viewAngle = 120f;
    private LayerMask obstacleMask;
    private LayerMask playerLayer;

    private const float attackRenge = 1f;
    GameObject player;
    public bool isChasing { get; private set; }

    private void Awake()
    {
        obstacleMask = LayerMask.GetMask("Wall");
        playerLayer = LayerMask.GetMask("Player");
        player = null;
    }

    public void SearchPlayer()
    {
        // �v���C���[�����E���ŒT��
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.gameObject;
                break;
            }
        }

        // ���E���̃v���C���[���m�F
        if (player != null && IsPlayerInView())
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    public bool IsAttackRenge()
    {
        bool IsAttackRenge = Vector3.Distance(transform.position, player.transform.position) <= attackRenge;
        Debug.Log(Vector3.Distance(transform.position, player.transform.position));

        return IsAttackRenge;
    }

    private bool IsPlayerInView()
    {
        // �v���C���[�̈ʒu�Ɍ����������x�N�g�����v�Z
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // �v���C���[�ƃG�l�~�[�̊Ԃ̊p�x���v�Z
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // ���E���ɂ��邩�ǂ����̔���
        if (angleToPlayer < viewAngle / 2f)
        {
            RaycastHit hit;
            // �v���C���[�܂Ń��C���΂��āA�����ɎՂ��Ă��邩�`�F�b�N
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance, ~obstacleMask))
            {
                return true;
            }
        }

        return false; // ���E�O�܂��͏�Q���ŎՂ��Ă���
    }

    public Transform GetPlayerPos()
    {
        if (player == null) return null;

        return player.transform;
    }

    void OnDrawGizmos()
    {
        // ���E�̋�����\��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // ���E�̊p�x��\��
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
