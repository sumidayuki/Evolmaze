using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; } // NavMeshAgentを参照します。
    
    private Vector3 m_currentTarget;                // 目的を格納します。

    private bool m_isWaiting = false;               // 待ち時間を確認します。
    private float m_waitTimer = 0;                  // 待ち時間のタイマーです。
    private float m_waitDuration = 0;               // 待ち時間を格納します。

    /// <summary>
    /// NavMeshAgentを取得
    /// </summary>
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize()
    {
        Agent.isStopped = true;
        Agent.ResetPath();
    }

    /// <summary>
    /// このメソッドをUpdateで呼ぶことでパトロールを行います。
    /// </summary>
    /// <param name="enemy"></param>
    public void Patrol(Enemy enemy)
    {
        // エージェントがナブメッシュ上にいるか、NavMeshAgentが設定されているかを確認します。
        if (!Agent.isOnNavMesh || !Agent.enabled)
        {
            Debug.LogWarning("Agent is not active or not on NavMesh.");
            return;
        }

        // 待ち時間かどうかを確認します。
        if (m_isWaiting)
        {
            // 待ち時間ならタイマーを開始します。
            HandleWaiting(enemy);
        }
        // 目的地についているかを確認します。
        else if (IsDestinationReached())
        {
            // 目的地についているなら待ち時間を設定します。
            StartWaiting();
        }
    }

    /// <summary>
    /// 待ち時間のタイマーを開始、完了するとランダムな場所に目的地を設定します。
    /// </summary>
    /// <param name="enemy"></param>
    private void HandleWaiting(Enemy enemy)
    {
        m_waitTimer += Time.deltaTime;

        if (m_waitTimer >= m_waitDuration)
        {
            m_isWaiting = false;
            SetNewRamdomDestination(enemy);
        }
    }

    /// <summary>
    /// 待ち時間の設定、目的地の初期化などを行います。
    /// </summary>
    private void StartWaiting()
    {
        m_isWaiting = true;
        m_waitTimer = 0f;
        m_waitDuration = Random.Range(2, 5);

        // NavMeshAgentを停止
        Agent.ResetPath();
    }

    /// <summary>
    /// 特定のオブジェクトを追跡したいときに使います。
    /// </summary>
    /// <param name="currentTarget">追跡したいオブジェクト</param>
    public void SetNewDestination(Transform currentTarget)
    {
        Agent.SetDestination(currentTarget.position);
    }

    /// <summary>
    /// 部屋のランダムな場所に目的地を設定します。
    /// </summary>
    /// <param name="enemy"></param>
    public void SetNewRamdomDestination(Enemy enemy)
    {
        Vector3 nearestWaypoint = GetRandomWaypoint(enemy);

        if (nearestWaypoint != Vector3.zero)
        {
            m_currentTarget = nearestWaypoint;

            if (!Agent.isOnNavMesh) return;
            Agent.SetDestination(m_currentTarget);
        }
    }

    /// <summary>
    /// 部屋のランダムな場所を取得します。
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private Vector3 GetRandomWaypoint(Enemy enemy)
    {
        // 部屋の最小値と最大値を使い部屋の中のランダムな場所を取得します。
        Vector3 wayPoint = new Vector3(Random.Range(enemy.Room.xMin + 1, enemy.Room.xMax - 1), 0, Random.Range(enemy.Room.yMin + 1, enemy.Room.yMax - 1)) * 4;

        // NavMesh上であるか確認します。
        NavMeshHit hit;
        if (NavMesh.SamplePosition(wayPoint, out hit, 1f, NavMesh.AllAreas))
        {
            // NavMesh上なら値を返します。
            return wayPoint;
        }
        else
        {
            // NavMesh上ではないならzeroを返します。
            return Vector3.zero;
        }
    }

    /// <summary>
    /// 目的地についているかを確認します。
    /// </summary>
    /// <returns>目的地についているなら True ついていないなら false</returns>
    private bool IsDestinationReached()
    {
        return !Agent.pathPending &&
               Agent.remainingDistance <= Agent.stoppingDistance &&
               (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f);
    }
}
