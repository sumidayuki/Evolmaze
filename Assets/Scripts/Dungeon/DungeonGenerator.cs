using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System.Collections;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject FloorPrefab;            // FloorPrefabを取得します。
    [SerializeField] GameObject RoofPrefab;             // RoofPrefabを取得します。
    [SerializeField] GameObject WallPrefab;             // WallPrefabを取得します。
    [SerializeField] GameObject BossRoomPrefab;
    [SerializeField] NavMeshSurface NavMeshSurface;     // NavMeshSurfaceを取得します。

    GameObject bossRoom;

    private float m_cellSize = 4;       // グリッドの1マスの大きさを表します。

    private float m_minSize = 10;         // エリアの最小サイズ
    private float m_minRoomSize = 5;      // 部屋の最小サイズ
    private float m_maxRoomSize = 15;     // 部屋の最大サイズ

    private BSPNode root;
    private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // 床を配置する場所を格納します。
    public Vector3 playerSpawnPosition { private set; get; }                // プレイヤーのスポーン位置

    private Rect playerSpawnRoom;

    public List<Rect> Rooms { private set; get; }       // Roomsの範囲を格納します。
    public List<BSPNode> Nodes { get; private set; }    // それぞれの部屋の情報を格納します。

    /// <summary>
    /// このメソッドを呼び出すことで、ダンジョンの生成を行います。（コルーチン版）
    /// </summary>
    /// <param name="floorData">階層の情報</param>
    public IEnumerator GenerateDungeonCoroutine(FloorData floorData)
    {
        Rooms = new List<Rect>();
        Nodes = new List<BSPNode>();
        yield return null; // 1フレーム待機（画面が固まるのを防ぐ）

        // ルートノードを作成
        root = new BSPNode(new Rect(0, 0, floorData.gridWidth, floorData.gridHeight));
        yield return null;

        // BSP分割
        SplitNode(root);
        yield return null;

        // 部屋の作成
        root.CreateRoom(m_minRoomSize, m_maxRoomSize);
        yield return null;

        // 部屋を配置
        CreateRooms(root);
        yield return null;

        // 通路を作成
        CreateCorridors();
        yield return null;

        // ボス部屋を生成
        GenerateBossRoom(floorData);
        yield return null;

        // 壁を配置
        PlaceWalls();
        yield return null;

        // NavMeshを生成
        NavMeshSurface.BuildNavMesh();
        yield return null;

        // 部屋を初期化（敵のスポーン数など）
        InitializeRooms(floorData.spawnCount);
        yield return null;

        Debug.Log("ダンジョン生成完了！");
    }

    /// <summary>
    /// プレイヤーのスポーン地点を取得します。
    /// </summary>
    /// <returns></returns>
    public Vector2Int SetPlayerSpawn()
    {
        List<Rect> rooms = CollectRooms(root);

        // ランダムな部屋を選ぶ
        Rect randomRoom = rooms[Random.Range(0, rooms.Count)];
        Vector2Int center = GetCenter(randomRoom);
        playerSpawnPosition = new Vector3(center.x * m_cellSize, 1, center.y * m_cellSize);
        playerSpawnRoom = randomRoom;
        return center;
    }

    /// <summary>
    /// 部屋の中心を取得します。
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public Vector2Int GetCenter(Rect rect)
    {
        int centerX = Mathf.FloorToInt(rect.xMin + rect.width / 2);
        int centerY = Mathf.FloorToInt(rect.yMin + rect.height / 2);
        return new Vector2Int(centerX, centerY);
    }

    /// <summary>
    /// nodeの分割を行います。
    /// </summary>
    /// <param name="node"></param>
    void SplitNode(BSPNode node)
    {
        if (node.IsLeaf && (node.Area.width > m_minSize || node.Area.height > m_minSize))
        {
            node.Split(m_minSize);
            if (node.Left != null) SplitNode(node.Left);
            if (node.Right != null) SplitNode(node.Right);
        }
    }

    /// <summary>
    /// 部屋を生成します。
    /// </summary>
    /// <param name="node"></param>
    void CreateRooms(BSPNode node)
    {
        if (node == null) return;

        if (node.IsLeaf && node.Room.HasValue)
        {
            var room = node.Room.Value;
            int startX = Mathf.FloorToInt(room.xMin);
            int startY = Mathf.FloorToInt(room.yMin);
            int width = Mathf.FloorToInt(room.width);
            int height = Mathf.FloorToInt(room.height);

            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    PlaceFloorAndRoof(position);
                }
            }

            Rooms.Add(room);  // 部屋をリストに追加
        }
        else
        {
            CreateRooms(node.Left);
            CreateRooms(node.Right);
        }
    }

    /// <summary>
    /// 通路をつなぐ部屋と部屋を取得します。
    /// </summary>
    void CreateCorridors()
    {
        List<Rect> rooms = CollectRooms(root);

        // 通路の生成ループ
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int start = GetCenter(rooms[i]);  // 現在の部屋の中心
            Vector2Int end = GetCenter(rooms[i + 1]); // 次の部屋の中心

            // 通路の生成
            CreateCorridor(start, end);
        }
    }

    /// <summary>
    /// 通路を生成します。
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="roomNumber"></param>
    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int current = start;
        List<Vector2Int> corridorPath = new List<Vector2Int>();

        // 通路を生成（X方向優先）
        while (current.x != end.x)
        {
            corridorPath.Add(current);
            current.x += current.x < end.x ? 1 : -1; // X軸で進む
        }

        // Y方向に進む
        while (current.y != end.y)
        {
            corridorPath.Add(current);
            current.y += current.y < end.y ? 1 : -1; // Y軸で進む
        }

        // 通路が作成されている場合
        if (corridorPath.Count > 0)
        {
            // 通路を床として配置
            foreach (var position in corridorPath)
            {
                PlaceFloorAndRoof(position);
            }
        }
    }

    /// <summary>
    /// 床と屋根を生成します。
    /// </summary>
    /// <param name="position"></param>
    void PlaceFloorAndRoof(Vector2Int position)
    {
        if (!floorPositions.Contains(position))
        {
            floorPositions.Add(position);
            Vector3 worldPosition = new Vector3(position.x * m_cellSize, 0, position.y * m_cellSize);
            Instantiate(FloorPrefab, worldPosition, Quaternion.identity, transform);

            Vector3 roofPosition = new Vector3(position.x * m_cellSize, 4, position.y * m_cellSize);
            Instantiate(RoofPrefab, roofPosition, Quaternion.identity, transform);
        }
    }

    /// <summary>
    /// 壁を生成します。
    /// </summary>
    void PlaceWalls()
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var floorPosition in floorPositions)
        {
            foreach (var direction in GetNeighbors())
            {
                Vector2Int neighbor = floorPosition + direction;
                if (!floorPositions.Contains(neighbor) && !wallPositions.Contains(neighbor))
                {
                    wallPositions.Add(neighbor);
                    Vector3 worldPosition = new Vector3(neighbor.x * m_cellSize, 2, neighbor.y * m_cellSize);

                    Vector2Int entrancePosition = new Vector2Int(
                    Mathf.RoundToInt(bossRoom.transform.position.x / m_cellSize),
                    Mathf.RoundToInt(bossRoom.transform.position.z / m_cellSize)
                    );

                    if (entrancePosition != neighbor)
                    {
                        Instantiate(WallPrefab, worldPosition, Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 上・下・右・左を取得します。
    /// </summary>
    /// <returns></returns>
    Vector2Int[] GetNeighbors()
    {
        return new Vector2Int[] {
            new Vector2Int(0, 1),  // 上
            new Vector2Int(0, -1), // 下
            new Vector2Int(1, 0),  // 右
            new Vector2Int(-1, 0)  // 左
        };
    }

    /// <summary>
    /// ルームを取得します。
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    List<Rect> CollectRooms(BSPNode node)
    {
        List<Rect> roomList = new List<Rect>();
        if (node == null) return roomList;

        if (node.IsLeaf && node.Room.HasValue)
        {
            roomList.Add(node.Room.Value);
        }
        else
        {
            roomList.AddRange(CollectRooms(node.Left));
            roomList.AddRange(CollectRooms(node.Right));
        }

        return roomList;
    }

    /// <summary>
    /// 全部屋の初期化を行います。
    /// 敵の生成位置を部屋に渡したりなど
    /// </summary>
    /// <param name="spawnCount">部屋の敵生成数</param>
    void InitializeRooms(int spawnCount)
    {
        foreach (var bounds in Rooms)
        {
            // プレイヤーの初期生成部屋ならスキップ
            if (bounds == playerSpawnRoom)
            {
                continue;
            }

            BSPNode node = new BSPNode(bounds);
            node.GenerateEnemySpawnPos(spawnCount, 2 * 4);

            Nodes.Add(node);
        }
    }

    // Listをシャッフルするユーティリティ（Fisher–Yatesアルゴリズム）
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }

    public void GenerateBossRoom(FloorData floorData)
    {
        List<Rect> rooms = CollectRooms(root);

        // プレイヤーの開始位置（最初の部屋の中心）
        Vector2Int startRoom = SetPlayerSpawn();

        // 一番遠い部屋を探す
        Rect farthestRoom = rooms[0];
        float maxDistance = 0f;

        foreach (Rect room in rooms)
        {
            Vector2Int center = GetCenter(room);
            float distance = Vector2Int.Distance(startRoom, center);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestRoom = room;
            }
        }

        Vector2Int target = GetCenter(farthestRoom);

        // ランダムな方向から周囲を試す（ランダムに回転させる）
        var directions = new List<Vector2Int>(GetNeighbors());
        Shuffle(directions); // 下で定義するユーティリティ

        foreach (var direction in directions)
        {
            Vector2Int neighbor = target + direction * 15;

            if (!floorPositions.Contains(neighbor)) // 空いてるなら設置
            {
                bossRoom = Instantiate(
                    BossRoomPrefab,
                    new Vector3(neighbor.x * m_cellSize, 0, neighbor.y * m_cellSize),
                    Quaternion.Euler(0, 180, 0)
                );

                BossRoom bossRoomScript = bossRoom.GetComponent<BossRoom>();
                bossRoomScript.SetBoss(floorData.bossData);

                Vector2Int entrancePosition = new Vector2Int(
                    Mathf.RoundToInt(bossRoom.transform.position.x / m_cellSize),
                    Mathf.RoundToInt(bossRoom.transform.position.z / m_cellSize)
                );
                CreateCorridor(target, entrancePosition);
                return;
            }
        }
    }

    // Gizmosを使って各部屋の中心に球体を表示
    private void OnDrawGizmos()
    {
        if (Rooms == null || Rooms.Count == 0) return;

        for (int i = 0; i < Rooms.Count; i++)
        {
            // 部屋の中心を計算
            Vector2Int center = GetCenter(Rooms[i]);

            // 部屋ごとに色を設定（部屋のインデックスに基づいて色を変更）
            Gizmos.color = new Color(i / (float)Rooms.Count, 0, 1 - i / (float)Rooms.Count);

            // 部屋の中心に球体を描画
            Gizmos.DrawSphere(new Vector3(center.x * m_cellSize, 0.5f, center.y * m_cellSize), 10f); // 球体の半径は0.5fに設定
        }
    }
}
