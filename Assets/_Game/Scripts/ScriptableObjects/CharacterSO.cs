using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class CharacterSO : ScriptableObject
{
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
