using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;

    public void setBulletSpeed(float newBulletSpeed) {
        bulletSpeed = newBulletSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }
}
