using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private SpriteRenderer dotSprite;
    [SerializeField] private Color dotColor;
    [SerializeField] private float rotateSpeed = -40;
    Color originalColor;

    void Start()
    {
        Cursor.visible = false;
        originalColor = dotSprite.color;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public void detectTarget(Ray ray) {
        if (Physics.Raycast(ray, 100, targetLayerMask))
        {
            dotSprite.color = dotColor;
        }
        else dotSprite.color = originalColor;
    }
}
