using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X���Ǘ�����N���X�ł��B
/// </summary>
public class Boss : Character, IDamageable
{
    private StateManager<Boss> m_stateManager;  // �X�e�[�g�}�l�[�W���[���i�[���邽�߂̕ϐ��ł��B

    [SerializeField] EnemyData m_bossData;      // ���̃{�X�̏����擾���܂��B

    /// <summary>
    /// �{�X��AI�������Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public BossAI AI { get; private set; }

    /// <summary>
    /// �{�X�̍U�����Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public BossAttack Attack { get; private set; }

    /// <summary>
    /// �{�X��UI���Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public BossUI UI { get; private set; }

    /// <summary>
    /// �{�X�̍ő�Health���Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public float MaxHealth { get; private set; }

    /// <summary>
    /// �{�X�̌��݂�Health���Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public float CurrentHealth { get; private set; }

    /// <summary>
    /// �{�X�������Ă��邩���Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public bool IsDead { get; private set; }

    /// <summary>
    /// �{�X�̏�Ԃ�\���X�e�[�g�ł��B
    /// </summary>
    public enum MYSTATE
    {
        NORMAL,
        ROARING,
    }

    /// <summary>
    /// �{�X�̏�ԃX�e�[�g���Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public MYSTATE STATE { get; private set; }

    /// <summary>
    /// �{�X�̏����擾�ł��܂��B
    /// </summary>
    public EnemyData BossData { get { return m_bossData; } }

    private void Awake()
    {
        // ���
        AI = gameObject.AddComponent<BossAI>();
        Anime = gameObject.AddComponent<CharacterAnime>();
        Attack = gameObject.GetComponent<BossAttack>();
        UI = FindObjectOfType<BossUI>();

        // ������
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
        // �������݂�Health�� 0 �ȉ��Ȃ�
        if (CurrentHealth <= 0)
        {
            IsDead = true;
            ChangeState(new BossDeathState());
            return;
        }

        // ���݂̃X�e�[�g���L���Ȃ�
        if (m_stateManager.CurrentState != null)
        {
            // ���݂̃X�e�[�g�̍X�V���������s���܂��B
            m_stateManager.CurrentState.Execute(this);
        }
    }

    /// <summary>
    /// �{�X�̏�ԃX�e�[�g��ύX���܂��B
    /// </summary>
    public void ChangeMyState(MYSTATE state)
    {
        STATE = state;
    }

    /// <summary>
    /// �{�X�̃X�e�[�g��ύX���܂��B
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(StateBase<Boss> newState)
    {
        m_stateManager.ChangeState(newState, this);
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(float amount)
    {
        // ���݂�Health����_���[�W�������܂��B
        CurrentHealth -= amount;
    }
}
