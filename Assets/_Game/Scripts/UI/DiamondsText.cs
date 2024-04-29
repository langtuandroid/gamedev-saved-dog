using System;
using TMPro;
using UnityEngine;
using Zenject;

public class DiamondsText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private DataController _dataController;

    [Inject]
    private void Construct (DataController dataController)
    {
        _dataController = dataController;
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = _dataController.currentGameData.diamonds.ToString();
    }
}
