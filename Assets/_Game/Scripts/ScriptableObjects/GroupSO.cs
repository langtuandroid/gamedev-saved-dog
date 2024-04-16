using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class GroupSO : ScriptableObject
{
    [FormerlySerializedAs("title")]
    public string levelsInGroupText;
    [FormerlySerializedAs("actImage")]
    public Sprite groupImage;
    [FormerlySerializedAs("starUnlock")]
    public int starsToUnlock;
}
