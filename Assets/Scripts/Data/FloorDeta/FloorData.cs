using System.Collections.Generic;
using UnityEngine;

// �yCreateAssetMenu�̈��������z
//  �������FfileName�͐��������Ƃ��̃t�@�C���̖��O
//  �������FmenuName�̓��j���[���łǂ̂悤�Ȗ��O�łǂ��ɕ\�����邩�����߂�
//  ��O�����Forder�͂��̃��j���[�̒��ŉ��Ԗڂɕ\������邩�����߂�i�������������قǏ�ɕ\�������j
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
