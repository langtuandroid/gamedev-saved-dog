using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float size;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = size * Screen.height / Screen.width * 0.5f;
    }
}
