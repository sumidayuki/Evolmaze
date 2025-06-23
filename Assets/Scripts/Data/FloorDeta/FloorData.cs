using System.Collections.Generic;
using UnityEngine;

// 【CreateAssetMenuの引数説明】
//  第一引数：fileNameは生成されるときのファイルの名前
//  第二引数：menuNameはメニュー欄でどのような名前でどこに表示するかを決める
//  第三引数：orderはそのメニューの中で何番目に表示されるかを決める（数字が小さいほど上に表示される）
[CreateAssetMenu(fileName = "FloorData", menuName = "Game/FloorData", order = 2)]

public class FloorData : ScriptableObject
{
    public int floorNumber;
    public string floorName;
    public int spawnCount;
    public int gridWidth;
    public int gridHeight;
    public List<EnemyData> enemyDatas;
    public EnemyData bossData;
}
