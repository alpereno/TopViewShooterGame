﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Manage thigns like equiping weapon
    [SerializeField] private Transform weaponHold;
    [SerializeField] private Gun startingGun;
    Gun equippedGun;

    private void Start()
    {
        if (startingGun != null)
        {
            equipGun(startingGun);
        }    
    }

    void equipGun(Gun gunToEquip) {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void shoot() {
        if (equippedGun != null)
        {
            equippedGun.shoot();
        }
    }
}
