using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private float rotZ;
    private float rotationSpeed = 699f;
    private float moveSpeed = 2f;
    [SerializeField] private Transform pos1, pos2;

    private Vector3 moveDirection, p1, p2;

    private Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    void Start() 
    {
        p1 = pos1.position;
        p2 = pos2.position;
    }

    void Update()
    {
        RotateObject();
        MoveObject();
    }

    void RotateObject()
    {
        rotZ += Time.deltaTime * rotationSpeed;
        TF.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    void MoveObject()
    {
        if (Vector3.Distance(TF.position, p1) < 0.1f)
        {
            moveDirection = (p2 - p1).normalized;
        }
        else if (Vector3.Distance(TF.position, p2) < 0.1f)
        {
            moveDirection = (p1 - p2).normalized;
        }

        TF.position += moveDirection * Time.deltaTime;
    }
}
