﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public event System.Action onDeath;
    [SerializeField] private float startingHealth = 10;
    protected float health;
    protected bool dead;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public void takeHit(float damage, RaycastHit hit) {
        health -= damage;

        if (health <= 0 && !dead)
        {
            die();
        }
    }

    protected void die()
    {
        health = 0;
        dead = true;
        if (onDeath != null)
        {
            onDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
