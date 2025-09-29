using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroSkeleton : EnemyMovement
{
    protected override void OnStartFunction()
    {
        moveSpeed = 0;
        Invoke("ResetSpeed", 1.2f);
    }

}
