using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }         // ���g���V���O���g�������܂��B

    public DungeonGenerator DungeonGenerator { get; private set; }    // DungeonGenerator���Q�Ƃ��܂��B
    private EnemySpawner m_spawner;                                     // EnemySpawner���Q�Ƃ��܂��B

    [SerializeField] GameObject playerPrefab;                           // PlayerPrefab���擾���܂��B

    public GameObject Player { get; private set; }                      // PlayerObjent�����J���܂��B

    private FloorData m_floorData;                                      // �K�w�̏����i�[���܂��B

    public int EnemyDefeatedCount { get; private set; }                 // �G�l�~�[��|���������i�[���A���J���܂��B
    public int EnemyEscapeCount { get; private set; }                   // �G�l�~�[���瓦�����񐔂��i�[���A���J���܂��B

    public bool IsDungeonGenerated { get; private set; }

    public FloorData FloorData { get { return m_floorData; } }

    /// <summary>
    /// �V���O���g�����E���������s���܂��B
    /// </summary>
    private void Awake()
    {
        Debug.Log("DungeonManager��Awake���Ă΂ꂽ�I Instance��ݒ肷��");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ���łɃC���X�^���X�����݂���ꍇ�͔j��
        }

        DungeonGenerator = GetComponent<DungeonGenerator>();
        m_spawner = GetComponent<EnemySpawner>();

        IsDungeonGenerated = false;
    }

    /// <summary>
    /// ���̃��\�b�h���Ăяo�����ƂŎ����Ń_���W�����E�G�l�~�[�E�v���C���[�̐������s���܂��B�i�R���[�`���Łj
    /// </summary>
    /// <param name="floorData">�K�w�̏��</param>
    public IEnumerator CreateDungeon(FloorData floorData)
    {
        // ������
        Init();
        yield return null; // 1�t���[���ҋ@

        // �K�w�̏����i�[���܂��B
        m_floorData = floorData;

        // �_���W�����𐶐����܂��B�i���Ԃ�������\��������̂ŃR���[�`�����g���j
        yield return StartCoroutine(DungeonGenerator.GenerateDungeonCoroutine(floorData));

        // �G�l�~�[���I�u�W�F�N�g�v�[���ɐ������܂��B
        m_spawner.CreateEnemyPool(floorData);
        yield return null; // 1�t���[���ҋ@

        // �v���C���[�𐶐����܂��B
        Player = Instantiate(playerPrefab, DungeonGenerator.playerSpawnPosition, Quaternion.identity);
        PlayerInput playerInput = Player.GetComponent<PlayerInput>();
        playerInput.enabled = false;

        // ���������t���O�𗧂Ă�
        IsDungeonGenerated = true;
        playerInput.enabled = true;
    }

    /// <summary>
    /// ���t���[���s���X�V����
    /// </summary>
    private void Update()
    {
        // �G�̏o�����X�V���܂��B
        m_spawner.UpdateEnemyGenerate(this, m_floorData);
    }

    /// <summary>
    /// �G�l�~�[��|��������1�������₵�܂��B
    /// </summary>
    public void AddEnemyDefeatedCount()
    {
        EnemyDefeatedCount++;
    }

    /// <summary>
    /// �G�l�~�[���瓦��������1�������₵�܂��B
    /// </summary>
    public void AddEnemyEscapeCount()
    {
        EnemyEscapeCount++;
    }

    /// <summary>
    /// ���������s���܂��B
    /// </summary>
    private void Init()
    {
        EnemyDefeatedCount = 0;
        EnemyEscapeCount = 0;
    }
}
