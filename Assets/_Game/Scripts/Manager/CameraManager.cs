using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float size;

    private void Start()
    {
        if (Camera.main != null)
        {
            Camera.main.orthographicSize = size * Screen.height / Screen.width * 0.5f;
        }
    }
}
