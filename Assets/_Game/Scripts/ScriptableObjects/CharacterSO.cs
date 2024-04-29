using UnityEngine;
using UnityEngine.Serialization;

public enum ShopType
{
    Coin,
    Ad,
    Diamond
}

[CreateAssetMenu]
public class CharacterSO : ScriptableObject
{
    public ShopType shopType;
    [FormerlySerializedAs("image")]
    public Sprite skinImage;
    [FormerlySerializedAs("animIndex")]
    public int animationIndex;
    [FormerlySerializedAs("hp")]
    public int health;
    public int price;
    public int adMustWatch;
    [FormerlySerializedAs("owned")]
    public bool isOwned;
    [FormerlySerializedAs("name")]
    public string skinName;
}
