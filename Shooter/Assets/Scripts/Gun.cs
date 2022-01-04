using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float msBetweenShots = 100;
    [SerializeField] private float muzzleVelocity = 35;

    [SerializeField] private Transform chamber;             //shell ejection point
    [SerializeField] private Transform shell;
    float nextShotTime;

    public void shoot() {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.setBulletSpeed(muzzleVelocity);

            Instantiate(shell, chamber.position, chamber.rotation);
        }
    }
}
