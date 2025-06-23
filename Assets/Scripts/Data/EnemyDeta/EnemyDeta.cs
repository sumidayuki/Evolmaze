using UnityEngine;

public enum EnemyType
{
    Normal,
    Aggressive,
    Shield,
    Boss
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    public float health;
    public float attackDamage;
    public float moveSpeed;
    public EnemyType enemyType;
    public float prob;
}