using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public LevelObject levelObject;
    [SerializeField] private GameObject[] starIcons;

    private void Start()
    {
        UpdateStars();
    }

    public void OnClick()
    {
        LevelManager.Instance.SelectLevel(levelObject);
    }

    private void UpdateStars()
    {
        for (int i = 0; i < starIcons.Length; i++)
            starIcons[i].SetActive(i < levelObject.earnedStars);
    }
}
