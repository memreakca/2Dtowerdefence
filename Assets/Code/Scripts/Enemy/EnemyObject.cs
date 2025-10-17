using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/New Enemy Object")]
public class EnemyObject : ScriptableObject
{
    [Header("Prefab")]
    public GameObject enemyPrefab;

    [Header("Stats")]
    public string enemyName;
    public float maxHp;
    public float moveSpeed;
    public float enemyDamage;
    public float attackCooldown;
    public int damageToBase;
    public int currencyWorth;

    [Header("Visuals")]
    public Sprite sprite;
}