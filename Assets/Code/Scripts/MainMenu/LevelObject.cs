using UnityEngine;

[CreateAssetMenu(menuName = "Level System/Level Data")]
public class LevelObject : ScriptableObject
{
    public int levelNumber;
    [Header("Star Status")]
    [Range(0, 3)] public int earnedStars;

    [Header("Star Requirements")]
    public float minimumCompleteTime;

}
