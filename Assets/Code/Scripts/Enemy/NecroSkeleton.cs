using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroSkeleton : EnemyAttributes
{
    protected override void OnStartFunction()
    {
        moveSpeed = 0;
        Invoke("ResetSpeed", 1.2f);
    }

}
