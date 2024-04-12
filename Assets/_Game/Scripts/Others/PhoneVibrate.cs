using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneVibrate : MonoBehaviour
{
    public void VibrateDevice()
    {
        Handheld.Vibrate();
        Debug.Log("Rung");
    }
}
