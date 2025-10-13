using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    public List<LevelObject> levelInfos = new List<LevelObject>();

    [Header("Upgrades")]
    public float bonusBps;
    public float bonusRange;
    public float bonusDamage;
    public float bonusCritChance;
    public float bonusCritDamage;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
