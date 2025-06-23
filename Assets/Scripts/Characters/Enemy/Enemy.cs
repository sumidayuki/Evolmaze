using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character , IDamageable
{
    private StateManager<Enemy> m_stateManager;  // �G�l�~�[�̃X�e�[�g�̏󋵂Ȃǂ̊Ǘ������܂��B
    [SerializeField] EnemyData m_enemyData;         // �G�l�~�[�f�[�^���Q�Ƃ��܂��B

    /// <summary>
    /// �G�l�~�[�̃J�������Ǘ����܂��B
    /// </summary>
    public EnemyCamera Camera { get; private set; }

    /// <summary>
    /// �G�l�~�[��AI���Ǘ����܂��B
    /// </summary>
    public EnemyAI AI { get; private set; }

    /// <summary>
    /// �G�l�~�[��UI���Ǘ����܂��B
    /// </summary>
    public EnemyUI UI { get; private set; }

    /// <summary>
    /// �G�l�~�[���p�j���Ă��镔�����擾���܂��B
    /// </summary>
    public Rect Room { get; set; }

    /// <summary>
    /// �G�l�~�[�̍ő�HP���擾���܂��B
    /// </summary>
    public float MaxHP { get; private set; }

    /// <summary>
    /// ���݂�HP���擾���܂��B
    /// </summary>
    public float CurrentHP { get; private set; }

    private float m_moveSpeed;      // �G�l�~�[�̈ړ����x���Ǘ����܂��B
    private EnemyType m_type;    // �G�l�~�[���A�O���b�V�u�^���ǂ������Ǘ����܂��B

    /// <summary>
    /// �ړ����x���擾���܂��B
    /// </summary>
    public float MoveSpeed { get { return m_moveSpeed; } }

    /// <summary>
    /// �G�l�~�[�̃^�C�v���擾���܂��B
    /// </summary>
    public EnemyType GetEnemyType { get { return m_type; } }

    public EnemyWeapon Weapon { get; private set; }

    public bool IsHit { get; private set; }

    /// <summary>
    /// ���������s���܂��B
    /// </summary>
    private void Awake()
    {
        Anime = gameObject.AddComponent<CharacterAnime>();                                      // CharacterAnime���R���|�[�l���g�Œǉ����AAnime�Ɋi�[
        Camera = gameObject.AddComponent<EnemyCamera>();                                        // EnemyCamera���R���|�[�l���g�Œǉ����ACamera�Ɋi�[
        AI = gameObject.AddComponent<EnemyAI>();                                                // EnemyAI���R���|�[�l���g�Œǉ����AAI�Ɋi�[
        UI = gameObject.GetComponentInChildren<EnemyUI>();                                                       // �V�[������EnemyUI��T���AUI�Ɋi�[
        DungeonManager dungeonManager = FindObjectOfType<DungeonManager>();
        Weapon = FindObjectOfType<EnemyWeapon>();

        MaxHP = m_enemyData.health + 
                (dungeonManager.FloorData.floorNumber * m_enemyData.health / 2) * 
                (1 + (dungeonManager.EnemyDefeatedCount / 100));                                // MaxHP��enemyDeta��health���i�[
        
        CurrentHP = MaxHP;                                                                      // ���݂�HP��MaxHP�ŏ�����
        
       float attackDamage = m_enemyData.attackDamage + 
                          (dungeonManager.FloorData.floorNumber * m_enemyData.attackDamage / 2) * 
                          (1 + (dungeonManager.EnemyDefeatedCount / 100));                          // �U���͂�enemyDeta��attackDamage�Ŋi�[

        Weapon.SetAttackDamage((int)attackDamage);

        m_moveSpeed = m_enemyData.moveSpeed * (1 + (dungeonManager.EnemyEscapeCount / 50));    // �ړ����x��enemyDeta��moveSpeed�Ŋi�[
        
        m_type = m_enemyData.enemyType;                                              // �G�l�~�[�̃^�C�v��enemyDeta��isAggressive�Ŋi�[

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
