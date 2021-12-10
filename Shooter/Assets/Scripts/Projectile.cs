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
    //if bullet and enemy move in one frame when near the intersection raycast won't detect enemy couse ray is so small (movedistance) one frame
    //so I've increased the length of ray by a small number. if enemy movement speed will increase, increase it too
    float skinThickness = .1f;

    public void setBulletSpeed(float newBulletSpeed) {
        bulletSpeed = newBulletSpeed;
    }

    //set bullet damage(headshoot, body, leg etc.)? maybe weapon variation...
    //idk if it's the right place...
    //you can do research...

    private void Start()
    {
        Destroy(gameObject, lifeTime);

        //RETURNS : Collider[] Returns an array with all colliders touching or inside the sphere.
        //DESCRIPTION : Computes and stores colliders touching or inside the sphere. ----> Physics.OverlapSphere
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            onHitObject(initialCollisions[0]);
        }
    }

    void Update()
    {
        float moveDistancePerFrame = bulletSpeed * Time.deltaTime;
        checkCollisions(moveDistancePerFrame);
        transform.Translate(Vector3.forward * moveDistancePerFrame);
    }

    private void checkCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance+skinThickness, collisionMask, QueryTriggerInteraction.Collide))
        {
            onHitObject(hit);
        }
    }

    //normal shooting
    private void onHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.takeHit(damage, hit);
        }
        Destroy(gameObject);
    }

    //if enemy so so close to enemy, this func. will work. Projectile will instantiate in enemy's collider if "initialCollisions" is not null
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
