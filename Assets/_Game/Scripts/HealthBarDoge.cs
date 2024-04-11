using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarDoge : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Color lowHealth, highHealth;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform character;

    public float smoothSpeed;
    private Vector3 targetPosition, desiredPosition;
    private Camera cam;

    private int health, maxHealth;

    private Transform tfHealthBar;

    private Transform tf;
    public Transform TF {
        get {
            if (tf == null) {
                tf = transform;
            }
            return tf;
        }
    }
    void Start()
    {
        smoothSpeed = 10f;
        cam = Camera.main;
        tfHealthBar = healthBar.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (character != null)
        {
            desiredPosition = cam.WorldToScreenPoint(character.position + offset);
            targetPosition = Vector3.Lerp(targetPosition, desiredPosition, smoothSpeed * Time.smoothDeltaTime);
            tfHealthBar.position = targetPosition;
        }
    }

    public void SetHealthBar(int health, int maxHealth)
    {
        healthBar.gameObject.SetActive(health < maxHealth);
        healthBar.value = health;
        healthBar.maxValue = maxHealth;

        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(lowHealth, highHealth, healthBar.normalizedValue);
    }
    public void TurnOffHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}
