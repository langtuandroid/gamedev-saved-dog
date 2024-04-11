using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneVibrate : Singleton<PhoneVibrate>
{
    public void VibrateDevice()
    {
        Handheld.Vibrate();
        Debug.Log("Rung");
    }
}
