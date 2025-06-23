using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスを管理するクラスです。
/// </summary>
public class Boss : Character, IDamageable
{
    private StateManager<Boss> m_stateManager;  // ステートマネージャーを格納するための変数です。

    [SerializeField] EnemyData m_bossData;      // このボスの情報を取得します。

    /// <summary>
    /// ボスのAI挙動を管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public BossAI AI { get; private set; }

    /// <summary>
    /// ボスの攻撃を管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public BossAttack Attack { get; private set; }

    /// <summary>
    /// ボスのUIを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public BossUI UI { get; private set; }

    /// <summary>
    /// ボスの最大Healthを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public float MaxHealth { get; private set; }

    /// <summary>
    /// ボスの現在のHealthを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public float CurrentHealth { get; private set; }

    /// <summary>
    /// ボスが生きているかを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public bool IsDead { get; private set; }

    /// <summary>
    /// ボスの状態を表すステートです。
    /// </summary>
    public enum MYSTATE
    {
        NORMAL,
        ROARING,
    }

    /// <summary>
    /// ボスの状態ステートを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public MYSTATE STATE { get; private set; }

    /// <summary>
    /// ボスの情報を取得できます。
    /// </summary>
    public EnemyData BossData { get { return m_bossData; } }

    private void Awake()
    {
        // 代入
        AI = gameObject.AddComponent<BossAI>();
        Anime = gameObject.AddComponent<CharacterAnime>();
        Attack = gameObject.GetComponent<BossAttack>();
        UI = FindObjectOfType<BossUI>();

        // 初期化
        MaxHealth = m_bossData.health;
        CurrentHealth = MaxHealth;
        IsDead = false;
        AI.Agent.speed = m_bossData.moveSpeed;

        m_stateManager = new StateManager<Boss>();
        m_stateManager.Init(new BossMoveState(), this);

        STATE = MYSTATE.NORMAL;
    }

    private void Update()
    {
        // もし現在のHealthが 0 以下なら
        if (CurrentHealth <= 0)
        {
            IsDead = true;
            ChangeState(new BossDeathState());
            return;
        }

        // 現在のステートが有効なら
        if (m_stateManager.CurrentState != null)
        {
            // 現在のステートの更新処理を実行します。
            m_stateManager.CurrentState.Execute(this);
        }
    }

    /// <summary>
    /// ボスの状態ステートを変更します。
    /// </summary>
    public void ChangeMyState(MYSTATE state)
    {
        STATE = state;
    }

    /// <summary>
    /// ボスのステートを変更します。
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(StateBase<Boss> newState)
    {
        m_stateManager.ChangeState(newState, this);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(float amount)
    {
        // 現在のHealthからダメージを引きます。
        CurrentHealth -= amount;
    }
}
