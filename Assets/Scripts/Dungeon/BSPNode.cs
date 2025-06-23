using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BSPNode
{
    public Rect Area;          // このノードが管理する領域
    public BSPNode Left;       // 左の子ノード
    public BSPNode Right;      // 右の子ノード
    public Rect? Room;         // このノードに属する部屋（分割が終わった後に設定）
    public bool IsLeaf => Left == null && Right == null; // 葉ノードかどうか
    public List<Vector3> SpawnPoints;
    public List<Vector3> WayPoints;

    private const float SplitRatioMin = 0.3f; // 分割の最小比率
    private const float SplitRatioMax = 0.7f; // 分割の最大比率

    public BSPNode(Rect area)
    {
        Area = area;
        SpawnPoints = new List<Vector3>();
        WayPoints = new List<Vector3>();
    }

    // ノードを水平または垂直に分割する
    public void Split(float minSize)
    {
        // 既に分割済みの場合は何もしない
        if (!IsLeaf) return;

        bool splitHorizontally = Random.value > 0.5f; // 水平方向に分割するか
        if (Area.width > Area.height && Area.width / Area.height >= 1.25f)
        {
            splitHorizontally = false; // 幅が高さより大きければ垂直分割
        }
        else if (Area.height > Area.width && Area.height / Area.width >= 1.25f)
        {
            splitHorizontally = true; // 高さが幅より大きければ水平分割
        }

        // 分割可能なサイズかチェック
        if ((splitHorizontally && Area.height < minSize * 2) ||
            (!splitHorizontally && Area.width < minSize * 2))
        {
            return; // 分割不能
        }

        // 分割点をランダムに決定
        float splitRatio = Random.Range(SplitRatioMin, SplitRatioMax); // 30% ～ 70%の間
        if (splitHorizontally)
        {
            float splitY = Area.yMin + Area.height * splitRatio;
            Left = new BSPNode(new Rect(Area.xMin, Area.yMin, Area.width, splitY - Area.yMin));
            Right = new BSPNode(new Rect(Area.xMin, splitY, Area.width, Area.yMax - splitY));
        }
        else
        {
            float splitX = Area.xMin + Area.width * splitRatio;
            Left = new BSPNode(new Rect(Area.xMin, Area.yMin, splitX - Area.xMin, Area.height));
            Right = new BSPNode(new Rect(splitX, Area.yMin, Area.xMax - splitX, Area.height));
        }
    }

    // 部屋を生成する（葉ノード内にランダムなサイズで部屋を配置）
    public void CreateRoom(float minRoomSize, float maxRoomSize)
    {
        if (!IsLeaf) // 葉ノードでない場合は再帰的に呼び出す
        {
            Left?.CreateRoom(minRoomSize, maxRoomSize);
            Right?.CreateRoom(minRoomSize, maxRoomSize);
            return;
        }

        // 部屋をランダムな位置とサイズで作成
        float roomWidth = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, Area.width * 0.8f));
        float roomHeight = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, Area.height * 0.8f));

        float roomX = Random.Range(Area.xMin + (Area.width - roomWidth) / 2, Area.xMax - roomWidth);
        float roomY = Random.Range(Area.yMin + (Area.height - roomHeight) / 2, Area.yMax - roomHeight);

        Room = new Rect(roomX, roomY, roomWidth, roomHeight);
    }

    public void GenerateEnemySpawnPos(int numPoints, float interval)
    {
        // 最初の位置
        Vector3 currentPosition = new Vector3(Area.xMin + 1, 0, Area.yMin + 1) * 4;

        for (int i = 0; i < numPoints; i++)
        {
            // 次の位置を指定した間隔で移動
            Vector3 direction = (Random.insideUnitSphere * 2f).normalized; // ランダムな方向
            Vector3 newPosition = currentPosition + direction * interval; // 間隔分移動

            // NavMesh上の位置をサンプリング
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPosition, out hit, 1f, NavMesh.AllAreas))
            {
                // 有効なNavMesh位置が見つかった場合
                SpawnPoints.Add(hit.position);
                currentPosition = hit.position; // 新しい位置を設定
            }
            else
            {
                // NavMesh上に有効な位置が見つからない場合
                i--; // 失敗した場合は再度ループ
            }
        }
    }
}