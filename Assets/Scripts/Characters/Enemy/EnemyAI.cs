using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; } // NavMeshAgent���Q�Ƃ��܂��B
    
    private Vector3 m_currentTarget;                // �ړI���i�[���܂��B

    private bool m_isWaiting = false;               // �҂����Ԃ��m�F���܂��B
    private float m_waitTimer = 0;                  // �҂����Ԃ̃^�C�}�[�ł��B
    private float m_waitDuration = 0;               // �҂����Ԃ��i�[���܂��B

    /// <summary>
    /// NavMeshAgent���擾
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
    /// ���̃��\�b�h��Update�ŌĂԂ��ƂŃp�g���[�����s���܂��B
    /// </summary>
    /// <param name="enemy"></param>
    public void Patrol(Enemy enemy)
    {
        // �G�[�W�F���g���i�u���b�V����ɂ��邩�ANavMeshAgent���ݒ肳��Ă��邩���m�F���܂��B
        if (!Agent.isOnNavMesh || !Agent.enabled)
        {
            Debug.LogWarning("Agent is not active or not on NavMesh.");
            return;
        }

        // �҂����Ԃ��ǂ������m�F���܂��B
        if (m_isWaiting)
        {
            // �҂����ԂȂ�^�C�}�[���J�n���܂��B
            HandleWaiting(enemy);
        }
        // �ړI�n�ɂ��Ă��邩���m�F���܂��B
        else if (IsDestinationReached())
        {
            // �ړI�n�ɂ��Ă���Ȃ�҂����Ԃ�ݒ肵�܂��B
            StartWaiting();
        }
    }

    /// <summary>
    /// �҂����Ԃ̃^�C�}�[���J�n�A��������ƃ����_���ȏꏊ�ɖړI�n��ݒ肵�܂��B
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
    /// �҂����Ԃ̐ݒ�A�ړI�n�̏������Ȃǂ��s���܂��B
    /// </summary>
    private void StartWaiting()
    {
        m_isWaiting = true;
        m_waitTimer = 0f;
        m_waitDuration = Random.Range(2, 5);

        // NavMeshAgent���~
        Agent.ResetPath();
    }

    /// <summary>
    /// ����̃I�u�W�F�N�g��ǐՂ������Ƃ��Ɏg���܂��B
    /// </summary>
    /// <param name="currentTarget">�ǐՂ������I�u�W�F�N�g</param>
    public void SetNewDestination(Transform currentTarget)
    {
        Agent.SetDestination(currentTarget.position);
    }

    /// <summary>
    /// �����̃����_���ȏꏊ�ɖړI�n��ݒ肵�܂��B
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
    /// �����̃����_���ȏꏊ���擾���܂��B
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private Vector3 GetRandomWaypoint(Enemy enemy)
    {
        // �����̍ŏ��l�ƍő�l���g�������̒��̃����_���ȏꏊ���擾���܂��B
        Vector3 wayPoint = new Vector3(Random.Range(enemy.Room.xMin + 1, enemy.Room.xMax - 1), 0, Random.Range(enemy.Room.yMin + 1, enemy.Room.yMax - 1)) * 4;

        // NavMesh��ł��邩�m�F���܂��B
        NavMeshHit hit;
        if (NavMesh.SamplePosition(wayPoint, out hit, 1f, NavMesh.AllAreas))
        {
            // NavMesh��Ȃ�l��Ԃ��܂��B
            return wayPoint;
        }
        else
        {
            // NavMesh��ł͂Ȃ��Ȃ�zero��Ԃ��܂��B
            return Vector3.zero;
        }
    }

    /// <summary>
    /// �ړI�n�ɂ��Ă��邩���m�F���܂��B
    /// </summary>
    /// <returns>�ړI�n�ɂ��Ă���Ȃ� True ���Ă��Ȃ��Ȃ� false</returns>
    private bool IsDestinationReached()
    {
        return !Agent.pathPending &&
               Agent.remainingDistance <= Agent.stoppingDistance &&
               (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f);
    }
}
