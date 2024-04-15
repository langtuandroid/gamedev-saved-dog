using UnityEngine;
using UnityEngine.UI;

public class HealthBarDoge : MonoBehaviour
{
    private const float SMOOTH_SPEED = 10f;
    
    [SerializeField] private Slider healthBar;
    [SerializeField] private Color lowHealth, highHealth;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform character;

    private Vector3 targetPosition, desiredPosition;
    private Camera cam;
    
    private Transform _healthBar;

    private void Start()
    {
        cam = Camera.main;
        _healthBar = healthBar.transform;
    }
    
    private void LateUpdate()
    {
        if (character == null)
        {
            return;
        }

        desiredPosition = cam.WorldToScreenPoint(character.position + offset);
        targetPosition = Vector3.Lerp(targetPosition, desiredPosition, SMOOTH_SPEED * Time.smoothDeltaTime);
        _healthBar.position = targetPosition;
    }

    public void SetHealthBar(int health, int maxHealth)
    {
        healthBar.gameObject.SetActive(health < maxHealth);
        healthBar.value = health;
        healthBar.maxValue = maxHealth;

        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(lowHealth, highHealth, healthBar.normalizedValue);
    }
    
    public void DisableHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}
