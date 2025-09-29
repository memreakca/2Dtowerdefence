using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecomancerBoss : EnemyMovement
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] summonPoints;
    
    protected override void OnPathIndexChanged()
    {
        animator.SetBool("isMoving",false);
        animator.SetTrigger("Summon");
        moveSpeed = 0f;     
    }

    public void SummonEnemy()
    {
        foreach (Transform t in summonPoints)
        {
            var spawnedEnemy = Instantiate(enemyPrefab,t.position,Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyMovement>().pathIndex = this.pathIndex;
        }

        Invoke("ResetSpeed", 2f);
    }

    
}
