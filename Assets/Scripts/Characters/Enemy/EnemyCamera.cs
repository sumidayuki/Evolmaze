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
        // プレイヤーを視界内で探す
        Collider[] hits = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.gameObject;
                break;
            }
        }

        // 視界内のプレイヤーを確認
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
        // プレイヤーの位置に向かう方向ベクトルを計算
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // プレイヤーとエネミーの間の角度を計算
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // 視界内にいるかどうかの判定
        if (angleToPlayer < viewAngle / 2f)
        {
            RaycastHit hit;
            // プレイヤーまでレイを飛ばして、何かに遮られているかチェック
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance, ~obstacleMask))
            {
                return true;
            }
        }

        return false; // 視界外または障害物で遮られている
    }

    public Transform GetPlayerPos()
    {
        if (player == null) return null;

        return player.transform;
    }

    void OnDrawGizmos()
    {
        // 視界の距離を表示
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // 視界の角度を表示
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
