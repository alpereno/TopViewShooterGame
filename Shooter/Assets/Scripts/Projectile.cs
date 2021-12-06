using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float bulletSpeed = 10f;
    float lifeTime = 3;
    float damage = 1;

    public void setBulletSpeed(float newBulletSpeed) {
        bulletSpeed = newBulletSpeed;
    }

    //set bullet damage? maybe weapon variation...
    //idk if it's the right place...
    //you can do research...

    private void Start()
    {
        Destroy(gameObject, lifeTime);

        //RETURNS : Collider[] Returns an array with all colliders touching or inside the sphere.
        //DESCRIPTION : Computes and stores colliders touching or inside the sphere.
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            onHitObject(initialCollisions[0]);
        }
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

    void onHitObject(Collider collider) 
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
