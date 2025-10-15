using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Levels")]
    public List<LevelObject> Levels = new List<LevelObject>();
    public LevelObject selectedLevel;
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
    }

    public void SelectLevel(LevelObject level)
    {
        if(level == null)
        {
            Debug.Log("level is null ");
        }
        selectedLevel = level;
        SceneManager.LoadScene("Level_" + level.levelNumber);
    }

}
