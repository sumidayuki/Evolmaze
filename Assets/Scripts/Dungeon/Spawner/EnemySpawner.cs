using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    Dictionary<EnemyType, Queue<GameObject>> enemyPools = new Dictionary<EnemyType, Queue<GameObject>>();

    public static EnemySpawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // すでにインスタンスが存在する場合は破棄
        }
    }

    /// <summary>
    /// エネミーのオブジェクトプールを作成します。
    /// </summary>
    /// <param name="floorDeta"></param>
    public void CreateEnemyPool(FloorData floorData)
    {
        foreach (var data in floorData.enemyDatas)
        {
            if (!enemyPools.ContainsKey(data.enemyType))
            {
                enemyPools[data.enemyType] = new Queue<GameObject>();
            }

            for (int i = 0; i < floorData.spawnCount * 3; i++)
            {
                GameObject enemy = Instantiate(data.enemyPrefab, new Vector3(0, 200, 0), Quaternion.identity);
                enemy.SetActive(false);
                enemyPools[data.enemyType].Enqueue(enemy);
            }
        }
    }

    /// <summary>
    /// NavMeshAgentをリセットします。
    /// </summary>
    /// <param name="agent"></param>
    void ResetNavMeshAgent(NavMeshAgent agent)
    {
        agent.enabled = false;
        agent.enabled = true;
    }

    /// <summary>
    /// 指定したタイプのエネミーを取得します。
    /// </summary>
    /// <param name="floorData"></param>
    /// <returns></returns>
    GameObject GetEnemy(FloorData floorData, EnemyType type)
    {
        if (enemyPools.ContainsKey(type) && enemyPools[type].Count > 0)
        {
            GameObject enemy = enemyPools[type].Dequeue();
            enemy.SetActive(true);
            return enemy;
        }

        // プールにない場合は新規生成（非常時）
        var prefab = floorData.enemyDatas.Find(e => e.enemyType == type).enemyPrefab;
        return Instantiate(prefab, new Vector3(0, 200, 0), Quaternion.identity);
    }

    /// <summary>
    /// エネミーをオブジェクトプールに戻します。
    /// </summary>
    /// <param name="enemy"></param>
    public void ReturnEnemy(GameObject enemy, EnemyType type)
    {
        enemy.SetActive(false);
        if (!enemyPools.ContainsKey(type))
        {
            enemyPools[type] = new Queue<GameObject>();
        }
        enemyPools[type].Enqueue(enemy);
    }
    /// <summary>
    /// 確率に基づいて、エネミーのタイプを選択します。
    /// </summary>
    /// <param name="dungeonManager"></param>
    /// <param name="floorDeta"></param>
    /// <returns></returns>
    GameObject SelectEnemy(DungeonManager dungeonManager, FloorData floorData)
    {
        float value = 0;
        foreach(var enemy in floorData.enemyDatas)
        {
            value += enemy.prob; 
        }

        float ramdomValue = Random.Range(0, value);

        EnemyType type = GetEnemyType(floorData, ramdomValue);

        return GetEnemy(floorData, type);
    }

    /// <summary>
    /// 確率を取得します。
    /// </summary>
    /// <param name="dungeonManager"></param>
    /// <returns></returns>
    EnemyType GetEnemyType(FloorData floorData, float value)
    {
        foreach(var enemy in floorData.enemyDatas)
        {
            if(enemy.prob > value)
            {
                return enemy.enemyType;
            }
        }

        return EnemyType.Normal;
    }

    /// <summary>
    /// エネミーの生成を管理します。
    /// このメソッドをUpdate関数で更新することでエネミーの生成・削除等を行います。
    /// </summary>
    /// <param name="dungeonManager"></param>
    /// <param name="floorDeta"></param>
    public void UpdateEnemyGenerate(DungeonManager dungeonManager, FloorData floorData)
    {
        if (dungeonManager.Player == null) return;

        foreach (var node in dungeonManager.DungeonGenerator.Nodes)
        {
            float distance = Vector3.Distance(dungeonManager.Player.transform.position,
                new Vector3(dungeonManager.DungeonGenerator.GetCenter(node.Area).x * 4, 0, dungeonManager.DungeonGenerator.GetCenter(node.Area).y * 4));

            if (distance < 30f)
            {
                // スポーン済みか確認
                if (!activeEnemies.ContainsKey(node) || activeEnemies[node].Count == 0)
                {
                    SpawnEnemiesInRoom(node, dungeonManager, floorData);
                }
            }
            else if (distance < 35f)
            {
                DeactivateEnemiesInRoom(node, dungeonManager, floorData);
            }
        }
    }

    Dictionary<BSPNode, List<GameObject>> activeEnemies = new Dictionary<BSPNode, List<GameObject>>();

    /// <summary>
    /// 部屋の中に敵を生成します。
    /// </summary>
    /// <param name="room">部屋の情報</param>
    /// <param name="dungeonManager"></param>
    /// <param name="floorDeta"></param>
    void SpawnEnemiesInRoom(BSPNode room, DungeonManager dungeonManager, FloorData floorData)
    {
        if (!activeEnemies.ContainsKey(room))
        {
            activeEnemies[room] = new List<GameObject>();
        }

        foreach (var spawnPoint in room.SpawnPoints)
        {
            GameObject enemy = SelectEnemy(dungeonManager, floorData);
            enemy.transform.position = spawnPoint;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.Room = room.Area;
            ResetNavMeshAgent(enemy.GetComponent<NavMeshAgent>());
            activeEnemies[room].Add(enemy);
        }
    }

    /// <summary>
    /// 部屋の中のエネミーを全て削除します。
    /// </summary>
    /// <param name="room"></param>
    /// <param name="dungeonManager"></param>
    void DeactivateEnemiesInRoom(BSPNode room, DungeonManager dungeonManager,  FloorData floorData)
    {
        if (activeEnemies.ContainsKey(room))
        {
            if (activeEnemies[room].Count == 0) return;
            foreach (var enemy in activeEnemies[room])
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();

                ReturnEnemy(enemy, enemyScript.GetEnemyType);

                if (enemyScript.Camera.GetPlayerPos() != null)
                {
                    foreach(var enemyDeta in floorData.enemyDatas)
                    {
                        dungeonManager.AddEnemyEscapeCount();

                        if(enemyDeta.enemyType == EnemyType.Aggressive)
                        {
                            enemyDeta.prob++;
                        }
                    }
                }
            }

            Debug.Log("部屋にいる敵を消去しました。");
            activeEnemies[room].Clear();
        }
    }
}
