using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Color trailColor;
    float lifeTime = 3;
    float damage = 1;
    //if bullet and enemy move in one frame when near the intersection raycast won't detect enemy cause ray is so small (movedistance) one frame
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
        //if enemy so so close to enemy, this func. will work. Projectile will instantiate in enemy's collider if "initialCollisions" is not null
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            //actually hit point is current pos. this projectile
            onHitEnemy(initialCollisions[0], transform.position);
        }
        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
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
            //onHitEnemy(hit);
            onHitEnemy(hit.collider, hit.point);
        }
    }

    //normal shooting                                                               OBSOLETE
    //                                                                      instead of thic func. use onhitenemy(collider) cause
    //                                                                      this method kind of duplicate of onhitenemy(collider)
    //private void onHitEnemy(RaycastHit hit)                               and I've added vector3 hitpoint that method
    //{
    //    IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
    //    if (damageableObject != null)
    //    {
    //        damageableObject.takeHit(damage, hit);
    //    }
    //    Destroy(gameObject);
    //}



    void onHitEnemy(Collider collider, Vector3 hitPoint) 
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.takeHit(damage, hitPoint, transform.forward);
        }
        Destroy(gameObject);
    }
}
