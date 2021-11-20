﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntity
{
    PlayerController playerController;
    GunController gunController;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 6.5f;
    Camera viewCamera;

    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    void Update()
    {
        moveInput();
        weaponInput();
        createAimRay();
    }

    private void weaponInput()
    {
        if (Input.GetMouseButton(0))
        {
            gunController.shoot();
        }
    }

    private void createAimRay()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            playerController.lookAt(point);
        }
    }

    private void moveInput()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized;
        if (Input.GetKey(KeyCode.LeftShift))
            moveVelocity *= runSpeed;
        else moveVelocity *= walkSpeed;
        //FPS game move direction (relative to local coordinate system)
        //moveVelocity = transform.TransformDirection(moveVelocity);
        playerController.setVelocity(moveVelocity);
    }
}
