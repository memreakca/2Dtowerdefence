using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 2.5f;
    private Transform target;

    private void Start()
    {
        Destroy(this.gameObject, 3f);
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {

        if (!target)
        {
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 85f;
        // Apply rotation to the bullet sprite
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = direction * bulletSpeed;

    }

}
