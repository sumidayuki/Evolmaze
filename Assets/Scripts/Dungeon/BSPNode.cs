using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BSPNode
{
    public Rect Area;          // ���̃m�[�h���Ǘ�����̈�
    public BSPNode Left;       // ���̎q�m�[�h
    public BSPNode Right;      // �E�̎q�m�[�h
    public Rect? Room;         // ���̃m�[�h�ɑ����镔���i�������I�������ɐݒ�j
    public bool IsLeaf => Left == null && Right == null; // �t�m�[�h���ǂ���
    public List<Vector3> SpawnPoints;
    public List<Vector3> WayPoints;

    private const float SplitRatioMin = 0.3f; // �����̍ŏ��䗦
    private const float SplitRatioMax = 0.7f; // �����̍ő�䗦

    public BSPNode(Rect area)
    {
        Area = area;
        SpawnPoints = new List<Vector3>();
        WayPoints = new List<Vector3>();
    }

    // �m�[�h�𐅕��܂��͐����ɕ�������
    public void Split(float minSize)
    {
        // ���ɕ����ς݂̏ꍇ�͉������Ȃ�
        if (!IsLeaf) return;

        bool splitHorizontally = Random.value > 0.5f; // ���������ɕ������邩
        if (Area.width > Area.height && Area.width / Area.height >= 1.25f)
        {
            splitHorizontally = false; // �����������傫����ΐ�������
        }
        else if (Area.height > Area.width && Area.height / Area.width >= 1.25f)
        {
            splitHorizontally = true; // �����������傫����ΐ�������
        }

        // �����\�ȃT�C�Y���`�F�b�N
        if ((splitHorizontally && Area.height < minSize * 2) ||
            (!splitHorizontally && Area.width < minSize * 2))
        {
            return; // �����s�\
        }

        // �����_�������_���Ɍ���
        float splitRatio = Random.Range(SplitRatioMin, SplitRatioMax); // 30% �` 70%�̊�
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

    // �����𐶐�����i�t�m�[�h���Ƀ����_���ȃT�C�Y�ŕ�����z�u�j
    public void CreateRoom(float minRoomSize, float maxRoomSize)
    {
        if (!IsLeaf) // �t�m�[�h�łȂ��ꍇ�͍ċA�I�ɌĂяo��
        {
            Left?.CreateRoom(minRoomSize, maxRoomSize);
            Right?.CreateRoom(minRoomSize, maxRoomSize);
            return;
        }

        // �����������_���Ȉʒu�ƃT�C�Y�ō쐬
        float roomWidth = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, Area.width * 0.8f));
        float roomHeight = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, Area.height * 0.8f));

        float roomX = Random.Range(Area.xMin + (Area.width - roomWidth) / 2, Area.xMax - roomWidth);
        float roomY = Random.Range(Area.yMin + (Area.height - roomHeight) / 2, Area.yMax - roomHeight);

        Room = new Rect(roomX, roomY, roomWidth, roomHeight);
    }

    public void GenerateEnemySpawnPos(int numPoints, float interval)
    {
        // �ŏ��̈ʒu
        Vector3 currentPosition = new Vector3(Area.xMin + 1, 0, Area.yMin + 1) * 4;

        for (int i = 0; i < numPoints; i++)
        {
            // ���̈ʒu���w�肵���Ԋu�ňړ�
            Vector3 direction = (Random.insideUnitSphere * 2f).normalized; // �����_���ȕ���
            Vector3 newPosition = currentPosition + direction * interval; // �Ԋu���ړ�

            // NavMesh��̈ʒu���T���v�����O
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPosition, out hit, 1f, NavMesh.AllAreas))
            {
                // �L����NavMesh�ʒu�����������ꍇ
                SpawnPoints.Add(hit.position);
                currentPosition = hit.position; // �V�����ʒu��ݒ�
            }
            else
            {
                // NavMesh��ɗL���Ȉʒu��������Ȃ��ꍇ
                i--; // ���s�����ꍇ�͍ēx���[�v
            }
        }
    }
}