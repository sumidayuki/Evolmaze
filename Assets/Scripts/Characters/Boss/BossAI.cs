using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ボスのAI挙動を管理します。
/// </summary>
public class BossAI : MonoBehaviour
{
    /// <summary>
    /// ボスのNavMeshAgentを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public NavMeshAgent Agent { get; private set; }

    /// <summary>
    /// 現在ボスが標的にしているTransformを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public Transform Target { get; private set; }

    private void Awake()
    {
        // 代入
        Agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// NavMeshAgentの初期化を行います。
    /// </summary>
    public void Init()
    {
        // NavMeshAgentの機能である現在のターゲット等をリセットする関数です。
        Agent.ResetPath();
    }

    /// <summary>
    /// ターゲットを設定します。
    /// </summary>
    /// <param name="target">ターゲットのTransform</param>
    public void SetTatget(Transform target)
    {
        Target = target;
    }

    /// <summary>
    /// 設定されているターゲットの場所に移動します。
    /// </summary>
    public void ToTarget()
    {
        Agent.SetDestination(Target.position);
    }

    /// <summary>
    /// ターゲットから少し後退します。
    /// </summary>
    /// <param name="boss"></param>
    public void Retreat(Boss boss)
    {
        // 自分のポジションからターゲットにポジションを引いて正規化することで方向をdirectionに格納します。
        Vector3 direction = (boss.gameObject.transform.position - Target.position).normalized;

        // 現在のポジションからdirection、自分のスピードを掛けた場所をtargetに格納します。
        Vector3 target = boss.transform.position + direction * boss.AI.Agent.angularSpeed * Time.deltaTime * 20;

        // targetの場所に移動します。
        Agent.SetDestination(target);
    }

    /// <summary>
    /// ターゲットの方向を向きます。
    /// </summary>
    /// <param name="boss"></param>
    public void LookTarget(Boss boss)
    {
        // 自分のポジションからターゲットにポジションを引いて正規化することで方向をdirectionに格納します。
        Vector3 direction = (boss.transform.position - Target.position).normalized;

        // 上下の回転を防ぐためにyを0にします。
        direction.y = 0f;

        // directionが0ではないなら
        if (direction != Vector3.zero)
        {
            // QuarternionのLookRotation関数を使って向く方向をrotationに格納します。（directionがtargerからbossに向けるものなので-directionとしている）
            Quaternion rotation = Quaternion.LookRotation(-direction);

            // ボスのローテーションを滑らかにrotationの方向に向かせます。
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, rotation, Time.deltaTime * 10);
        }
    }

    /// <summary>
    /// ターゲットとの距離を取得します。
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetTargetDistance(Vector3 pos)
    {
        return Vector3.Distance(pos, Target.position);
    }
}
