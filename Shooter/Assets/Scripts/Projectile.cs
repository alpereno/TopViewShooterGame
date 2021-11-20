using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float bulletSpeed = 10f;
    float damage = 1;

    public void setBulletSpeed(float newBulletSpeed) {
        bulletSpeed = newBulletSpeed;
    }

    void Update()
    {
        float moveDistance = bulletSpeed * Time.deltaTime;
        checkCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void checkCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            onHitObject(hit);
        }
    }

    private void onHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.takeHit(damage, hit);
        }
        Destroy(gameObject);
    }
}
