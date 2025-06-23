using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }         // 自身をシングルトン化します。

    public DungeonGenerator DungeonGenerator { get; private set; }    // DungeonGeneratorを参照します。
    private EnemySpawner m_spawner;                                     // EnemySpawnerを参照します。

    [SerializeField] GameObject playerPrefab;                           // PlayerPrefabを取得します。

    public GameObject Player { get; private set; }                      // PlayerObjentを公開します。

    private FloorData m_floorData;                                      // 階層の情報を格納します。

    public int EnemyDefeatedCount { get; private set; }                 // エネミーを倒した数を格納し、公開します。
    public int EnemyEscapeCount { get; private set; }                   // エネミーから逃げた回数を格納し、公開します。

    public bool IsDungeonGenerated { get; private set; }

    public FloorData FloorData { get { return m_floorData; } }

    /// <summary>
    /// シングルトン化・初期化を行います。
    /// </summary>
    private void Awake()
    {
        Debug.Log("DungeonManagerのAwakeが呼ばれた！ Instanceを設定する");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // すでにインスタンスが存在する場合は破棄
        }

        DungeonGenerator = GetComponent<DungeonGenerator>();
        m_spawner = GetComponent<EnemySpawner>();

        IsDungeonGenerated = false;
    }

    /// <summary>
    /// このメソッドを呼び出すことで自動でダンジョン・エネミー・プレイヤーの生成を行います。（コルーチン版）
    /// </summary>
    /// <param name="floorData">階層の情報</param>
    public IEnumerator CreateDungeon(FloorData floorData)
    {
        // 初期化
        Init();
        yield return null; // 1フレーム待機

        // 階層の情報を格納します。
        m_floorData = floorData;

        // ダンジョンを生成します。（時間がかかる可能性があるのでコルーチンを使う）
        yield return StartCoroutine(DungeonGenerator.GenerateDungeonCoroutine(floorData));

        // エネミーをオブジェクトプールに生成します。
        m_spawner.CreateEnemyPool(floorData);
        yield return null; // 1フレーム待機

        // プレイヤーを生成します。
        Player = Instantiate(playerPrefab, DungeonGenerator.playerSpawnPosition, Quaternion.identity);
        PlayerInput playerInput = Player.GetComponent<PlayerInput>();
        playerInput.enabled = false;

        // 生成完了フラグを立てる
        IsDungeonGenerated = true;
        playerInput.enabled = true;
    }

    /// <summary>
    /// 毎フレーム行う更新処理
    /// </summary>
    private void Update()
    {
        // 敵の出現を更新します。
        m_spawner.UpdateEnemyGenerate(this, m_floorData);
    }

    /// <summary>
    /// エネミーを倒した数を1だけ増やします。
    /// </summary>
    public void AddEnemyDefeatedCount()
    {
        EnemyDefeatedCount++;
    }

    /// <summary>
    /// エネミーから逃げた数を1だけ増やします。
    /// </summary>
    public void AddEnemyEscapeCount()
    {
        EnemyEscapeCount++;
    }

    /// <summary>
    /// 初期化を行います。
    /// </summary>
    private void Init()
    {
        EnemyDefeatedCount = 0;
        EnemyEscapeCount = 0;
    }
}
