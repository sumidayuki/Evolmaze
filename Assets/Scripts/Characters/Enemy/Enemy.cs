using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character , IDamageable
{
    private StateManager<Enemy> m_stateManager;  // エネミーのステートの状況などの管理をします。
    [SerializeField] EnemyData m_enemyData;         // エネミーデータを参照します。

    /// <summary>
    /// エネミーのカメラを管理します。
    /// </summary>
    public EnemyCamera Camera { get; private set; }

    /// <summary>
    /// エネミーのAIを管理します。
    /// </summary>
    public EnemyAI AI { get; private set; }

    /// <summary>
    /// エネミーのUIを管理します。
    /// </summary>
    public EnemyUI UI { get; private set; }

    /// <summary>
    /// エネミーが徘徊している部屋を取得します。
    /// </summary>
    public Rect Room { get; set; }

    /// <summary>
    /// エネミーの最大HPを取得します。
    /// </summary>
    public float MaxHP { get; private set; }

    /// <summary>
    /// 現在のHPを取得します。
    /// </summary>
    public float CurrentHP { get; private set; }

    private float m_moveSpeed;      // エネミーの移動速度を管理します。
    private EnemyType m_type;    // エネミーがアグレッシブ型かどうかを管理します。

    /// <summary>
    /// 移動速度を取得します。
    /// </summary>
    public float MoveSpeed { get { return m_moveSpeed; } }

    /// <summary>
    /// エネミーのタイプを取得します。
    /// </summary>
    public EnemyType GetEnemyType { get { return m_type; } }

    public EnemyWeapon Weapon { get; private set; }

    public bool IsHit { get; private set; }

    /// <summary>
    /// 初期化を行います。
    /// </summary>
    private void Awake()
    {
        Anime = gameObject.AddComponent<CharacterAnime>();                                      // CharacterAnimeをコンポーネントで追加し、Animeに格納
        Camera = gameObject.AddComponent<EnemyCamera>();                                        // EnemyCameraをコンポーネントで追加し、Cameraに格納
        AI = gameObject.AddComponent<EnemyAI>();                                                // EnemyAIをコンポーネントで追加し、AIに格納
        UI = gameObject.GetComponentInChildren<EnemyUI>();                                                       // シーン内のEnemyUIを探し、UIに格納
        DungeonManager dungeonManager = FindObjectOfType<DungeonManager>();
        Weapon = FindObjectOfType<EnemyWeapon>();

        MaxHP = m_enemyData.health + 
                (dungeonManager.FloorData.floorNumber * m_enemyData.health / 2) * 
                (1 + (dungeonManager.EnemyDefeatedCount / 100));                                // MaxHPにenemyDetaのhealthを格納
        
        CurrentHP = MaxHP;                                                                      // 現在のHPをMaxHPで初期化
        
       float attackDamage = m_enemyData.attackDamage + 
                          (dungeonManager.FloorData.floorNumber * m_enemyData.attackDamage / 2) * 
                          (1 + (dungeonManager.EnemyDefeatedCount / 100));                          // 攻撃力をenemyDetaのattackDamageで格納

        Weapon.SetAttackDamage((int)attackDamage);

        m_moveSpeed = m_enemyData.moveSpeed * (1 + (dungeonManager.EnemyEscapeCount / 50));    // 移動速度をenemyDetaのmoveSpeedで格納
        
        m_type = m_enemyData.enemyType;                                              // エネミーのタイプをenemyDetaのisAggressiveで格納

        IsHit = false;

        m_stateManager = new StateManager<Enemy>();      
        m_stateManager.Init(new EnemyPatrolState(), this);
    }

    public void Update()
    {
        m_stateManager.CurrentState.Execute(this);
        UI.RotateCanvas();
    }

    public void ChangeState(StateBase<Enemy> newState)
    {
        if (m_stateManager.CurrentState != new EnemyDeathState())
        {
            m_stateManager.ChangeState(newState, this);
        }
    }

    public void Damage(float damage)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
        StartCoroutine(TakeDamage());

        UI.TakeDamage(CurrentHP / MaxHP);
    }

    private IEnumerator TakeDamage()
    {
        IsHit = true;
        yield return new WaitForSeconds(0.2f);
        IsHit = false;
    }

    public void Death()
    {
        ChangeState(new EnemyDeathState());
        UI.HideHPBar();
    }
}
