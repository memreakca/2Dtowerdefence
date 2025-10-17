using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("References")]
    [SerializeField] private UserManager userManager;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private List<UpgradeNode> allUpgradeNodes;
    [SerializeField] private List<LevelObject> allLevels;

    private const string SAVE_KEY = "GameSaveData";

    [System.Serializable]
    private class SaveData
    {
        public int unusedStars;
        public int starsGained;

        public float bonusBps;
        public float bonusRange;
        public float bonusDamage;
        public float bonusCritChance;
        public float bonusCritDamage;

        public List<bool> upgradePurchased;
        public List<int> levelStars;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        // User stats
        data.unusedStars = userManager.unusedStars;
        data.starsGained = userManager.starsGained;

        data.bonusBps = userManager.bonusBps;
        data.bonusRange = userManager.bonusRange;
        data.bonusDamage = userManager.bonusDamage;
        data.bonusCritChance = userManager.bonusCritChance;
        data.bonusCritDamage = userManager.bonusCritDamage;

        // Upgrades
        data.upgradePurchased = new List<bool>();
        foreach (var node in allUpgradeNodes)
        {
            data.upgradePurchased.Add(node.isPurchased);
        }

        // Levels
        data.levelStars = new List<int>();
        foreach (var level in allLevels)
        {
            data.levelStars.Add(level.earnedStars);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Load User data
        userManager.unusedStars = data.unusedStars;
        userManager.starsGained = data.starsGained;

        userManager.bonusBps = data.bonusBps;
        userManager.bonusRange = data.bonusRange;
        userManager.bonusDamage = data.bonusDamage;
        userManager.bonusCritChance = data.bonusCritChance;
        userManager.bonusCritDamage = data.bonusCritDamage;

        // Load Upgrade states
        for (int i = 0; i < allUpgradeNodes.Count; i++)
        {
            if (i < data.upgradePurchased.Count)
                allUpgradeNodes[i].isPurchased = data.upgradePurchased[i];
        }

        // Load Levels
        for (int i = 0; i < allLevels.Count; i++)
        {
            if (i < data.levelStars.Count)
                allLevels[i].earnedStars = data.levelStars[i];
        }
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
    }
}
