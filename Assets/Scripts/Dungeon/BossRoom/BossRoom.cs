using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BossRoom : MonoBehaviour
{
    GameObject boss;
    [SerializeField] GameObject[] lights;
    [SerializeField] GameObject entrance;
    [SerializeField] Transform bossPos;
    [SerializeField] GameObject startWall;
    [SerializeField] GameObject endWall;

    private EnemyData m_bossData;
    private StartLine m_startLine;
    private EndLine m_endLine;
    private Boss m_bossScript;

    private void Awake()
    {
        m_startLine = GetComponentInChildren<StartLine>();
        m_endLine = GetComponentInChildren<EndLine>();

        // lightsを全て非表示
        for(int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(false);
        }

        startWall.gameObject.SetActive(false);
        endWall.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (m_startLine.IsStart)
        {
            if (!startWall.activeSelf)
            {
                // NavMesh上の最も近い位置を検索
                if (NavMesh.SamplePosition(bossPos.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    boss = Instantiate(m_bossData.enemyPrefab, hit.position, new Quaternion(0, 180, 0, 0));

                }
                else
                {
                    Debug.LogError("ターゲットがNavMeshの範囲外です！");
                }
                m_bossScript = boss.GetComponent<Boss>();
                m_bossScript.AI.SetTatget(m_startLine.Player.transform);
                StartCoroutine(OnStart());
            }
        }

        if (boss == null) return;

        if (m_bossScript.IsDead)
        {
            lights[lights.Length - 1].gameObject.SetActive(true);
            endWall.SetActive(false);
            if(m_endLine.IsEnd)
            {
                GameManager.Instance.GameClear();
            }
        }
    }

    public void SetBoss(EnemyData bossData)
    {
        m_bossData = bossData;
    }

    /// <summary>
    /// 入り口を塞ぎ、
    /// ライトを少しずつ表示していきます。
    /// </summary>
    /// <returns></returns>
    IEnumerator OnStart()
    {
        startWall.gameObject.SetActive(true);

        for(int i = 0; i < lights.Length - 1; i++)
        {
            lights[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
