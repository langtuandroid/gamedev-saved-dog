using UnityEngine;

public class Saw : MonoBehaviour
{
    private const float ROTATION_SPEED = 699f;
    
    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;
    
    private float rotZ;
    private Vector3 moveDirection, p1, p2;

    private Transform tf;
    private Transform TF
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

    private void Start() 
    {
        p1 = pos1.position;
        p2 = pos2.position;
    }

    private void Update()
    {
        RotateObject();
        MoveObject();
    }

    private void RotateObject()
    {
        rotZ += Time.deltaTime * ROTATION_SPEED;
        TF.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    private void MoveObject()
    {
        if (Vector3.Distance(TF.position, p1) < 0.1f)
        {
            moveDirection = (p2 - p1).normalized;
        } else if (Vector3.Distance(TF.position, p2) < 0.1f)
        {
            moveDirection = (p1 - p2).normalized;
        }

        TF.position += moveDirection * Time.deltaTime;
    }
}
