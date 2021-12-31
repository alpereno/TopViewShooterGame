using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    //damage and hit variable
    void takeHit(float damage, Vector3 hitPoint, Vector3 hitDirection);

    //just damage
    void takeDamage(float damage);
}