using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State {Idle, Chasing, Attacking};
    State currentState;
    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;

    Material material;
    Color originalColor;
    Color attackColor = Color.red;

    float damage = 1;
    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float nextAttackTime;
    float collisionRadius;
    float targetCollisionRadius;
    bool targetAlive;

    protected override void Start()
    {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        material = GetComponent<Renderer>().material;
        originalColor = material.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            targetAlive = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.onDeath += onTargetDeath;

            collisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(updatePath());
        }
    }

    private void Update()
    {
        //attack to the target if the distance from enemy to target less than attack distance 
        //sqrt operation is costly which is why I'm not using Vector3.Distance.  So just compare sqr distance
        //or you can use (transform.pos - target.pos).sqrMagnitude           below
        if (targetAlive)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistanceToTarget = Utility.calculateDistanceSqr(transform.position, target.position);
                if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThreshold + collisionRadius + targetCollisionRadius, 2))
                {
                    //dont want to attack every frame use timer
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(attack());
                }
            }
        }
    }

    void onTargetDeath() {
        targetAlive = false;
        currentState = State.Idle;
    }

    IEnumerator attack() {
        //when enemy attacking shouldn't move to target
        currentState = State.Attacking;
        pathFinder.enabled = false;
        //for lunge store our starting attack pos then lunge to attack pos then go back starting pos
        Vector3 startPosition = transform.position;
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - directionToTarget * collisionRadius;
        float attackSpeed = 3;
        float percent = 0;
        material.color = attackColor;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.takeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            //percent should be start pos fo to attack pos and BACK to start pos so value be 0 to 1 and back to 0 again
            //so parabola equation working y = 4(-x^2+x)      when percent 1/2 apply damage
            float interpolation = 4*(-percent * percent + percent);
            transform.position = Vector3.Lerp(startPosition, attackPosition, interpolation);
            yield return null;
        }
        material.color = originalColor;
        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

    IEnumerator updatePath() {
        //enemy doesn't need to get into target exact position
        //pathfind to outside of target pos. collision radius
        float refreshRate = .25f;

        while (targetAlive)
        {
            if (currentState == State.Chasing)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                Vector3 targetPosition = target.position - directionToTarget * (collisionRadius + targetCollisionRadius +attackDistanceThreshold/2);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
