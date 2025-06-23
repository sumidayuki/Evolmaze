using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �{�X��AI�������Ǘ����܂��B
/// </summary>
public class BossAI : MonoBehaviour
{
    /// <summary>
    /// �{�X��NavMeshAgent���Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public NavMeshAgent Agent { get; private set; }

    /// <summary>
    /// ���݃{�X���W�I�ɂ��Ă���Transform���Ǘ����܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public Transform Target { get; private set; }

    private void Awake()
    {
        // ���
        Agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// NavMeshAgent�̏��������s���܂��B
    /// </summary>
    public void Init()
    {
        // NavMeshAgent�̋@�\�ł��錻�݂̃^�[�Q�b�g�������Z�b�g����֐��ł��B
        Agent.ResetPath();
    }

    /// <summary>
    /// �^�[�Q�b�g��ݒ肵�܂��B
    /// </summary>
    /// <param name="target">�^�[�Q�b�g��Transform</param>
    public void SetTatget(Transform target)
    {
        Target = target;
    }

    /// <summary>
    /// �ݒ肳��Ă���^�[�Q�b�g�̏ꏊ�Ɉړ����܂��B
    /// </summary>
    public void ToTarget()
    {
        Agent.SetDestination(Target.position);
    }

    /// <summary>
    /// �^�[�Q�b�g���班����ނ��܂��B
    /// </summary>
    /// <param name="boss"></param>
    public void Retreat(Boss boss)
    {
        // �����̃|�W�V��������^�[�Q�b�g�Ƀ|�W�V�����������Đ��K�����邱�Ƃŕ�����direction�Ɋi�[���܂��B
        Vector3 direction = (boss.gameObject.transform.position - Target.position).normalized;

        // ���݂̃|�W�V��������direction�A�����̃X�s�[�h���|�����ꏊ��target�Ɋi�[���܂��B
        Vector3 target = boss.transform.position + direction * boss.AI.Agent.angularSpeed * Time.deltaTime * 20;

        // target�̏ꏊ�Ɉړ����܂��B
        Agent.SetDestination(target);
    }

    /// <summary>
    /// �^�[�Q�b�g�̕����������܂��B
    /// </summary>
    /// <param name="boss"></param>
    public void LookTarget(Boss boss)
    {
        // �����̃|�W�V��������^�[�Q�b�g�Ƀ|�W�V�����������Đ��K�����邱�Ƃŕ�����direction�Ɋi�[���܂��B
        Vector3 direction = (boss.transform.position - Target.position).normalized;

        // �㉺�̉�]��h�����߂�y��0�ɂ��܂��B
        direction.y = 0f;

        // direction��0�ł͂Ȃ��Ȃ�
        if (direction != Vector3.zero)
        {
            // Quarternion��LookRotation�֐����g���Č���������rotation�Ɋi�[���܂��B�idirection��targer����boss�Ɍ�������̂Ȃ̂�-direction�Ƃ��Ă���j
            Quaternion rotation = Quaternion.LookRotation(-direction);

            // �{�X�̃��[�e�[�V���������炩��rotation�̕����Ɍ������܂��B
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, rotation, Time.deltaTime * 10);
        }
    }

    /// <summary>
    /// �^�[�Q�b�g�Ƃ̋������擾���܂��B
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public float GetTargetDistance(Vector3 pos)
    {
        return Vector3.Distance(pos, Target.position);
    }
}
