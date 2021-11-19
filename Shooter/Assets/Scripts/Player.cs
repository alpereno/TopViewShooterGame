using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : MonoBehaviour
{
    PlayerController playerController;
    GunController gunController;
    [SerializeField] private float moveSpeed = 5f;
    Camera viewCamera;

    private void Start()
    {
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
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        //FPS game move direction (relative to local coordinate system)
        //moveVelocity = transform.TransformDirection(moveVelocity);
        playerController.setVelocity(moveVelocity);
    }
}
