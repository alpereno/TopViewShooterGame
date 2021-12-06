using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    //damage and hit variable
    void takeHit(float damage, RaycastHit hit);

    //just damage
    void takeDamage(float damage);
}

