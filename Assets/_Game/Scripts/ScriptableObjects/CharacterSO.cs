using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class CharacterSO : ScriptableObject
{
    public Sprite image;
    public int animIndex;
    public int hp;
    public int price;
    public int adMustWatch;
    public int currentAd;
    public bool owned;
    public new string name;
}
