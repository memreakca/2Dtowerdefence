using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet main;

    [Header("Attributes")]
    [SerializeField] private float bulletDamage = 3f;
    [SerializeField] private float maxBulletdamage = 7f;
    [SerializeField] private float UpgradeCost = 5000f;

    private bool isDone;
    public static float extraDamage;
    

    public void UpgradeBulletDamage()
    {
        if (extraDamage + bulletDamage == maxBulletdamage)
            return;// Debug.Log(" MAX bullet Damage");
        else
        {
            if (LevelManager.main.currency >= UpgradeCost)
            {
                LevelManager.main.currency = LevelManager.main.currency - UpgradeCost;
                extraDamage++;

            }
            else
                return;//Debug.Log("Not Enogh Money");
        }

    }


    private void OnCollisionEnter2D(Collision2D collision2D)
    {//take hp from enemy
        if (isDone)
        {
            return;
        }
        isDone = true;
        collision2D.gameObject.GetComponent<Health>().TakeDamage(extraDamage+bulletDamage);
        Destroy(gameObject);
    }
}
