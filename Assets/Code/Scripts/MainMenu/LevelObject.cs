using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public int levelNumber;

    public GameObject star1Object;
    public GameObject star2Object;
    public GameObject star3Object;

    public bool star1Complete;
    public bool star2Complete;
    public bool star3Complete;

    private void OnEnable()
    {
        if (star3Complete)
        {
            star1Object.SetActive(true);
            star2Object.SetActive(true);
            star3Object.SetActive(true);
            return;
        }

        if (star2Complete)
        {
            star1Object.SetActive(true);
            star2Object.SetActive(true);
            star3Object.SetActive(false);
            return;
        }

        if (star1Complete)
        {
            star1Object.SetActive(true);
            star2Object.SetActive(false);
            star3Object.SetActive(false);
            return;
        }

        star1Object.SetActive(false);
        star2Object.SetActive(false);
        star3Object.SetActive(false);
    }
}
